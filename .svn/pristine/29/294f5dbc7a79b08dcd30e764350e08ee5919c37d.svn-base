<%
Class tNG_UserLoggedIn
	Private levels
	Private connection
	
	Private Sub Class_Initialize()
		levels = array()
		connection = ""
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (connection__param)
		connection = connection__param
	End Sub

	Public Sub addLevel(level)
		levels = KT_array_push(levels, level)
	End Sub

	Public Function Execute()
		tNG_cookieLogin(connection)
		' access denied defaults to "redirect_failed" specified in Login Config
		grantAccess = false
	
		If Session("kt_login_user") <> ""  Then
			If ubound(levels) <> -1 Then
				If Session("kt_login_level") <> "" Then
					If KT_in_array(Session("kt_login_level"), levels, false) Then
						grantAccess = true
					End If
				End If
			Else	
				' no levels are required for this page access
				' the user is logged in, so grant the access
				grantAccess = true
			End If
		End If
		Execute = grantAccess
	End Function
End Class	
%>