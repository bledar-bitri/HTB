<%
'
'	Copyright (c) InterAKT Online 2000-2005
'

'
' The dispatcher class, that handles all the transactions in a page.
'

Class tNG_dispatcher
	Public tNGs
	Public n
	Public relPath
	Public UnivalProps
	Public fieldHints
	Public UnivalErrors
	
	Public multiCounter
	
	Private UnivalCount

	Public Sub Class_Initialize()
		Set tNGs = Server.CreateObject("Scripting.Dictionary")
		n = 0
		relPath = ""

		Set fieldHints = Server.CreateObject("Scripting.Dictionary")
		Set UnivalProps = Server.CreateObject("Scripting.Dictionary")
		Set UnivalErrors = Server.CreateObject("Scripting.Dictionary")
		multiCounter = 0
		UnivalCount = 0
	End Sub

	Public Sub Class_Terminate()
		Set tNGs = nothing
	End Sub

	Public Sub Init(relPath__param)
		relPath = relPath__param
	End Sub



	Public Sub addTransaction (ByRef tNG) 
		Set tNGs(n) = tNG
		n = n + 1 
		tNG.setDispatcher Me
	End Sub
	

	Public Function getRecordset (tableName)
		method_where  = -1
		method = -1
		
		'	-1 = unknown
		'	 1 = default values
		'	 2 = from recordset
		'	 3 = submitted values
		
		For i = 0  to  n-1
			If tNGs(i).getTable() = tableName Then
				transactionType = tNGs(i).getTransactionType()				
				If transactionType = "_insert" Or transactionType = "_custom" Or transactionType = "_login" Or transactionType = "_multipleInsert" Then
					If method < 1 Then
						method = 1
						method_where = i
					End If
				ElseIf transactionType = "_update" Or transactionType = "_multipleUpdate" Then
					pkv = tNGs(i).getPrimaryKeyValue()
					If KT_isSet(pkv) Then
						If method < 2 Then
							method = 2
							method_where = i
						End If
					End If	
				End If
				If tNGs(i).isStarted() Then
					If tNGs(i).exportsRecordset() Then
						If isArray(tNGs(i).getErrorMsg()) Then
							method = 3
							method_where = i
						End If
					End If
				End If
			End If
		Next
		
		If method_where = -1 Then
			Set getRecordset = tNGs(0).getRecordset()
		Else
			Set getRecordset = tNGs(method_where).getRecordset()
		End If
	End Function
	

	Public Sub executeTransactions 
		For i=0 to n-1
			tNGs(i).executeTransaction
		Next
	End Sub

	Public Function displayValidationRules()
		outRules = ""
		outRules = outRules & "<script src=""" & relPath & "includes/tng/scripts/FormValidation.js"" type=""text/javascript"" language=""javascript""></script>"  & vbNewLine
		outRules = outRules & "<script src=""" & relPath & "includes/tng/scripts/FormValidation.js.asp"" type=""text/javascript"" language=""javascript""></script>" & vbNewLine
		If UnivalProps.Count > 0 Then
			outRules = outRules & "<script type=""text/javascript"" language=""javascript"">" & vbNewLine
			univalPropKeys = KT_array_keys(UnivalProps)
			sw = false
			Dim i
			For i=0 to Ubound(univalPropKeys) 
				fieldName = univalPropKeys(i)
				Set field = UnivalProps(fieldName)
				
					' get the form field name
					formFieldName = fieldName
					For j=0 to n-1
						Set cols = tNGs(j).columns
						If cols.Exists(fieldName) Then
							formFieldName = cols(fieldName)("reference")
							Exit for
						End If
					Next
					
					If formFieldName <> "" Then
						outRules = outRules & vbNewLine & "  KT_FVO['" & KT_escapeJS(formFieldName) & "'] = {"
						outRules = outRules & "required: " & field("required") & ", "
						outRules = outRules & "type: '" & field("type") & "', "
						If field("format") <> "" Then
							outRules = outRules & "format: '" & field("format") & "', "
						End If
	
						If field("additional_params") <> "" Then
							outRules = outRules & "additional_params: '" & KT_escapeJS(field("additional_params")) & "', "
						End If
						If field("min") <> "" Then
							outRules = outRules & "min: '" & field("min") & "', "
						End If					
						If field("max") <> "" Then
							outRules = outRules & "max: '" & field("max") & "', "
						End If					
						If UnivalErrors(fieldName) <> "" Then
								outRules = outRules & "errorMessage: '" & KT_escapeJS(UnivalErrors(fieldName))& "', "
						End If
						
						outRules = left(outRules, len(outRules)-2)
						outRules = outRules & "}"
					End If
					
			Next
			outRules = outRules & vbNewLine & vbNewLine
			outRules = outRules & "  KT_FVO_properties['noTriggers'] += "  & UnivalCount & ";" & vbNewLine
			outRules = outRules & "  KT_FVO_properties['noTransactions'] += " & n & ";" & vbNewLine
			outRules = outRules & "</script>"
		End If
		displayValidationRules = outRules
	End Function


	Public Function displayFieldHint(fieldName) 
		If KT_isSet(fieldHints) And fieldHints.Exists(fieldName) And fieldHints(fieldName) <> "()" Then
			displayFieldHint =  "<span class=""KT_field_hint"">" & fieldHints(fieldName) & "</span>" & vbNewLine
		End If
	End Function


	Public Sub SetCounter(multiCounter__param)
		multiCounter = multiCounter__param
	End Sub

	Function displayFieldError(tableName, fieldName)
		ret = ""
		exitfor = false
		For i=0 to n-1
			If tNGs(i).getTable() = tableName Then
				If tNGs(i).isStarted Then
					Set tmp = tNGs(i).getError()
					If KT_isSet(tmp) Then
						If KT_in_array(tNGs(i).getTransactionType(), Array("_multipleInsert", "_multipleUpdate", "_multipleDelete"), True) Then
							ret = tNGs(i).getFieldError(fieldName, multiCounter)
						Else
							ret = tNGs(i).getFieldError(fieldName)
						End If
						exitfor = true
					End If
				End If
			End If
			
			If exitfor Then
				Exit For
			End If
		Next
		
		If ret <> "" Then
			displayFieldError = "<br class=""clearfixplain"" /><div class=""KT_field_error"">" & ret &  "</div>" & vbNewLine
		Else
			displayFieldError = ""
		End If
	End Function



	Public Sub prepareValidation(ByRef uniVal)
		UnivalCount = UnivalCount + 1	
		If uniVal.columns.Count = 0 Then
			Exit Sub
		End If
		
		For each columnName in uniVal.columns 
			Set column = uniVal.columns(columnName)
			' Set unival JS div errors
			' here we set the least restrictive required prop
			If column("required") Then
				required = "true"
			Else
				required = "false"
			End If

			If UnivalProps.Exists(columnName) Then
				If UnivalProps(columnName)("required") <> required Then
					required = "false"
				End If
			End If
			
			If Not UnivalProps.Exists(columnName) Then
				Set UnivalProps(columnName) = Server.CreateObject("Scripting.Dictionary")
			End If	
			
			UnivalProps(columnName)("required") = required
			UnivalProps(columnName)("type") = column("type")
			UnivalProps(columnName)("format") = column("format")
			UnivalProps(columnName)("additional_params") = column("additional_params")
			UnivalProps(columnName)("min") = column("min_cs")
			UnivalProps(columnName)("max") = column("max_cs")
			If Not UnivalProps(columnName).Exists("count") Then
				UnivalProps(columnName)("count") = 1
			Else	
				UnivalProps(columnName)("count") = UnivalProps(columnName)("count") + 1
			End If
			UnivalErrors(columnName) = column("message")
			' Set field Hints
			If column("type") = "regexp" Then
				fieldHints(columnName) = ""
			ElseIf column("type") = "mask" Then
				fieldHints(columnName) = "(" & column("additional_params") & ")"
			ElseIf column("type") = "date" and column("format") <> "" Then
				fieldHints(columnName) = "(" & uniVal.genericValidationMessages("date" & "_" & column("format")) & " " & column("date_screen_format") & ")"
			ElseIf column("format") <> "" Then
				fieldHints(columnName) = "(" & uniVal.genericValidationMessages(column("type") & "_" & column("format")) & ")"	
			End If
		Next
	End Sub


	Public Function getErrorMsg()
		ret_warning = ""
		ret_user = ""
		ret_devel = ""

		errorWasFound = false
		Dim i
		For i=0 to n-1
			arrErrors = tNGs(i).getErrorMsg()
			ret_warning = arrErrors(0)
			ret_user = arrErrors(1)
			ret_devel = arrErrors(2)
			If ret_warning <> "" Or ret_user <> "" Or ret_devel <> "" Then
				errorWasFound = true
				Exit For
			End If
		Next

		Dim rethead
		rethead = ""
		' rethead = "<link href=""" &  relPath & "includes/tng/styles/default.css"" rel=""stylesheet"" type=""text/css"" />"  & vbNewLine
		' rethead = rethead & "<script src=""" & relPath & "includes/common/js/base.js"" type=""text/javascript"" language=""javascript""></script>"  & vbNewLine
		' rethead = rethead & "<script src=""" & relPath & "includes/common/js/utility.js"" type=""text/javascript"" language=""javascript""></script>" & vbNewLine
		Dim ret
		ret = ""
		txtContent = ""
		txtContent = txtContent & "Client IP:" & vbNewLine & Request.ServerVariables("REMOTE_ADDR")
		txtContent = txtContent & vbNewLine & vbNewLine & "Host:" & vbNewLine & Request.ServerVariables("HTTP_HOST")
		txtContent = txtContent & vbNewLine & vbNewLine & "Requested URI:" & vbNewLine & Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			txtContent = txtContent & "?" & Request.ServerVariables("QUERY_STRING")
		End If
		txtContent = txtContent & vbNewLine & vbNewLine & "Date" & vbNewLine & Now()
		If errorWasFound Then
			If ret_warning <> "" Then
				ret = ret & "<div id=""KT_tngwarning"">" & ret_warning & "</div>" & vbNewLine
				txtContent = txtContent & vbNewLine & vbNewLine & "Warning:" & vbNewLine & " " & ret_warning
			End If
			If ret_user <> "" Then
				ret = ret & "<div id=""KT_tngerror""><label>" & KT_getResource("ERROR_LABEL", "tNG", array()) & "</label><div>" & ret_user & "</div></div>" & vbNewLine
				txtContent = txtContent & vbNewLine & vbNewLine & KT_getResource("ERROR_LABEL", "tNG", array()) & vbNewLine & " " & ret_user
			End If

			If tNG_debug_mode = "DEVELOPMENT" Then
				js_err = KT_escapeJS(ret_user)
				js_devNotes = KT_escapeJS(ret_devel)
				js_os = ""
				js_webserver = Request.ServerVariables("SERVER_SOFTWARE")
				js_servermodel = "ASP VBScript"
				js_installation = ""
				js_extensions = ""
				ret = rethead & ret
				ret = ret & _
				"<script type=""text/javascript"" language=""javascript"">" & vbNewLine & _
				"function needHelp() {" & vbNewLine & _
					"if (confirm('Some data will be submitted to InterAKT. Do you want to continue?')) {" & vbNewLine & _
						"var rand = Math.random().toString().substring(3, 10);" & vbNewLine & _
						"var wnd = window.open('" & relPath & "includes/tng/pub/blank.html', 'KTDebugger_' + rand, '');" & vbNewLine & _
						"try {" & vbNewLine & _
							"var doc = wnd.document;" & vbNewLine & _
						"} catch(e) { " & vbNewLine & _
							"alert('The popup could not be opened. Please configure your pop-up blocker software to allow this.'); " & vbNewLine & _
							"return;" & vbNewLine & _
						"}" & vbNewLine & _
						vbNewLine & _
						"var frm = utility.dom.createElement(" & vbNewLine & _
							"""FORM"", " & vbNewLine & _
							"{" & vbNewLine & _
							"'action': 'http://www.interaktonline.com/error/', " & vbNewLine & _
							"'method': 'POST', " & vbNewLine & _
							"'style': ""display: none""" & vbNewLine & _
							"}, " & vbNewLine & _
							"wnd" & vbNewLine & _
						");" & vbNewLine & _
						"frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'name': 'err', 'value': '" & js_err & "'}, wnd));" & vbNewLine & _
						"frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'name': 'devNotes', 'value': '" & js_devNotes & "'}, wnd));" & vbNewLine & _
						"frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'name': 'os', 'value': '" & js_os & "'}, wnd));" & vbNewLine & _
						"frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'name': 'webserver', 'value': '" & js_webserver & "'}, wnd));" & vbNewLine & _
						"frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'name': 'servermodel', 'value': '" & js_servermodel & "'}, wnd));" & vbNewLine & _
						"frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'name': 'installation', 'value': '" & js_installation & "'}, wnd));" & vbNewLine & _
						"frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'name': 'extensions', 'value': '" & js_extensions & "'}, wnd));" & vbNewLine & _
						"setTimeout(function() {" & vbNewLine & _
							"wnd.document.body.appendChild(frm);" & vbNewLine & _
							"frm.submit();" & vbNewLine & _
						"}, 5);" & vbNewLine & _
					"}" & vbNewLine & _
					"//return false;" & vbNewLine & _
				"}" & vbNewLine & _
				"</script>" & vbNewLine
				
				If ret_devel <> "" Then
					ret = ret & "<div id=""KT_tngdeverror""><label>Developer Details:</label><div>" & ret_devel & "</div><div id=""KT_needhelp""><a href=""javascript:needHelp()"">" & KT_getResource("ONLINE_TROUBLESHOOT", "tNG", null) & "</a></div></div>" 
				End If
				tmp = tNG_log__getResult("html")
				ret = ret & "<div id=""KT_tngtrace""><label>tNG Execution Trace - <a href=""#"" onclick=""document.getElementById('KT_tngtrace_details').style.display=(document.getElementById('KT_tngtrace_details').style.display!='block'?'block':'none');return false;"">VIEW</a></label>" &  tmp & "</div>"
			End If
			If tNG_debug_log_type <> "" Then
				txtContent = txtContent & vbNewLine & vbNewLine & "Developer Details:" & vbNewLine & ret_devel
				tmp = tNG_log__getResult("text")
				txtContent = txtContent & vbNewLine & vbNewLine & "tNG Execution Trace:" & vbNewLine & tmp
				
				If tNG_debug_log_type =  "logfile" Then
					' log file
					yyyy =  datepart("yyyy", date) 
					mm = datepart("m", date)
					If Cint (mm) < 10 Then
						mm = "0" & mm
					End If
					fileName = yyyy & "-" & mm & ".log"
					logFile = KT_GetAbsolutePathToRootFolder() & "includes/tng/logs/" & fileName
					Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
					Dim f
					On Error Resume Next
					If fso.FileExists (logFile) Then
						Set f = fso.OpenTextFile(logFile, 8, True)
					Else
						Set f = fso.OpenTextFile(logFile, 2, True)
					End If
					f.Write "=== BEGIN MESSAGE ===" & vbNewLine & _
							 txtContent &  vbNewLine & _
							"=== END MESSAGE ===" & vbNewLine & vbNewLine & vbNewLine 
					f.Close
					On Error GoTo 0
					Set f = nothing
				Else
					Set email = new KT_email
					email.sendEmail tNG_email_host, tNG_email_port, tNG_email_user, tNG_email_password, tNG_debug_email_from, tNG_debug_email_to, "", "", tNG_debug_email_subject, "ISO-8859-1", txtContent, ""
				End If
			End If
		End If
		getErrorMsg = ret
	End Function
	
	Public Function getLoginMsg()
		show = false
		Dim i
		For i = 0 to n-1 
			If tNGs(i).getTransactionType() = "_login" And Not tNGs(i).started Then
				show = true
				Exit For
			End If
		Next
		If show Then
			info_resources = Array("REG_ACTIVATE", "REG_EMAIL", "REG", "ACTIVATED", "FORGOT", "DENIED")
			info_key = tNG_getRealValue("GET", "info")
			If info_key <> "" Then
				If KT_in_array(info_key, info_resources, false) Then
					getLoginMsg = "<div id=""KT_tngdeverror"">" & _
					"<label>Message:</label>" & _
					"<div>" &  KT_getResource("LOGIN_MESSAGE__" & info_key, "tNG", null)  & "</div>" & _ 
					"</div>"
					Exit Function
				End If
			End If
			Exit Function
		End If
		getLoginMsg = ""
	End Function
End Class	
%>
