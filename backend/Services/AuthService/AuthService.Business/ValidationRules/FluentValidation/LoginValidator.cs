using AuthService.Entities.DTOs;
using FluentValidation;

namespace AuthService.Business.ValidationRules.FluentValidation
{
    public class LoginValidator : AbstractValidator<UserForLoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.");
        }
    }
}

