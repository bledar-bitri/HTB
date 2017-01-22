<!-- #include file = "../common/KT_common.asp" -->
<%
	If KT_isSet(Request.QueryString("KT_back")) Then
		Dim url
		url = KT_getFullUri()
		tmp = KT_addReplaceParam(url, "KT_back", "")

		If KT_isSet(Request.ServerVariables("HTTP_REFERER")) Then
			backURL = Request.ServerVariables("HTTP_REFERER")
			KT_SessionKtBack(backURL)
		End If

		If KT_isSet(Request.Form("KT_Delete1")) Then
			strToPrint = "<html>" & vbNewLine & _
			"<head>" & vbNewLine & _
			"</head>" & vbNewLine & _
			"<body>" & vbNewLine & _
				"<form action=""" & tmp & """ method=""POST""  name=""KT_backForm"">" & vbNewLine
				For each key in Request.Form
					'For idx = 1 to Request.Form(key).Count	
					If key = "KT_Delete1" Or instr(key, "kt_pk_") = 1 Then
						value = Request.Form(key)
						
						strToPrint = strToPrint & "<input type=""hidden"" name=""" & key & """ value=""" & KT_escapeAttribute(value) & """ />" & vbNewLine

					 End If	
					'Next
				Next
				
				strToPrint = strToPrint & "</form>"  & vbNewLine & _
				"<script>"  & vbNewLine & _
					"document.forms.KT_backForm.submit();"  & vbNewLine & _
				"</script>"  & vbNewLine & _
			"</body>"  & vbNewLine & _
			"</html>"
			Response.write strToPrint	
		Else
			KT_redir(tmp)
		End If
		Response.End()
	End If
%>