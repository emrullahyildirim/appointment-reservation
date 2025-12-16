using AuthService.Business.Abstract;
using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthService.Business.Concrete
{
    public class TokenManager : ITokenService
    {
        private readonly TokenOptions _tokenOptions;

        public TokenManager(IOptions<TokenOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }

        public AccessToken CreateAccessToken(User user, List<OperationClaim> claims)
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var expiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: expiration,
                notBefore: DateTime.UtcNow,
                claims: CreateClaims(user, claims),
                signingCredentials: signingCredentials
            );

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = expiration
            };
        }

        public RefreshToken CreateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expiration = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenExpiration)
            };
        }

        public TokenDto CreateTokenDto(User user, List<OperationClaim> claims)
        {
            var accessToken = CreateAccessToken(user, claims);
            var refreshToken = CreateRefreshToken();

            return new TokenDto
            {
                AccessToken = accessToken.Token,
                AccessTokenExpiration = accessToken.Expiration,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.Expiration ?? DateTime.UtcNow.AddDays(7)
            };
        }

        private IEnumerable<Claim> CreateClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}".Trim());
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            // Custom claims
            claims.Add(new Claim("uid", user.Id.ToString()));
            claims.Add(new Claim("emailVerified", user.IsEmailVerified.ToString().ToLower()));

            return claims;
        }
    }
}

