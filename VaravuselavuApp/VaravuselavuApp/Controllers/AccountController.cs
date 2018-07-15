using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VaravuselavuStandard.Util;
using VaravuselavuStandard.Access;
using VaravuselavuStandard.Models;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Controllers
{
    public class AccountController : CustomController
    {
        ExpenseConnection _expenseConnection;
        AppConfiguration _appConfiguration;
        ILogger<AccountController> _logger;

        public AccountController(IOptions<AppConfiguration> appConfiguration,ILogger<AccountController>logger,ExpenseConnection expenseConnection)
        {
            _appConfiguration = appConfiguration.Value;
            _logger = logger;
            _expenseConnection = expenseConnection;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login/auth")]
        public async Task<IActionResult> Login(AppUser appUser)
        {
            var errors = new List<string>();
            try
            {
                AppUserAccess appUserAccess = new AppUserAccess(_appConfiguration) { ExpenseConnection = _expenseConnection };

                if (appUserAccess.VerifyUser(ref appUser))
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, appUser.Username),
                    new Claim(ClaimTypes.PrimarySid,appUser.Id.ToString())
                };

                    var userIdentity = new ClaimsIdentity(claims, "login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);

                    return Json(new
                    {
                        success = true,
                        data = appUser,
                        error = errors
                    });
                }
            }
            catch(Exception ex)
            {
                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();
            }
            return Json(new { success = false, error = errors });

        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Json(new { success = true});
        }

    }
}