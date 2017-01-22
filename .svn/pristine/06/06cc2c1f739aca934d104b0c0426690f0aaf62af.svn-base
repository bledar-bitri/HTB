<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlTelAndEmailCollection.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlTelAndEmailCollection" %>
<%@ Register TagPrefix="ctl" TagName="message" Src="~/v2/intranetx/global_files/CtlMessage.ascx" %>

<table width="100%" border="0" cellspacing="0" cellpadding="1">
    <tr>
        <td bgcolor="#000000">
            <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table  width="100%" border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td colspan="2" class="tblHeader" title="Telefon und Email">
                                    Telefon und Email
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tblDataAll">
                                    <ctl:message ID="ctlMessage" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">Telefon:</div>
                                </td>
                                <td class="EditData">
                                    (
                                    <asp:TextBox runat="server" ID="txtPhoneCity" class="docText" size="10" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                    )
                                    <asp:TextBox runat="server" ID="txtPhone" class="docText" size="45" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                    &nbsp;&nbsp;<asp:CheckBox runat="server" ID="chkNoPhone"  Text="keine Telefonnummer" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" class="EditCaption">
                                     <div align="right">E-Mail:</div>
                                </td>
                                <td class="EditData">
                                   <asp:TextBox runat="server" ID="txtEmail" class="docText" size="50" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                   &nbsp;&nbsp;<asp:CheckBox runat="server" ID="chkNoEmail"  Text="keine Emailadresse" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>