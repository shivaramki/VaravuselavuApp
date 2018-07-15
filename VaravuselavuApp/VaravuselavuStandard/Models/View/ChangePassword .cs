using System;
using System.Collections.Generic;
using System.Text;

namespace VaravuselavuStandard.Models.View
{
    public class ChangePassword
    {
		public string EmailId { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
    }
}
