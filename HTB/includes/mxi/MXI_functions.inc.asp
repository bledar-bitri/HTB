<%
	KT_MXI_parse_css  = True
	
	Function mxi_getBaseURL()
		Dim ret: ret = KT_getUri()
		pos = instrrev(ret, "/")
		If pos<>0 Then
			ret = left(KT_getUri, pos)
		End If
		mxi_getBaseURL = ret
	End Function	
	
	Function mxi_ParseHtml (text, relpath)
		Dim content
		content = text
		
		content = KT_transformsPaths(relpath, content)
		
		' get the <body>
		start_body = instr (1, content, "<body", 1)
		If start_body <> 0  Then
			content_before_body = left(content, start_body-1)

			' get the CSSes
			If KT_MXI_parse_css Then
				css_before_body = vbNewLine

				Dim link_matches	
				KT_INCLUDEENGINE_preg_match "<link([^>]*)>", content_before_body, link_matches
				For Each Match in link_matches
					css_before_body = css_before_body & match.Value & vbNewLine
				Next
				css_before_body = css_before_body & vbNewLine
				
				
				Dim style_matches
				KT_INCLUDEENGINE_preg_match "<style([^>]*)>", content_before_body, style_matches
				For Each Match in style_matches
					style_start = match.firstIndex
					style_end = instr(style_start + 1, content_before_body, "</style>", 1) 
					If style_end <> 0 Then
						style_length = style_end + len("</style>") - (style_start) 
						css_before_body = css_before_body & mid(content_before_body, style_start, style_length) & vbNewLine
					End If
				Next
			End If

			' get the JavaScripts
			script_before_body = ""
			KT_INCLUDEENGINE_preg_match "<script([^>]*)>", content_before_body, script_matches
			For Each Match in script_matches
				script_start = match.firstIndex
				script_end = instr(script_start + 1, content_before_body, "</script>", 1)
				script_length = script_end + len("</script>") - (script_start + 1) 
				source_code = mid(content_before_body, script_start + 1, script_length)
				script_before_body = script_before_body & source_code & vbNewLine
			Next

		
			before_body = css_before_body & script_before_body

			
            end_body_tag = instr (start_body + 1,content, ">")
			start_body = end_body_tag  + 1
			end_body = instr (end_body_tag, content, "</body>", 1)
			If end_body <> 0 Then
				mxi_ParseHtml = before_body & mid (content, start_body, end_body-start_body)
			Else
				mxi_ParseHtml = before_body & mid (content, start_body)	
			End If
		Else
			mxi_ParseHtml = content
		End If
		
		
		' solve relative includes
		'relpath = replace(relpath, "\", "/")
		'If left(relpath, 1) <> "/" Then
		'	pos = InstrRev(relpath, "/")
		'	If pos <> 0 Then
		'		relpath = left(relpath, pos)
		'		'build pattern	
		'		relpathPattern = KT_INCLUDEENGINE_preg_replace("[^/]+", "\.\.",  relpath)
		'		mxi_ParseHtml = KT_INCLUDEENGINE_preg_replace( "(src|href)\s*=\s*([""']?)" & relpathPattern & "(\w)", "$1=$2$3",  mxi_ParseHtml)
		'	End If
		'End If 
	End Function

%>