<%
'
'	Copyright (c) InterAKT Online 2000-2005
'
'

Class tNG
	Public connection ' object
	Public connectionString
	Public sql
	Public triggers ' object
	Public started
	Public transactionType
	Public exportRecordset
	Public transactionResult ' object
	Public errorObj ' object
	Public dispatcher ' object


	Private Sub Class_Initialize()
		Set this = Me
		transactionType = "_UNKNOWN"
		exportRecordset = false
		started = false
		Set triggers = Server.CreateObject("Scripting.Dictionary")
		Set transactionResult = nothing
		Set dispatcher = nothing
	End Sub
	
	Private Sub Class_Terminate()
		Set triggers = nothing
	End Sub

	Public Sub Init (connection__par)
		connectionString = connection__par

		Set connection = KT_GetPooledConnection(connectionString)
		KT_setDbType connection ' set global variable KT_DatabaseType
	End Sub


	'===========================================
	' Inheritance
	'===========================================
	Public this
	
	Public Sub SetContextObject(ByRef objContext)
		Set this = objContext
	End Sub
	'===========================================
	' End Inheritance
	'===========================================


	
	Public Sub setDispatcher(ByRef dispatcher__par)
		Set dispatcher = dispatcher__par
	End Sub
	
	Public Function getDispatcher()
		Set getDispatcher = dispatcher
	End Function
	
	Public Function exportsRecordset()
		exportsRecordset = exportRecordset
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
		i_triggerName = params(1)
		i_triggerPriority = params(2)		
		
		If KT_in_array(i_triggerType, Array("STARTER","AFTER","BEFORE","END","ERROR"), True) Then
			If not triggers.Exists(i_triggerType) Then
				Set triggers(i_triggerType) = Server.CreateObject("Scripting.Dictionary")
			End If
			index = triggers(i_triggerType).Count
			triggers(i_triggerType)(index) = Array(condition, i_triggerName, i_triggerPriority, triggParams)
		Else
			Set myErr = new tNG_error
			tNG_error.Init "UNKOWN_TRIGGERS", Array(), Array(i_triggerType)
			this.setError myErr
		End If
	End Function


	Public Function registerTrigger(params)
		this.registerConditionalTrigger True, params
	End Function


	Public Function prepareSQL()
		' must be overwritten
		Set prepareSQL  = nothing 
	End Function 
	

	Public Function executeTransaction()
		tNG_log__log "tNG" & transactionType, "executeTransaction", "begin"
		If started Then
			tNG_log__log "tNG" & transactionType, "executeTransaction", "end"
			executeTransaction = false
			Exit Function
		End If	
			
		' calling the starter triggers and terminate execution if we had an error
		' we do not throw the errors triggers.
		this.executeTriggers "STARTER"
		If not started Then
			tNG_log__log "tNG"  & transactionType, "executeTransaction", "end"
			executeTransaction = false
			Exit Function
		End If	
		
		ret = this.doTransaction()
		tNG_log__log "tNG"  & transactionType, "executeTransaction", "end"
		executeTransaction = ret
	End Function 	
	

	
	Public Function executeTriggers (i_triggerType)
	 	If triggers.Exists(i_triggerType) Then
			 'must execute the triggers in triggers(i_triggerType)
			' Create a separate Array for Trigger Names and Priorities, 
			'		because we cannot do Sorting on Collection  
			Dim mTriggersCollection
			Set mTriggersCollection = triggers(i_triggerType)
			
			dim mTriggersArray()
			ReDim mTriggersArray(mTriggersCollection.Count - 1, 3)
			
			Dim mTemp(3)
			Dim mArrayValue, mPriority		
			
			Dim mItem, i, j
			
		
			i = 0
			for each mItemIndex in mTriggersCollection
				mArrayValue = mTriggersCollection.Item(mItemIndex) 
				
				mTriggersArray(i, 0) = mArrayValue(1)		' trigger name
				mTriggersArray(i, 1) = mArrayValue(2)		' priority
				mTriggersArray(i, 2) = mArrayValue(3)		' params
				mTriggersArray(i, 3) = mArrayValue(0)		' condition
				
				i = i + 1
			next
			
			' Sort the Array with Priorities only if we hav more then one record
			if UBound(mTriggersArray) > 0 then
				for i = 0 to UBound(mTriggersArray) - 1
					for j = i + 1 to UBound(mTriggersArray)
						' Switch elements if necessary
						if mTriggersArray(i, 1) > mTriggersArray(j, 1) then
							mTemp(0) = mTriggersArray(i, 0)
							mTemp(1) = mTriggersArray(i, 1)
							mTemp(2) = mTriggersArray(i, 2)
							mTemp(3) = mTriggersArray(i, 3)
							
							mTriggersArray(i, 0) = mTriggersArray(j, 0)
							mTriggersArray(i, 1) = mTriggersArray(j, 1)
							mTriggersArray(i, 2) = mTriggersArray(j, 2)
							mTriggersArray(i, 3) = mTriggersArray(j, 3)
	
							mTriggersArray(j, 0) = mTemp(0)
							mTriggersArray(j, 1) = mTemp(1)
							mTriggersArray(j, 2) = mTemp(2)
							mTriggersArray(j, 3) = mTemp(3)						
						end if
					next 
				next
			end if		
			
			
			' Run Triggers
			if UBound(mTriggersArray) >= 0 then
				for i = 0 to UBound(mTriggersArray)
					i_triggerName = mTriggersArray(i, 0) 
					i_triggerCondition =  mTriggersArray(i, 3) 
					i_triggerParams =  mTriggersArray(i, 2) 
					
					run = tNG_DynamicData(i_triggerCondition, this, "expression", null, null, null)
					runTrigger = false
					On Error Resume Next
					Execute "runTrigger = " & run
					If err.Number <> 0 Then
						Response.write "Internal Error. Invalid boolean expression: " & run
						Response.End()
						On Error GoTo 0					
					End If
					On Error GoTo 0
					
					If runTrigger  Then
						tNG_log__log i_triggerType, i_triggerName, "begin"
						
						paramsCall = ""
						For idx_param = 0 to UBound(i_triggerParams)
							paramsCall = paramsCall & ", i_triggerParams(" &  idx_param & ")"
						Next
						Execute "Set ret = " & i_triggerName & "(this" & paramsCall & ")"
						
						If i_triggerType <> "ERROR" and i_triggerType <> "STARTER" Then
							If KT_isSet(ret) Then
								tNG_log__log "KT_ERROR", null, null
								tNG_log__log i_triggerType, i_triggerName, "end"
								Set executeTriggers = ret	
								Exit Function
							End If
						End If
						tNG_log__log i_triggerType, i_triggerName, "end"
					End If

				Next
			End if
		End If

		Set executeTriggers = Nothing
	End Function
	

	Public Function getTransactionType()
		getTransactionType = transactionType
	End Function
	
	Public Function isStarted()
		isStarted = started
	End Function

	Public Sub setStarted (started__param)
		started = started__param
	End Sub

	Public Sub setSQL (sql__param)
		sql = sql__param
	End Sub

	Public Function postExecuteSql()
		' To be overwritten
		Set postExecuteSql = nothing
	End Function
		
	Public Function parseSQLError (sql__param, errorMsg) 
		Dim sql
		sql = KT_escapeAttribute(sql__param)
		Set errObj =  new tNG_error
		errObj.Init "SQL_ERROR", array(errorMsg), array(sql)
		Set parseSQLError = errObj
	End Function

	Public Sub setError (ByRef errorObj__param) 
		Set errorObj = errorObj__param
	End Sub
	
	Public Function getError()
		If KT_isSet(errorObj) Then
			Set getError = errorObj
		Else
			Set getError = nothing
		End If
	End Function
	
	Public Function getErrorMsg()
		ret_warning = ""
		ret_user = ""
		ret_devel = ""
		Set errObj = this.getError()
		If KT_isSet(errObj) Then
			ret_user = errObj.getDetails()
			ret_devel = errObj.getDeveloperDetails()
		End If
		getErrorMsg = Array(ret_warning, ret_user, ret_devel)
	End Function	
	
		
	Public Function doTransaction() 
		tNG_log__log "tNG" & transactionType, "doTransaction", "begin"
		Dim ret, i
				
		'calling the before triggers and terminate execution if we had an error
		Set ret = this.executeTriggers("BEFORE")
		if KT_isSet(ret) Then
			this.setError ret
			this.executeTriggers "ERROR"
			tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
			doTransaction = false
			Exit Function
		End If

		'process the SQL for eventual auto-generation
		Set ret = this.prepareSQL()
		If KT_isSet(ret) Then
			tNG_log__log "KT_ERROR", null, null
			this.setError ret
			this.executeTriggers "ERROR"
			tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
			doTransaction = false
			Exit Function
		End If

		Set ret = this.getError()
		If KT_isSet(ret) Then
			this.executeTriggers "ERROR"
			tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
			doTransaction = false
			Exit Function
		End If

		' executing the transaction
		If KT_isSet(sql) Then
			tNG_log__log "tNG"  & transactionType, "executeTransaction", "execute sql"
			
			On Error resume next
			If not isArray(sql) Then
				Set transactionResult = connection.Execute(sql)
			Else
				For i=lbound(sql) to ubound(sql) 
					Set transactionResult = connection.Execute(sql(i))
				Next	
			End If
			
			' check if the transaction has been done OK
			If err.number <> 0 Then
				thrownSQLError = err.description
				On Error GoTo 0
				Set ret  = this.parseSQLError(SQL, thrownSQLError)
				this.setError ret
				tNG_log__log "KT_ERROR", null, null
				this.executeTriggers "ERROR"
				tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
				doTransaction = false
				Exit Function				
			End If
			On Error GoTo 0
		End If
		
		Set ret = this.postExecuteSql()
		If KT_isSet(ret) Then
			this.setError ret
			this.executeTriggers "ERROR"
			tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
			doTransaction = false
			Exit Function				
		End If


		If KT_isSet(transactionResult) Then
			If transactionResult.state  = 0 Then
				' closed recordset
				Set transactionResult = nothing
			Else
				' opened recordset
				' check for recordCount. If the driver does not support it, this method returns -1
				If transactionResult.recordCount = 0 Then
					Set transactionResult = nothing
				End If
			End If
		End If

		'calling the after triggers
		Set ret = this.executeTriggers("AFTER")
		if KT_isSet(ret) Then
			this.setError ret
			this.executeTriggers "ERROR"
			tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
			doTransaction = false
			Exit Function
		End If

		Set ret = this.executeTriggers("END")
		if KT_isSet(ret) Then
			this.setError ret
			this.executeTriggers "ERROR"
			tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
			doTransaction = false
			Exit Function
		End If

		tNG_log__log "tNG"  & transactionType, "doTransaction", "end"
		doTransaction  = true	
	End Function	
		
	Public Sub rollBackTransaction(ByRef errorObj__param)
		this.setError(errorObj)
		this.executeTriggers("ERROR")
	End Sub

End Class

%>