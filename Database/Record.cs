using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Data;

namespace HTB.Database
{
    public class Record
    {
        private static int xmlTabs;

        #region Property Declaration
        private string _tableName;
        [MappingAttribute(FieldType = MappingAttribute.NO_DB_SAVE)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        #endregion

        public void Assign(Record rec)
        {
            Type recordType = GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();
            for (int i = 0; i < fieldInf.Length; i++)
            {
                if (fieldInf[i].MemberType == MemberTypes.Property && rec.HasField(fieldInf[i].Name))
                {
                    GetType().GetProperty(fieldInf[i].Name).SetValue(this, rec.GetPropertyValue(fieldInf[i].Name), null);
                }
            }
        }
        
        #region XML
        public string ToXmlString(bool addXmlHeader = false, bool encodeXML = false)
        {

            var sb = new StringBuilder();
            if (addXmlHeader)
                sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\r");
            
            Type recordType = GetType();
            sb.Append(GetStartXmlTag(recordType.Name));
            PropertyInfo[] fieldInf = recordType.GetProperties();
            
            foreach (PropertyInfo propInfo in fieldInf)
            {
                if (propInfo.MemberType == MemberTypes.Property)
                {
                    if (propInfo.PropertyType.Name.ToLower().Trim().IndexOf("arraylist") >= 0)
                    {
                        var list = (ArrayList)propInfo.GetValue(this, null);
                        foreach (Record rec in list)
                        {
                            sb.Append(rec.ToXmlString());
                        }
                    }
                    else if (propInfo.PropertyType.BaseType != null && propInfo.PropertyType.BaseType.Name.ToLower().Trim().Equals("record"))
                    {
                        var rec = (Record)propInfo.GetValue(this, null);
                        sb.Append(rec.ToXmlString());
                    }
                    else if(propInfo.Name.ToLower() != "tablename")
                    {
                        sb.Append(GetXmlLine(propInfo.Name, propInfo.GetValue(this, null)));
                    }
                }
            }
            sb.Append(GetEndXmlTag(recordType.Name));
            return sb.ToString();
        }
        private string GetStartXmlTag(string name)
        {
            StringBuilder sb = new StringBuilder();
            AddTabs(sb);
            sb.Append("<");
            sb.Append(name);
            sb.Append(">\r\n");
            xmlTabs++;
            return sb.ToString();
        }
        private string GetEndXmlTag(string name)
        {
            xmlTabs--;
            var sb = new StringBuilder();
            AddTabs(sb);
            sb.Append("</");
            sb.Append(name);
            sb.Append(">\r\n");
            return sb.ToString();
        }
        private string GetXmlLine(string name, object val, bool encodeXML = false)
        {
            var sb = new StringBuilder();
            AddTabs(sb);
            sb.Append("<");
            sb.Append(name);
            sb.Append(">");
            if(val == null)
                sb.Append("");
            else if(val is DateTime)
                sb.Append(encodeXML ? System.Web.HttpUtility.HtmlEncode(((DateTime)val).ToString("yyyy-MM-dd").Trim()) : ((DateTime)val).ToString("yyyy-MM-dd").Trim());
            else
                sb.Append(encodeXML ? System.Web.HttpUtility.HtmlEncode(val.ToString().Trim()) : val.ToString().Trim());
            sb.Append("</");
            sb.Append(name);
            sb.Append(">\r\n");
            return sb.ToString();
        }
        private void AddTabs(StringBuilder sb)
        {
            for (int i = 0; i < xmlTabs; i++)
            {
                sb.Append("\t");
            }
        }
        #endregion

        #region TAB
        public string ToTabString()
        {
            var sb = new StringBuilder();
            Type recordType = GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();
            foreach (PropertyInfo propInfo in fieldInf)
            {
                if (propInfo.MemberType == MemberTypes.Property)
                {
                    if (propInfo.PropertyType.Name.ToLower().Trim().IndexOf("arraylist") < 0)
                    {
                        sb.Append(GetTabValue(propInfo.GetValue(this, null)));
                    }
                }
            }
            return sb.ToString();
        }
        private string GetTabValue(object val)
        {
            var sb = new StringBuilder();
            sb.Append(val == null ? "" : val.ToString().Trim());
            sb.Append("\t");
            return sb.ToString();
        }
        #endregion

        #region Objective-C
        public static string GetObjectiveCString(Type recordType)
        {
            var sb = new StringBuilder();
            var declaration = new StringBuilder();
            var property = new StringBuilder();
            var synthesize = new StringBuilder();
            var init = new StringBuilder();
            var setnil = new StringBuilder();
            var release = new StringBuilder();

            var sqlSelect = new StringBuilder();
            var sqlInsert = new StringBuilder();
            var sqlUpdate = new StringBuilder();

            var toXml = new StringBuilder("-(NSString *) toXmlString \n{\n\tNSMutableString *retString = [[[NSMutableString alloc]init] autorelease];\n\n");
            toXml.Append("[retString appendString:@\"<");
            toXml.Append(recordType.Name);
            toXml.Append(">\"];\n\n");
            PropertyInfo[] fieldInf = recordType.GetProperties();
            foreach (PropertyInfo propInfo in fieldInf)
            {
                if (propInfo.MemberType == MemberTypes.Property)
                {
                    string datatype = propInfo.PropertyType.Name.ToLower();
                    string objCtype = "NSString";
                    string defaultValue = "@\"\"";
                    bool addPointer = true;
                    string varName = propInfo.Name.Substring(0, 1).ToLower() + propInfo.Name.Substring(1);
                    if(datatype.StartsWith("int"))
                    {
                        objCtype = "NSInteger";
                        addPointer = false;
                        defaultValue = "0";
                    }
                    else if(datatype == "decimal" || datatype == "double")
                    {
                        objCtype = "double";
                        addPointer = false;
                        defaultValue = "0";
                    }
                    else if(datatype == "boolean")
                    {
                        objCtype = "BOOL";
                        addPointer = false;
                        defaultValue = "false";
                    }
                    else if(datatype == "datetime")
                    {
                        objCtype = "NSDate";
                        defaultValue = "[Util getDateFromString:@\"1.1.1900\"]";
                    }
                    
                    /********** declaration **********/
                    declaration.Append(objCtype);
                    declaration.Append(addPointer ? " *" : " ");
                    declaration.Append(varName);
                    declaration.Append(";\n");

                    /********** property **********/
                    property.Append("@property (nonatomic");
                    if(addPointer)
                        property.Append(", retain");
                    property.Append(") ");
                    property.Append(objCtype);
                    property.Append(addPointer ? " *" : " ");
                    property.Append(varName);
                    property.Append(";\n");

                    /********** synthesize **********/
                    synthesize.Append("@synthesize ");
                    synthesize.Append(varName);
                    synthesize.Append("=_");
                    synthesize.Append(varName);
                    synthesize.Append(";\n");

                    /********** init **********/
                    if(defaultValue != "XXX")
                    {
                        init.Append("[self set");
                        init.Append(propInfo.Name); // starts with uppercase 
                        init.Append(":");
                        init.Append(defaultValue);
                        init.Append("];\n");
                    }

                    if (addPointer)
                    {
                        /********** setnil **********/
                        setnil.Append("[self set");
                        setnil.Append(propInfo.Name); // starts with uppercase 
                        setnil.Append(":nil];\n");
                        
                        
                        /********** release **********/
                        release.Append("[_");
                        release.Append(varName);
                        release.Append(" release];\n");
                        
                    }


                    /********** toXml **********/
                    toXml.Append("[retString appendString:@\"<");
                    toXml.Append(propInfo.Name);
                    toXml.Append(">\"];\n");
                    
                    if(datatype.StartsWith("int"))
                    {
                        toXml.Append("[retString appendString:[NSString stringWithFormat:@\"%i\",  [self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");
                    }
                    else if(datatype == "decimal" || datatype == "double")
                    {
                        toXml.Append("[retString appendString:[Util formatCurrencyNumber:[self ");
                        toXml.Append(varName);
                        toXml.Append("] replaceDotsWithCommas:YES]];");
                    }
                    else if(datatype == "boolean")
                    {
                        toXml.Append("[retString appendString:[NSString stringWithFormat:@\"%i\",  [self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");
                    }
                    else if(datatype == "datetime")
                    {
                        toXml.Append("[retString appendString:[Util formatDate:[self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");
                    }
                    else
                    {
                        toXml.Append("[retString appendString:[self ");
                        toXml.Append(varName);
                        toXml.Append("]];");
                    }
                    toXml.Append("\n[retString appendString:@\"</");
                    toXml.Append(propInfo.Name);
                    toXml.Append(">\"];\n\n");

                    /********** sqlSelect **********/
                    sqlSelect.Append("[retString appendString:@\"<");
                    sqlSelect.Append(propInfo.Name);
                    sqlSelect.Append(">\"];\n");

                    if (datatype.StartsWith("int"))
                    {
                        toXml.Append("[retString appendString:[NSString stringWithFormat:@\"%i\",  [self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");
                    }
                    else if (datatype == "decimal" || datatype == "double")
                    {
                        toXml.Append("[retString appendString:[Util formatCurrencyNumber:[self ");
                        toXml.Append(varName);
                        toXml.Append("] replaceDotsWithCommas:YES]];");
                    }
                    else if (datatype == "boolean")
                    {
                        toXml.Append("[retString appendString:[NSString stringWithFormat:@\"%i\",  [self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");
                    }
                    else if (datatype == "datetime")
                    {
                        toXml.Append("[retString appendString:[Util formatDate:[self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");
                    }
                    else
                    {
                        toXml.Append("[retString appendString:[self ");
                        toXml.Append(varName);
                        toXml.Append("]];");
                    }
                    toXml.Append("\n[retString appendString:@\"</");
                    toXml.Append(propInfo.Name);
                    toXml.Append(">\"];\n\n");
                }
            }

            toXml.Append("[retString appendString:@\"</");
            toXml.Append(recordType.Name);
            toXml.Append(">\"];\n\nreturn retString;\n}\n");

            const string newLines = "\n\n\n";
            sb.Append("/************** Declaration *********************/\n");
            sb.Append(declaration);
            sb.Append(newLines);
            sb.Append("/************** Property *********************/\n");
            sb.Append(property);
            sb.Append(newLines);
            sb.Append("/************** Synthesize *********************/\n");
            sb.Append(synthesize);
            sb.Append(newLines);
            sb.Append("/************** Init *********************/\n");
            sb.Append(init);
            sb.Append(newLines);
            sb.Append("/************** Release *********************/\n");
            sb.Append(release);
            sb.Append(newLines);
            sb.Append("/************** Setnil *********************/\n");
//            sb.Append(setnil);
            sb.Append(newLines);
            sb.Append("/************** TO XML *********************/\n");
            sb.Append(toXml);
            return sb.ToString();
        }
        public static ArrayList GetObjectiveCFiles(Type recordType)
        {
            var list = new ArrayList();
            var sbH = new StringBuilder();
            var sbM = new StringBuilder();

            var declaration = new StringBuilder();
            var property = new StringBuilder();
            var synthesize = new StringBuilder();
            var init = new StringBuilder();
            var setnil = new StringBuilder();
            var release = new StringBuilder();
            var select = new StringBuilder("- (NSString*) sqlSelect\n{\n NSMutableString *sb = [[NSMutableString alloc] initWithString:@\"SELECT \"];\n[sb appendString:@");

            var insert = new StringBuilder("- (BOOL) insertIntoDB:(FMDatabase *) db\n{\nreturn [db executeUpdate:@\"INSERT INTO "+ recordType.Name + "( ");
            var insertFieldName = new StringBuilder();
            var insertQuestionMarks = new StringBuilder();
            var insertValues = new StringBuilder();
            var update = new StringBuilder("- (BOOL) updateDB:(FMDatabase *) db\n{\nreturn [db executeUpdate:@\"UPDATE " + recordType.Name + " SET ");
            var updateFieldName = new StringBuilder();
            var updateValues = new StringBuilder();
            var load = new StringBuilder("- (BOOL) loadFromFMResultSetRow:(FMResultSet*)set\n{\n ");
            var description = new StringBuilder("- (NSString*) description\n{\n [[NSMutableString* alloc]initWithFormat:@\"\\n%@\", tableName];\n");

            var toXml = new StringBuilder("-(NSString *) toXmlString \n{\n\tNSMutableString *retString = [[[NSMutableString alloc]init] autorelease];\n\n");
            toXml.Append("\t[retString appendString:@\"<");
            toXml.Append(recordType.Name);
            toXml.Append(">\"];\n\n");
            PropertyInfo[] fieldInf = recordType.GetProperties();
            bool importUtil = false;
            foreach (PropertyInfo propInfo in fieldInf)
            {
                if (propInfo.MemberType == MemberTypes.Property && propInfo.Name != "TableName")
                {
                    string datatype = propInfo.PropertyType.Name.ToLower();
                    string objCtype = "NSString";
                    string defaultValue = "@\"\"";
                    bool addPointer = true;
                    string varName = propInfo.Name.Substring(0, 1).ToLower() + propInfo.Name.Substring(1);
                    if (datatype.StartsWith("int"))
                    {
                        objCtype = "NSInteger";
                        addPointer = false;
                        defaultValue = "0";
                    }
                    else if (datatype == "decimal" || datatype == "double")
                    {
                        objCtype = "double";
                        addPointer = false;
                        defaultValue = "0";
                    }
                    else if (datatype == "boolean")
                    {
                        objCtype = "BOOL";
                        addPointer = false;
                        defaultValue = "false";
                    }
                    else if (datatype == "datetime")
                    {
                        objCtype = "NSDate";
                        defaultValue = "[Util getDateFromString:@\"1.1.1900\"]";
                        importUtil = true;
                    }

                    /********** declaration **********/
                    AppendToString(1, declaration, objCtype);
                    AppendToString(declaration, addPointer ? " *" : " ");
                    AppendToString(declaration, varName);
                    AppendToString(declaration, ";\n");

                    /********** property **********/
                    property.Append("@property (nonatomic");
                    if (addPointer)
                        property.Append(", strong");
                    property.Append(") ");
                    property.Append(objCtype);
                    property.Append(addPointer ? " *" : " ");
                    property.Append(varName);
                    property.Append(";\n");

                    /********** synthesize **********/
                    synthesize.Append("@synthesize ");
                    synthesize.Append(varName);
                    synthesize.Append("=_");
                    synthesize.Append(varName);
                    synthesize.Append(";\n");

                    #region db  operations

                    select.Append("\"");
                    select.Append(propInfo.Name);
                    select.Append(", \"\n");

                    insertFieldName.Append(propInfo.Name);
                    insertFieldName.Append(", ");
                    insertQuestionMarks.Append("?, ");

                    updateFieldName.Append(propInfo.Name);
                    updateFieldName.Append(" = ?, ");
                    #endregion


                    /********** init **********/
                    if (defaultValue != "XXX")
                    {
                        AppendToString(2, init, "[self set");
                        init.Append(propInfo.Name); // starts with uppercase 
                        init.Append(":");
                        init.Append(defaultValue);
                        init.Append("];\n");
                    }

                    if (addPointer)
                    {
                        /********** release **********/
                        release.Append("\t[_");
                        release.Append(varName);
                        release.Append(" release];\n");

                        /********** setnil **********/
                        setnil.Append("\t[self set");
                        setnil.Append(propInfo.Name); // starts with uppercase 
                        setnil.Append(":nil];\n");
                    }


                    /********** toXml **********/
                    toXml.Append("\t[retString appendString:@\"<");
                    toXml.Append(propInfo.Name);
                    toXml.Append(">\"];\n");
                    var descriptionFirstPart = "[sb appenFormat:@\"\\n\\t" + propInfo.Name + " = ";
                    var descriptionSecondPart = "self."+varName+"];\n";
                    if (datatype.StartsWith("int"))
                    {
                        toXml.Append("\t[retString appendString:[NSString stringWithFormat:@\"%i\",  [self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");

                        var valString = "[NSNumber numberWithLong:self." + varName +"], ";

                        insertValues.Append(valString);
                        updateValues.Append(valString);
                        description.Append(descriptionFirstPart + "%ld \", (long)" + descriptionSecondPart);

                        load.Append("[self set" + propInfo.Name + ":[set intForColumn:@\"" + propInfo.Name + "\"]];\n");
                    }
                    else if (datatype == "decimal" || datatype == "double")
                    {
                        toXml.Append("\t[retString appendString:[Util formatCurrencyNumber:[self ");
                        toXml.Append(varName);
                        toXml.Append("] replaceDotsWithCommas:YES]];");

                        var valString = "[NSNumber numberWithDouble:self."+varName+"], ";
                        insertValues.Append(valString);
                        updateValues.Append(valString);

                        load.Append("[self set" + propInfo.Name + ":[set doubleForColumn:@\"" + propInfo.Name + "\"]];\n");
                        description.Append(descriptionFirstPart + "%lf \", " + descriptionSecondPart);

                        importUtil = true;

                    }
                    else if (datatype == "boolean")
                    {
                        toXml.Append("\t[retString appendString:[NSString stringWithFormat:@\"%i\",  [self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");

                        var valString = "[NSNumber numberWithBool:self." + varName + "], ";
                        insertValues.Append(valString);
                        updateValues.Append(valString);

                        load.Append("[self set" + propInfo.Name + ":[set boolForColumn:@\"" + propInfo.Name + "\"]];\n");
                        description.Append(descriptionFirstPart + "%d \", " + descriptionSecondPart);
                    }
                    else if (datatype == "datetime")
                    {
                        toXml.Append("\t[retString appendString:[Util formatDate:[self ");
                        toXml.Append(varName);
                        toXml.Append("]]];");

                        var valString = varName + ", ";

                        insertValues.Append(valString);
                        updateValues.Append(valString);

                        load.Append("[self set" + propInfo.Name + ":[set dateForColumn:@\"" + propInfo.Name + "\"]];\n");
                        description.Append(descriptionFirstPart + "%@ \", " + descriptionSecondPart);
                        importUtil = true;
                    }
                    else
                    {
                        toXml.Append("\t[retString appendString:[self ");
                        toXml.Append(varName);
                        toXml.Append("]];");

                        var valString = varName + ", ";

                        insertValues.Append(valString);
                        updateValues.Append(valString);

                        load.Append("[self set" + propInfo.Name + ":[set stringForColumn:@\"" + propInfo.Name + "\"]];\n");
                        description.Append(descriptionFirstPart + "%@ \", " + descriptionSecondPart);
                    }
                    toXml.Append("\n\t[retString appendString:@\"</");
                    toXml.Append(propInfo.Name);
                    toXml.Append(">\"];\n\n");
                }
            }

            toXml.Append("\t[retString appendString:@\"</");
            toXml.Append(recordType.Name);
            toXml.Append(">\"];\n\n\treturn retString;\n}\n\n");

            select.Remove(select.ToString().LastIndexOf(','), 2);
            select.Append("];\n[sb appendFormat:@\"FROM @% \", tableName];\nreturn sb;\n}\n\n");

            insert.Append(insertFieldName.ToString().Substring(0, insertFieldName.Length-2));
            insert.Append(") VALUES (");
            insert.Append(insertQuestionMarks.ToString().Substring(0, insertQuestionMarks.Length - 2));
            insert.Append(") \", ");
            insert.Append(insertValues.ToString().Substring(0, insertValues.Length - 2));
            insert.Append("];\n}\n\n");

            update.Append(updateFieldName.ToString().Substring(0, updateFieldName.Length - 2));
            update.Append("\", ");
            update.Append(updateValues.ToString().Substring(0, updateValues.Length - 2));
            update.Append("];\n}\n\n");

            load.Append("\nreturn YES;\n}\n\n");
            description.Append("\nreturn sb;\n};\n\n");
            #region H file
            AppendToString(sbH, "#import \"Record.h\"\n\n");
            AppendToString(sbH, "@interface ");
            AppendToString(sbH, recordType.Name);
            AppendToString(sbH, " : Record\n");
            //AppendToString(sbH, "\n{\n\n");
            //AppendToString(sbH, declaration.ToString());
            //AppendToString(sbH, "\n}\n\n");
            AppendToString(sbH, property.ToString());
            AppendToString(sbH, "\n\n- (NSString *) toXmlString;\n\n@end");
            #endregion

            #region M file
            // import
            AppendToString(sbM, "#import \"");
            AppendToString(sbM, recordType.Name);
            AppendToString(sbM, ".h\"\n");
            if(importUtil)
                AppendToString(sbM, "#import \"Util.h\"\n");
            AppendToString(sbM, "\n");
            
            // implementation
            AppendToString(sbM, "@implementation ");
            AppendToString(sbM, recordType.Name);
            AppendToString(sbM, "\n\n");
            
            // synthesize
            AppendToString(sbM, synthesize.ToString());
            
            // init
            AppendToString(sbM, "-(id) init\n{\n\tself = [super init];\n\tif(self)\n\t{\n");
            AppendToString(sbM, init.ToString());
            AppendToString(sbM, "\n\t}\n\treturn self;\n}\n");

            // dealloc
            AppendToString(sbM, "-(void) dealloc\n{\n");
            AppendToString(sbM, release.ToString());
            AppendToString(sbM, "\n");
            AppendToString(sbM, setnil.ToString());
            AppendToString(sbM, "\n\t[super dealloc];\n}\n\n");

            // toXml
            AppendToString(sbM, toXml.ToString());
            // tableName
            AppendToString(sbM, "-(NSString *) tableName {return tableName; }\n\n");

            // select
            AppendToString(sbM, select.ToString());
            // insert
            AppendToString(sbM, insert.ToString());
            // update
            AppendToString(sbM, update.ToString());
            // load
            AppendToString(sbM, load.ToString());

            // description
            AppendToString(sbM, description.ToString());

            AppendToString(sbM, "\n@end");
            #endregion
            
            list.Add(sbH);
            list.Add(sbM);

            return list;
        }

        private static void AppendToString(StringBuilder sb, String pstr)
        {
            AppendToString(0, sb,pstr);
        }

        private static void AppendToString(int tabCount, StringBuilder sb, String pstr)
        {
            for (int i = 0; i < tabCount; i++)
            {
                sb.Append("\t");
            }
            sb.Append(pstr);
        }
        #endregion

        public ArrayList ToArrayList()
        {
            var list = new ArrayList();
            Type recordType = GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();
            foreach (PropertyInfo propInfo in fieldInf)
            {
                if (propInfo.MemberType == MemberTypes.Property)
                {
                    if (propInfo.PropertyType.Name.ToLower().Trim().IndexOf("arraylist") < 0)
                    {
                        list.Add(propInfo.GetValue(this, null));
                    }
                }
            }
            return list;
        }

        private object GetValue(object val)
        {
            return (val == null ? "" : val.ToString().Trim());
        }

        public void LoadFromDataRow(DataRow dr)
        {
            Type recordType = GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();
            for (int i = 0; i < fieldInf.Length; i++)
            {
                if (fieldInf[i].MemberType == MemberTypes.Property)
                {
                    try
                    {
                        string dataType = fieldInf[i].PropertyType.Name.ToLower();
                        object value = dr[fieldInf[i].Name];
                        if (dataType.StartsWith("int")) // use startswith to catch different types of int
                        {
                             GetType().GetProperty(fieldInf[i].Name).SetValue(this, DbUtil.FixInt(value), null);
                        }
                        else if (dataType.Equals("string"))
                        {
                            GetType().GetProperty(fieldInf[i].Name).SetValue(this, DbUtil.FixString(value), null);
                        }
                        else if (dataType.Equals("decimal"))
                        {
                            GetType().GetProperty(fieldInf[i].Name).SetValue(this, DbUtil.FixDecimal(value), null);
                        }
                        else if (dataType.Equals("float") || dataType.Equals("single") || dataType.Equals("double"))
                        {
                            GetType().GetProperty(fieldInf[i].Name).SetValue(this, DbUtil.FixDouble(value), null);
                        }
                        else if (dataType.StartsWith("date"))
                        {
                            GetType().GetProperty(fieldInf[i].Name).SetValue(this, DbUtil.FixDate(value), null);
                        }
                        else if (dataType.StartsWith("bool"))
                        {
                            GetType().GetProperty(fieldInf[i].Name).SetValue(this, DbUtil.FixBoolean(value), null);
                        }
                    }
                    catch { 
                    }
                }
            }
        }

        private bool HasField(string name)
        {
            bool ret = false;
            Type recordType = GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();
            for (int i = 0; i < fieldInf.Length; i++)
            {
                if (fieldInf[i].MemberType == MemberTypes.Property && fieldInf[i].Name.Equals(name))
                {
                    ret = true;
                }
            }
            return ret;
        }

        private object GetPropertyValue(string name)
        {
            Type recordType = GetType();
            PropertyInfo[] fieldInf = recordType.GetProperties();
            for (int i = 0; i < fieldInf.Length; i++)
            {
                if (fieldInf[i].MemberType == MemberTypes.Property && fieldInf[i].Name.Equals(name))
                {
                    return fieldInf[i].GetValue(this, null);
                }
            }
            return null;
        }
    }
}
