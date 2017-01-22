/*
 * ClassName:       MappingAttribute
 * Author:          Blade
 * Date Created:    07/29/2009
 * Description:     Attribute class to help decide how to map a field (see RecordLoader)  
 */
using System;

namespace HTB.Database
{
    [AttributeUsage(AttributeTargets.All)]
    public class MappingAttribute : System.Attribute
    {
        public const string NO_DB_SAVE = "NO___SAVE";
        public const string FIELD_TYPE_ID = "FIELD___TYPE___ID";
        public const string FIELD_FORMAT_CURRENCY = "FIELD___FORMAT___CURRENCY";

        private string _fieldName = String.Empty;
        private string _functionClass = String.Empty;
        private string _functionName = String.Empty;
        private string _fieldType = String.Empty;
        private string _fieldFormat = String.Empty;
        private bool _fieldAutoNumber = false;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        public string MappingMethodName
        {
            get { return _functionName; }
            set { _functionName = value; }
        }
        public string MappingClass
        {
            get { return _functionClass; }
            set { _functionClass = value; }
        }
        public string FieldType
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }
        public string FieldFormat
        {
            get { return _fieldFormat; }
            set { _fieldFormat = value; }
        }
        public bool FieldAutoNumber
        {
            get { return _fieldAutoNumber; }
            set { _fieldAutoNumber = value; }
        }
    }
}
