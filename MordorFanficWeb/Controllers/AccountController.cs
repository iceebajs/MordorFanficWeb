using Microsoft.AspNetCore.Mvc;
using MordorFanficWeb.PresentationAdapters.AccountAdapter;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MordorFanficWeb.ViewModels;
using System;
using NLog.Fluent;
using System.Collections.Generic;

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

                user = SetUserVariables(user);
                await accountAdapter.CreateUser(user, user.Password).ConfigureAwait(false);
                logger.LogInformation($"User account {user.UserName} is successfully created");
                return Ok("Account successfully created!");
            }
            catch (Exception ex)
            {
                logger.LogError($"Something gone wrong inside RegisterUser action: {ex.Message}");
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
                logger.LogError($"Something gone wrong inside UpdateUserInformation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}")]
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
                logger.LogError($"Something gone wrong inside GetUserById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

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
                logger.LogError($"Something gone wrong inside GetUsersList action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                if (id == null)
                {
                    logger.LogError("User id object sent from client is null.");
                    return BadRequest("User id object is null");
                }

                await accountAdapter.DeleteUser(id);
                logger.LogInformation($"User {id} successfully deleted");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something gone wrong inside DeleteUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
