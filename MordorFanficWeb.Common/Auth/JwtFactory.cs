using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using MordorFanficWeb.Models;
using Microsoft.Extensions.Options;

namespace MordorFanficWeb.Common.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptionsModel jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptionsModel> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(this.jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator().ConfigureAwait(false)),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(Helper.Constants.Strings.JwtClaimIdentifiers.Rol),
                identity.FindFirst(Helper.Constants.Strings.JwtClaimIdentifiers.Id)
            };

            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: jwtOptions.NotBefore,
                expires: jwtOptions.Expiraton,
                signingCredentials: jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsIdentity GenerateClaimsIdentity(string email, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(email, "Token"), new[]
            {
                new Claim(Helper.Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Helper.Constants.Strings.JwtClaimIdentifiers.Rol, Helper.Constants.Strings.JwtClaims.ApiAccess)
            });
        }

        private static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptionsModel options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan", nameof(JwtIssuerOptionsModel.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptionsModel.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptionsModel.JtiGenerator));
        }
    }
}
