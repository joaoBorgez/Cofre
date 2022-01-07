using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using WinFormsApp1.Entities;

namespace WinFormsApp1.DataProvider
{
    public class DataProviderBase : IDisposable
    {
        #region Private / Internal Members

        private class DatabaseContext
        {

            public DatabaseContext(DateTime initializeDateTime) 
            {
                this.InitializeDateTime = initializeDateTime;
            }

            public Database DataBase { get; set; }
            public DbConnection Connection { get; set; }
            public DbTransaction Transaction { get; set; }
            public DateTime InitializeDateTime { private set; get; }
        }

        public class SafeDictionary<TKey, TValue>
        {
            private readonly object m_Lock = new object();

            private readonly Dictionary<TKey, TValue> m_Dictionary = new Dictionary<TKey, TValue>();

            public TValue this[TKey key]
            {
                get
                {
                    lock (m_Lock)
                    {
                        return m_Dictionary[key];
                    }
                }

                set
                {
                    lock (m_Lock)
                    {
                        m_Dictionary[key] = value;
                    }
                }
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                lock (m_Lock)
                {
                    return m_Dictionary.TryGetValue(key, out value);
                }
            }

            public bool ContainsKey(TKey key)
            {
                lock (m_Lock)
                {
                    return m_Dictionary.ContainsKey(key);
                }
            }

            public bool Remove(TKey key)
            {
                lock (m_Lock)
                {
                    return m_Dictionary.Remove(key);
                }
            }

            public Dictionary<TKey, TValue>.KeyCollection Keys
            {
                get
                {
                    lock (m_Lock)
                    {
                        return m_Dictionary.Keys;
                    }
                }
            }
        }

        private DatabaseContext CurrentDataBase
        {
            get 
            {
                if (!CurrentDataBaseRequests.ContainsKey(this.InteractionId))
                {
                    throw new ArgumentException("CurrentDataBaseRequests.ContainsKey(this.m_InteractionId)");
                }

                return (DatabaseContext)CurrentDataBaseRequests[this.InteractionId];
            }
            set { CurrentDataBaseRequests[this.InteractionId] = value; }
        }

        private static SafeDictionary<Guid, DatabaseContext> CurrentDataBaseRequests = new SafeDictionary<Guid, DatabaseContext>();

        internal static readonly string LavConnectionString = "connLav";

        internal static readonly string DCRHConnectionString = "connRH";

        internal static readonly Database LavDataBaseStatic;

        //internal static readonly Database LavDataBaseStatic = CreateDatabase(DataProviderBase.LavConnectionString);

        //internal static readonly Database DCRHDataBaseStatic = DatabaseFactory.CreateDatabase(DataProviderBase.DCRHConnectionString);

        #endregion

        #region Constructors And Destructors

        public DataProviderBase() { }

        public DataProviderBase(InteractionContextEntity interactionContext)
        {
            if (interactionContext == null)
            {
                throw new NullReferenceException("DataProviderBase | interactionContext could not be null");
            }

            this.InteractionContext = interactionContext;
            this.InteractionId = interactionContext.InteractionId;

            if (!CurrentDataBaseRequests.ContainsKey(this.InteractionId))
            {
                this.CurrentDataBase = new DatabaseContext(DateTime.Now);
                this.CurrentDataBase.DataBase = DatabaseFactory.CreateDatabase(DataProviderBase.LavConnectionString);

            }
        }

        ~DataProviderBase()
        {
            this.Dispose();
        }

        #endregion

        #region Properties

        protected Guid InteractionId { private set; get; }

        protected InteractionContextEntity InteractionContext { private set; get; }

        protected string UserId
        {
            get { return this.InteractionContext == null ? null : this.InteractionContext.UserId; }
        }

        public string UserAplicacional
        {
            get { return this.InteractionContext == null ? null : this.InteractionContext.UserAplicacional; }
        }

        internal Database CofreDatabase
        {
            get { return this.CurrentDataBase.DataBase; }
        }

        internal bool ExistsActiveTransaction
        {
            get { return this.CurrentDataBase!= null && this.CurrentDataBase.Transaction != null; }
        }

        internal DbTransaction CurrentTransaction
        {
            get { return this.CurrentDataBase == null ? null : this.CurrentDataBase.Transaction; }
        }

        #endregion

        #region Methods

        internal IDataReader ExecuteReader(DbCommand command)
        {
            if (command.CommandTimeout > 0 && command.CommandTimeout <= 30)
            {
                command.CommandTimeout = 45;
            }

            if (ExistsActiveTransaction)
            {

                return this.CofreDatabase.ExecuteReader(command, CurrentTransaction);
            }
            else
            {
                return this.CofreDatabase.ExecuteReader(command);
            }
        }

