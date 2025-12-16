using AuthService.Entities.DTOs;
using FluentValidation;

namespace AuthService.Business.ValidationRules.FluentValidation
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
        }
    }
}

