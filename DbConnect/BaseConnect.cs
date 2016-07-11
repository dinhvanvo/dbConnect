using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.Caching;

namespace DbConnect
{
    public class BaseConnect : Interface.IBaseConnect
    {

        static int WebConnectCount { get { return ConnectionCount("WebConnectionCount"); } }
        static int GameConnectCount { get { return ConnectionCount("GameConnectionCount"); } }
        static int CMSConnectCount { get { return ConnectionCount("CMSConnectionCount"); } }

        public static string WebConnectionString { get { return ConnectionName("OngameWeb_ConnectionString"); } }
        public static string GameConnectionString { get { return ConnectionName("OngameGame_ConnectionString"); } }
        public static string CMSConnectionString { get { return ConnectionName("CMS_ConnectionString"); } }
        
        public static string ConnectionStringName { get; set; }
        
        private static int ConnectionCount(string connectionName)
        {
            int count = 0;
            int.TryParse(ConfigurationManager.AppSettings[connectionName], out count);
            return count;
        }

        private static string ConnectionName(string connectionName)
        {
            var _Count = WebConnectCount;
            if (_Count > 0)
            {
                Random rnd = new Random();
                return string.Format(connectionName + "_{0}", rnd.Next(1, _Count));
            }
            else return connectionName;
        }
        

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual DataSet GetDataByQuery(string strQuery)
        {
            throw new NotImplementedException();
        }

        public virtual DataSet GetDataByStore(string storeName, params object[] paramValues)
        {
            throw new NotImplementedException();
        }

        public virtual DataSet GetDataByStore(string storeName, List<object> paramValues)
        {
            throw new NotImplementedException();
        }

        public virtual DataSet GetDataByStore(string storeName, List<KeyValuePair<string, object>> paramValues)
        {
            throw new NotImplementedException();
        }

        public virtual int ExecNonQuery(string strQuery)
        {
            throw new NotImplementedException();
        }

        public virtual int ExecStore(string storeName, List<object> paramValues)
        {
            throw new NotImplementedException();
        }

        public virtual int ExecStore(string storeName, List<KeyValuePair<string, object>> paramValues)
        {
            throw new NotImplementedException();
        }

        public virtual int ExecStoreWithOutputParameter(string storeName, List<object> paramValues)
        {
            throw new NotImplementedException();
        }

        public virtual int ExecStoreWithOutputParameter(string storeName, List<object> paramValues, ref List<object> dbset)
        {
            throw new NotImplementedException();
        }

        public virtual int ExecStoreWithOutputParameter(string storeName, List<KeyValuePair<string, object>> paramValues)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cmd"></param>
        ///// <returns></returns>
        //public static int ExecuteNonQuery(DbCommand cmd)
        //{
        //    return cmd.ExecuteNonQuery();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cmd"></param>
        ///// <returns></returns>
        //public static IDataReader ExecuteReader(DbCommand cmd)
        //{
        //    return ExecuteReader(cmd, CommandBehavior.Default);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cmd"></param>
        ///// <param name="behavior"></param>
        ///// <returns></returns>
        //public static IDataReader ExecuteReader(DbCommand cmd, CommandBehavior behavior)
        //{
        //    return cmd.ExecuteReader(behavior);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DbCommand cmd)
        {
            return cmd.ExecuteScalar();
        }

        #region Caching data
        /// <summary>
        /// Thuộc tính cache có bật ko?
        /// </summary>
        private static bool _enableCaching;
        protected static bool EnableCaching
        {
            get { return _enableCaching; }
            set { _enableCaching = value; }
        }

        /// <summary>
        /// The time store value in cache
        /// </summary>
        private static int _cacheDuration = 0;
        protected static int CacheDuration
        {
            get { return _cacheDuration; }
            set { _cacheDuration = value; }
        }

        /// <summary>
        /// Lấy thông tin cache tại thời điểm hiện tại
        /// </summary>
        protected static Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }

        #endregion

    }
}
