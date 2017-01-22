<%
'
'	Copyright (c) InterAKT Online 2000-2005
'


Class tNG_ThrowError
	Private tNG
	Private errorMsg
	Private fieldErrorMsg
	Private field
	
	Private Sub Class_Initialize()
	End Sub
	Private Sub Class_Terminate()
	End Sub	
	
	Public Sub Init(ByRef tNG__par)
		Set tNG = tNG__par
		errorMsg = ""
		field = ""
		fieldErrorMsg = ""
	End Sub

	Public Sub setErrorMsg(errorMsg__par)
		errorMsg = errorMsg__par
	End Sub
	
	Public Sub setFieldErrorMsg(fieldErrorMsg__par)
		fieldErrorMsg = fieldErrorMsg__par
	End Sub

	Public Sub setField(field__par)
		field = field__par
	End Sub

	Public Function Execute()
		useSavedData = false
		If KT_in_array(tNG.transactionType, Array("_delete", "_multipleDelete"), false) Then
			useSavedData  = true
		End If
		errorMsg = tNG_DynamicData(errorMsg, tNG, "", useSavedData, null, null)
		fieldErrorMsg = tNG_DynamicData(fieldErrorMsg, tNG, "", useSavedData, null, null)

		Set cols = tNG.columns
		If cols.Exists(field) Then
			If cols(field)("method") = "POST" Then
				' set field error to $this->errorMsg
				' set user error to $message
				Set myErr = new tNG_error
				myErr.Init "%s", array(errorMsg), array("")
				myErr.setFieldError field, "%s", array(fieldErrorMsg)
			End If
		End If
		
		If not KT_isSet(myErr) Then
			' don't set field error
			' set composed message as user error
			Set myErr = new tNG_error
			myErr.Init "%s", array(errorMsg), array("")
			myErr.addDetails "%s", array(fieldErrorMsg), array("")
		End If
		
		Set Execute = myErr
	End Function
End Class

%>