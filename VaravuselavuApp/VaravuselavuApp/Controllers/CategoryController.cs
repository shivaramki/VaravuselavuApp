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

namespace VaravuselavuStandard.Controllers
{
    [Authorize]
    public class CategoryController : CustomController
    {
        ExpenseConnection _expenseConnection;
        AppConfiguration _appConfiguration;
        ILogger<CategoryController> _logger;

        public CategoryController(IOptions<AppConfiguration> appConfiguration, ILogger<CategoryController> logger, ExpenseConnection expenseConnection)
        {
            _appConfiguration = appConfiguration.Value;
            _logger = logger;
            _expenseConnection = expenseConnection;
        }

        [HttpPost]
        [Route("category/add")]
        public IActionResult CreateCategory(Category category)
        {
            var errors = new List<string>();
            var categoryAccess = new CategoryAccess() { ExpenseConnection = _expenseConnection };

            try
            {
                _expenseConnection.BeginTransaction();
                categoryAccess.Insert(category);
                _expenseConnection.Commit();

                _logger.LogInformation("User : {username} added new category '{categoryname}'", LoggedInUser, category.Name);
            }
            catch (CustomExceptions Ex)
            {
                _logger.LogWarning("User : {username} get validation exception while adding category with name '{categoryname}' ", LoggedInUser, category.Name);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                errors = (List<string>)Ex.Data["errors"];
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("User : {username} get exception while adding category with name '{categoryname}' ", LoggedInUser, category.Name);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = category,
                error = errors
            });

        }


        [HttpPost]
        [Route("category/update")]
        public IActionResult UpdateCategory(Category category)
        {
            var errors = new List<string>();
            var categoryAccess = new CategoryAccess() { ExpenseConnection = _expenseConnection };

            try
            {
                _expenseConnection.BeginTransaction();
                categoryAccess.Update(category);
                _expenseConnection.Commit();

                _logger.LogInformation("User : {username} updated category '{categoryname}'", LoggedInUser, category.Name);
            }
            catch (CustomExceptions Ex)
            {
                _logger.LogWarning("User : {username} get validation exception while updating category with name '{categoryname}' ", LoggedInUser, category.Name);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                errors = (List<string>)Ex.Data["errors"];
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("User : {username} get exception while updating category with name '{categoryname}' ", LoggedInUser, category.Name);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = category,
                error = errors
            });
        }

        [HttpDelete]
        [Route("category/delete")]
        public IActionResult DeleteCategory(int id)
        {
            var errors = new List<string>();
            var categoryAccess = new CategoryAccess() { ExpenseConnection = _expenseConnection };

            try
            {
                _expenseConnection.BeginTransaction();
                categoryAccess.Delete(id);
                _expenseConnection.Commit();

                _logger.LogInformation("User : {username} deleted category id '{id}'", LoggedInUser, id);
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("User : {username} get exception while deleting category with id '{id}' ", LoggedInUser, id);

                if (_expenseConnection.InTransaction)
                    _expenseConnection.RollBack();

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                error = errors
            });
        }

        [HttpGet]
        [Route("category/search")]
        public IActionResult FindCategoryById(int id)
        {
            var errors = new List<string>();
            var categoryAccess = new CategoryAccess() { ExpenseConnection = _expenseConnection };

            var category = new Category();
            try
            {
                category = categoryAccess.GetById(id);
                _logger.LogInformation("User : {username} fetches the category id : {id}", LoggedInUser, id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to fetch the category : {id}", LoggedInUser, id);

                LogExecption(ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new { success = (errors.Count == 0), data = category, error = errors });
        }

        [HttpGet]
        [Route("category/lists/{pageNo}")]
        public IActionResult GetPagedCategory(int pageNo)
        {
            var errors = new List<string>();
            var rowsPerPage = 5;
            int count = 0;
            var categoryAccess = new CategoryAccess() { ExpenseConnection = _expenseConnection };

            IEnumerable<Category> categoryList = null;

            try
            {
                categoryList = categoryAccess.GetAllCategoryByPaged(rowsPerPage,pageNo);
                count = categoryAccess.GetCategoryCount();

                _logger.LogInformation("User : {username} fetches all category", LoggedInUser);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to fetch data for pageno {pageNo}", LoggedInUser,pageNo);

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = new { categoryList, count },
                error = errors
            });
        }

        [HttpGet]
        [Route("category/all-lists")]
        public IActionResult GetAllCategory()
        {
            var errors = new List<string>();
            var categoryAccess = new CategoryAccess() { ExpenseConnection = _expenseConnection };

            IEnumerable<Category> categoryList = null;

            try
            {
                categoryList = categoryAccess.GetAll();

                _logger.LogInformation("User : {username} fetches all category", LoggedInUser);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("User : {username} gets expection while trying to fetch data for pageno {pageNo}", LoggedInUser);

                LogExecption(Ex, _expenseConnection);

                errors = new List<string>() { _appConfiguration.Messages.InternalServerError };
            }

            return Json(new
            {
                success = (errors.Count == 0),
                data = categoryList,
                error = errors
            });
        }
    }
}