/*
 * ClassName:       MappingAttribute
 * Author:          Blade
 * Date Created:    07/29/2008
 * Description:     Attribute class to help decide how to map a field (see RecordLoader)  
 */
using System;

namespace RecordBuilder
{
    [AttributeUsage(AttributeTargets.All)]
    public class MappingAttribute : System.Attribute
    {
        public string fieldName = String.Empty;
        private string _functionClass = String.Empty;
        private string _functionName = String.Empty;
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
        
        public MappingAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }
}
