using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaravuselavuStandard.Models
{
    [Table("errorlog")]
    public class AppErrors
    {
		public int Id { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; } 
        public string HttpMethod { get; set; }
        public string IpAddress { get; set; }
        public string InnerException { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public DateTime TimeStamp { get; set; }
        public string StackTrace { get; set; }
    }
}
