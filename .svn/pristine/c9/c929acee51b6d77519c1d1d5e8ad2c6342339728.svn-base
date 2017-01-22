<%
Sub IncludeDynamicFile(scriptfile)
	Dim f, file
	Set f = CreateObject("Scripting.FileSystemObject")
	on error resume next
	'on error resume next
	Set file = f.GetFile(scriptfile)
	Dim stream 
	Set stream = file.OpenAsTextStream(1)
	Dim content 
	content = stream.Read(file.Size)
	ExecuteGlobal(Replace(Replace(content, "<" & "%", ""), "%" & ">", ""))
	if err.number>0 then
		Response.Write "Include file: """ & scriptfile & """"
		Response.Write "<br>" & err.Description & "<br>" & err.Source & "<br>"
	end if
	set f = nothing
End Sub
%>