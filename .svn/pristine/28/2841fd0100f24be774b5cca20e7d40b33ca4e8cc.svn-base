<%
	If IsEmpty(KT_ResourcesFunctions__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/lib/resources/KT_Resources.asp")
	End If

	If IsEmpty(KT_Folder__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/lib/folder/KT_Folder.asp")
	End If
	
	If isEmpty(KT_FileUpload__ALREADYLOADED) Then
		KT_FileUpload__ALREADYLOADED = True
		KT_LoadASPFiles Array("includes/common/lib/file_upload/KT_FileUpload.class.asp", "includes/common/lib/file_upload/KT_FileUploadFunctions.inc.asp")
	End If
%>
