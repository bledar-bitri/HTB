<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlLookupKlient.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlLookupKlient" %>

<script type="text/javascript">
    function OnKlientSelected(source, eventArgs) {
        var results = eval('(' + eventArgs.get_value() + ')');
        $get('<%= ComponentName %>_txtKlientSearch').value = results.KlientName1 + " " + results.KlientName2;
        if (results.KlientID != null)
            $get('<%= ComponentName %>_hdnKlientId').value = results.KlientID;
        if (results.KlientOldID != null)
            $get('<%= ComponentName %>_hdnKlientOldId').value = results.KlientOldID;
        if (results.KlientStrasse != null)
            $get('<%= ComponentName %>_lblKlientAddress').innerHTML = "<strong>Addresse</strong>: " + results.KlientStrasse + ', ' + results.KlientPLZ + ', ' + results.KlientOrt;

        $get($get('<%= ComponentName %>_hdnNextFocusId').value).focus();
    }
    </script>

<asp:TextBox ID="txtKlientSearch" runat="server" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor=''" size="80" />
<br />
<asp:Label ID="lblKlientAddress" runat="server" />

<ajax:AutoCompleteExtender 
    ID="contactExtender" 
    runat="server" 
    TargetControlID="txtKlientSearch" 
    ServicePath="~/v2/intranetx/WS/WsLookup.asmx" 
    ServiceMethod="GetKlientInfo"
    MinimumPrefixLength="3"
    ContextKey="1234" 
    UseContextKey="True" 
    OnClientItemSelected="OnKlientSelected" 
    CompletionListCssClass="completionList" 
    CompletionListHighlightedItemCssClass="itemHighlighted"
    CompletionListItemCssClass="listItem"
>
</ajax:AutoCompleteExtender>

<asp:HiddenField ID="hdnKlientId" runat="server" />
<asp:HiddenField ID="hdnKlientOldId" runat="server" />
<asp:HiddenField ID="hdnNextFocusId" runat="server" />

