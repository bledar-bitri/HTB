<%
Class tNG_RestrictAccess
	Private relPath
	Private levels
	Private connection
	
	Private Sub Class_Initialize()
		relPath = ""
		levels = array()
		connection = ""
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (connection__param, relPath__param)
		connection = connection__param
		relPath = KT_makeIncludedURL(relPath__param)
	End Sub

	Public Sub addLevel(level)
		levels = KT_array_push(levels, level)
	End Sub

	Public Sub Execute() 
		tNG_cookieLogin (connection)

		' access denied defaults to "redirect_failed" specified in Login Config
		grantAccess = false
		redirect_page = tNG_login_config("redirect_failed")
		
		If Session("kt_login_user") <> "" Then
			If Ubound(levels) <> -1 Then
				If Session("kt_login_level") <> "" Then
					If KT_in_array(Session("kt_login_level"), levels, false) Then
						grantAccess = true
					Else
						' acceess denied. check for level default redirect pages
						redirect_page = tNG_login_config("redirect_failed") ' fallback to default
						If tNG_login_config_redirect_failed.Exists(Session("kt_login_level")) Then
							If  tNG_login_config_redirect_failed(Session("kt_login_level")) <> "" Then
								redirect_page = tNG_login_config_redirect_failed(Session("kt_login_level"))
							End If	
						End If
					End If
				End If ' // if levels are required, and the current user doesn't have one.. access is denied
			Else
				' no levels are required for this page access
				' the user is logged in, so grant the access
				grantAccess = true
			End If
		End If

		If Not grantAccess Then
			' save the accessed page into a session for later use
			url = Request.ServerVariables("URL")
			If Request.ServerVariables("QUERY_STRING") <> "" Then
				url = url & "?" & Request.ServerVariables("QUERY_STRING")
			End If
			Session("KT_denied_pageuri") = url
			Session("KT_denied_pagelevels") = levels
			
			If instr(redirect_page, "?") = 0 Then
	 			redirect_page = redirect_page & "?"
			Else
				redirect_page = redirect_page & "&"
			End If
			redirect_page = redirect_page & "info=DENIED"
			KT_redir (relPath & redirect_page)
		Else
			' clear the sessions used for redirect ??
		End If	
	End Sub
End Class	
%>