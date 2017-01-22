/*
 * Author:			Generated Code
 * Date Created:	28.02.2011
 * Description:		Represents a row in the tblAktenIntPos table
*/
using System.Data.SqlClient;
using System.Data;
using System;

namespace HTB.Database
{
	public class tblAktenIntPos : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber=true)]
	    public int AktIntPosID { get; set; }
	    public int AktIntPosAkt { get; set; }
	    public string AktIntPosNr { get; set; }
	    public DateTime AktIntPosDatum { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntPosBetrag { get; set; }
	    public string AktIntPosCaption { get; set; }
	    public DateTime AktIntPosDueDate { get; set; }
        public DateTime AktIntPosTransferredDate { get; set; }
        public int AktIntPosTypeCode { get; set; }
	    #endregion
	}
}
