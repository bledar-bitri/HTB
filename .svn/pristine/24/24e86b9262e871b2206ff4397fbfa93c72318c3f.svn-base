<%
Class MXI_Includes
	Private urlParamName
	Private urlParamValue
	Private pages
	Private includedFile	
	

	Private Sub Class_Initialize()
		urlParamName = "mod"
		urlParamValue = ""
		Set pages = Server.CreateObject("Scripting.Dictionary")
		includedFile = null
	End Sub
	
	Private Sub Class_Terminate()

	End Sub
	
	
	Public Sub Init(urlParamName__param)
		If urlParamName__param <> "" Then
			urlParamName = urlParamName__param
		End If	
		urlParamValue = ""
		
		If Not isnull(urlParamName) And Request.QueryString(urlParamName) <> "" Then
			urlParamValue = Request.QueryString(urlParamName)
		End If
	End Sub
	
	
	Public Sub IncludeStatic(url, file, title, keywords, description)
		Set pages(url) = Server.CreateObject("Scripting.Dictionary")
			pages(url)("file") = file
			pages(url)("title") = title
			pages(url)("keywords") = keywords
			pages(url)("description") = description
	End Sub

	
	Public Sub IncludeDynamicRecordset (ByRef rs, urlField, fileField, titleField, keywordsField, descriptionField)
		' check the fields
		If not rs.EOF Then
			On Error Resume Next

			val = rs.Fields.Item(urlField).Value
			If Err.Number <> 0 Then
				Response.write KT_getResource ("MISSING_FIELD", "MXI", array (urlField))
				Response.End()
			End If
			val = rs.Fields.Item(fileField).Value			
			If Err.Number <> 0 Then
				Response.write KT_getResource ("MISSING_FIELD", "MXI", array (fileField))
				Response.End()
			End If
			
			If titleField <> "" Then	
				val = rs.Fields.Item(titleField).Value
				If Err.Number <> 0 Then
					Response.write KT_getResource ("MISSING_FIELD", "MXI", array (titleField))
					Response.End()
				End If
			End If			
			
			If keywordsField <> "" Then
				val = rs.Fields.Item(keywordsField).Value
				If Err.Number <> 0 Then
					Response.write KT_getResource ("MISSING_FIELD", "MXI", array (keywordsField))
					Response.End()
				End If
			End If
			If descriptionField <> "" Then
				val = rs.Fields.Item(descriptionField).Value
				If Err.Number <> 0 Then
					Response.write KT_getResource ("MISSING_FIELD", "MXI", array (descriptionField))
					Response.End()
				End If
			End If	
			
			On Error GoTo 0
		End If
		
		While Not rs.EOF
			url = ""
			file = ""
			title = ""
			keywords = ""
			description = ""		
			
			'On Error Resume Next
			url = rs.Fields.Item(urlField).Value		
			file = rs.Fields.Item(fileField).Value		
			If titleField <> "" Then	
				title = rs.Fields.Item(titleField).Value		
			End If
			If keywordsField <> "" Then
				keywords = rs.Fields.Item(keywordsField).Value		
			End If
			If descriptionField <> "" Then
				description = rs.Fields.Item(descriptionField).Value		
			End If	
			'On Error GoTo 0
			
			IncludeStatic url, file, title, keywords, description
			rs.MoveNext
		Wend
	End Sub
	
	Public Sub IncludeDynamic (connStr, tableName, urlField, fileField, titleField, keywordsField, descriptionField)
		Dim conn: Set conn = Server.CreateObject("ADODB.Connection")
		conn.ConnectionString = connStr
		conn.Open
		KT_setDbType conn ' set global variable KT_DatabaseType

		Dim sql
		sql = "SELECT " & KT_escapeFieldName(urlField) & ", " &  KT_escapeFieldName(fileField)
		If titleField <> "" Then
			sql = sql & ", " & KT_escapeFieldName(titleField)
		End If
		If keywordsField <> "" Then
			sql = sql & ", " & KT_escapeFieldName(keywordsField)
		End If
		If descriptionField <> "" Then
			sql = sql & ", " & KT_escapeFieldName(descriptionField)
		End If
		sql = sql  &  " FROM " & tableName
		
		On Error Resume Next
		Set localRs = conn.Execute(sql)
		If Err.Number <> 0 Then
			res_errorMsg = KT_getResource ("SQL_ERROR", "MXI", array (err.description, sql))
			Response.write res_errorMsg
			Response.End()
		End If
		On Error GoTo 0
		IncludeDynamicRecordset localRs, urlField, fileField, titleField, keywordsField, descriptionField
	End Sub

	Public Function getKeywords()
		Dim ret
		ret = ""
		If Not isnull(getCurrentInclude())  Then
			ret = pages(urlParamValue)("keywords")
		End If
		getKeywords = KT_escapeAttribute(ret)
	End Function	

	
	Public Function getDescription()
		Dim ret
		ret = ""
		If Not isnull(getCurrentInclude()) Then
			ret = pages(urlParamValue)("description")
		End If
		getDescription = KT_escapeAttribute(ret)
	End Function
	
	
	Public Function getTitle()
		Dim ret
		ret = ""
		If Not isnull(getCurrentInclude()) Then
			ret = pages(urlParamValue)("title")
		End If
		getTitle = ret
	End Function
	
	
	Public Function getCurrentInclude()
		Dim ret
		If isNull(includedFile) Then
			ret = null
			
			If pages.Exists(urlParamValue) Then
				ret = pages(urlParamValue)("file")

				On Error Resume Next
				retPath = Server.MapPath(ret) 
				If Err.Number = 0 Then
					On Error GoTo 0
					Set fso = Server.CreateObject("Scripting.FileSystemObject")
					If Not fso.FileExists(retPath) Then
						ret = null
					End If
					Set fso = nothing
				Else
					On Error GoTo 0	
					ret = null
				End If
			End If			

			If isnull(ret) Then
				param404 = "404"
				If pages.Exists(param404) Then
					ret = pages(param404)("file")
					urlParamValue = "404"

					On Error Resume Next
					retPath = Server.MapPath(ret) 
					If Err.Number = 0 Then
						On Error GoTo 0
						Set fso = Server.CreateObject("Scripting.FileSystemObject")
						If Not fso.FileExists(retPath) Then
							ret = null
						End If
						Set fso = nothing
					Else
						On Error GoTo 0	
						ret = null
					End If
				End If
				includedFile = ret
			End If
		Else
			ret = includedFile
		End If
		getCurrentInclude = ret
	End Function

End Class
%>