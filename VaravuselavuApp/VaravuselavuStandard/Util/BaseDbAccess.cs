using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Util
{
    public class BaseDbAccess
    {
        protected ExpenseConnection _connection;
        public ExpenseConnection ExpenseConnection
        {
            get=> _connection;
            set => _connection = value;
        }

        protected List<string> _errors;

        public BaseDbAccess()
        {
            _errors = new List<string>();
        }

        public void RaiseException()
        {
            var exception = new CustomExceptions();
            exception.Data["errors"] = _errors;
            throw exception;
        }

    }
}