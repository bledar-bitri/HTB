<%
Class tNG_EmailRecordset
	Public recordset
	Public recordsetName
	Private errObj
	
	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_Email
		parent.SetContextObject Me
		
		Set errObj = nothing
	End Sub

	Private Sub Class_Terminate()

	End Sub

	Public Function setRecordset (recordsetName__param)
		recordsetName = recordsetName__param
		On Error Resume Next
		ExecuteGlobal "Set rs_tNG_EmailRecordset = " & recordsetName
		Set recordset = rs_tNG_EmailRecordset
		If err.number <> 0 Then
			Set errObj = new tNG_error
			errObj.Init "EMAIL_ERROR_RECORDSET", array(), array(recordsetName)
		End If
		On Error GoTo 0
	End Function

	Public Function getTo()
		getTo = recordset.Fields.Item(to2).Value
	End Function

	Public Function Execute()
		If KT_isSet(errObj) Then
			Set Execute = errObj
			Exit Function
		End If
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

		arrErrors = array()
		While not recordset.EOF 
			Set email = new KT_email
			email.setPriority importance
			email.sendEmail tNG_email_host, tNG_email_port, tNG_email_user, tNG_email_password, this.getFrom(), this.getTo(), this.getCc(), this.getBcc(), this.getSubject(), encoding, this.getTextBody(), this.getHtmlBody()
			If email.hasError() Then
				arr = email.getError()
				arrErrors = KT_array_push(arrErrors, "Email to user: <strong>" & this.getTo() & "</strong> was not sent. Error returned: " & arr(i))
			End If
			recordset.MoveNext
		Wend				
		If ubound(arrErrors) <> -1 Then
			Set myErr = new tNG_error
			myErr.Init "EMAIL_FAILED", array(""), array(join(arrErrors, "<br />"))
			Set Execute = myErr
			Exit Function
		End If
		Set Execute = nothing
	End Function

	'===========================================
	' Inheritance
	'===========================================
	Public this
	Public parent

	Public Sub SetContextObject(ByRef objContext)
		Set this = objContext
		parent.SetContextObject objContext
	End Sub

	'  Inherited properties from tNG_Email
	'-------------------------------------------
	' tNG	
	Public Property Get tNG	
		Set tNG	 = parent.tNG	
	End Property
	Public Property Set tNG	(ByRef tNG__par)
		Set parent.tNG = tNG__par
	End Property

	' from
	Public Property Get from
		from = parent.from
	End Property
	Public Property Let from(from__par)
		parent.from = from__par
	End Property

	' to2
	Public Property Get to2
		to2 = parent.to2
	End Property
	Public Property Let to2(to2__par)
		parent.to2 = to2__par
	End Property

	' cc
	Public Property Get cc
		cc = parent.cc
	End Property
	Public Property Let cc(cc__par)
		parent.cc = cc__par
	End Property

	' bcc
	Public Property Get bcc
		bcc = parent.bcc
	End Property
	Public Property Let bcc(bcc__par)
		parent.bcc = bcc__par
	End Property

	' subject
	Public Property Get subject
		subject = parent.subject
	End Property
	Public Property Let subject(subject__par)
		parent.subject = subject__par
	End Property

	' content
	Public Property Get content
		content = parent.content
	End Property
	Public Property Let content(content__par)
		parent.content = content__par
	End Property

	' contentFile
	Public Property Get contentFile
		contentFile = parent.contentFile
	End Property
	Public Property Let contentFile(contentFile__par)
		parent.contentFile = contentFile__par
	End Property

	' file
	Public Property Get file
		file = parent.file
	End Property
	Public Property Let file(file__par)
		parent.file = file__par
	End Property

	' encoding
	Public Property Get encoding
		encoding = parent.encoding
	End Property
	Public Property Let encoding(encoding__par)
		parent.encoding = encoding__par
	End Property

	' format
	Public Property Get format
		format = parent.format
	End Property
	Public Property Let format(format__par)
		parent.format = format__par
	End Property

	' importance
	Public Property Get importance
		importance = parent.importance
	End Property
	Public Property Let importance(importance__par)
		parent.importance = importance__par
	End Property

	' escapeMethod
	Public Property Get escapeMethod
		escapeMethod = parent.escapeMethod
	End Property
	Public Property Let escapeMethod(escapeMethod__par)
		parent.escapeMethod = escapeMethod__par
	End Property

	' useSavedData
	Public Property Get useSavedData
		useSavedData = parent.useSavedData
	End Property
	Public Property Let useSavedData(useSavedData__par)
		parent.useSavedData = useSavedData__par
	End Property

	'  Inherited properties as Property
	'-------------------------------------------

	'  Inherited methods
	'-------------------------------------------
	Public Sub Init (ByRef tNG__param)
		parent.Init  tNG__param
	End Sub

	Public Sub setFrom (from__param)
		parent.setFrom from__param
	End Sub

	Public Sub setTo (to__param)
		parent.setTo to__param
	End Sub

	Public Sub setCC (cc__param)
		parent.setCC cc__param
	End Sub

	Public Sub setBCC (bcc__param)
		parent.setBCC bcc__param
	End Sub

	Public Sub setSubject (subject__param)
		parent.setSubject subject__param
	End Sub

	Public Sub setContent (content__param)
		parent.setContent content__param
	End Sub

	Public Sub setContentFile (contentFile__param)
		parent.setContentFile contentFile__param
	End Sub

	Public Sub setEncoding (encoding__param)
		parent.setEncoding encoding__param
	End Sub

	Public Sub setFormat (format__param)
		parent.setFormat format__param
	End Sub

	Public Sub setImportance (importance__param)
		parent.setImportance importance__param
	End Sub

	Public Function getFrom()
		getFrom = parent.getFrom()
	End Function

	Public Function getCc()
		getCc = parent.getCc()
	End Function

	Public Function getBcc()
		getBcc = parent.getBcc()
	End Function

	Public Function getsubject()
		getsubject = parent.getsubject()
	End Function

	Public Function getTextBody()
		getTextBody = parent.getTextBody()
	End Function

	Public Function getHtmlBody()
		getHtmlBody = parent.getHtmlBody()
	End Function

	Public Function removeScript(text)
		removeScript = parent.removeScript(text)
	End Function

	Public Function removeStyle(text)
		removeStyle = parent.removeStyle(text)
	End Function

	Public Function getUseSavedData()
		getUseSavedData = parent.getUseSavedData()
	End Function



End Class
%>