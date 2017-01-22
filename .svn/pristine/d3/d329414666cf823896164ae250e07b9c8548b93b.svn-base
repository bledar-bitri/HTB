<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlLookupDealer.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlLookupDealer" %>

<script type="text/javascript">
    function OnDealerSelected(source, eventArgs) {
        var results = eval('(' + eventArgs.get_value() + ')');
        $get('<%= ComponentName %>_txtDealerSearch').value = results.AutoDealerName;
        if (results.AutoDealerID != null)
            $get('<%= ComponentName %>_hdnDealerId').value = results.AutoDealerID;
        if (results.AutoDealerStrasse != null)
            $get('<%= ComponentName %>_lblDealerAddress').innerHTML = "<strong>Addresse</strong>: " + results.AutoDealerStrasse + ', ' + results.AutoDealerPLZ + ', ' + results.AutoDealerOrt;

        $get($get('<%= ComponentName %>_hdnNextFocusId').value).focus();
    }
    </script>

<asp:TextBox ID="txtDealerSearch" runat="server" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor=''" size="80" />
<br />
<asp:Label ID="lblDealerAddress" runat="server" />

<ajax:AutoCompleteExtender 
    ID="contactExtender" 
    runat="server" 
    TargetControlID="txtDealerSearch" 
    ServicePath="~/v2/intranetx/WS/WsLookup.asmx" 
    ServiceMethod="GetDealerInfo"
    MinimumPrefixLength="3"
    ContextKey="1234" 
    UseContextKey="True" 
    OnClientItemSelected="OnDealerSelected" 
    CompletionListCssClass="completionList" 
    CompletionListHighlightedItemCssClass="itemHighlighted"
    CompletionListItemCssClass="listItem"
>
</ajax:AutoCompleteExtender>

<asp:HiddenField ID="hdnDealerId" runat="server" />
<asp:HiddenField ID="hdnNextFocusId" runat="server" />

