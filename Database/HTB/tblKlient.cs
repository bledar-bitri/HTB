/*
 * Author:			Generated Code
 * Date Created:	22.02.2011
 * Description:		Represents a row in the tblKlient table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblKlient : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int KlientID { get; set; }
	    public int KlientType { get; set; }
	    public string KlientAnrede { get; set; }
	    public string KlientTitel { get; set; }
	    public string KlientName1 { get; set; }
	    public string KlientName2 { get; set; }
	    public string KlientName3 { get; set; }
	    public string KlientStrasse { get; set; }
        public string KlientLKZ { get; set; }
	    public string KlientPLZ { get; set; }
	    public string KlientOrt { get; set; }
	    public int KlientStaat { get; set; }
	    public string KlientMemo { get; set; }
	    public int KlientDetektei { get; set; }
	    public int KlientInkasso { get; set; }
	    public string KlientAnsprech { get; set; }
	    public string KlientPhoneCountry { get; set; }
	    public string KlientPhoneCity { get; set; }
	    public string KlientPhone { get; set; }
	    public string KlientFaxCountry { get; set; }
	    public string KlientFaxCity { get; set; }
	    public string KlientFax { get; set; }
	    public string KlientEMail { get; set; }
	    public string KlientWeb { get; set; }
	    public string KlientBLZ1 { get; set; }
	    public string KlientKtoNr1 { get; set; }
	    public string KlientBankCaption1 { get; set; }
	    public string KlientBLZ2 { get; set; }
	    public string KlientKtoNr2 { get; set; }
	    public string KlientBankCaption2 { get; set; }
	    public string KlientBLZ3 { get; set; }
	    public string KlientKtoNr3 { get; set; }
	    public string KlientBankCaption3 { get; set; }
	    public int KlientCreator { get; set; }
	    public DateTime KlientTimeStam { get; set; }
	    public int KlientNachricht { get; set; }
	    public string KlientOldID { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double KlientPreis { get; set; }
	    public DateTime KlientLastChange { get; set; }
	    public int KlientVerrechnung { get; set; }
	    public int KlientPreisDruckAEBrief { get; set; }
	    public int KlientRechnungMittels { get; set; }
	    public int KlientInterventionsmeldung { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double KlientZinsenAusForderungen { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double KlientKlagslimit { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double KlientBonLimit { get; set; }
	    public int KlientAccountManager { get; set; }
	    public int KlientAccountManager2 { get; set; }
	    public int KlientINKISImport { get; set; }
	    public DateTime KlientINKISImportDate { get; set; }
	    public int KlientWFINKBookingOrderHF { get; set; }
	    public int KlientWFINKBookingOrderZINZ { get; set; }
	    public int KlientWFINKBookingOrderKOST { get; set; }
	    public int KlientWFINKPaymentSplitHF { get; set; }
	    public int KlientWFINKPaymentSplitZINS { get; set; }
	    public int KlientWFINKPaymentSplitKosten { get; set; }
	    public int KlientWFINKPaymentMode { get; set; }
	    public int KlientWFINKPaymentTime { get; set; }
	    public int KlientWFINKInterestSplitCompany { get; set; }
	    public int KlientWFINKInterestSplitKlient { get; set; }
	    public int KlientMeldeNachricht { get; set; }
	    public int KlientShowGebdat { get; set; }
	    public int KlientContacter { get; set; }
	    public int KlientSalesPromoter { get; set; }
	    public string KlientFirmenbuchnummer { get; set; }
        public string KlientVersicherung { get; set; }
        public string KlientPolizzennummer { get; set; }
        public bool KlientReceivesInterest { get; set; }
        public int KlientLawyerId { get; set; }
        public string KlientIBAN { get; set; }
        public string KlientBIC { get; set; }
        public int KlientExcelInterfaceCode { get; set; }
        
		#endregion
	}
}
