using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VaravuselavuStandard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Route("health")]
        [HttpGet]
        public IActionResult Index()
        {
            return Json(new
            {
                success = true,
                data = "",
                error = new List<String>()
            });
        }
    }
}