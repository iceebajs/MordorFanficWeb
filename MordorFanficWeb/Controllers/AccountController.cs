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

                if (!RegistrationPasswordValidator.Validation(user.Password))
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

        private static RegistrationViewModel SetUserVariables([FromBody] RegistrationViewModel user)
        {
            user.AccountStatus = true;
            user.CreationDate = DateTime.UtcNow.ToString("g");
            user.LastVisit = DateTime.UtcNow.ToString("g");
            user.IsMasterAdmin = false;
            return user;
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("update-user-information")]
        public async Task<ActionResult> UpdateUserInformation([FromBody] UpdateUserViewModel user)
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
        [HttpPost("update-user-status")]
        public async Task<ActionResult> UpdateUserStatus([FromBody] UpdateUserStatusViewModel user)
        {
            try
            {
                if (user == null)
                {
                    logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if(!await accountAdapter.UpdateUserStatus(user).ConfigureAwait(false))
                {
                    logger.LogError("Master administrator block action is abbadoned");
                    return BadRequest("Cant block master administrator");
                }
                logger.LogInformation($"User status with id: '{user.Id}' successfully updated");
                return Ok("Account successfully updated");
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateUserInformation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordViewModel userData)
        {
            try
            {
                if (userData == null)
                {
                    logger.LogError("UserData object sent from client is null.");
                    return BadRequest("UserData object is null");
                }

                var result = await accountAdapter.ChangeUserPassword(userData).ConfigureAwait(false);
                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
                logger.LogInformation($"Users password with id: {userData.Id} is successfully updated");
                return Ok("Account password successfully updated!");
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside ChangeUserPassword action: {ex.Message}");
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            try
            {
                if (id == "null" || id == null)
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
        [HttpGet("get-users-list")]
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
                if (id == "null" || id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                if(!await accountAdapter.DeleteUser(id).ConfigureAwait(false))
                {
                    logger.LogError("Master administrator delete action is abbadoned");
                    return BadRequest("Cant delete master administrator");
                }

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
                if(id == "null" || id == null)
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
                if (id == "null" || id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                if(!await accountAdapter.UnsetAsAdmin(id).ConfigureAwait(false))
                {
                    logger.LogError("Master administrator unset action is abbadoned");
                    return BadRequest("Cant unset master administrator");
                }
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
                if (id == "null" || id == null)
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

        [Authorize(Policy = "Admin")]
        [HttpPost("user-status/{id}")]
        public async Task<ActionResult> UserStatus(string id)
        {
            try
            {
                if (id == "null" || id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                var user = await accountAdapter.GetUserById(id).ConfigureAwait(false);
                if (user == null || !user.AccountStatus)
                    return Ok(false);
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UserStatus action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-account-id/{id}")]
        public async Task<ActionResult<int>> GetAccountId(string id)
        {
            try
            {
                if (id == "null" || id == null)
                {
                    logger.LogError("User id object sent from client is null");
                    return BadRequest("User id object is null");
                }

                return await accountAdapter.GetAccountId(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetAccountId action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpGet]
        public ActionResult TokenCheck()
        {
            return Ok();
        }
    }
}
