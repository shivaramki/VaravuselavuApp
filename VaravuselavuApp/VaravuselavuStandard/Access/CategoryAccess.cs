using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaravuselavuStandard.AbstractDbAccess;
using VaravuselavuStandard.Models;

namespace VaravuselavuStandard.Access
{
    public class CategoryAccess : CategoryDbAccess
    {
        protected override bool UserInsert(Category entity)
        {
            return _connection.Insert(entity);
        }

        protected override bool UserUpdate(Category entity)
        {
            return _connection.Update(entity);
        }

        protected override bool ValidateInsert(Category entity)
        {
            if (CategoryExists(entity)==true)
                _errors.Add("Category already exists, cannot duplicate");

            return Convert.ToBoolean(_errors.Count == 0);
        }

        protected override bool ValidateMandatory(Category entity)
        {
            if (entity.Name == string.Empty)
                _errors.Add("Category name should not be empty");

            return Convert.ToBoolean(_errors.Count == 0);
        }

        protected override bool ValidateUpdate(Category entity)
        {
            if (CategoryExists(entity) == true)
                _errors.Add("Category with that name exists already, correct category name");

            return Convert.ToBoolean(_errors.Count == 0);
       }

        public bool CategoryExists(Category entity)
        {
            return Convert.ToBoolean(_connection.ExecuteScalar(@"select count(name) from category where upper(trim(name)) = @name", new { name = entity.Name.Trim().ToUpper() }));
        }

        public IEnumerable<Category> GetAll()
        {
            return _connection.GetAll<Category>();
        }

        public IEnumerable<Category> GetAllCategoryByPaged(int rowsPerPage,int pageNo)
        {
            return _connection.Query<Category>(@"select * from category OFFSET ((@pageNo -1) * @rowsPerPage) ROWS FETCH NEXT 5 ROWS ONLY", new { pageNo = pageNo, rowsPerPage = rowsPerPage });
        }

        public Category GetCategoryByExactName(string name)
        {
            return _connection.Query<Category>(@"select * from category where upper(trim(name)) = @name", new { name = name.Trim().ToUpper()}).FirstOrDefault();
        }

        public int GetCategoryCount()
        {
            return Convert.ToInt32(_connection.ExecuteScalar(@"select count(*) from category"));
        }

        public Category GetById(dynamic id)
        {
            return _connection.Get<Category>(id);
        }

        public void Delete(int id)
        {
            _connection.Execute("delete from category where id=@id", new { id = id });
        }

    }

}
