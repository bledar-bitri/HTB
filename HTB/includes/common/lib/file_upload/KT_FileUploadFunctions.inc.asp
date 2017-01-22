<%
	'KT_FileUpload__ 
	Dim KT_FileUpload__OlderFilesExpireTime: KT_FileUpload__OlderFilesExpireTime = 60 * 15 '  15 minutes

	If InStr(Request.ServerVariables("CONTENT_TYPE"), "multipart/form-data") Then
		Session.Contents.Remove("KT_FileUpload__Error_U") ' user error
		Session.Contents.Remove("KT_FileUpload__Error_D") ' developer error
		Session.Contents.Remove("KT_FileUpload__UploadedFiles")
		Session.Contents.Remove("KT_FileUpload__Resubmit")
		
		Dim KT_FileUpload__Handler: Set KT_FileUpload__Handler = new FreeASPUpload
		KT_FileUpload__Handler.Upload
		
		Dim KT_FileUpload__tempFolder: KT_FileUpload__tempFolder = KT_FileUpload__GetAbsolutePathToTempFolder()
		If KT_FileUpload__tempFolder <> "" Then
			' save the files from the stream
			On Error Resume Next
			KT_FileUpload__Handler.Save KT_FileUpload__tempFolder
			If err.number <> 0 Then
				KT_FileUpload__setError "ASP_UPLOAD_ERR_STREAM_WRITE", Array(), Array(KT_FileUpload__tempFolder) 
		
				' the files couldn't be saved into temp folder, so remove them from UploadedFiles dict
				KT_FileUpload__Handler.UploadedFiles.removeAll
			End If
			On Error GoTo 0
			
			' delete older temporary uploaded files
			KT_FileUpload__RemoveOlderTempFiles KT_FileUpload__tempFolder
		Else
			' the files couldn't be saved into temp folder, so remove them from UploadedFiles dict
			KT_FileUpload__Handler.UploadedFiles.removeAll
		End If			
		

		Session("KT_FileUpload__Resubmit") = True
		Set Session("KT_FileUpload__UploadedFiles") = KT_FileUpload__Handler.GetUploadedFilesAsDictionary
		
		' Resubmit form
		Response.AddHeader "Pragma", "No-Cache"
		Response.CacheControl = "no-cache"
		Response.Expires = -1
		
		Response.write "<html><head><title>KT_FileUpload Resubmit</title></head>" & vbNewLine
		Response.write "<body onload=""javascript: document.forms.KT_fileUploadForm.submit()"">"  & vbNewLine
		Response.write "<form action="""" method=""post"" name=""KT_fileUploadForm"" enctype=""application/x-www-form-urlencoded"">"  & vbNewLine
		' Form keys
		For each formKey in KT_FileUpload__Handler.FormElements
			For each multiKey in KT_FileUpload__Handler.FormElements(formKey)
				Response.write "<input type=""hidden"" name=""" & formKey & """ value=""" & Server.HTMLEncode(KT_FileUpload__Handler.FormElements(formKey)(multiKey)) & """ >" & vbNewLine
			Next
		Next
		' Files
		For each fileKey in KT_FileUpload__Handler.UploadedFiles
			Response.write "<input type=""hidden"" name=""" & fileKey & """ value=""" & Server.HTMLEncode(KT_FileUpload__Handler.UploadedFiles(fileKey).FileName) & """ >" & vbNewLine
		Next
		Response.write "<input name=""KT_FileUpload__Resubmit"" type=""hidden"" value=""True"">"
		Response.write "</form></body></html>"
		Response.End()
	Else
		If Request.Form("KT_FileUpload__Resubmit") = "True" Then
			If Session("KT_FileUpload__Resubmit") Then				
				Session("KT_FileUpload__Resubmit") = False
			Else
				Session.Contents.Remove("KT_FileUpload__Error_D")
				Session.Contents.Remove("KT_FileUpload__Error_U")
				Session.Contents.Remove("KT_FileUpload__UploadedFiles")
				Session.Contents.Remove("KT_FileUpload__Resubmit")			
				
				' refresh for the resubmit.. 
				Response.write "<html><head><title>KT_FileUpload Resubmit</title></head>" & vbNewLine
				Response.write "<body onload=""javascript: document.forms[0].submit()"">"  & vbNewLine
				Response.write "<form action="""" method=""post"" enctype=""application/x-www-form-urlencoded"">"  & vbNewLine
				Response.write "</form></body></html>"
				Response.End()
			End If	
		End If
	End If


