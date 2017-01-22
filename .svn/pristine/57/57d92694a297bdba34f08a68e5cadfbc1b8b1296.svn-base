<%
	If isEmpty(KT_Common__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/KT_common.asp")
	End If	

	If isEmpty(KT_tNGCore__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/tng/tNG.inc.asp")
	End If
	
	If isEmpty(KT_TSO__ALREADYLOADED) Then
		KT_TSO__ALREADYLOADED = True
		KT_LoadASPFiles Array("includes/tso/TSO_TableSorter.class.asp")
	End If
%>