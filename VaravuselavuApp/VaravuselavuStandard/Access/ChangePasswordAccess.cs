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

		protected override bool ValidateMandatory(ChangePassword entity)
		{
			if (!entity.EmailId.isValidEmail())
				_errors.Add("Invalid email address");

			if (entity.OldPassword.isNullOrEmpty())
				_errors.Add("Old password cannot be empty");

			if (entity.NewPassword.isNullOrEmpty())
				_errors.Add("New password cannot be empty");

			return Convert.ToBoolean(_errors.Count == 0);
		}
	}
}
