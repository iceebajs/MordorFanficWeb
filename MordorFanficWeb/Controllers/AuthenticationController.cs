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
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

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
            var userToVerify = await accountAdapter.GetUserIdentity(credentials.Email).ConfigureAwait(false);
            string role = await SetUserRole(userToVerify).ConfigureAwait(false);
            var identity = await GetClaimsIdentity(userToVerify, credentials.Password, role).ConfigureAwait(false);

            if(userToVerify != null && !userToVerify.AccountStatus)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "User blocked", ModelState));

            if (identity == null)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username of password", ModelState));

            var response = new
            {
                id = identity.Claims.Single(claim => claim.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(credentials.Email, identity, role).ConfigureAwait(false),
                userPermissions = role,
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, serializerSettings);
            return new OkObjectResult(json);
        }

        private async Task<string> SetUserRole(AppUserModel user)
        {
            if (user != null)
            {
                IList<string> roles = await accountAdapter.GetUserRoles(user.Id).ConfigureAwait(false);
                return roles.Contains("admin") ? "admin" : "user";
            }
            return null;
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(AppUserModel userToVerify, string password, string role)
        {
            if (userToVerify != null && userToVerify.AccountStatus == true)
            {
                if (await accountAdapter.VerifyUserPassword(userToVerify, password).ConfigureAwait(false))
                    return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(userToVerify.Email, userToVerify.Id, role)).ConfigureAwait(false);
            }
            return await Task.FromResult<ClaimsIdentity>(null).ConfigureAwait(false);
        }
    }
}
