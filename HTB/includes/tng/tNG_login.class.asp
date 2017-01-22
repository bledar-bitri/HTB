<%
Class tNG_Login
	Public loginType

	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_Custom
		parent.SetContextObject Me
	End Sub

	Private Sub Class_Terminate()
	End Sub

	Public Sub Init(ByRef connection__par)
		parent.Init  connection__par
		loginType = "form"
		transactionType = "_login"
		
		' TODO: Check that tNG_login_config("table") really exist. If not, die w/error
		If tNG_login_config("table") = "" Then
			Response.write "Internal error. Please configure your login table in InterAKT Control Panel > Login Settings."
			Response.End()
		End If
		
		If tNG_login_config("pk_field") = "" Or tNG_login_config("pk_type") = "" Then
			Response.write "Internal error. Please configure your login table in InterAKT Control Panel > Login Settings."
			Response.End()			
		End If

		this.setPrimaryKey tNG_login_config("pk_field"), tNG_login_config("pk_type"), "", ""
		exportRecordset = true
		this.registerTrigger Array("AFTER", "Trigger_Login_CheckLogin", -10)
		If tNG_login_config("activation_field") <> "" Then
			this.registerTrigger Array("AFTER", "Trigger_Login_CheckUserActive", -8)
		End If
		this.registerTrigger Array("AFTER", "Trigger_Login_AddDynamicFields", -6)	
		this.registerTrigger Array("AFTER", "Trigger_Login_SaveDataToSession", -4)	
		this.registerTrigger Array("AFTER", "Trigger_Login_AutoLogin", -2)	
	End Sub

	Public Function setLoginType(loginType__param)
		loginType = loginType__param
	End Function


	Public Function prepareSQL()
		tNG_log__log "tNG_login", "prepareSQL", "begin"
		login_table = tNG_login_config("table")
		pk_column = this.getPrimaryKey()
		user_column = tNG_login_config("user_field")
		password_column = tNG_login_config("password_field")
	
		local_sql = "SELECT *, " & KT_escapeFieldName(pk_column) & " AS kt_login_id, " & _
			KT_escapeFieldName(user_column) & " AS kt_login_user, " & _
 			KT_escapeFieldName(password_column) & " AS kt_login_password FROM " & login_table
		If loginType  = "form" Then
			local_sql = local_sql &  " WHERE " & KT_escapeFieldName(user_column) &  "={kt_login_user}"
		Else
			local_sql = local_sql &  " WHERE " & KT_escapeFieldName(pk_column) &  "={kt_login_id}"
		End If
		local_sql = tNG_DynamicData(local_sql, Me, "SQL", null, null, null)
		this.setSQL local_sql
		tNG_log__log "tNG_login", "prepareSQL", "end"
		Set prepareSQL = nothing
	End Function

	Public Function getLocalRecordset()
		tNG_log__log "tNG_login", "getLocalRecordset", null
		Set fakeArr  = Server.CreateObject("Scripting.Dictionary")
		Set tmpArr = columns
		For each colName in tmpArr
			Set colDetails = tmpArr(colName)
			tmpVal = KT_escapeForFakeRs(colDetails("value"), colDetails("type"))
			fakeArr(colName) = tmpVal
		Next
		Set getLocalRecordset = this.getFakeRecordset(fakeArr)
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

	'  Inherited properties from tNG_Custom
	'-------------------------------------------
	' multipleIdx
	Public Property Get multipleIdx
		multipleIdx = parent.multipleIdx
	End Property
	Public Property Let multipleIdx(multipleIdx__par)
		parent.multipleIdx = multipleIdx__par
	End Property

	' executeSubSets
	Public Property Get executeSubSets
		executeSubSets = parent.executeSubSets
	End Property
	Public Property Let executeSubSets(executeSubSets__par)
		parent.executeSubSets = executeSubSets__par
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
	Public Sub addColumn (colName__param, type__param, method__param, reference__param, defaultValue__param)
		parent.addColumn colName__param, type__param, method__param, reference__param, defaultValue__param
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