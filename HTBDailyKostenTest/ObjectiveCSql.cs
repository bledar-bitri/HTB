using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HTBUtilities;

namespace HTBDailyKosten
{
    public class ObjectiveCSql
    {
        static string dir = @"C:\temp\ObjC2Sql";
        
        public static void GenerateSql()
        {
            foreach (string f in Directory.GetFiles(dir))
            {
                if (f.EndsWith(".h"))
                {
                    Console.WriteLine(f);
                    CreateObjCSqlCode(f);
                }
            }
        }

        static void CreateObjCSqlCode(string fileName)
        {
            
            string text = HTBUtils.GetFileText(fileName);
            string[] lines = text.Split('\n');
            int idx = 0;
           
            using (var file = new StreamWriter(fileName + ".txt"))
            {
                var vars = new List<ObjCVar>();
                foreach (string line in lines)
                {
                    idx++;
                    if (line.StartsWith("@property"))
                    {
                        string l = line.Replace("*", "").Replace(";", "");
                        idx = l.IndexOf(")");
                        if (idx > 0)
                        {
                            l = l.Substring(idx+1);
                            l = Regex.Replace(l, @"^\s+", ""); // trim
                            var v = new ObjCVar(l);
                            if (v.IsOK)
                            {
                                //file.WriteLine("[" + var.DataType + "]  [" + var.Name + "]");
                                vars.Add(v);
                            }
                            else
                            {
                                file.WriteLine("Skipped: ["+l+"]");
                            }
                        }
                    }
                }

                file.WriteLine(GetCreate(vars));
                file.WriteLine(GetSelect(vars));
                file.WriteLine(GetInsert(vars));
                file.WriteLine(GetUpdate(vars));
                file.WriteLine(GetLoad(vars));
                file.WriteLine(GetDescription(vars));
            }
        }

        static string GetCreate(List<ObjCVar> vars)
        {

            var sb = new StringBuilder();
            
            sb.Append("CREATE TABLE IF NOT EXISTS ... (");

            for (int i = 0; i < vars.Count; i++)
            {
                sb.Append("\"");
                sb.Append(vars[i].GetSqliteNameAndType());
                sb.Append(",\"");

                sb.Append(Environment.NewLine);
            }

            sb.Append("\" ) \"");
            return sb.ToString();
        }

