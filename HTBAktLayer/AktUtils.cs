using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HTB.Database;
using System.Collections;
using HTBUtilities;
using HTBInvoiceManager;
using HTB.Database.Views;
using HTBServices.Mail;
using HTBServices;

namespace HTBAktLayer
{
    public class AktUtils
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region properties
        private readonly AktInkassoUtils _inkAktUtils;
        public AktInkassoUtils InkAktUtils
        {
            get { return _inkAktUtils; }
        }

        private readonly AktInterventionUtils _intAktUtils;
        public AktInterventionUtils IntAktUtils
        {
            get { return _intAktUtils; }
        }

        private AktMeldeUtils _meldeAktUtils;
        public AktMeldeUtils MeldeAktUtils
        {
            get { return _meldeAktUtils; }
            set { _meldeAktUtils = value; }
        }

        private AktLawyerUtil _lawyerUtil;

        private AktLawyerUtil LawyerUtil
        {
            get { return _lawyerUtil; }
            set { _lawyerUtil = value; }
        }

        private readonly int _inkAktId;
        public int InkAktId
        {
            get { return _inkAktId; }
        }
        
        private tblControl _control = HTBUtils.GetControlRecord();
        public tblControl control
        {
            get { return _control; }
            set { _control = value; }
        }
        #endregion

        public AktUtils(int aktId)
        {
            _inkAktId = aktId;
            _inkAktUtils = new AktInkassoUtils(this);
            _intAktUtils = new AktInterventionUtils(this);
            _meldeAktUtils = new AktMeldeUtils(this);
            _lawyerUtil = new AktLawyerUtil(this);
        }

        public void InsertAktenIntPos(int aktId, double amount, string description)
        {
            RecordSet.Insert(new tblAktenIntPos
                                 {
                                     AktIntPosAkt = aktId, 
                                     AktIntPosNr = "Buchung lt. ", 
                                     AktIntPosDatum = DateTime.Now, 
                                     AktIntPosBetrag = amount, 
                                     AktIntPosCaption = description, 
                                     AktIntPosDueDate = DateTime.Now
                                 });
        }
        public void CreateWeggebuer(int aktId, double amount)
        {
            new InvoiceManager().CreateAndSaveInvoice(aktId, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE, amount, "Weggebühr ", true);
        }
        public int CreateMeldeAkt(qryCustInkAkt custInkAkt)
        {
            return _meldeAktUtils.CreateMeldeAkt(custInkAkt);
        }
        
        public void CreateRechtsanwaldCosts(qryCustInkAkt custInkAkt)
        {
            var costTypesList = new List<int> {control.RechtsanwaldKostenArtId};
            decimal balance = Convert.ToDecimal(GetAktBalance());
            var wset = new qryKostenSet();
            wset.LoadKostenBasedOnForderungAndArtId(balance, costTypesList);
            foreach (qryKosten record in wset.qryKostenList)
            {
                decimal amount = HTBUtils.GetCalculatedKost(balance, record, custInkAkt.CustInkAktInvoiceDate);
                new InvoiceManager().CreateAndSaveInvoice(custInkAkt.CustInkAktID, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE, Convert.ToDouble(amount), record.KostenArtText, true);
            }
        }

        public void CreateInterventionCosts(int aktId, DateTime invoiceDate)
        {
            CreateCosts(aktId, control.InterventionKostenArtId, invoiceDate);
        }

