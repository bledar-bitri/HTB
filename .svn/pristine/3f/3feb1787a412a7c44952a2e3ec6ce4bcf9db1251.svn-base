using System;
using System.Web.UI;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenink
{
    public partial class ActionsRat : Page
    {
        public string BtnCancelText = "Abbrechen";
        
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ctlInstallment.SaveInstallment())
                {
                    try
                    {
                        var control = HTBUtils.GetControlRecord();
                        int id = Convert.ToInt32(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]));
                        var set = new RecordSet();
                        set.ExecuteNonQuery("UPDATE tblCustInkAkt SET CustInkAktCurStatus = " + control.InkassoAktInstallmentStatus + " WHERE CustInkAktID = " + id);
                        var action = (tblCustInkAktAktion)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktAktion WHERE CustInkAktAktionTyp = 10 AND CustInkAktAktionAktID = " + id, typeof(tblCustInkAktAktion));
                        if(action != null)
                        {
                            action.CustInkAktAktionEditDate = DateTime.Now;
                            action.CustInkAktAktionUserId = GlobalUtilArea.GetUserId(Session);
                            set.UpdateRecord(action);
                        }
                        else
                        {
                            new AktUtils(id).CreateAktAktion(10, GlobalUtilArea.GetUserId(Session));
                        }
                    }
                    catch
                    {
                    }

                    ScriptManager.RegisterStartupScript(updPanel1, typeof (string), "closeScript", "MM_refreshParentAndClose();", true);
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
    }
}