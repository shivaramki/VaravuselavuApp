using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.AbstractDbAccess;
using VaravuselavuStandard.Models;
using VaravuselavuStandard.Models.View;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Access
{
	public class ChangePasswordAccess : ChangePasswordDBAccess
	{
		AppConfiguration _appConfiguration;
		public ChangePasswordAccess(AppConfiguration appConfiguration)
		{
			_appConfiguration = appConfiguration;
		}

		protected override bool UserChangePassword(ChangePassword entity)
		{
			var appUser = new AppUser();
			var appUserAccess = new AppUserAccess(_appConfiguration) { ExpenseConnection = _connection };
			var isSuccess = false;

			appUser.EmailId = entity.EmailId;
			appUser.Password = entity.OldPassword;

			if (appUserAccess.VerifyUser(ref appUser))
			{
				UpdatePassword(entity.EmailId, entity.NewPassword);
				TriggerEmail(appUser.EmailId,entity.NewPassword, appUser.Username);
				isSuccess = true;
			}
			else
			{
				throw new Exception("Invalid username and password");
			}

			return isSuccess;
		}

		public bool UpdatePassword(string emailId, string password)
		{
			var isUpdateSuccess = false;
			try
			{
				var hashpassword = Helpers.HashPassword(password);
				var salt = Helpers.salt;
				_connection.Execute(@"update app_user set password = @password,salt = @saltvalue where emailid = @emailId", new { password = hashpassword, saltvalue = salt, emailid = emailId });
				isUpdateSuccess = true;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return isUpdateSuccess;
		}

		public Boolean TriggerEmail(String toAddress, String userPassword, string username)
		{
			var emailSubject = "Updated password !!!";
			var bodyContent = "The updated password for the Varavuselavu app is <br/> <b>Username:</b>" + username + "<br/>" + "<b>Password:</b>" + userPassword + "<br/>" + $"Application URL: <a href = \"http:www.varavuselavu.xyz\">VaravuselavuApp</a>";

			return Email.Send(
				CrytoHelper.DecryptString(_appConfiguration.EmailConfigurations.FromAddress, Constants.KEY),
				toAddress,
				CrytoHelper.DecryptString(_appConfiguration.EmailConfigurations.FromAddress, Constants.KEY),
				CrytoHelper.DecryptString(_appConfiguration.EmailConfigurations.Password, Constants.KEY), userPassword, username, emailSubject, bodyContent);
		}

		protected override bool ValidateMandatory(ChangePassword entity)
		{
			if (!entity.EmailId.isValidEmail())
				_errors.Add("Invalid email address");

			if (entity.OldPassword.isNullOrEmpty())
				_errors.Add("Old password cannot be empty");

			if (entity.NewPassword.isNullOrEmpty())
				_errors.Add("New password cannot be empty");

			if (entity.OldPassword == entity.NewPassword)
				_errors.Add("Old password and new password cannot be same");

			return _errors.Count == 0;
		}
	}
}
