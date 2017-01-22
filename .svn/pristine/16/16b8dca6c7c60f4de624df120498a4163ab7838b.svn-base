<%

Class tNG_delete
	Public multipleIdx
	Public executeSubSets
	
	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_fields
		parent.SetContextObject Me
	End Sub

	Private Sub Class_Terminate()
	End Sub

	'************ START SPECIFIC ********************		
	Public Sub Init(ByRef connection__par)
		parent.Init  connection__par
		transactionType = "_delete"
		exportRecordset = false
		registerTrigger Array("BEFORE", "Trigger_Default_saveData", -1)
		
		multipleIdx = null
		executeSubSets = false
	End Sub


	Public Function prepareSQL() 
		tNG_log__log "tNG_delete", "prepareSQL", "begin"
		parent.prepareSQL()
		' check if we have a valid primaryKey
		If primaryKey = "" Then
			Set myErr = new tNG_error
			myErr.Init "DEL_NO_PK_SET", array(), array()
			Set prepareSQL = myErr
			Exit Function
		End If

		' begin the SQL generator
		Set pkCol = primaryKeyColumn
		prep_sql = "DELETE FROM " & table & " WHERE " & KT_escapeFieldName(primaryKey) & " = "
		prep_sql = prep_sql & KT_escapeForSql(pkCol("value"), pkCol("type"))
		this.setSQL prep_sql
		tNG_log__log "tNG_delete", "prepareSQL", "end"
		Set prepareSQL = nothing
	End Function


	Public Function getLocalRecordset()
		Set myErr = new tNG_error 
		myErr.Init "DEL_NO_RS", array(), array()
		this.setError myErr
		Set getLocalRecordset = nothing
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
	Public Function postExecuteSql()
		Set postExecuteSql = parent.postExecuteSql
	End Function
	
	Public Sub addColumn(colName__param, type__param, method__param, reference__param, defaultValue)
		parent.addColumn colName__param, type__param, method__param, reference__param
	End Sub
	
	Public Function executeTransaction()
		executeTransaction = parent.executeTransaction()
	End Function

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
	End function

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

	Public Function doTransaction()
		doTransaction = parent.doTransaction()
	End Function

	Public Sub rollBackTransaction(ByRef errorObj__param)
		parent.rollBackTransaction  errorObj__param
	End Sub
End Class

%>