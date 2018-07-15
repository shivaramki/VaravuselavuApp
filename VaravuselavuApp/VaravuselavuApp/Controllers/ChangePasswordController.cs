using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VaravuselavuStandard.Access;
using VaravuselavuStandard.Models.View;
using VaravuselavuStandard.Util;

namespace VaravuselavuApp.Controllers
{
	[Authorize]
    public class ChangePasswordController : CustomController
	{

		ExpenseConnection _expenseConnection;
		AppConfiguration _appConfiguration;
		ILogger<ChangePasswordController> _logger;

		public ChangePasswordController(IOptions<AppConfiguration> appConfiguration, ILogger<ChangePasswordController> logger, ExpenseConnection expenseConnection)
		{
			_appConfiguration = appConfiguration.Value;
			_logger = logger;
			_expenseConnection = expenseConnection;
		}

		[HttpPost]
		[Route("user/changepassword")]
		public IActionResult ChangePassword(ChangePassword changePassword)
		{
			var errors = new List<String>();
			var changePasswordAccess = new ChangePasswordAccess(_appConfiguration) { ExpenseConnection = _expenseConnection };

			try
			{
				_expenseConnection.BeginTransaction();
				changePasswordAccess.ChangePassword(changePassword);
				_expenseConnection.Commit();
			}
			catch (CustomExceptions Ex)
			{
				_logger.LogWarning("User : {username} get exception while changing the password", LoggedInUser);

				if (_expenseConnection.InTransaction)
					_expenseConnection.RollBack();

				errors = (List<string>)Ex.Data["errors"];
			}
			catch (Exception Ex)
			{
				_logger.LogWarning("User : {username} get exception while changing the password", LoggedInUser);

				if (_expenseConnection.InTransaction)
					_expenseConnection.RollBack();

				LogExecption(Ex, _expenseConnection);

				errors = new List<string>() { Ex.Message };
			}

			return Json(new
			{
				success = (errors.Count == 0),
				data = changePassword,
				error = errors
			});
		}

	}
}