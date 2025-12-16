using AuthService.Business.Abstract;
using AuthService.Business.Constants;
using AuthService.Business.ValidationRules.FluentValidation;
using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using Core.Aspect.Autofac.Validation;
using Core.Utilities.Result;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Notification.Mail;
using System.Security.Cryptography;

namespace AuthService.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserDal _userDal;
        private readonly IOperationClaimDal _operationClaimDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        private readonly IEmailVerificationTokenDal _emailVerificationTokenDal;
        private readonly IPasswordResetTokenDal _passwordResetTokenDal;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;

        private const int MaxFailedLoginAttempts = 5;
        private const int LockoutDurationMinutes = 15;

        public AuthManager(
            IUserDal userDal,
            IOperationClaimDal operationClaimDal,
            IUserOperationClaimDal userOperationClaimDal,
            IEmailVerificationTokenDal emailVerificationTokenDal,
            IPasswordResetTokenDal passwordResetTokenDal,
            ITokenService tokenService,
            IMailService mailService)
        {
            _userDal = userDal;
            _operationClaimDal = operationClaimDal;
            _userOperationClaimDal = userOperationClaimDal;
            _emailVerificationTokenDal = emailVerificationTokenDal;
            _passwordResetTokenDal = passwordResetTokenDal;
            _tokenService = tokenService;
            _mailService = mailService;
        }

        [ValidationAspect(typeof(RegisterValidator))]
        public IDataResult<TokenDto> Register(UserForRegisterDto userForRegisterDto)
        {
            // Check if user exists
            var userExistsResult = UserExists(userForRegisterDto.Email);
            if (!userExistsResult.IsSuccess)
                return new ErrorDataResult<TokenDto>(userExistsResult.Message);

            // Create password hash
            HashingHelper.CreatePasswordHash(
                userForRegisterDto.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var user = new User
            {
                Email = userForRegisterDto.Email.ToLowerInvariant().Trim(),
                FirstName = userForRegisterDto.FirstName.Trim(),
                LastName = userForRegisterDto.LastName?.Trim(),
                PhoneNumber = userForRegisterDto.PhoneNumber?.Trim(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                IsEmailVerified = false,
                CreatedDate = DateTime.UtcNow
            };

            _userDal.Add(user);

            // Assign default "User" role
            var userRole = _operationClaimDal.GetByName("User");
            if (userRole != null)
            {
                _userOperationClaimDal.Add(new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = userRole.Id
                });
            }

            // Send verification email
            SendVerificationEmail(user);

            // Create tokens
            var claims = _userDal.GetClaims(user);
            var tokenDto = _tokenService.CreateTokenDto(user, claims);

            // Save refresh token
            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpiration = tokenDto.RefreshTokenExpiration;
            _userDal.Update(user);

            return new SuccessDataResult<TokenDto>(tokenDto, Messages.UserRegistered);
        }

        [ValidationAspect(typeof(LoginValidator))]
        public IDataResult<TokenDto> Login(UserForLoginDto userForLoginDto)
        {
            var user = _userDal.GetByEmail(userForLoginDto.Email.ToLowerInvariant().Trim());

            if (user == null)
                return new ErrorDataResult<TokenDto>(Messages.InvalidCredentials);

            // Check if account is disabled
            if (!user.Status)
                return new ErrorDataResult<TokenDto>(Messages.AccountDisabled);

            // Check if account is locked
            if (IsAccountLocked(user))
                return new ErrorDataResult<TokenDto>(Messages.AccountLocked);

            // Verify password
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                HandleFailedLogin(user);
                return new ErrorDataResult<TokenDto>(Messages.InvalidCredentials);
            }

            // Reset failed attempts on successful login
            ResetFailedLoginAttempts(user);

            // Create tokens
            var claims = _userDal.GetClaims(user);
            var tokenDto = _tokenService.CreateTokenDto(user, claims);

            // Save refresh token
            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpiration = tokenDto.RefreshTokenExpiration;
            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);

            return new SuccessDataResult<TokenDto>(tokenDto, Messages.LoginSuccessful);
        }

        public IDataResult<TokenDto> RefreshToken(string refreshToken)
        {
            var user = _userDal.GetByRefreshToken(refreshToken);

            if (user == null)
                return new ErrorDataResult<TokenDto>(Messages.InvalidRefreshToken);

            if (user.RefreshTokenExpiration < DateTime.UtcNow)
                return new ErrorDataResult<TokenDto>(Messages.RefreshTokenExpired);

            if (!user.Status)
                return new ErrorDataResult<TokenDto>(Messages.AccountDisabled);

            // Create new tokens
            var claims = _userDal.GetClaims(user);
            var tokenDto = _tokenService.CreateTokenDto(user, claims);

            // Update refresh token (rotation)
            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpiration = tokenDto.RefreshTokenExpiration;
            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);

            return new SuccessDataResult<TokenDto>(tokenDto, Messages.TokenRefreshed);
        }

        public IResult Logout(string refreshToken)
        {
            var user = _userDal.GetByRefreshToken(refreshToken);

            if (user == null)
                return new ErrorResult(Messages.InvalidRefreshToken);

            user.RefreshToken = null;
            user.RefreshTokenExpiration = null;
            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);

            return new SuccessResult(Messages.LogoutSuccessful);
        }

        [ValidationAspect(typeof(ForgotPasswordValidator))]
        public IResult ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = _userDal.GetByEmail(forgotPasswordDto.Email.ToLowerInvariant().Trim());

            // Always return success to prevent email enumeration
            if (user == null)
                return new SuccessResult(Messages.PasswordResetEmailSent);

            // Create password reset token
            var token = GenerateSecureToken();
            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };

            _passwordResetTokenDal.Add(resetToken);

            // Send reset email
            _ = _mailService.SendMessageAsync(
                user.Email,
                "Şifre Sıfırlama",
                $"Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:\n\nToken: {token}\n\nBu bağlantı 1 saat geçerlidir."
            );

            return new SuccessResult(Messages.PasswordResetEmailSent);
        }

        [ValidationAspect(typeof(ResetPasswordValidator))]
        public IResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var resetToken = _passwordResetTokenDal.GetByToken(resetPasswordDto.Token);

            if (resetToken == null)
                return new ErrorResult(Messages.InvalidPasswordResetToken);

            var user = _userDal.Get(u => u.Id == resetToken.UserId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            // Update password
            HashingHelper.CreatePasswordHash(
                resetPasswordDto.NewPassword,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RefreshToken = null;
            user.RefreshTokenExpiration = null;
            user.FailedLoginAttempts = 0;
            user.LockoutEndTime = null;
            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);

            // Mark token as used
            resetToken.IsUsed = true;
            _passwordResetTokenDal.Update(resetToken);

            return new SuccessResult(Messages.PasswordResetSuccess);
        }

        public IResult VerifyEmail(string token)
        {
            var verificationToken = _emailVerificationTokenDal.GetByToken(token);

            if (verificationToken == null)
                return new ErrorResult(Messages.InvalidEmailVerificationToken);

            var user = _userDal.Get(u => u.Id == verificationToken.UserId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            if (user.IsEmailVerified)
                return new ErrorResult(Messages.EmailAlreadyVerified);

            // Verify email
            user.IsEmailVerified = true;
            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);

            // Mark token as used
            verificationToken.IsUsed = true;
            _emailVerificationTokenDal.Update(verificationToken);

            return new SuccessResult(Messages.EmailVerified);
        }

        public IResult ResendVerificationEmail(string email)
        {
            var user = _userDal.GetByEmail(email.ToLowerInvariant().Trim());

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            if (user.IsEmailVerified)
                return new ErrorResult(Messages.EmailAlreadyVerified);

            SendVerificationEmail(user);

            return new SuccessResult(Messages.VerificationEmailSent);
        }

        [ValidationAspect(typeof(ChangePasswordValidator))]
        public IResult ChangePassword(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = _userDal.Get(u => u.Id == userId);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            // Verify current password
            if (!HashingHelper.VerifyPasswordHash(changePasswordDto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return new ErrorResult(Messages.CurrentPasswordIncorrect);

            // Update password
            HashingHelper.CreatePasswordHash(
                changePasswordDto.NewPassword,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RefreshToken = null;
            user.RefreshTokenExpiration = null;
            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);

            return new SuccessResult(Messages.PasswordChanged);
        }

        public IResult UserExists(string email)
        {
            if (_userDal.GetByEmail(email.ToLowerInvariant().Trim()) != null)
                return new ErrorResult(Messages.UserAlreadyExists);

            return new SuccessResult();
        }

        #region Private Methods

        private bool IsAccountLocked(User user)
        {
            return user.LockoutEndTime.HasValue && user.LockoutEndTime > DateTime.UtcNow;
        }

        private void HandleFailedLogin(User user)
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= MaxFailedLoginAttempts)
            {
                user.LockoutEndTime = DateTime.UtcNow.AddMinutes(LockoutDurationMinutes);
            }

            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);
        }

        private void ResetFailedLoginAttempts(User user)
        {
            if (user.FailedLoginAttempts > 0 || user.LockoutEndTime.HasValue)
            {
                user.FailedLoginAttempts = 0;
                user.LockoutEndTime = null;
            }
        }

        private void SendVerificationEmail(User user)
        {
            var token = GenerateSecureToken();
            var verificationToken = new EmailVerificationToken
            {
                UserId = user.Id,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddHours(24),
                IsUsed = false
            };

            _emailVerificationTokenDal.Add(verificationToken);

            _ = _mailService.SendMessageAsync(
                user.Email,
                "Email Doğrulama",
                $"Email adresinizi doğrulamak için aşağıdaki token'ı kullanın:\n\nToken: {token}\n\nBu bağlantı 24 saat geçerlidir."
            );
        }

        private static string GenerateSecureToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        #endregion
    }
}

