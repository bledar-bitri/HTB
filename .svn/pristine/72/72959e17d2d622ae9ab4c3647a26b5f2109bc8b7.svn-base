<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlLookupAuftraggeber.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlLookupAuftraggeber" %>

<script type="text/javascript">
    function OnAuftraggeberSelected(source, eventArgs) {
        var results = eval('(' + eventArgs.get_value() + ')');
        $get('<%= ComponentName %>_txtAuftraggeberSearch').value = results.AuftraggeberName1 + " " + results.AuftraggeberName2;
        if (results.AuftraggeberID != null)
            $get('<%= ComponentName %>_hdnAuftraggeberId').value = results.AuftraggeberID;
        if (results.AuftraggeberStrasse != null)
            $get('<%= ComponentName %>_lblAuftraggeberAddress').innerHTML = "<strong>Addresse</strong>: " + results.AuftraggeberStrasse + ', ' + results.AuftraggeberPLZ + ', ' + results.AuftraggeberOrt;

        $get($get('<%= ComponentName %>_hdnNextFocusId').value).focus();
    }
    </script>

<asp:TextBox ID="txtAuftraggeberSearch" runat="server" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor=''" size="80" />
<br />
<asp:Label ID="lblAuftraggeberAddress" runat="server" />

<ajax:AutoCompleteExtender 
    ID="contactExtender" 
    runat="server" 
    TargetControlID="txtAuftraggeberSearch" 
    ServicePath="~/v2/intranetx/WS/WsLookup.asmx" 
    ServiceMethod="GetAuftraggeberInfo"
    MinimumPrefixLength="3"
    ContextKey="1234" 
    UseContextKey="True" 
    OnClientItemSelected="OnAuftraggeberSelected" 
    CompletionListCssClass="completionList" 
    CompletionListHighlightedItemCssClass="itemHighlighted"
    CompletionListItemCssClass="listItem"
>
</ajax:AutoCompleteExtender>

<asp:HiddenField ID="hdnAuftraggeberId" runat="server" />
<asp:HiddenField ID="hdnNextFocusId" runat="server" />

