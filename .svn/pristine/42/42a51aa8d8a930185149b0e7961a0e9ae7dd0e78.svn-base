<%

Dim TNG_EMAILPAGESECTION_BUFFER
Dim TNG_EMAILPAGESECTION_HTMLCODE: Set TNG_EMAILPAGESECTION_HTMLCODE  = Server.CreateObject ("Scripting.Dictionary")


Class tNG_EmailPageSection
	Public from
	Public to2
	Public cc
	Public bcc
	Public subject
	Public content
	Public file
	Public relfile
	Public encoding
	Public format
	Public importance
	Public css	

	Private Sub Class_Initialize()
		importance = "Normal"
		css = ""
		file = Request.ServerVariables("PATH_TRANSLATED")
	End Sub
	
	Private Sub Class_Terminate()
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


	Public Sub setContentFile (contentFile__param)
		relfile = contentFile__param
		file = KT_GetAbsolutePathToRootFolder() & replace(contentFile__param, "/", "\")
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
		getFrom = tNG_DynamicData(from, nothing, null, null, params, null)
	End Function

	Public Function getTo()
		getTo = tNG_DynamicData(to2, nothing, null, null, null, null)
	End Function

	Public Function getCc()
		getCc = cc
	End Function

	Public Function getBcc()
		getBcc = bcc
	End Function

	Public Function getSubject()
		getSubject = tNG_DynamicData(subject, nothing, null, null, null, null)
	End Function

    Public Function getContent()
      getContent = content
    End Function

	Public Function getTextBody()
		If LCase(format) = "text" Then
			ret = content
			ret = KT_strip_tags(ret)
			getTextBody = ret
		Else
			getTextBody = "" ' email object automatically sets text content when html content is set
		End If
	End Function
	
	Public Function getHtmlBody()
		If LCase(format) <> "text" Then
			ret = getHeader() & content & getFooter()
			ret = KT_transformsPaths(KT_GetURLToResource(relfile), ret)
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


	Public Function getHeader()
		text = "<html>" & vbNewLine & "<header>" & vbNewLine & _
		css & vbNewLine & _
		"</header>"  & vbNewLine & _ 
		"<body>"  & vbNewLine
		getHeader = text
	End Function

	Public Function getFooter()
		getFooter = "</body>" & vbNewLine & "</html>" & vbNewLine
	End Function
	
	Public Function BeginContent()
		BeginContent = False
	End Function

	Public Function BeginContent_true()
		BeginContent_true = True
	End Function

	Public Function EndContent()
		' do nothing.. this is just a marker
	End Function

	Public Function Execute()
		ParseFile
		If encoding = "" Then
			encoding = "iso-8859-1"
		End If

		Set email = new KT_email
		email.setPriority importance
		email.sendEmail tNG_email_host, tNG_email_port, tNG_email_user, tNG_email_password, getFrom(), getTo(), getCc(), getBcc(), getSubject(), encoding, getTextBody(), getHtmlBody()
		If email.hasError() Then
			arr = email.getError()
			Set myErr = new tNG_error
			'TNG_EMAILPAGESECTION_throwError arr(1)
			'Response.End()
			Set Execute = myErr
			Exit Function
		End If
		Set Execute = nothing
	End Function

	Private Sub searchCss(text)
	
		Dim link_matches	
		KT_preg_match "<link([^>]*)>", text, link_matches
		For Each Match in link_matches
			If instr(1, match.Value, "stylesheet", 1) <> 0 Then
				css = css & match.Value & vbNewLine
			End If	
		Next
			
		Dim style_matches
		KT_preg_match "<style([^>]*)>", text, style_matches
		For Each Match in style_matches
			style_start = match.firstIndex
			style_end = instr(style_start + 1, text, "</style>", 1) 
			If style_end <> 0 Then
				style_length = style_end + len("</style>") - (style_start) 
				css = css & mid(text, style_start, style_length) & vbNewLine
			End If
		Next
	
		
		Dim meta_match
		Dim charset_match
		KT_preg_match "<\s*meta([\s\S][^>]*)>", text, meta_match
		For each Match in meta_match
			meta = Match.Value
			KT_preg_match "charset=(.[^>\""\']*)", meta, charset_match
			For each m in charset_match
				encoding = replace(m.Value, "charset=", "", 1, -1, 1)
				Exit For
			Next
			Exit For
		Next
	End Sub

	' BUFFER ENGINE FUNCTIONS
	Private Sub ParseFile()
		TNG_EMAILPAGESECTION_HTMLCODE.removeAll
		TNG_EMAILPAGESECTION_BUFFER = ""

		asp_file_content = TNG_EMAILPAGESECTION_getFileContent(file)
		searchCss asp_file_content
		asp_pagesection_content  = 	TNG_EMAILPAGESECTION_getBufferedContent(asp_file_content)

		If asp_pagesection_content <> "" Then
			asp_pagesection_content = TNG_EMAILPAGESECTION_transformContentToCodeForExecute(asp_pagesection_content)
			On Error Resume Next
			ExecuteGlobal asp_pagesection_content
			If Err.Number<>0 Then
				TNG_EMAILPAGESECTION_throwError ("Couldn't process tNG_EmailPageSection code.<br />Error description: " & Err.Description)
			End If	
			On Error GoTo 0 
		End If
		content = TNG_EMAILPAGESECTION_BUFFER

		start_body = instr (1, content, "<body", 1)
		If start_body <> 0  Then
			content_before_body = left(content, start_body-1)
            end_body_tag = instr (start_body + 1,content, ">")
			start_body = end_body_tag  + 1
			end_body = instr (end_body_tag, content, "</body>", 1)
			If end_body <> 0 Then
				content = mid (content, start_body, end_body-start_body)
			Else
				content = mid (content, start_body)	
			End If
		End If

		content = removeScript(content)
		content = removeStyle(content)
	End Sub


	
	Private Function TNG_EMAILPAGESECTION_throwError (errMessage)
		Response.write "<br /><span style=""color:red"">tNG_EmailPageSection Error<br />"
		Response.write errMessage	
		Response.write "</span><br />"
		Response.End()
	End Function
		
		
		
	Private Function TNG_EMAILPAGESECTION_getFileContent (asp_file_path)
		TNG_EMAILPAGESECTION_getFileContent = ""
		Dim fso, f
		Set fso = Server.CreateObject("Scripting.FileSystemObject")
		If fso.FileExists(asp_file_path) Then
			On Error Resume Next
			Set f = fso.OpenTextFile(asp_file_path, 1, false)
			TNG_EMAILPAGESECTION_getFileContent = f.ReadAll
			TNG_EMAILPAGESECTION_getFileContent = replace(TNG_EMAILPAGESECTION_getFileContent, vbNewLine, vbCr) ' for MAC
			TNG_EMAILPAGESECTION_getFileContent = replace(TNG_EMAILPAGESECTION_getFileContent, vbCr, vbNewLine)
			f.Close
			Set f = nothing
			If err.Number<>0 Then
				TNG_EMAILPAGESECTION_throwError ("Couldn't open file '" & relfile & "'<br />Error description: " & Err.Description)	
			End If	
			On Error GoTo 0
		Else
			TNG_EMAILPAGESECTION_throwError ("Couldn't find file '" & relfile & "'. Please make sure that the file is uploaded on the testing server.")
		End If
		Set fso = nothing
	End Function
	
	
	Private Function TNG_EMAILPAGESECTION_getBufferedContent (ByRef content)
		Dim function_name__start_buffer: function_name__start_buffer = "BeginContent"
		Dim function_name__get_buffer: function_name__get_buffer = "EndContent"
		Dim s, e
		'******************************************************
		'* 	Find the call for KT_ob_start  (S)	
		'*  Find the call for KT_ob_get_contents (E)
		'*  Get the content between a line before (S) and a line before (E)
		'******************************************************
		
		s = instr (1, content, function_name__start_buffer, 1)
		If s = 0 Then
			' this error can be raised only if the name of the functions change and they aren't updated here
			TNG_EMAILPAGESECTION_throwError ("Couldn't find method '" & function_name__start_buffer & "'")	
		End If
		s = instrrev (content, vbNewLine, s)
		
		e = instr (s, content, function_name__get_buffer, 1)
		If e = 0 Then
			TNG_EMAILPAGESECTION_getBufferedContent = ""
			Exit Function
		End If
		e = instrrev (content, vbNewLine, e)
		
		
		 tmp = "<" & "%" & vbNewLine & mid (content, s, e-s) & vbNewLine & "%" & ">"
		 tmp = replace(tmp, function_name__start_buffer, function_name__start_buffer & "_true", 1, -1, 1)
		 TNG_EMAILPAGESECTION_getBufferedContent = tmp
	End Function


	' parse the code and make it available for the ExecuteGlobal function
	Private Function TNG_EMAILPAGESECTION_transformContentToCodeForExecute (ByRef content)
		delimiter_str = "##KT##KT##"
		delimiter_len = len(delimiter_str)
		starter_str = "$"
		starter_len = len (starter_str)
		
		Dim tmp
		' replace the code delimiters
		tmp = replace (content, "<" & "%", delimiter_str & starter_str)
		tmp = replace (tmp, "%" &  ">", delimiter_str)
		
		' check if there is any ASP code 
		If instr (tmp, delimiter_str) <> 0 Then
			If left(tmp, delimiter_len) = delimiter_str Then
				tmp = right (tmp, len(tmp)  - delimiter_len)
			End If
			If right(tmp, delimiter_len) = delimiter_str Then
				tmp = left (tmp, len(tmp)  - delimiter_len)
			End If
						
			tmp_array = split(tmp, delimiter_str)
			For i = lbound(tmp_array) to ubound (tmp_array)
				tmp  =  tmp_array(i)
				If left(tmp,starter_len) = starter_str Then
					' This is ASP code
					tmp = right (tmp, len(tmp) - starter_len)
					tmp = replace (tmp, "response.write", "TNG_EMAILPAGESECTION_BUFFER = TNG_EMAILPAGESECTION_BUFFER & ", 1, -1, 1)
					
					' check if there is quit output ( < %="str"% > )
					If left (ltrim(tmp), 1) = "=" Then
						tmp = ltrim(tmp)
						tmp = right (tmp, len(tmp) - 1) 
						tmp = "TNG_EMAILPAGESECTION_BUFFER = TNG_EMAILPAGESECTION_BUFFER & " & tmp
					End If
				Else
					' This is HTML code
					 If tmp <> "" Then
						' cannot output this string with Response.write - it must be first escaped, SO
						' must save it in some variable and the write will be done at runtime
						TNG_EMAILPAGESECTION_HTMLCODE("HTML_" & i)  = tmp
						tmp = "TNG_EMAILPAGESECTION_BUFFER = TNG_EMAILPAGESECTION_BUFFER & TNG_EMAILPAGESECTION_HTMLCODE(""HTML_" & i & """)" 
					 End If
				End If
				tmp_array(i) = tmp
			Next
			TNG_EMAILPAGESECTION_transformContentToCodeForExecute = join(tmp_array, vbNewLine)
		Else
			' pure HTML Code
			TNG_EMAILPAGESECTION_HTMLCODE("only_HTML") = tmp
			TNG_EMAILPAGESECTION_transformContentToCodeForExecute = "TNG_EMAILPAGESECTION_BUFFER = TNG_EMAILPAGESECTION_BUFFER & TNG_EMAILPAGESECTION_HTMLCODE(""only_HTML"")" 
		End If
	End Function

End Class	
	
%>