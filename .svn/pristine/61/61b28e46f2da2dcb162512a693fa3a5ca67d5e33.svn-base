using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlMessage : System.Web.UI.UserControl
    {
        private const int MESSAGE_TYPE_ERROR = 0;
        private const int MESSAGE_TYPE_SUCCESS = 1;
        private const int MESSAGE_TYPE_INFO = 2;

        #region show message
        public void ShowInfo(String message)
        {
            ShowMessage(message, MESSAGE_TYPE_INFO);
        }
        public void ShowError(String message)
        {
            ShowMessage(message, MESSAGE_TYPE_ERROR);
        }
        public void ShowSuccess(String message)
        {
            ShowMessage(message, MESSAGE_TYPE_SUCCESS);
        }
        public void ShowException(Exception e)
        {
            ShowException(e, false);
        }
        public void ShowException(Exception e, bool isInnerException)
        {
            if (isInnerException)
                AppendError(e.Message);
            else
                ShowError(e.Message);

            AppendError("<br/>");
            AppendError(e.StackTrace.Replace(Environment.NewLine, "<br/>"));
            if (e.InnerException != null)
            {
                AppendError("<br/><br/>Inner Exception:<br/><br/>");
                ShowException(e.InnerException, true);
            }
        }
        
        #endregion

        #region append to messages
        public void AppendInfoLine(String message)
        {
            AppendInfo(message + "<br/>");
        }
        public void AppendInfo(String message)
        {
            ShowMessage(message, MESSAGE_TYPE_INFO, true);
        }
        public void AppendError(String message)
        {
            ShowMessage(message, MESSAGE_TYPE_ERROR, true);
        }
        public void AppendSuccess(String message)
        {
            ShowMessage(message, MESSAGE_TYPE_SUCCESS, true);
        }
        #endregion

        private void ShowMessage(string message, int messageType)
        {
            ShowMessage(message, messageType, false);
        }
        private void ShowMessage(string message, int messageType, bool append)
        {

            switch (messageType)
            {
                case MESSAGE_TYPE_ERROR:
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.BackColor = Color.OldLace;
                    break;
                case MESSAGE_TYPE_SUCCESS:
                    lblMessage.ForeColor = Color.Green;
                    lblMessage.BackColor = Color.MintCream;
                    break;
                case MESSAGE_TYPE_INFO:
                    lblMessage.ForeColor = Color.Black;
                    lblMessage.BackColor = Color.Ivory;
                    break;
            }
            if (append)
            {
                lblMessage.Text += message;
            }
            else
            {
                lblMessage.Text = message;
            }
        }

        public void Clear()
        {
            lblMessage.Text = "";
            lblMessage.ForeColor = Color.Black;
            lblMessage.BackColor = Color.White;
        }
    }
}