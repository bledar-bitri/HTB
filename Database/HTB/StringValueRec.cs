/*
 * Author:			Generated Code
 * Date Created:	12/21/2010
 * Description:		Represents a single value returned from the database (usefull for agragate functions: sum, avg, max.c..)
*/

using System;
namespace HTB.Database
{
	public class StringValueRec : Record
	{
		#region Property Declaration

	    public string Name { get; set; }
	    public string Value { get; set; }
        public StringValueRec(string name, string value)
        {
            Name = name;
            Value = value;
        }
        
	    #endregion
	}
}
