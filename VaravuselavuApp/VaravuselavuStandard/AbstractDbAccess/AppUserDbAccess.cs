using System;
using System.Collections.Generic;
using System.Text;
using VaravuselavuStandard.Models;
using Dapper;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.AbstractDbAccess
{
    public abstract class AppUserDbAccess : BaseDbAccess
    {
        protected abstract bool ValidateInsert(AppUser entity);
        protected abstract bool ValidateUpdate(AppUser entity);
        protected abstract bool ValidateMandatory(AppUser entity);

        protected abstract bool UserInsert(AppUser entity);
        protected abstract bool UserUpdate(AppUser entity);

        public bool Insert(AppUser entity)
        {
            if ( !ValidateMandatory(entity) || !ValidateInsert(entity))
                RaiseException();

            return UserInsert(entity);
        }

        public bool Update(AppUser entity)
        {
            if ( !ValidateMandatory(entity) || !ValidateUpdate(entity) )
                RaiseException();

            return UserUpdate(entity);
        }
    }
}
