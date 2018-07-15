using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VaravuselavuStandard.Util;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Util
{
    public class CustomController : Controller
    {
        public string LoggedInUser
        {
            get
            {
                return User.Identity.ToString();
            }
        }

        public int GetLoggedUserId
        {
            get
            {
               var userId = Request.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).FirstOrDefault()?.Value;

                if (!string.IsNullOrEmpty(userId))
                    return Convert.ToInt32(userId);

                return 0;
            }
           
        }

        public void LogExecption(Exception ex, ExpenseConnection _conn)
        {
            ErrorLog.LogError(ex, Request.HttpContext, _conn);
        }
    }
}