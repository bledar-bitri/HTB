<%
Class tNG_Logout
	Private logoutType
	Private pageRedirect
	
	Private Sub Class_Initialize()
		logoutType = "load"
		pageRedirect = ""
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub setLogoutType(logoutType__param) 
		logoutType = logoutType__param
	End Sub

	Public Sub setPageRedirect (pageRedirect__param)
		pageRedirect = pageRedirect__param
	End Sub

	Private Sub unsetAll()
		Session.Contents.Remove("kt_login_id")
		Session.Contents.Remove("kt_login_level")
		Session.Contents.Remove("kt_login_user")

		Session.Contents.Remove("kt_denied_pageuri")
		Session.Contents.Remove("kt_denied_pagelevels")

		For each ses_name in tNG_login_config_session
			Session.Contents.Remove(ses_name)
		Next

		' remove cookies
		Response.Cookies("kt_login_id") = ""
		Response.Cookies("kt_login_id").Expires = Date - 1
		Response.Cookies("kt_login_id").Path = KT_GetSitePath()
		
		Response.Cookies("kt_login_test") = ""
		Response.Cookies("kt_login_test").Expires = Date - 1
		Response.Cookies("kt_login_test").Path = KT_GetSitePath()


	End Sub

	Public Sub Execute()
		' remove sessions

		If lcase(logoutType) = "load" Then
			unsetAll
			If pageRedirect <> "" Then
				KT_redir(pageRedirect)
			End If	
		Else
			If  Request.QueryString("KT_logout_now") = "true" Then
				unsetAll
				If pageRedirect <> "" Then
					KT_redir (KT_makeIncludedURL(pageRedirect))
				Else
					' redirect to self - after removing value for KT_logout_now
					url = Request.ServerVariables("URL")
					If Request.ServerVariables("QUERY_STRING") <> "" Then
						url = url & "?" & Request.ServerVariables("QUERY_STRING")
					End If
					KT_redir (KT_addReplaceParam(url, "KT_logout_now", ""))
				End If
			End If
		End If
	End Sub

	Public Function getLogoutLink()
		url = Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			url = url & "?" & Request.ServerVariables("QUERY_STRING")
		End If
		getLogoutLink = KT_addReplaceParam(url, "KT_logout_now", "true")
	End Function
End Class	
%>