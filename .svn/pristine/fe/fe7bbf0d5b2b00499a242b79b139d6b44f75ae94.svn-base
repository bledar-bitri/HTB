/*
 * ClassName:       StoredProcedureBuilder
 * Author:          Blade
 * Date Created:    10/05/2008
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.Sql;

namespace RecordBuilder
{
    class StoredProcedureBuilder
    {
        private SqlConnection connection;
        private String tableName;
        private String dbName;
        private TableDefList list;

        public StoredProcedureBuilder(SqlConnection connection, String tableName, String dbName, TableDefList list)
        {
            this.tableName = tableName;
            this.connection = connection;
            this.dbName = dbName;
            this.list = list;
        }

        public void createSaveSP() {

            PrimaryKeyList primaryKeys = new PrimaryKeyList(tableName);
            primaryKeys.LoadPrimaryKeys(connection);

            Server server = new Server(new ServerConnection(connection));

            // Add the stored procedure to the [dbName] Database
            Database db = server.Databases[dbName];

            // Create a Stored Procedure called [spName] in [dbName]
            StoredProcedure mySP = new StoredProcedure(db, "Save" + tableName);

            mySP.TextMode = false;
            mySP.AnsiNullsStatus = false;
            mySP.QuotedIdentifierStatus = false;

            // Declare Input Parameters
            foreach (TableDef def in list.DefinitionList)
            {
                StoredProcedureParameter idParam = new StoredProcedureParameter(mySP, "@"+def.Column, GetDatatype(def));
                mySP.Parameters.Add(idParam);
            }

            // The SQL Text
            
            StringBuilder wbuffer = new StringBuilder("");
            #region if_exists
            wbuffer.Append("\r\n IF EXISTS (SELECT * FROM ");
            wbuffer.Append(tableName);
            wbuffer.Append(" WHERE ");
            for (int i = 0; i < primaryKeys.KeysList.Count; i++)
            {
                PrimaryKey pk = (PrimaryKey)primaryKeys.KeysList[i];
                wbuffer.Append(pk.ColumnName);
                wbuffer.Append(" = @");
                wbuffer.Append(pk.ColumnName);
                if (i < primaryKeys.KeysList.Count - 1)
                {
                    wbuffer.Append(" AND ");
                }
            }
            #endregion
            #region update
            wbuffer.Append(")\r\n UPDATE ");
            wbuffer.Append(tableName);
            wbuffer.Append(" SET ");
            for (int i = 0; i < list.DefinitionList.Count; i++)
            {
                TableDef def = (TableDef)list.DefinitionList[i];
                if (!isPrimaryKey(primaryKeys, def))
                {
                    wbuffer.Append(def.Column);
                    wbuffer.Append(" = @");
                    wbuffer.Append(def.Column);
                    if (i < list.DefinitionList.Count - 1)
                    {
                        wbuffer.Append(", ");
                    }
                }
            }
            wbuffer.Append(", ");
            if (wbuffer.ToString().EndsWith(", "))
            {
                wbuffer.Remove(wbuffer.ToString().Length - 2, 2);
            }

            wbuffer.Append(" WHERE ");
            for (int i = 0; i < primaryKeys.KeysList.Count; i++)
            {
                PrimaryKey pk = (PrimaryKey)primaryKeys.KeysList[i];
                wbuffer.Append(pk.ColumnName);
                wbuffer.Append(" = @");
                wbuffer.Append(pk.ColumnName);
                if (i < primaryKeys.KeysList.Count - 1)
                {
                    wbuffer.Append(" AND ");
                }
            }
            #endregion
            wbuffer.Append("\r\n ELSE ");
            #region insert
            wbuffer.Append("\r\n INSERT INTO ");
            for (int i = 0; i  < list.DefinitionList.Count; i++)
            {
                TableDef def = (TableDef)list.DefinitionList[i];
                if (i == 0)
                {
                    wbuffer.Append("[");
                    wbuffer.Append(def.Table);
                    wbuffer.Append("] (");
                }
                wbuffer.Append(def.Column);
                if (i < list.DefinitionList.Count - 1)
                {
                    wbuffer.Append(", ");
                }
            }
            wbuffer.Append(") VALUES (");
            for (int i = 0; i < list.DefinitionList.Count; i++)
            {
                TableDef def = (TableDef)list.DefinitionList[i];
                wbuffer.Append("@"+def.Column);
                if (i < list.DefinitionList.Count - 1)
                {
                    wbuffer.Append(", ");
                }
            }
            wbuffer.Append(")");
            #endregion

            mySP.TextBody = wbuffer.ToString();

            // Create the stored procedure in the database
            try
            {
                //mySP.Drop();
            }
            catch{}
            Console.Write(mySP.TextBody);
            mySP.Create();

        }

        private bool isPrimaryKey(PrimaryKeyList primaryKeys, TableDef def)
        {
            
            for (int i = 0; i < primaryKeys.KeysList.Count; i++)
            {
                PrimaryKey pk = (PrimaryKey)primaryKeys.KeysList[i];
                if (pk.ColumnName.Equals(def.Column))
                {
                    return true;
                }
            }
            return false;
        }
        private DataType GetDatatype(TableDef def)
        {
            switch (def.Datatype.ToLower())
            {
                case "nvarchar":
                    return DataType.NVarChar(def.Precision);
                case "varchar":
                    return DataType.VarChar(def.Precision);
                case "text":
                    return DataType.Text;
                case "nchar":
                    return DataType.NChar(def.Precision);
                case "char":
                    return DataType.Char(def.Precision);
                case "ntext":
                    return DataType.NText;
                case "datetime":
                    return DataType.DateTime;
                case "bit":
                    return DataType.Bit;
                case "tinyint":
                    return DataType.TinyInt;
                case "int":
                    return DataType.Int;
                case "float":
                    return DataType.Float;
                case "money":
                    return DataType.Money;
                case "numeric":
                    return DataType.Numeric(def.XScale, def.XPrecision);
                default:
                    throw new Exception("Datatype not found");
            }
        }
    }
}
