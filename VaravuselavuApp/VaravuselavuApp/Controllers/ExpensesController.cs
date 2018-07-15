using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VaravuselavuStandard.Util;
using VaravuselavuStandard.Access;
using VaravuselavuStandard.Models;
using VaravuselavuStandard.Query;

namespace VaravuselavuStandard.Controllers
{
    [Authorize]
    public class ExpensesController : CustomController
    {
        ExpenseConnection _expenseConnection;
        AppConfiguration _appConfiguration;
        ILogger<ExpensesController> _logger;

        public ExpensesController(IOptions<AppConfiguration> appConfiguration, ILogger<ExpensesController> logger, ExpenseConnection expenseConnection)
        {
            _appConfiguration = appConfiguration.Value;
            _logger = logger;
            _expenseConnection = expenseConnection;
        }
        
        [HttpPost]
        [Route("expense/add")]
        public IActionResult AddExpenses(Expenses expenses)
        {
            var errors = new List<string>();
            var expenseAccess = new ExpensesAccess() { ExpenseConnection = _expenseConnection };

            try
            {
                expenses.AddedBy = GetLoggedUserId;
                expenses.PostedDate = DateTime.Now;
                _expenseConnection.BeginTransaction();
                expenseAccess.Insert(expenses);
                _expenseConnection.Commit();

                _logger.LogInformation("User: { username}added new expense of '{category}'", LoggedInUser, expenses.Category);
            }
            catch(CustomExceptions ex)
            {
                _logger.LogInformation("User : {username} get validation exception while adding expenses of category '{categoryname}' ", LoggedInUser, expenses.Category);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                errors = (List<string>)ex.Data["errors"];
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("User : {username} get exception while adding expenses of category '{categoryname}' ", LoggedInUser, expenses.Category);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new { success = (errors.Count == 0), data = expenses, error = errors });
        }

        [HttpPost]
        [Route("expense/update")]
        public IActionResult UpdateExpenses(Expenses expenses)
        {
            var errors = new List<string>();
            var expenseAccess = new ExpensesAccess() { ExpenseConnection = _expenseConnection };

            try
            {
                _expenseConnection.BeginTransaction();
                expenseAccess.Update(expenses);
                _expenseConnection.Commit();

                _logger.LogInformation("User: { username} updated expense of '{category}'", LoggedInUser, expenses.Category);
            }
            catch (CustomExceptions ex)
            {
                _logger.LogInformation("User : {username} get validation exception while updating expenses of category '{categoryname}' ", LoggedInUser, expenses.Category);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                errors = (List<string>)ex.Data["errors"];
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("User : {username} get exception while updating expenses of category '{categoryname}' ", LoggedInUser, expenses.Category);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new { success = (errors.Count == 0), data = expenses, error = errors });
        }

        [HttpGet]
        [Route("expenses/lists/{pageNo}")]
        public IActionResult GetAllExpenses(int pageNo)
        {
            var errors = new List<string>();
            var rowsPerPage = 5;
            int count = 0;
            var expensesQuery = new ExpensesQuery(_expenseConnection);
            IEnumerable<ExpensesView> expenseList = null;

            try
            {
                expenseList = expensesQuery.GetAllExpensesPaged(rowsPerPage, pageNo);
                count = expensesQuery.getExpensesCount();

                _logger.LogInformation("User : {username} fetches all expenses", LoggedInUser);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to fetch all expenses", LoggedInUser);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = new { expenseList, count },
                error = errors
            });
        }

        [HttpDelete]
        [Route("expenses/delete")]
        public IActionResult DeleteExpense(int  id)
        {
            var errors = new List<String>();
            var expenseAccess = new ExpensesAccess() { ExpenseConnection = _expenseConnection };

            try
            {
                _expenseConnection.BeginTransaction();
                expenseAccess.Delete(id);
                _expenseConnection.Commit();
            }
            catch(Exception ex)
            {
                _logger.LogWarning("User : {username} get exception while deleting expenses.", LoggedInUser);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = "",
                error = errors
            });
        }
    }
}