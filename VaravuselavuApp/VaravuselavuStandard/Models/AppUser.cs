using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaravuselavuStandard.Models
{
    [Table("app_user")]
    public class AppUser
    {
        public int Id { get; set;}
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public long AdvanceAmount { get; set; }
        public Boolean IsAdmin { get; set; }
		public string Salt { get; set; }
    }
}
