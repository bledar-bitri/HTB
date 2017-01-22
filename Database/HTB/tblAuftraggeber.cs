/*
 * Author:			Generated Code
 * Date Created:	22.02.2011
 * Description:		Represents a row in the tblAuftraggeber table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAuftraggeber : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int AuftraggeberID { get; set; }
        public string AuftraggeberAnrede { get; set; }
        public string AuftraggeberTitel { get; set; }
        public string AuftraggeberName1 { get; set; }
        public string AuftraggeberName2 { get; set; }
        public string AuftraggeberName3 { get; set; }
        public string AuftraggeberStrasse { get; set; }
        public string AuftraggeberLKZ { get; set; }
        public string AuftraggeberPLZ { get; set; }
        public string AuftraggeberOrt { get; set; }
        public int AuftraggeberStaat { get; set; }
        public string AuftraggeberMemo { get; set; }
        public string AuftraggeberPhoneCountry { get; set; }
        public string AuftraggeberPhoneCity { get; set; }
        public string AuftraggeberPhone { get; set; }
        public string AuftraggeberFaxCountry { get; set; }
        public string AuftraggeberFaxCity { get; set; }
        public string AuftraggeberFax { get; set; }
        public string AuftraggeberEMail { get; set; }
        public string AuftraggeberWeb { get; set; }
        public int AuftraggeberIntAktPovAbzug { get; set; }
        public int AuftraggeberHinterlegung { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AuftraggeberInterventionshonorar { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AuftraggeberInterventionsProvision { get; set; }
        public int AuftraggeberBank { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AuftraggeberInterventionsProvision2 { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AuftraggeberInterventionsKost { get; set; }
        public bool AuftraggeberSendConfirmation { get; set; }
        public bool AuftraggeberUseMerzedesProtocol { get; set; }
        #endregion
    }
}
