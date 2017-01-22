/*
 * Author:			Generated Code
 * Date Created:	12/21/2010
 * Description:		Represents a single value returned from the database (usefull for agragate functions: sum, avg, max.c..)
*/

using System;
namespace HTB.Database
{
	public class SingleValue
	{
		#region Property Declaration

	    public int IntValue { get; set; }
	    public double DoubleValue { get; set; }
	    public string StringValue { get; set; }
        public DateTime DateValue { get; set; }
        public long LongValue { get; set; }
	    
	    #endregion
	}
}