'##########################################################################
' 	 Helper Functions 	

	' Save errors to Session
	Sub KT_FileUpload__setError (errorCode, arrArgsUsr, arrArgsDev)
		errorCodeDev = errorCode & "_D"
		
		Session("KT_FileUpload__Error_U") = KT_getResource(errorCode, "FileUpload", arrArgsUsr)
		Session("KT_FileUpload__Error_D") = KT_getResource(errorCodeDev, "FileUpload", arrArgsDev)
	End Sub


	' deletes older uploaded files from the temporary folder
	Private Sub KT_FileUpload__RemoveOlderTempFiles (pathToTempFolder)
		On error resume next
		Dim fso, fld, fc, f
		Set fso = Server.CreateObject("Scripting.FileSystemObject")

		Set fld = fso.GetFolder(pathToTempFolder)
		Set fc = fld.Files
		For Each f in fc
			DateCreated = Cdate(f.DateCreated)
			elapsed  = DateDiff("s", DateCreated, Now)

			If elapsed > KT_FileUpload__OlderFilesExpireTime Then
				f.Delete (true)
				If err.number<> 0 Then
					' do nothing if couldn't delete a file
					' missing full permissions on temp folder are caught somewhere else
					err.clear
					Exit For
				End If
			End If
		Next
		Set fso = nothing
		On error Goto 0
	End Sub
	

	Function KT_FileUpload__GetAbsolutePathToTempFolder()
		Dim absolutePathToRootFolder: absolutePathToRootFolder = KT_GetAbsolutePathToRootFolder()
		Dim relativePathToTempFolder: relativePathToTempFolder = "includes\common\lib\file_upload\Temp\"
		
		If absolutePathToRootFolder <> "" Then
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			On Error Resume Next
			If not fso.FolderExists(absolutePathToRootFolder & relativePathToTempFolder) Then
				fso.CreateFolder(absolutePathToRootFolder & relativePathToTempFolder)
			End If
			If Err.Number <> 0 Then
				KT_FileUpload__setError "ASP_UPLOAD_ERR_CREATE_TEMP", Array(), Array(absolutePathToRootFolder & relativePathToTempFolder)
				KT_FileUpload__GetAbsolutePathToTempFolder = ""
			Else
				KT_FileUpload__GetAbsolutePathToTempFolder = absolutePathToRootFolder & relativePathToTempFolder
			End If
			Set fso = nothing
			On Error GoTo 0
		Else
			KT_FileUpload__setError "ASP_UPLOAD_MISSING_TEMP", Array(), Array(absolutePathToRootFolder & relativePathToTempFolder) 
		End If
	End Function




'##########################################################################

