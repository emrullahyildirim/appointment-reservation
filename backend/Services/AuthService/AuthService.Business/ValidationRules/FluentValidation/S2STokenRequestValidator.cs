using AuthService.Entities.DTOs;
using Core.Utilities.Security.S2S;
using FluentValidation;

namespace AuthService.Business.ValidationRules.FluentValidation
{
    public class S2STokenRequestValidator : AbstractValidator<S2STokenRequestDto>
    {
        public S2STokenRequestValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("ClientId gereklidir.")
                .MinimumLength(5).WithMessage("ClientId en az 5 karakter olmalıdır.");

            RuleFor(x => x.ClientSecret)
                .NotEmpty().WithMessage("ClientSecret gereklidir.")
                .MinimumLength(10).WithMessage("ClientSecret en az 10 karakter olmalıdır.");
        }
    }

    public class ServiceClientValidator : AbstractValidator<ServiceClientDto>
    {
        public ServiceClientValidator()
        {
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("Servis adı gereklidir.")
                .MinimumLength(3).WithMessage("Servis adı en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Servis adı en fazla 100 karakter olabilir.")
                .Matches("^[a-zA-Z][a-zA-Z0-9]*$").WithMessage("Servis adı harf ile başlamalı ve sadece harf/rakam içermelidir.");

            RuleFor(x => x.AllowedScopes)
                .NotEmpty().WithMessage("En az bir scope belirtilmelidir.");

            RuleFor(x => x.TokenExpirationMinutes)
                .InclusiveBetween(5, 1440).WithMessage("Token süresi 5 dakika ile 24 saat arasında olmalıdır.");
        }
    }

    public class TokenValidationRequestValidator : AbstractValidator<TokenValidationRequestDto>
    {
        public TokenValidationRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token gereklidir.");
        }
    }
}
