using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Transactions;

namespace HTB.Database
{
    public class RecordSet
    {
        public const string DefaultDateString = "1900-01-01";
        public const string DateFormat = "yyyyMMdd HH:mm:ss";

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected DbConnection Con;
        readonly CommittableTransaction _transaction = new CommittableTransaction();
        public readonly int ConnectionType = DbConnection.ConnectionType_SqlServer;
       

        #region Property Declaration
        private ArrayList _lsttblMahnung = new ArrayList();
        public ArrayList RecordsList
        {
            get { return _lsttblMahnung; }
            set { _lsttblMahnung = value; }
        }
        #endregion

        public RecordSet(int connType = DbConnection.ConnectionType_SqlServer)
        {
            ConnectionType = connType;
        }

        #region Load
        public void LoadRecords(ArrayList list, string psqlCommand, Type recordName)
        {
            LoadListFromDataReader(list, GetDataReader(psqlCommand, false), recordName);
        }

        public void LoadRecords(string psqlCommand, Type recordName)
        {
            LoadListFromDataReader(RecordsList, GetDataReader(psqlCommand, false), recordName);
        }
        #endregion

        public void TestDbFieldNames()
        {

            var rec = new tblMahnung();
            rec.MahnungID = 1;
            rec.MahnungDate = DateTime.Now;
            rec.MahnungNr = 123123;
            Console.WriteLine(GetDbInsertStatement(rec));
            InsertRecord(rec);
            rec.MahnungAnrede = "111111";
            UpdateRecord(rec);
            DeleteRecord(rec, "MahnungNr = 123123");
            Console.WriteLine("DONE");
        }

        #region Write
        #region insert
        public bool InsertRecord(Record record)
        {
            return ExecuteNonQuery(GetDbInsertStatement(record)) >= 0;
        }
        public bool InsertRecordInTransaction(Record record)
        {
            return ExecSqlInTransaction(GetDbInsertStatement(record));
        }
        #endregion
        #region update
        public bool UpdateRecord(Record record)
        {
            return UpdateRecord(record, null);
        }
        public bool UpdateRecord(Record record, String where)
        {
            return ExecuteNonQuery(GetDbUpdateStatement(record, where)) >= 0;
        }
        public bool UpdateRecordInTransaction(Record record) 
        {
            return UpdateRecordInTransaction(record, null);
        }
        public bool UpdateRecordInTransaction(Record record, String where)
        {
            return ExecSqlInTransaction(GetDbUpdateStatement(record, where));
        }
        #endregion
        #region delete
        public bool DeleteRecord(Record record)
        {
            return DeleteRecord(record, null);
        }
        public bool DeleteRecord(Record record, String where)
        {
            return ExecuteNonQuery(GetDbDeleteStatement(record, where)) >= 0;
        }
        public bool DeleteRecordInTransaction(Record record)
        {
            return DeleteRecordInTransaction(record, null);
        }
        public bool DeleteRecordInTransaction(Record record, String where)
        {
            return ExecSqlInTransaction(GetDbDeleteStatement(record, where));
        }
        #endregion

        private bool ExecSqlInTransaction(String sql)
        {
            try
            {
                Con.Connection.EnlistTransaction(_transaction);
                var cmd = new SqlCommand(sql, Con.Connection) {CommandType = CommandType.Text};
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                _transaction.Rollback();
                return false;
            }
        }
        
        public string GetDbInsertStatement(Record record)
        {
            var sb = new StringBuilder("INSERT INTO ");
            if (record.TableName != null && !record.TableName.Trim().Equals(String.Empty))
            {
                sb.Append(record.TableName);
            }
            else
            {
                sb.Append(record.GetType().Name);
            }
            sb.Append(" ( ");
            sb.Append(GetDbFieldNamesOrValues(record, true, false));
            sb.Append(" ) VALUES ( ");
            sb.Append(GetDbFieldNamesOrValues(record, false, false));
            sb.Append(")");
            Log.DebugFormat("SQL: {0}", sb);
            return sb.ToString();
        }

