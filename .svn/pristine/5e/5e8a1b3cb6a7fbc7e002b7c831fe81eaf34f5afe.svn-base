<%
	Function MXL_Recordset(ByRef RS, ByRef maxRows, ByRef maxCols)		
		Set fakeRS = new MXL_RS

		fakeRS.ActiveConnection = RS.ActiveConnection
		fakeRS.Source = RS.Source
		fakeRS.CursorLocation = RS.CursorLocation
		fakeRS.CursorType = RS.CursorType
		fakeRS.LockType = RS.LockType
		
		' add fields
		For each field in RS.Fields
			fakeRS.Fields.Append field.Name
		Next
		fakeRS.Open 
		
		' add values
		maxRecords = maxRows * maxCols
		While ((maxRecords <> 0) AND (NOT RS.EOF))
			fakeRS.AddNew
			For each field in RS.Fields
				fakeRS.Fields.Item(field.Name).value  = RS.Fields.Item(field.Name).value
			Next
			
			RS.MoveNext
			maxRecords = maxRecords - 1
		Wend 

		If not fakeRS.BOF Then
			fakeRS.MoveFirst
		End if
		RS.Close()
		
		maxCols = Fix(fakeRS.RecordCount / maxRows)
		If (fakeRS.RecordCount mod maxRows) <> 0 Then
			maxCols = maxCols  + 1
		End If
		Set MXL_Recordset = fakeRS
	End Function	
			


	Class MXL_RS
		Public Fields
		
		Public CursorLocation
		Public CursorType
		Public LockType
		
		Public ActiveConnection
		Public Source
		
		' // constructor 
		private Sub Class_Initialize()
			' // initialisation of all properties goes here
			Set Fields = new MXL_RSItemsClass
		End Sub

		Public Function Close()
			Set Fields = Nothing
		End Function
		
		Public Function MoveFirst
			Fields.m_position = 1
			If Fields.m_position <= Fields.m_noRows Then
				Fields.EOF = False
				Fields.BOF = False
			End If
		End Function
		
		Public Function Requery
			MoveFirst()
		End Function
		
		Public Function MoveNext
			If Fields.m_position = Fields.m_noRows Then
				Fields.EOF = True
				'Fields.BOF = True
			End If	
			Fields.m_position = Fields.m_position + 1 
		End Function
		
		Public Function Move(newPosition, fromWhere)
			If fromWhere <> 0 And fromWhere <> 1 Then
				' move not supported
				Move = False
				Exit Function
			End If
			
			If fromWhere = 0 Then ' Starts at the current record
				If (Fields.m_position + newPosition) <= Fields.m_noRows And (Fields.m_position + newPosition) >= 1 Then
					Fields.m_position = Fields.m_position + newPosition
					Move = True
				Else
					Move = False
				End If
				Exit Function
			End If
			
			If fromWhere = 1 Then '  Starts at the first record
				If (newPosition + 1) <= Fields.m_noRows And newPosition >=0  Then
					Fields.m_position = newPosition + 1
					Move = True
				Else
					Move = False
				End If
				Exit Function
			End If
			
		End Function
		
		Public Function RecordCount
			RecordCount = Fields.m_noRows
		End Function
		
		Public Function AddNew	
			Fields.AddNew
		End Function
		
		Public Function BOF
			BOF = Fields.BOF
		End Function
		
		Public Function EOF
			EOF = Fields.EOF
		End Function
		
		Public Function Open
			
		End Function
		
		Public Default Property Get Value (p_fieldName)
			Set Value = Fields (p_fieldName)
		End Property
	End Class



	Class MXL_RSItemClass
		Private fieldValue
		private Sub Class_Initialize()
			fieldValue = ""
		End Sub
	
		public Default Property Get Value()
			Value = fieldValue
		End Property
		
		public Property Let Value(p_fieldValue)	
			If isnull(p_fieldValue) Then
				p_fieldValue = ""
			End If
			fieldValue = p_fieldValue
		End Property
	End Class



	Class MXL_RSItemsClass
		Public m_FieldValues	' dictionary with indexed by rows / fieldNames
		Public m_Fields		' current used fields

		Public m_noRows	
		Public m_noFields
		Public m_position
		Public EOF
		Public BOF			
		
		
		Private Sub Class_Initialize()
			Set m_Fields = Server.CreateObject("Scripting.Dictionary")
			Set m_FieldValues = Server.CreateObject("Scripting.Dictionary")
			m_noRows = 0
			m_noFields = 0
			m_position = 0
			EOF = True
			BOF = True
		End Sub
		
		Public default property Get Value(p_fieldName)
			If m_FieldValues.Exists(m_position) Then
				If m_FieldValues(m_position).Exists (lcase(p_fieldName)) Then
					Set Value = m_FieldValues(m_position)(lcase(p_fieldName))
				Else
					Response.write "MXL_Recordset Error: The field '" & p_fieldName & "' is not in the Fields Collection"
					Response.End()
				End If
			Else
				Response.write "MXL_Recordset: No records for the current cursor position"
				Response.end
			End If
		End property

	
		Public Function Item(p_fieldName)
			If m_FieldValues.Exists(m_position) Then
				If m_FieldValues(m_position).Exists (lcase(p_fieldName)) Then
					Set Item = m_FieldValues(m_position)(lcase(p_fieldName))
				Else
					Response.write "MXL_Recordset Error: The field '" & p_fieldName & "' is not in the Fields Collection"
					Response.End()
				End If
			Else
				Response.write "MXL_Recordset Error: No records for the current cursor position"
				Response.end
			End If
		End Function
		
		
		
		Public Function Append (p_fieldName)
			If m_Fields.Exists (lcase(p_fieldName)) Then
				Response.write "MXL_Recordset Error: You cannot append the same field twice"
				Response.End()
			Else
				m_Fields.Add lcase(p_fieldName), ""
				m_noFields = m_noFields + 1
			End If
		End Function		
		
		Public Function AddNew
			If m_noFields = 0 Then
				Response.write "MXL_Recordset Error: There are no fields"
				Response.end
			Else
				m_position = m_position + 1
				Set m_FieldValues(m_position) = Server.CreateObject("Scripting.Dictionary")
				' add the column values
				For each fieldname in m_Fields
					Set m_FieldValues(m_position)(fieldName) = new MXL_RSItemClass
				Next
				m_noRows = m_noRows + 1
				
				EOF = false
				BOF = false
			End If
		End Function
	End Class
 %>