/*
 * ClassName:       RecordLoader
 * Author:          Blade
 * Date Created:    07/29/2008
 */
using System.Data.SqlClient;
using System;
using System.Reflection;
using System.Collections;
using MySql.Data.MySqlClient;

namespace RecordBuilder
{
    public class RecordLoader
    {
        /*
         * Loads records from a datareader to into a list
         * 
         * @param SqlDataReader dr  [DataReader to load the data from]
         * @param List<object> list [List to load the data into]
         * @param string className  [the name of datatype to use]
         */
        public static void LoadRecordsFromDataReader(SqlDataReader dr, ArrayList list, string className)
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
                        string dataType = fieldInf[i].PropertyType.Name.ToLower();
                        string fieldName = fieldInf[i].Name;
                        object[] attrs = fieldInf[i].GetCustomAttributes(typeof(MappingAttribute), false);
                        bool valueSet = false;
                        if (attrs != null && attrs.Length > 0)
                        {
                            MappingAttribute attr = (MappingAttribute)attrs[0];
                            if (attr.fieldName != null && !attr.fieldName.Equals(string.Empty))
                            {
                                fieldName = attr.fieldName;
                                if (attr.MappingClass != null && !attr.MappingClass.Equals(string.Empty) && 
                                    attr.MappingMethodName != null && !attr.MappingMethodName.Equals(string.Empty))
                                {
                                    LoadField(fieldInf[i], attr, record, dr[fieldName]);
                                    valueSet = true;
                                }
                            }
                        }
                        if (!valueSet)
                        {
                            if (dataType.StartsWith("int")) // use startswith to catch different types of int
                            {
                                fieldInf[i].SetValue(record, Util.FixInt(dr[fieldName]), null);
                            }
                            else if (dataType.Equals("string"))
                            {
                                fieldInf[i].SetValue(record, Util.FixString(dr[fieldName]), null);
                            }
                            else if (dataType.Equals("decimal"))
                            {
                                fieldInf[i].SetValue(record, Util.FixDecimal(dr[fieldName]), null);
                            }
                            else if (dataType.Equals("float"))
                            {
                                fieldInf[i].SetValue(record, Util.FixFloat(dr[fieldName]), null);
                            }
                            else if (dataType.StartsWith("date"))
                            {
                                fieldInf[i].SetValue(record, Util.FixDate(dr[fieldName]), null);
                            }
                            else if (dataType.StartsWith("bool"))
                            {
                                fieldInf[i].SetValue(record, Util.FixBoolean(dr[fieldName]), null);
                            }
                        }
                    }
                }
                #endregion
                list.Add(record);
            }
            dr.Close();
            dr.Dispose();
        }

        /*
         * Loads records from a datareader to into a list
         * 
         * @param SqlDataReader dr  [DataReader to load the data from]
         * @param List<object> list [List to load the data into]
         * @param string className  [the name of datatype to use]
         */
        public static void LoadRecordsFromMySqlDataReader(MySqlDataReader dr, ArrayList list, string className)
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
                        string dataType = fieldInf[i].PropertyType.Name.ToLower();
                        string fieldName = fieldInf[i].Name;
                        object[] attrs = fieldInf[i].GetCustomAttributes(typeof(MappingAttribute), false);
                        bool valueSet = false;
                        if (attrs != null && attrs.Length > 0)
                        {
                            MappingAttribute attr = (MappingAttribute)attrs[0];
                            if (attr.fieldName != null && !attr.fieldName.Equals(string.Empty))
                            {
                                fieldName = attr.fieldName;
                                if (attr.MappingClass != null && !attr.MappingClass.Equals(string.Empty) &&
                                    attr.MappingMethodName != null && !attr.MappingMethodName.Equals(string.Empty))
                                {
                                    LoadField(fieldInf[i], attr, record, dr[fieldName]);
                                    valueSet = true;
                                }
                            }
                        }
                        if (!valueSet)
                        {
                            if (dataType.StartsWith("int")) // use startswith to catch different types of int
                            {
                                fieldInf[i].SetValue(record, Util.FixInt(dr[fieldName]), null);
                            }
                            else if (dataType.Equals("string"))
                            {
                                fieldInf[i].SetValue(record, Util.FixString(dr[fieldName]), null);
                            }
                            else if (dataType.Equals("decimal"))
                            {
                                fieldInf[i].SetValue(record, Util.FixDecimal(dr[fieldName]), null);
                            }
                            else if (dataType.Equals("float"))
                            {
                                fieldInf[i].SetValue(record, Util.FixFloat(dr[fieldName]), null);
                            }
                            else if (dataType.StartsWith("date"))
                            {
                                fieldInf[i].SetValue(record, Util.FixDate(dr[fieldName]), null);
                            }
                            else if (dataType.StartsWith("bool"))
                            {
                                fieldInf[i].SetValue(record, Util.FixBoolean(dr[fieldName]), null);
                            }
                        }
                    }
                }
                #endregion
                list.Add(record);
            }
            dr.Close();
            dr.Dispose();
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
                     * make sure the right method exists
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
    }
}