        public string GetDbUpdateStatement(Record record)
        {
            return GetDbUpdateStatement(record, null);
        }

        public string GetDbUpdateStatement(Record record, string where)
        {
            var sb = new StringBuilder("UPDATE ");
            if (record.TableName != null && !record.TableName.Trim().Equals(String.Empty))
            {
                sb.Append(record.TableName);
            }
            else
            {
                sb.Append(record.GetType().Name);
            }
            sb.Append(" SET ");
            sb.Append(GetDbFieldNameAndValuePairs(record, false, false));
            sb.Append(" WHERE ");
            sb.Append(where ?? GetDbFieldNameAndValuePairs(record, true, true, " = ", " AND "));
            Log.DebugFormat("SQL: {0}", sb);
            return sb.ToString();
        }

        private string GetDbFieldNamesOrValues(Record record, bool isNames, bool addAutoNumberFields)
        {

            var sb = new StringBuilder();

            Type recordType = record.GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();

            #region Set Member Values
            foreach (PropertyInfo t in fieldInf)
            {
                if (t.MemberType == MemberTypes.Property)
                {
                    object value;
                    string format = null;
                    try
                    {
                        value = t.GetValue(record, null);
                    }
                    catch
                    {
                        value = null;
                    }
                    var dataType = t.PropertyType.Name.ToLower();
                    var attrs = t.GetCustomAttributes(typeof(MappingAttribute), false);
                    bool done = false;
                    if (attrs.Length > 0)
                    {
                        var attr = (MappingAttribute)attrs[0];
                        format = attr.FieldFormat;
                        if (!addAutoNumberFields && attr.FieldAutoNumber)
                        {
                            done = true;
                        }
                        else
                        {
                            if (isNames && attr.FieldName != null && !attr.FieldName.Equals(string.Empty) && !attr.FieldType.Equals(MappingAttribute.NO_DB_SAVE))
                            {
                                // set the name based on the attribute
                                sb.Append(attr.FieldName);
                                sb.Append(", ");
                                done = true;
                            }
                            if (attr.FieldType.Equals(MappingAttribute.NO_DB_SAVE))
                            {
                                // skip value if attribute is set not to save to database
                                done = true;
                            }
                        }
                    }
                    if (!done)
                    {
                        if (isNames)
                        {
                            sb.Append(t.Name);
                        }
                        else
                        {
                            AppendValueForDB(sb, dataType, value, format);
                        }
                        sb.Append(", ");
                    }
                }
            }

            #endregion
            // remove last (,) comma from the string
            if (sb.Length > 2)
                sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }
        
