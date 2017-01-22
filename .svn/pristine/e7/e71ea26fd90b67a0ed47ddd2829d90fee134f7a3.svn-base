using System;
using System.IO;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System.Security.AccessControl;

namespace HTB.v2.intranetx.aktenink
{
    public partial class DeleteMahnung : System.Web.UI.Page
    {
        private int _aktId;
        private int _mahnung;
        private int _docId;

        protected void Page_Load(object sender, EventArgs e)
        {
            bdy.Attributes.Add("onload", "movesizeandopen();");
            _aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.INKASSO_AKT]);
            _mahnung = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.MAHNUNG_NUMBER]);
            _docId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.DOCUMENT_ID]);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWindow();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(_aktId > 0 && _mahnung > 0)
            {
                bool ok = true;
                var mahnungsText = _mahnung + ". Mahnung";
                var inv = (tblCustInkAktInvoice) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + _aktId + " AND InvoiceDescription = '" + mahnungsText + "' ORDER BY InvoiceDate", typeof (tblCustInkAktInvoice));
                var action = (tblCustInkAktAktion)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktAktion WHERE CustInkAktAktionAktID = " + _aktId + " AND CustInkAktAktionCaption like '%" + mahnungsText + "' ORDER BY CustInkAktAktionDate", typeof(tblCustInkAktAktion));
                var doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblDokument WHERE DokID = " + _docId, typeof(tblDokument));
                tblMahnung mahnung = null;
                
                if(doc != null)
                    mahnung = (tblMahnung)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblMahnung WHERE MahnungAktID = " + _aktId + " AND MahnungDate between '" + doc.DokCreationTimeStamp.ToShortDateString() + "' AND '" + doc.DokCreationTimeStamp.AddDays(1).ToShortDateString() + "'", typeof(tblMahnung));
                
                var set = new RecordSet(); 
                set.StartTransaction();
                try
                {
                    if(inv != null)
                        set.DeleteRecordInTransaction(inv);
                    if(action != null)
                        set.DeleteRecordInTransaction(action);
                    if(doc != null)
                        set.DeleteRecordInTransaction(doc);
                    if (mahnung != null)
                        set.DeleteRecordInTransaction(mahnung);

                    set.CommitTransaction();
                }
                catch (Exception ex)
                {
                    cltMessage.ShowException(ex);
                    set.RollbackTransaction();
                    ok = false;
                }
                try
                {
                    if (ok && doc != null)
                    {
                        string user = HTBUtils.GetConfigValue("ImpersonatorUser");
                        string domain = HTBUtils.GetConfigValue("ImpersonatorDomain");
                        string password = HTBUtils.GetConfigValue("ImpersonatorPassword");

                        using (new Impersonator(user, domain, password))
                        {
                            File.Delete(HTBUtils.GetConfigValue("DocumentsFolder") + doc.DokAttachment);
                        }
                    }
                    if (ok)
                        CloseWindowAndRefresh();
                }
                catch(Exception ex)
                {
                    cltMessage.ShowException(ex);
                }
            }
        }

        private void CloseWindowAndRefresh()
        {
            bdy.Attributes.Add("onload", "MM_refreshParentAndClose();");
        }

        private void CloseWindow()
        {
            bdy.Attributes.Add("onload", "window.close();");
        }
    }
}