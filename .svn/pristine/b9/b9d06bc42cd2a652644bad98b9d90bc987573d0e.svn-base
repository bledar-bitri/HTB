<%

Class tNG_insert
	Public multipleIdx
	Public executeSubSets
	Public KT_RESERVED 	' dictionary for dynamic fields

	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_fields
		parent.SetContextObject Me
		
		Set KT_RESERVED = Server.CreateObject("Scripting.Dictionary")
	End Sub

	Private Sub Class_Terminate()
	End Sub

	'************ START SPECIFIC ********************		
	Public Sub Init(ByRef connection__par)
		parent.Init  connection__par
		transactionType = "_insert"
		exportRecordset = true
		registerTrigger Array("ERROR", "Trigger_Default_Insert_RollBack", 99)
		
		multipleIdx = null
		executeSubSets = false
	End Sub

	Public Function doTransaction()
		' Login code
		conn_name = tNG_login_config("connection")
		Execute "conn_string = MM_" & conn_name & "_STRING"
		login_conn = false
		If conn_string = connectionString Then
			login_conn = true
		End If	
			
		table = this.getTable()
		If login_conn And tNG_login_config("table") = table Then
			' BEFORE triggers
			this.registerTrigger Array("BEFORE", "Trigger_Registration_CheckUniqueUsername", 200)
			this.registerTrigger Array("BEFORE", "Trigger_Registration_CheckPassword", 210)
			If tNG_login_config("password_encrypt") = "true" Then
				this.registerTrigger Array("BEFORE", "Trigger_Registration_EncryptPassword", 220)
			End If
			If tNG_login_config("activation_field") <> "" Then
				this.registerTrigger Array("BEFORE", "Trigger_Registration_PrepareActivation", 230)
			End If
			' AFTER triggers
			If tNG_login_config("password_encrypt") = "true" Then
				this.registerTrigger Array("AFTER", "Trigger_Registration_RestorePassword", -10)
			End If
			this.registerTrigger Array("AFTER", "Trigger_Registration_AddDynamicFields", -5)
		End If
		doTransaction = parent.doTransaction()
	End Function

	Public Function prepareSQL()
		tNG_log__log "tNG_insert", "prepareSQL", "begin"
		parent.prepareSQL()
		prep_sql = "INSERT INTO " & table
		tmColStr = ""
		tmValStr = ""
		MM_sp = false
		
		' generate the column and the value strings
		Set cols = columns
		For each colName in cols
			Set colDetail = cols(colName)
			colType = colDetail("type")
			colValue = colDetail("value")
			colMethod = colDetail("method")
			If colMethod <> "HIDDEN" Then
				' if we handle a hidden field, we should not use it in the update SQL.
				if MM_sp then
					sep = ", "
				else
					sep = ""
				end if
				MM_sp = true
				' build the nameList and valueList
				tmColStr = tmColStr & sep & KT_escapeFieldName(colName)
				If colType = "FILE_TYPE" Then	
					' if we handle a file upload, the file name will be set afterwards.
					tmValStr = tmValStr & sep & "''"
				Else
					tmValStr = tmValStr & sep & KT_escapeForSql(colValue, colType)
				End If
			End If
		Next

		If not MM_sp Then
			' no column was actually added
			Response.write "tNG_insert.prepareSQL:<br />Please specify some fields to insert."
			Response.End()
		End If

		' build the final SQL
		prep_sql = prep_sql & " (" & tmColStr & ") VALUES (" & tmValStr & ")"
		tNG_log__log "tNG_insert", "prepareSQL", "end"
		this.setSQL (prep_sql)
		Set prepareSQL = nothing
	End Function


	Public Function getLocalRecordset()
		tNG_log__log "tNG_insert", "getLocalRecordset", null
		Dim fakeArr: Set fakeArr = Server.CreateObject("Scripting.Dictionary")
		Set tmpArr = KT_cloneObject(columns)
		If not tmpArr.Exists(primaryKey) Then
			Set tmpArr(primaryKey) = primaryKeyColumn
			tmpArr(primaryKey)("default") = null
		End If
		For each colName in tmpArr
			Set colDetails = tmpArr(colName)
			tmpVal = KT_escapeForFakeRs(colDetails("default"), colDetails("type"))
			fakeArr(colName) = tmpVal
		Next
		Set getLocalRecordset = this.getFakeRecordset(fakeArr)
	End Function


	Public Sub addColumn(colName__param, type__param, method__param, reference__param, defaultValue)
		parent.addColumn colName__param, type__param, method__param, reference__param
		Set cols = columns
		If method__param = "VALUE" Then
			cols(colName__param)("default") = reference__param
		Else
			cols(colName__param)("default") = defaultValue
		End If
	End Sub


	Public Function postExecuteSql()
		tNG_log__log "tNG_insert", "postExecuteSql", null
		Set cols = columns
		Set pkCol = primaryKeyColumn
		If cols.Exists(primaryKey) Then
			pkCol("value") = this.getColumnValue(primaryKey)
		Else
			use_max = false
		
			strSQL = "SELECT @@IDENTITY as newid"
			On Error Resume Next
			Set rsID = connection.Execute(strSQL)
			pkValue = Cstr(rsID.Fields.Item("newid").Value & "")
			On Error GoTo 0
			If isNull(pkValue) Or pkValue = "" OR pkValue = "0" Then
				use_max = true
			End If
			If use_max Then			
				strSQL = "SELECT MAX(" & KT_escapeFieldName(primaryKey) & ") as newid FROM " & KT_escapeFieldName(table)
				On Error Resume Next
				Set rsID = connection.Execute(strSQL)
				pkValue = Cstr(rsID.Fields.Item("newid").Value & "")
				If err.Number <> 0 Then
					Set myErr = new tNG_error 
					myErr.Init "FIELDS_GET_PK_ERROR", array(), array(err.Description)
					this.setError myErr			
					Set postExecuteSql = myErr
					On Error GoTo 0
					Exit Function
				End If
			End If
			pkCol("value") = pkValue
		End If
		Set postExecuteSql = nothing
	End Function

	Public Function wereValuesSubmitted()
		ret = false
		Set cols = columns
		For each colName in cols
			Set colDetails = cols(colName)
			If colDetails("method") = "POST" Then
				If colDetails("default") <> colDetails("value") Then
					ret = true
					Exit For
				End If
			End If
		Next
		wereValuesSubmitted = ret
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

	'  Inherited properties from tNG_fields
	'-------------------------------------------
	' columns
	Public Property Get columns
		Set columns = parent.columns
	End Property
	Public Property Set columns(ByRef columns__par)
		Set parent.columns = columns__par
	End Property

	' primaryKey
	Public Property Get primaryKey
		primaryKey = parent.primaryKey
	End Property
	Public Property Let primaryKey(primaryKey__par)
		parent.primaryKey = primaryKey__par
	End Property

	' primaryKeyColumn
	Public Property Get primaryKeyColumn
		Set primaryKeyColumn = parent.primaryKeyColumn
	End Property
	Public Property Set primaryKeyColumn(ByRef primaryKeyColumn__par)
		Set parent.primaryKeyColumn = primaryKeyColumn__par
	End Property

	' pkName
	Public Property Get pkName
		pkName = parent.pkName
	End Property
	Public Property Let pkName(pkName__par)
		parent.pkName = pkName__par
	End Property

	' savedData
	Public Property Get savedData
		Set savedData = parent.savedData
	End Property
	Public Property Set savedData(ByRef savedData__par)
		Set parent.savedData = savedData__par
	End Property

	' table
	Public Property Get table
		table = parent.table
	End Property
	Public Property Let table(table__par)
		parent.table = table__par
	End Property

	'  Inherited properties as Property
	'-------------------------------------------
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
	Public Property Get sql
		sql = parent.sql
	End Property
	Public Property Let sql(sql__par)
		parent.sql = sql__par
	End Property
	Public Property Get triggers
		Set triggers = parent.triggers
	End Property
	Public Property Set triggers(ByRef triggers__par)
		Set parent.triggers = triggers__par
	End Property
	Public Property Get started
		started = parent.started
	End Property
	Public Property Let started(started__par)
		parent.started = started__par
	End Property
	Public Property Get transactionType
		transactionType = parent.transactionType
	End Property
	Public Property Let transactionType(transactionType__par)
		parent.transactionType = transactionType__par
	End Property
	Public Property Get exportRecordset
		exportRecordset = parent.exportRecordset
	End Property
	Public Property Let exportRecordset(exportRecordset__par)
		parent.exportRecordset = exportRecordset__par
	End Property
	Public Property Get transactionResult
		Set transactionResult = parent.transactionResult
	End Property
	Public Property Set transactionResult(ByRef transactionResult__par)
		Set parent.transactionResult = transactionResult__par
	End Property
	Public Property Get errorObj
		Set errorObj = parent.errorObj
	End Property
	Public Property Set errorObj(ByRef errorObj__par)
		Set parent.errorObj = errorObj__par
	End Property
	Public Property Get dispatcher
		Set dispatcher = parent.dispatcher
	End Property
	Public Property Set dispatcher(ByRef dispatcher__par)
		Set parent.dispatcher = dispatcher__par
	End Property

	'  Inherited methods
	'-------------------------------------------
	Public Function evaluateNumeric(expr)
		evaluateNumeric = parent.evaluateNumeric(expr)
	End Function

	Public Sub setColumnValue(colName, colValue)
		parent.setColumnValue colName, colValue
	End Sub

	Public Sub setRawColumnValue(colName, colValue)
		parent.setRawColumnValue colName, colValue
	End Sub

	Public Function getColumnValue(colName)
		getColumnValue = parent.getColumnValue(colName)
	End Function

	Public Function getColumnReference(colName)
		getColumnReference = parent.getColumnReference(colName)
	End Function

	Public Function getColumnType(colName)
		getColumnType = parent.getColumnType(colName)
	End Function

	Public Sub setTable(tableName)
		parent.setTable tableName
	End Sub

	Public Function getTable()
		getTable = parent.getTable()
	End Function

	Public Sub setPrimaryKey (colName__param, type__param, method__param, reference__param)
		parent.setPrimaryKey colName__param, type__param, method__param, reference__param
	End Sub

	Public Function getPrimaryKey()
		getPrimaryKey = parent.getPrimaryKey()
	End Function

	Public Function getPrimaryKeyValue()
		getPrimaryKeyValue = parent.getPrimaryKeyValue()
	End Function

	Public Sub compileColumnsValues()
		parent.compileColumnsValues 
	End Sub

	Public Function getFakeRecordset(ByRef fakeArr)
		Set getFakeRecordset = parent.getFakeRecordset( fakeArr)
	End Function

	Public Function getRecordset()
		Set getRecordset = parent.getRecordset()
	End Function

	Public Function getFakeRsArr()
		Set getFakeRsArr = parent.getFakeRsArr()
	End Function

	Public Function parseSQLError (sql, error)
		Set parseSQLError = parent.parseSQLError(sql, error)
	End Function

	Public Function afterUpdateField (fieldName, fieldValue)
		Set afterUpdateField = parent.afterUpdateField(fieldName, fieldValue)
	End Function

	Public Function getFieldError(fName)
		getFieldError = parent.getFieldError(fName)
	End Function

	Public Function getSavedValue(colName)
		getSavedValue = parent.getSavedValue(colName)
	End Function

	Public Function saveData()
		Set saveData = parent.saveData()
	End Function

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

	Public Sub setError (ByRef errorObj__param)
		parent.setError  errorObj__param
	End Sub

	Public Function getError()
		Set getError = parent.getError()
	End Function

	Public Function getErrorMsg()
		getErrorMsg = parent.getErrorMsg()
	End Function

	Public Function executeTransaction()
		executeTransaction = parent.executeTransaction()
	End Function

	Public Sub rollBackTransaction(ByRef errorObj__param)
		parent.rollBackTransaction  errorObj__param
	End Sub
End Class

%>