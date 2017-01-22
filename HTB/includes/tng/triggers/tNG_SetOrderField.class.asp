<%
Class tNG_SetOrderField
	Private tNG
	Private table
	Private field
	Private fType
	
	Private Sub Class_Initialize()
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (ByRef tNG__param)
		Set tNG = tNG__param
		table = tNG.getTable()
		field = "myfield"
		fType = "NUMERIC_TYPE"
	End Sub

	Public Sub setTable(table__param)
		table = table__param
	End Sub
	
	Public Sub setFieldName(field__param)
		field = field__param
	End Sub
	
	Public Function Execute()
		Dim sql
		sql = "SELECT max(" & KT_escapeFieldName(field) & ") + 1 AS kt_sortValue FROM " & table
		On Error Resume Next
		Set ret = tNG.connection.Execute(sql)
		If err.Number <> 0 Then
			Set myErr = new tNG_error
			myErr.Init "SET_ORDER_FIELD_SQL_ERROR", array(), array(err.Description, sql)
			Set Execute = myErr
			On Error GoTo 0
			Exit Function
		End If
		
		value = ret.Fields.Item("kt_sortValue").Value & ""
		If Not isnumeric(value) Then
			value = 1
		End If
		
		If KT_in_array(tNG.getTransactionType(), Array("_insert", "_multipleInsert"), True) Then
			tNG.addColumn field, "NUMERIC_TYPE", "VALUE", value, ""
		Else
			tNG.addColumn field, "NUMERIC_TYPE", "VALUE", value		
		End If	
		Set Execute = nothing
	End Function	
End Class
%>
