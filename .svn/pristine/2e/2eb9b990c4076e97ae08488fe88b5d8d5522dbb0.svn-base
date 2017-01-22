/*
 * ClassName:       RecordSaver
 * Author:          Blade
 * Date Created:    07/29/2008
 */
using System.Data.SqlClient;
using System;
using System.Reflection;
using System.Collections;

namespace RecordBuilder
{
    public class RecordSaver
    {
        /*
         * Saves record (object) into a database
         */

        public static void SaveRecord(SqlCommand command, object record)
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
                        command.Parameters.Add(new SqlParameter("@"+fieldInf[i].Name, fieldInf[i].GetValue(record, null)));
                    }
                }
                #endregion
                command.ExecuteNonQuery();
        }

    }
}
