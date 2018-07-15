using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.Models;
using Dapper;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.AbstractDbAccess
{
    public abstract class CategoryDbAccess : BaseDbAccess
    {
        protected abstract bool ValidateInsert(Category entity);
        protected abstract bool ValidateUpdate(Category entity);
        protected abstract bool ValidateMandatory(Category entity);

        protected abstract bool UserInsert(Category entity);
        protected abstract bool UserUpdate(Category entity);

        public bool Insert(Category entity)
        {
            if (!ValidateMandatory(entity) || !ValidateInsert(entity))
                RaiseException();

            return UserInsert(entity);
        }

        public bool Update(Category entity)
        {
            if (!ValidateMandatory(entity) || !ValidateUpdate(entity))
                RaiseException();

            return UserUpdate(entity);
        }
    }
}
