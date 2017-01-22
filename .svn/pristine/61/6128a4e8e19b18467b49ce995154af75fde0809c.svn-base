using System;
using HTBUtilities;

namespace HTB.v2.intranetx.global_files
{
    public partial class UploadFile : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fieldname.Value = Request.QueryString["fieldname"];
                configname.Value = Request.QueryString["configname"];
            }
        }

        protected void SUB1_Click(object sender, EventArgs e)
        {
            string msg = "";
            message.InnerHtml = msg;
            try
            {
                string importFolder = HTBUtils.GetConfigValue(configname.Value);
                if (importFolder != null)
                {
                    file1.PostedFile.SaveAs(importFolder +"/" + file1.FileName);
                    msg = "SUCCESS";
                    bdy.Attributes.Add("OnLoad", "window.opener.document.forms(0).item(\"" + Request.QueryString["fieldname"] + "\").value = \"" + file1.FileName + "\"; window.close();");
                }
                else
                {
                    msg = "<b>Import Folder<b> NOT Found in database: <br>&nbsp;&nbsp;&nbsp;Add '<b><i>" + configname.Value + "</i></b>' ConfigName to tblConfig";
                }
            }
            catch (Exception ex)
            {
                msg = "Error saving file" + file1.FileName + "<br>" + ex.ToString();
            }
            finally
            {
                message.InnerHtml = msg;
                tblMessage.Visible = true;
            }
        }
    }
}