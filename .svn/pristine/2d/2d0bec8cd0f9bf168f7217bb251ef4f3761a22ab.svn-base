<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteMahnung.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.DeleteMahnung" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Mahnung L&ouml;schen</title>
     
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />

    <script language="JavaScript">
        function movesizeandopen() {
            self.moveTo(screen.width / 2 - 200, screen.height / 2 - 150)
            self.resizeTo(500, 500)
        }
    </script>
</head>

<body id="bdy" runat="server" bgcolor="#FFFFFF" text="#000000" background="../../intranet/images/osxback.gif" >
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <table width="100%" border="1" cellspacing="0" cellpadding="3" bgcolor="#FFFFFF">
        <tr> 
        <td> 
          <div align="center" class="docText">
            <p align="right">&nbsp;</p>
            <p>
            <asp:Label ID="lblQuestion" runat="server" /><br/>
              <br/>
              <b>Diese Mahnung [inkludiert Kosten und Aktion] werden gel&ouml;scht.</b>
              <br/>
              <br/>Sind Sie sicher, da&szlig; Sie das m&ouml;chten?<br/>
              &nbsp; </p>
              <ctl:message ID="cltMessage" runat="server"/>
          </div>
        </td>
      </tr>
      </table>
      <div align="center">
        <table width="100%" border="1" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
          <tr>
            <td><div align="center">
                <asp:Button ID="btnCancel" runat="server" Text="nein" class="btnCancel" onclick="btnCancel_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="ja" class="btnSave" onclick="btnSubmit_Click" />
              </div>
            </td>
          </tr>
        </table>
        <br />
        <br />
        &nbsp;
      </div>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
