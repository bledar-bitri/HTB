<%
Class NAV_Regular
	Public navName
	Public rsName
	
	Public relPath
	Public currentPage
	
	Public maxRows
	Public pageNum
	Public totalPages
	Public totalRows
	
	Public startRow

	Public StatisticsAndNavigation_Computed
	
	Private Sub Class_Initialize()
		StatisticsAndNavigation_Computed = False
	End Sub
	
	Private Sub Class_Terminate()

	End Sub
	
	
	Public Sub Init(navName__param, rsName__param, relPath__param, currentPage__param, maxRows__param)
		navName = navName__param
		rsName = rsName__param
		
		relPath = relPath__param
		currentPage = currentPage__param
		
		
		If  maxRows__param > 0 Then
			maxRows = maxRows__param
		Else
			maxRows = 1
		End If
		Session("default_max_rows_" & navName) = maxRows
		
		If KT_isSet(Request.QueryString("show_all_" & navName)) Then
			maxRows = 10000
		End If
		Session("max_rows_" & navName) = maxRows
	End Sub
	

	Public Function getShowAllLink()
		Dim url
		url = Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			url = url & "?" & Request.ServerVariables("QUERY_STRING")
		End If
		
		show_all_reference = "show_all_" & navName
		If KT_isSet(Request.QueryString(show_all_reference)) Then
			url = KT_addReplaceParam(url, show_all_reference, "")	
		Else
			url = KT_addReplaceParam(url, show_all_reference, "1")
		End If
		url = KT_addReplaceParam(url, "pageNum_" & rsName, "")
		
		getShowAllLink = url
	End Function
	
	
	
	Public Sub ComputeStatisticsAndNavigation()
		ExecuteGlobal "Dim rsTableName: Set rsTableName = " & rsName
		
		' compute totalRows
		totalRows = 0
		totalRows = rsTableName.RecordCount
		If (totalRows = -1) Then
		  ' count the total records by iterating through the recordset
		  totalRows = 0
		  While (Not rsTableName.EOF)
			totalRows = totalRows + 1
			rsTableName.MoveNext
		  Wend
		
		  ' reset the cursor to the beginning
		  If totalRows <> 0 Then
			  If (rsTableName.CursorType > 0) Then
				rsTableName.MoveFirst
			  Else
				rsTableName.Requery
			  End If
		  End If		
		End If

		' get pageNum
		pageNum  = 0
		If Request.QueryString("pageNum_" & rsName) <> "" Then
			On Error Resume Next
			pageNum = Int ( Request.QueryString("pageNum_" & rsName) )
			On Error GoTo 0
		End If
		
		startRow = pageNum * maxRows
		totalPages = Fix(totalRows / maxRows) - 1
		If totalRows mod maxRows <> 0 Then
			totalPages = totalPages  + 1
		End If

		
		' must move cursor to the selected record
		If totalRows > 0 and pageNum > 0 Then
		  ' move the cursor to the selected record
			KT_index = 0
			While ((Not rsTableName.EOF) And (KT_index < startRow))
				rsTableName.MoveNext
				KT_index = KT_index + 1
			Wend
		End If

		StatisticsAndNavigation_Computed = True
	End Sub
	
	
	
	Public Function checkBoundries() 
		If Not StatisticsAndNavigation_Computed Then
			ComputeStatisticsAndNavigation
		End If
			
		Dim KT_url
		KT_url = Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			KT_url = KT_url & "?" & Request.ServerVariables("QUERY_STRING")
		End If
		
		If pageNum > totalPages And totalPages > -1 Then
			KT_url = KT_addReplaceParam(KT_url, "pageNum_" & rsName, totalPages)
			KT_redir(KT_url)
		End If
		
		If pageNum < 0 Then
			KT_url = KT_addReplaceParam(KT_url, "pageNum_" & rsName, "")
			KT_redir(KT_url)
		End If
				
	End Function


	
	Public Sub Prepare()
		queryString = Request.ServerVariables("QUERY_STRING")
		
		strToMakeGlobal = "" & _
			"nav_rsName = """ & rsName & """" &  vbNewLine & _
			"nav_queryString = """ & queryString & """" &  vbNewLine & _
			vbNewLine & _
			"nav_relPath = """ & relPath & """" &  vbNewLine & _
			"nav_currentPage = """ & currentPage & """" & _
			vbNewLine & _
			"nav_maxRows = " & maxRows &  vbNewLine & _
			"nav_pageNum = " & pageNum &  vbNewLine & _
			"nav_totalPages = " & totalPages &  vbNewLine & _
			"nav_totalRows = " & totalRows &  vbNewLine
		ExecuteGlobal strToMakeGlobal
	End Sub
End Class
%>