<%
	If isEmpty(KT_Common__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/KT_common.asp")
	End If	

	If isEmpty(KT_tNGCore__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/tng/tNG.inc.asp")
	End If
	
	If isEmpty(KT_TOR__ALREADYLOADED) Then
		KT_TOR__ALREADYLOADED = True
		KT_LoadASPFiles Array("includes/tor/TOR_SetOrder.class.asp")
	End If
%>