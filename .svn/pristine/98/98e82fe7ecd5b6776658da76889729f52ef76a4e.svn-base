<div class="KT_textnav clearfix">
  <ul>
	<li class="first">
		<a href="<%
			If nav_pageNum > 0 Then
				Response.Write KT_addReplaceParam(nav_currentPage & "?" & nav_queryString, "pageNum_" & nav_rsName, 0)
			Else
				Response.Write "javascript: void(0);"
			End If
		%>"><%=NXT_getResource("First")%></a>
	</li>
	<li class="prev">
		<a href="<%
			If nav_pageNum > 0 Then
				Response.Write KT_addReplaceParam(nav_currentPage & "?" & nav_queryString, "pageNum_" & nav_rsName, KT_max(0, nav_pageNum - 1))
			Else
				Response.Write "javascript: void(0);"
			End If
		%>"><%=NXT_getResource("Previous")%></a>
	</li>
	<li class="next">
		<a href="<%
			If nav_pageNum < nav_totalPages Then
				Response.Write KT_addReplaceParam(nav_currentPage & "?" & nav_queryString, "pageNum_" & nav_rsName, KT_min(nav_totalPages, nav_pageNum + 1))
			Else
				Response.Write "javascript: void(0);"
			End If
		%>"><%=NXT_getResource("Next")%></a>
	</li>
	<li class="last">
		<a href="<%
			If nav_pageNum < nav_totalPages Then
				Response.Write KT_addReplaceParam(nav_currentPage & "?" & nav_queryString, "pageNum_" & nav_rsName, nav_totalPages)
			Else
				Response.Write "javascript: void(0);"
			End If
		%>"><%=NXT_getResource("Last")%></a>
	</li>
  </ul>
</div>
