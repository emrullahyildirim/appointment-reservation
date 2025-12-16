using AuthService.Entities.DTOs;
using FluentValidation;

namespace AuthService.Business.ValidationRules.FluentValidation
{
    public class RegisterValidator : AbstractValidator<UserForRegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
                .MaximumLength(100).WithMessage("Email adresi en fazla 100 karakter olabilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Şifre en fazla 50 karakter olabilir.")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("İsim boş olamaz.")
                .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("İsim en fazla 50 karakter olabilir.");

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Soyisim en fazla 50 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Geçerli bir telefon numarası giriniz.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}

