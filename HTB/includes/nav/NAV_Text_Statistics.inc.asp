<script type="text/javascript">
	$NAV_Text_start = <% If nav_totalRows=0 Then Response.write "0" Else Response.write (nav_pageNum * nav_maxRows + 1) End If%>;
</script>
 
 - 
<% If nav_totalRows=0 Then Response.write "0" Else Response.write (nav_pageNum * nav_maxRows + 1) End If%>&nbsp;<%=NXT_getResource("to")%>&nbsp;<%=KT_min(nav_pageNum * nav_maxRows + nav_maxRows, nav_totalRows)%>&nbsp;<%=NXT_getResource("of")%>&nbsp;<%=nav_totalRows%>
