using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VaravuselavuStandard.Util;
using VaravuselavuStandard.Access;
using VaravuselavuStandard.Models;
using VaravuselavuStandard.Util;
using VaravuselavuStandard.Models.View;

namespace VaravuselavuStandard.Controllers
{
    [Authorize]
    public class UserController : CustomController
    {
        ExpenseConnection _expenseConnection;
        AppConfiguration _appConfiguration;
        ILogger<CategoryController> _logger;

        public UserController(IOptions<AppConfiguration> appConfiguration, ILogger<CategoryController> logger, ExpenseConnection expenseConnection)
        {
            _appConfiguration = appConfiguration.Value;
            _logger = logger;
            _expenseConnection = expenseConnection;
        }

        [HttpGet]
        [Route("user/search")]
        public IActionResult FindUserById(int id)
        {
            var errors = new List<string>();
            var appUserAccess = new AppUserAccess(_appConfiguration) { ExpenseConnection = _expenseConnection };
            var appUser = new AppUser();

            try
            {
                appUser = _expenseConnection.Get<AppUser>(id);
                _logger.LogInformation("User : {username} fetches the user id : {id}", LoggedInUser, id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to fetch the userid : {id}", LoggedInUser, id);

                LogExecption(ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }
            return Json(new { success = (errors.Count == 0), data = appUser, error = errors });
        }

        [HttpGet]
        [Route("users/list")]
        public IActionResult GetUsers()
        {
            var errors = new List<string>();
            var appUserAccess = new AppUserAccess(_appConfiguration) { ExpenseConnection = _expenseConnection };
            IEnumerable<AppUser> userList = null;

            try
            {
                userList = appUserAccess.getAllUsers();
                _logger.LogInformation("User : {username} fetches all users", LoggedInUser);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to fetch all user", LoggedInUser);

                LogExecption(ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = userList,
                loggedInId = GetLoggedUserId,
                error = errors
            });
        }


        [HttpPost]
        [Route("user/add")]
        public IActionResult AddUsers(AppUser appUser)
        {
            var errors = new List<string>();
            var appUserAccess = new AppUserAccess(_appConfiguration) { ExpenseConnection = _expenseConnection };
            var userPassword = System.Guid.NewGuid().ToString().Substring(0, 5);

            try
            {
                appUser.Password = userPassword;
                _expenseConnection.BeginTransaction();
                appUserAccess.Insert(appUser);
                _expenseConnection.Commit();

                _logger.LogInformation("User : {username} added new user '{appUsername}'", LoggedInUser, appUser.Username);
            }
            catch (CustomExceptions Ex)
            {
                _logger.LogWarning("User : {username} get validation exception while adding user with name '{appUsername}' ", LoggedInUser, appUser.Username);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                errors = (List<string>)Ex.Data["errors"];
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("User : {username} get exception while adding appuser with name '{appUsername}' ", LoggedInUser, appUser.Username);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = appUser,
                error = errors
            });
        }

        [HttpPost]
        [Route("user/edit")]
        public IActionResult EditUsers(AppUser appUser)
        {
            var errors = new List<string>();
            var appUserAccess = new AppUserAccess(_appConfiguration) { ExpenseConnection = _expenseConnection };

            try
            {
                _expenseConnection.BeginTransaction();
                appUserAccess.Update(appUser);
                _expenseConnection.Commit();

                _logger.LogInformation("User : {username} updated  user '{appUsername}' details", LoggedInUser, appUser.Username);
            }
            catch (CustomExceptions Ex)
            {
                _logger.LogWarning("User : {username} get validation exception while updating user with name '{appUsername}' ", LoggedInUser, appUser.Username);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                errors = (List<string>)Ex.Data["errors"];
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("User : {username} get exception while updating appuser with name '{appUsername}' ", LoggedInUser, appUser.Username);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = appUser,
                error = errors
            });
        }

		
    }
}