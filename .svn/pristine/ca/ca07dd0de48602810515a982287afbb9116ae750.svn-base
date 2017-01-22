<%
Class TSO_TableSorter
	Private columns
	Private rsName
	Private sorterName
	Private defaultColumn

	Private Sub Class_Initialize()
		Set columns = Server.CreateObject("Scripting.Dictionary")
	End Sub
	
	Private Sub Class_Terminate()
	
	End Sub
	
	
	Public Sub Init(rsName__param, sorterName__param)
		rsName = rsName__param
		sorterName = sorterName__param
		
		KT_SessionKtBack(KT_getFullUri())
	End Sub
	
	Public Sub addColumn(colName)
		If not columns.Exists(colName) Then
			columns(colName) = True
		End If
	End Sub
	
	
	Public Sub setDefault (defaultColumn__param) 
		defaultColumn = defaultColumn__param
		sorter_reference = "sorter_" & sorterName
		If Session(sorter_reference) = "" Then
			Session(sorter_reference) = defaultColumn
		End If
	End Sub
	
	Public Sub Execute()
		sorter_reference = "sorter_" & sorterName
		
		If KT_isSet(Request.QueryString(sorter_reference)) Then
			sorterString = Request.QueryString(sorter_reference)
			columnName = replace(sorterString, " DESC", "")
			If columns.Exists(columnName) Then
				Session(sorter_reference) = Request.QueryString(sorter_reference)
				Dim url
				url = Request.ServerVariables("URL")
				If Request.ServerVariables("QUERY_STRING") <> "" Then
					url = url & "?" & Request.ServerVariables("QUERY_STRING")
				End If
				url = KT_addReplaceParam(url, sorter_reference, "")
				KT_redir(url)
			End If
		End If
	End Sub
	
	
	' Get Current Sort
	Public Function getCurrentSort()
		value = defaultColumn
		sorter_reference = "sorter_" & sorterName
		If Session(sorter_reference) <> "" Then
			value = Session(sorter_reference)
		End If
		getCurrentSort = value
	End Function

	' Get Sort Icon Function
	Function getSortIcon(column)
		value = getCurrentSort()
		If value = column Then
			getSortIcon = "KT_asc"
		ElseIf value = column & " DESC" Then
			getSortIcon = "KT_desc"
		End If
	End Function



	' Get Sort Link Function
	Function  getSortLink(column)
		sorter_reference = "sorter_" & sorterName
		value = getCurrentSort()
		paramVal = column
		If value = column Then
			paramVal = paramVal & " DESC"
		End If
		Dim url
		url = Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			url = url & "?" & Request.ServerVariables("QUERY_STRING")
		End If
		url = KT_addReplaceParam(url, sorter_reference, paramVal)
		getSortLink = url
	End Function

	
End Class
%>