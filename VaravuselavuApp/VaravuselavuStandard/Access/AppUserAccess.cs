using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using VaravuselavuStandard.AbstractDbAccess;
using VaravuselavuStandard.Models;
using VaravuselavuStandard.Util;
using VaravuselavuStandard.Models.View;

namespace VaravuselavuStandard.Access
{
	public class AppUserAccess : AppUserDbAccess
	{
		AppConfiguration _appConfiguration;
		public AppUserAccess(AppConfiguration appConfiguration)
		{
			_appConfiguration = appConfiguration;
		}

		protected override bool UserInsert(AppUser entity)
		{
			entity.Password = Helpers.HashPassword(entity.Password);
			entity.Salt = Helpers.salt;

			_connection.Insert(entity);

			return TriggerEmail(entity.EmailId, entity.Password, entity.Username);
		}

		protected override bool UserUpdate(AppUser entity)
		{
			return _connection.Update(entity);
		}

		protected override bool ValidateInsert(AppUser entity)
		{
			return Convert.ToBoolean(_errors.Count == 0);
		}

		protected override bool ValidateMandatory(AppUser entity)
		{

			if (!entity.EmailId.isValidEmail())
				_errors.Add("Invalid email address");

			if (entity.Username.isNullOrEmpty())
				_errors.Add("User name cannot be empty");

			if (entity.PhoneNumber.isNullOrEmpty() || entity.PhoneNumber.Length == 0 || entity.PhoneNumber.Length < 10)
				_errors.Add("Please enter the valid phone number");

			if (entity.AdvanceAmount == 0)
				_errors.Add("Amount cannot be zero");

			return Convert.ToBoolean(_errors.Count == 0);
		}

		protected override bool ValidateUpdate(AppUser entity)
		{
			return Convert.ToBoolean(_errors.Count == 0);
		}

		public Boolean TriggerEmail(String toAddress, String userPassword, string username)
		{

			return Email.Send(
				CrytoHelper.DecryptString(_appConfiguration.EmailConfigurations.FromAddress, Constants.KEY),
				toAddress,
				CrytoHelper.DecryptString(_appConfiguration.EmailConfigurations.FromAddress, Constants.KEY),
				CrytoHelper.DecryptString(_appConfiguration.EmailConfigurations.Password, Constants.KEY), userPassword, username);
		}

		public IEnumerable<AppUser> getAllUsers()
		{
			return _connection.Query<AppUser>("select * from app_user order by username");
		}

		public bool VerifyUser(ref AppUser appuser)
		{
			var password = appuser.Password;
			if (appuser.EmailId.Trim().isNullOrEmpty())
				throw new Exception("Email id cannot be empty");

			if (appuser.Password.Trim().isNullOrEmpty())
				throw new Exception("Password cannot be empty");

			appuser = _connection.Query<AppUser>(@"select * from app_user where emailid=@emailid", new { emailid = appuser.EmailId }).FirstOrDefault();

			return Helpers.ValidatePassword(password, appuser.Password, appuser.Salt);

		}
	}
}
