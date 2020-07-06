using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace APITest.Helper
{
    /// <summary>
    /// Class thao tác với CSDL
    /// </summary>
    public class SqlHelper
    {

        #region "# SQLAuthenticationType #"
        public enum SQLAuthenticationType
        {
            WindowsAuthentication = 0,
            SQLServerAuthentication = 1
        }
        #endregion

        #region "# Property SQL Server  #"

        private static string _ServerName = "";
        private static string _DatabaseName = "";
        private static string _SqlUserName = "";
        private static string _SqlPassword = "";
        private static SQLAuthenticationType _Authentication = SQLAuthenticationType.SQLServerAuthentication;
        private string _SQLConnectionString = "";
        private SqlConnection _objSqlConn;
        private string _LoiNgoaiLe = "";

        public string _strSQL = "";
        public static string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }

        public static string DatabaseName
        {
            get { return _DatabaseName; }
            set { _DatabaseName = value; }
        }

        public static string SqlUserName
        {
            get { return _SqlUserName; }
            set { _SqlUserName = value; }
        }

        public static string SqlPassword
        {
            get { return _SqlPassword; }
            set { _SqlPassword = value; }
        }

        public static SQLAuthenticationType Authentication
        {
            get { return _Authentication; }
            set { _Authentication = value; }
        }

        public string SQLConnectionString
        {
            get { return _SQLConnectionString; }
            set { _SQLConnectionString = value; }
        }

        public string LoiNgoaiLe
        {
            get { return _LoiNgoaiLe; }
            set { _LoiNgoaiLe = value; }
        }

        public SqlConnection objSqlConn
        {
            get { return _objSqlConn; }
            set { _objSqlConn = value; }
        }

        #endregion

        #region "# Sub New -#"
        protected SqlHelper(string strServerName, string strDatabaseName, string strUserNameSQL, string strPasswordSQL)
        {
            //_ServerName = strServerName
            //_DatabaseName = strDatabaseName
            //_UserNameSQL = strUserNameSQL
            //_PasswordSQL = strPasswordSQL
            //_Authentication = SQLAuthenticationType.SQLServerAuthentication
        }
        private string strConn;
		//viet them
        public SqlHelper ()
        {
            _objSqlParamtter = new List<SqlParameter>();
            _objSqlParamtterWhere = new List<SqlParameter>();
            //strConn = @"Data Source=DUONGDT;Initial Catalog=BARSDATA_HP_NEW;User ID=sa;Password=123456a@";
            //strConn = @"Data Source=192.168.11.227;Initial Catalog=BARSDATA_HP_NEW;User ID=sa;Password=cntt@123";
            //strConn = @"Data Source=192.168.11.227;Initial Catalog=HoKyThuat_MangLuoi;User ID=sa;Password=cntt@123";
            strConn = @"Data Source=192.168.11.10;Initial Catalog=HoKyThuat_MangLuoi;User ID=sa;Password=sqlqlml";
        }
        
        public SqlHelper(string _strConn)
        {
            _objSqlParamtter = new List<SqlParameter>();
            _objSqlParamtterWhere = new List<SqlParameter>();
            strConn = _strConn;
        }
        protected virtual void Dispose()
        {
            try
            {
                _objSqlConn.Dispose();
                if ((_objSqlConnWithTrans != null))
                {
                    _objSqlConnWithTrans.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region "# getSQLConnectionString #"
        public string getSQLConnectionString()
        {
            //return @"Data Source=.\SQLEXPRESS;Initial Catalog=SQEMS_OFFICE;User ID=sa;Password=sa";
            //return String.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServerName, DatabaseName, SqlUserName, SqlPassword);




            //if (!Program._IS_DEBUG_MODE)
            //{
            //    return String.Format(@"Data Source=vps_mes_server\SQL2016;Initial Catalog=VPS_MES_SOFTWARE;User ID=vps_mes_software;Password=p@ssw0rd123qwe");
            //}
            //else
            //{
            //    return String.Format(@"Data Source=vps_mes_server\SQL2016;Initial Catalog=VPS_TEST;User ID=sa;Password=umbalap@ssw0rd123qwe");
            //}

            //return String.Format(@"Data Source=vps_mes_server\SQL2016;Initial Catalog=VPS_MES_SOFTWARE;User ID=vps_mes_scada;Password=umbalap@ssw0rd123qwe");

            //return String.Format(@"Data Source=192.168.1.3;Initial Catalog=GIAMDINHCONTVAORA;User ID=sa;Password=123qwe");

            return strConn;
        }
        #endregion

        #region "# AddParameter #"
        private List<SqlParameter> _objSqlParamtter;
        private List<SqlParameter> _objSqlParamtterWhere;
        public List<SqlParameter> objSqlParamtter
        {
            get { return _objSqlParamtter; }
        }
        public List<SqlParameter> objSqlParamtterWhere
        {
            get { return _objSqlParamtterWhere; }
        }

        public void AddParameterBinaryNull(string parmName)
        {
            SqlParameter prm = new SqlParameter(parmName, SqlDbType.VarBinary);
            prm.Value = DBNull.Value;
            _objSqlParamtter.Add(prm);
        }

        public void AddParameter(string parmName, object parmValue)
        {
            if ((parmValue != null))
            {
                _objSqlParamtter.Add(new SqlParameter(parmName, parmValue));
            }
            else
            {
                _objSqlParamtter.Add(new SqlParameter(parmName, DBNull.Value));
            }
        }
        public void AddParameterWhere(string parmName, object parmValue)
        {
            if ((parmValue != null))
            {
                _objSqlParamtterWhere.Add(new SqlParameter(parmName, parmValue));
            }
            else
            {
                _objSqlParamtterWhere.Add(new SqlParameter(parmName, DBNull.Value));
            }
        }
        #endregion

        #region "# Transaction #"
        private SqlTransaction _objTransaction = null;
        private SqlConnection _objSqlConnWithTrans = null;
        public bool BeginTransaction()
        {
            _objSqlConnWithTrans = new SqlConnection(getSQLConnectionString());
            try
            {
                _objSqlConnWithTrans.Open();
                _objTransaction = _objSqlConnWithTrans.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _LoiNgoaiLe = ex.Message;
                return false;
            }
        }

        public bool ComitTransaction()
        {
            try
            {
                if (_objSqlConnWithTrans == null)
                    return false;
                _objTransaction.Commit();
                if (_objSqlConnWithTrans.State != ConnectionState.Closed)
                {
                    _objSqlConnWithTrans.Close();
                    _objTransaction = null;
                    _objSqlConnWithTrans = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RollBackTransaction()
        {
            try
            {
                if (_objSqlConnWithTrans == null)
                    return false;
                _objTransaction.Rollback();
                if (_objSqlConnWithTrans.State != ConnectionState.Closed)
                {
                    _objSqlConnWithTrans.Close();
                    _objTransaction = null;
                    _objSqlConnWithTrans = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

    
        #region "# ExecuteSQLDataTable #"
        public DataTable ExecuteSQLDataTable(string sSQL)
        {
            DataTable dt = new DataTable();
            SqlCommand _sqlCmd = new SqlCommand();

            try
            {
                _objSqlConn = new SqlConnection(getSQLConnectionString());
                _sqlCmd.CommandText = sSQL;
                _sqlCmd.Connection = _objSqlConn;
                _objSqlConn.Open();
                foreach (SqlParameter p in _objSqlParamtter)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                foreach (SqlParameter p in _objSqlParamtterWhere)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                SqlDataAdapter da = new SqlDataAdapter(_sqlCmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                _LoiNgoaiLe = ex.Message;
                throw ex;
            }
            finally
            {
                _sqlCmd.Dispose();
                _objSqlParamtter.Clear();
                _objSqlParamtterWhere.Clear();
                if (_objSqlConn.State != ConnectionState.Closed)
                {
                    _objSqlConn.Close();
                    _objSqlConn.Dispose();
                }
            }
        }
        #endregion

        #region "# ExecuteSQLDataSet #"
        public DataSet ExecuteSQLDataSet(string sSQL)
        {
            DataSet ds = new DataSet();
            SqlCommand _sqlCmd = new SqlCommand();
            try
            {
                _objSqlConn = new SqlConnection(getSQLConnectionString());
                _sqlCmd.CommandText = sSQL;
                _sqlCmd.Connection = _objSqlConn;
                _objSqlConn.Open();
                foreach (SqlParameter p in _objSqlParamtter)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                foreach (SqlParameter p in _objSqlParamtterWhere)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                SqlDataAdapter da = new SqlDataAdapter(_sqlCmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                _LoiNgoaiLe = ex.Message;
                throw ex;
            }
            finally
            {
                _sqlCmd.Dispose();
                _objSqlParamtter.Clear();
                _objSqlParamtterWhere.Clear();
                if (_objSqlConn.State != ConnectionState.Closed)
                {
                    _objSqlConn.Close();
                    _objSqlConn.Dispose();
                }
            }
        }
        #endregion

        #region "# ExecuteSQLNonQuery #"
        public object ExecuteSQLNonQuery(string sSQL)
        {
            SqlCommand _sqlCmd = new SqlCommand();
            try
            {
                if (_objSqlConnWithTrans == null)
                {
                    _objSqlConn = new SqlConnection(getSQLConnectionString());
                    _sqlCmd.Connection = _objSqlConn;
                    _objSqlConn.Open();
                }
                else
                {
                    _sqlCmd.Connection = _objSqlConnWithTrans;
                    _sqlCmd.Transaction = _objTransaction;
                }
                _sqlCmd.CommandText = sSQL;
                foreach (SqlParameter p in _objSqlParamtter)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                foreach (SqlParameter p in _objSqlParamtterWhere)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                return Convert.ToInt32(_sqlCmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                LoiNgoaiLe = ex.Message;
                throw ex;
            }
            finally
            {
                _sqlCmd.Dispose();
                _objSqlParamtter.Clear();
                _objSqlParamtterWhere.Clear();
                if (_objSqlConnWithTrans == null)
                {
                    if (_objSqlConn.State != ConnectionState.Closed)
                    {
                        _objSqlConn.Close();
                        _objSqlConn.Dispose();
                    }
                }
            }
        }
        #endregion

        #region "# ExecuteSQLScalar #"
        public object ExecuteSQLScalar(string sSQL)
        {
            SqlCommand _sqlCmd = new SqlCommand();
            try
            {
                if (_objSqlConnWithTrans == null)
                {
                    _objSqlConn = new SqlConnection(getSQLConnectionString());
                    _objSqlConn.Open();
                    _sqlCmd.Connection = _objSqlConn;
                }
                else
                {
                    _sqlCmd.Connection = _objSqlConnWithTrans;
                    _sqlCmd.Transaction = _objTransaction;
                }
                _sqlCmd.CommandText = sSQL;
                foreach (SqlParameter p in _objSqlParamtter)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                foreach (SqlParameter p in _objSqlParamtterWhere)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                return _sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LoiNgoaiLe = ex.Message;
                throw ex;
            }
            finally
            {
                _sqlCmd.Dispose();
                _objSqlParamtter.Clear();
                _objSqlParamtterWhere.Clear();
                if (_objSqlConn.State != ConnectionState.Closed)
                {
                    _objSqlConn.Close();
                    _objSqlConn.Dispose();
                }
            }
        }
        #endregion

        #region "# ExecutePrcDataTable #"
        public DataTable ExecutePrcDataTable(string sSQL)
        {
            DataTable dt = new DataTable();
            SqlCommand _sqlCmd = new SqlCommand();
            try
            {
                _objSqlConn = new SqlConnection(getSQLConnectionString());
                _sqlCmd.CommandText = sSQL;
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _sqlCmd.Connection = _objSqlConn;
                _objSqlConn.Open();
                foreach (SqlParameter p in _objSqlParamtter)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                foreach (SqlParameter p in _objSqlParamtterWhere)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                SqlDataAdapter da = new SqlDataAdapter(_sqlCmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                _LoiNgoaiLe = ex.Message;
                throw ex;
            }
            finally
            {
                _sqlCmd.Dispose();
                _objSqlParamtter.Clear();
                _objSqlParamtterWhere.Clear();
                if (_objSqlConn.State != ConnectionState.Closed)
                {
                    _objSqlConn.Close();
                    _objSqlConn.Dispose();
                }
            }
        }
        #endregion

        #region "# ExecutePrcDataSet #"
        public DataSet ExecutePrcDataSet(string sSQL)
        {
            DataSet ds = new DataSet();
            SqlCommand _sqlCmd = new SqlCommand();
            try
            {
                _objSqlConn = new SqlConnection(getSQLConnectionString());
                _sqlCmd.CommandText = sSQL;
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _sqlCmd.Connection = _objSqlConn;
                _objSqlConn.Open();
                foreach (SqlParameter p in _objSqlParamtter)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                foreach (SqlParameter p in _objSqlParamtterWhere)
                {
                    _sqlCmd.Parameters.Add(p);
                }
                SqlDataAdapter da = new SqlDataAdapter(_sqlCmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                _LoiNgoaiLe = ex.Message;
                throw ex;
            }
            finally
            {
                _sqlCmd.Dispose();
                _objSqlParamtter.Clear();
                _objSqlParamtterWhere.Clear();
                if (_objSqlConn.State != ConnectionState.Closed)
                {
                    _objSqlConn.Close();
                    _objSqlConn.Dispose();
                }
            }
        }
        #endregion

        #region "# doInsert #"
        public object doInsert(string TenBang)
        {
            string sql = "SET DATEFORMAT DMY ";
            string str1 = "";
            string str2 = "";
            foreach (SqlParameter p in objSqlParamtter)
            {
                str1 += p.ParameterName.Substring(1) + ", ";
                str2 += p.ParameterName + ", ";
            }
            if (str1.Length > 2)
                str1 = str1.Substring(0, str1.Length - 2);
            if (str2.Length > 2)
                str2 = str2.Substring(0, str2.Length - 2);
            sql += "INSERT INTO [" + TenBang + "](" + str1 + ") VALUES(" + str2 + "); SELECT SCOPE_IDENTITY();";
            return ExecuteSQLScalar(sql);
        }

        public object doInsertWithIdentity(string TenBang)
        {
            string sql = "SET DATEFORMAT DMY SET IDENTITY_INSERT " + TenBang + " ON ";
            string str1 = "";
            string str2 = "";
            foreach (SqlParameter p in objSqlParamtter)
            {
                str1 += p.ParameterName.Substring(1) + ", ";
                str2 += p.ParameterName + ", ";
            }
            if (str1.Length > 2)
                str1 = str1.Substring(0, str1.Length - 2);
            if (str2.Length > 2)
                str2 = str2.Substring(0, str2.Length - 2);
            sql += "INSERT INTO [" + TenBang + "](" + str1 + ") VALUES(" + str2 + ")  SET IDENTITY_INSERT " + TenBang + " OFF ; SELECT SCOPE_IDENTITY();";
            return ExecuteSQLScalar(sql);
        }
        #endregion

        #region "# doUpdate #"
        public object doUpdate(string TenBang, string @where)
        {
            string sql = "SET DATEFORMAT DMY UPDATE [" + TenBang + "] ";
            string str1 = "";
            foreach (SqlParameter p in objSqlParamtter)
            {
                str1 += p.ParameterName.Substring(1) + " = " + p.ParameterName + ", ";
            }
            if (str1.Length > 2)
                str1 = str1.Substring(0, str1.Length - 2);
            sql += "SET " + str1 + " ";
            sql += "WHERE " + @where + " ";
            return ExecuteSQLNonQuery(sql);
        }
        #endregion

        #region "# doDelete #"
        public object doDelete(string TenBang, string @where)
        {
            string sql = "DELETE FROM [" + TenBang + "] WHERE " + @where;
            return ExecuteSQLNonQuery(sql);
        }
        #endregion

        public class ThamSo
        {


            private string _Ten;
            public string Ten
            {
                get { return _Ten; }
                set { _Ten = value; }
            }


            private object _GiaTri;
            public object GiaTri
            {
                get { return _GiaTri; }
                set { _GiaTri = value; }
            }


            public ThamSo(string __Ten, object __GiaTri)
            {
                Ten = __Ten;
                GiaTri = __GiaTri;
            }


        }


    }
}