        private string GetDbFieldNameAndValuePairs(Record record, bool isIdOnly, bool addAutoNumberFields)
        {
            return GetDbFieldNameAndValuePairs(record, isIdOnly, addAutoNumberFields, " = ", ", ");
        }
        private string GetDbFieldNameAndValuePairs(Record record, bool isIdOnly, bool addAutoNumberFields, string delim, string pairDelim)
        {

            StringBuilder sb = new StringBuilder();

            Type recordType = record.GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();

            #region Set Member Values
            for (int i = 0; i < fieldInf.Length; i++)
            {
                if (fieldInf[i].MemberType == MemberTypes.Property)
                {
                    string fieldName = fieldInf[i].Name;
                    object value = null;
                    string format = null;
                    try
                    {
                        value = fieldInf[i].GetValue(record, null);
                    }
                    catch
                    {
                        value = null;
                    }
                    if (value != null)
                    {
                        string dataType = fieldInf[i].PropertyType.Name.ToLower();
                        object[] attrs = fieldInf[i].GetCustomAttributes(typeof(MappingAttribute), false);
                        bool done = false;
                        if (attrs != null && attrs.Length > 0)
                        {
                            MappingAttribute attr = (MappingAttribute)attrs[0];
                            format = attr.FieldFormat;
                            if (isIdOnly && !attr.FieldType.Equals(MappingAttribute.FIELD_TYPE_ID))
                            {
                                done = true;
                            }
                            else
                            {
                                if (!addAutoNumberFields && attr.FieldAutoNumber)
                                {
                                    done = true;
                                }
                                else
                                {
                                    if (attr.FieldName != null && !attr.FieldName.Equals(string.Empty) && !attr.FieldType.Equals(MappingAttribute.NO_DB_SAVE))
                                    {
                                        // set the name based on the attribute
                                        sb.Append(attr.FieldName);
                                        sb.Append(delim);
                                        AppendValueForDB(sb, dataType, value, format);
                                        sb.Append(pairDelim);
                                        done = true;
                                    }
                                    if (attr.FieldType.Equals(MappingAttribute.NO_DB_SAVE))
                                    {
                                        // skip value if attribute is set not to save to database
                                        done = true;
                                    }
                                }
                            }
                        }
                        else if (isIdOnly)
                        {
                            // skip fields that don't have atributes when getting only ID fields
                            done = true;
                        }
                        if (!done)
                        {
                            sb.Append(fieldInf[i].Name);
                            sb.Append(delim);
                            AppendValueForDB(sb, dataType, value, format);
                            sb.Append(pairDelim);
                        }
                    }
                }
            }
            #endregion
            // remove last (pairDelim) from the string
            if (sb.Length > pairDelim.Length)
                sb.Remove(sb.Length - pairDelim.Length, pairDelim.Length);

            return sb.ToString();
        }

        private void AppendValueForDB(StringBuilder sb, string dataType, object value, string format)
        {

            if (dataType.StartsWith("int") || dataType.Equals("decimal") ||
                dataType.Equals("float") || dataType.Equals("single") || dataType.Equals("double"))
            {
                if (format != null && format.Equals(MappingAttribute.FIELD_FORMAT_CURRENCY))
                {
                    sb.Append(Math.Round((double)value, 2).ToString().Replace(",", "."));
                }
                else
                {
                    sb.Append(value.ToString().Replace(",", "."));
                }
            }
            else if (dataType.Equals("string"))
            {
                sb.Append("'");
                if (value != null)
                    sb.Append(value.ToString().Replace("'", "''"));
                sb.Append("'");
            }
            else if (dataType.Equals("datetime"))
            {
                sb.Append("'");
                sb.Append(value != null ? GetDBDateTimeString((DateTime) value) : DefaultDateString);
                sb.Append("'");
            }
            else if (dataType.StartsWith("date"))
            {
                sb.Append("'");
                if (value != null)
                    sb.Append(value);
                else
                    sb.Append(DefaultDateString);
                sb.Append("'");
            }
            else if (dataType.StartsWith("bool"))
            {
                sb.Append(DbUtil.GetBoolValueForDB(value));
            }
        }

        private string GetDBDateTimeString(DateTime value)
        {
            if (value == DateTime.MinValue || value.ToShortDateString() == "01.01.0001")
                return GetDbDefaultDateString();
            
            return value.ToString(DateFormat);
        }
        private static string GetDbDefaultDateString()
        {
            return DefaultDateString;
        }

        #endregion

        #region Delete
        private string GetDbDeleteStatement(Record record)
        {
            return GetDbDeleteStatement(record, null);
        }
        private string GetDbDeleteStatement(Record record, string where)
        {
            var sb = new StringBuilder("DELETE ");
            if (record.TableName != null && !record.TableName.Trim().Equals(String.Empty))
            {
                sb.Append(record.TableName);
            }
            else
            {
                sb.Append(record.GetType().Name.ToString());
            }
            sb.Append(" WHERE ");
            if (where == null)
            {
                sb.Append(GetDbFieldNameAndValuePairs(record, true, true, " = ", " AND "));
            }
            else
            {
                sb.Append(where);
            }
            sb.Append(" ");
            return sb.ToString();
        }
        #endregion

