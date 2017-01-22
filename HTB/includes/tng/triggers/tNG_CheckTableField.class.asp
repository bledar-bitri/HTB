<%
Class tNG_CheckTableField
	Public tNG  ' object
	Public table
	Public field
	Public fieldtype
	Public value
	Public errorMsg
	Public throwErrorIfExists

	Private Sub Class_Initialize()
		Set this = Me
	End Sub
	
	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (ByRef tNG__param)
		Set tNG = tNG__param
		table = "mytable"
		field = "myfield"
		fieldtype = "NUMERIC_TYPE"
		value = -1
		errorMsg = ""
		throwErrorIfExists = false
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
	
	Public Sub setTable (table__param)
		table = table__param
	End Sub

	Public Sub setFieldName (field__param)
		field = field__param
	End Sub

	Public Sub setFieldType (fieldtype__param)
		fieldtype = fieldtype__param
	End Sub

	Public Sub setFieldValue (value__param)
		value = value__param
	End Sub

	Public Sub errorIfExists (throwErrorIfExists__param)
		throwErrorIfExists = throwErrorIfExists__param
	End Sub
	
	Public Sub setErrorMsg (errorMsg__param)
		errorMsg = errorMsg__param
	End Sub



	Public Function Execute()
		field_value = KT_escapeForSql(value, fieldtype)
		sql = "SELECT " & KT_escapeFieldName (field) & " FROM " & table & " WHERE " & KT_escapeFieldName(field) & " = " & field_value
		On Error Resume Next
		Set ret = tNG.connection.Execute(sql)
		If err.Number <> 0 Then
			Set myErr = new tNG_error
			myErr.Init "CHECK_TF_SQL_ERROR", array(), array(err.Description, sql)
			Set Execute = myErr
			On Error GoTo 0
			Exit Function
		End If	
		
		useSavedData = false
		If KT_in_array(tNG.transactionType, Array("_delete", "_multipleDelete"), false) Then
			useSavedData  = true
		End If
		
		If throwErrorIfExists And Not ret.EOF Then
			Set myErr = new tNG_error
			myErr.Init "DEFAULT_TRIGGER_MESSAGE", array(), array()
			Set Execute = myErr
			On Error GoTo 0
			Exit Function
		End If	
		
		If Not throwErrorIfExists And ret.EOF Then
			Set myErr = new tNG_error
			myErr.Init "DEFAULT_TRIGGER_MESSAGE", array(), array()
			Set Execute = myErr
			On Error GoTo 0
			Exit Function
		End If
		
		Set Execute = nothing
	End Function
End Class	
	
%>