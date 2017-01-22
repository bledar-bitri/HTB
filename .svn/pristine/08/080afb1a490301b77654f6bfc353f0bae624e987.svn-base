<%
Class tNG_multiple

	'************ START SPECIFIC ********************		
	Public multTNGs 'obj
	Public multTriggers 'obj
	Public noSuccess
	Public errorWasCompiled

	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_fields
		parent.SetContextObject Me
		
		Set multTNGs = Server.CreateObject("Scripting.Dictionary")
		Set multTriggers = Server.CreateObject("Scripting.Dictionary")
		errorWasCompiled = false
		noSuccess = 0
	End Sub

	Private Sub Class_Terminate()

	End Sub

	Public Function registerTrigger(params)
		If not isArray (params) Then
			Response.Write "registerTrigger must have a parameter as Array"
			Response.End()
		End If
		noParams = ubound(params) + 1
		If noParams < 3 Then
			Response.Write "registerTrigger Array parameter must have at least 3 values (trigger type, name and priority)"
			Response.End()
		End If
		
		i_triggerType = params(0)
		ret = true
		' check if the trigger type is valid
		If KT_in_array(i_triggerType, Array("STARTER","END"), True) Then
			ret = this.registerConditionalTrigger(True, params)
		ElseIf KT_in_array(i_triggerType, Array("AFTER","BEFORE","ERROR"), True) Then	
			index = multTriggers.Count
			multTriggers(index) = Array(True, params)
		Else
			Set myErr = new tNG_error
			tNG_error.Init "UNKOWN_TRIGGERS", Array(), Array(i_triggerType)
			this.setError myErr
			ret = false
		End If
		registerTrigger = True
	End Function


	Public Function registerConditionalTrigger(condition, params)
		If not isArray (params) Then
			Response.Write "registerTrigger must have a parameter as Array"
			Response.End()
		End If
		noParams = ubound(params) + 1
		If noParams < 3 Then
			Response.Write "registerTrigger Array parameter must have at least 3 values (trigger type, name and priority)"
			Response.End()
		End If
		
		Dim triggParams
		triggParams = Array()
		If noParams > 3 Then
			Redim triggParams(noParams-3-1)
			j = 0
			For i=3 to ubound(Params)
				If isObject(params(i)) Then
					Set triggParams(j) = params(i)
				Else
					triggParams(j) = params(i)
				End If
				j = j + 1
			Next
		End If
		i_triggerType = params(0)
		i_triggerCallbackFunction = params(1)
		i_triggerPriority = params(2)		
		
		ret = true
		' check if the trigger type is valid
		If KT_in_array(i_triggerType, Array("STARTER","END"), True) Then
			Set trgs = triggers
			If not trgs.Exists(i_triggerType) Then
				Set trgs(i_triggerType) = Server.CreateObject("Scripting.Dictionary")
			End If
			index = trgs(i_triggerType).Count
			trgs(i_triggerType)(index) = Array(condition, i_triggerCallbackFunction, i_triggerPriority, triggParams)
		ElseIf KT_in_array(i_triggerType, Array("AFTER","BEFORE","ERROR"), True) Then
			index = multTriggers.Count
			multTriggers(index) = Array(condition, params)
		Else
			Set myErr = new tNG_error
			tNG_error.Init "UNKOWN_TRIGGERS", Array(), Array(i_triggerType)
			this.setError myErr
			ret = false
		End If
		registerConditionalTrigger = ret
	End Function

	Public Sub compileError() 
		If Not errorWasCompiled Then
			Set errObj = this.getError()
			Dim i
			Set multTNGsObj = multTNGs
			For each i in multTNGsObj
				Set tNGi = multTNGs(i)
				Set tmp = tNGi.getError()
				If KT_isSet(tmp) Then
					errObj.addDetails "%s", Array(tmp.getDetails()), Array(tmp.getDeveloperDetails())
				End If
			Next
			errorWasCompiled = true
		End If
	End Sub

	Public Function getErrorMsg()
		ret_warning = ""
		If noSuccess <> 0 Then
			ret_warning = KT_getResource("MULTIPLE_OPERATIONS_SUCCEDED", "tNG", array(noSuccess))
		End If
		Set errObj = this.getError()
		If Not KT_isSet(errObj) Then
			getErrorMsg = Array(ret_warning, "", "")
		End If
		this.compileError
		
		ret = parent.getErrorMsg()
		ret(0) = ret(0) & ret_warning
		getErrorMsg = ret
	End Function


	' POSSIBLE PROBLEM: the method signature was broke!!!!!
	Public Function getFieldError(fName, cnt)
		Set multTNGsObj =  multTNGs
		Set tNGi = multTNGsObj(cnt-1)
		If KT_isSet(tNGi) Then
			Set tmp = tNGi.getError()
			If KT_isSet(tmp) Then
				getFieldError = tmp.getFieldError(fName)
				Exit Function
			End If
		End If
		getFieldError = ""
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
		
		boolFieldsAdded = false
		For each key in fakeArray
			If Not boolFieldsAdded Then
				For each f in fakeArray(key)
					fakeRS.Fields.Append f
				Next
				fakeRS.Open 
				boolFieldsAdded = True
			End If			
			

			' add current values
			fakeRS.AddNew 
			For each f in fakeArray(key)
				fakeRS.Fields(f).value = fakeArray(key)(f)
			Next
		Next ' for each key in fakeArray


		If not fakeRS.BOF Then
			fakeRS.MoveFirst
		End if
		
		Set getFakeRecordset = fakeRs
	End Function


	Public Function getRecordset()
		tNG_log__log "tNG" & transactionType, "getRecordset", null 
		If KT_isSet(this.getError()) Then
			Set fakeArr = Server.CreateObject("Scripting.Dictionary")
			Set multTNGsObj = multTNGs
			Dim i
			i = 0
			Do While i < multTNGsObj.Count
				Set tNGi = multTNGsObj(i)
				If KT_isSet(tNGi.getError()) Then
					Set fakeArr(i) = tNGi.getFakeRsArr()
				Else
					For j=i+1 to multTNGsObj.Count-1
						Set multTNGsObj(j-1) = multTNGsObj(j)
					Next
					multTNGsObj.remove(multTNGsObj.Count-1)
					i = i - 1
				End If
				i = i + 1
			Loop
			If fakeArr.Count > 0 Then
				Set getRecordset = this.getFakeRecordset(fakeArr)
				Exit Function
			End If
		End If
		Set getRecordset = this.getLocalRecordset()	
	End Function


	Public Function getSavedValue(colName)
		Set multTNGsObj = multTNGs
		Set tNGi = multTNGsObj(0)
		getSavedValue = tNGi.getSavedValue(colName)
	End Function


	Public Function getLocalRecordset()
		Response.write "tNG_multiple.getLocalRecordset:<br />Method must be implemented in inherited class."
		Response.End()
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
	Public Sub Init(ByRef connection__par)
		parent.Init  connection__par
	End Sub

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

	Public Function prepareSQL()
		Set prepareSQL = parent.prepareSQL()
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