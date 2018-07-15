using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.Models;
using Dapper;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.AbstractDbAccess
{
    public abstract class ExpenseDbAccess : BaseDbAccess
    {
        protected abstract bool ValidateInsert(Expenses entity);
        protected abstract bool ValidateUpdate(Expenses entity);
        protected abstract bool ValidateMandatory(Expenses entity);

        protected abstract bool UserInsert(Expenses entity);
        protected abstract bool UserUpdate(Expenses entity);

        public bool Insert(Expenses entity)
        {
            if (!ValidateMandatory(entity) || !ValidateInsert(entity))
                RaiseException();

            return UserInsert(entity);
        }

        public bool Update(Expenses entity)
        {
            if ( !ValidateMandatory(entity) || !ValidateUpdate(entity))
                RaiseException();

            return UserUpdate(entity);
        }
    }
}
