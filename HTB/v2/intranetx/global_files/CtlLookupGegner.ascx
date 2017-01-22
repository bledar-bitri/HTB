<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlLookupGegner.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlLookupGegner" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script type="text/javascript">
        function <%= ComponentName %>_OnGegnerSelected(source, eventArgs) {
            //debugger;
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%= ComponentName %>_txtGegnerSearch').value = results.GegnerName;
            if (results.GegnerID != null) {
                $get('<%= ComponentName %>_hdnGegnerId').value = results.GegnerID;
            }
            if (results.GegnerOldID != null) {
                $get('<%= ComponentName %>_hdnGegnerOldId').value = results.GegnerOldID;
            }
            if (results.GegnerLastStrasse != null) {
                $get('<%= ComponentName %>_lblGegnerAddress').innerHTML = "<strong>Addresse</strong>: " + results.GegnerLastStrasse + ', ' + results.GegnerLastOrt;
            }
            $get($get('<%= ComponentName %>_hdnNextFocusId').value).focus();
        }
    </script>

<asp:TextBox ID="txtGegnerSearch" runat="server" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor=''" size="80" />
<br />
<asp:Label ID="lblGegnerAddress" runat="server" />

<ajax:AutoCompleteExtender 
    ID="contactExtender" 
    runat="server" 
    TargetControlID="txtGegnerSearch" 
    ServicePath="~/v2/intranetx/WS/WsLookup.asmx" 
    ServiceMethod="GetGegnerInfoNew"
    MinimumPrefixLength="3"
    ContextKey="1234" 
    UseContextKey="True" 
    
    OnClientItemSelected="ctlLookupGegner_OnGegnerSelected"

    CompletionListCssClass="completionList" 
    CompletionListHighlightedItemCssClass="itemHighlighted"
    CompletionListItemCssClass="listItem"
>
</ajax:AutoCompleteExtender>

<asp:HiddenField ID="hdnGegnerId" runat="server" />
<asp:HiddenField ID="hdnGegnerOldId" runat="server" />
<asp:HiddenField ID="hdnNextFocusId" runat="server" />
