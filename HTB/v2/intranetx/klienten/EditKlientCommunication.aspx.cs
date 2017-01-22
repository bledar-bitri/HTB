using System;
using System.Text;
using HTB.Database;
using HTBUtilities;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.klienten
{
    public partial class EditKlientCommunication : System.Web.UI.Page
    {
        int _communicationId;
        private int _clientId;
        tblKlientCommunication _agComm;
        bool isNew = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            _clientId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.QueryString[GlobalHtmlParams.CLIENT_ID]);
            if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.ID]).Equals(""))
            {
                _communicationId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.QueryString[GlobalHtmlParams.ID]);
                _agComm = (tblKlientCommunication)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlientCommunication WHERE KlientComID = " + _communicationId, typeof(tblKlientCommunication));
                if (_agComm == null)
                    isNew = true;
                else if (!IsPostBack)
                    SetValues();
            }
            else
                isNew = true;


            lblHeader.Text = isNew ? "NEU KLIENTKOMMUNIKATION" : "KLIENTKOMMUNIKATION EDITIEREN";
            litTitle.Text = isNew ? "HTB.ASP [ Neu Klientkommunikation]" : "HTB.ASP [ Klientkommunikation editieren ]";
        }

        private void SetValues()
        {
            txtMemo.Text = _agComm.KlientComText;
        }

        private void LoadValues()
        {
            if (isNew)
            {
                _agComm = new tblKlientCommunication {KlientComKlient = _clientId};
            }
            else
            {
                _agComm.KlientComID = _communicationId;
            }
            _agComm.KlientComDate = DateTime.Now;
            _agComm.KlientComUser = GlobalUtilArea.GetUserId(Session);
            _agComm.KlientComText = txtMemo.Text;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LoadValues();
            var action = "insert";
            try
            {
                if (isNew)
                {
                    RecordSet.Insert(_agComm);
                }
                else
                {
                    RecordSet.Update(_agComm);
                    action = "update";
                }

                CloseWindowAndRefresh();
            }
            catch(Exception ex)
            {
                ctlMessage.ShowException(ex);
                ctlMessage.AppendError("<BR/>");
                ctlMessage.AppendError("<BR/> Action: "+action);
                ctlMessage.AppendError("<BR/> ComID: "+_communicationId);
                ctlMessage.AppendError("<BR/> KLientID: " + _agComm.KlientComKlient);
                ctlMessage.AppendError("<BR/> UserId: " + _agComm.KlientComUser);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWindow();
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