        #region DB Access
        
        protected SqlDataReader GetDataReader(string psqlCommand, bool debugMode = false)
        {
            return GetDataReader(psqlCommand, DatabasePool.ConnectToHTB, debugMode);
        }

        protected SqlDataReader GetDataReader(string psqlCommand, int connectToDatabase = DatabasePool.ConnectToHTB, bool debugMode = false)
        {
            SqlDataReader _Results;
            LogIfDebug(debugMode, "Getting DB Connection");
            Con = DatabasePool.GetConnection(psqlCommand, connectToDatabase, debugMode);
            LogIfDebug(debugMode, "Creating DB Command");
            var cmd = new SqlCommand(psqlCommand, Con.Connection);
            LogIfDebug(debugMode, "Calling Excecute Reader");
            _Results = cmd.ExecuteReader();
            LogIfDebug(debugMode, "Returning Results");
            return _Results;
        }
        
        private void LoadListFromDataReader(ArrayList list, SqlDataReader dr, Type ptype)
        {
            RecordsList.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                list,
                ptype, Con);
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ExecuteNonQuery(string sqlCommand, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            return SqlServerExecuteNonQuery(sqlCommand, connectToDatabase);
        }

        public void ExecuteStoredProcedure(string spName, ArrayList parameters, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            Con = DatabasePool.GetConnection(spName, connectToDatabase);
            var cmd = new SqlCommand(spName, Con.Connection) { CommandType = CommandType.StoredProcedure };
            bool containsOutput = false;
            if (parameters != null)
            {
                foreach (StoredProcedureParameter p in parameters)
                {
                    var sqlParam = new SqlParameter(p.Name, p.Value) { Direction = p.Direction };
                    cmd.Parameters.Add(sqlParam);

                    if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.ReturnValue)
                        containsOutput = true;
                }
            }
            cmd.ExecuteNonQuery();
            if (containsOutput)
            {
                var returnValues = new ArrayList();
                foreach (StoredProcedureParameter p in parameters)
                {
                    if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.ReturnValue)
                    {
                        returnValues.Add(new StoredProcedureParameter(p.Name, p.SqlType, cmd.Parameters[p.Name].Value));
                    }
                }
                parameters.Add(returnValues);
            }
            Con.IsInUse = false;
        }
        
        private int SqlServerExecuteNonQuery(string psqlCommand, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            try
            {
                Con = DatabasePool.GetConnection(psqlCommand, connectToDatabase);
                var cmd = new SqlCommand(psqlCommand, Con.Connection);
                int ret = cmd.ExecuteNonQuery();
                Con.IsInUse = false;
                return ret;
            }
            catch (Exception e)
            {
                //TODO: do something with the exception
                throw new Exception($"Error while executing sql command: <br/>{psqlCommand}", e);
            }
        }
        
        public void ExecuteNonQueryInTransaction(string sqlCommand)
        {
            ExecSqlInTransaction(sqlCommand);
        }

        public void StartTransaction(int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            Con = DatabasePool.GetConnection("", connectToDatabase);
            Con.Connection.EnlistTransaction(_transaction);
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            Con.NeedsToBeDestroyed = true;
            try
            {
                Thread.Sleep(100);
            }
            catch
            {
            }
        }
        public void RollbackTransaction()
        {
            _transaction.Rollback();
            Con.NeedsToBeDestroyed = true;
            try
            {
                Thread.Sleep(100);
            }
            catch
            {
            }
        }

        #endregion

        public static bool Delete(Record rec)
        {
            return new RecordSet().DeleteRecord(rec);
        }
        public static bool Update(Record rec)
        {
            return new RecordSet().UpdateRecord(rec);
        }
        public static bool Insert(Record rec)
        {
            return new RecordSet().InsertRecord(rec);
        }

        protected void LogIfDebug(bool debug, string message)
        {
            if (debug)
            {
                Log.Info(message);
                Thread.Sleep(100);
            }
        }
    }
}
