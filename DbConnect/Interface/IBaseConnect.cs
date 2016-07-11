using System;
using System.Collections.Generic;
using System.Data;

namespace DbConnect.Interface
{
    public interface IBaseConnect : IDisposable
    {
       
        DataSet GetDataByQuery(string strQuery ) ;
        DataSet GetDataByStore(string storeName, params object[] paramValues);
        DataSet GetDataByStore(string storeName, List<object> paramValues);
        DataSet GetDataByStore(string storeName, List<KeyValuePair<string, object>> paramValues);

        int ExecNonQuery(string strQuery);
        int ExecStore(string storeName, List<object> paramValues);
        int ExecStore(string storeName, List<KeyValuePair<string, object>> paramValues);
        int ExecStoreWithOutputParameter(string storeName, List<object> paramValues);
        int ExecStoreWithOutputParameter(string storeName, List<object> paramValues, ref List<object> dbset);
        int ExecStoreWithOutputParameter(string storeName, List<KeyValuePair<string,object>> paramValues);

    }
}
