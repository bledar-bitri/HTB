using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HTB.Database
{
    public class StoredProcedureParameter
    {
        string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        object _value = null;
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private ParameterDirection _direction;
        public ParameterDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private SqlDbType _sqlType;
        public SqlDbType SqlType
        {
            get { return _sqlType; }
            set { _sqlType = value; }
        }

        private StoredProcedureParameter() { }
        public StoredProcedureParameter(string name, SqlDbType type, object value) : this(name, type, value, ParameterDirection.Input) { }
        
        public StoredProcedureParameter(string name, SqlDbType type, object value, ParameterDirection direction)
        {
            this.Name = name;
            this.SqlType = type;
            this.Value = value;
            this.Direction = direction;
        }
    }
}
