/*
 * Author:			Generated Code
 * Date Created:	02.08.2011
 * Description:		Represents a row in the tblCustInkAktStatus table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCustInkAktStatus : Record
	{
		#region Property Declaration 

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int CustInkAktStatusId { get; set; }
	    public int CustInkAktStatusCode { get; set; }
	    public string CustInkAktStatusCaption { get; set; }

	    #endregion
	}
}
