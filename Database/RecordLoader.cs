/*
 * ClassName:       RecordLoader
 * Author:          Blade
 * Date Created:    07/29/2009
 */
using System.Data.SqlClient;
using System;
using System.Reflection;
using System.Collections;
using MySql.Data.MySqlClient;

namespace HTB.Database
{
    public class RecordLoader
    {
        /*
         * Loads records from a datareader to into a list
         * 
         * @param SqlDataReader dr  [DataReader to load the data from]
         * @param ArrayList<object> resultsList [List to lists to be loaded]
         * @param ArrayList<string> classNameList  [List of class names to use for the load]
         * @param DbConnection dbConnection  [database connection to free up]
         */
        public static void LoadRecordsFromMultipleResultsets(SqlDataReader dr, ArrayList[] resultsList, Type[] ptypeList, DbConnection dbConnection)
        {
            if (resultsList.Length != ptypeList.Length)
            {
                throw new FieldAccessException("Results-List and ClassName-List must contain the same number of elements [resultsList: " + resultsList.Length + "] [classNameList: " + ptypeList.Length + "]");
            }
            for (int i = 0; i < ptypeList.Length; i++) 
            {
                resultsList[i].Clear();

                LoadRecordsFromDataReader(dr, resultsList[i], ptypeList[i], dbConnection, false);
                if (!dr.NextResult()) // close db if for some reason we have more classes than resultsets
                { 
                    CloseAndDispose(dr, dbConnection);
                    i = ptypeList.Length;
                }   
            }
            CloseAndDispose(dr, dbConnection);
        }
        /*
         * Loads records from a datareader to into a list
         * 
         * @param SqlDataReader dr  [DataReader to load the data from]
         * @param List<object> list [List to load the data into]
         * @param string className  [the name of datatype to use]
         * @param DbConnection dbConnection  [database connection to set free up]
         */
        public static void LoadRecordsFromDataReader(SqlDataReader dr, ArrayList list, Type ptype, DbConnection dbConnection)
        {
            LoadRecordsFromDataReader(dr, list, ptype, dbConnection, true);
        }
        /*
         * Loads records from a datareader to into a list
         * 
         * @param SqlDataReader dr  [DataReader to load the data from]
         * @param List<object> list [List to load the data into]
         * @param string className  [the name of datatype to use]
         * @param DbConnection dbConnection  [database connection to set free up]
         * @param Boolean closeAndDispose  [indicates whether or not to dispose of the connection and free it for future use]
         */
        public static void LoadRecordsFromDataReader(SqlDataReader dr, ArrayList list, Type ptype, DbConnection dbConnection, Boolean closeAndDispose)
        {
            list.Clear();
            while (dr.Read())
            {
                object record = Activator.CreateInstance(ptype);
                PropertyInfo[] fieldInf = ptype.GetProperties();
                #region Set Member Values
                foreach (PropertyInfo t in fieldInf)
                {
                    if (t.MemberType == MemberTypes.Property)
                    {
                        string fieldName = t.Name;
                        object value = null;
                        string dataType = t.PropertyType.Name.ToLower();
                        object[] attrs = t.GetCustomAttributes(typeof(MappingAttribute), false);
                        if (attrs.Length > 0)
                        {
                            var attr = (MappingAttribute)attrs[0];
                            if (attr.FieldName != null && !attr.FieldName.Equals(string.Empty))
                            {
                                fieldName = attr.FieldName;
                            }
                        }
                        try
                        {
                            value = dr[fieldName];
                        }
                        catch
                        {
                            value = null;
                        }
                        if (value != null) {
                            bool valueSet = false;
                            if (attrs != null && attrs.Length > 0)
                            {
                                var attr = (MappingAttribute)attrs[0];
                                if (attr.FieldName != null && !attr.FieldName.Equals(string.Empty))
                                {
                                    fieldName = attr.FieldName;
                                    if (attr.MappingClass != null && !attr.MappingClass.Equals(string.Empty) && 
                                        attr.MappingMethodName != null && !attr.MappingMethodName.Equals(string.Empty))
                                    {
                                        LoadField(t, attr, record, value);
                                        valueSet = true;
                                    }
                                }
                            }
                            if (!valueSet)
                            {
                                // easy debug
//                                if(fieldName.ToUpper() == "AKTINTTIMESTAMP")
//                                {
//                                    int STOP_HERE = 0;
//                                }
                                if(dataType.Equals("int64"))
                                {
                                    t.SetValue(record, DbUtil.FixLong(value), null);
                                }
                                else if (dataType.StartsWith("int")) // use startswith to catch different types of int
                                {
                                    t.SetValue(record, DbUtil.FixInt(value), null);
                                }
                                else if (dataType.Equals("string"))
                                {
                                    t.SetValue(record, DbUtil.FixString(value), null);
                                }
                                else if (dataType.Equals("decimal"))
                                {
                                    t.SetValue(record, DbUtil.FixDecimal(value), null);
                                }
                                else if (dataType.Equals("float") || dataType.Equals("single") || dataType.Equals("double"))
                                {
                                    t.SetValue(record, DbUtil.FixDouble(value), null);
                                }
                                else if (dataType.StartsWith("date"))
                                {
                                    t.SetValue(record, DbUtil.FixDate(value), null);
                                }
                                else if (dataType.StartsWith("bool"))
                                {
                                    t.SetValue(record, DbUtil.FixBoolean(value), null);
                                }
                            }
                        }
                    }
                }

                #endregion
                list.Add(record);
            }
            if (closeAndDispose)
            {
                CloseAndDispose(dr, dbConnection);
            }
        }