        static string GetInsert(List<ObjCVar> vars)
        {
            var sb = new StringBuilder("- (NSString*) sqlInsert");

            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("NSMutableString *sb = [[NSMutableString alloc]initWithString:@\"INSERT INTO \"];");
            sb.Append(Environment.NewLine);
            sb.Append("[sb appendFormat:@\" %@ (\", tableName];");
            sb.Append(Environment.NewLine);
            
            
            for (int i = 0; i < vars.Count; i++)
            {
                string coma = ",";
                if (i >= vars.Count - 1)
                    coma = "";
                sb.Append("[sb appendString:@\"" + vars[i].SqlName + coma + "\"];");
                sb.Append(Environment.NewLine);
            }

            sb.Append("[sb appendString:@\" ) VALUES (\"];");
            sb.Append(Environment.NewLine);

            for (int i = 0; i < vars.Count; i++)
            {
                string comma = ",";
                if (i >= vars.Count - 1)
                    comma = ")";

                sb.Append("[sb appendFormat:@\"" + vars[i].GetObjCFormat() + comma + " \", self." + vars[i].Name + "];");
                sb.Append(Environment.NewLine);
            }
            sb.Append("return sb;");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        static string GetUpdate(List<ObjCVar> vars)
        {
            var sb = new StringBuilder("- (NSString*) sqlUpdate");

            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("NSMutableString *sb = [[NSMutableString alloc]initWithFormat:@\"UPDATE %@ SET \", tableName];");
            sb.Append(Environment.NewLine);
            
            
            for (int i = 0; i < vars.Count; i++)
            {
                string coma = ",";
                if (i >= vars.Count - 1)
                    coma = "";

//                sb.Append("[sb appendFormat:@\"" + vars[i].SqlName + " = " + coma + "\"];");
                sb.Append("[sb appendFormat:@\"");
                sb.Append(vars[i].SqlName);
                sb.Append(" = ");
                sb.Append(vars[i].GetObjCFormat());
                sb.Append(coma);
                sb.Append(" \", self.");
                sb.Append(vars[i].Name);
                sb.Append("];");
                sb.Append(Environment.NewLine);
            }

            sb.Append("return sb;");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        static string GetSelect(List<ObjCVar> vars)
        {
            var sb = new StringBuilder("- (NSString*) sqlSelect");

            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("NSMutableString *sb = [[NSMutableString alloc]initWithString:@\"SELECT \"];");
            sb.Append(Environment.NewLine);
            sb.Append("[sb appendString:@");

            for (int i = 0; i < vars.Count; i++)
            {
                string coma = ",";
                if (i >= vars.Count - 1)
                    coma = "";
                sb.Append("\"" + vars[i].SqlName + coma + "\"");
                sb.Append(Environment.NewLine);
            }
            sb.Append("];");
            sb.Append(Environment.NewLine);
            sb.Append("[sb appendFormat:@\" FROM %@ \", tableName];");
            sb.Append(Environment.NewLine);

            sb.Append("return sb;");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        static string GetLoad(List<ObjCVar> vars)
        {
            var sb = new StringBuilder("- (BOOL) loadFromStatementRow:(sqlite3_stmt*)statement");

            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("NSInteger i = 0;");
            sb.Append(Environment.NewLine);
            
            for (int i = 0; i < vars.Count; i++)
            {
                sb.Append(vars[i].GetLoadLine());
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);

            sb.Append("return YES;");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        static string GetDescription(List<ObjCVar> vars)
        {
            var sb = new StringBuilder("- (NSString*) description");

            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("NSMutableString *sb = [[NSMutableString alloc]initWithFormat:@\"\\n %@ \", tableName];");
            sb.Append(Environment.NewLine);


            for (int i = 0; i < vars.Count; i++)
            {
                sb.Append("[sb appendFormat:@\"\\n\\t");
                sb.Append(vars[i].SqlName);
                sb.Append(" = ");
                sb.Append(vars[i].GetObjCFormat());
                sb.Append(" \", self.");
                sb.Append(vars[i].Name);
                sb.Append("];");
                sb.Append(Environment.NewLine);
            }

            sb.Append("return sb;");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

    }

    internal class ObjCVar
    {
        public readonly string DataType;
        public readonly string Name;
        public readonly string SqlName;
        public bool IsOK { get; set; }

        public ObjCVar(string var)
        {
            IsOK = false;
            var = var.Replace("    ", " ");
            var = var.Replace("   ", " ");
            var = var.Replace("  ", " ");
            string[] arr = var.TrimStart().TrimEnd().Split(' ');
            if (arr.Length != 2)
            {
                arr = var.TrimStart().TrimEnd().Split('\t');
            }
            if(arr.Length == 2)
            {
                DataType = arr[0].Trim();
                Name = arr[1].Trim();
                SqlName = Name.Substring(0, 1).ToUpper() + Name.Substring(1);
                IsOK = true;
            }
        }
        public ObjCVar(string varName, string dtype)
        {
            Name = varName;
            DataType = dtype;
            SqlName = Name.Substring(0, 1).ToUpper() + Name.Substring(1);
        }

        public string GetSqliteNameAndType()
        {
            return SqlName + " " + GetSqlDatatype();
        }

        private string GetSqlDatatype()
        {
            switch (DataType.ToLower())
            {
                case "nsinteger":
                case "int":
                case "bool":
                    return "INTEGER";
                case "double":
                    return "REAL";
                case "nsstring":
                case "nsdate":
                    return "TEXT";
            }
            return "???";
        }

        public string GetObjCFormat(bool includeQuote = true)
         {
             switch (DataType.ToLower())
             {
                 case "nsinteger":
                 case "int":
                 case "bool":
                     return "%d";
                 case "double":
                     return "%f";
                 case "nsstring":
                 case "nsdate":
                     return includeQuote ? "'%@'" : "%@";
             }
             return "???";
         }

        public string GetLoadLine()
        {
            var brakets = "]";
            var sb = new StringBuilder("[self set");
            sb.Append(Name.Substring(0, 1).ToUpper() + Name.Substring(1));
            sb.Append(":");
                switch (DataType.ToLower())
            {
                case "nsinteger":
                case "int":
                case "bool":
                    sb.Append("sqlite3_column_int");
                    break;
                case "double":
                    sb.Append("sqlite3_column_double");
                    break;
                case "nsstring":
                case "nsdate":
                    sb.Append("[NSString stringWithUTF8String:(const char *)sqlite3_column_text");
                    brakets = "]]";
                    break;
            }
            sb.Append("(statement, i++)");
            sb.Append(brakets);
            sb.Append(";");
            return sb.ToString();
        }
    }
}