        private void CreateCosts(int aktId, int kostenArtId, DateTime invoiceDate)
        {
            var costTypesList = new List<int> { kostenArtId };
            decimal balance = Convert.ToDecimal(GetAktBalance());
            var wset = new qryKostenSet();
            wset.LoadKostenBasedOnForderungAndArtId(balance, costTypesList);
            foreach (qryKosten record in wset.qryKostenList)
            {
                decimal amount = HTBUtils.GetCalculatedKost(balance, record, invoiceDate);
                new InvoiceManager().CreateAndSaveInvoice(aktId, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE, Convert.ToDouble(amount), record.KostenArtText, true);
            }
        } 
        private ArrayList GetAllCombinedActions()
        {
            var aktionsList = new ArrayList();
            var inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionAktID = " + InkAktId + " ORDER BY CustInkAktAktionDate", typeof(qryCustInkAktAktionen));
            var interventionAktions = new ArrayList();
            if (_intAktUtils.IntAkt != null)
            {
                interventionAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + _intAktUtils.IntAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof(qryInktAktAction));
            }
            if (inkassoAktions.Count > 0)
            {
                foreach (object t in inkassoAktions)
                {
                    var inkasso = (qryCustInkAktAktionen)t;
                    for (int j = 0; j < interventionAktions.Count; j++)
                    {
                        var intervention = (qryInktAktAction)interventionAktions[j];
                        if (intervention.AktIntActionDate.CompareTo(inkasso.CustInkAktAktionDate) > 0)
                        {
                            aktionsList.Add(intervention);
                            interventionAktions.Remove(intervention);
                            j--;
                        }
                    }
                    aktionsList.Add(inkasso);
                }
            }
            foreach (object t in interventionAktions)
            {
                aktionsList.Add(t);
            }
            return aktionsList;
        }

        #region Create Intervention-Akt based on Collection-Akt
        public tblAktenInt CreateNewInterventionAkt(qryCustInkAkt custInkAkt, int allowedWorkDays, int aktType = 1)
        {
            return CreateNewInterventionAkt(custInkAkt, string.Empty, allowedWorkDays, aktType);
        }
        public tblAktenInt CreateNewInterventionAkt(qryCustInkAkt custInkAkt, string memo, int allowedWorkDays, int aktType = 1)
        {
            return _intAktUtils.CreateNewInterventionAkt(custInkAkt, memo, allowedWorkDays, aktType);
        }
        #endregion

        #region CollectionInvoice
        public double GetAktOriginalInvoiceAmount()
        {
            return _inkAktUtils.GetAktOriginalInvoiceAmount();
        }
        public double GetAktBalance()
        {
            return _inkAktUtils.GetAktBalance();
        }
        public double GetAktKlientCostsBalance()
        {
            return _inkAktUtils.GetAktKlientCostsBalance();
        }
        public double GetAktKlientTotalBalance()
        {
            return _inkAktUtils.GetAktKlientTotalBalance();
        }
        public double GetAktOriginalInvoiceBalance()
        {
            return _inkAktUtils.GetAktOriginalInvoiceBalance();
        }
        public double GetAktTotalCollectionBalance()
        {
            return _inkAktUtils.GetAktTotalCollectionBalance();
        }
        public double GetAktTotalCollectionAmount()
        {
            return _inkAktUtils.GetAktTotalCollectionAmount();
        }
        public double GetAktTotalInterest()
        {
            return _inkAktUtils.GetAktTotalInterest();
        }
        public double GetAktTotalInvoiceAmount()
        {
            return _inkAktUtils.GetAktTotalInvoiceAmount();
        }
        public double GetAktTotalPayments()
        {
            return _inkAktUtils.GetAktTotalPayments();
        }
        public double GetAktTotalCollectionNettoAmount()
        {
            return _inkAktUtils.GetAktTotalCollectionNettoAmount();
        }
        public double GetAktTotalTax()
        {
            return _inkAktUtils.GetAktTotalTax();
        }

        public void CreateAktAktion(int type, int userId)
        {
            CreateAktAktion(type, userId, "");
        }
        public void CreateAktAktion(int type, int userId, string memo)
        {
            _inkAktUtils.CreateAktion(type, userId, memo);
        }

        public bool IsLaywerInWorkflow()
        {
            return _inkAktUtils.IsLaywerInWorkflow();
        }

        public void SetInkassoStatusBasedOnWflAction(int kostenArtId, int userId, tblAktenInt intAkt = null, string memo = "", int daysToAddToWorkflow = 0)
        {
            tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(InkAktId);
            var kostenArt = (tblKostenArt) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKostenArt WHERE KostenArtID = " + kostenArtId, typeof (tblKostenArt));
            if (kostenArt != null)
            {
                if (kostenArt.KostenCustInkAktCurStatus >= 0)
                {
                    inkAkt.CustInkAktCurStatus = kostenArt.KostenCustInkAktCurStatus;
                }
                if (kostenArt.KostenCustInkAktStatus >= 0)
                {
                    inkAkt.CustInkAktStatus = kostenArt.KostenCustInkAktStatus;
                }
                if (kostenArt.KostenKZID > 0)
                {
                    if(intAkt != null)
                        IntAktUtils.IntAkt = intAkt;
                    CreateAktAktion(kostenArt.KostenKZID, userId, memo);
                }
                inkAkt.CustInkAktNextWFLStep = DateTime.Today.AddDays(daysToAddToWorkflow);
                RecordSet.Update(inkAkt);
            }
        }

        public void CancelInkassoAkt()
        {
            var akt = HTBUtils.GetInkassoAkt(_inkAktId);
            CreateAktAktion(27, HTBUtils.GetControlRecord().AutoUserId);
            akt.CustInkAktStatus = _control.InkassoAktStornoStatus;
            akt.CustInkAktCurStatus = 27; // Storno lt. Auftraggeber
            RecordSet.Update(akt);
        }
        
        
        public void SendInkassoPackageToLawyer(HTBEmailAttachment zwischenbericht)
        {
            _lawyerUtil.SendInkassoPackageToLawyer(zwischenbericht, false, null);
        }

        public void SendInkassoPackageToLawyer(HTBEmailAttachment zwischenbericht, bool saveCopyToFolder, string folderPath)
        {
            _lawyerUtil.SendInkassoPackageToLawyer(zwischenbericht, saveCopyToFolder, folderPath);
        }
        #endregion

        #region Create Intervention Action
        public void CreateInterventionAktAktion(int type, string memo = "")
        {
            _intAktUtils.CreateAktion(type, memo);
        }
        #endregion

        
        public void SendAbschlusBerichtToClient(tblCustInkAkt akt, Stream bericht, string configValue, string subject)
        {
            var klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientID = " + akt.CustInkAktKlient, typeof (tblKlient));
            var gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + akt.CustInkAktGegner, typeof (tblGegner));
            var sb = new StringBuilder();
            List<string> receipients = HTBUtils.GetKlientEmailAddresses(klient.KlientID, klient.KlientEMail);
            if(HTBUtils.IsTestEnvironment)
                receipients = new List<string>(); // send email only to office
            if (receipients.Count == 0)
            {
                subject += " [An Klient NICHT Geschickt]!!!";
                sb.Append("Keine Emailadresse f&uuml;r Klient eingestellt: <strong>");
                sb.Append(klient.KlientName1);
                sb.Append(" ");
                sb.Append(klient.KlientName2);
                sb.Append("</strong>");
            }
            receipients.Add(HTBUtils.GetConfigValue("Default_EMail_Addr")); // send copy to office

            var contact = (tblAnsprechpartner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAnsprechpartner WHERE AnsprechKlient = " + klient.KlientID, typeof(tblAnsprechpartner));

            string body = HTBUtils.GetFileText(HTBUtils.GetConfigValue(configValue));
            body = contact != null ? body.Replace("[name]", contact.AnsprechTitel + " " + contact.AnsprechNachname) : body.Replace("[name]", "Damen und Herren");
            body = body.Replace("[akt]", akt.CustInkAktID.ToString());
            body = body.Replace("[gegner]", gegner.GegnerName1 + " " + gegner.GegnerName2 + " " + gegner.GegnerName3);
            sb.Append(body);

            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(
                receipients,
                subject + ": [" + InkAktId + "]",
                sb.ToString(),
                true,
                new HTBEmailAttachment[]
                    {
                        new HTBEmailAttachment(bericht, "Abschlussbericht_"+InkAktId+".pdf", "Application/pdf")
                    }
               , akt.CustInkAktID, 0);
        }
    }
}
