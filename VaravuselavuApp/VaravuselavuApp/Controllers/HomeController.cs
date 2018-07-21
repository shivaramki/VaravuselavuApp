using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VaravuselavuStandard.Controllers
{
    
    public class HomeController : Controller
    {
		[Authorize]
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

		[Route("status")]
		[HttpGet]
		public IActionResult AppStatusCheck()
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