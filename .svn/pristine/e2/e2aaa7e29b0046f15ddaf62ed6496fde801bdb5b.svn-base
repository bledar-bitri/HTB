<%
Class tNG_Email
	Public tNG	' object
	Public from
	Public to2
	Public cc
	Public bcc
	Public subject
	Public content
	Public contentFile
	Public file
	Public encoding
	Public format
	Public importance
	Public escapeMethod
	Public useSavedData
	

	Private Sub Class_Initialize()
		Set this = Me
	End Sub
	
	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (ByRef tNG__param)
		Set tNG = tNG__param
		from = ""
		to2 = ""
		cc = ""
		bcc = ""
		subject = ""
		content = ""
		contentFile = ""
		file = KT_getUri()
		encoding = ""
		importance = "Normal"
		escapeMethod = "none"
		useSavedData = null
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
	
	Public Sub setFrom (from__param)
		from = from__param
	End Sub

	Public Sub setTo (to__param)
		to2 = to__param
	End Sub


	Public Sub setCC (cc__param)
		cc = cc__param
	End Sub

	Public Sub setBCC (bcc__param)
		bcc = bcc__param
	End Sub
	

	Public Sub setSubject (subject__param)
		subject = subject__param
	End Sub


	Public Sub setContent (content__param)
		content = content__param
	End Sub

	Public Sub setContentFile (contentFile__param)
		file = contentFile__param
		contentFile = tNG_DynamicData(contentFile__param, tNG, escapeMethod, this.getUseSavedData(), null, null)
	End Sub

	Public Sub setEncoding (encoding__param)
		encoding = encoding__param
	End Sub

	Public Sub setFormat (format__param)
		format = LCase(format__param)
	End Sub

	Public Sub setImportance (importance__param)
		importance = LCase(importance__param)
	End Sub


	Public Function getFrom()
		Dim params: Set params = Server.CreateObject("Scripting.Dictionary")
		params("KT_defaultSender") = tNG_email_defaultFrom
		getFrom = tNG_DynamicData(from, tNG, escapeMethod, this.getUseSavedData, params, null)
	End Function

	Public Function getTo()
		getTo = tNG_DynamicData(to2, tNG, escapeMethod, this.getUseSavedData, null, null)
	End Function

	Public Function getCc()
		getCc = tNG_DynamicData(cc, tNG, escapeMethod, this.getUseSavedData, null, null)
	End Function

	Public Function getBcc()
		getBcc = tNG_DynamicData(bcc, tNG, escapeMethod, this.getUseSavedData, null, null)
	End Function

	Public Function getSubject()
		getSubject = tNG_DynamicData(subject, tNG, escapeMethod, this.getUseSavedData, null, null)
	End Function

	Public Function getTextBody()
		If LCase(format) = "text" Then
			ret = tNG_DynamicData(content, tNG, escapeMethod, this.getUseSavedData(), null, false)
			ret = this.removeScript(ret)
			ret = this.removeStyle(ret)
			ret = KT_strip_tags(ret)
			getTextBody = ret
		Else
			getTextBody = "" ' email object automatically sets text content when html content is set
		End If
	End Function
	
	Public Function getHtmlBody()
		If LCase(format) <> "text" Then
			ret = tNG_DynamicData(content, tNG, escapeMethod, this.getUseSavedData(), null, false)
			ret = this.removeScript(ret)
			ret = KT_transformsPaths(KT_makeIncludedURL(file), ret)
			getHtmlBody = ret
		Else	
			getHtmlBody = ""
		End If
	End Function

	Public Function removeScript(text)
		Dim script_matches
		offset = 0
		searched_content = text
		KT_preg_match "<script([^>]*)>", searched_content, script_matches
		For Each Match in script_matches
			script_start = match.firstIndex
			script_end = instr(script_start + offset + 1, searched_content, "</script>", 1) 
			script_length = script_end + len("</script>") - (script_start + offset + 1) 
			searched_content = left(searched_content, script_start + offset) & mid (searched_content, script_end + len("</script>"))
			offset = offset - script_length
		Next
		removeScript = searched_content
	End Function

	Public Function removeStyle(text)
		Dim style_matches
		offset = 0
		searched_content = text
		KT_preg_match "<style([^>]*)>", searched_content, style_matches
		For Each Match in style_matches
			style_start = match.firstIndex
			style_end = instr(style_start + offset + 1, searched_content, "</style>", 1) 
			style_length = style_end + len("</style>") - (style_start + offset + 1) 
			searched_content = left(searched_content, style_start + offset) & mid (searched_content, style_end + len("</style>"))
			offset = offset - style_length
		Next
		removeStyle = searched_content
	End Function
	
	Public Function getUseSavedData()
		If Not KT_isSet(useSavedData) Then
			If tNG.getTransactionType() = "_delete" Or tNG.getTransactionType() = "_multipleDelete" Then
				useSavedData = true
			Else
				useSavedData = false
			End If
		End If
		getUseSavedData = useSavedData
	End Function	

	Public Function Execute()
		If contentFile <> "" Then
			cFile = Server.MapPath(KT_makeIncludedPath(contentFile))
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			If fso.FileExists(cFile) Then
				Set f= fso.OpenTextFile(cFile, 1, false)
				content = f.ReadAll
				f.Close
				Set f = nothing
			Else
				Set myErr = new tNG_error
				myErr.Init "EMAIL_NO_TEMPLATE", array(), array()
				Set Execute = myErr
				Exit Function
			End If
		End If

		Set email = new KT_email
		email.setPriority importance
		email.sendEmail tNG_email_host, tNG_email_port, tNG_email_user, tNG_email_password, this.getFrom(), this.getTo(), this.getCc(), this.getBcc(), this.getSubject(), encoding, this.getTextBody(), this.getHtmlBody()
		If email.hasError() Then
			arr = email.getError()
			Set myErr = new tNG_error
			myErr.Init "EMAIL_FAILED", array(""), array(arr(1))
			Set Execute = myErr
			Exit Function
		End If
		Set Execute = nothing
	End Function
End Class	
	
%>