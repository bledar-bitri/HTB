<%
	Class tNG_Redirect
		Public tNG
		Public URL
		Public keepUrlParams
		
		Private Sub Class_Initialize()
		End Sub
		Private Sub Class_Terminate()
		End Sub
		
		Public Sub Init (ByRef tNG__param)
			Set tNG = tNG__param
		End Sub
		
		Public Sub setURL (URL__param)
			URL = URL__param
			If Instr(lcase(URL), "includes/nxt/back.asp") <> 0 Then
				URL = KT_makeIncludedURL(URL)
			End If
		End Sub
		
		Public Sub setKeepURLParams (keepUrlParams__param)
			keepUrlParams = keepUrlParams__param
		End Sub	
		
		Public Function Execute()
			If Not KT_isSet(tNG) Then
				page = tNG_DynamicData(URL, nothing, "rawurlencode", null, null, null)
			Else
				useSavedData = false
				If tNG.getTransactionType() = "_delete" Or tNG.getTransactionType() = "_multipleDelete" Then
					useSavedData = true
				End If
				page = tNG_DynamicData(URL, tNG, "rawurlencode", useSavedData, null, null)
			End If
			
			If keepUrlParams Then
				For each param in Request.QueryString
					value = Request.QueryString(param)
					page = KT_addReplaceParam(page, param, value)
				Next
			End If
			
			KT_redir(page)
		End Function
	End Class
%>