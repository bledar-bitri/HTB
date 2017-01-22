<%
Class TFI_TableFilter
	public columns
	Private filterName

	Private Sub Class_Initialize()
		Set columns = Server.CreateObject("Scripting.Dictionary")
	End Sub
	
	Private Sub Class_Terminate()

	End Sub
	
	
	Public Sub Init(connection__param, filterName__param)
		filterName = filterName__param
			
		Set connection = KT_GetPooledConnection(connection__param)
		KT_setDbType connection

		KT_SessionKtBack(KT_getFullUri())
	End Sub

	Public Sub addColumn (colName, colType, reference, compareType) 
		defaultValue  = ""
		' Developer 's note:
		' If you want to set a default value for a column, pass defaultValue as an argument to this
		' method, then call it from page with the extra parameter. Unfortunatelly you'll break DW SBs
		If Not columns.Exists(colName) Then
			Set columns(colName) = Server.CreateObject("Scripting.Dictionary")
		End If
		Set columns(colName)(reference) = Server.CreateObject("Scripting.Dictionary")
		columns(colName)(reference)("type") = colType
		columns(colName)(reference)("reference") = filterName & "_"  & reference
		columns(colName)(reference)("compareType") = compareType
		
		If defaultValue <> "" Then
			Set details = Server.CreateObject("Scripting.Dictionary")
			details("type") = colType
			details("method") = "VALUE"
			details("reference") = defaultValue
			tNG_prepareValues details
			defaultValue = details("value")
		End If
		
		If Session(filterName & "_" & reference) = "" Then
			Session(filterName & "_" & reference) = defaultValue
		End If
	End Sub
	
	
	Public Sub Execute()
		show_filter_reference = "show_filter_" & filterName
		reset_filter_reference = "reset_filter_" & filterName
		has_filter_reference = "has_filter_" & filterName
		filter_reference = "filter_" & filterName	
	

		Dim url
		url = Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			url = url & "?" & Request.ServerVariables("QUERY_STRING")
		End If
	
		If KT_isSet(Request.QueryString(show_filter_reference)) Then
			Session(has_filter_reference) = "1"
			url = KT_addReplaceParam(url, show_filter_reference, "")
			KT_redir(url)
		End If
		
		If KT_isSet(Request.QueryString(reset_filter_reference)) Then
			Session.Contents.Remove(reset_filter_reference)
			Session.Contents.Remove(has_filter_reference)
			Session.Contents.Remove(filter_reference)
			For each colName in columns
				For each ref in columns(colName)
					Set details = columns(colName)(ref)
					Session(details("reference")) = ""
				Next
			Next
			url = KT_addReplaceParam(url, reset_filter_reference, "")
			KT_redir(url)
		End If
		
		If Request.Form(filterName) <> "" Then
			For each columnName in columns
				For each ref in columns(columnName)
					Set details = columns(columnName)(ref)
					variableName = details("reference")
					If KT_isSet(Request.Form(variableName)) Then
						details("method") = "POST"
						If details("type") = "DATE_TYPE" OR details("type") = "DATE_ACCESS_TYPE" Then
							details("type") = "STRING_TYPE"
							tNG_prepareValues(details)
						Else
							tNG_prepareValues(details)
						End If
						Session(variableName) = details("value")
					Else
						Session(variableName) = ""	
					End If
				Next
			Next
			
			url = KT_addReplaceParam(url, "#pageNum_.*", "")
			KT_redir(url)
		End If
		
		
		condition = ""
		For each columnName in columns
			For each ref in columns(columnName)
				Set details = columns(columnName)(ref)
				variableName = details("reference")
				details("value") = Session(variableName)
				If details("value") <> "" Then
					If condition <> "" Then
						condition = condition & " AND "
					End If
					
					variableValue = trim(details("value"))
					compareType = details("compareType")
					Select Case True
						Case (details("type") = "NUMERIC_TYPE" OR details("type") = "DOUBLE_TYPE")
							' if decimal separator is , -> .
							variableValue = replace(variableValue, ",", ".")
							If KT_preg_test("^(<=|>=|<>|!=|<|>|=)\s?-?\d*\.?\d+$", variableValue) Then
								variableValue = replace(variableValue, "!=", "<>")
								condition = condition & KT_escapeFieldName(columnName) & variableValue
							Else
								condition = condition & KT_escapeFieldName(columnName) &  " " & compareType & " " & KT_escapeForSql(variableValue, details("type"))
							End If
						
						Case (details("type") = "CHECKBOX_1_0_TYPE" OR details("type") = "CHECKBOX_-1_0_TYPE")
							If KT_preg_test("^[<>]{1}\s?-?\d*\.?\d+$", variableValue) Then
								condition = condition & KT_escapeFieldName(columnName) & variableValue
							Else
								condition = condition & KT_escapeFieldName(columnName) &  " = " & KT_escapeForSql(variableValue, details("type"))
							End If							
						
						Case (details("type") = "DATE_TYPE" OR details("type") = "DATE_ACCESS_TYPE")
							localCond = prepareDateCondition(columnName, details)
							condition = condition & localCond
						Case Else
							Select Case compareType
								case "="
								
								case "A%"
									variableValue = variableValue & "%"
									compareType = "LIKE"
								case "%A"
									variableValue = "%" & variableValue
									compareType = "LIKE"
								case else
									variableValue = "%" & variableValue & "%"
									compareType = "LIKE"		
							End Select
							variableValue = KT_escapeForSql(variableValue, details("type"))
							condition = condition & KT_escapeFieldName(columnName) & " " & compareType & " " & variableValue
					End Select
				End If
			Next
		Next
		
		If condition = "" Then
			condition = "1=1"
		End If
		Session(filter_reference) = condition
	End Sub
	
	
	
	Public Function getShowFilterLink()
		show_filter_reference = "show_filter_" & filterName
		
		Dim url
		url = Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			url = url & "?" & Request.ServerVariables("QUERY_STRING")
		End If

		If KT_isSet(Request.QueryString(show_filter_reference)) Then
			url = KT_addReplaceParam(url, show_filter_reference, "")
		Else
			url = KT_addReplaceParam(url, show_filter_reference, "1")
		End If
		getShowFilterLink = url
	End Function
	

	Public Function getResetFilterLink()
		reset_filter_reference = "reset_filter_" & filterName
		
		Dim url
		url = Request.ServerVariables("URL")
		If Request.ServerVariables("QUERY_STRING") <> "" Then
			url = url & "?" & Request.ServerVariables("QUERY_STRING")
		End If

		If KT_isSet(Request.QueryString(reset_filter_reference)) Then
			url = KT_addReplaceParam(url, reset_filter_reference, "")
		Else
			url = KT_addReplaceParam(url, reset_filter_reference, "1")
			url = KT_addReplaceParam(url, "#pageNum_.*", "")
		End If
		getResetFilterLink = url
	End Function



	Private Function prepareDateCondition(columnName, ByRef arr)
		date_value = trim(arr("value"))
		If KT_preg_test("^(<=|>=|<>|!=|<|>|=).*", date_value) Then
			modifier = KT_preg_replace("^(<=|>=|<>|!=|<|>|=).*", "$1", date_value)
			date_value = trim(replace(date_value, modifier, ""))
			If modifier = "!=" Then
				modifier = "<>"
			End If
			If modifier = "=" Then
				modifier = ""
			End If
		End If
		
	
		c_year = ""
		c_month = ""
		c_day = ""
		c_hour = ""
		c_min = ""
		c_sec = ""

		condType = ""
		condition = ""
		

		' NN
		' NNNN 
		If KT_preg_test("^(\d{2}|\d{4})$", date_value) Then
			condType = "year"
			c_year = Cint(date_value)

			If len(date_value) = 2 Then
				If c_year < 70 Then
					c_year = 2000 + c_year
				Else
					c_year = 1900 + c_year
				End If
			End If	
		End If
		
		
		' NNNN/NN
		' NN/NN
		' NN/NNNN		
		If KT_preg_test("^(\d{2}|\d{4})[-\/\[\]\(\)\*\|\+\.=,]{1}(\d{2}|\d{4})$", date_value) Then
			condType = "year-month"
			
			dateArr = KT_preg_split("([-\/\[\]\(\)\*\|\+\.=,])", date_value, -1, "PREG_SPLIT_NO_EMPTY")
			If len(dateArr(0)) = 2 Then
				' MM/YYYY or MM/YY
				str_month = dateArr(0)
				str_year = dateArr(1)
			Else
				If len(dateArr(1)) = 2 Then
					' YYYY/MM
					str_year = dateArr(0)
					str_month = dateArr(1)
				Else
					' NNNN / NNNN is invalid
					condType = ""
				End If
			End If
			
			If condType <> "" Then
				c_year = Cint(str_year)
				c_month = Cint(str_month)

				If len(str_year) = 2 Then
					If c_year < 70 Then
						c_year = 2000 + c_year
					Else
						c_year = 1900 + c_year
					End If
				End If	
				If c_month < 1 Or c_month > 12 Then
					c_month = 1
				End If

				' normalize
				If c_month < 10 Then
					c_month = "0" & c_month
				End If
			End If
		End If
		
		' TIME validation
		If KT_preg_test("^\d+[-\/\[\]\(\)\*\|\+\.=,]{1}\d+[-\/\[\]\(\)\*\|\+\.=,]{1}\d+\s+\d+.*$", date_value) Then
			pos_space = instr(date_value, " ")
			time_value = trim(mid(date_value, pos_space+1))
			date_value = left(date_value, pos_space-1)

			
			If KT_preg_test("^(\d+)(:)?(\d*)(:)?(\d*)(:)?\s*(am|pm|a|p)?$", time_value) Then
				formated_time_value = KT_preg_replace("^(\d+)(:)?(\d*)(:)?(\d*)(:)?\s*(am|pm|a|p)?$", "$1:$3:$5#$7", time_value)
				time_part = left(formated_time_value, instr(formated_time_value, "#")-1)
				ampm_part = mid(formated_time_value, instr(formated_time_value, "#")+1)
				
				timeArr = split(time_part, ":")
				str_hour = timeArr(0)
				str_min  = timeArr(1)
				str_sec = timeArr(2)
				
				c_hour = 0
				c_min = 0
				c_sec = 0
				If str_hour <> "" And str_min = "" And str_sec = "" Then
					condType = "DATE hour"
					c_hour = Cint(str_hour)
				End If
				If str_hour <> "" And str_min <> "" And str_sec = "" Then
					condType = "DATE hour:min"
					c_hour = Cint(str_hour)
					c_min = Cint(str_min)
				End If
				If str_hour <> "" And str_min <> "" And str_sec <> "" Then
					condType = "DATE hour:min:sec"
					c_hour = Cint(str_hour)
					c_min = Cint(str_min)
					c_sec = Cint(str_sec)
				End If

				If (lcase(ampm_part) = "p" Or lcase(ampm_part) = "pm") And c_hour < 12 Then
					c_hour = 12 + c_hour
				End If
				
				' validate
				If c_hour > 23 Then
					c_hour = 0
				End If
				If c_min > 59 Then
					c_min = 0
				End If
				If c_sec > 59 Then
					c_sec = 0
				End If

				
				' normalize
				If c_hour < 10 Then
					c_hour = "0" & c_hour
				End If
				If c_min < 10 Then
					c_min = "0" & c_min
				End If
				If c_sec < 10 Then
					c_sec = "0" & c_sec
				End If
			End If
		End If


		' NNNN/NN/NN -> YYYY/MM/DD but in screen format
		If KT_preg_test("^\d+[-\/\[\]\(\)\*\|\+\.=,]{1}\d+[-\/\[\]\(\)\*\|\+\.=,]{1}\d+$", date_value) Then
			If condType = "" Then
				condType = "year-month-day"
			End If
			fixformat_date_value = KT_convertDate(date_value, KT_screen_date_format, "yyyy-mm-dd")
			dateArr = split(fixformat_date_value, "-")
			
			str_year = dateArr(0)
			str_month = dateArr(1)
			str_day = dateArr(2)
			
			c_year = Cint(str_year)
			c_month = Cint(str_month)
			c_day = Cint(str_day)
			
			' validation
			If len(str_year) <= 2 Then
				If c_year < 70 Then
					c_year = 2000 + c_year
				Else
					c_year = 1900 + c_year
				End If
			End If	
			If c_month < 1 Or c_month > 12 Then
				c_month  = 1
			End If
			maxday = KT_getDaysOfMonth(c_month, c_year)
			If c_day < 1 Or c_day > maxday Then
				c_day = 1
			End If
			
			' normalize
			If c_month < 10 Then
				c_month = "0" & c_month
			End If			
			If c_day < 10 Then
				c_day = "0" & c_day
			End If			
		End If

		
		KT_getInternalTimeFormat()

		' prepare filtering rules
		Select case condType
			Case "year"
				variableValue1 = KT_convertDate(c_year & "-01-01", "yyyy-mm-dd", KT_db_date_format)
				variableValue2 = KT_convertDate(c_year & "-12-31", "yyyy-mm-dd", KT_db_date_format)
				compareType1 = ">="
				compareType2 = "<="
				
			case "year-month"
				variableValue1 = KT_convertDate(c_year & "-" & c_month & "-01", "yyyy-mm-dd", KT_db_date_format)
				maxday = KT_getDaysOfMonth(Cint(c_month), Cint(c_year))
				variableValue2 = KT_convertDate(c_year & "-" & c_month & "-" & maxday, "yyyy-mm-dd", KT_db_date_format)
				compareType1 = ">="
				compareType2 = "<="
				
			case "year-month-day"
				variableValue1 = KT_convertDate(c_year & "-" & c_month & "-" & c_day &  " 00:00:00", "yyyy-mm-dd HH:ii:ss", KT_db_date_format & " " & KT_db_time_format_internal)
				variableValue2 = KT_convertDate(c_year & "-" & c_month & "-" & c_day &  " 23:59:59", "yyyy-mm-dd HH:ii:ss", KT_db_date_format & " " & KT_db_time_format_internal)
				compareType1 = ">="
				compareType2 = "<="

			case "DATE hour"
				variableValue1 = KT_convertDate(c_year & "-" & c_month & "-" & c_day &  " " & c_hour & ":00:00", "yyyy-mm-dd HH:ii:ss", KT_db_date_format & " " & KT_db_time_format_internal)
				variableValue2 = KT_convertDate(c_year & "-" & c_month & "-" & c_day &  " " & c_hour & ":59:59", "yyyy-mm-dd HH:ii:ss", KT_db_date_format & " " & KT_db_time_format_internal)
				compareType1 = ">="
				compareType2 = "<="
				
			case "DATE hour:min"
				variableValue1 = KT_convertDate(c_year & "-" & c_month & "-" & c_day &  " " & c_hour & ":" & c_min & ":00", "yyyy-mm-dd HH:ii:ss", KT_db_date_format & " " & KT_db_time_format_internal)
				variableValue2 = KT_convertDate(c_year & "-" & c_month & "-" & c_day &  " " & c_hour & ":" & c_min & ":59", "yyyy-mm-dd HH:ii:ss", KT_db_date_format & " " & KT_db_time_format_internal)
				compareType1 = ">="
				compareType2 = "<="
				
			case "DATE hour:min:sec"
				variableValue1 = KT_convertDate(c_year & "-" & c_month & "-" & c_day &  " " & c_hour & ":" & c_min & ":" & c_sec, "yyyy-mm-dd HH:ii:ss", KT_db_date_format & " " & KT_db_time_format_internal)
				variableValue2 = ""
				compareType1 = "="
				compareType2 = ""
		End Select
		
		
		
		' prepare filter when modifiers are used
		operator = "AND"
		If modifier <> "" Then
			If modifier = "<>" Then
				operator = "OR"
				If variableValue2 = "" Then
					' single full date, so keep only the modifier, and use a single date-time comparation
					compareType1 = modifier
				Else
					' two dates,  use the interval
					compareType1 = "<"
					compareType2 = ">"
				End If
			Else
				compareType1 = modifier
				If (modifier = "<=" Or modifier = ">") And variableValue2 <> "" Then
					variableValue1 = variableValue2
				End If
				variableValue2 = ""
				compareType2 = ""
			End If
		End If
		
		
		If condType <> "" Then
			condition = "("
			condition  = condition & KT_escapeFieldName(columnName) & " " & compareType1 & " " & KT_escapeForSql(variableValue1, arr("type"))
			If variableValue2 <> "" Then
				condition  = condition & " " & operator & " " & KT_escapeFieldName(columnName) & " " & compareType2 & " " & KT_escapeForSql(variableValue2, arr("type"))
			End If
			condition = condition & ")"
		End If

		prepareDateCondition  = condition
	End Function

End Class
%>