<%
Class tNG_DeleteDetailRec
	Public tNG  ' object
	Public table
	Public field

	Private Sub Class_Initialize()
		Set this = Me
	End Sub
	
	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (ByRef tNG__param)
		Set tNG = tNG__param
		table = "mytable"
		field = "myfield"
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

	Public Function Execute()
		pk = tNG.getPrimaryKey()
		pk_value = tNG.getPrimaryKeyValue()
		pk_type = tNG.getColumnType(tNG.getPrimaryKey())
		pk_value = KT_escapeForSql (pk_value, pk_type)

		sql  = "DELETE FROM " & table  & " WHERE " & KT_escapeFieldName (field) & " = " & pk_value

		On Error Resume Next
		Set ret = tNG.connection.Execute(sql)
		If err.Number <> 0 Then
			Set myErr = new tNG_error
			myErr.Init "DEL_DR_SQL_ERROR", array(), array(err.Description, sql)
			Set Execute = myErr
			On Error GoTo 0
			Exit Function
		End If	
		Set Execute = nothing
	End Function
End Class	
	
%>