/*
 * Author:			Generated Code
 * Date Created:	24.02.2011
 * Description:		Represents a row in the tblCustInkAkt table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCustInkAkt : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType=MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber=true)]
	    public int CustInkAktID { get; set; }
	    public string CustInkAktAZ { get; set; }
	    public int CustInkAktKlient { get; set; }
	    public int CustInkAktGegner { get; set; }
        public DateTime CustInkAktEnterDate { get; set; }
	    public int CustInkAktStatus { get; set; }
	    public string CustInkAktKunde { get; set; }
	    public string CustInkAktGEName { get; set; }
	    public string CustInkAktGEStrasse { get; set; }
	    public string CustInkAktGEPrefix { get; set; }
	    public string CustInkAktGEZIP { get; set; }
	    public string CustInkAktGEOrt { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double CustInkAktBetragOffen { get; set; }
	    public string CustInkAktStatus2 { get; set; }
	    public int CustInkAktCurStatus { get; set; }
	    public string CustInkAktCurStatusText { get; set; }
	    public DateTime CustInkAktLastChange { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double CustInkAktForderung { get; set; }
	    public string CustInkAktOldID { get; set; }
	    public int CustInkAktEnterUser { get; set; }
	    public int CustInkAktLastChangeUser { get; set; }
	    public int CustInkAktSB { get; set; }
	    public string CustInkAktMemo { get; set; }
	    public int CustInkAktAuftraggeber { get; set; }
	    public string CustInkAktGothiaNr { get; set; }
	    public DateTime CustInkAktInvoiceDate { get; set; }
	    public int CustInkAktBankeinzugFlag { get; set; }
	    public int CustInkAktRatenerinnerungFlag { get; set; }
	    public int CustInkAktDelFlag { get; set; }
	    public DateTime CustInkAktNextWFLStep { get; set; }
	    public int CustInkAktRV { get; set; }
	    public int CustInkAktDZ { get; set; }
	    public int CustInkAktCurStatusHold { get; set; }
	    public int CustInkAktMeldeRetour { get; set; }
	    public int CustInkAktRT { get; set; }
	    public decimal CustInkAktID2 { get; set; }
	    public int CustInkAktCurrentStep { get; set; }
	    public bool CustInkAktIsPartial { get; set; }
	    public bool CustInkAktSkipInitialInvoices { get; set; }
	    public bool CustInkAktIsWflStopped { get; set; }
        public bool CustInkAktSendBericht { get; set; }
        public int CustInkAktLawyerId { get; set; }
        public bool CustInkAktIsAuftragsbestaetigungSent { get; set; }
        public string CustInkAktSource { get; set; }
        public int CustInkAkKlientSB { get; set; }
        
	    #endregion
	}
}
