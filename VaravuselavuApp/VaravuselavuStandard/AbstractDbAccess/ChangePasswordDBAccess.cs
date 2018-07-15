using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.Models.View;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.AbstractDbAccess
{
    public abstract class ChangePasswordDBAccess: BaseDbAccess
	{
		protected abstract bool ValidateMandatory(ChangePassword entity);

		protected abstract bool UserChangePassword(ChangePassword entity);

		public bool ChangePassword(ChangePassword entity)
		{
			if (!ValidateMandatory(entity))
				RaiseException();

			return UserChangePassword(entity);
		}
	}
}
