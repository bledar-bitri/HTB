using System;
using HTB.Database;
using HTBUtilities;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.aktenintaction
{
    public partial class EditActionType : System.Web.UI.Page
    {
        private int _actionTypeId = -1;
        private tblAktenIntActionType _type = new tblAktenIntActionType();
        bool _isNew;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request["ID"] != null && !Request["ID"].Equals(""))
            {
                _actionTypeId = Convert.ToInt32(Request["ID"]);
                LoadRecords();
                if (_type == null)
                    _isNew = true;
            }
            else
            {
                _isNew = true;
            }
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(
                ddlNextStep,
                "SELECT * FROM tblAktenIntActionTypeNextStep ORDER BY ActionTypeNextStepSequenceNumber",
                typeof(tblAktenIntActionTypeNextStep),
                "ActionTypeNextStepID",
                "ActionTypeNextStepCaption",
                false);

                SetValues();
            }
            lblHeader.Text = _isNew ? "NEUER T&Auml;TIGKEIT" : "T&Auml;TIGKEIT EDITIEREN";
        }

        private void LoadRecords()
        {
            _type = (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + _actionTypeId, typeof(tblAktenIntActionType));
        }

        private void SetValues()
        {
            if (!_isNew)
            {
                txtKlientenTypCaption.Text = _type.AktIntActionTypeCaption;
                ddlNextStep.SelectedValue = _type.ActionTypeNextStepID.ToString();
                chkDefault.Checked = _type.AktIntActionIsDefault;
                chkIsExtensionRequest.Checked = _type.AktIntActionIsExtensionRequest;
                chkIsInstallment.Checked = _type.AktIntActionTypeIsInstallment;
                chkAktIntActionIsPersonalCollection.Checked = _type.AktIntActionIsPersonalCollection;
                chkIsWithCollection.Checked = _type.AktIntActionIsWithCollection;
                chkIsDirectPayment.Checked = _type.AktIntActionIsDirectPayment;
                chkIsVoid.Checked = _type.AktIntActionIsVoid;
                chkIsTelAndEmailCollection.Checked = _type.AktIntActionIsTelAndEmailCollection;
                chkIsPositive.Checked = _type.AktIntActionIsPositive;
                chkIsInternal.Checked = _type.AktIntActionIsInternal;
                chkIsThroughPhone.Checked = _type.AktIntActionIsThroughPhone;
                chkAktIntActionIsAutoRepossessed.Checked = _type.AktIntActionIsAutoRepossessed;
                chkAktIntActionIsAutoMoneyCollected.Checked = _type.AktIntActionIsAutoMoneyCollected;
                chkAktIntActionIsAutoNegative.Checked = _type.AktIntActionIsAutoNegative;
                chkAktIntActionIsAutoPaymentInquiry.Checked = _type.AktIntActionIsAutoPaymentInquiry;
                chkAktIntActionIsAutoPayment.Checked = _type.AktIntActionIsAutoPayment;
                chkAktIntActionIsReceivable.Checked = _type.AktIntActionIsReceivable;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            _type.AktIntActionTypeCaption = txtKlientenTypCaption.Text;
            _type.ActionTypeNextStepID = Convert.ToInt16(ddlNextStep.SelectedValue);
            _type.AktIntActionIsDefault = chkDefault.Checked;
            _type.AktIntActionIsExtensionRequest = chkIsExtensionRequest.Checked;
            _type.AktIntActionTypeIsInstallment = chkIsInstallment.Checked;
            _type.AktIntActionIsPersonalCollection = chkAktIntActionIsPersonalCollection.Checked;
            _type.AktIntActionIsWithCollection = chkIsWithCollection.Checked;
            _type.AktIntActionIsDirectPayment = chkIsDirectPayment.Checked;
            _type.AktIntActionIsVoid = chkIsVoid.Checked;
            _type.AktIntActionIsTelAndEmailCollection = chkIsTelAndEmailCollection.Checked;
            _type.AktIntActionIsPositive = chkIsPositive.Checked;
            _type.AktIntActionIsInternal = chkIsInternal.Checked;
            _type.AktIntActionIsThroughPhone = chkIsThroughPhone.Checked;
            _type.AktIntActionIsAutoRepossessed = chkAktIntActionIsAutoRepossessed.Checked;
            _type.AktIntActionIsAutoMoneyCollected = chkAktIntActionIsAutoMoneyCollected.Checked;
            _type.AktIntActionIsAutoNegative = chkAktIntActionIsAutoNegative.Checked;
            _type.AktIntActionIsAutoPaymentInquiry = chkAktIntActionIsAutoPaymentInquiry.Checked;
            _type.AktIntActionIsAutoPayment = chkAktIntActionIsAutoPayment.Checked;
            _type.AktIntActionIsReceivable = chkAktIntActionIsReceivable.Checked;
            var set = new RecordSet();
            if (_actionTypeId <= 0 )
                set.InsertRecord(_type);
            else
                set.UpdateRecord(_type);

            Response.Redirect("../../intranet/aktenintaction/actiontypes.asp");
        }
    }
}