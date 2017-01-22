<%
If isEmpty(KT_CoreFunctions__ALREADYLOADED) Then
	KT_CoreFunctions__ALREADYLOADED = True
	KT_uploadErrorMsg = "<strong>File not found:</strong> <br />###<br /><strong>Please upload the includes/ folder to the testing server.</strong> <br /><a href=""http://www.interaktonline.com/error/?error=upload_includes"" onclick=""return confirm('Some data will be submitted to InterAKT. Do you want to continue?');"" target=""KTDebugger_0"">Online troubleshooter</a>"
	
	Function KT_SetPathSessions()
		' sets 2 sessions: KT_AbsolutePathToRootFolder and KT_SiteURL
		
		' In order to know which one is the root folder, must check for (a) particular folder(s) 
		' that we know for sure that is(are) located in the root folder of the site
		Dim SearchForFolderName: SearchForFolderName = "includes\common" 

		Dim url: url = Request.ServerVariables("URL")
		' cut the trailing /
		LastSeparator = InStrRev(url, "/")
		If LastSeparator > 0 Then
			url = left(url, LastSeparator-1)
		End If

		Dim path: path = Server.MapPath(".") & "\"
		' cut the trailing \
		LastSeparator = InStrRev(path, "\")
		If LastSeparator > 0 Then
			path = left(path, LastSeparator-1)
		End If
		
		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		Dim prefix: prefix = ""
		Dim found: found = False
		Do while not Found
			If fso.FolderExists (path &  "\" & SearchForFolderName) Then
				Found = true
				Exit do
			Else
				' remove one folder lever both from path and url
				LastSeparator = InStrRev(url, "/")
				If LastSeparator > 0 Then
					url = left(url, LastSeparator-1)
				Else
					found = true ' force the exit from loop	
				End If
		
				LastSeparator = InStrRev(path, "\")
				If LastSeparator > 0 Then
					path = left(path, LastSeparator-1)
					prefix = prefix & "..\"
				Else
					found = true ' force the exit from loop	
				End If				 	
			End If						
		Loop
		Set fso = nothing

		If prefix = "" Then
			prefix = "."
		End If
		If found Then
			Session("KT_SitePath") = url
			Session("KT_AbsolutePathToRootFolder") = Server.MapPath(prefix) & "\"
		Else
			KT_GetAbsolutePathToRootFolder = ""
		End If	
	End Function

	' retrieves the path on disk to the site root (eg C:\www\sites\MYSITE\)
	Function KT_GetAbsolutePathToRootFolder()
		If Session("KT_AbsolutePathToRootFolder") = ""  Then
			KT_SetPathSessions
		End If
		KT_GetAbsolutePathToRootFolder	= Session("KT_AbsolutePathToRootFolder")
	End Function
	

	Sub KT_LoadASPFiles (arrPathsRelativeToRoot)
		absolutePathToRootFolder = KT_GetAbsolutePathToRootFolder()
		
		On Error Resume Next	
		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		Dim i
		For i=0 to ubound(arrPathsRelativeToRoot)		
			pathRelativeToRootFolder = arrPathsRelativeToRoot(i)
			absolutePathToFile = absolutePathToRootFolder & replace(pathRelativeToRootFolder, "/", "\")
			If fso.FileExists(absolutePathToFile) Then
				' read the file content
				Dim f: Set f = fso.OpenTextFile(absolutePathToFile, 1, False)
				content = f.ReadAll
				f.Close
				Set f = nothing
				
				If instr(pathRelativeToRootFolder, "KT_common.asp") <> 0 Then
					content = replace (content, "<S" & "CRIPT language=""jscript"" runat=""ser" & "ver"" src=""md5.js""></S" & "CRIPT>", "")
				End If
				If instr(pathRelativeToRootFolder, "tNG.inc.asp") <> 0 Then
					content = replace (content, "<S" & "CRIPT language=""jscript"" runat=""ser" & "ver"" src=""../common/md5.js""></S" & "CRIPT>", "")
				End If								
				' replace ASP tags 
				execcontent = replace (content, "<" & "%", "")
				execcontent = replace (execcontent, "%" & ">", "")
				ExecuteGlobal(execcontent)
			Else
				Session.Contents.RemoveAll
				Response.write replace(KT_uploadErrorMsg, "###", pathRelativeToRootFolder)
				Response.End()
			End If
			If err.number<>0 Then
				Response.write "<br><span style=""color:red"">Error loading file '" & pathRelativeToRootFolder & "'<br>" & err.description & "</font>"
				Response.End()
			End If
		Next
		On Error GoTo 0
	End Sub

End If

	' load KT_common
	If isEmpty(KT_Common__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/KT_common.asp")
	End If
	
	If isEmpty(KT_ResourcesFunctions__ALREADYLOADED) Then
		KT_LoadASPFiles Array("includes/common/lib/resources/KT_ResourcesFunctions.inc.asp")
	End If	
	
	If isEmpty(KT_MXI__ALREADYLOADED) Then
		KT_MXI__ALREADYLOADED = True
		KT_LoadASPFiles Array("includes/mxi/MXI_functions.inc.asp", "includes/mxi/MXI_Includes.class.asp")
	End If


	
' MXI ENGINE FUNCTIONS
'=====================

' INCLUDE ENGINE CORE FUNCTION must be defined only once
If isEmpty(KT_INCLUDEENGINE_CoreExists) Then
	'	GLOBAL DEFINES
	KT_INCLUDEENGINE_CoreExists	= True
	KT_INCLUDEENGINE_ThisFileName = "MXI.asp"
	KT_INCLUDEENGINE_UseUniqueIncludes = False
	
	KT_REL_PATH = ""	' relative from master to include (folder1/folder2/ or ../../)
	
	'	each call to a KT_include function uses its own entry for KT_INCLUDEENGINE_KEEPER	
	KT_INCLUDEENGINE_KeeperKey = 0
	Set KT_INCLUDEENGINE_KEEPER = Server.CreateObject ("Scripting.Dictionary")
	Set KT_INCLUDEENGINE_AlreadyIncluded = Server.CreateObject ("Scripting.Dictionary") ' used for Unique Includes
	

	'	ENTRY POINT FUNCTIONS
	Function KT_include(relative_path)
		this_physical_path = Server.MapPath("KT_MASTER.asp")
		KT_include = KT_include_absolute(this_physical_path, relative_path)
		KT_REL_PATH = ""
	End Function


	Function KT_include_absolute(caller_absolute_path, relative_path)
		Dim keeper_key
		keeper_key = KT_INCLUDEENGINE_KeeperKey
		KT_INCLUDEENGINE_KeeperKey = KT_INCLUDEENGINE_KeeperKey + 1
		
		Set KT_INCLUDEENGINE_KEEPER(keeper_key) = Server.CreateObject("Scripting.Dictionary")
		Set KT_INCLUDEENGINE_KEEPER(keeper_key)("HtmlCode") = Server.CreateObject("Scripting.Dictionary")
		KT_INCLUDEENGINE_KEEPER(keeper_key)("Buffer") = ""
		KT_INCLUDEENGINE_KEEPER(keeper_key)("VBRunnatCode") = ""
		KT_INCLUDEENGINE_KEEPER(keeper_key)("JSRunnatCode") = ""
		Set KT_INCLUDEENGINE_KEEPER(keeper_key)("JScriptFunctionsNames") =  Server.CreateObject("Scripting.Dictionary")
		
		Dim absolute_path
		Dim content
		RelPath_BeforeInclude = KT_REL_PATH
		
		content = KT_INCLUDEENGINE_GetFileContent(caller_absolute_path, relative_path, absolute_path)

		' build KT_REL_PATH
		relpath = replace(relative_path, "\", "/")
		If left(relpath, 1) <> "/" Then
			pos = InstrRev(relpath, "/")
			If pos <> 0 Then
				relpath = left(relpath, pos)
				KT_REL_PATH = KT_CanonizeRelPath(KT_REL_PATH & relpath)
			End If
		End If	

		If content <> "" Then
			content = KT_INCLUDEENGINE_RecursiveIncludes(content, absolute_path, keeper_key)
			KT_INCLUDEENGINE_PrepareCode content, keeper_key
			' replace calls to Response.End to a function that outputs the buffer before ending
			content = KT_INCLUDEENGINE_preg_replace("response.end\(?\)?", "KT_INCLUDEENGINE_ResponseEnd " & keeper_key & ", """ & KT_REL_PATH & """ ", content)
			' If there are JScript functions -> export them to global object, then replace their call from vb
			func_match_str = ""
			If KT_INCLUDEENGINE_KEEPER(keeper_key)("JScriptFunctionsNames").Count <> 0 Then
				For each func_name in KT_INCLUDEENGINE_KEEPER(keeper_key)("JScriptFunctionsNames")
					If func_match_str <> "" Then
						func_match_str = func_match_str & "|"
					End If
					func_match_str = func_match_str & func_name
				Next
				func_match_str = "(" & func_match_str & ")"
			
				' find the called js functions and export them
				Dim export_matches
				func_export_str = ""
				KT_INCLUDEENGINE_preg_match func_match_str, content, export_matches
				func_match_str = ""
				For each func_match in export_matches
					func_name = func_match.value
					func_export_str = func_export_str & "KT_INCLUDEENGINE_ExportedFunctions." & func_name & " = " & func_name & ";" & vbNewLine
					If func_match_str <> "" Then
						func_match_str = func_match_str & "|"
					End If
					func_match_str = func_match_str & func_name
				Next
				
				If func_match_str <> "" Then
					func_match_str = "(" & func_match_str & ")"
					content = KT_INCLUDEENGINE_preg_replace(func_match_str, "KT_INCLUDEENGINE_ExportedFunctions.$1", content)
				End If			

				' Export JScript functions
				ExecuteGlobalJScript KT_INCLUDEENGINE_KEEPER(keeper_key)("JSRunnatCode"), func_export_str
			End If
			
			' Export inline VBscript functions
			 If KT_INCLUDEENGINE_KEEPER(keeper_key)("VBRunnatCode") <> "" Then
			 	ExecuteGlobal KT_INCLUDEENGINE_KEEPER(keeper_key)("VBRunnatCode")	
			 End If

			 ' Finally execute the page
			ExecuteGlobal content
		End If
		
		' KT_include_absolute = KT_INCLUDEENGINE_KEEPER(key)("Buffer")
		KT_include_absolute = mxi_ParseHtml(KT_INCLUDEENGINE_KEEPER(keeper_key)("Buffer"), KT_REL_PATH)
		KT_REL_PATH = RelPath_BeforeInclude
	End Function



	Function KT_INCLUDEENGINE_ResponseEnd (key, relpath)
		Response.write mxi_ParseHtml(KT_INCLUDEENGINE_KEEPER(key)("Buffer"), relpath)
		Response.End()
	End Function


	Function KT_INCLUDEENGINE_GetFileContent(caller_absolute_path, relative_path_to_caller, ByRef absolute_path)
		KT_INCLUDEENGINE_GetFileContent = ""
		
		Dim relative_path
		relative_path = relative_path_to_caller
		relative_path = replace(relative_path, "\", "/")
		pos = InstrRev(relative_path, "/")
		If pos <> 0 Then
			file_name = Mid(relative_path, InstrRev(relative_path, "/") + 1)
		Else
			file_name = relative_path
		End If

		' extra_check for file_name
		If lcase(file_name) = lcase(KT_INCLUDEENGINE_ThisFileName) Then
			Exit Function
		End If

		' compute absolute path
		caller_absolute_path = replace(caller_absolute_path, "\", "/")
		If left(relative_path, 1) = "/" Then
			absolute_path = Server.MapPath(relative_path)
		Else
			absolute_path = Left(caller_absolute_path, InstrRev(caller_absolute_path, "/")-1)
			
			While Instr(relative_path, "../") = 1 
				'remove ../ from the relative_path
				relative_path = Mid(relative_path, 4)
				pos = InstrRev(absolute_path, "/")
				If pos <> 0 Then
					'remove another directory from absolute_path
					absolute_path = Left(absolute_path, pos-1)
				End If
			Wend
			absolute_path = trim(replace(absolute_path & "/" & relative_path, "/", "\"))
		End If

		If KT_INCLUDEENGINE_UseUniqueIncludes Then
			If KT_INCLUDEENGINE_AlreadyIncluded.Exists(lcase(absolute_path)) Then
				Exit Function
			Else
				KT_INCLUDEENGINE_AlreadyIncluded(lcase(absolute_path)) = ""	
			End If
		End If

		Dim fso, file
		Set fso = Server.CreateObject("Scripting.FileSystemObject")
		If fso.FileExists(absolute_path) Then
			Set file = fso.GetFile(absolute_path)
			If file.Size > 0 Then
				Set file = fso.OpenTextFile(absolute_path, 1, false)
				KT_INCLUDEENGINE_GetFileContent = file.ReadAll
				KT_INCLUDEENGINE_GetFileContent = replace (KT_INCLUDEENGINE_GetFileContent, vbNewLine, vbCr) ' added for MAC
				KT_INCLUDEENGINE_GetFileContent = replace (KT_INCLUDEENGINE_GetFileContent, vbCr, vbNewLine)
				file.Close
			End If	
			Set file = nothing
		Else
			Response.write "<span style=""color:red""><br><br>Error: Required file '" & absolute_path & "' was not found.</span>"
			Response.flush()
		End If
		Set fso = nothing
	End Function


	
	Function KT_INCLUDEENGINE_RecursiveIncludes (ByRef content, ByRef absolute_path, keeper_key)
		' must extract all <script runat="server"> scripets
		' for VBScript code -> placed it in the current buffer for VBScode
		' for JScript code  -> placed it in the current buffer for JScode
		'					-> extract all the function names and place them in the dictionary

		Dim returned_content
			returned_content = content
		
		Dim script_matches
		Dim attrs_matches
		Dim func_matches
		Dim source_code
		Dim offset
		
		offset = 0
		KT_INCLUDEENGINE_preg_match "<script([^>]*)>", returned_content, script_matches
		For Each Match in script_matches
			script_start =  Match.FirstIndex
			script_attrs_length = Match.Length
			attrs_vals = Match.value
			
			' check runat=server attr/val
			If KT_INCLUDEENGINE_preg_test("runat\s*=\s*""*server""*\s*", attrs_vals) Then 
				isVBCode = False
				isJSCode = False
				src = ""
				KT_INCLUDEENGINE_preg_match "([^<\s=]*)\s*=\s*([^\s>]*)", attrs_vals, attrs_matches
				For each pair in attrs_matches
					av = split(pair.Value, "=")
					attr_name = lcase(trim(av(0)))
					attr_val = lcase(trim(replace(av(1),"""","")))
					
					If attr_name = "language" Then
						If attr_val = "vbscript" Then
							isVBCode = True
							isJSCode = False
						End If
						If attr_val = "jscript" Then
							isJSCode = True
							isVBCode = False
						End If
						' else -> unsuported server language	
					End If
					
					If attr_name = "src" Then
						src = attr_val
					End If
				Next
				
				If isVBCode OR isJSCode Then
					source_code = ""
					script_end = instr(script_start + offset + 1, returned_content, "</script>", 1) 
					script_length = script_end + len("</script>") - (script_start + offset + 1) 
					
					If src <> "" Then
						' the code content is taken from an outside file
						source_code = KT_INCLUDEENGINE_GetFileContent(absolute_path, src, resulted_path)
					Else
						' the code content is inline. 
						' must extract it
						If script_end <> 0 Then
							'source_code = mid(returned_content, script_start + offset + 1, script_length)
							source_code = mid(returned_content, script_start + offset + 1 + script_attrs_length, script_length - script_attrs_length - len("</script>"))
						End If
					End If
					returned_content = left(returned_content, script_start + offset) & mid (returned_content, script_end + len("</script>"))
					offset = offset - script_length

					If isVBCode Then	
						KT_INCLUDEENGINE_KEEPER(keeper_key)("VBRunnatCode")	= KT_INCLUDEENGINE_KEEPER(keeper_key)("VBRunnatCode") & source_code & vbNewLine 	
					End If
					If isJSCode Then
						KT_INCLUDEENGINE_KEEPER(keeper_key)("JSRunnatCode")	= KT_INCLUDEENGINE_KEEPER(keeper_key)("JSRunnatCode") & source_code & vbNewLine						
						' must extract the name of the functions from source_code and put them in the dictionary
						KT_INCLUDEENGINE_preg_match "function\s([^\(]*)\(", source_code, func_matches
						For Each func in func_matches
							func_name = func.value
							func_name = trim(replace(replace(func_name, "function", ""),"(",""))
							KT_INCLUDEENGINE_KEEPER(keeper_key)("JScriptFunctionsNames")(func_name) = ""
						Next
					End If
				End If
			End If
		Next	
				
	
		Dim included_content
		Dim included_absolute_path		
		
		Dim include_matches
		Dim path_matches
		offset = 0
		KT_INCLUDEENGINE_preg_match "<!--\s*#include\s(file|virtual)\s*=\s*""([^""]*)""\s*-->", returned_content, include_matches
		For Each Match in include_matches
			included_string = Match.Value
			included_start = Match.FirstIndex
			included_length = Match.Length
			
			isVirtual = false
			If instr(1, included_string, "virtual", 1) <> 0 Then
				isVirtual = true
			End If
			
			' extract the path
			KT_INCLUDEENGINE_preg_match """([^""]*)""", included_string, path_matches
			Set pathmatch = path_matches(0)
			included_relative_path = trim(replace(pathmatch.value,"""", ""))
			If isVirtual Then
				If left(included_relative_path, 1) <> "/" And  left(included_relative_path, 1) <> "\" Then
					included_relative_path  = "/" & included_relative_path	
				End If
			End If	

			included_content = KT_INCLUDEENGINE_GetFileContent(absolute_path, included_relative_path, included_absolute_path)
			included_content = KT_INCLUDEENGINE_RecursiveIncludes(included_content, included_absolute_path, keeper_key)

			returned_content = left (returned_content, included_start + offset) & included_content & mid (returned_content, included_start + offset + included_length +1)
			offset = offset + len(included_content) - len(included_string)
		Next
		returned_content = KT_INCLUDEENGINE_preg_replace("KT_include\s*\(", "KT_include_absolute(""" & absolute_path &  """,", returned_content)
		KT_INCLUDEENGINE_RecursiveIncludes = returned_content
	End Function

		

	Sub KT_INCLUDEENGINE_PrepareCode (ByRef content, keeper_key)
		delimiter_str = "##KT##KT##"
		delimiter_len = len(delimiter_str)
		starter_str = "$"
		starter_len = len (starter_str)
		
		Dim tmp
		' replace the code delimiters
		tmp = replace (content, "<" & "%", delimiter_str & starter_str)
		tmp = replace (tmp, "%" &  ">", delimiter_str)
		
		' check if there is any ASP code 
		If instr (tmp, delimiter_str) <> 0 Then
			If left(tmp, delimiter_len) = delimiter_str Then
				tmp = right (tmp, len(tmp)  - delimiter_len)
			End If
			If right(tmp, delimiter_len) = delimiter_str Then
				tmp = left (tmp, len(tmp)  - delimiter_len)
			End If
						
			tmp_array = split(tmp, delimiter_str)
			For i = lbound(tmp_array) to ubound (tmp_array)
				If KT_INCLUDEENGINE_preg_test("^\$\s*@\s*(language)\s*=*", tmp_array(i))  Then
					tmp_array (i) = ""
				End If
				If len(test) > 0 Then	
					' check to see if this line contains the directive
					If instr(1, test, "@language", 1) <> 0 Then
						tmp_array (i) = "" ' remove the page directive
					End If
				End If
				
				tmp  =  tmp_array(i)
				If left(tmp,starter_len) = starter_str Then
					' This is ASP code
					tmp = right (tmp, len(tmp) - starter_len)
					tmp = replace (tmp, "Response.write", "KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") =  KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") & ",1, -1, 1)
					
					' check if this is quit output ( < %="str"% > )
					If left(ltrim(tmp), 1) = "=" Then
						tmp = right (tmp, len(tmp) - instr(tmp, "=")) 
						tmp = "KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") =  KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") & " & tmp
					End If
				Else
					' This is just HTML code
					 If tmp <> "" Then
						KT_INCLUDEENGINE_KEEPER(keeper_key)("HtmlCode")(i) = tmp
						tmp = "KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") =  KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") & " & "KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""HtmlCode"")(" & i & ")"
					 End If
				End If
				tmp_array(i) = tmp
			Next
			content = join(tmp_array, vbNewLine)
		Else
			' all page is html
			KT_INCLUDEENGINE_KEEPER(keeper_key)("HtmlCode")("Html") = tmp
			content = "KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") =  KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""Buffer"") & " & "KT_INCLUDEENGINE_KEEPER(" & keeper_key & ")(""HtmlCode"")(""Html"")"
		End If
	End Sub

	
	Function KT_INCLUDEENGINE_preg_match(byval m_Pattern, byval m_Subject, byref m_Matches)
		KT_preg_match m_Pattern, m_Subject, m_Matches
		'Dim regEx
		'Set regEx = New RegExp

		'regEx.Global = True
		'regEx.MultiLine = True
		'regEx.IgnoreCase = True
		'regEx.Pattern = m_Pattern
		
		'Set m_Matches = regEx.Execute(m_Subject)
		'Set regEx = Nothing
	End Function
	
	
	Function KT_INCLUDEENGINE_preg_replace(byval m_Pattern, byval m_Replacement, byval m_Subject)
		KT_INCLUDEENGINE_preg_replace = KT_preg_replace (m_Pattern, m_Replacement, m_Subject)
		
		'Dim regEx						
		'Set regEx = New RegExp   
		'regEx.Global = True
		'regEx.IgnoreCase = True      
		'regEx.MultiLine = True
		'regEx.Pattern = m_Pattern           
   
		'KT_INCLUDEENGINE_preg_replace = regEx.Replace(m_Subject, m_Replacement)   
		'Set regEx = Nothing
	End Function	
	
	
	Function KT_INCLUDEENGINE_preg_test(byval m_Pattern, byval m_Subject)
		KT_INCLUDEENGINE_preg_test = KT_preg_test (m_Pattern, m_Subject)
		'Dim regEx
		'Set regEx = New RegExp
		
		'regEx.Global = True
		'regEx.MultiLine = True
		'regEx.IgnoreCase = True
		'regEx.Pattern = m_Pattern
		
		'KT_INCLUDEENGINE_preg_test = regEx.Test(m_Subject)
		'Set regEx = Nothing
	End Function
	
End If
%>

<script language="JScript" runat="server">
//var KT_INCLUDEENGINE_ExportedFunctions;
KT_INCLUDEENGINE_ExportedFunctions = new Object;
function ExecuteGlobalJScript(functions_source, functions_export) {
	eval(functions_source);
	eval(functions_export);
}
</script>
