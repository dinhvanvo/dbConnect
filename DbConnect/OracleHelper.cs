using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using System.Collections;
using System.Data.Common;

namespace DbConnect
{
    public class OracleHelper : BaseConnect
    {
        public override DataSet GetDataByQuery(string strQuery)
        {
            DataSet dset = new DataSet();
            using (OracleConnection conn = new OracleConnection(ConnectionStringName))
            {
                var cmd = new OracleCommand(strQuery, conn) { CommandType = CommandType.Text };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    var adap = new OracleDataAdapter(cmd);
                    adap.Fill(dset);
                }
                catch(Exception ex)
                {
                    dset = null;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return dset;
        }

        public override DataSet GetDataByStore(string storeName, List<object> paramValues)
        {
            DataSet dset = new DataSet();
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OracleCommandBuilder.DeriveParameters(cmd);
                    int index = 0;
                    foreach (OracleParameter oraParam in cmd.Parameters)
                    {
                        if (oraParam.Direction == ParameterDirection.Input || oraParam.Direction == ParameterDirection.InputOutput)
                        {
                            if (paramValues.Count <= index || paramValues[index] == null)
                                oraParam.Value = DBNull.Value;
                            else
                                oraParam.Value = paramValues[index];
                            index++;
                        }
                    }
                    var adap = new OracleDataAdapter(cmd);
                    adap.Fill(dset);
                }
                catch (Exception ex)
                {
                    dset = null;
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }

            return dset;
        }

        public override DataSet GetDataByStore(string storeName, List<KeyValuePair<string, object>> paramValues)
        {
            DataSet dset = new DataSet();
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    foreach (var item in paramValues)
                        cmd.Parameters.Add(item.Key, item.Value);
                    var adap = new OracleDataAdapter(cmd);
                    adap.Fill(dset);
                }
                catch (Exception ex)
                {
                    dset = null;
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return dset;
        }

        public override DataSet GetDataByStore(string storeName, params object[] paramValues)
        {
            DataSet dset = new DataSet();
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OracleCommandBuilder.DeriveParameters(cmd);
                    int index = 0;
                    foreach (OracleParameter oraParam in cmd.Parameters)
                    {
                        if (oraParam.Direction == ParameterDirection.Input)
                        {
                            if (paramValues.Count() <= index || paramValues[index] == null)
                                oraParam.Value = DBNull.Value;
                            else
                                oraParam.Value = paramValues[index];
                            index++;
                        }
                    }
                    var adap = new OracleDataAdapter(cmd);
                    adap.Fill(dset);
                }
                catch (Exception ex)
                {
                    dset = null;
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }

            return dset;
        }

        public override int ExecNonQuery(string strQuery)
        {
            int irec = 0;
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                var cmd = new OracleCommand(strQuery, conn) { CommandType = CommandType.Text, Transaction = trans };
                try
                {
                    irec = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    irec = -1;
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Commit();
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return irec;
        }

        public override int ExecStore(string storeName, List<KeyValuePair<string, object>> paramValues)
        {
            int irec = 0;
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                try
                {
                    foreach (var item in paramValues)
                        cmd.Parameters.Add(item.Key, item.Value);
                    irec = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    irec = -1;
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Commit();
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return irec;
        }

        public override int ExecStore(string storeName, List<object> paramValues)
        {
            int irec = 0;
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                try
                {
                    OracleCommandBuilder.DeriveParameters(cmd);
                    int index = 0;
                    foreach (OracleParameter oraParam in cmd.Parameters)
                    {
                        if (oraParam.Direction == ParameterDirection.Input)
                        {
                            if (paramValues.Count() <= index || paramValues[index] == null)
                                oraParam.Value = DBNull.Value;
                            else
                                oraParam.Value = paramValues[index];
                            index++;
                        }
                    }
                    irec = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    irec = -1;
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Commit();
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return irec;
        }

        public override int ExecStoreWithOutputParameter(string storeName, List<KeyValuePair<string, object>> paramValues)
        {
            int _irec = 0;
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                try
                {
                    foreach (var param in paramValues)
                        cmd.Parameters.Add(param.Key, param.Value);
                    _irec = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    _irec = -1;
                    trans.Rollback();
                    throw ex;                    
                }
                finally
                {
                    trans.Commit();
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return _irec;
        }

        public override int ExecStoreWithOutputParameter(string storeName, List<object> paramValues)
        {
            int _irec = 0;
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                Hashtable hashOut = new Hashtable();
                OracleCommandBuilder.DeriveParameters(cmd);
                int index = 0;
                int i = 0;
                try
                {
                    foreach (OracleParameter parameter in cmd.Parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Input )
                        {
                            if (paramValues.Count() <= index || paramValues[index] == null){
                                parameter.Value = DBNull.Value;
                            } else {
                                parameter.Value = paramValues[index];
                            }

                        } else if (parameter.Direction == ParameterDirection.Output){
                            hashOut.Add(index, i);
                        }

                        index++;
                        i++;
                    }
                    _irec = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    _irec = -1;
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Commit();
                    foreach (DictionaryEntry item in hashOut)
                    {
                        index = (int)item.Key;
                        i = (int)item.Value;
                        paramValues[index] = cmd.Parameters[i].Value;
                    }
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return _irec;
        }

        public override int ExecStoreWithOutputParameter(string storeName, List<object> paramValues, ref List<object> dbset)
        {
            int _irec = 0;
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                var cmd = new OracleCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                Hashtable hashOut = new Hashtable();
                OracleCommandBuilder.DeriveParameters(cmd);
                int index = 0;
                int i = 0;
                try
                {
                    foreach (OracleParameter parameter in cmd.Parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Input)
                        {
                            if (paramValues.Count() <= index || paramValues[index] == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = paramValues[index];
                            }

                        }
                        else if (parameter.Direction == ParameterDirection.Output)
                        {
                            hashOut.Add(index, i);
                        }

                        index++;
                        i++;
                    }
                    _irec = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    _irec = -1;
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Commit();
                    foreach (DictionaryEntry item in hashOut)
                    {
                        index = (int)item.Key;
                        i = (int)item.Value;
                        paramValues[index] = cmd.Parameters[i].Value;
                        if(paramValues[index].GetType() == typeof(Oracle.DataAccess.Client.OracleDataReader))
                        {
                            DataTable dt = new DataTable();
                            var _ord = (OracleDataReader)paramValues[index];
                            dt.Load(_ord);
                            dbset.Add(dt);
                        }
                    }
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return _irec;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand cmd)
        {
            return ExecuteReader(cmd, CommandBehavior.Default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand cmd, CommandBehavior behavior)
        {
            return cmd.ExecuteReader(behavior);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(string strQuery)
        {
            object irec = null;
            using (var conn = new OracleConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var cmd = new OracleCommand(strQuery, conn) { CommandType = CommandType.Text };
                try
                {
                    irec = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    irec = null;
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            return irec;
        }
    }
}
