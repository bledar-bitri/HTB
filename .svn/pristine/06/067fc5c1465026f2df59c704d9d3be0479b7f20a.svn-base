<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlLookupUser.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlLookupUser" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script type="text/javascript">
        function OnUserSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%= ComponentName %>_txtUserSearch').value = results.UserVorname + " " + results.UserNachname;
            if (results.UserID != null)
                $get('<%= ComponentName %>_hdnUserId').value = results.UserID;
            if (results.AbteilungCaption != null)
                $get('<%= ComponentName %>_lblUserDescription').innerHTML = "<strong>Abteilung</strong>: " + results.AbteilungCaption;

            $get($get('<%= ComponentName %>_hdnNextFocusId').value).focus();
        }
    </script>

<asp:TextBox ID="txtUserSearch" runat="server" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor=''" size="80" />
<br />
<asp:Label ID="lblUserDescription" runat="server" />

<ajax:AutoCompleteExtender 
    ID="contactExtender" 
    runat="server" 
    TargetControlID="txtUserSearch" 
    ServicePath="~/v2/intranetx/WS/WsLookup.asmx" 
    ServiceMethod="GetActiveUsersInfo"
    MinimumPrefixLength="1"
    ContextKey="1234" 
    UseContextKey="True" 
    OnClientItemSelected="OnUserSelected" 
    CompletionListCssClass="completionList" 
    CompletionListHighlightedItemCssClass="itemHighlighted"
    CompletionListItemCssClass="listItem"
>
</ajax:AutoCompleteExtender>

<asp:HiddenField ID="hdnUserId" runat="server" />
<asp:HiddenField ID="hdnNextFocusId" runat="server" />
