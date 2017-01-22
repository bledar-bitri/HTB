<%
Class tNG_FormValidation
	Public tNG
	Public columns
	Public validationRules
	Public genericValidationMessages
	Public mustValidate

	Private Sub Class_Initialize()
		Set tNG = nothing
		Set columns = Server.CreateObject("Scripting.Dictionary")
		Set validationRules = Server.CreateObject("Scripting.Dictionary")
		Set genericValidationMessages = Server.CreateObject("Scripting.Dictionary")
		mustValidate = false		
	End Sub

	Private Sub Class_Terminate()
	End Sub

	'************ START SPECIFIC ********************		
	Public Sub Init()
		loadValidationRules
		loadGenericValidationMessages
		
		KT_getInternalTimeFormat
	End Sub

	Public Sub setTransaction(ByRef tNG__param)
		Set tNG = tNG__param
		mustValidate = true
	End Sub

	Public Sub addField (fieldName, required, validationType, format, min, max, errorMessage)
		validationType = lcase(validationType)
		Set field = Server.CreateObject("Scripting.Dictionary")
		field("name") = fieldName
		field("required") = required
		field("type") = validationType
		If validationType = "mask" Then
			field("format") = ""
			field("additional_params") = format	
		ElseIf validationType = "regexp" Then
			field("format") = ""
			If left(format, 1) <> "/" Then
				format = "/" & format & "/im"
			End If
			field("additional_params") = format
		Else
			field("format") = lcase(format)
			additionals = ""
			If validationRules.Exists(validationType) Then
				If validationRules(validationType).Exists(format) Then
					additionals = validationRules(validationType)(format)("regexp")
				End If
			End If	
			field("additional_params") = additionals
		End If

		Select case field("format") 
			case "date"
				field("additional_params") = KT_screen_date_format
				field("date_screen_format") = KT_screen_date_format
			case "time"
				field("additional_params") = KT_screen_time_format_internal
				field("date_screen_format") = KT_screen_time_format
			case "datetime"
				field("additional_params") = KT_screen_date_format &  " " & KT_screen_time_format_internal
				field("date_screen_format") = KT_screen_date_format & " " & KT_screen_time_format
		End select
	
		min1 = min
		max1 = max
		
		Set min_placeholders = tNG_getReplacementsFromMessage(min1)
		If min_placeholders.Count > 0 Then
			min1 = ""
		End If
		Set max1_placeholders = tNG_getReplacementsFromMessage(max1)
		If max1_placeholders.Count > 0 Then
			max1 = ""
		End If

		' min_cs and max_cs are used for client side validation
		field("min_cs") = min1
		field("max_cs") = max1
		
		min = tNG_DynamicData(min, nothing, null, null, null, null)
		max = tNG_DynamicData(max, nothing, null, null, null, null)
		field("min") = min
		field("max") = max
		field("message") = errorMessage
		Set columns(fieldName) = field
	End Sub


	Private Sub loadValidationRules()
		Set validationRules("text") = Server.CreateObject("Scripting.Dictionary")
		Set validationRules("text")("") = Server.CreateObject("Scripting.Dictionary")
		Set validationRules("text")("email") = Server.CreateObject("Scripting.Dictionary")		
			validationRules("text")("email")("regexp") = "/^.+@.+\..+$/i"
		Set validationRules("text")("cc_generic") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("cc_generic")("regexp") = "/^[3-6]{1}[0-9]{12,15}$/"
			validationRules("text")("cc_generic")("callback") = "validate_cc"
		Set validationRules("text")("cc_visa") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("cc_visa")("regexp") = "/^4[0-9]{12,15}$/"
			validationRules("text")("cc_visa")("callback") = "validate_cc"
		Set validationRules("text")("cc_mastercard") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("cc_mastercard")("regexp") = "/^5[1-5]{1}[0-9]{14}$/"
			validationRules("text")("cc_mastercard")("callback") = "validate_cc"
		Set validationRules("text")("cc_americanexpress") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("cc_americanexpress")("regexp") = "/^3(4|7){1}[0-9]{13}$/"
			validationRules("text")("cc_americanexpress")("callback") = "validate_cc"
		Set validationRules("text")("cc_discover") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("cc_discover")("regexp") = "/^6011[0-9]{12}$/"
			validationRules("text")("cc_discover")("callback") = "validate_cc"
		Set validationRules("text")("cc_dinersclub") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("cc_dinersclub")("regexp") = "/^3((0[0-5]{1}[0-9]{11})|(6[0-9]{12})|(8[0-9]{12}))$/"
			validationRules("text")("cc_dinersclub")("callback") = "validate_cc"
		Set validationRules("text")("zip_generic") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("zip_generic")("regexp") = "/^\d+$/"
		Set validationRules("text")("zip_us5") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("zip_us5")("regexp") = "/^\d{5}$/"
		Set validationRules("text")("zip_us9") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("zip_us9")("regexp") = "/^\d{5}-\d{4}$/"
		Set validationRules("text")("zip_canada") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("zip_canada")("regexp") = "/^[A-Z]{1}\d[A-Z]{1}\s?\d[A-Z]{1}\d$/i"
		Set validationRules("text")("zip_uk") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("zip_uk")("regexp") = "/^[A-Z]{1,2}\d[\dA-Z]?\s?\d[A-Z]{2}$/i"
		Set validationRules("text")("phone") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("phone")("regexp") = "/^[(]?[+]{0,2}[0-9-.\s\/()]+$/"
		Set validationRules("text")("ssn") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("ssn")("regexp") = "/^\d{3}\s?\d{2}\s?\d{4}$/"
		Set validationRules("text")("url") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("url")("regexp") = "/^http(s)?:\/\/([0-9a-z-_]+\.)+[a-z]{2,4}(\/|\\)?.*$/i"
		Set validationRules("text")("ip") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("ip")("regexp") = "/^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}$/"
			validationRules("text")("ip")("callback") = "validate_ip"
		Set validationRules("text")("color_generic") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("color_generic")("callback") = "validate_color"
		Set validationRules("text")("color_hex") = Server.CreateObject("Scripting.Dictionary")
			validationRules("text")("color_hex")("regexp") = "/^#[0-9a-f]{6}$/i"

		Set validationRules("numeric") = Server.CreateObject("Scripting.Dictionary")
		Set validationRules("numeric")("") = Server.CreateObject("Scripting.Dictionary")
		Set validationRules("numeric")("int") = Server.CreateObject("Scripting.Dictionary")
			validationRules("numeric")("int")("regexp") = "/^-?\d+$/"
		Set validationRules("numeric")("int_positive") = Server.CreateObject("Scripting.Dictionary")
			validationRules("numeric")("int_positive")("regexp") = "/^\d+$/"
			validationRules("numeric")("int_positive")("callback") = "validate_positive"
		Set validationRules("numeric")("zip_generic") = Server.CreateObject("Scripting.Dictionary")
			validationRules("numeric")("zip_generic")("regexp") = "/^\d+$/"

			
			
		Set validationRules("double") = Server.CreateObject("Scripting.Dictionary")
		Set validationRules("double")("float") = Server.CreateObject("Scripting.Dictionary")
			validationRules("double")("float")("regexp") = "/^-?[0-9]*(\.|,)?[0-9]+([eE]\-[0-9]+)?$/"
		Set validationRules("double")("float_positive") = Server.CreateObject("Scripting.Dictionary")
			validationRules("double")("float_positive")("regexp") = "/^[0-9]*(\.|,)?[0-9]+([eE]\-[0-9]+)?$/"
			validationRules("double")("float_positive")("callback") = "validate_positive"

		Set validationRules("date") = Server.CreateObject("Scripting.Dictionary")
		Set	validationRules("date")("date") = Server.CreateObject("Scripting.Dictionary")
		Set	validationRules("date")("datetime") = Server.CreateObject("Scripting.Dictionary")
		Set	validationRules("date")("time") = Server.CreateObject("Scripting.Dictionary")

		Set validationRules("mask") = Server.CreateObject("Scripting.Dictionary")
		Set	validationRules("regexp")= Server.CreateObject("Scripting.Dictionary")
	End Sub

	Private Sub loadGenericValidationMessages()
		d = "tNG_FormValidation"
		
		genericValidationMessages("failed") = KT_getResource("FAILED", d, null)
		
		genericValidationMessages("required") = KT_getResource("REQUIRED", d, null)
		genericValidationMessages("type") = KT_getResource("TYPE", d, null)
		genericValidationMessages("format") = KT_getResource("FORMAT", d, null)
		
		genericValidationMessages("text_") = KT_getResource("TEXT_", d, null)
		genericValidationMessages("text_email") = KT_getResource("TEXT_EMAIL", d, null)
		genericValidationMessages("text_cc_generic") = KT_getResource("TEXT_CC_GENERIC", d, null)
		genericValidationMessages("text_cc_visa") = KT_getResource("TEXT_CC_VISA", d, null)
		genericValidationMessages("text_cc_mastercard") = KT_getResource("TEXT_CC_MASTERCARD", d, null)
		genericValidationMessages("text_cc_americanexpress") = KT_getResource("TEXT_CC_AMERICANEXPRESS", d, null)
		genericValidationMessages("text_cc_discover") = KT_getResource("TEXT_CC_DISCOVER", d, null)
		genericValidationMessages("text_cc_dinersclub") = KT_getResource("TEXT_CC_DINERSCLUB", d, null)
		genericValidationMessages("text_zip_generic") = KT_getResource("TEXT_ZIP_GENERIC", d, null)
		genericValidationMessages("text_zip_us5") = KT_getResource("TEXT_ZIP_US5", d, null)
		genericValidationMessages("text_zip_us9") = KT_getResource("TEXT_ZIP_US9", d, null)
		genericValidationMessages("text_zip_canada") = KT_getResource("TEXT_ZIP_CANADA", d, null)
		genericValidationMessages("text_zip_uk") = KT_getResource("TEXT_ZIP_UK", d, null)
		genericValidationMessages("text_phone") = KT_getResource("TEXT_PHONE", d, null)
		genericValidationMessages("text_ssn") = KT_getResource("TEXT_SSN", d, null)
		genericValidationMessages("text_url") = KT_getResource("TEXT_URL", d, null)
		genericValidationMessages("text_ip") = KT_getResource("TEXT_IP", d, null)
		genericValidationMessages("text_color_hex") = KT_getResource("TEXT_COLOR_HEX", d, null)
		genericValidationMessages("text_color_generic") = KT_getResource("TEXT_COLOR_GENERIC", d, null)
		
		genericValidationMessages("numeric_") = KT_getResource("NUMERIC_", d, null)
		genericValidationMessages("numeric_int") = KT_getResource("NUMERIC_INT", d, null)
		genericValidationMessages("numeric_int_positive") = KT_getResource("NUMERIC_INT_POSITIVE", d, null)
		genericValidationMessages("numeric_zip_generic") = KT_getResource("TEXT_ZIP_GENERIC", d, null)
		
		genericValidationMessages("double_") = KT_getResource("DOUBLE_", d, null)
		genericValidationMessages("double_float") = KT_getResource("DOUBLE_FLOAT", d, null)
		genericValidationMessages("double_float_positive") = KT_getResource("DOUBLE_FLOAT_POSITIVE", d, null)
		
		genericValidationMessages("date_") = KT_getResource("DATE_", d, null)
		genericValidationMessages("date_date") = KT_getResource("DATE_DATE", d, null)
		genericValidationMessages("date_time") = KT_getResource("DATE_TIME", d, null)
		genericValidationMessages("date_datetime") = KT_getResource("DATE_DATETIME", d, null)
		
		genericValidationMessages("mask_") = KT_getResource("MASK_", d, null)
		
		genericValidationMessages("regexp_") = KT_getResource("REGEXP_", d, null)
		genericValidationMessages("regexp_failed") = KT_getResource("REGEXP_FAILED", d, null)
		
		genericValidationMessages("text_min") = KT_getResource("TEXT_MIN", d, null)
		genericValidationMessages("text_max") =  KT_getResource("TEXT_MAX", d, null) 
		genericValidationMessages("text_between") = KT_getResource("TEXT_BETWEEN", d, null) 
		
		genericValidationMessages("other_min") = KT_getResource("OTHER_MIN", d, null)
		genericValidationMessages("other_max") = KT_getResource("OTHER_MAX", d, null)
		genericValidationMessages("other_between") = KT_getResource("OTHER_BETWEEN", d, null)
	End Sub


	Private Function mask2regexp(in_txt)
		txt = in_txt
		txt = KT_preg_replace("([-\/\[\]()\*\+])", "\$1", txt)
		txt = KT_preg_quote(txt)
		txt = replace(txt, "\?", "?")
		txt = replace(txt, "?", ".")
		txt = replace(txt, "9", "\d")
		txt = replace(txt, "X", "\w")
		txt = replace(txt, "A", "[A-Za-z]")
		txt = "/^" & txt & "$/"
		mask2regexp = txt
	End Function
	


	Public Function Execute()	
		failed = false
		Set errObj = new tNG_error
		errObj.Init "", array(), array()
		If mustValidate And columns.Count > 0 Then
			columnKeys = KT_array_keys(columns)
			Dim i
			For i = 0 to ubound(columnKeys)
				doRequiredVal = true
				colIdx = columnKeys(i)			

				Set tNGcols = tNG.columns
				If tNGcols.Exists(columns(colIdx)("name")) Then
					' VALIDATE ONLY IF FIELD EXISTS IN TRANSACTION
					
					' on update don't require FILE_TYPE and tNG password fields
					If tNG.getTransactionType() = "_update" Or tNG.getTransactionType() = "_multipleUpdate" Then
						If tNG.getColumnType(columns(colIdx)("name")) = "FILE_TYPE" Then
							doRequiredVal = false
						End If
						
						If tNG.getTable() = tNG_login_config("table") And columns(colIdx)("name") = tNG_login_config("password_field") Then
							doRequiredVal = false
						End If		
					End If

					
					hasRequiredError = false
					hasTypeError = false
					
					tmpFieldValue = tNG.getColumnValue(columns(colIdx)("name")) & ""
					
					If columns(colIdx)("type") = "date" And columns(colIdx)("format") <> "" Then
						If Not KT_in_array(tNG.getColumnType(columns(colIdx)("name")), Array("DATE_TYPE", "DATE_ACCESS_TYPE"), True) Then
							tmpFieldValue = KT_formatDate2DB(tmpFieldValue)
						End If
					End If
					
					columns(colIdx)("failed")  = false
			
					' required parameter validation
					colCustomMsg  = columns(colIdx)("message")
					If doRequiredVal And columns(colIdx)("required") Then
						If len(colCustomMsg) = 0 Then
							colCustomMsg = genericValidationMessages("required")
						End If
						If tmpFieldValue = "" Then
							failed = true
							hasRequiredError = true
							columns(colIdx)("failed") = true
							If tNG.exportsRecordset() = false Then
								usd = false
								If tNG.transactionType = "_delete" Then
									usd = true
								End If
								colCustomMsg = tNG_DynamicData(colCustomMsg, tNG, null, usd, null, null)
								errObj.addDetails "%s", array(colCustomMsg), array(colCustomMsg)	
							Else
								errObj.setFieldError columns(colIdx)("name"), "%s", array(colCustomMsg)
							End If
							
						End If
					End If		
			
			
					' type and format validation
					colCustomMsg = columns(colIdx)("message")
					If tmpFieldValue <> "" and columns(colIdx)("type") <> "" Then
						If len(colCustomMsg) = 0 Then
							colCustomMsgBefore = genericValidationMessages("format")
							colCustomMsgAfter = genericValidationMessages(columns(colIdx)("type") & "_" & columns(colIdx)("format"))
							colCustomMsg = KT_sprintf(colCustomMsgBefore, Array(colCustomMsgAfter))
						End If
						tmpFieldValue = left(tmpFieldValue, 400)
						
						validation_type = columns(colIdx)("type")
						SELECT CASE True
						CASE (validation_type = "regexp")
							res = KT_preg_test(columns(colIdx)("additional_params"), tmpFieldValue)
							If res = false Then
								hasTypeError = true
								colCustomMsgBefore = genericValidationMessages("format")
								colCustomMsgAfter = genericValidationMessages("regexp_failed")
								colCustomMsg = KT_sprintf (colCustomMsgBefore, Array(colCustomMsgAfter))
							End If
							' DIFFERENT ://  res == 0 ?? 
						
						CASE (validation_type = "mask")
							myRegexp = mask2regexp(columns(colIdx)("additional_params"))
							myRegexp = KT_VBRegexp(myRegexp)
							If Not KT_preg_test(myRegexp, tmpFieldValue) Then
								hasTypeError = true
							End If
						
						CASE (validation_type = "text" Or validation_type = "numeric" Or validation_type="double")
							colType = columns(colIdx)("type")
							format = columns(colIdx)("format")
			
							If isObject(validationRules(colType)(format)) Then
								Set myValidationRule = validationRules(colType)(format)
								If myValidationRule.Exists("mask") Then
									myRegexp = mask2regexp(myValidationRule("mask"))
									myValidationRule("regexp") = myRegexp
								End If
								
								If myValidationRule.Exists("regexp") Then
									myRegexp = KT_VBRegexp(myValidationRule("regexp"))
									If Not KT_preg_test(myRegexp, tmpFieldValue) Then
										hasTypeError = true
									End If
								End If
								
								If Not hasTypeError Then
									If myValidationRule.Exists("callback") Then
										exec_string = "ret = Me." & myValidationRule("callback") & "(""" & tmpFieldValue & """)"
										ExecuteGlobal exec_string
										If Not ret Then
											hasTypeError = true
										End If
									End If
								End If	
							End If
			
			
						CASE (validation_type = "date")
							format = columns(colIdx)("format")
							If format <> "" Then
								checkFullDateTime = True
								Select Case format
									case "date"
										Set inFmtRule = KT_format2rule(KT_db_date_format)
										checkFullDateTime = True
									case "time"
										Set inFmtRule = KT_format2rule(KT_db_time_format_internal)
										checkFullDateTime = False
									case "datetime"
										Set inFmtRule = KT_format2rule(KT_db_date_format & " " & KT_db_time_format_internal)
										checkFullDateTime = True									
								End Select
								Set dateArr = KT_applyDate2rule(tmpFieldValue, inFmtRule)
								ret = KT_isValidDate(dateArr, checkFullDateTime)
								If Not ret Then
									hasTypeError = true
		
									' DIFFERENT FROM PHP VERSION :// restore the unformatted value in the tNG.columns
									Set cols = tNG.columns
									Set colDetails = cols(columns(colIdx)("name"))
									colRealValue = tNG_getRealValue(colDetails("method"), colDetails("reference"))
									colDetails("value") = colRealValue
								End If
							End If	
						END SELECT
					End If	
						
					If (Not hasRequiredError) And hasTypeError Then
						columns(colIdx)("failed") = true
						failed = true
						
						If tNG.exportsRecordset() = false Then
							usd = false
							If tNG.transactionType = "_delete" Then
								usd = true
							End If
							colCustomMsg = tNG_DynamicData(colCustomMsg, tNG, null, usd, null, null)
							errObj.addDetails "%s", array(colCustomMsg), array(colCustomMsg)	
						Else
							errObj.setFieldError columns(colIdx)("name"), "%s", array(colCustomMsg)
						End If						
						
					End If
				End If ' if validate	
			Next
		
		
			' MIN MAX parameter validation
			For i = 0 to ubound(columnKeys)
				colIdx = columnKeys(i)			

				Set tNGcols = tNG.columns
				If tNGcols.Exists(columns(colIdx)("name")) Then
					' VALIDATE ONLY IF FIELD EXISTS IN TRANSACTION
			
					If Not columns(colIdx)("failed") Then	
						hasMinMaxError = false
						tmpFieldValue = tNG.getColumnValue(columns(colIdx)("name")) & ""
						
						If columns(colIdx)("type") = "date" And columns(colIdx)("format") <> "" Then
							If Not KT_in_array(tNG.getColumnType(columns(colIdx)("name")), Array("DATE_TYPE", "DATE_ACCESS_TYPE"), True) Then
								tmpFieldValue = KT_formatDate2DB(tmpFieldValue)
							End If
						End If
						
						Set tNG_tNGfield_min = Server.CreateObject("Scripting.Dictionary")
						Set tNG_tNGfield_max = Server.CreateObject("Scripting.Dictionary")
				
						min = columns(colIdx)("min")
						Set min_placeholders = tNG_getReplacementsFromMessage(min)
						For each index in min_placeholders
							placeholder = min_placeholders(index)
							If instr(placeholder, ".") = 0 Then
								pos = tNG_tNGfield_min.Count
								tNG_tNGfield_min(pos) = placeholder
							End If
						Next
				
						max = columns(colIdx)("max")
						Set max_placeholders = tNG_getReplacementsFromMessage(max)
						For each index in max_placeholders
							placeholder = mxn_placeholders(index)
							If instr(placeholder, ".") = 0 Then
								pos = tNG_tNGfield_max.Count
								tNG_tNGfield_max(pos) = placeholder
							End If
						Next		
						min = tNG_DynamicData(min, tNG, null, null, null, null)
						max = tNG_DynamicData(max, tNG, null, null, null, null)
				
						' MIN parameter validation
						If tmpFieldValue <> "" And min <> "" Then
							If columns(colIdx)("type") = "text" Then
								If len(tmpFieldValue) < Cint(min) Then
									hasMinMaxError = true
								End If
							End If
							
							If columns(colIdx)("type") = "numeric" Or columns(colIdx)("type") = "double" Then
								evaluateNumeric = true
								For each index in tNG_tNGfield_min 
									tNG_tNGfield = tNG_tNGfield_min(index)
									If 	Not columns.Exists(tNG_tNGfield) Then
										evaluateNumeric = false
										Exit For
									End If
									If	columns(tNG_tNGfield)("type") <> "numeric" Or columns(tNG_tNGfield)("type") <> "double"  Or _
										columns(tNG_tNGfield)("format") = "" Or _
										columns(colIdx)("failed") Then
										
										evaluateNumeric = false
										Exit For
									End If
								Next
								tmpFieldValue = replace(tmpFieldValue, ",", ".")
								min = replace(min, ",", ".")
								If evaluateNumeric Then
									min = tNG.evaluateNumeric(min)
								End If
								If isNull(min) Then
									hasMinMaxError = true
								Else
									If Cdbl(tmpFieldValue) < Cdbl(min) Then
										hasMinMaxError = true
									End If
								End If
							End If
							
							If columns(colIdx)("type") = "date" Then
								For each index in tNG_tNGfield_min
									tNG_tNGfield = tNG_tNGfield_min(index)
									If KT_in_array(tNG.getColumnType(tNG_tNGfield), Array("DATE_TYPE", "DATE_ACCESS_TYPE"), True) Then
										min = KT_formatDate(min)
										Exit For
									End If
								Next
								minDate = KT_formatDate2DB(min)
								format = columns(colIdx)("format")
								checkFullDateTime = True
								Select case format
									Case "date"
										Set inFmtRule = KT_format2rule(KT_db_date_format)
										checkFullDateTime = True
									Case "time"
										Set inFmtRule = KT_format2rule(KT_db_time_format_internal)
										checkFullDateTime = False
									Case "datetime"
										Set inFmtRule = KT_format2rule(KT_db_date_format & " " & KT_db_time_format_internal)
										checkFullDateTime = True
								End Select 
								Set dateArr = KT_applyDate2rule (tmpFieldValue, inFmtRule)
								Set minArr = KT_applyDate2rule (minDate, inFmtRule)
		
								If KT_isValidDate(minArr, checkFullDateTime) Then
									If KT_compareDates(dateArr, minArr) = 1 Then
										hasMinMaxError = true
									End If
								End If
							End If
						End If
				
						' MAX parameter validation
						If tmpFieldValue <> "" And max <> "" Then
							If columns(colIdx)("type") = "text" Then
								If len(tmpFieldValue) > Cint(max) Then
									hasMinMaxError = true
								End If
							End If
							
							If columns(colIdx)("type") = "numeric" Or columns(colIdx)("type") = "double" Then
								evaluateNumeric = true
								For each index in tNG_tNGfield_max
									tNG_tNGfield = tNG_tNGfield_max(index)
									If 	Not columns.Exists(tNG_tNGfield) Then
										evaluateNumeric = false
										Exit For
									End If
									If	columns(tNG_tNGfield)("type") <> "numeric" Or columns(tNG_tNGfield)("type") <> "double"  Or _
										columns(tNG_tNGfield)("format") = "" Or _
										columns(colIdx)("failed") Then
										
										evaluateNumeric = false
										Exit For
									End If
								Next
								tmpFieldValue = replace(tmpFieldValue, ",", ".")
								max = replace(max, ",", ".")
								If evaluateNumeric Then
									max = tNG.evaluateNumeric(max)
								End If
								If isNull(max) Then
									hasMinMaxError = true
								Else
									If Cdbl(tmpFieldValue) > Cdbl(max) Then
										hasMinMaxError = true
									End If
								End If
							End If
							
							If columns(colIdx)("type") = "date" Then
								For each index in tNG_tNGfield_max
									tNG_tNGfield = tNG_tNGfield_max(index)
									If KT_in_array(tNG.getColumnType(tNG_tNGfield), Array("DATE_TYPE", "DATE_ACCESS_TYPE"), True) Then
										max = KT_formatDate(max)
										Exit For
									End If
								Next
								maxDate = KT_formatDate2DB(max)
								format = columns(colIdx)("format")
								checkFullDateTime = True
								Select case format
									Case "date"
										Set inFmtRule = KT_format2rule(KT_db_date_format)
										checkFullDateTime = True
									Case "time"
										Set inFmtRule = KT_format2rule(KT_db_time_format_internal)
										checkFullDateTime = False
									Case "datetime"
										SetinFmtRule = KT_format2rule(KT_db_date_format & " " & KT_db_time_format_internal)
										checkFullDateTime = True
								End Select 
								Set dateArr = KT_applyDate2rule (tmpFieldValue, inFmtRule)
								Set maxArr = KT_applyDate2rule (maxDate, inFmtRule)
								If KT_isValidDate(maxArr, checkFullDateTime) Then
									If KT_compareDates(dateArr, maxArr) = -1 Then
										hasMinMaxError = true
									End If
								End If
							End If
						End If
				
						If hasMinMaxError Then
							colCustomMsg = columns(colIdx)("message")
							If len(colCustomMsg) = 0 Then
								If columns(colIdx)("type") = "text" Then
									colCustomMsgBefore = "text"
								Else
									colCustomMsgBefore = "other"
								End If
								
								If min <> "" And max <> "" Then
									colCustomMsgAfter = "between"
									colCustomMsg = genericValidationMessages(colCustomMsgBefore & "_" & colCustomMsgAfter)
									colCustomMsg = KT_sprintf(colCustomMsg, array(min, max))
								ElseIf min <> "" Then
									colCustomMsgAfter = "min"
									colCustomMsg = genericValidationMessages(colCustomMsgBefore & "_" & colCustomMsgAfter)
									colCustomMsg = KT_sprintf(colCustomMsg, array(min))			
								Else
									colCustomMsgAfter = "max"
									colCustomMsg = genericValidationMessages(colCustomMsgBefore & "_" & colCustomMsgAfter)
									colCustomMsg = KT_sprintf(colCustomMsg, array(max))			
								End If
							End If
				
							columns(colIdx)("failed") = true
							failed = true
							If tNG.exportsRecordset() = false Then
								usd = false
								If tNG.transactionType = "_delete" Then
									usd = true
								End If
								colCustomMsg = tNG_DynamicData(colCustomMsg, tNG, null, usd, null, null)
								errObj.addDetails "%s", array(colCustomMsg), array(colCustomMsg)	
							Else
								errObj.setFieldError columns(colIdx)("name"), "%s", array(colCustomMsg)
							End If							
						End If
					End If
				End If ' if validate min max		
			Next	
		End If
		
		If Not failed Then
			Set Execute = nothing
		Else
			If tNG.exportsRecordset() = true Then
				errObj.addDetails "%s", array(genericValidationMessages("failed")), array("")	
			End If
			Set Execute = errObj
		End If
	End Function



	Public Function validate_positive(value)
		value = replace(value, ",", ".")
		If Cdbl(value) >= 0 Then
			validate_positive = true
			Exit Function
		End If
		validate_positive = false
	End Function

	Public Function validate_ip(value)
		pieces = split(value, ".")
		If ubound(pieces) <> 3 Then
			validate_ip = false
			Exit Function
		End If
		Dim i
		For i = 0 to ubound(pieces)
			piece = Cint(pieces(i))
			If piece > 255 Then
				validate_ip = false
				Exit Function
			End If
		Next
		validate_ip = true
	End Function

	Public Function validate_color(value)
		colors = array( _
			"black", _
			"green", _
			"silver", _
			"lime", _
			"gray", _
			"olive", _
			"white", _
			"yellow", _
			"maroon", _
			"navy", _
			"red", _
			"blue", _
			"purple", _
			"teal", _
			"fuchsia", _
			"aqua")
		If not KT_in_array(value, colors, false) Then
			validate_color = false
			Exit Function
		End If
		validate_color = true
	End Function

	Public Function validate_cc(value)
		digits = array()
		
		Dim i, j 
		j = 1
		For i = len(value) to 1 step -1
			If (j mod 2) = 0 Then
				digit = Cint (mid(value, i, 1)) * 2
				digits = KT_array_push(digits, Cint(left(digit, 1)))
				If len(digit) = 2 Then
					digits = KT_array_push(digits, Cint(mid(digit, 2, 1)))
				End If
			Else
				digit = mid(value, i, 1)
				digits = KT_array_push(digits, Cint(digit))
			End If
			j = j + 1
		Next
		sum = 0
		For i = 0 to ubound(digits)
			sum = sum + digits(i)
		Next
		If (sum mod 10) = 0 Then
			validate_cc = true
			Exit Function
		End If
		validate_cc = false
	End Function

End Class
%>