<%
Class tNG_fields

	'************ START SPECIFIC ********************		
	Public columns ' object
	Public primaryKey
	Public primaryKeyColumn ' object
	Public pkName
	Public savedData ' object
	Public table
	


	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG
		parent.SetContextObject Me
		
		Set columns = Server.CreateObject("Scripting.Dictionary")
		Set primaryKeyColumn = Server.CreateObject("Scripting.Dictionary")
		Set savedData = Server.CreateObject("Scripting.Dictionary")
		pkName = "kt_pk"
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub Init(ByRef connection__par)
		parent.Init  connection__par
	End Sub



	' overwritten
	Public Function executeTransaction()
		this.compileColumnsValues
		executeTransaction = parent.executeTransaction()
	End Function


	Public Sub addColumn (colName__param, type__param, method__param, reference__param) 
		If started And method__param <> "VALUE" and method__param <> "EXPRESSION" Then
			Response.write "You can only add supplemental columns by value or by expression once the transaction is started."
		End If
		If Not columns.Exists(colName__param) Then
			Set columns(colName__param) = Server.CreateObject("Scripting.Dictionary")
		End If
		columns(colName__param)("type") = type__param
		columns(colName__param)("method") = method__param
		columns(colName__param)("reference") = reference__param
		
		If method__param = "VALUE" Then
			columns(colName__param)("value") = reference__param
		End If
		If method__param = "EXPRESSION" Then
			columns(colName__param)("method") = "VALUE"
			useSavedData = false
			If KT_in_array(this.getTransactionType(), Array("_delete", "_multipleDelete"), false) Then
				useSavedData  = true
			End If
			columns(colName__param)("reference") = tNG_DynamicData(reference__param, this, useSavedData, null, null, null)
			If type__param = "NUMERIC_TYPE" Or type__param = "DOUBLE_TYPE" Then
				columns(colName__param)("reference") = this.evaluateNumeric(columns(colName__param)("reference"))
			End If 
		End If
		If started Then
			tNG_prepareValues columns(colName__param)
		End If
	End Sub

	
	Public Function evaluateNumeric(expr)
		retVal = null
		If KT_preg_test("^[\d\*\-\+\/\.\(\)]+$", expr) Then
			ok = false
			On Error Resume Next
			Execute "retVal=" & expr
			If err.number <> 0 Then
				retVal = null
				Set myErr = new tNG_error
				myErr.Init "FIELDS_EVAL_EXPR_FAILED", array(), array(expr)
				this.setError myErr
				On Error GoTo 0				
				Exit Function
			End If
			On Error GoTo 0
		Else
			If tNG_debug_mode = "DEVELOPMENT" Then
				Set myErr = new tNG_error
				myErr.Init "FIELDS_EVAL_EXPR_INVALID", array(), array(expr)
				this.setError myErr
				On Error GoTo 0				
				Exit Function	
			End If
		End If
		evaluateNumeric = retVal
	End Function
	
	
	
	Public Sub setColumnValue(colName, colValue)
		If columns.Exists(colName) Then
			columns(colName)("value") = colValue
			columns(colName)("reference") = colValue
			columns(colName)("method") = "VALUE"
		Else
			Response.write "tNG_fields.setColumnValue:<br />Column " & colName & " is not part of the current transaction."
			Response.End()
		End If
	End Sub
	
	Public Sub setRawColumnValue(colName, colValue)
		If columns.Exists(colName) Then
			columns(colName)("value") = colValue
		Else
			Response.write "tNG_fields.setRawColumnValue:<br />Column " & colName & " is not part of the current transaction."
			Response.End()
		End If
	End Sub	
	
	Public Function getColumnValue(colName)
		If columns.Exists(colName) Then
			getColumnValue = columns(colName)("value")
		ElseIf colName = this.getPrimaryKey() Then
			getColumnValue = this.getPrimaryKeyValue()
		Else
			Response.write "tNG_fields.getColumnValue:<br />Column " & colName & " is not part of the current transaction."
			Response.End()			
		End If
	End Function


	Public Function getColumnReference(colName) 
		If columns.Exists(colName) Then
			getColumnReference = columns(colName)("reference")
		Else
			Response.write "tNG_fields.getColumnReference:<br />Column " & colName & " is not part of the current transaction."
			Response.End()
		End If
	End Function

	Public Function getColumnType(colName)
		Set cols = columns
		If cols.Exists(colName) Then
			getColumnType = cols(colName)("type")
		ElseIf colName = primaryKey Then
			getColumnType = primaryKeyColumn("type")
		Else
			Response.write "tNG_fields.getColumnType:<br />Column " & colName & " is not part of the current transaction."
			Response.End()
		End If
	End Function


	Public Sub setTable(tableName) 
		If table = "" Then
			table = tableName
			pkName = pkName & "_" & KT_preg_replace("[^\w]", "_", table)
		Else
			Response.write "tNG_fields.setTable:<br />The table has already been set."
			Response.End()
		End If
	End Sub
	
	Public Function getTable()
		getTable = table
	End Function

	Public Sub setPrimaryKey (colName__param, type__param, method__param, reference__param)
		If isNull(method__param) Or method__param = "" Then
			method__param = "VALUE"
		End If
		
		primaryKey = colName__param
		primaryKeyColumn("type") = type__param
		primaryKeyColumn("method") = method__param
		primaryKeyColumn("reference") = reference__param
	End Sub

	Public Function getPrimaryKey()
		getPrimaryKey = primaryKey
	End Function

	Public Function getPrimaryKeyValue()
		If primaryKeyColumn.Exists("value") Then
			getPrimaryKeyValue = primaryKeyColumn("value")
		Else
			getPrimaryKeyValue = null
		End If
	End Function

	Public Sub compileColumnsValues()
		'  Use multiple values in kt_pk or from get
		Set savedPK = KT_cloneObject(primaryKeyColumn)
		primaryKeyColumn("method") = "POST"
		primaryKeyColumn("reference") = pkName
		tNG_prepareValues primaryKeyColumn
		If not KT_isSet(primaryKeyColumn("value")) Then
			Set primaryKeyColumn = savedPK
			tNG_prepareValues primaryKeyColumn
		End If
		For each colName in columns
			tNG_prepareValues columns(colName)
		Next
	End Sub

	Public Function getLocalRecordset()
		Set myErr = new tNG_error 
		myErr.Init "FIELDS_LOCAL_RS", array(), array()
		this.setError myErr
		Set getLocalRecordset = nothing
	End Function


	Public Function getFakeRecordset(ByRef fakeArray) 
		tNG_log__log  "tNG" & transactionType, "getFakeRecordset", null
		
		' CREATE FAKE RS
		' ---- Cursor Location 
		Const adUseServer = 2
		'Const adUseClient = 3

		' ---- CursorType
		'Const adOpenForwardOnly = 0
		'Const adOpenKeyset = 1
		Const adOpenDynamic = 2
		'Const adOpenStatic = 3
		
		' ---- LockType
		'Const adLockReadOnly = 1
		'Const adLockPessimistic = 2
		Const adLockOptimistic = 3
		'Const adLockBatchOptimistic = 4


		Set fakeRS = new KT_FakeRecordset

		fakeRS.CursorLocation = adUseServer
		fakeRS.CursorType = adOpenDynamic
		fakeRS.LockType = adLockOptimistic
		
		' add fields
		For each key in fakeArray
			fakeRS.Fields.Append key
		Next
		
		fakeRS.Open 
		
		' add values
		If fakeArray.Count > 0 Then		
			fakeRS.AddNew 
			For each key in fakeArray
				fakeRS.Fields(key).value = fakeArray(key)
			Next
		End If

		If not fakeRS.BOF Then
			fakeRS.MoveFirst
		End if
		Set getFakeRecordset = fakeRS
	End Function



	Public Function getRecordset()
		tNG_log__log "tNG" & transactionType, "getRecordset", null 
		If KT_isSet(this.getError()) Then
			Set getRecordset = this.getFakeRecordset(this.getFakeRsArr())
		Else
			Set getRecordset = this.getLocalRecordset()
		End If
	End Function


	Public Function prepareSQL()
		If table = "" Then
			Set myErr = new tNG_error 
			myErr.Init "FIELDS_NO_TABLE", array(), array(sql)
			Set prepareSQL = myErr
			Exit Function
		End If
		Set prepareSQL = nothing
	End Function


	Public Function getFakeRsArr()
		tNG_log__log  "tNG" & transactionType, "getFakeRsArr", null
		Set localRs = this.getLocalRecordset()
		
		Dim fakeArr: Set fakeArr = Server.CreateObject("Scripting.Dictionary")
		Set tmpArr = KT_cloneObject(columns)
		If primaryKey <> "" Then
			If not tmpArr.Exists(primaryKey) Then
				Set tmpArr(primaryKey) = primaryKeyColumn
			End If
		End If
		
		'Transaction was executed and failed, create the recordset from the submitted values
		For each colName in tmpArr
			Set colDetails = tmpArr(colName)
			If colDetails("method") = "CURRVAL" Then
				value = KT_escapeForFakeRs(localRs(colName), "STRING_TYPE")
			Else
				value = KT_escapeForFakeRs(colDetails("value"), "STRING_TYPE")
				If value = "null" Then
					If colDetails("type") = "CHECKBOX_1_0_TYPE" Or colDetails("type") = "CHECKBOX_-1_0_TYPE" Or colDetails("type") = "CHECKBOX_YN_TYPE" Or colDetails("type") = "CHECKBOX_TF_TYPE" Then
						value = "''"
					End If
				End If
			End If
			fakeArr(colName) = value
		Next
		savedPK = this.getSavedValue(primaryKey)
		If not isnull(savedPK) Then
			fakeArr(pkName) = KT_escapeForFakeRs(savedPK, "STRING_TYPE")
		Else
			fakeArr(pkName) = "''"		
		End If
		Set getFakeRsArr = fakeArr
	End Function


	Public Function parseSQLError (sql, error)
		Set errObj = parent.parseSQLError(sql, error)
		If KT_isSet(errObj) Then
			For each colName in columns
				If KT_preg_test("^.*[^a-z]+" & KT_preg_quote(colName)  & "[^a-z]+.*$", error)  Then
					fieldError = KT_preg_replace("\[[^\]]*\]", "", error)
					errObj.setFieldError colName, "%s", array(fieldError)
					Exit For
				End If
			Next	
		End If
		Set parseSQLError = errObj
	End Function


	Public Function afterUpdateField (fieldName, fieldValue)
		tNG_log__log "tNG" & transactionType, "afterUpdateField",  fieldName & ", " & fieldValue
		keyName = primaryKey
		keyValue = primaryKeyColumn("value")
		localsql = "UPDATE " &  table &  " SET " & KT_escapeFieldName(fieldName) & " = '" & fieldValue & "' WHERE " & KT_escapeFieldName(keyName) & " = "  & KT_escapeForSql(keyValue, primaryKeyColumn("type"))
		On Error Resume Next
		connection.Execute (localsql)
		If err.Number <> 0 Then
			Set myErr = new tNG_error 
			myErr.Init "FIELDS_AFTER_UPDATE_ERROR", array(), array(err.Description)
			Set afterUpdateField = myErr
			On Error GoTo 0
			Exit Function
		End If
		On Error GoTo 0
		Set afterUpdateField = nothing
	End Function
	

	Public Function getFieldError(fName) 
		getFieldError = ""
		Set tmp = this.getError()
		If KT_isSet(tmp) Then
			getFieldError = tmp.getFieldError(fName)
		End If
	End Function

	Public Function getSavedValue(colName)
		If  savedData.Exists(colName) Then
			getSavedValue = savedData(colName)
		Else
			getSavedValue = null
		End If
	End Function

	Public Function saveData() 
		tNG_log__log "tNG" & transactionType, "saveData",  null
		keyName = this.getPrimaryKey()
		keyValue = this.getPrimaryKeyValue()
		keyType = this.getColumnType(keyName)
		escapedKeyValue = KT_escapeForSql(keyValue, keyType)
		local_sql = "SELECT * FROM " & this.getTable() & " WHERE " & KT_escapeFieldName(keyName) & " = " & escapedKeyValue
		On Error Resume Next
		Set rs = connection.Execute(local_sql)
		If err.Number <> 0 Then
			Set myErr = new tNG_error 
			myErr.Init "FIELDS_SAVEDATA_ERROR", array(), array(local_sql, err.Description)
			Set saveData = myErr
			On Error GoTo 0
			Exit Function
		End If
		If not rs.EOF Then
			For each f in rs.Fields
				savedData(f.name) = rs(f.name)
			Next		
		End If
		Set saveData = nothing
	End Function
	
	'************ END SPECIFIC ********************		
	
	
	
	

	'===========================================
	' Inheritance
	'===========================================
	Public this
	Public parent

	Public Sub SetContextObject(ByRef objContext)
		Set this = objContext
		parent.SetContextObject objContext
	End Sub

	'  Inherited properties from tNG
	'-------------------------------------------
	' connection
	Public Property Get connection
		Set connection = parent.connection
	End Property
	Public Property Set connection(ByRef connection__par)
		Set parent.connection = connection__par
	End Property

	' connectionString
	Public Property Get connectionString
		connectionString = parent.connectionString
	End Property
	Public Property Let connectionString(connectionString__par)
		parent.connectionString = connectionString__par
	End Property

	' sql
	Public Property Get sql
		sql = parent.sql
	End Property
	Public Property Let sql(sql__par)
		parent.sql = sql__par
	End Property

	' triggers
	Public Property Get triggers
		Set triggers = parent.triggers
	End Property
	Public Property Set triggers(ByRef triggers__par)
		Set parent.triggers = triggers__par
	End Property

	' started
	Public Property Get started
		started = parent.started
	End Property
	Public Property Let started(started__par)
		parent.started = started__par
	End Property

	' transactionType
	Public Property Get transactionType
		transactionType = parent.transactionType
	End Property
	Public Property Let transactionType(transactionType__par)
		parent.transactionType = transactionType__par
	End Property

	' exportRecordset
	Public Property Get exportRecordset
		exportRecordset = parent.exportRecordset
	End Property
	Public Property Let exportRecordset(exportRecordset__par)
		parent.exportRecordset = exportRecordset__par
	End Property

	' transactionResult
	Public Property Get transactionResult
		Set transactionResult = parent.transactionResult
	End Property
	Public Property Set transactionResult(ByRef transactionResult__par)
		Set parent.transactionResult = transactionResult__par
	End Property

	' errorObj
	Public Property Get errorObj
		Set errorObj = parent.errorObj
	End Property
	Public Property Set errorObj(ByRef errorObj__par)
		Set parent.errorObj = errorObj__par
	End Property

	' dispatcher
	Public Property Get dispatcher
		Set dispatcher = parent.dispatcher
	End Property
	Public Property Set dispatcher(ByRef dispatcher__par)
		Set parent.dispatcher = dispatcher__par
	End Property

	'  Inherited properties as Property
	'-------------------------------------------

	'  Inherited methods
	'-------------------------------------------
	Public Sub setDispatcher(ByRef dispatcher__par)
		parent.setDispatcher  dispatcher__par
	End Sub

	Public Function getDispatcher()
		Set getDispatcher = parent.getDispatcher()
	End Function

	Public Function exportsRecordset()
		exportsRecordset = parent.exportsRecordset()
	End Function

	Public Function registerConditionalTrigger(condition, params)
		parent.registerConditionalTrigger condition, params
	End Function

	Public Function registerTrigger(params)
		registerTrigger = parent.registerTrigger(params)
	End Function

	Public Function executeTriggers (i_triggerType)
		Set executeTriggers = parent.executeTriggers(i_triggerType)
	End Function

	Public Function getTransactionType()
		getTransactionType = parent.getTransactionType()
	End Function

	Public Function isStarted()
		isStarted = parent.isStarted()
	End Function

	Public Sub setStarted (started__param)
		parent.setStarted started__param
	End Sub

	Public Sub setSQL (sql__param)
		parent.setSQL sql__param
	End Sub

	Public Function postExecuteSql()
		Set postExecuteSql = parent.postExecuteSql()
	End Function

	Public Sub setError (ByRef errorObj__param)
		parent.setError  errorObj__param
	End Sub

	Public Function getError()
		Set getError = parent.getError()
	End Function

	Public Function getErrorMsg()
		getErrorMsg = parent.getErrorMsg()
	End Function

	Public Function doTransaction()
		doTransaction = parent.doTransaction()
	End Function

	Public Sub rollBackTransaction(ByRef errorObj__param)
		parent.rollBackTransaction  errorObj__param
	End Sub

End Class
%>