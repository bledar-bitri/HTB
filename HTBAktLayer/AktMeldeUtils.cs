using System;
using System.Collections.Generic;
using HTB.Database;
using System.Collections;
using HTBUtilities;
using HTBInvoiceManager;
using HTB.Database.Views;

namespace HTBAktLayer
{
    public class AktMeldeUtils
    {
        private readonly AktUtils _aktUtils;

        public AktMeldeUtils(AktUtils aktU)
        {
            _aktUtils = aktU;
        }

        public int CreateMeldeAkt(qryCustInkAkt custInkAkt)
        {
            CreateMeldeCosts(custInkAkt);
            return CreateAndSaveMeldeAkt(custInkAkt);
        }

        private int CreateAndSaveMeldeAkt(qryCustInkAkt custInkAkt)
        {
            RecordSet.Insert(new tblAktMelde
                            {
                                AMNr = custInkAkt.CustInkAktID.ToString(),
                                AMSBKlient = custInkAkt.KlientOldID,
                                AMGegner = custInkAkt.GegnerID,
                                AMStatus = 1,
                                AMErfasstDatum = DateTime.Today,
                                AMErledigtDatum = new DateTime(1900, 1, 1),
                                AMUebergabeDatum = new DateTime(1900, 1, 1),
                                AMInfoAuswahl = 0,
                                AMSB = 99,
                                AMKlient = custInkAkt.KlientID,
                                AMCreatedUser = 99,
                                AMAktFlagInvoice = 0
                            });
            
            return((SingleValue)HTBUtils.GetSqlSingleRecord("SELECT MAX(AMID) IntValue FROM tblAktMelde", typeof (SingleValue))).IntValue;
        }

        private void CreateMeldeCosts(qryCustInkAkt custInkAkt)
        {
            var costTypesList = new List<int>();
            if (custInkAkt.GegnerZipPrefix.Trim().ToUpper().Equals("") || custInkAkt.GegnerZipPrefix.Trim().ToUpper().Equals("A")) 
                costTypesList.Add(_aktUtils.control.MeldeKostenArtId);
            else 
                costTypesList.Add(_aktUtils.control.MeldeKostenAuslandArtId);
            decimal balance = Convert.ToDecimal(_aktUtils.GetAktBalance());
            var wset = new qryKostenSet();
            wset.LoadKostenBasedOnForderungAndArtId(balance, costTypesList);
            foreach (qryKosten record in wset.qryKostenList)
            {
                decimal amount = HTBUtils.GetCalculatedKost(balance, record, custInkAkt.CustInkAktInvoiceDate); // calc based on balance
                new InvoiceManager().CreateAndSaveInvoice(custInkAkt.CustInkAktID, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MELDE, Convert.ToDouble(amount), record.KostenArtText, true);
            }
        }
    }
}
