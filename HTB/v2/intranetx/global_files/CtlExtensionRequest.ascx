<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlExtensionRequest.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlExtensionRequest" %>

<table width="100%" border="0" cellspacing="0" cellpadding="1">
    <tr>
        <td bgcolor="#000000">
            <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table  width="100%" border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td colspan="2" class="tblHeader" title="RATENVEREINBARUNG">
                                    Verl&auml;ngerungsanfrage
                                </td>
                            </tr>
                            <tr id="trMessage" runat="server" visible="false">
                                <td  colspan="2" class="tblDataLeftBottomRight">
                                    <asp:Label ID="lblMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption"  width="145">
                                    <div align="right">Zeit:</div>
                                </td>
                                <td class="EditData">
                                    <asp:DropDownList ID="ddlExtensionDays" runat="server">
                                        <asp:ListItem Value="1" Text="1 Woche" />
                                        <asp:ListItem Value="2" Text="2 Wochen" />
                                        <asp:ListItem Value="3" Text="3 Wochen" />
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblExtensionDays" visible="false"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">Grund:</div>
                                </td>
                                <td class="EditData">
                                   <asp:TextBox runat="server" ID="txtExtensionRequestReason" class="docText" size="80%" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                   <asp:Label runat="server" ID="lblExtensionRequestReason"  visible="false"/>
                                </td>
                            </tr>
                            <tr runat="server" id="trExtensionRequestDate">
                                <td class="EditCaption">
                                    <div align="right">Anfrage Datum:</div>
                                </td>
                                <td class="EditData">
                                   <asp:Label runat="server" ID="lblExtensionRequestDate"/>
                                </td>
                            </tr>
                            <tr runat="server" id="trStatus">
                                <td class="EditCaption">
                                    <div align="right">Status:</div>
                                </td>
                                <td class="EditData">
                                   <asp:Label runat="server" ID="lblExtensionRequestStatus"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>