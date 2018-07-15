using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VaravuselavuStandard.Util
{
    public static class StringExtension
    {
		public static bool isNullOrEmpty(this string str)
		{
			var isNullOrEmpty = false;

			if (str == string.Empty || str == null)
				isNullOrEmpty = true;
			else
				isNullOrEmpty = false;

			return isNullOrEmpty;
		}

		public static bool isValidEmail(this string str)
		{
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			Match match = regex.Match(str == null ? "" : str);

			return match.Success;
		}
    }
}
