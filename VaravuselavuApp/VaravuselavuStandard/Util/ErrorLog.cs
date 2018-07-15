using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using VaravuselavuStandard.Models;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Util
{
    public class ErrorLog
    {
        public static void LogError(Exception ex, HttpContext httpContext,ExpenseConnection _conn)
        {
            var appErrors = new AppErrors();

            try
            {
                appErrors.Host = httpContext.Request.Host.HasValue ? httpContext.Request.Host.ToString() : "";

                appErrors.IpAddress = httpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();

                appErrors.Url = httpContext.Request.Path;

                appErrors.HttpMethod = httpContext.Request.Method;

                var userId = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).FirstOrDefault()?.Value;

                if (!string.IsNullOrEmpty(userId))
                    appErrors.UserId = Convert.ToInt32(userId);

                appErrors.Username = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

                appErrors.StackTrace = ex.StackTrace;

                appErrors.InnerException = ex.InnerException.Message;

				appErrors.TimeStamp = DateTime.Now;

                _conn.BeginTransaction();
                _conn.Insert(appErrors);
                _conn.Commit();

            }
            catch(Exception ex1)
            {
                if (_conn.InTransaction)
                    _conn.RollBack();
                var x = ex1.Message.ToString();
            }

        }
    }
}
