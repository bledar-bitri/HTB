<%@ Page Language="C#"  Culture="de-DE" AutoEventWireup="true" CodeBehind="EditKlientCommunication.aspx.cs" Inherits="HTB.v2.intranetx.klienten.EditKlientCommunication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="litTitle" runat="server" /> </title>
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
        .style2 {	color: #FF0000;
	        font-weight: bold;
        }
    </style>
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="tblHeader">
                                        <asp:Label ID="lblHeader" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <ctl:message ID="ctlMessage" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditDataTopLine">
                                        <asp:TextBox runat="server" ID="txtMemo" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" TextMode="MultiLine" Columns="100" Rows="10"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblFooter1">
                                        <div align="right">
                                             <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Speichern" title="Speichern" OnClick="btnSubmit_Click" />
                                             <asp:Button ID="btnCancel2" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
