<%

Class tNG_FileUpload
	Public tNG	' object
	Public fieldName
	Public formFieldName
	Public dbFieldName
	Public folder
	Public maxSize
	Public allowedExtensions
	Public rename
	Public renameRule
	Public uploadedFileName
	Public dynamicFolder
	Public errObj

	Private Sub Class_Initialize()
		Set this = Me
	End Sub
	
	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (ByRef tNG__param)
		Set tNG = tNG__param
		formFieldName = ""
		dbFieldName = ""
		folder = ""
		maxSize = 0
		allowedExtensions = array()
		rename = "none"
		renameRule = ""
		uploadedFileName = ""
	 	dynamicFolder  = ""
		Set errObj = nothing
	End Sub

	'===========================================
	' Inheritance
	'===========================================
	Public this
	
	Public Sub SetContextObject(ByRef objContext)
		Set this = objContext
	End Sub
	'===========================================
	' End Inheritance
	'===========================================
	
	Public Sub setFormFieldName (formFieldName__param)
		formFieldName = formFieldName__param
	End Sub	

	Public Sub setDbFieldName (dbFieldName__param)
		dbFieldName = dbFieldName__param
	End Sub	

	Public Sub setFolder (folder__param)
		folder = folder__param
	End Sub	

	Public Sub setMaxSize (maxSize__param)
		maxSize = maxSize__param
	End Sub	
	
	Public Sub setAllowedExtensions (allowedExtensions__param)
		arrExtensions = split(allowedExtensions__param, ",")
		Dim i
		For i=0 to ubound(arrExtensions)
			arrExtensions(i) = trim(arrExtensions(i))
		Next
		allowedExtensions  = arrExtensions
	End Sub

	Public Sub setRename (rename__param)
		rename = rename__param
	End Sub	

	Public Sub setRenameRule (renameRule__param)
		renameRule = renameRule__param
	End Sub	

	
	Public Sub Rollback()
		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		On Error Resume Next
		fso.DeleteFile dynamicFolder & uploadedFileName, True
		On Error GoTo 0
		Set fso = Nothing
	End Sub

	
	Public Sub deleteThumbnails(folder, oldName)
	End Sub

	Public Function Execute() 
		Set ret = nothing
		If dbFieldName <> "" Then
			oldFileName = tNG.getSavedValue(dbFieldName)
			saveFileName  = tNG.getColumnValue(dbFieldName)
			If tNG.getColumnType(dbFieldName) <> "FILE_TYPE" Then
				Set ret = new tNG_error
				ret.Init "FILE_UPLOAD_WRONG_COLTYPE", array(), array(dbFieldName)
				ret.addFieldError dbFieldName, "FILE_UPLOAD_WRONG_COLTYPE_D", array(dbFieldName)
				Set errObj = ret
				Set Execute = ret
				Exit Function
			End If
		Else
			oldFileName = tNG_DynamicData(renameRule, tNG, null, true, null, null)
            If KT_isSet(tNG.multipleIdx) Then
                saveFileName = Request.Form(formFieldName & "_" & tNG.multipleIdx)    
            Else
                saveFileName = Request.Form(formFieldName)
            End If
		End If
		dynamicFolder = tNG_DynamicData(folder, tNG, null, false, null, null)
		dynamicFolder = KT_makeIncludedPath(dynamicFolder)
		
		autoRename = false	
		Select case LCase(rename)
			case "auto"
				autoRename = true
			case "none"
			case "custom"
				Set arrArgs = Server.CreateObject("Scripting.Dictionary")
				Set path_info = KT_pathinfo(saveFileName)
				arrArgs("KT_name") = path_info("filename")
				arrArgs("KT_ext") = path_info("extension")
				saveFileName = tNG_DynamicData(renameRule, tNG, null, false, arrArgs, null)
			case else
				Response.write "INTERNAL ERROR: Unknown upload rename method."
				Response.End()
		End Select
		' Upload File
		Set fileUpload = new KT_fileUpload
		If KT_isSet(tNG.multipleIdx) Then
			fileUpload.setFileInfo formFieldName & "_" & tNG.multipleIdx
		Else
			fileUpload.setFileInfo formFieldName
		End If
		fileUpload.setFolder Server.MapPath(dynamicFolder)
		fileUpload.setRequired false
		fileUpload.setMinSize 0
		fileUpload.setAllowedExtensions allowedExtensions
		fileUpload.setAutoRename autoRename
		fileUpload.setMaxSize maxSize
		uploadedFileName = fileUpload.uploadFile(saveFileName, oldFileName)

		updateDB = uploadedFileName
		If fileUpload.hasError() Then
			arrError = fileUpload.getError()
			Set errObj  = new tNG_error
			errObj.Init "FILE_UPLOAD_ERROR", array(arrError(0)), array(arrError(1))
			If dbFieldName <> "" Then
				errObj.addFieldError dbFieldName, "%s", array(arrError(0))
			End If
			Set ret = errObj
		Else
			dynamicFolder = Server.MapPath(dynamicFolder) & "\"
			If uploadedFileName = "" Then	
				Set arrArgs = Server.CreateObject("Scripting.Dictionary")
				If rename = "custom" Then
					Set path_info = KT_pathinfo(oldFileName)
					arrArgs("KT_name") = path_info("filename")
				End If
				tmpFileName = tNG_DynamicData(renameRule, tNG, null, false, arrArgs, null)
				If tmpFileName <> "" and oldFileName <> "" and tmpFileName <> oldFileName Then
					Set fso = Server.CreateObject("Scripting.FileSystemObject")
					If fso.FileExists(dynamicFolder & oldFileName) Then
						On Error Resume Next
						fso.MoveFile dynamicFolder & oldFileName, dynamicFolder & tmpFileName 
						uploadedFileName = tmpFileName
						updateDB = uploadedFileName
						If err.Number <> 0 Then
							Set ret = new tNG_error
							ret.Init "FILE_UPLOAD_RENAME", array(), array(dynamicFolder & oldFileName, dynamicFolder & tmpFileName )
						End If
						On Error GoTo 0

					End If
				End If	
			End If

			If Not KT_isSet(ret) Then
				If tNG.getTransactionType = "_insert"  Or tNG.getTransactionType = "_multipleInsert" Then
					tNG.registerTrigger Array("ERROR", "Trigger_Default_RollBack", 1, Me)
				End If
				this.deleteThumbnails dynamicFolder & "thumbnails\", oldFileName 
				If uploadedFileName <> "" Then
					this.deleteThumbnails dynamicFolder & "thumbnails\", uploadedFileName 
				End If
				If dbFieldName <> "" And uploadedFileName <> "" Then
					Set ret = tNG.afterUpdateField(dbFieldName, updateDB)
				End If
			End If
			If Not KT_isSet(ret) and dbFieldName <> "" Then
				tNG.setRawColumnValue dbFieldName, updateDB
			End If
		End If
		Set errObj = ret
		Set Execute = ret
	End Function
	
End Class	
	
%>