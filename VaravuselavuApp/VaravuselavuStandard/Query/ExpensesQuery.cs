using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Query
{
    public class ExpensesQuery
    {
        public ExpenseConnection _connection;

        public ExpensesQuery(ExpenseConnection expenseConnection)
        {
            _connection = expenseConnection;
        }

        public IEnumerable<ExpensesView> GetAllExpensesPaged(int rowsPerPage,int pageNo)
        {

           return _connection.Query<ExpensesView>(@"select result.*,app_user.username 
                    from (select expenses.id,(select getdivideamongnames(expenses.divideamong))as divideamongnames,expenses.paidby,expenses.comment,expenses.divideamong,
                    expenses.amount,expenses.expensedate::timestamp::date,category.id as categoryid,category.name as categoryname 
                    from expenses left join category on expenses.category = category.id)as result
                    left join app_user on result.paidby = app_user.id order by expensedate DESC OFFSET ((@pageNo -1) * @rowsPerPage) ROWS FETCH NEXT 5 ROWS ONLY", new { pageNo = pageNo, rowsPerPage = rowsPerPage });
        }

        public int getExpensesCount()
        {
            return Convert.ToInt32(_connection.ExecuteScalar(@"select count(*)
                    from (select expenses.id,(select getdivideamongnames(expenses.divideamong))as divideamongnames,expenses.paidby,expenses.comment,expenses.divideamong,
                    expenses.amount,expenses.expensedate::timestamp::date,category.id as categoryid,category.name as categoryname 
                    from expenses left join category on expenses.category = category.id)as result
                    left join app_user on result.paidby = app_user.id"));
        }
    }
}
