using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Query
{
    public class DashboardQuery
    {
        public ExpenseConnection _connection;

        public DashboardQuery(ExpenseConnection expenseConnection)
        {
            _connection = expenseConnection;
        }

        public dynamic GetCategoryWiseExpenseAmount()
        {
            return _connection.Query<dynamic>(@"SELECT name, SUM(amount) AS amount FROM expenses left join category  on expenses.category = category.id GROUP BY name");
        }

        public dynamic GetMonthWiseExpenditure()
        {
            return _connection.Query<dynamic>(@"select g.month,SUM(e.amount) from generate_series(1, 12) as g(month) left join expenses as e on extract(year from e.expensedate) = 2018 and extract(month from e.expensedate) = g.month group by g.month order by g.month");
        }
    }
}
