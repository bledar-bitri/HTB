<%
Class tNG_multipleDelete
	Public multipleIdx
	Public executeSubSets
	Public KT_RESERVED 	' dictionary for dynamic fields

	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_multiple
		parent.SetContextObject Me
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub Init(ByRef connection__par)
		parent.Init  connection__par
		executeSubSets = True
		
		transactionType = "_multipleDelete"
		exportRecordset = false
		multipleIdx = null
	End Sub


	Public Function prepareSQL()
		tNG_log__log "tNG_multipleDelete", "prepareSQL", "begin"
		Dim ret: Set ret = nothing
		noSuccess = 0
		failed = false
		
		Set multTNGsObj = multTNGs
		Dim i: i = 1
		Do While true
			tmp = tNG_getRealValue("POST", pkName & "_" & i)
			If Not KT_isSet(tmp) Then
				Exit Do
			End If

			Set tNGi = new tNG_delete
			tNGi.Init connectionString
			Set disp = this.getDispatcher()
			tNGi.setDispatcher disp
			tNGi.multipleIdx = i
			Set multTNGsObj(i-1) = tNGi
			
			' register triggers
			tNGi.registerTrigger Array("STARTER", "Trigger_Default_Starter", 1, "VALUE", "1")
			Set mT = multTriggers
			For each j in mT
				currParams = mT(j)
				tNGi.registerConditionalTrigger currParams(0), currParams(1)
			Next
			' add columns
			tNGi.setTable table
			Set cols = columns
			For each colName in cols
				Set colDetails = cols(colName)
				tNGi.addColumn colName, colDetails("type"), colDetails("method"), colDetails("reference") & "_"  & i, ""
			Next
			Set pk = primaryKeyColumn
			tNGi.setPrimaryKey primaryKey, pk("type"), "POST", pkName & "_"  & i
			
			tNGi.executeTransaction()

			If KT_isSet(tNGi.getError()) Then
				failed =true
			End If

			i = i + 1	
		Loop


		If failed Then
			Set ret = new tNG_error
			ret.Init "MDEL_ERROR", array(), array()
		End If
		
		tNG_log__log "tNG_multipleDelete", "prepareSQL", "end"
		Set prepareSQL = ret
	End Function



	Public Function getLocalRecordset()
		Set myErr = new tNG_error
		myErr.Init "MDEL_NO_RS", array(), array()
		setError myErr
		Set getLocalRecordset = nothing
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

	'  Inherited properties from tNG_multiple
	'-------------------------------------------
	' multTNGs
	Public Property Get multTNGs
		Set multTNGs = parent.multTNGs
	End Property
	Public Property Set multTNGs(ByRef multTNGs__par)
		Set parent.multTNGs = multTNGs__par
	End Property

	' multTriggers
	Public Property Get multTriggers
		Set multTriggers = parent.multTriggers
	End Property
	Public Property Set multTriggers(ByRef multTriggers__par)
		Set parent.multTriggers = multTriggers__par
	End Property

	' noSuccess
	Public Property Get noSuccess
		noSuccess = parent.noSuccess
	End Property
	Public Property Let noSuccess(noSuccess__par)
		parent.noSuccess = noSuccess__par
	End Property

	' errorWasCompiled
	Public Property Get errorWasCompiled
		errorWasCompiled = parent.errorWasCompiled
	End Property
	Public Property Let errorWasCompiled(errorWasCompiled__par)
		parent.errorWasCompiled = errorWasCompiled__par
	End Property

	'  Inherited properties as Property
	'-------------------------------------------
	Public Property Get columns
		Set columns = parent.columns
	End Property
	Public Property Set columns(ByRef columns__par)
		Set parent.columns = columns__par
	End Property
	Public Property Get primaryKey
		primaryKey = parent.primaryKey
	End Property
	Public Property Let primaryKey(primaryKey__par)
		parent.primaryKey = primaryKey__par
	End Property
	Public Property Get primaryKeyColumn
		Set primaryKeyColumn = parent.primaryKeyColumn
	End Property
	Public Property Set primaryKeyColumn(ByRef primaryKeyColumn__par)
		Set parent.primaryKeyColumn = primaryKeyColumn__par
	End Property
	Public Property Get pkName
		pkName = parent.pkName
	End Property
	Public Property Let pkName(pkName__par)
		parent.pkName = pkName__par
	End Property
	Public Property Get savedData
		Set savedData = parent.savedData
	End Property
	Public Property Set savedData(ByRef savedData__par)
		Set parent.savedData = savedData__par
	End Property
	Public Property Get table
		table = parent.table
	End Property
	Public Property Let table(table__par)
		parent.table = table__par
	End Property
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
	Public Function registerTrigger(params)
		registerTrigger = parent.registerTrigger(params)
	End Function

	Public Function registerConditionalTrigger(condition, params)
		registerConditionalTrigger = parent.registerConditionalTrigger(condition, params)
	End Function

	Public Sub compileError()
		parent.compileError 
	End Sub

	Public Function getErrorMsg()
		getErrorMsg = parent.getErrorMsg()
	End Function

	Public Function getFieldError(fName, cnt)
		getFieldError = parent.getFieldError(fName, cnt)
	End Function

	Public Function getFakeRecordset(ByRef fakeArr)
		Set getFakeRecordset = parent.getFakeRecordset( fakeArr)
	End Function

	Public Function getRecordset()
		Set getRecordset = parent.getRecordset()
	End Function

	Public Function getSavedValue(colName)
		getSavedValue = parent.getSavedValue(colName)
	End Function


	Public Function executeTransaction()
		executeTransaction = parent.executeTransaction()
	End Function

	Public Sub addColumn (colName__param, type__param, method__param, reference__param)
		parent.addColumn colName__param, type__param, method__param, reference__param
	End Sub

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

	Public Function getFakeRsArr()
		Set getFakeRsArr = parent.getFakeRsArr()
	End Function

	Public Function parseSQLError (sql, error)
		Set parseSQLError = parent.parseSQLError(sql, error)
	End Function

	Public Function afterUpdateField (fieldName, fieldValue)
		Set afterUpdateField = parent.afterUpdateField(fieldName, fieldValue)
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

	Public Function doTransaction()
		doTransaction = parent.doTransaction()
	End Function

	Public Sub rollBackTransaction(ByRef errorObj__param)
		parent.rollBackTransaction  errorObj__param
	End Sub

End Class
%>