        #region MySql
        /*
         * Loads records from a (MySql) datareader to into a list
         * 
         * @param MySqlSqlDataReader dr  [DataReader to load the data from]
         * @param ArrayList<object> resultsList [List to lists to be loaded]
         * @param ArrayList<string> classNameList  [List of class names to use for the load]
         * @param DbConnection dbConnection  [database connection to free up]
         */
        public static void LoadRecordsFromMultipleMySqlResultsets(MySqlDataReader dr, ArrayList[] resultsList, String[] classNameList, DbConnection dbConnection)
        {
            if (resultsList.Length != classNameList.Length)
            {
                throw new FieldAccessException("ResultsList and ClassName List must contain the same number of elements [resultsList: " + resultsList.Length + "] [classNameList: " + classNameList.Length + "]");
            }
            for (int i = 0; i < classNameList.Length; i++)
            {
                resultsList[i].Clear();

                LoadRecordsFromMySqlDataReader(dr, resultsList[i], classNameList[i], dbConnection, false);
                if (!dr.NextResult()) // close db if for some reason we have more classes than resultsets
                {
                    CloseAndDispose(dr, dbConnection);
                    i = classNameList.Length;
                }
            }
            CloseAndDispose(dr, dbConnection);
        }
        /*
         * Loads records from a (MySql) datareader to into a list
         * 
         * @param MySqlSqlDataReader dr  [DataReader to load the data from]
         * @param List<object> list [List to load the data into]
         * @param string className  [the name of datatype to use]
         * @param DbConnection dbConnection  [database connection to set free up]
         */
        public static void LoadRecordsFromMySqlDataReader(MySqlDataReader dr, ArrayList list, string className, DbConnection dbConnection)
        {
            LoadRecordsFromMySqlDataReader(dr, list, className, dbConnection, true);
        }
        /*
         * Loads records from a (MySql) datareader to into a list
         * 
         * @param MySqlSqlDataReader dr  [DataReader to load the data from]
         * @param List<object> list [List to load the data into]
         * @param string className  [the name of datatype to use]
         * @param DbConnection dbConnection  [database connection to set free up]
         * @param Boolean closeAndDispose  [indicates whether or not to dispose of the connection and free it for future use]
         */
        public static void LoadRecordsFromMySqlDataReader(MySqlDataReader dr, ArrayList list, string className, DbConnection dbConnection, Boolean closeAndDispose)
        {
            list.Clear();
            while (dr.Read())
            {
                object record = Activator.CreateInstance(Type.GetType(className));
                Type recordType = record.GetType();
                PropertyInfo[] fieldInf = recordType.GetProperties();

                #region Set Member Values
                for (int i = 0; i < fieldInf.Length; i++)
                {
                    if (fieldInf[i].MemberType == MemberTypes.Property)
                    {
                        string fieldName = fieldInf[i].Name;
                        object value = null;
                        try
                        {
                            value = dr[fieldName];
                        }
                        catch
                        {
                            value = null;
                        }
                        if (value != null)
                        {
                            string dataType = fieldInf[i].PropertyType.Name.ToLower();
                            object[] attrs = fieldInf[i].GetCustomAttributes(typeof(MappingAttribute), false);
                            bool valueSet = false;
                            if (attrs != null && attrs.Length > 0)
                            {
                                MappingAttribute attr = (MappingAttribute)attrs[0];
                                if (attr.FieldName != null && !attr.FieldName.Equals(string.Empty))
                                {
                                    fieldName = attr.FieldName;
                                    if (attr.MappingClass != null && !attr.MappingClass.Equals(string.Empty) &&
                                        attr.MappingMethodName != null && !attr.MappingMethodName.Equals(string.Empty))
                                    {
                                        LoadField(fieldInf[i], attr, record, value);
                                        valueSet = true;
                                    }
                                }
                            }
                            if (!valueSet)
                            {
                                if (dataType.StartsWith("int")) // use startswith to catch different types of int
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixInt(value), null);
                                }
                                else if (dataType.Equals("string"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixString(value), null);
                                }
                                else if (dataType.Equals("decimal"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixDecimal(value), null);
                                }
                                else if (dataType.Equals("float") || dataType.Equals("single") || dataType.Equals("double"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixFloat(value), null);
                                }
                                else if (dataType.StartsWith("date"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixDate(value), null);
                                }
                                else if (dataType.StartsWith("bool"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixBoolean(value), null);
                                }
                            }
                        }
                    }
                }
                #endregion
                list.Add(record);
            }
            if (closeAndDispose)
            {
                CloseAndDispose(dr, dbConnection);
            }

        }
        #endregion

        
        /*
         * Set Field value to a field (name - value as parameters)
         */
        public static void SetFieldValueByName(object record, string name, object value)
        {
           
                Type recordType = record.GetType();
                PropertyInfo[] fieldInf = recordType.GetProperties();

                #region Set Member Values
                for (int i = 0; i < fieldInf.Length; i++)
                {
                    if (fieldInf[i].MemberType == MemberTypes.Property)
                    {
                        string dataType = fieldInf[i].PropertyType.Name.ToLower();
                        string fieldName = fieldInf[i].Name;
                        if (fieldName.Equals(name))
                        {
                            object[] attrs = fieldInf[i].GetCustomAttributes(typeof(MappingAttribute), false);
                            bool valueSet = false;
                            if (attrs != null && attrs.Length > 0)
                            {
                                MappingAttribute attr = (MappingAttribute)attrs[0];
                                if (attr.FieldName != null && !attr.FieldName.Equals(string.Empty))
                                {
                                    fieldName = attr.FieldName;
                                    if (attr.MappingClass != null && !attr.MappingClass.Equals(string.Empty) &&
                                        attr.MappingMethodName != null && !attr.MappingMethodName.Equals(string.Empty))
                                    {
                                        LoadField(fieldInf[i], attr, record, value);
                                        valueSet = true;
                                    }
                                }
                            }
                            if (!valueSet)
                            {
                                if (dataType.StartsWith("int")) // use startswith to catch different types of int
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixInt(value), null);
                                }
                                else if (dataType.Equals("string"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixString(value), null);
                                }
                                else if (dataType.Equals("decimal"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixDecimal(value), null);
                                }
                                else if (dataType.Equals("float"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixFloat(value), null);
                                }
                                else if (dataType.StartsWith("date"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixDate(value), null);
                                }
                                else if (dataType.StartsWith("bool"))
                                {
                                    fieldInf[i].SetValue(record, DbUtil.FixBoolean(value), null);
                                }
                            }
                        }
                    }
                }
                #endregion
            }

        private static void LoadField(PropertyInfo fieldInf, MappingAttribute attr, object record, object value) 
        {
            object obj = Activator.CreateInstance(Type.GetType(attr.MappingClass));
            MethodInfo[] methods = obj.GetType().GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].Name.Equals(attr.MappingMethodName))
                {
                    ParameterInfo[] parameters = methods[i].GetParameters();
                    /*
                     *      make sure the right method exists
                     *      even though it is tempting (and maybe better... for debuging?!) 
                     *      to just invoke it and let the caller handle it
                     */ 

                    if (parameters != null && parameters.Length == 1)
                    {
                        if (parameters[0].ParameterType.FullName.ToLower().Equals("system.object"))
                        {
                            fieldInf.SetValue(record, methods[i].Invoke(obj, new object[] { value }), null);
                            i = methods.Length; // break the loop
                        }
                    }
                }
            }
        }

        /*
         * Close and Dispose the datareader and set free up the connection 
         */
        private static void CloseAndDispose(SqlDataReader dr, DbConnection dbConnection) 
        {
            dr.Close();
            dr.Dispose();
            if (dbConnection != null)
                dbConnection.IsInUse = false;
        }

        /*
         * Close and Dispose the datareader and set free up the connection 
         */
        private static void CloseAndDispose(MySqlDataReader dr, DbConnection dbConnection)
        {
            dr.Close();
            dr.Dispose();
            if (dbConnection != null)
                dbConnection.IsInUse = false;
        }
    }
}