<%
Class tNG_FileDelete
	Public tNG
	Public dbFieldName
	Public folder
	Public rename
	Public renameRule

	Private Sub Class_Initialize()
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (ByRef tNG__param)
		Set tNG = tNG__param
		dbFieldName = ""
		folder = ""
		rename = false
		renameRule = ""
	End Sub

	Public Sub setDbFieldName (dbFieldName__param)
		dbFieldName = dbFieldName__param
	End Sub	

	Public Sub setFolder (folder__param)
		folder = folder__param
	End Sub	

	Public Sub setRenameRule (renameRule__param)
		rename = true
		renameRule = renameRule__param
	End Sub	

	Public Sub deleteThumbnails(folder, oldName)
		If oldName <> "" Then
			Set path_info = KT_pathinfo(oldName)
			regexp_str = KT_preg_quote (path_info("filename")) & "_\d+x\d+"
			If path_info("extension") <> "" Then
				regexp_str = regexp_str & KT_preg_quote(path_info("dot") & path_info("extension"))
			End If
			Set folderObj = new KT_folder
			Set entry = folderObj.readFolder (folder, false)
			If not folderObj.hasError() Then
				Set fso = Server.CreateObject("Scripting.FileSystemObject")
				For each fl_i in entry("files")
					fname  = entry("files")(fl_i)("name") 
					If KT_preg_test(regexp_str, fname) Then
						On Error resume Next
						fso.DeleteFile folder & fname
						On Error GoTo 0
					End If
				Next
				Set fso  = nothing
			End If
		End If
	End Sub

	Public Function Execute()
		Set ret = nothing
		folder = tNG_DynamicData(folder, tNG, null, true, null, null)
		folder = KT_makeIncludedPath(folder)
		folder = Server.MapPath(folder) & "\"
		If rename = false and dbFieldName <> "" Then
			fileName = tNG.getSavedValue(dbFieldName)
		Else
			fileName = tNG_DynamicData(renameRule, tNG, null, true, null, null)	
		End If
		
		If fileName <> "" Then
			fullFileName = folder & fileName
			Set fso = Server.CreateObject("Scripting.FileSystemObject")
			If fso.FileExists(fullFileName) Then
				On Error Resume Next
				fso.DeleteFile fullFileName
				If err.Number <> 0 Then
					Set ret = new tNG_error
					ret.Init "FILE_DEL_ERROR", array(), array(fullFileName)
					If dbFieldName <> "" Then
						ret.setFieldError dbFieldName, "FILE_DEL_ERROR_D", array(fullFileName)
					End If
				End If
				deleteThumbnails folder & "thumbnails/", fileName
				On Error GoTo 0
			End If
		End If

		Set Execute = ret
	End Function

End Class
%>