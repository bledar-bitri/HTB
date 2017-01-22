<%
Class KT_Folder
	Private userErrorMessage		'array		-	error message to be displayed as User Error
	Private develErrorMessage		'array		-	error message to be displayed as Developer Error
	

	Private Sub Class_Initialize()
		userErrorMessage	= Array()
		develErrorMessage	= Array()
	End Sub

	Private Sub Class_terminate()
	End Sub

	Public Function checkRights(folder, rights)
		ret = false
		Select case lcase(rights)
			case "read"
				ret = is_readable(folder)
			case "write"
				ret = is_writable(folder)
		End Select
		checkRights = ret
	End Function
	
	Public Function is_readable(folder)
		' assumes folder exists
		is_readable = true
		On Error Resume Next
		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		Dim objFolder: Set objFolder = fso.GetFolder(folder)
		If err.Number <> 0 Then
			is_readable = false
		End If	
		On Error GoTo 0
		Set objFolder = nothing
		Set fso = nothing			
	End Function
			
		
	Public Function is_writable(folder)
		' assumes folder exists and the check is only for rights
		is_writable = true
		
		test_folder = folder
		test_folder = replace(test_folder, "/", "\")
		If right(test_folder, 1) <> "\" Then
			test_folder = test_folder & "\"
		End If
		
		On Error Resume Next
		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		filename = test_folder & fso.GetTempName
		Set f = fso.OpenTextFile(filename, 2, True)
		f.Write "write test"
		f.Close
		fso.DeleteFile filename, True
		If err.Number <> 0 Then
			is_writable = false
		End If	
		On Error GoTo 0
		Set f = nothing
		Set fso = nothing
	End Function


	Public Sub createFolder(path)
		If path <> "" Then
			' windows-like path separator
			path = replace(path, "/", "\")
			If right(path,1) <> "\" Then
				path = path & "\"
			End If
		Else
			setError "ASP_FOLDER_NO_PATH", array(), array()
			Exit Sub
		End If

		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		If not fso.FolderExists(path) Then
			' check path type
			regExp_DriveTest = "^[A-Za-z]:(\\[^\\:\*\?\""<>|]+)*\\$"
			pathDriveTest = KT_preg_test (regExp_DriveTest, path)
							
			regExp_ShareTest = "^\\\\([A-Za-z_0-9]+|(\d{1,3}\.){3}\d{1,3})(\\[^\\:\*\?\""<>|]+)+\\$"
			pathShareTest = KT_preg_test (regExp_ShareTest, path)

			If pathDriveTest Then	' path like D:\FirstFolder\SecondFolder\
				rightpart = mid (path,instr (path, ":\") + 2)
				leftpart = left (path,instr (path, ":\") + 1) 
	
				On Error resume next
				ready = false
				Do while not ready
					If not fso.FolderExists(leftpart) Then
						fso.CreateFolder(leftpart)
					End If
					If err.number <> 0 Then	
						error = err.Description
						setError "ASP_FOLDER_CREATE_ERR", array(), array(path, error)
						err.clear
						Exit do					
					End If
					If rightpart <> "" Then			
						leftpart = leftpart & left(rightpart, instr(rightpart,"\"))
						rightpart = mid (rightpart, instr(rightpart,"\")+1)
					Else
						ready = true
					End If
				Loop
				On Error GoTo 0
			' end path like D:\FirstFolder\SecondFolder\
			
			ElseIf pathShareTest Then	 ' path like \\machinename\share\folder1\ or \\machineip\share\folder1\
				leftpart = "\\"
				rightpart = mid (path, 3)				
				
				leftpart  = leftpart & left (rightpart, instr(rightpart, "\"))  'machine name
				rightpart =	mid (rightpart, instr(rightpart, "\") + 1)					

				leftpart  = leftpart & left (rightpart, instr(rightpart, "\"))  ' share
				rightpart =	mid (rightpart, instr(rightpart, "\") + 1)					
				
				On Error resume next
				ready = false
				Do while not ready
					If not fso.FolderExists(leftpart) Then
						fso.CreateFolder(leftpart)
					End If
					If err.number <> 0 Then	
						error = err.Description
						setError "ASP_FOLDER_CREATE_ERR", array(), array(path, error)
						err.clear
						Exit do					
					End If
					If rightpart <> "" Then			
						leftpart = leftpart & left(rightpart, instr(rightpart,"\"))
						rightpart = mid (rightpart, instr(rightpart,"\")+1)
					Else
						ready = true
					End If
				Loop
				On Error GoTo 0
			' end  path like \\machinename\share\folder1\ or \\machineip\share\folder1\					
			Else
				setError "ASP_FOLDER_INVALID_PATH", Array(), Array(path)
			End If
		End If
		Set fso = nothing	
	End Sub



	Public Function readFolder(path, details)
		If path = "" Then
			setError "ASP_FOLDER_NO_PATH", array(), array()			
			Set readFolder = nothing
			Exit Function
		End If

		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		If fso.FolderExists(path) Then
			Dim objFolder: Set objFolder = fso.GetFolder(path)
			
			Dim fcol: Set fcol = Server.CreateObject("Scripting.Dictionary")   ' files/foldes collection
			Set fcol("files") = Server.CreateObject("Scripting.Dictionary")
			Set fcol("folders") = Server.CreateObject("Scripting.Dictionary")
			
			On Error Resume Next
			Dim i: i=0
			For Each objFile in objFolder.Files
				Set fcol("files")(i) = Server.CreateObject("Scripting.Dictionary")
				fcol("files")(i)("name") = objFile.Name
				If details Then
					fcol("files")(i)("size") = objFile.Size
					fcol("files")(i)("path") = objFile.Path
				End If	
				i = i + 1 
			Next	
						
			i=0
			For Each objSubFolder in objFolder.SubFolders
				Set fcol("folders")(i) = Server.CreateObject("Scripting.Dictionary")
				fcol("folders")(i)("name") = objSubFolder.Name
				If details Then
					fcol("folders")(i)("size") = 0 'objSubFolder.Size
					fcol("folders")(i)("path") = objSubFolder.Path
				End If	
				i = i + 1 
			Next				
			If err.Number <> 0 Then
				fcol("files").removeAll()
				fcol("folders").removeAll()
			End If
			On Error GoTo 0

			Set readFolder = fcol		
			Set objFolder = nothing
		Else
			' the folder does not exist
			setError  "ASP_FOLDER_MISSING", Array(), Array(path)
			Set readFolder = nothing
		End If
		Set fso = nothing	
	End Function	
	
	

	Public Sub deleteFolder(path)
		If path <> "" Then
			' windows-like path separator
			path = replace(path, "/", "\")
			If right(path,1) = "\" Then
				path = left(path, len(path)-1)
			End If
		Else
			setError "ASP_FOLDER_NO_PATH", array(), array()			
			Exit Sub
		End If
		

		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		If fso.FolderExists (path) Then
			On Error Resume Next
			fso.DeleteFolder path, True
			If err.Number <> 0 Then
				error = err.Description
				setError  "ASP_FOLDER_DELETE_ERR", Array(), Array(path, error)
			End If
			On Error GoTo 0	
		'Else
			' the folder does not exist - DO nothing
		'	setError  "FOLDER_MISSING", Array(), Array(path)
		End If
		Set fso = nothing	
	End Sub	
	
	
			
	Private Sub setError (errorCode, arrArgsUsr, arrArgsDev)
		errorCodeDev = errorCode
		If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
			errorCodeDev = errorCodeDev & "_D"
		End If
		If errorCode <> "" Then
			userErrorMessage = KT_array_push (userErrorMessage, KT_getResource(errorCode, "Folder", arrArgsUsr))
		Else
			userErrorMessage = array()
		End If
		
		If errorCodeDev <> "" Then
			develErrorMessage = KT_array_push (develErrorMessage, KT_getResource(errorCodeDev, "Folder", arrArgsDev))
		Else
			develErrorMessage = array()
		End If			
	End Sub

	Public Function hasError
		If ubound(userErrorMessage) > -1 Then
			hasError = True
		Else
			hasError = False	
		End If
	End Function
	

	Public Function getError
		getError = Array ( join(userErrorMessage,"<br />"), join (develErrorMessage, "<br />"))
	End Function

End Class
%>
