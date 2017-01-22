<%
Class tNG_CheckMasterRecord
	Public fkField
	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_CheckTableField
		parent.SetContextObject Me
	End Sub

	Private Sub Class_Terminate()

	End Sub

	Public Function setFkFieldName (field__param)
		fkField = field__param
		this.setFieldType tNG.getColumnType(field__param)
		this.setFieldValue tNG.getColumnValue(field__param)
	End Function


	Public Function Execute()
		this.errorIfExists false
		Set myErr = parent.Execute()
		If Not myErr is Nothing Then
			useSavedData  = false
			If KT_in_array(tNG.transactionType, Array("_delete", "_multipleDelete"), false) Then
				useSavedData = true
			End If
			
			errorMsg = tNG_DynamicData(errorMsg, tNG, null, useSavedData, null, null)
			Set cols = tNG.columns
			Set myErr = nothing
			
			If fkField <> "" Then
				If cols.Exists(fkField) Then
					If cols(fkField)("method") = "POST" Then
						Set myErr = new tNG_error
						myErr.Init "TRIGGER_MESSAGE__CHECK_MASTER_RECORD", array(), array()
						myErr.setFieldError fkField, "%s", array(errorMsg)
					End If
				End If
			End If
					
			If Not KT_isSet(myErr) Then	
				Set myErr = new tNG_error
				myErr.Init "TRIGGER_MESSAGE__CHECK_MASTER_RECORD", array(), array()
				myErr.addDetails "%s", array(errorMsg), array("")
			End If
			
			Set Execute = myErr
			Exit Function			
		End If
		Set Execute = myErr
	End Function
			

	'===========================================
	' Inheritance
	'===========================================
	Public this
	Public parent

	Public Sub SetContextObject(ByRef objContext)
		Set this = objContext
		parent.SetContextObject objContext
	End Sub

	'  Inherited properties from tNG_CheckTableField
	'-------------------------------------------
	' tNG
	Public Property Get tNG
		Set tNG = parent.tNG
	End Property
	Public Property Set tNG(ByRef tNG__par)
		Set parent.tNG = tNG__par
	End Property

	' table
	Public Property Get table
		table = parent.table
	End Property
	Public Property Let table(table__par)
		parent.table = table__par
	End Property

	' field
	Public Property Get field
		field = parent.field
	End Property
	Public Property Let field(field__par)
		parent.field = field__par
	End Property

	' fieldtype
	Public Property Get fieldtype
		fieldtype = parent.fieldtype
	End Property
	Public Property Let fieldtype(fieldtype__par)
		parent.fieldtype = fieldtype__par
	End Property

	' value
	Public Property Get value
		value = parent.value
	End Property
	Public Property Let value(value__par)
		parent.value = value__par
	End Property

	' errorMsg
	Public Property Get errorMsg
		errorMsg = parent.errorMsg
	End Property
	Public Property Let errorMsg(errorMsg__par)
		parent.errorMsg = errorMsg__par
	End Property

	' throwErrorIfExists
	Public Property Get throwErrorIfExists
		throwErrorIfExists = parent.throwErrorIfExists
	End Property
	Public Property Let throwErrorIfExists(throwErrorIfExists__par)
		parent.throwErrorIfExists = throwErrorIfExists__par
	End Property

	'  Inherited properties as Property
	'-------------------------------------------

	'  Inherited methods
	'-------------------------------------------
	Public Sub Init (ByRef tNG__param)
		parent.Init  tNG__param
	End Sub

	Public Sub setTable (table__param)
		parent.setTable table__param
	End Sub

	Public Sub setFieldName (field__param)
		parent.setFieldName field__param
	End Sub

	Public Sub setFieldType (fieldtype__param)
		parent.setFieldType fieldtype__param
	End Sub

	Public Sub setFieldValue (value__param)
		parent.setFieldValue value__param
	End Sub

	Public Sub errorIfExists (throwErrorIfExists__param)
		parent.errorIfExists throwErrorIfExists__param
	End Sub

	Public Sub setErrorMsg (errorMsg__param)
		parent.setErrorMsg errorMsg__param
	End Sub


End Class
%>