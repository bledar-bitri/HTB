<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="Actions.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.Actions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
<script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
<title>HTB [ Inkassoakt - <asp:Literal ID="litKZCaption" runat="server" /> ]</title>
</head>
 
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
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
        <td><table width="100%" border="0" cellpadding="3" cellspacing="0">
          <tr>
            <td colspan="2" class="tblHeader">Aktion setzen  [AZ: <asp:Label ID="lblAktId" runat="server" />]</td>
            </tr>
          <tr>
            <td nowrap="nowrap" class="EditCaption">Datum /Zeit:&nbsp;</td>
            <td class="EditData"><%= DateTime.Now %>&nbsp;</td>
          </tr>
          <tr>
            <td class="EditCaption">Aktion:&nbsp;</td>
            <td class="EditData"><asp:Label ID="lblKZCaption" runat="server" /></td>
          </tr>
          <tr>
            <td colspan="2"><img name="" src="" width="1" height="1" alt=""/></td>
            </tr>
          <tr>
            <td class="EditCaptionTopLine">Bemerkung:&nbsp;</td>
            <td class="EditDataTopLine">
                <asp:TextBox ID="txtMemo" runat="server" TextMode="MultiLine" Columns="80" Rows="6" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
            </td>
          </tr>
          <tr>
            <td class="EditCaption">&nbsp;</td>
            <td class="EditData"><ctl:message ID="ctlMessage" runat="server" /></td>
          </tr>
          <tr>
            <td class="EditCaption">&nbsp;</td>
            <td class="EditData">&nbsp;<asp:Label ID= "lblDescription" runat="server"/></td>
          </tr>
          <tr>
            <td colspan="2"><img name="" src="" width="1" height="1" alt=""></td>
            </tr>
          <tr>
            <td class="tblFooter1">&nbsp;</td>
            <td class="tblFooter2"><div align="right">
              <asp:Button id="btnSubmit" runat="server" class="btnSave" text="Speichern" onclick="btnSubmit_Click" Enabled="true"/>
              <input name="Button" type="button" class="btnCancel" onclick="window.close();" value="Abbrechen" />
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
    <ctl:footer ID="footer1" runat="server" />
</body>
</html>
