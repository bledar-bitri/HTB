<%
	If IsEmpty(KT_ResourcesFunctions__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/lib/resources/KT_Resources.asp")
	End If
	
	If IsEmpty(KT_Folder__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/lib/folder/KT_Folder.asp")
	End If

	If IsEmpty(KT_Shell__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/lib/shell/KT_Shell.asp")
	End If

	If isEmpty(KT_Image__ALREADYLOADED) Then
		KT_Image__ALREADYLOADED = True
		KT_LoadASPFiles Array("includes/common/lib/image/KT_Image.class.asp")
	End If
%>
