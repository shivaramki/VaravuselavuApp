using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VaravuselavuStandard.Util;
using VaravuselavuStandard.Query;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Controllers
{
    [Authorize]
    public class DashboardController : CustomController
    {
        ExpenseConnection _expenseConnection;
        AppConfiguration _appConfiguration;
        ILogger<DashboardController> _logger;       

        public DashboardController(IOptions<AppConfiguration> appConfiguration, ILogger<DashboardController> logger, ExpenseConnection expenseConnection)
        {
            _appConfiguration = appConfiguration.Value;
            _logger = logger;
            _expenseConnection = expenseConnection;
        }

        [HttpGet]
        [Route("dashboard/category")]
        public IActionResult GetCategoryWiseData()
        {
            var errors = new List<string>();
            var dashBoardQuery = new DashboardQuery(_expenseConnection);
            dynamic dashboardData  = null;

            try
            {
                dashboardData = dashBoardQuery.GetCategoryWiseExpenseAmount();

                _logger.LogInformation("User : {username} fetches dashboard data", LoggedInUser);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to get dashboard data", LoggedInUser);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = dashboardData,
                error = errors
            });
        }

        [HttpGet]
        [Route("dashboard/monthwise")]
        public IActionResult GetAllMonthWiseData()
        {
            var errors = new List<string>();
            var dashBoardQuery = new DashboardQuery(_expenseConnection);
            dynamic dashboardData = null;

            try
            {
                dashboardData = dashBoardQuery.GetMonthWiseExpenditure();

                _logger.LogInformation("User : {username} fetches dashboard data", LoggedInUser);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to get dashboard data", LoggedInUser);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = dashboardData,
                error = errors
            });
        }
    }
}