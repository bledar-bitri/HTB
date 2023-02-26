using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using HTB.Database;
using System.Collections;
using System.Data;
using HTB.v2.intranetx.util;
using HTBUtilities;
using HTB.Database.StoredProcs;
using System.Text;

namespace HTB.v2.intranetx.aktenintprovfix
{
    public partial class ProvFix : System.Web.UI.Page
    {
        static DateTime _startDate;
        static DateTime _endDate;
        static bool _showInvalidProvision;
        static bool _showInvalidPrice;
        static int _actionCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart.Text);
                _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd.Text);
                chkShowInvalidProvision.Checked = true;
                chkShowInvalidPrice.Checked = true;
                _showInvalidProvision = true;
                _showInvalidPrice = true;
            }
        }

        public ArrayList GetActions()
        {
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("@startDate", SqlDbType.Date, _startDate), 
                                     new StoredProcedureParameter("@endDate", SqlDbType.Date, _endDate)
                                 };

            ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetIntActions", parameters, typeof(spGetIntActions));
            var results = new ArrayList();
            var provisionCalc = new ProvisionCalc();
            foreach (spGetIntActions action in list)
            {
                action.CalculatedProvision = provisionCalc.GetProvision(action.CollectedAmount, 0, action.AuftraggeberID, action.AktTypeID, action.UserID, action.ActionTypeID);
                action.CalculatedPrice = provisionCalc.GetPrice(action.CollectedAmount, 0, action.AuftraggeberID, action.AktTypeID, action.UserID, action.ActionTypeID);
                if (HTBUtils.FormatCurrency(action.CalculatedProvision) != HTBUtils.FormatCurrency(action.ActionProvision) || HTBUtils.FormatCurrency(action.CalculatedPrice) != HTBUtils.FormatCurrency(action.ActionPrice))
                {

                    action.ActionIdLink = "<a href=\"javascript:MM_openBrWindow('/v2/intranetx/aktenint/NewAction.aspx?INTID=" + action.AktId + "&AktionID=" + action.ActionID + "&AG=" + action.AuftraggeberID + "','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10')\">" + action.ActionID + "</a>";
                    if (_showInvalidProvision && HTBUtils.FormatCurrency(action.CalculatedProvision) != HTBUtils.FormatCurrency(action.ActionProvision))
                    {
                        action.ErrorDescription = "Invalid Provision [" + action.CalculatedProvision + "] [" + action.ActionProvision + "]";
                        results.Add(action);
                    }
                    else if (_showInvalidPrice && HTBUtils.FormatCurrency(action.CalculatedPrice) != HTBUtils.FormatCurrency(action.ActionPrice))
                    {
                        action.ErrorDescription = "Ivalid Price [" + action.CalculatedPrice + "] [" + action.ActionPrice + "]";
                        results.Add(action);
                    }
                }
            }
            _actionCount = results.Count;
            return results;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<spGetIntActions> GetInvalidActions(int maximumRows, int startRowIndex)
        {
            return GetActions().Cast<spGetIntActions>().ToList();
        }

        public int GetTotalActionsCount()
        {
            return _actionCount;
        }

        public void FixActions()
        {
            if (_showInvalidPrice || _showInvalidProvision)
            {
                var parameters = new ArrayList
                                     {
                                         new StoredProcedureParameter("@startDate", SqlDbType.Date, _startDate), 
                                         new StoredProcedureParameter("@endDate", SqlDbType.Date, _endDate)
                                     };
                try
                {
                    ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetIntActions", parameters, typeof(spGetIntActions));
                    var results = new ArrayList();
                    var set = new RecordSet();
                    set.StartTransaction();
                    int count = 0;
                    var provisionCalc = new ProvisionCalc();
                    foreach (spGetIntActions action in list)
                    {
                        var sb = new StringBuilder("UPDATE tblAktenIntAction SET ");
                        action.CalculatedProvision = provisionCalc.GetProvision(action.CollectedAmount, 0, action.AuftraggeberID, action.AktTypeID, action.UserID, action.ActionTypeID);
                        action.CalculatedPrice = provisionCalc.GetPrice(action.CollectedAmount, 0, action.AuftraggeberID, action.AktTypeID, action.UserID, action.ActionTypeID);

                        if (HTBUtils.FormatCurrency(action.CalculatedProvision) != HTBUtils.FormatCurrency(action.ActionProvision) || HTBUtils.FormatCurrency(action.CalculatedPrice) != HTBUtils.FormatCurrency(action.ActionPrice))
                        {
                            if (_showInvalidProvision && _showInvalidPrice)
                            {
                                sb.Append("AktIntActionPrice = ");
                                sb.Append(HTBUtils.FormatCurrencyNumber(action.CalculatedPrice).Replace(",", "."));
                                sb.Append(", AktIntActionProvision = ");
                                sb.Append(HTBUtils.FormatCurrencyNumber(action.CalculatedProvision).Replace(",", "."));
                            }
                            else if (_showInvalidPrice)
                            {
                                sb.Append("AktIntActionPrice = ");
                                sb.Append(HTBUtils.FormatCurrencyNumber(action.CalculatedPrice).Replace(",", "."));
                            }
                            else if (_showInvalidProvision)
                            {
                                sb.Append("AktIntActionProvision = ");
                                sb.Append(HTBUtils.FormatCurrencyNumber(action.CalculatedProvision).Replace(",", "."));
                            }
                            sb.Append(", AktIntActionHonorar = 0");
                            sb.Append(" WHERE AktIntActionID = " + action.ActionID);
                            set.ExecuteNonQueryInTransaction(sb.ToString());
                            count++;
                        }
                    }
                    set.CommitTransaction();
                    _actionCount = results.Count;
                    ctlMessage.ShowSuccess("Fixed " + count + " Aktions");
                }
                catch (Exception ex)
                {
                    ctlMessage.ShowException(ex);
                }
            }
        }

        private void LoadScreenEntry()
        {
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart.Text);
            _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd.Text);
            _showInvalidProvision = chkShowInvalidProvision.Checked;
            _showInvalidPrice = chkShowInvalidPrice.Checked;
        }

        #region Event Hanlders
        protected void btnShowBadProv_Click(object sender, EventArgs e)
        {
            try
            {
                LoadScreenEntry();
                gvBadProv.DataBind();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LoadScreenEntry();
            FixActions();
        }
        public void gvBadProv_DataBound(object sender, EventArgs e)
        {
            ctlMessage.ShowInfo(_actionCount + " Invalid Actions Found");
        }
        #endregion
    }
}