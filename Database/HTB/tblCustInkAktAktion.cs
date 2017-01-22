/*
 * Author:			Generated Code
 * Date Created:	26.04.2011
 * Description:		Represents a row in the tblCustInkAktAktion table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCustInkAktAktion : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int CustInkAktAktionID { get; set; }

	    public DateTime CustInkAktAktionDate { get; set; }

	    public string CustInkAktAktionCaption { get; set; }

	    public int CustInkAktAktionTyp { get; set; }

	    public DateTime CustInkAktAktionEditDate { get; set; }

	    public int CustInkAktAktionAktID { get; set; }

	    public string CustInkAktAktionMemo { get; set; }
	    public int CustInkAktAktionUserId { get; set; }

	    #endregion
	}
}
