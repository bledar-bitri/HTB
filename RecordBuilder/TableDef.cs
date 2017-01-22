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
    class TableDef
    {
        private string _table;
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }
        
        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; }
        }
        
        private string _datatype;
        public string Datatype
        {
            get { return _datatype; }
            set { _datatype = value; }
        }
        
        private int _length;
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        
        private string _default;
        
        [MappingAttribute("Default")]
        public string DefaultValue
        {
            get { return _default; }
            set { _default = value; }
        }
        
        private string _nullable;
        public string Nullable
        {
            get { return _nullable; }
            set { _nullable = value; }
        }

        private int _xprecision;
        public int XPrecision
        {
            get { return _xprecision; }
            set { _xprecision = value; }
        }

        private int _xscale;
        public int XScale
        {
            get { return _xscale; }
            set { _xscale = value; }
        }
        
        private int _precision;
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }
    }
}
