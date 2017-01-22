<%
'
'	Copyright (c) InterAKT Online 2000-2005
'
Class WDG_JsRecordset
	Private outputString
	

	Private Sub Class_Initialize()
		outputString = ""
	End Sub
	
	
	Public Sub Init (RecordsetName)
		Execute("Dim rs: Set rs = " & RecordsetName )
		outputString = "<script>" & vbNewLine & _
			"top.jsRawData_" & RecordsetName & " = [ "  & vbNewLine
		
		Dim fieldNames: Set fieldNames = Server.CreateObject("Scripting.Dictionary")
		first = true
		For each f in rs.Fields
			meta = f.name
			fieldNames(meta) = "-"
			If first Then
				first = false
				outputString = outputString & "["
			Else
				outputString = outputString & ", "	
			End If
			outputString = outputString & """" & meta & """"
		Next
		outputString = outputString	& "], " &  vbNewLine
	
		Dim arr: Set arr = Server.CreateObject("Scripting.Dictionary")
		While not rs.EOF
			arr.RemoveAll
			For each field in fieldNames
				arr(field) = rs.Fields.Item(field).Value
			Next
			outputString = outputString & WDG_aspdict2jsarray(arr) & ", "
			rs.Movenext
		Wend 
		outputString = outputString & vbNewLine & _
			"[]" & vbNewLine & _
			"]" & vbNewLine & _
			"top." & RecordsetName & " = new top.JSRecordset('" & RecordsetName & "')"  & vbNewLine & _
			"</script>"  & vbNewLine
		If (rs.CursorType > 0) Then
			rs.MoveFirst
		Else
			rs.Requery
		End If
	End Sub

	Public Function getOutput
		getOutput = outputString
	End Function
End Class
%>