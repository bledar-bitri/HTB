<%
	If IsEmpty(KT_ResourcesFunctions__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/lib/resources/KT_Resources.asp")
	End If	

	If isEmpty(KT_Email__ALREADYLOADED) Then
		KT_Email__ALREADYLOADED = True
		KT_LoadASPFiles Array("includes/common/lib/email/KT_Email.class.asp")
	End If
%>