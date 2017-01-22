<!-- #include file = "../common/KT_common.asp" -->
<%
	If Not KT_isSet(Session("KT_backArr")) Then
		If KT_isSet(Request.ServerVariables("HTTP_REFERER")) Then
			KT_backArr = array(Request.ServerVariables("HTTP_REFERER"))
			Session("KT_backArr") = KT_backArr
		Else
			Response.write "There is no page set to go back to. Please click the Back link to be redirected to the form. <a href=""javascript: history.go(-1)"">Back</a>"
			Response.End()
		End If	
	Else
		KT_backArr	= Session("KT_backArr")
		If Not IsArray(KT_backArr) Then
			KT_backArr = Array()
		End If
		If UBound(KT_backArr) = -1 Then
			If Session("KT_exBack") <> "" Then
				KT_backArr = KT_array_push(KT_backArr, Session("KT_exBack"))
				Session("KT_backArr") = KT_backArr
			Else
				Response.write "Internal Error"
				Response.End()
			End If
		End If
	End If
	
	KT_backArr	= Session("KT_backArr")

	' pop it
	KT_back = KT_backArr(Ubound(KT_backArr))
	If Ubound(KT_backArr) = 0 Then
		KT_backArr = Array()
	Else
		Redim Preserve KT_backArr(Ubound(KT_backArr)-1)
	End If
	
	If  Ubound(KT_backArr) > -1 And Request.QueryString("KT_back") = "-2" Then
		' pop it
		KT_back = KT_backArr(Ubound(KT_backArr))
		If Ubound(KT_backArr) = 0 Then
			KT_backArr = Array()
		Else
			Redim Preserve KT_backArr(Ubound(KT_backArr)-1)
		End If
	End If

	Session("KT_backArr") = KT_backArr
	Session("KT_exBack") = KT_back

	KT_redir(KT_back)
	Response.End()
%>
