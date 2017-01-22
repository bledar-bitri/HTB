<%
Class KT_FileUpload
	Private fileInfo				'UploadedFile instance	-	the file upload information
	Private destFolderPath			'string		-	destination destFolderPath for upload
	Private isRequired				'bollean	-	specifies if the file is required for upload or not
	Private allowedExtensions		'array		-	the allowed types for upload
	Private autoRename				'boolean	-	specifies if autorename can be done if a file with the same name already exists in dest destFolderPath
	Private minSize					'int		-	minimum allowed size of the uploaded file
	Private maxSize					'int		-	maximum allowed size of the uploaded file
	Private destinationName			'string		-	the name under which the file was saved after upload
	Private userErrorMessage		'array		-	error message to be displayed as User Error
	Private develErrorMessage		'array		-	error message to be displayed as Developer Error
	
	Private uploadingPhaseError		'boolean	-	specifies if an error occured during the first phase (uploading phase)
	Private fileInfoExists			'boolean 	- 	specifies if info about this uploaded file exists in fileInfo object
	Private fileExists				'boolean	- 	specifies if Length in fileInfo is > 0
	Private fileInputName
	
	Private Sub Class_Initialize()
		destFolderPath		= ""
		isRequired	 		= false
		allowedExtensions	= Array()
		autoRename 			= false
		minSize				= -1
		maxSize				= -1
		userErrorMessage	= Array()
		develErrorMessage	= Array()
		
		fileInfoExists 		= false
		fileExists			= false			
		If Session("KT_FileUpload__Error_U")<>"" OR Session("KT_FileUpload__Error_D")<>"" Then
			uploadingPhaseError = True
			userErrorMessage  = Array(Session("KT_FileUpload__Error_U"))
			develErrorMessage = Array(Session("KT_FileUpload__Error_D"))
		Else
			uploadingPhaseError = False
		End If	
	End Sub


	Private Sub Class_terminate()
	
	End Sub
	
	Public Sub setFileInfo (fileInputName__param)
		fileInputName = fileInputName__param
		If uploadingPhaseError Then
			Exit Sub
		End If
		
		If isObject(Session("KT_FileUpload__UploadedFiles")) Then
			If Session("KT_FileUpload__UploadedFiles").Exists(fileInputName) Then
				Set fileInfo = Session("KT_FileUpload__UploadedFiles")(fileInputName)
				fileInfoExists = true
				If Clng("0" & fileInfo("Length")) > 0 Then
					fileExists = true
				End If
			End If
		End If
	End Sub

	
	
	Public Sub setFolder (folderPath)
		destFolderPath = folderPath
	End Sub

	Public Sub setRequired (isReq)
		isRequired = isReq
	End Sub
	
	Public Sub setAllowedExtensions (allowedExtensions__param)
		If isArray(allowedExtensions__param) Then
			allowedExtensions = allowedExtensions__param
		End If
	End Sub
	
	Public Sub setAutoRename (autoRename__param)
		autoRename = autoRename__param
	End Sub	
	
	Public Sub setMinSize (minSize__param)
		minSize = minSize__param
	End Sub	
	
	Public Sub setMaxSize (maxSize__param)
		maxSize = maxSize__param
	End Sub	


	Public Function uploadFile (fileName, oldFName)
		If Not KT_isSet(oldFName) Then
			oldFName = ""			
		End If
		uploadFile = null
		If uploadingPhaseError Then
			Exit Function
		End If	
		checkUpload
		checkFolder
		checkSize
		checkExtensions
		
		If hasError Then
			Exit Function
		End If
		
		If fileExists Then
			destinationName = fileName 'fileInfo("FileName")
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			If fso.FileExists (destFolderPath & destinationName) Then
				If lcase(destinationName) <> lcase(oldFName) Then
					If not autorename Then
						setError "UPLOAD_FILE_EXISTS",  Array(), Array(destinationName) 
						Exit Function	 
					Else
						destinationName = getUniqueName(destinationName)
					End If
				End If
			End If
			
			If oldFName <> "" Then
				On Error Resume Next
				fso.DeleteFile (destFolderPath & oldFName)
				On Error GoTo 0
			End If		
			
			On Error resume next
			fso.MoveFile fileInfo("Path"), destFolderPath & destinationName 
			'fso.CopyFile fileInfo("Path"), destFolderPath & destinationName , True
			If err.number <> 0 Then
				error = Err.Description
				setError "ASP_UPLOAD_MOVE_TMP_ERROR", Array(), Array(destFolderPath, error) 				
				Exit Function			 
			End If
			On Error GoTo 0
			Set fso = nothing
			uploadFile = destinationName
		End If		
	End Function
	
	Private Function getUniqueName(fName)
		extPart = ""
		filePart  = fName
		getUniqueName = "TempUniqueName"	' default
		If fName <> "" Then
			If instrrev(fName, ".") <> 0 Then
				extPart = mid(fName, instrrev(fName,"."))
				filePart = mid(fName, 1, instrRev(fName, ".")-1)
			End If
			
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			Dim i: i = 1
			getUniqueName = filePart & "_" & i & extPart
			do while fso.FileExists (destFolderPath & getUniqueName)
				If i=1000 Then  ' we stop at 1000
					getUniqueName = "TempUniqueName"	' default
					setError "ASP_UPLOAD_MAX_AUTORENAME", Array(), Array(1000) 						
					Exit Do
				End If
				i = i + 1
				getUniqueName = filePart & "_" & i & extPart
			loop
			Set fso = nothing
		End If
	End Function 	
	

	Private Sub checkUpload 
		If fileInfoExists Then
			If fileExists Then
				' check if the file really exists on the disk
				' it is speciefied in fileInfo structure .. but maybe it couldn't be saved
				Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
				If not fso.FileExists(fileInfo("Path")) then
					' probably this error will never be raised
					setError "ASP_UPLOAD_MISSING_FILE", array(), array(fileInfo("FileName"))
					fileExists = false
				End If
				Set fso = nothing	
			End If	
		End If
		
		If isRequired Then
			If Not fileInfoExists Then
				setError "ASP_UPLOAD_INVALID_FIELDNAME", Array(), Array(fileInputName) 
			Else
				If Not fileExists Then
					' required but missing
					setError "ASP_UPLOAD_FILE_REQUIRED", Array(), Array(fileInputName) 
				End If
			End If
		End If
	End Sub
	

	Private sub checkFolder
		If fileExists Then
			Dim fld: Set fld = new KT_Folder
			fld.createFolder destFolderPath
			
			If fld.hasError Then
				errors = fld.getError
				setError "%s", Array(errors(0)), Array(errors(1))
			End If			  
			Set fld = nothing
		End If	
	End Sub
	
	
	Private Sub checkSize
		If fileExists Then
			If minSize > 0 and Clng(fileInfo("Length")) < (minSize * 1024) Then
				setError "UPLOAD_CHECK_SIZE_S", Array(minSize), Array(minSize)
			End If
			If maxSize > 0 and Clng(fileInfo("Length")) > (maxSize * 1024) Then
				setError "UPLOAD_CHECK_SIZE_G", Array(maxSize), Array(maxSize)
			End If				
		End If	
	End Sub	
	
	
	Private Sub checkExtensions 
		If fileExists Then
			fileExt = fileInfo("FileExtension")
			If not KT_in_array(fileExt, allowedExtensions, false) Then
				setError "UPLOAD_EXT_NOT_ALLOWED", Array(), Array(fileExt)
			End If
		End If
	End Sub
	
	
	Public Sub Rollback()
		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		On Error Resume Next
		fso.DeleteFile destFolderPath & destinationName, True
		On Error GoTo 0
		Set fso = Nothing
	End Sub
		
	Private Sub setError (errorCode, arrArgsUsr, arrArgsDev)
		errorCodeDev = errorCode
		If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
			errorCodeDev = errorCodeDev & "_D"
		End If
		If errorCode <> "" Then
			userErrorMessage = KT_array_push (userErrorMessage, KT_getResource(errorCode, "FileUpload", arrArgsUsr))
		Else
			userErrorMessage = array()
		End If
		
		If errorCodeDev <> "" Then
			develErrorMessage = KT_array_push (develErrorMessage, KT_getResource(errorCodeDev, "FileUpload", arrArgsDev))
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
