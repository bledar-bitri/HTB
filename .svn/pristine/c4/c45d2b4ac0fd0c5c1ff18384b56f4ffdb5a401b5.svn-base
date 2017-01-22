<%@ LANGUAGE="VBScript" %>
<%Option Explicit%>

<%

' *** Restrict Access To Page: Grant or deny access to this page
If Session("MM_Username") = "" Then
  Response.Redirect("../../intranet/login/expired.asp")
End If

'***********************************
'Functions
'***********************************

' force no-cache
Response.Expires = 15
Response.ExpiresAbsolute = Now() - 2
Response.AddHeader "pragma","no-cache"
Response.AddHeader "cache-control","private"
Response.CacheControl = "private"


dim connString
connString = "Driver={IBM DB2 ODBC DRIVER};Hostname=localhost;Port=50000;Protocol=TCPIP;Database=HTBRTE;UID=b.bitri;PWD=13tetor77"

'Function returns a valid Globally Unique ID (GUID) for identifying a session.
Function GetGuid()
	Dim TypeLib, guid_temp
	
	Set TypeLib = Server.CreateObject("Scriptlet.TypeLib")
	
	'A true GUID contains a unicode null termination, that needs to be stripped to behave like 
	'  a string.
	guid_temp = TypeLib.Guid
	GetGuid = Left(guid_temp, Len(guid_temp) - 2)
		
	Set TypeLib = Nothing
End Function

'This function adds all Session information to the database and returns the GUID used to 
'  identify the Session information.
Function AddSessionToDatabase()
	'Declare Variables
	Dim con, cmd, strSql, guidTemp, i

	'Initialize Variables
	Set con = Server.CreateObject("ADODB.Connection")

'******************
	con.Open connString
'******************

	Set cmd = Server.CreateObject("ADODB.Command")
	cmd.ActiveConnection = con
	i = 1

	'Iterate through all Session variables and add them to the database with the 
	'  same GUID as an identifier.
	guidTemp = GetGuid()
	
	Do While (i <= Session.Contents.Count)
    	strSql = "INSERT INTO tblASPSessionState (GUID, SessionKey, SessionValue) " + _
			"VALUES ('" & guidTemp & "', '" & Session.Contents.Key(i) & "', '" & Session.Contents.Item(i) & "')"
        If (Not UCase(Session.Contents.Key(i)) = "SQLSET") Then
            Response.Write strSql + "<BR>"
		    cmd.CommandText = strSql
		    cmd.Execute
        End If
        i = i + 1
	Loop

	'Return the GUID used to identify the Session information
	AddSessionToDatabase = guidTemp

	'Clean up database objects
	con.Close
	Set cmd = Nothing
	Set con = Nothing
End Function

'This function retrieves the Session information identified by the parameter guidIn. The 
'  resulting Session information is loaded into the Session object, it is not returned.
Sub GetSessionFromDatabase(guidIn)
	'Declare Variables
	Dim con, cmd, rs, strSql, guidTemp, i

	'Initialize Variables
	Set con = Server.CreateObject("ADODB.Connection")

'******************
	con.Open connString
'******************

	Set cmd = Server.CreateObject("ADODB.Command")
	cmd.ActiveConnection = con
	i = 1

	strSql = "SELECT * FROM tblASPSessionState WHERE GUID = '" & guidIn & "'"
	Set rs = Server.CreateObject("ADODB.Recordset")
	rs.Open strSql, con

	While Not(rs.EOF)
		Session(rs("SessionKey")) = rs("SessionValue")
		rs.MoveNext
	Wend

	i = 1
	Do While (i <= Session.Contents.Count)
		Response.Write "Session(""" & Session.Contents.Key(i) & """) - " & Session.Contents.Item(i) & "<BR>"
		i = i + 1
	Loop
	
	'Clean up database objects
	rs.Close
	con.Close

	Set rs = Nothing
	Set cmd = Nothing
	Set con = Nothing
End Sub

'This performs cleanup of the Session information identified by the parameter guidIn. All Session
'  information in the database with the specified GUID is deleted.
Sub ClearSessionFromDatabase(guidIn)
	'Declare Variables
	Dim con, cmd, strSql

	'Initialize Variables
	Set con = Server.CreateObject("ADODB.Connection")

'******************
	con.Open connString
'******************

	Set cmd = Server.CreateObject("ADODB.Command")
	cmd.ActiveConnection = con
	
	'Remove all session variables from the database
	strSql = "DELETE FROM tblASPSessionState WHERE GUID = '" & guidIn & "'"
	cmd.CommandText = strSql
	cmd.Execute
    
    Response.Write strSql + "<BR>"

	'Clean up database objects
	con.Close
	Set cmd = Nothing
	Set con = Nothing
End Sub

'***********************************
'Main code execution
'***********************************

Dim guidSave

If Request.QueryString("dir") = "2aspx" Then
	'Store the session information in the database, and switch to ASP.NET
	guidSave = AddSessionToDatabase()
	Response.Redirect("SessionTransfer.aspx?dir=2aspx&guid=" & guidSave & "&url=" & Server.URLEncode(Request.QueryString("url")))
Else
	'Retrieve the session information and redirect to the specified URL
	Call GetSessionFromDatabase(Request.QueryString("guid"))

	'Clean up the database
	Call ClearSessionFromDatabase(Request.QueryString("guid"))
	Response.Redirect(Request.QueryString("url"))
End If
%>