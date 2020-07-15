using Microsoft.AspNetCore.Mvc;
using MordorFanficWeb.PresentationAdapters.AccountAdapter;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MordorFanficWeb.ViewModels;
using System;
using System.Collections.Generic;
using MordorFanficWeb.Common;
using MordorFanficWeb.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using NLog.Fluent;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountAdapter accountAdapter;
        private readonly ILogger<AccountController> logger;

        public AccountController(IAccountAdapter accountAdapter, ILogger<AccountController> logger)
        {
            this.accountAdapter = accountAdapter;
            this.logger = logger;
        }

        [HttpPost("register-user")]
        public async Task<ActionResult> RegisterUser([FromBody] RegistrationViewModel user)
        {
            try
            {
                if (user == null)
                {
                    logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if(!RegistrationPasswordValidator.Validation(user.Password))
                    return BadRequest("Pasword can contain only basic latin symbols");

                user = SetUserVariables(user);
                var result = await accountAdapter.CreateUser(user, user.Password).ConfigureAwait(false);
                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
                logger.LogInformation($"User account {user.UserName} is successfully created");
                return Ok("Account successfully created!");
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside RegisterUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private static RegistrationViewModel SetUserVariables(RegistrationViewModel user)
        {
            user.AccountStatus = true;
            user.CreationDate = DateTime.UtcNow.ToString("g");
            user.LastVisit = DateTime.UtcNow.ToString("g");
            user.IsMasterAdmin = false;
            return user;
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("update-user-information")]
        public async Task<ActionResult> UpdateUserInformation(UpdateUserViewModel user)
        {
            try
            {
                if (user == null)
                {
                    logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                await accountAdapter.UpdateUser(user).ConfigureAwait(false);
                logger.LogInformation($"User account {user.UserName} successfully updated");
                return Ok("Account successfully updated");
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateUserInformation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("user/{email}")]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            try
            {
                if (email == null)
                {
                    logger.LogError("User email object sent from client is null.");
                    return BadRequest("User email object is null");
                }

                var user = await accountAdapter.GetUserByEmail(email).ConfigureAwait(false);
                logger.LogInformation("Get user by email action succeed");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetUserByEmail action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            try
            {
                if (id == null)
                {
                    logger.LogError("User id object sent from client is null.");
                    return BadRequest("User id object is null");
                }

                var user = await accountAdapter.GetUserById(id).ConfigureAwait(false);
                logger.LogInformation("Get user by id action succeed");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetUserByEmail action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetUsersList()
        {
            try
            {
                List<UsersListViewModel> usersList = await accountAdapter.GetUsersList().ConfigureAwait(false);
                logger.LogInformation("Users list successfully returned");
                return Ok(usersList);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetUsersList action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                if (id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                await accountAdapter.DeleteUser(id).ConfigureAwait(false);
                logger.LogInformation($"User {id} successfully deleted");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("set-role/{id}")]
        public async Task<ActionResult> SetRole(string id)
        {
            try
            {
                if(id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                await accountAdapter.SetAsAdmin(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside SetRole action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("unset-role/{id}")]
        public async Task<ActionResult> UnsetRole(string id)
        {
            try
            {
                if (id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                await accountAdapter.UnsetAsAdmin(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UnsetRole action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("get-user-roles/{id}")]
        public async Task<ActionResult> GetUserRoles(string id)
        {
            try
            {
                if (id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                return Ok(await accountAdapter.GetUserRoles(id).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetUserRoles action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
