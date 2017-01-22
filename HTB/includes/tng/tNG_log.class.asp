<%

	Dim tNG_log__KT_logArray: Set tNG_log__KT_logArray = Server.CreateObject("Scripting.Dictionary" )
	tNG_log__Index = 0
	
	Sub tNG_log__log (className, methodName, message)
		 tNG_log__KT_logArray (tNG_log__Index) = Array (className, methodName, message)
		 tNG_log__Index = tNG_log__Index + 1
	End Sub
	
	Function tNG_log__getResult (mode)
		Dim ret
		alt = ""
		ret = "<ul id=""KT_tngtrace_details"">" & vbNewLine
		depth = 2

		For each i in tNG_log__KT_logArray
			i_logarr = tNG_log__KT_logArray (i)
			If Not KT_isSet(i_logarr) Then
				Exit For
			End If
			ipp = Cint(i) + 1
			ipp_logarr = tNG_log__KT_logArray ( ipp )

			if KT_isSet(ipp_logarr) then
				If ipp_logarr(0) = "KT_ERROR" Then
					alt = " style=""color:red"""
				End If
			End If
			If i_logarr(0) = "KT_ERROR" Then
				alt = ""
			Else
				If i_logarr(2) = "begin" Then
					ret = ret & String(depth, " ") & "<li" & alt & ">" & i_logarr (0) & "." & i_logarr(1)
					If alt <> "" Then 
						ret = ret & "*"
					End If
					ret = ret & "</li>" & vbNewLine
					ret = ret & "<ul>" & vbNewLine
					depth = depth + 2
				Elseif i_logarr(2) = "end" Then
					ret = ret & "</ul>" & vbNewLine
					depth = depth - 2
				Else
					If not isNull(i_logarr(2)) Then
						ret = ret &  String(depth, " ") & "<li"  & alt & ">" & i_logarr(0) & "." & i_logarr(1) & " - " & i_logarr(2) 
						If alt <> "" Then 
							ret = ret & "*"
						End If						
						ret = ret & "</li>" & vbNewLine
					Else
						ret = ret &  String(depth, " ") & "<li"  & alt & ">" & i_logarr(0) & "." & i_logarr(1) 
						If alt <> "" Then 
							ret = ret & "*"
						End If						
						ret = ret & "</li>" & vbNewLine
					End If
				End If
			End If
		Next
		ret = ret & "</ul>" & vbNewLine
		If mode = "text" Then
			ret = KT_preg_replace("([" & vbNewLine & "]{2,})", vbNewLine, KT_strip_tags(ret))
		End If
		tNG_log__getResult = ret
	End Function
	
	
%>
