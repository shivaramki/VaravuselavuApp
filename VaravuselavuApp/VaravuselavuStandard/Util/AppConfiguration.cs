using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaravuselavuStandard.Util
{
    public class AppConfiguration
    {
        public _ConnectionStrings ConnectionStrings { get; set; }
        public _Messages Messages { get; set; }
        public _EmailConfigurations EmailConfigurations { get; set; }

        public class _ConnectionStrings
        {
            public string DefaultConnectionString { get; set; }
        }
        public class _Messages
        {
            public string InternalServerError { get; set; }
        }
        public class _EmailConfigurations
        {
            public string FromAddress { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
