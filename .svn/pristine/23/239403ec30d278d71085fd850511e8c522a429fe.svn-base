<SCRIPT language="jscript" runat="server" src="../../common/md5.js"></SCRIPT><!-- #include file="../../common/lib/Resources/KT_Resources.asp"--><%
	d = "tNG"

	uniqueId = Request.QueryString("id")
	pass = false
	If isObject(Session("tNG_download")) Then
		If Session("tNG_download").Exists(uniqueId) Then
			pass = true
		End If
	End If
	If Not pass Then
		Response.write KT_getResource("ERR_DOWNLOAD_FILE", d, null)
		Response.End()
	End If	
	
	realPath = Session("tNG_download")(uniqueId)("realPath")
	fileName = Session("tNG_download")(uniqueId)("fileName")

	If hex_md5(realPath) <> uniqueId Then
		Response.Write KT_getResource("ERR_DOWNLOAD_FILE_WRONG_HASH", d, null)
		Response.End()
	End If
	
	' check if file exists
	Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
	If Not fso.FileExists(realPath) Then
		Response.write KT_getResource("ERR_DOWNLOAD_FILE_NO_READ", d, array(realPath))
		Response.End()
	End If
	Set fso = nothing

	
	' try to download file
	On Error Resume Next
	Response.ContentType = "application/octet-stream"
	Response.AddHeader "Cache-control", "private"
	Response.AddHeader "Content-disposition", "attachment; filename=""" & fileName & """;"
'	Response.AddHeader "Pragma", "No-Cache"
'	Response.CacheControl = "no-cache"
'	Response.Expires = -1

	Dim objStream
	Set objStream = Server.CreateObject("ADODB.Stream")
	objStream.Open
	objStream.Type = 1
	objStream.LoadFromFile realPath
	Response.BinaryWrite objStream.Read

	objStream.Close
	Set objStream = Nothing
	If err.Number <> 0 Then
		Response.AddHeader "Status-code", "404"
		Response.Write KT_getResource("ERR_DOWNLOAD_FILE_NO_READ", d, array(realPath))
		Response.End()
	End If
	On Error GoTo 0
%>