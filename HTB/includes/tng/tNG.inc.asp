<%
'
'	Copyright (c) InterAKT Online 2000-2005
'
If Session("KT_SitePath") <> "" Then
	If Instr(1, Request.ServerVariables("URL"), Session("KT_SitePath"), 1) = 0 Then
		Session.Contents.RemoveAll
	End If
End If

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

	' tNG.inc can overwrite this function if it already exists
	KT_MD5Function__ALREADYLOADED = True
	' map js function on a vb function
	ExecuteGlobal 	"Function KT_md5(str_to_encode) "  & vbNewLine & _
					"	KT_md5 = hex_md5(str_to_encode)"  & vbNewLine & _
					"End Function"

	If isEmpty(KT_tNGCore__ALREADYLOADED) Then
		KT_tNGCore__ALREADYLOADED = True
		KT_tNG_uploadFileList = Array( _
								"includes/common/KT_common.asp", _
								"includes/tng/tNG_functions.inc.asp", _
								"includes/tng/tNG_log.class.asp", _
								"includes/tng/tNG_dispatcher.class.asp", _
								"includes/tng/tNG.class.asp", _
								"includes/tng/tNG_fields.class.asp", _
								"includes/tng/tNG_insert.class.asp", _
								"includes/tng/tNG_update.class.asp", _
								"includes/tng/tNG_delete.class.asp", _
								"includes/tng/tNG_multiple.class.asp", _
								"includes/tng/tNG_custom.class.asp", _
								"includes/tng/tNG_error.class.asp", _
								"includes/tng/triggers/tNG_defTrigg.inc.asp", _
								"includes/tng/triggers/tNG_Redirect.class.asp", _
								"includes/tng/triggers/tNG_LinkedTrans.class.asp", _
								"includes/tng/triggers/tNG_CheckTableField.class.asp", _
								"includes/tng/triggers/tNG_CheckUnique.class.asp", _
								"includes/tng/triggers/tNG_ThrowError.class.asp", _
								"includes/common/lib/resources/KT_Resources.asp", _
								"includes/common/lib/email/KT_Email.asp", _
								"includes/common/lib/db/KT_FakeRecordset.asp", _
"includes/tng/tNG_config.inc.asp")
		KT_LoadASPFiles KT_tNG_uploadFileList
	End If
%>
<SCRIPT language="jscript" runat="server" src="../common/md5.js"></SCRIPT>
