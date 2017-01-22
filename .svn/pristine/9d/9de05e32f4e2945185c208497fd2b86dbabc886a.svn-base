/*
 * ClassName:       TableDef
 * Author:          Blade
 * Date Created:    08/12/2008
 * Description:     Represents a table definition row
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace RecordBuilder
{
    class MySqlTableDef
    {
          
        private string _column;
        [MappingAttribute("Field")]
        public string Column
        {
            get { return _column; }
            set { _column = value; }
        }
        
        private string _datatype;
        [MappingAttribute("Type")]
        public string Datatype
        {
            get { return _datatype; }
            set { _datatype = value; }
        }
        
        
        private string _default;
        [MappingAttribute("Default")]
        public string DefaultValue
        {
            get { return _default; }
            set { _default = value; }
        }
        
        private string _nullable;
        [MappingAttribute("Null")]
        public string Nullable
        {
            get { return _nullable; }
            set { _nullable = value; }
        }

        private string _extra;
        public string extra
        {
            get { return _extra; }
            set { _extra = value; }
        }
    }
}