        /// <summary>
        /// This statement will close the connection if there is no active transaction
        /// </summary>
        internal object ExecuteScalar(DbCommand command)
        {
            try
            {
                if (command.CommandTimeout > 0 && command.CommandTimeout <= 30)
                {
                    command.CommandTimeout = 45;
                }

                if (ExistsActiveTransaction)
                {
                    return this.CofreDatabase.ExecuteScalar(command, CurrentTransaction);
                }
                else
                {
                    return this.ImoveisDatabase.ExecuteScalar(command);
                }
            }
            finally
            {
                CloseConnectionIfThereIsNoTransaction();
            }
        }

        /// <summary>
        /// This statement will close the connection if there is no active transaction
        /// </summary>
        internal int ExecuteNonQuery(DbCommand command)
        {
            try
            {
                if (command.CommandTimeout > 0 && command.CommandTimeout <= 30)
                {
                    command.CommandTimeout = 45;
                }

                
                if (ExistsActiveTransaction)
                {
                    return this.CofreDatabase.ExecuteNonQuery(command, CurrentTransaction);
                }
                else
                {
                    return this.ImoveisDatabase.ExecuteNonQuery(command);
                }
            }
            finally
            {
                CloseConnectionIfThereIsNoTransaction();
            }
        }

        internal DbTransaction BeginTransaction()
        {
            if (this.CurrentDataBase.Connection == null || this.CurrentDataBase.Connection.State == ConnectionState.Closed)
            {
                this.CurrentDataBase.Connection = this.CurrentDataBase.DataBase.CreateConnection();
                this.CurrentDataBase.Connection.Open();
                this.CurrentDataBase.Transaction = this.CurrentDataBase.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                return this.CurrentDataBase.Transaction;
            }

            if (this.CurrentDataBase.Connection != null && this.CurrentDataBase.Connection.State == ConnectionState.Open && this.CurrentDataBase.Transaction.Connection == null)
            {
                this.CurrentDataBase.Transaction = this.CurrentDataBase.Connection.BeginTransaction();
                return this.CurrentDataBase.Transaction;
            }

            throw new Exception("Could not initiate transaction");

        }

        /// <summary>
        /// Commit Transaction And Close Connection
        /// </summary>
        internal void CommitTransaction()
        {


            if (this.CurrentDataBase.Connection != null && this.CurrentDataBase.Connection.State == ConnectionState.Open)
            {
                this.CurrentDataBase.Transaction.Commit();

                this.CurrentDataBase.Connection.Close();
                this.CurrentDataBase.Transaction = null;

                this.CurrentDataBase = null;

                CurrentDataBaseRequests.Remove(this.InteractionId);

            }

        }

        /// <summary>
        /// Rollback Transaction And Close Connection
        /// </summary>
        internal void RollbackTransaction()
        {

            if (this.CurrentDataBase.Connection != null && this.CurrentDataBase.Connection.State == ConnectionState.Open)
            {
                if (this.CurrentDataBase.Transaction != null && this.CurrentDataBase.Transaction.Connection != null && this.CurrentDataBase.Transaction.Connection.State == ConnectionState.Open)
                {
                    this.CurrentDataBase.Transaction.Rollback();
                }

                this.CurrentDataBase.Connection.Close();

                this.CurrentDataBase = null;

                CurrentDataBaseRequests.Remove(this.InteractionId);

            }
        }


        private void CloseConnectionIfThereIsNoTransaction()
        {
            if (!ExistsActiveTransaction)
            {
                if (this.CurrentDataBase.Connection != null && this.CurrentDataBase.Connection.State == ConnectionState.Open)
                {
                    this.CurrentDataBase.Connection.Close();
                }

                this.CurrentDataBase = null;

                CurrentDataBaseRequests.Remove(this.InteractionId);
            }
        }

        public void Dispose()
        {
            List<Guid> requestsToRemove = new List<Guid>();

            foreach (Guid key in CurrentDataBaseRequests.Keys)
            {
                DatabaseContext context = CurrentDataBaseRequests[key];

                if (context.InitializeDateTime < DateTime.Now.AddMinutes(-5))
                {
                    requestsToRemove.Add(key);
                }
            }

            if (requestsToRemove.Count == 0)
            {
                return;
            }

            for (int i = 0; i < requestsToRemove.Count; i++)
            {
                CurrentDataBaseRequests.Remove(requestsToRemove[i]);
            }

            
        }

        #endregion

        #region Static Methods

        internal static void CloseConnection(DbCommand command)
        {

            if (command.Connection != null && command.Connection.State != ConnectionState.Closed)
            {
                command.Connection.Close();
                command.Connection = null;
            }
        }

        #endregion
    }
}