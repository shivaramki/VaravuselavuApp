using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using Npgsql;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using VaravuselavuStandard.Util;

namespace VaravuselavuStandard.Util
{
    public class ExpenseConnection : IDisposable
    {
        AppConfiguration _appConfiguration;
        DbConnection _connection;
        DbTransaction _dbTransaction;

        private bool _inTransaction;
        public bool InTransaction
        {
            get => _inTransaction;
            set => _inTransaction = value;
        }

        private int? _commandTimeout;
        public int? CommandTimeout { set { _commandTimeout = value; } }

        public ExpenseConnection(IOptions<AppConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration.Value;
            CreateConnection();
        }

        public void CreateConnection()
        {
            if (_appConfiguration.ConnectionStrings.DefaultConnectionString != string.Empty)
                _connection = new NpgsqlConnection(CrytoHelper.DecryptString(_appConfiguration.ConnectionStrings.DefaultConnectionString, "E546C8DF278CD5931069B522E695D4F2"));
            else
                throw new Exception("No Connection string found");

            _connection.Open();
        }

        public void BeginTransaction()
        {
            if (_inTransaction)
                throw new Exception("Already in transaction");

            _dbTransaction = _connection.BeginTransaction();
            _inTransaction = true;
        }

        public void Commit()
        {
            if (!_inTransaction)
                throw new Exception("No transaction is in process");

            _dbTransaction.Commit();
            _inTransaction = false;
        }

        public void RollBack()
        {
            if (!_inTransaction)
                throw new Exception("No transaction is in process");

            _dbTransaction.Rollback();
            _inTransaction = false;
        }

        public bool Insert<T>(T entity) where T : class
        {
            if (!_inTransaction)
                throw new Exception("No transaction is in progress");

            return Convert.ToBoolean(_connection.Insert(entity, transaction: _dbTransaction, commandTimeout: _commandTimeout));
        }

        public bool Update<T>(T entity) where T : class
        {
            if (!_inTransaction)
                throw new Exception("No transaction is in progress");
            return _connection.Update(entity, transaction: _dbTransaction, commandTimeout: _commandTimeout);
        }

        public bool Delete<T>(T entity) where T : class
        {
            if (!_inTransaction)
                throw new Exception("No Transaction is in progress");

            return _connection.Delete(entity, transaction: _dbTransaction, commandTimeout: _commandTimeout);
        }

        public T Get<T>(dynamic Id) where T : class
        {
            return SqlMapperExtensions.Get<T>(_connection, Id, transaction: _dbTransaction, commandTimeout: _commandTimeout);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _connection.GetAll<T>(_dbTransaction, _commandTimeout);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return _connection.Query<T>(sql, param, transaction: _dbTransaction, commandTimeout: _commandTimeout);
        }

        public object ExecuteScalar(string sql, object param = null)
        {
            return _connection.ExecuteScalar(sql, param, transaction: _dbTransaction, commandTimeout: _commandTimeout);
        }

        public void Execute(string sql,object param = null)
        {
             _connection.Execute(sql, param, transaction: _dbTransaction, commandTimeout: _commandTimeout);
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
