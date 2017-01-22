/*
 * ClassName:       PrimaryKey
 * Author:          Blade
 * Date Created:    10/05/2008
 * Description:     Represents a primary key definition 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace RecordBuilder
{
    class PrimaryKey
    {
        private string _tableName;
        [MappingAttribute("TABLE_NAME")]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        private string _constraintName;

        [MappingAttribute("CONSTRAINT_NAME")]
        public string ConstraintName
        {
            get { return _constraintName; }
            set { _constraintName = value; }
        }

        private string _columnName;
        [MappingAttribute("COLUMN_NAME")]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
    }
}
