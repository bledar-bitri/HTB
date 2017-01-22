<%
Class TOR_SetOrder
	Private connectionString
	Private connection
	Private tableName
	Private pk
	Private pkType				' STRING_TYPE or NUMERIC_TYPE
	Private orderField
	Private orderPostField

	Private Sub Class_Initialize()

	End Sub
	
	Private Sub Class_Terminate()
	End Sub
	
	Public Sub Init(connection__param, tableName__param, pk__param, pkType__param, orderField__param, orderPostField__param)
		connectionString = connection__param
		
		Set connection = KT_GetPooledConnection(connectionString)
		KT_setDbType connection
		
		tableName = tableName__param
		pk = pk__param
		pkType = pkType__param
		orderField = orderField__param
		orderPostField = orderPostField__param
		
		KT_SessionKtBack(KT_getFullUri())
	End Sub
	
	Public Function scriptDefinition()
		scriptDefinition  = "<script type=""text/javascript"" language=""javascript"">" & vbNewLine & _
			"$NXT_MOVE_SETTINGS = {" & vbNewLine & _
			"	orderfield: '"  & orderPostField & "'" & vbNewLine & _
			"}" & vbNewLine & _
			"</script>"
	End Function
	
	
	Public Function getOrderFieldName()
		getOrderFieldName = "order_" & pk  &  "_" & orderField
	End Function


	Public Function getOrderFieldValue(ByRef obj)
		getOrderFieldValue = obj.Fields.Item(orderField).Value & "|" & obj.Fields.Item(orderField).Value
	End Function


	Public Sub Execute()
		If KT_isSet(Request.Form(orderPostField)) Then
			permArr = array()
			arr = split (Request.Form(orderPostField), ",")
			For i = 0 to ubound(arr)
				val = arr(i)
				arrParts = split(val, "|")
				If ubound(arrParts) = 2 Then
					If arrParts(1) <> arrParts(2) Then
						permArr = KT_array_push(permArr, arrParts)
					End If
				End If
			Next
			
			If ubound(permArr) <> -1 Then
				Dim sql
				sql = "SELECT max(" & KT_escapeFieldName(orderField) & ") +1 AS kt_tor_max FROM " & tableName
				On Error Resume Next
				Set rs = connection.Execute(sql)
				If Err.Number <> 0 Then
					Response.Write "Internal Error. Table Order:<br/>" & Err.Description
					Response.End()
				End If
				On Error GoTo 0
				max = 0
				On Error Resume Next
				max = Clng(rs("kt_tor_max"))
				On Error GoTo 0
				
				For i=0 to ubound(permArr)
					UpdateOrder permArr(i)(0), permArr(i)(1) + max
				Next
				For i=0 to ubound(permArr)
					UpdateOrder permArr(i)(0), permArr(i)(2)
				Next
			End If
			
			
			Dim url
			url = Request.ServerVariables("URL")
			If Request.ServerVariables("QUERY_STRING") <> "" Then
				url = url & "?" & Request.ServerVariables("QUERY_STRING")
			End If
			KT_redir(url)
		End If
	End Sub
	

	Public Sub UpdateOrder (id, order)
		Dim sql
		sql = "UPDATE " &  tableName & " SET " &  KT_escapeFieldName(orderField)  & " = " &  KT_escapeForSql(order, "NUMERIC_TYPE") & " WHERE " & KT_escapeFieldName(pk)  & " = " &  KT_escapeForSql(id, pkType)
		On Error Resume Next
		connection.Execute(sql)
		If Err.Number <> 0 Then
			Response.Write "Internal Error. Table Order:<br/>" & Err.Description
			Response.End()
		End If
		On Error GoTo 0
	End Sub

End Class
%>
