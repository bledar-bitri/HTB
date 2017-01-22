<%
	' create interakt object if not already created
	If not isObject(interakt) Then
		ExecuteGlobal ("Dim interakt" & vbNewLine & "Set interakt = Server.CreateObject(""Scripting.Dictionary"")")
	End If	
	
	If not isObject(interakt("resources")) Then
		 Set interakt("resources") = Server.CreateObject("Scripting.Dictionary")
	End If


	Function KT_getResource(resourceName, dictionary, args)
		If isnull(resourceName) Then
			resourceName = "default"
		End If
		If isnull(dictionary) Then
			dictionary = "default"
		End If
		If isnull(args) Or isempty(args) Or (Not isarray(args)) Then
			args = array()
		End If
		
		Dim resourceValue: resourceValue = resourceName
		Dim dictionaryFileName: 
			dictionaryFileName = KT_GetAbsolutePathToRootFolder() & "includes\resources\" & dictionary & ".res.asp"



		' First thing: check the dictionary for the corresponding resourceName
		If Not isObject(interakt("resources")(dictionary)) Then
			' must load the dictionary
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			If fso.FileExists(dictionaryFileName) Then
				' read the file content
				Dim f: Set f = fso.OpenTextFile(absolutePathToResourceDictionaries & dictionaryFileName, 1, False)
				content = f.ReadAll
				f.Close
				Set f = nothing
					
				execcontent = replace (content, "<" & "%", "")
				execcontent = replace (execcontent, "%" & ">", "")
				Execute execcontent
				
				If isObject(res) Then
					Set interakt("resources")(dictionary) = res
				End If	
			End If
			Set fso = nothing
		End If		


		foundResource = false
		If isObject(interakt("resources")(dictionary)) Then
			If interakt("resources")(dictionary).Exists(resourceName) Then
				foundResource = true
				resourceValue = interakt("resources")(dictionary)(resourceName)
			End If
		End IF	
		
		If Not foundResource Then
			'If trim(resourceName) <> "" And trim(resourceName) <> "%s" Then
			'	Response.write "<br />Resource '" & resourceName & "' not defined in dictionary '" & dictionary & "'.<br />"
			'	Response.End()
			'End If

			If right(resourceValue, 2)= "_D" Then
				resourceValue = left(resourceValue, len(resourceValue)-2)
			End If
		End If
				
		If ubound(args) <> -1 Then
			resourceValue = KT_sprintf(resourceValue, args)
		End If
		
		KT_getResource = resourceValue
	End Function
%>