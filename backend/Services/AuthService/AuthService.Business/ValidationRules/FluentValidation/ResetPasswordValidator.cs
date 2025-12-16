using AuthService.Entities.DTOs;
using FluentValidation;

namespace AuthService.Business.ValidationRules.FluentValidation
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token boş olamaz.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Şifre en fazla 50 karakter olabilir.")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Şifre tekrarı boş olamaz.")
                .Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor.");
        }
    }
}

