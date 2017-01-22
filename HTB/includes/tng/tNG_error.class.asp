<%
'
'	Copyright (c) InterAKT Online 2000-2005
'


Class tNG_error
	Private devDetails
	Private details
	Private fieldErrors
	
	Private Sub Class_Initialize()
		Set fieldErrors = Server.CreateObject("Scripting.Dictionary")
	End Sub
	Private Sub Class_Terminate()
	End Sub	
	
	Public Sub Init(errorCode, arrArgsUsr, arrArgsDev)
		errorCodeDev = errorCode
		If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
			errorCodeDev = errorCodeDev & "_D"
		End If
		res_details = KT_getResource(errorCode, "tNG", arrArgsUsr)
		res_devDetails = KT_getResource(errorCodeDev, "tNG", arrArgsDev)
		
		details = res_details
		devDetails = res_devDetails
		If errorCode <> "%s" and errorCode <> "" and res_devDetails <> "" Then
			devDetails = devDetails & " (" & errorCode & ")"
		End If
	End Sub

	Public Sub setFieldError (fieldName, errorCode, arrArgs)
		res_errorMsg = KT_getResource(errorCode, "tNG", arrArgs)
		fieldErrors(fieldName) = res_errorMsg
	End Sub


	Public Sub addFieldError (fieldName, errorCode, arrArgs)
		res_errorMsg = KT_getResource(errorCode, "tNG", arrArgs)
		
		If not fieldErrors.Exists(fieldName) Then
			fieldErrors(fieldName) = res_errorMsg
		Else
			fieldErrors(fieldName) = fieldErrors(fieldName) &  "<br />" & res_errorMsg
		End If
	End Sub

	Public Function getFieldError (fieldName)
		If fieldErrors.Exists(fieldName) Then
			getFieldError = fieldErrors (fieldName)
		Else
			getFieldError = null
		End If
	End Function

	Public Sub setDetails (errorCode, arrArgsUsr, arrArgsDev)
		errorCodeDev = errorCode
		If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
			errorCodeDev = errorCodeDev & "_D"
		End If
		res_details = KT_getResource(errorCode, "tNG", arrArgsUsr)
		res_devDetails = KT_getResource(errorCodeDev, "tNG", arrArgsDev)
		
		details = res_details
		devDetails = res_devDetails
		If errorCode <> "%s" and errorCode <> "" and res_devDetails <> "" Then
			devDetails = devDetails & " (" & errorCode & ")"
		End If
	End Sub


	Public Sub addDetails (errorCode, arrArgsUsr, arrArgsDev)
		If details <> "" Then
			details = details & "<br />"
		End If
		If devDetails <> "" Then
			devDetails = devDetails & "<br />"
		End If
	
		errorCodeDev = errorCode
		If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
			errorCodeDev = errorCodeDev & "_D"
		End If
		res_details = KT_getResource(errorCode, "tNG", arrArgsUsr)
		res_devDetails = KT_getResource(errorCodeDev, "tNG", arrArgsDev)
		
		details = details & res_details
		devDetails = devDetails & res_devDetails
		If errorCode <> "%s" and errorCode <> "" and res_devDetails <> "" Then
			devDetails = devDetails & " (" & errorCode & ")"
		End If
	End Sub

	
	Public Function getDetails 
		getDetails 	= details
	End Function
	
	Public Function getDeveloperDetails 
		getDeveloperDetails = devDetails
	End Function
End Class

%>