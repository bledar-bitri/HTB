/*
 * Author:			Generated Code
 * Date Created:	01.04.2011
 * Description:		Represents a row in the tblAktenIntAction table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktenIntAction : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int AktIntActionID { get; set; }

	    public int AktIntActionType { get; set; }

	    public DateTime AktIntActionDate { get; set; }

	    public DateTime AktIntActionTime { get; set; }

	    public string AktIntActionMemo { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntActionBetrag { get; set; }

	    public int AktIntActionAkt { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntActionProvision { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntActionHonorar { get; set; }

	    public string AktIntActionBeleg { get; set; }

	    public int AktIntActionUeberwiesen { get; set; }

	    public int AktIntActionProvAbzug { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntActionPrice { get; set; }

	    public int AktIntActionAktIntExtID { get; set; }

	    public int AktIntActionSB { get; set; }
        public int AktIntActionReceiptID { get; set; }
        public double AktIntActionLatitude { get; set; }
        public double AktIntActionLongitude { get; set; }
        public string AktIntActionAddress { get; set; }
        public string AktIntActionAuftraggeberSB { get; set; }
        
        #endregion
	}
}
