using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.AbstractDbAccess;
using VaravuselavuStandard.Models;

namespace VaravuselavuStandard.Access
{
    public class ExpensesAccess : ExpenseDbAccess
    {
        
        protected override bool UserInsert(Expenses entity)
        {
            return _connection.Insert(entity);
        }

        protected override bool UserUpdate(Expenses entity)
        {
            return _connection.Update(entity);
        }

        protected override bool ValidateInsert(Expenses entity)
        {
            return Convert.ToBoolean(_errors.Count == 0);
        }

        protected override bool ValidateMandatory(Expenses entity)
        {
            if (entity.Category == 0)
                _errors.Add("Please select the category to which expense made");

            if (entity.PaidBy == 0)
                _errors.Add("Paid by cannot be empty, please select the paid by user");

            if (entity.DivideAmong.Length == 0)
                _errors.Add("Expense added should be divided among others");

            if (entity.Amount == 0)
                _errors.Add("Amount field should not be empty");

            if (!entity.ExpenseDate.HasValue)
                _errors.Add("Please provide the expense date");

            return Convert.ToBoolean(_errors.Count == 0);
        }

        protected override bool ValidateUpdate(Expenses entity)
        {
            return Convert.ToBoolean(_errors.Count == 0);
        }

        public IEnumerable<Expenses> GetAll()
        {
            return _connection.GetAll<Expenses>();
        }

        public IEnumerable<Expenses> GetAllExpnesesByPaged(int rowsPerPage, int pageNo)
        {
            return _connection.Query<Expenses>("@select * from expenses OFFSET ((@pageNo -1) * @rowsPerPage) ROWS FETCH NEXT 5 ROWS ONLY", new { pageNo = pageNo, rowsPerPage = rowsPerPage });
        }

        public Expenses GetById(dynamic id)
        {
            return _connection.Get<Expenses>(id);
        }

        public void Delete(int id)
        {
            _connection.Execute("delete from expenses where id=@id", new { id = id });
        }

    }
}
