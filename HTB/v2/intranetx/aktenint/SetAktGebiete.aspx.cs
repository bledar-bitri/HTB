using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.HTB.StoredProcs;
using HTBUtilities;
using System.Web.UI;

namespace HTB.v2.intranetx.aktenint
{
    public partial class SetAktGebiete : Page
    {
        private readonly ArrayList _selectedUsers = new ArrayList();
        private ArrayList _aktenToFixList = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            btnProcessAkten.Visible = false;
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        public void LoadUsers()
        {
            ArrayList users = HTBUtils.GetStoredProcedureRecords("spGetAktiveUsersAndAktCount", null, typeof (spGetAktiveUsersAndAktCount));
            foreach (spGetAktiveUsersAndAktCount user in users)
            {
                chklst.Items.Add(new ListItem(user.UserFirstName + " " + user.UserLastName + " [" + user.AktCount+ " Akten]", user.UserID.ToString()));
            }
        }
        

        #region Event Handlers
        protected void BtnShowAktenClicked(object sender, EventArgs e)
        {
            ShowAkten();

        }
        protected void BtnProcessAktenClicked(object sender, EventArgs e)
        {
            ProcessAkten();
        }
        #endregion

        #region Grid
        private void PopulateGrid()
        {
            
            DataTable dt = GetDataTableStructure();
            foreach (spGetAktsAssignedToWrongUser rec in _aktenToFixList)
            {
                DataRow dr = dt.NewRow();

                dr["Akt"] = rec.AktIntId.ToString();
                dr["EnterDate"] = rec.EnterDate.ToShortDateString();
                dr["DueDate"] = rec.DueDate.ToShortDateString();
                dr["Client"] = rec.ClientFirstName + " " + rec.ClientLastName;
                dr["Gegner"] = rec.GegnerFirstName + " " + rec.GegnerLastName + "<br/>" + rec.GegnerZip + ", " + rec.GegnerCity;
                dr["AG"] = rec.AuftraggeberFirstName + " " + rec.AuftraggeberLastName;
                dr["SB"] = rec.UserFirstName + " " + rec.UserLastName + "<br/>--> " + rec.CorrectUserFirstName + " " + rec.CorrectUserLastName;
                dr["SBID"] = rec.CorrectUserID.ToString();

                dt.Rows.Add(dr);
            }
            gvAkte.DataSource = dt;
            gvAkte.DataBind();
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("Akt", typeof(string)));
            dt.Columns.Add(new DataColumn("EnterDate", typeof(string)));

            dt.Columns.Add(new DataColumn("DueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Client", typeof(string)));
            dt.Columns.Add(new DataColumn("Gegner", typeof(string)));
            dt.Columns.Add(new DataColumn("AG", typeof(string)));
            dt.Columns.Add(new DataColumn("SB", typeof(string)));
            dt.Columns.Add(new DataColumn("SBID", typeof(string)));
            return dt;
        }
        #endregion
    
        public ArrayList GetAkten()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _selectedUsers.Count; i++)
            {
                var s = (string)_selectedUsers[i];
                sb.Append(s);
                if (i < _selectedUsers.Count - 1)
                {
                    sb.Append(",");
                }
            }
            var spParams = new ArrayList
                               {
                                   new StoredProcedureParameter("userIdList", SqlDbType.VarChar, sb.ToString())
                               };
            return HTBUtils.GetStoredProcedureRecords("spGetAktsAssignedToWrongUser", spParams, typeof (spGetAktsAssignedToWrongUser));
        }
        
        public void ShowAkten()
        {
            _selectedUsers.Clear();
            foreach (ListItem lstItem in chklst.Items)
            {
                if (lstItem.Selected)
                {
                    _selectedUsers.Add(lstItem.Value);
                }
            }
            if (_selectedUsers.Count == 0)
            {
                ctlMessage.ShowError("Sie m&uuml;ssen mindestenst ein Sachbearbeiter aufw&auml;hlen!");
            }
            else
            {
                _aktenToFixList = GetAkten();
                if (_aktenToFixList.Count > 0)
                {
                    PopulateGrid();
                }
                else
                {
                    ctlMessage.ShowInfo("Es gibt keine &Uuml;bernehmbare Atke! Alle Akten sind auf dem richtigen Sachbearbeiter hinterlegt!");
                    PopulateGrid();
                }

            }
            btnProcessAkten.Visible = _aktenToFixList.Count > 0;

        }
        public void ProcessAkten()
        {
            var set = new RecordSet();
            try
            {
                set.StartTransaction();
                for (int i = 0; i < gvAkte.Rows.Count; i++)
                {
                    GridViewRow row = gvAkte.Rows[i];
                    var lblAkt = (Label)row.FindControl("lblAkt");
                    var lblSbId = (Label)row.FindControl("lblSbId");
                    if (lblSbId.Text.Trim() != "")
                        set.ExcecuteNonQueryInTransaction("UPDATE tblAktenInt SET AktIntSb = " + lblSbId.Text + " WHERE AktIntID = " + lblAkt.Text);
                }
                set.CommitTransaction();
                ctlMessage.ShowSuccess("Die Akten sind &uuml;bernohmen!");
                //ShowAkten();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
                set.RollbackTransaction();
            }
        }

        public int GetTotalAktenToFix()
        {
            return _aktenToFixList.Count;
        }
    }
}