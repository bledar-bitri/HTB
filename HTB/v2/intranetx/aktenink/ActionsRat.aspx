<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="ActionsRat.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.ActionsRat" %>
<%@ Register TagPrefix="ctl" TagName="installment" Src="~/v2/intranetx/global_files/CtlInstallment.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
<style type="text/css"> 
body {
	margin-left: 5px;
	margin-top: 5px;
	margin-right: 5px;
	margin-bottom: 5px;
	background-image: url(../../intranet/images/osxback.gif);
}
a:link {
	color: #CC0000;
}
a:visited {
	color: #CC0000;
}
a:hover {
	color: #CC0000;
}
a:active {
	color: #CC0000;
}
</style>
<script language="JavaScript" type="text/JavaScript"> 
<!--
    function MM_goToURL() { //v3.0
        var i, args = MM_goToURL.arguments; document.MM_returnValue = false;
        for (i = 0; i < (args.length - 1); i += 2) eval(args[i] + ".location='" + args[i + 1] + "'");
    }

//-->
</script>
<script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
 
<title>HTB.ASP [ Inkassoakt - Ratenvereinbarung ]</title>
</head>
 
<body>
<ctl:headerNoMenu ID="header" runat="server" />

<form  id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true"  EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
    <ContentTemplate>
        <table width="100%" border="0" cellspacing="0" cellpadding="1">
  <tr>
    <td bgcolor="#000000"><table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
      <tr>
        <td>
        <table width="100%" border="0" cellpadding="3" cellspacing="0">
          <tr>
            <td colspan="2" class="tblHeader">Aktion setzen  [AZ: <%=Request["ID"]%>]</td>
          </tr>
          <tr>
            <td colspan="2">
                <ctl:message ID="ctlMessage" runat="server"/>
            </td>
          </tr>
          <tr id="trExtra" runat="server">
            <td colspan="2">
                <ctl:installment ID="ctlInstallment" runat="server" />
            </td>
          </tr>
          <tr>
            <td class="tblFooter1">&nbsp;</td>
            <td class="tblFooter2"><div align="right">
                <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Speichern" onclick="btnSubmit_Click" />
                <input name="Button" type="button" class="btnCancel" onclick="window.opener.document.location.reload(); window.close();" value="Abbrechen" />
            </div></td>
            </tr>
        </table>
          </td>
      </tr>
    </table></td>
  </tr>
</table>
    </ContentTemplate>
    </asp:UpdatePanel>
</form>
<br />
<ctl:footer ID="ftr" runat="server" />
</body>
</html>
