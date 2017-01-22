<%

'NAME:
'	KT_Email
'DESCRIPTION:
'	Sends emails	

' CDO constants
' IMPORTANCE
'	Const cdoLow  = 0  ' Low importance 
'	Const cdoNormal = 1  ' Normal importance 
'	Const cdoHigh = 2 ' High importance 

' PRIORITY
'	Const cdoPriorityNonUrgent = -1 ' Not urgent 
'	Const cdoPriorityNormal  = 0  ' Normal priority 
'	Const cdoPriorityUrgent = 1 ' Urgent 
		
' X-PRIORITY		
'	Const XPriorityHigh = 1 
'	Const XPriorityNormal  = 3
'	Const XPriorityLow = 5

Class KT_Email
	Private userErrorMessage		'array		-	error message to be displayed as User Error
	Private develErrorMessage		'array		-	error message to be displayed as Developer Error
	
	
	Private priorityName			' the priorityName
	
	Private Sub Class_Initialize()
		userErrorMessage	= Array()
		develErrorMessage	= Array()
		
		priorityName = "Normal"
	End Sub

	Private Sub Class_terminate()
	End Sub

	Public Sub SendEmail (serv, port, user, password, from, to2, cc, bcc, subject, encoding, textBody, htmlBody )
		If trim(serv) = "" Then
			setError "ASP_EMAIL_ERROR_NOCONFIG", Array(), Array()
			Exit Sub
		End If
		' type of send
		Const cdoSendUsingPickup = 1 
		Const cdoSendUsingPort = 2 
		Const cdoAnonymous = 0	

		' Use basic (clear-text) authentication. 
		Const cdoBasic = 1
		' Use NTLM authentication 
		Const cdoNTLM = 2 'NTLM

		' init mail object
		Dim objMessage
		
		On Error Resume Next
		Set objMessage = CreateObject("CDO.Message")
		If Err.Number <> 0 Then
			setError "ASP_EMAIL_MISSING_OBJECT", Array(), Array()
			On Error GoTo 0
			Exit Sub
		End If
		On Error GoTo 0
		
		' config
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/sendusing") = cdoSendUsingPort		 
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpserver") = trim(serv)
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = cdoBasic
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/sendusername") = trim(user)
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/sendpassword") = trim(password)
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = trim(port) 	
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = False
		'Set the number of seconds to wait for a valid socket to be established with the SMTP service before timing out.
		objMessage.Configuration.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout") = 60			
		objMessage.Configuration.Fields.Update
		
		objMessage.Fields.Item("urn:schemas:mailheader:to")    				 =  trim(to2)
		objMessage.Fields.Item("urn:schemas:mailheader:from")   			 =  trim(from)
		objMessage.Fields.Item("urn:schemas:mailheader:cc")   				 =  trim(cc)
		objMessage.Fields.Item("urn:schemas:mailheader:bcc")   				 =  trim(bcc)
		objMessage.Fields.Item("urn:schemas:mailheader:subject")			 =  trim(subject) 
		'objMessage.Fields.Item("urn:schemas:httpmail:priority")			 =	getPriorityNumber()
		'objMessage.Fields.Item("urn:schemas:httpmail:importance")			 =  getImportanceNumber()
		objMessage.Fields.Item("urn:schemas:mailheader:X-Priority")			 =  getXPriorityNumber()
		'objMessage.Fields.Item("urn:schemas:mailheader:X-MSMail-Priority")   =  priorityName
		objMessage.Fields.Item("urn:schemas:mailheader:X-Mailer")   =  "InterAKT tNG mailer"
		objMessage.Fields.Update


		If htmlBody <> "" Then
			'Setting the HTMLBody property causes the TextBody property to be set immediately.
			
			objMessage.HTMLBody = breakOnMultilines(htmlBody)
			
			Set ihbp = objMessage.HTMLBodyPart 	
			ihbp.Charset = encoding
			
			Set itbp = objMessage.TextBodyPart 	
			itbp.Charset = encoding
		End If
			
		If textBody <> "" Then
			objMessage.TextBody = breakOnMultilines(textBody)
			
			Set itbp = objMessage.TextBodyPart 	
			itbp.Charset = encoding
		End If	
	  
		On Error resume next
		objMessage.Send
		If err.number <> 0 Then
			error = err.Description
			setError "EMAIL_ERROR_SENDING", Array(), Array(error)
		End If				
		On Error GoTo 0
		Set objMessage = nothing	
	End Sub
	
	Private Function breakOnMultilines (strText)	
		MaxLineLength = 900
		
		If len(strText) < MaxLineLength Then
			breakOnMultilines = strText
			Exit Function
		Else
			' must break the text on mutiple lines
			strOutput = ""
			leftPart = strText
			ready = false
			
			do while not ready
				rightPart = mid (leftPart, MaxLineLength + 1)
				leftPart = left (leftPart, MaxLineLength)
				If rightPart <> "" Then
					If left(rightPart, 1) <> " " Then
						lastSpacePoz = instrrev(leftPart, " ")
						If lastSpacePoz <> 0 Then
							rightPart = mid (leftPart, lastSpacePoz + 1) & rightPart
							leftPart = left (leftPart, lastSpacePoz)
						End If	
					End If
				End If
				
				strOutput = strOutput & leftPart & vbNewLine
				
				If len(rightPart) < MaxLineLength Then
					ready = true
					strOutput = strOutput & rightPart 
				Else	
					leftPart = rightPart
				End If
			loop

			breakOnMultilines = strOutput					
		End If
	End Function
	

	Public Sub setPriority ( pName )
		If lcase(pName) = "low"  or lcase(pName)  = "high" Then
			priorityName = pName
		Else
			priorityName = "Normal"
		End If
	End Sub
	
	Private Function getPriorityNumber ()
		Const cdoPriorityNonUrgent = -1 ' Not urgent 
		Const cdoPriorityNormal  = 0  ' Normal priority 
		Const cdoPriorityUrgent = 1 ' Urgent 
	
		Select case lcase(priorityName)
			case "high"
				getPriorityNumber = cdoPriorityUrgent
			case "low"
				getPriorityNumber = cdoPriorityNonUrgent
			case else
				getPriorityNumber = cdoPriorityNormal				
		End Select
	End Function

	Private Function getXPriorityNumber ()
		Const XPriorityHigh = 1 
		Const XPriorityNormal  = 3
		Const XPriorityLow = 5
	
		Select case lcase(priorityName)
			case "high"
				getXPriorityNumber = XPriorityHigh
			case "low"
				getXPriorityNumber = XPriorityLow
			case else
				getXPriorityNumber = XPriorityNormal				
		End Select
	End Function
	
	Private Function getImportanceNumber ()
		Const cdoLow  = 0  ' Low importance 
		Const cdoNormal = 1  ' Normal importance 
		Const cdoHigh = 2 ' High importance 
		
		Select case lcase(priorityName)
			case "high"
				getImportanceNumber = cdoHigh
			case "low"
				getImportanceNumber = cdoLow
			case else
				getImportanceNumber = cdoNormal				
		End Select
	End Function
		
				
	Private Sub setError (errorCode, arrArgsUsr, arrArgsDev)
		errorCodeDev = errorCode
		If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
			errorCodeDev = errorCodeDev & "_D"
		End If
		If errorCode <> "" Then
			userErrorMessage = KT_array_push (userErrorMessage, KT_getResource(errorCode, "Email", arrArgsUsr))
		Else
			userErrorMessage = array()
		End If
		
		If errorCodeDev <> "" Then
			develErrorMessage = KT_array_push (develErrorMessage, KT_getResource(errorCodeDev, "Email", arrArgsDev))
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
