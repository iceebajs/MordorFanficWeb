using Microsoft.AspNetCore.Mvc;
using MordorFanficWeb.PresentationAdapters.AccountAdapter;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MordorFanficWeb.ViewModels;
using System;
using NLog.Fluent;
using System.Collections.Generic;
using MordorFanficWeb.Common;
using MordorFanficWeb.Common.Helper;
using MordorFanficWeb.Common.Auth;
using Newtonsoft.Json;
using MordorFanficWeb.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAccountAdapter accountAdapter;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly JsonSerializerSettings serializerSettings;

        public AuthenticationController(
            IAccountAdapter accountAdapter,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.accountAdapter = accountAdapter;
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions.Value;
            serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] CredentialsViewModel credentials)
        {
            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password).ConfigureAwait(false);
            if (identity == null)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username of password", ModelState));

            var response = new
            {
                id = identity.Claims.Single(claim => claim.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(credentials.Email, identity).ConfigureAwait(false),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, serializerSettings);
            return new OkObjectResult(json);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string email, string password)
        {
            var userToVerify = await accountAdapter.GetUserIdentity(email).ConfigureAwait(false);
            if (userToVerify != null && userToVerify.AccountStatus == true)
            {
                if (await accountAdapter.VerifyUserPassword(userToVerify, password).ConfigureAwait(false))
                    return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(email, userToVerify.Id)).ConfigureAwait(false);
            }
            return await Task.FromResult<ClaimsIdentity>(null).ConfigureAwait(false);
        }
    }
}
