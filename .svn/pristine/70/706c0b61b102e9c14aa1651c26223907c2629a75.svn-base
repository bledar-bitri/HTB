/*
 * ClassName:       RecordBuilder
 * Author:          Blade
 * Date Created:    08/12/2008
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace RecordBuilder
{
    class MySqlRecordBuilder
    {
        /*
         * Table Name For Which To Build The Record Structure
         */
        string tableName = "AlertDailyKpi";

        string path = @"C:\maaco\EMailService\Database\Polaris";
        string namespc = "DrivenBrands.Database.Polaris";
        int tabCount = 0;
        string connString = @"server=localhost;user=root;database=polaris;port=4006;password=Driven1;";
        
        bool smartLowerCase = false;
        bool isCSharp = true;
        bool isCPlus = false;

        TextWriter fout;
        
        public MySqlRecordBuilder()
        {
            OpenFile();
            if (isCSharp)
                GenerateCSharp();
            else if (isCPlus)
                GenerateCPlus();
            CloseFile();
        }
        
        #region CPlus
        public void GenerateCPlus()
        {

            tabCount = 0;
            WriteCommentLineStrart("ClassName:\t\tM" + tableName+"Rcd");
            WriteCommentLine("Author:\t\t\tGenerated Code");
            WriteCommentLine("Date Created:\t" + System.DateTime.Now.ToShortDateString());
            WriteCommentLine("Description:\t\t" + "Represents a row in the " + tableName + " table");
            WriteCommentLineEnd();
            WriteLine();
            WriteLine("#ifndef _"+tableName.ToUpper()+"_H");
            WriteLine("#define _"+tableName.ToUpper()+"_H");
            WriteLine();
            WriteLine("#include <MRecordset.h>");
            WriteLine();
            WriteLine();
            WriteLine("class M" + tableName + "Rcd");
            WriteLine("{");
            WriteLine();
            WriteLine("public:");
            tabCount++;
            MySqlTableDefList list = new MySqlTableDefList();
            list.LoadGenericTableDef(connString, tableName);

            foreach (MySqlTableDef def in list.DefinitionList)
            {
                WriteLine(GetCPlusDatatype(def) + "  " + def.Column + ";");
            }
            tabCount--;
            WriteLine("};");
            WriteLine();
            WriteCPlusRecordset(list);
        }

        private string GetCPlusDatatype(MySqlTableDef def)
        {
            throw new NotImplementedException ("Don't have time for C++ now");
            /*switch (def.Datatype.ToLower())
            {
                case "nvarchar":
                case "varchar":
                case "text":
                case "nchar":
                case "char":
                case "ntext":
                    return "MVString" + def.Length;
                case "datetime":
                    return "MDate";
                case "bit":
                    return "MVString1";
                case "tinyint":
                    return "MDecimal4";
                case "smallint":
                    return "MDecimal6";
                case "int":
                    return "MDecimal9";
                case "money":
                case "numeric":
                case "decimal":
                    return "MDecimal" + def.XPrecision + (def.XScale == 0 ? "" : "_" + def.XScale);
            }
            return def.Datatype;*/
            //return"";
        }
        private void WriteCPlusRecordset(MySqlTableDefList list) 
        {
            WriteLine("class M"+tableName+"Set  :  public MRecordset");
            WriteLine("{");
            WriteLine("private:");
            tabCount++;
            WriteLine("M"+tableName+"Rcd  "+tableName+"RcdWrk;");
            WriteLine();
            tabCount--;
            WriteLine("public:");
            tabCount++;

            WriteLine("M"+tableName+"Set ();");
            WriteLine("void InitRcd (M"+tableName+"Rcd&);");
            WriteLine("void MoveFirstRcd (M"+tableName+"Rcd&);");
            WriteLine("void MoveNextRcd (M"+tableName+"Rcd&);");
            WriteLine("void SelectKey (M"+tableName+"Rcd&, short = -1);");
            WriteLine("BOOL InsertRcd (M"+tableName+"Rcd&);");
            WriteLine("BOOL UpdateRcd (M"+tableName+"Rcd&);");
            WriteLine("BOOL DeleteRcd (M"+tableName+"Rcd&, short = -1);");
            tabCount--;
            WriteLine("};");

            WriteLine();
            
            WriteLine("void inline");
            WriteLine("M"+tableName+"Set::InitRcd (M"+tableName+"Rcd& passed_rcd)");
            WriteLine("{");
            tabCount++;
            WriteLine(tableName+"RcdWrk = passed_rcd;");
            WriteLine("InitDbRcd ();");
            WriteLine("passed_rcd = "+tableName+"RcdWrk;");
            WriteLine("return;");
            tabCount--;
            WriteLine("}");

            WriteLine();

            WriteLine("void inline");
            WriteLine("M"+tableName+"Set::MoveFirstRcd (M"+tableName+"Rcd& passed_rcd)");
            WriteLine("{");
            tabCount++;
            WriteLine(tableName+"RcdWrk = passed_rcd;");
            WriteLine("MoveFirst ();");
            WriteLine("passed_rcd = "+tableName+"RcdWrk;");
            WriteLine("return;");
            tabCount--;
            WriteLine("}");

            WriteLine();

            WriteLine("void inline");
            WriteLine("M"+tableName+"Set::MoveNextRcd (M"+tableName+"Rcd& passed_rcd)");
            WriteLine("{");
            tabCount++;
            WriteLine(tableName+"RcdWrk = passed_rcd;");
            WriteLine("MoveNext ();");
            WriteLine("passed_rcd = "+tableName+"RcdWrk;");
            WriteLine("return;");
            tabCount--;
            WriteLine("}");

            WriteLine();

            WriteLine("void inline");
            WriteLine("M"+tableName+"Set::SelectKey (M"+tableName+"Rcd& passed_rcd, short num_keys)");
            WriteLine("{");
            tabCount++;
            WriteLine(tableName+"RcdWrk = passed_rcd;");
            WriteLine("SelectDbKey (num_keys);");
            WriteLine("return;");
            tabCount--;
            WriteLine("}");
            
            WriteLine();

            WriteLine("BOOL inline");
            WriteLine("M"+tableName+"Set::InsertRcd (M"+tableName+"Rcd& passed_rcd)");
            WriteLine("{");
            tabCount++;
            WriteLine(tableName+"RcdWrk = passed_rcd;");
            WriteLine("return (InsertDbRcd ());");
            tabCount--;
            WriteLine("}");

            WriteLine();

            WriteLine("BOOL inline");
            WriteLine("M"+tableName+"Set::UpdateRcd (M"+tableName+"Rcd& passed_rcd)");
            WriteLine("{");
            tabCount++;
            WriteLine(tableName+"RcdWrk = passed_rcd;");
            WriteLine("return (UpdateDbRcd ());");
            tabCount--;
            WriteLine("}");

            WriteLine();

            WriteLine("BOOL inline");
            WriteLine("M"+tableName+"Set::DeleteRcd (M"+tableName+"Rcd& passed_rcd, short num_keys)"); 
            WriteLine("{");
            tabCount++;
            WriteLine(tableName+"RcdWrk = passed_rcd;");
            WriteLine("return (DeleteDbRcd (num_keys));");
            tabCount--;
            WriteLine("}");
            
            WriteLine();
            
            WriteLine("inline");
            WriteLine("M"+tableName+"Set::M"+tableName+"Set ()");
            WriteLine("{");
            tabCount++;
            WriteLine("DefTable (\""+tableName+"\");");
            foreach (TableDef def in list.DefinitionList)
            {
                WriteLine("DefCol (\""+ def.Column+"\",  "+tableName+"RcdWrk."+ def.Column+");");
            }
            WriteLine();
            WriteLine();
            WriteLine("/* Define Primary Key */");
            WriteLine("DefKey(\"" + tableName + "1\",  \"" + tableName + "1\");");
            WriteLine();
            WriteLine("/* Add Columns That Make Up The Primary Key */");
            WriteLine("DefKeyCol(\"" + tableName + "1\",  \"\");");
            WriteLine();
            WriteLine("/* If The Primary Key Contains More Than One Column */");
            WriteLine("//DefKeyCol(\"" + tableName + "1\",  \"\");");
            tabCount--;
            

            WriteLine("}");
            WriteLine("#endif");
        }
        
        #endregion


        #region CSharp
        public void GenerateCSharp() {

            tabCount = 0;
            WriteCommentLineStrart("ClassName:\t\t" + tableName);
            WriteCommentLine("Author:\t\t\tGenerated Code");
            WriteCommentLine("Date Created:\t" + System.DateTime.Now.ToShortDateString());
            WriteCommentLine("Description:\t\t" + "Represents a row in the "+tableName+" table");
            WriteCommentLineEnd();
            WriteLine("using System.Data.SqlClient;");
            WriteLine("using System.Data;");
            WriteLine("using System;");
            WriteLine("namespace " + namespc);
            WriteLine("{");
            tabCount++;
            WriteLine("public class " + tableName);
            WriteLine("{");
            tabCount++;
            WriteLine("#region Property Declaration");
            MySqlTableDefList list = new MySqlTableDefList();
            list.LoadGenericTableDef(connString, tableName);

            foreach (MySqlTableDef def in list.DefinitionList)
            {
                WriteLine("private " + GetCSharpDatatype(def) + " " + GetPrivateFieldName(def.Column) + ";");
            }
            foreach (MySqlTableDef def in list.DefinitionList)
            {
                WriteLine("public " + GetCSharpDatatype(def) + " " + GetPropertyName(def.Column));
                WriteLine("{");
                tabCount++;
                WriteLine("get { return " + GetPrivateFieldName(def.Column) + "; }");
                WriteLine("set { " + GetPrivateFieldName(def.Column) + " = value; }");
                tabCount--;
                WriteLine("}");
            } 
            WriteLine("#endregion");
            tabCount--;
            WriteLine("}");
            tabCount--;
            WriteLine("}");
        }
        private string GetCSharpDatatype(MySqlTableDef def)
        {
            string[] wtype = def.Datatype.Split('(');

            switch (wtype[0].ToLower())
            {
                case "nvarchar":
                case "varchar":
                case "text":
                case "nchar":
                case "char":
                case "ntext":
                case "tinytext":
                    return "string";
                case "datetime":
                    return "DateTime";
                case "bit":
                    return "bool";
                case "smallint":
                case "int":
                case "tinyint":
                    return "int";
                case "money":
                case "numeric":
                case "decimal":
                    return "decimal";
                case "float":
                    return "double";

            }
            return def.Datatype;
        }
        #endregion

        
        private string GetPrivateFieldName(String columnName)
        {
            StringBuilder wbuffer = new StringBuilder("_");
            if (smartLowerCase && (columnName.Length == 6 || columnName.Length == 9))
            {
                String wname = columnName.ToLower();
                wbuffer.Append(wname.Substring(0, 3));
                wbuffer.Append(wname.Substring(3,1).ToUpper());
                wbuffer.Append(wname.Substring(4, 2));
                if (columnName.Length == 9)
                {
                    wbuffer.Append(wname.Substring(6, 1).ToUpper());
                    wbuffer.Append(wname.Substring(7));
                }
            }
            else
            {
                wbuffer.Append(columnName.Substring(0, 1).ToLower());
                wbuffer.Append(columnName.Substring(1));
            }
            return wbuffer.ToString();
        }

        private string GetPropertyName(String columnName)
        {
            StringBuilder wbuffer = new StringBuilder();
            if (smartLowerCase && (columnName.Length == 6 || columnName.Length == 9))
            {
                String wname = columnName.ToLower();
                wbuffer.Append(wname.Substring(0, 1).ToUpper());
                wbuffer.Append(wname.Substring(1, 2));
                wbuffer.Append(wname.Substring(3, 1).ToUpper());
                wbuffer.Append(wname.Substring(4, 2));
                if (columnName.Length == 9)
                {
                    wbuffer.Append(wname.Substring(6, 1).ToUpper());
                    wbuffer.Append(wname.Substring(7));
                }

            }
            else
            {
                wbuffer.Append(columnName);
            }
            return wbuffer.ToString();
        }


        private void OpenFile()
        {
            if (isCSharp)
                fout = new StreamWriter(path + "\\" + tableName + ".cs");
            else if (isCPlus)
                fout = new StreamWriter(path + "\\" + tableName + ".h");
        }
        private void CloseFile()
        {
            fout.Flush();
            fout.Close();
            fout.Dispose();
        }
        private void WriteLine()
        {
            WriteLine("");
        }

        private void WriteLine(String pstr)
        {

            StringBuilder wbuffer = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
            {
                wbuffer.Append("\t");
            }
            wbuffer.Append(pstr);
            fout.WriteLine(wbuffer.ToString());
        }

        private void WriteCommentLineStrart(String pstr)
        {
            WriteLine("/*");
        }
        private void WriteCommentLine(String pstr)
        {
            WriteLine(" * " + pstr);
        }
        private void WriteCommentLineEnd()
        {
            WriteLine("*/");
        }

        static void Main(string[] args)
        {
            new MySqlRecordBuilder();
        }
    }
}
