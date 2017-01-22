/*
 * ClassName:       RecordBuilder
 * Author:          Blade
 * Date Created:    08/12/2008
 */
using System;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace RecordBuilder
{
    class RecordBuilder
    {
        /*
         * Table Name For Which To Build The Record Structure
         */
        string[] tableName = new string[] { "tblCountry" };

        string path = @"C:\Source\csharp\htb\Database\HTB";
        //string path = @"badpath";//  a fail-safe feature so that we don't run this by mistake
        //string path = @"t:\tst\bbitri\adv\include";
        string namespc = "HTB.Database";
        int tabCount = 0;
        //string connString = @"Data Source=DrivenAcc2;Initial Catalog=Mas500_App;User Id=sa;Password=Meineke100;";
        //string connString = @"Data Source=ares\PROD;Initial Catalog=MEP_Production;User Id=sa;Password=Meineke99;";
        string connString = @"Data Source=localhost\deuexpress;Initial Catalog=HTB;User Id=ecp;Password=ecp;";
        
        /*if all three bools below are set to false, it will use the connection string above*/
        bool isMEP = false;
        bool isRhea = false;
        bool isCM = false;

        bool isCSharp = true;
        bool isCPlus = false;

        TextWriter fout;
        
        public RecordBuilder()
        {
            if (isCSharp)
            {
                foreach (string tname in tableName)
                {
                    OpenFile(tname);
                    GenerateCSharp(tname);
                    CloseFile();
                }
            }
            else if (isCPlus)
            {
                foreach (string tname in tableName)
                {
                    OpenFile(tname);
                    GenerateCPlus(tname);
                    CloseFile();
                }
            }
        }
        
        #region CPlus
        public void GenerateCPlus(string tableName)
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
            TableDefList list = new TableDefList();
            if (isMEP)
                list.LoadMepTableDef(tableName);
            else if (isRhea)
                list.LoadRheaTableDef(tableName);
            else if (isCM)
                list.LoadCmTableDef(tableName);
            else
                list.LoadGenericTableDef(connString, tableName);

            foreach (TableDef def in list.DefinitionList)
            {
                WriteLine(GetCPlusDatatype(def) + "  " + def.Column + ";");
            }
            tabCount--;
            WriteLine("};");
            WriteLine();
            WriteCPlusRecordset(list);
        }

        private string GetCPlusDatatype(TableDef def)
        {
            switch (def.Datatype.ToLower())
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
            return def.Datatype;
        }
        private void WriteCPlusRecordset(TableDefList list) 
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
        public void GenerateCSharp(string tblName) {

            tabCount = 0;
            WriteCommentLineStrart("ClassName:\t\t" + tblName);
            WriteCommentLine("Author:\t\t\tGenerated Code");
            WriteCommentLine("Date Created:\t" + System.DateTime.Now.ToShortDateString());
            WriteCommentLine("Description:\t\t" + "Represents a row in the "+tblName+" table");
            WriteCommentLineEnd();
            WriteLine("using System.Data.SqlClient;");
            WriteLine("using System.Data;");
            WriteLine("using System;");
            WriteLine("namespace " + namespc);
            WriteLine("{");
            tabCount++;
            WriteLine("public class " + tblName+" : Record");
            WriteLine("{");
            tabCount++;
            WriteLine("#region Property Declaration");
            TableDefList list = new TableDefList();
            if (isMEP)
                list.LoadMepTableDef(tblName);
            else if (isRhea)
                list.LoadRheaTableDef(tblName);
            else if (isCM)
                list.LoadCmTableDef(tblName);
            else
                list.LoadGenericTableDef(connString, tblName);
            /*
            foreach (TableDef def in list.DefinitionList)
            {
                WriteLine("private " + GetCSharpDatatype(def) + " " + GetPrivateFieldName(def.Column) + ";");
            }
             */
            foreach (TableDef def in list.DefinitionList)
            {
                /*
                WriteLine("public " + GetCSharpDatatype(def) + " " + def.Column);
                WriteLine("{");
                tabCount++;
                WriteLine("get { return " + GetPrivateFieldName(def.Column) + "; }");
                WriteLine("set { " + GetPrivateFieldName(def.Column) + " = value; }");
                tabCount--;
                WriteLine("}");
                 */
                WriteLine("public " + GetCSharpDatatype(def) + " " + def.Column + "{get; set; }");
            } 
            WriteLine("#endregion");
            tabCount--;
            WriteLine("}");
            tabCount--;
            WriteLine("}");
            
            //StoredProcedureBuilder spb = new StoredProcedureBuilder(getSqlConnection(), tableName, "MEP", list);
            //spb.createSaveSP();
        }
        private string GetCSharpDatatype(TableDef def)
        {
            switch (def.Datatype.ToLower())
            {
                case "nvarchar":
                case "varchar":
                case "text":
                case "nchar":
                case "char":
                case "ntext":
                    return "string";
                case "datetime":
                    return "DateTime";
                case "bit":
                    return "bool";
                case "tinyint":
                case "bigint":
                case "smallint":
                    return "int";
                case "money":
                case "numeric":
                    return "decimal";
                case "float":
                case "real":
                    return "double";

            }
            return def.Datatype;
        }
        #endregion

        private SqlConnection getSqlConnection()
        {
            if (isRhea) return RheaDatabase.Instance.Connection;
            else return HTBDatabase.Instance.Connection;
        }
        
        private string GetPrivateFieldName(String columnName)
        {
            StringBuilder wbuffer = new StringBuilder("_");
            wbuffer.Append(columnName.Substring(0, 1).ToLower());
            wbuffer.Append(columnName.Substring(1));
            return wbuffer.ToString();
        }
        
        private void OpenFile(string tblName)
        {
            if (isCSharp)
                fout = new StreamWriter(path + "\\" + tblName + ".cs");
            else if (isCPlus)
                fout = new StreamWriter(path + "\\" + tblName + ".h");
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
            new RecordBuilder();
        }
    }
}