' For examples, documentation, and your own free copy, go to:
' http://www.freeaspupload.net
' Note: You can copy and use this script for free and you can make changes
' to the code, but you cannot remove the above comment.

 Class FreeASPUpload
	  Public UploadedFiles
	  Public FormElements
	
	  Private VarArrayBinRequest
	  Private StreamRequest
	  Private uploadedYet
	
	  Private Sub Class_Initialize()
		Set UploadedFiles 	= Server.CreateObject("Scripting.Dictionary")
		Set FormElements 	= Server.CreateObject("Scripting.Dictionary")
		Set StreamRequest 	= Server.CreateObject("ADODB.Stream")
		StreamRequest.Type 	= 1 'adTypeBinary
		StreamRequest.Open
		uploadedYet = false
	  End Sub
	  
	  Private Sub Class_Terminate()
		If IsObject(UploadedFiles) Then
		  UploadedFiles.RemoveAll()
		  Set UploadedFiles = Nothing
		End If
		If IsObject(FormElements) Then
		  FormElements.RemoveAll()
		  Set FormElements = Nothing
		End If
		StreamRequest.Close
		Set StreamRequest = Nothing
	  End Sub
	
	  Public Function GetUploadedFilesAsDictionary()
	  		Dim dict: Set dict = Server.CreateObject("Scripting.Dictionary")
			For each f in UploadedFiles
				Set dict(f) = UploadedFiles(f).GetUploadedFileAsDictionary
			Next
			Set GetUploadedFilesAsDictionary = dict
	  End Function
	
	  Public Property Get Form(sIndex)
		Form = ""
		If FormElements.Exists(LCase(sIndex)) Then Form = FormElements.Item(LCase(sIndex))
	  End Property
	
	  Public Property Get Files()
		Files = UploadedFiles.Items
	  End Property
	
	  'Calls Upload to extract the data from the binary request and then saves the uploaded files
	  Public Sub Save(path)
		Dim streamFile, fileItem
	
		if Right(path, 1) <> "\" then path = path & "\"
	
		if not uploadedYet then Upload
	
		For Each fileItem In UploadedFiles.Items
			If fileItem.Length > 0 Then
				Set streamFile = Server.CreateObject("ADODB.Stream")
				streamFile.Type = 1
				streamFile.Open
				StreamRequest.Position = fileItem.Start
				StreamRequest.CopyTo streamFile, fileItem.Length
				streamFile.SaveToFile path & fileItem.TempFileName, 2
				streamFile.close
				Set streamFile = Nothing
				fileItem.Path = path & fileItem.TempFileName
			End If	
		 Next
	  End Sub
	
	  Public Function SaveBinRequest(path) ' For debugging purposes
		StreamRequest.SaveToFile path & "\debugStream.bin", 2
	  End Function
	
	  Public Sub DumpData() 
		Dim i, aKeys, f
		response.write "<b>Form Items:</b><br>"
		aKeys = FormElements.Keys
		For i = 0 To FormElements.Count -1 ' Iterate the array
	  	    response.write aKeys(i) & " = <br>"
		  	For each k in FormElements(aKeys(i)) 
				Response.write "&nbsp;&nbsp;&nbsp;&nbsp;" & FormElements(aKeys(i))(k) & "<BR>"
			 Next
		Next
		response.write "<b>Uploaded Files:</b><br>"
		For Each f In UploadedFiles.Items
		  response.write "Name: " & f.FileName & "<br>"
		  response.write "TempName: " & f.TempFileName & "<br>"
		  response.write "Type: " & f.ContentType & "<br>"
		  response.write "Start: " & f.Start & "<br>"
		  response.write "Size: " & f.Length & "<br>"
		 Next
	  End Sub
	
	  Public Sub Upload()
		Dim nCurPos, nDataBoundPos, nLastSepPos
		Dim nPosFile, nPosBound
		Dim sFieldName, osPathSep, auxStr
	
		'RFC1867 Tokens
		Dim vDataSep
		Dim tNewLine, tDoubleQuotes, tTerm, tFilename, tName, tContentDisp, tContentType
		tNewLine = Byte2String(Chr(13))
		tDoubleQuotes = Byte2String(Chr(34))
		tTerm = Byte2String("--")
		tFilename = Byte2String("filename=""")
		tName = Byte2String("name=""")
		tContentDisp = Byte2String("Content-Disposition")
		tContentType = Byte2String("Content-Type:")
	
		uploadedYet = true
	
		on error resume next
		VarArrayBinRequest = Request.BinaryRead(Request.TotalBytes)
		if Err.Number <> 0 then 
			error = Err.Description
			KT_FileUpload__setError "ASP_UPLOAD_ERR_BINARY_READ", Array(), Array(error) 
			Response.Write Session("KT_FileUpload__Error_D")
			Response.End
			' maybe  I should print the error and stop the execution
			on error goto 0
			Exit Sub
		end if
		on error goto 0 'reset error handling
		
		nCurPos = FindToken(tNewLine,1) 'Note: nCurPos is 1-based (and so is InstrB, MidB, etc)
	
		If nCurPos <= 1 Then Exit Sub
		 
		'vDataSep is a separator like -----------------------------21763138716045
		vDataSep = MidB(VarArrayBinRequest, 1, nCurPos-1)
	
		'Start of current separator
		nDataBoundPos = 1
	
		'Beginning of last line
		nLastSepPos = FindToken(vDataSep & tTerm, 1)
	
		Do Until nDataBoundPos = nLastSepPos
		  
		  nCurPos = SkipToken(tContentDisp, nDataBoundPos)
		  nCurPos = SkipToken(tName, nCurPos)
		  sFieldName = ExtractField(tDoubleQuotes, nCurPos)
	
		  nPosFile = FindToken(tFilename, nCurPos)
		  nPosBound = FindToken(vDataSep, nCurPos)
		  
		  If nPosFile <> 0 And nPosFile < nPosBound Then
			Dim oUploadFile
			Set oUploadFile = New UploadedFile
			
			nCurPos = SkipToken(tFilename, nCurPos)
			auxStr = ExtractField(tDoubleQuotes, nCurPos)
			' We are interested only in the name of the file, not the whole path
			' Path separator is \ in windows, / in UNIX
			' While IE seems to put the whole pathname in the stream, Mozilla seem to 
			' only put the actual file name, so UNIX paths may be rare. But not impossible.
			osPathSep = "\"
			if InStr(auxStr, osPathSep) = 0 then osPathSep = "/"
			oUploadFile.FileName = Right(auxStr, Len(auxStr)-InStrRev(auxStr, osPathSep))
	
			'if (Len(oUploadFile.FileName) > 0) then 'File field not left empty
			   nCurPos = SkipToken(tContentType, nCurPos)
			  
			   auxStr = ExtractField(tNewLine, nCurPos)
				' NN on UNIX puts things like this in the streaa:
				' ?? python py type=?? python application/x-python
			   oUploadFile.ContentType = Right(auxStr, Len(auxStr)-InStrRev(auxStr, " "))
			   nCurPos = FindToken(tNewLine, nCurPos) + 4 'skip empty line
			  
			   oUploadFile.Start = nCurPos-1
			   oUploadFile.Length = FindToken(vDataSep, nCurPos) - 2 - nCurPos
			   oUploadFile.SetTempFileName
			  
			   'If oUploadFile.Length > 0 Then Set UploadedFiles(sFieldName) = oUploadFile
			   Set UploadedFiles(sFieldName) = oUploadFile
			'End If
		  Else
			Dim nEndOfData
			nCurPos = FindToken(tNewLine, nCurPos) + 4 'skip empty line
			nEndOfData = FindToken(vDataSep, nCurPos) - 2
			If Not FormElements.Exists(sFieldName) Then 
				Dim multipleValues: Set multipleValues = Server.CreateObject("Scripting.Dictionary")
				multipleValues.Add 0, String2Byte(MidB(VarArrayBinRequest, nCurPos, nEndOfData-nCurPos))
				Set FormElements(sFieldName) = multipleValues
			Else
				FormElements(sFieldName).Add FormElements(sFieldName).Count, String2Byte(MidB(VarArrayBinRequest, nCurPos, nEndOfData-nCurPos))	
			End If	
		  End If
	
		  'Advance to next separator
		  nDataBoundPos = FindToken(vDataSep, nCurPos)
		Loop
		StreamRequest.Write(VarArrayBinRequest)
	  End Sub
	
	  Private Function SkipToken(sToken, nStart)
		SkipToken = InstrB(nStart, VarArrayBinRequest, sToken)
		If SkipToken = 0 then
		 ' Exception thrown
		  Response.write "Error in parsing uploaded binary request."
		  Response.End
		end if
		SkipToken = SkipToken + LenB(sToken)
	  End Function
	
	  Private Function FindToken(sToken, nStart)
		FindToken = InstrB(nStart, VarArrayBinRequest, sToken)
	  End Function
	
	  Private Function ExtractField(sToken, nStart)
		Dim nEnd
		nEnd = InstrB(nStart, VarArrayBinRequest, sToken)
		If nEnd = 0 then
		 ' Exception thrown
		  Response.write "Error in parsing uploaded binary request."
		  Response.End
		end if
		ExtractField = String2Byte(MidB(VarArrayBinRequest, nStart, nEnd-nStart))
	  End Function
	
	  'String to byte string conversion
	  Private Function Byte2String(sString)
		Dim i
		For i = 1 to Len(sString)
		 Byte2String = Byte2String & ChrB(AscB(Mid(sString,i,1)))
		Next
	  End Function
	
	  'Byte string to string conversion
	  Private Function String2Byte(bsString)
		Dim i
		String2Byte =""
		For i = 1 to LenB(bsString)
		 String2Byte = String2Byte & Chr(AscB(MidB(bsString,i,1))) 
		Next
	  End Function
 	End Class


	Class UploadedFile
		Public ContentType
		Public Start
		Public Length
		Public Path
		Private nameOfFile
		Private tmpNameOfFile
		
		' Need to remove characters that are valid in UNIX, but not in Windows
		Public Property Let FileName(fN)
		nameOfFile = fN
		nameOfFile = replace(nameOfFile, "\", "_")
		nameOfFile = replace(nameOfFile, "/", "_")
		nameOfFile = replace(nameOfFile, ":", "_")
		nameOfFile = replace(nameOfFile, "*", "_")
		nameOfFile = replace(nameOfFile, "?", "_")
		nameOfFile = replace(nameOfFile, """", "_")
		nameOfFile = replace(nameOfFile, "<", "_")
		nameOfFile = replace(nameOfFile, ">", "_")
		nameOfFile = replace(nameOfFile, "|", "_")
		nameOfFile = replace(nameOfFile, "..", "_")		
		End Property

		Public Property Get FileName()
			FileName = nameOfFile
		End Property
		
		Public Sub SetTempFileName()
			' build a unique temp file name
			' unique per session / per time of execution / per position in the submitted stream
			
			'tmpNameOfFile = Session.SessionID & "_" & Replace(Cstr(Timer),".","") & "_" & left(Start & "000000", 6)
			
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			tmpNameOfFile = fso.GetTempName
			Set fso = nothing
		End Sub
		
		Public Property Get TempFileName()
			TempFileName = tmpNameOfFile
		End Property
		
		Public Property Get FileExtension()
			FileExtension = ""
			If nameOfFile <> "" Then
				If instrrev(nameOfFile, ".") <> 0 Then
					FileExtension = mid(nameOfFile, instrrev(nameOfFile,".")+1)
				End If
			End If
		End Property
		
		Public Function GetUploadedFileAsDictionary
			Dim upDict: Set upDict = Server.CreateObject ("Scripting.Dictionary")
			upDict.Add "ContentType", ContentType
			upDict.Add "Length", Length
			upDict.Add "Path", Path
			upDict.Add "FileName", FileName
			upDict.Add "FileExtension", FileExtension
			upDict.Add "TempFileName", TempFileName
			Set GetUploadedFileAsDictionary = upDict
		End Function
	End Class

%>
