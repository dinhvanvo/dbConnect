using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DbConnect
{
    public class SqlHelper : BaseConnect
    {
        public override DataSet GetDataByQuery(string strQuery)
        {
            DataSet dset = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionStringName))
            {
                SqlCommand cmd = new SqlCommand(strQuery, conn) { CommandType = CommandType.Text };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    adap.Fill(dset);
                }
                catch { dset = null; }
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
            using (var conn = new SqlConnection(ConnectionStringName))
            {
                SqlCommand cmd = new SqlCommand(storeName, conn) { CommandType = CommandType.StoredProcedure };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    SqlCommandBuilder.DeriveParameters(cmd);
                    int index = 0;
                    foreach (SqlParameter sqlParam in cmd.Parameters)
                    {
                        if (sqlParam.Direction == ParameterDirection.Input || sqlParam.Direction == ParameterDirection.InputOutput)
                        {
                            if (paramValues.Count <= index || paramValues[index] == null)
                                sqlParam.Value = DBNull.Value;
                            else
                                sqlParam.Value = paramValues[index];
                            index++;
                        }
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dset);
                }
                catch { dset = null; }
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
            using (var conn = new SqlConnection(ConnectionStringName))
            {
                SqlCommand cmd = new SqlCommand(storeName, conn) { CommandType = CommandType.StoredProcedure };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    foreach (var item in paramValues)
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dset);
                }
                catch { dset = null; }
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
            using (var conn = new SqlConnection(ConnectionStringName))
            {
                SqlCommand cmd = new SqlCommand(storeName, conn) { CommandType = CommandType.StoredProcedure };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    SqlCommandBuilder.DeriveParameters(cmd);
                    int index = 0;
                    foreach (SqlParameter sqlParam in cmd.Parameters)
                    {
                        if (sqlParam.Direction == ParameterDirection.Input)
                        {
                            if (paramValues.Count() <= index || paramValues[index] == null)
                                sqlParam.Value = DBNull.Value;
                            else
                                sqlParam.Value = paramValues[index];
                            index++;
                        }
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dset);
                }
                catch { dset = null; }
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
            using (var conn = new SqlConnection(ConnectionStringName))
            {
                var trans = conn.BeginTransaction();
                var cmd = new SqlCommand(strQuery, conn) { CommandType = CommandType.Text, Transaction = trans };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
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
            using (SqlConnection conn = new SqlConnection(ConnectionStringName))
            {
                SqlTransaction trans = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    foreach (var item in paramValues)
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    irec = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
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
            using (var conn = new SqlConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                try
                {
                    SqlCommandBuilder.DeriveParameters(cmd);
                    int index = 0;
                    foreach (SqlParameter sqlParam in cmd.Parameters)
                    {
                        if (sqlParam.Direction == ParameterDirection.Input)
                        {
                            if (paramValues.Count() <= index || paramValues[index] == null)
                                sqlParam.Value = DBNull.Value;
                            else
                                sqlParam.Value = paramValues[index];
                            index++;
                        }
                    }
                    irec = cmd.ExecuteNonQuery();
                }
                catch
                {
                    irec = -1;
                    trans.Rollback();
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
            using (var conn = new SqlConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                try
                {
                    foreach (var param in paramValues)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);        
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
            using (var conn = new SqlConnection(ConnectionStringName))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                var trans = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(storeName, conn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                Hashtable hashOut = new Hashtable();
                SqlCommandBuilder.DeriveParameters(cmd);
                int index = 0;
                int i = 0;
                try
                {
                    foreach (SqlParameter parameter in cmd.Parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Input || parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (paramValues.Count() <= index || paramValues[index] == null)
                                parameter.Value = DBNull.Value;
                            else parameter.Value = paramValues[index];

                            if (parameter.Direction == ParameterDirection.InputOutput)
                                hashOut.Add(index, i);
                            index++;
                        }
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

    }

}
