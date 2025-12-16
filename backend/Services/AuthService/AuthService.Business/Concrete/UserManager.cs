using AuthService.Business.Abstract;
using AuthService.Business.Constants;
using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using Core.Utilities.Result;

namespace AuthService.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IOperationClaimDal _operationClaimDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserManager(
            IUserDal userDal,
            IOperationClaimDal operationClaimDal,
            IUserOperationClaimDal userOperationClaimDal)
        {
            _userDal = userDal;
            _operationClaimDal = operationClaimDal;
            _userOperationClaimDal = userOperationClaimDal;
        }

        public IDataResult<User> GetById(int id)
        {
            var user = _userDal.Get(u => u.Id == id);

            if (user == null)
                return new ErrorDataResult<User>(Messages.UserNotFound);

            return new SuccessDataResult<User>(user);
        }

        public IDataResult<User> GetByEmail(string email)
        {
            var user = _userDal.GetByEmail(email.ToLowerInvariant().Trim());

            if (user == null)
                return new ErrorDataResult<User>(Messages.UserNotFound);

            return new SuccessDataResult<User>(user);
        }

        public IDataResult<UserDto> GetUserDto(int id)
        {
            var user = _userDal.Get(u => u.Id == id);

            if (user == null)
                return new ErrorDataResult<UserDto>(Messages.UserNotFound);

            var claims = _userDal.GetClaims(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsEmailVerified = user.IsEmailVerified,
                CreatedDate = user.CreatedDate,
                Roles = claims.Select(c => c.Name).ToList()
            };

            return new SuccessDataResult<UserDto>(userDto);
        }

        public IDataResult<List<UserDto>> GetAll()
        {
            var users = _userDal.GetAll();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var claims = _userDal.GetClaims(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    IsEmailVerified = user.IsEmailVerified,
                    CreatedDate = user.CreatedDate,
                    Roles = claims.Select(c => c.Name).ToList()
                });
            }

            return new SuccessDataResult<List<UserDto>>(userDtos);
        }

        public IResult Update(User user)
        {
            var existingUser = _userDal.Get(u => u.Id == user.Id);

            if (existingUser == null)
                return new ErrorResult(Messages.UserNotFound);

            user.UpdatedDate = DateTime.UtcNow;
            _userDal.Update(user);

            return new SuccessResult(Messages.UserUpdated);
        }

        public IResult Delete(int id)
        {
            var user = _userDal.Get(u => u.Id == id);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            _userDal.Remove(user);

            return new SuccessResult(Messages.UserDeleted);
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            var claims = _userDal.GetClaims(user);
            return new SuccessDataResult<List<OperationClaim>>(claims);
        }

        public IResult AddRole(int userId, string roleName)
        {
            var user = _userDal.Get(u => u.Id == userId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var role = _operationClaimDal.GetByName(roleName);
            if (role == null)
                return new ErrorResult(Messages.RoleNotFound);

            var existingClaim = _userOperationClaimDal.Get(
                uoc => uoc.UserId == userId && uoc.OperationClaimId == role.Id);

            if (existingClaim != null)
                return new ErrorResult(Messages.UserAlreadyHasRole);

            _userOperationClaimDal.Add(new UserOperationClaim
            {
                UserId = userId,
                OperationClaimId = role.Id
            });

            return new SuccessResult(Messages.RoleAdded);
        }

        public IResult RemoveRole(int userId, string roleName)
        {
            var user = _userDal.Get(u => u.Id == userId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var role = _operationClaimDal.GetByName(roleName);
            if (role == null)
                return new ErrorResult(Messages.RoleNotFound);

            var userClaim = _userOperationClaimDal.Get(
                uoc => uoc.UserId == userId && uoc.OperationClaimId == role.Id);

            if (userClaim == null)
                return new ErrorResult(Messages.UserDoesNotHaveRole);

            _userOperationClaimDal.Remove(userClaim);

            return new SuccessResult(Messages.RoleRemoved);
        }
    }
}

