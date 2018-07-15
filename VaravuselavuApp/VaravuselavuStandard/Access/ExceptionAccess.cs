using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.Models;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Access
{
    public class ExceptionAccess
    {
        public ExpenseConnection _connection;

        public ExceptionAccess(ExpenseConnection expenseConnection)
        {
            _connection = expenseConnection;
        }

        public void InsertException(AppErrors appError)
        {
            _connection.Insert(appError);
        }
    }
}
