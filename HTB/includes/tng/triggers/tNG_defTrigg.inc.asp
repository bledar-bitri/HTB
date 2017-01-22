<%
'
'	Copyright (c) InterAKT Online 2000-2005
'

'
' Default Starter trigger 
' Verifies if additional parameters are set and if not invalidate the transaction
' this is usefull for verifying some global variables.
' Type: STARTER
'

Function Trigger_Default_Starter(ByRef tNG, method, reference)
	ret = tNG_getRealValue(method, reference)
	If KT_isSet(ret) Then
		tNG.setStarted True
	End If
	Set Trigger_Default_Starter = nothing
End Function


'
' Default Redirect trigger 
' Type: ERROR
'
Function Trigger_Default_Redirect (ByRef tNG, page) 
	Dim redObj: Set redObj = new tNG_Redirect
	redObj.Init tNG
	redObj.setURL page
	redObj.setKeepURLParams false
	Set Trigger_Default_Redirect = redObj.Execute()	
End Function

'
' Default Form Validation trigger
' Type: BEFORE
'
Function Trigger_Default_FormValidation (ByRef tNG, ByRef uniVal) 
	uniVal.setTransaction tNG
	Set Trigger_Default_FormValidation = uniVal.Execute()
End Function



'
' Default Insert RollBack trigger
' Type: ERROR
 '
Function Trigger_Default_Insert_RollBack (ByRef tNG) 
	keyName = tNG.getPrimaryKey()
	keyValue = tNG.getPrimaryKeyValue()
	keyType = tNG.getColumnType(keyName)
	escapedKeyValue = KT_escapeForSql(keyValue, keyType)
	
	sql = "DELETE FROM " & tNG.getTable() & " WHERE " & KT_escapeFieldName(keyName) & " = " &  escapedKeyValue
	On Error Resume Next
	tNG.connection.Execute(sql)
	On Error GoTo 0
	Set Trigger_Default_Insert_RollBack = nothing
End Function



'
' Default RollBack trigger
' Type: ERROR
'
Function Trigger_Default_RollBack (ByRef tNG, ByRef obj) 
	obj.RollBack
	Set Trigger_Default_RollBack = nothing
End Function


'
' Saves the SQL data to be altered in a local variable (savedData)
'
Function Trigger_Default_saveData (ByRef tNG) 
	Set Trigger_Default_saveData = tNG.saveData()
End Function



' Login triggers

Function Trigger_Login_CheckLogin (ByRef tNG)
	' check username
	Dim rs: Set rs  = tNG.transactionResult
	If rs.EOF Then
		Set errObj = new tNG_error
		errObj.Init "LOGIN_FAILED", Array(), Array()
		errObj.setFieldError "kt_login_user", "LOGIN_INVALID_USERNAME", array()
		Set Trigger_Login_CheckLogin = errObj
		Exit Function
	End If

	' Save this row of data
	Set rsRow = Server.CreateObject("Scripting.Dictionary")
	For each f in rs.Fields
		rsRow(f.name) = Cstr(rs.Fields.Item(f.name).Value & "")
	Next
	'  overwrite transactionResult
	Set tNG.transactionResult = rsRow

	rs.MoveNext
	If Not rs.EOF Then
		Set errObj = new tNG_error
		errObj.Init "LOGIN_FAILED", Array(), Array()
		errObj.setFieldError "kt_login_user", "LOGIN_FAILED_MANYRECORDS_FIELDERR", array()
		Set Trigger_Login_CheckLogin = errObj
		Exit Function
	End If	

	' check password
	Select case tNG.loginType
		case "form" 
			db_password = rsRow("kt_login_password")
			password_enc = tNG.getColumnValue("kt_login_password")
			If tNG_login_config("password_encrypt") = "true" Then
				password_enc = tNG_encryptString(password_enc)
			End If
			If db_password <> password_enc Then
				Set errObj = new tNG_error
				errObj.Init "LOGIN_FAILED", Array(), Array()
				errObj.setFieldError "kt_login_password", "LOGIN_INVALID_PASSWORD", array()
				Set Trigger_Login_CheckLogin = errObj
				Exit Function
			End If

		case "cookie"
			db_password_enc  = tNG_encryptString(rsRow("kt_login_password"))
			password_enc_cookie = tNG.getColumnValue("kt_login_test")
			If db_password_enc <> password_enc_cookie Then
				Set errObj = new tNG_error
				errObj.Init "LOGIN_FAILED", Array(), Array()
				Set Trigger_Login_CheckLogin = errObj
				Exit Function
			End If

		case "activation"
			random_key_trans = tNG.getColumnValue("kt_login_random")
			random_key_db = rsRow(tNG_login_config("randomkey_field"))
			If random_key_trans <> random_key_db Then
				Set errObj = new tNG_error
				errObj.Init "LOGIN_FAILED", Array(), Array()
				Set Trigger_Login_CheckLogin = errObj
				Exit Function
			End If
	End Select
	Set Trigger_Login_CheckLogin = nothing
End Function


Function Trigger_Login_CheckUserActive(ByRef tNG)
	If tNG_login_config("activation_field") <> "" Then
		' check activation
		Set rs = tNG.transactionResult  ' transactionResult is a dictionary, not a recordset (see Trigger_Login_CheckLogin)
		active_column = tNG_login_config("activation_field")
		If Not rs.Exists(active_column) Then
			Set errObj = new tNG_error
			errObj.Init "LOGIN_FAILED_NO_ACTIVE_FIELD", Array(), Array(active_column)
			Set Trigger_Login_CheckUserActive = errObj
			Exit Function		
		End If
		If Cstr(rs(active_column) & "") = "0" Then
			Set errObj = new tNG_error
			errObj.Init "LOGIN_INACTIVE_USER", Array(), Array(active_column)
			errObj.setFieldError "kt_login_user", "LOGIN_INACTIVE_USER_FIELDERR", array()
			Set Trigger_Login_CheckUserActive = errObj
			Exit Function					
		End If	
	End If
	Set Trigger_Login_CheckUserActive  = nothing
End Function


Function Trigger_Login_AddDynamicFields(ByRef tNG)
	' register all the columns from the recordset as transaction columns (to be available later)
	Set rs = tNG.transactionResult ' transactionResult is a dictionary, not a recordset (see Trigger_Login_CheckLogin)

	tNG.addColumn "kt_login_id", "STRING_TYPE", "VALUE", rs("kt_login_id"), ""

	tNG.addColumn "kt_login_user", "STRING_TYPE", "VALUE", rs("kt_login_user"), ""
	tNG.addColumn "kt_login_password_db", "STRING_TYPE", "VALUE", rs("kt_login_password"), ""
	If tNG_login_config("level_field") <> "" Then
		tNG.addColumn "kt_login_level", "STRING_TYPE", "VALUE", rs(tNG_login_config("level_field")), ""
	End If
	' must add {kt_login_redirect}
	login_redirect = ""
	If tNG.loginType = "form" Or tNG.loginType = "activation" Then  ' cookie login doesn't use redirect	
		If Session("KT_denied_pageuri") <> "" and isArray(Session("KT_denied_pagelevels")) Then
			' if restrict using levels is used
			If tNG_login_config("level_field") <> "" Then
					level_column = tNG_login_config("level_field")
					level_value = Cstr(rs(level_column) & "")
					
					arr_allowed_levels = Session("KT_denied_pagelevels")
					' check if the current user can be redirected to previously denied page
					If Ubound(arr_allowed_levels) <> -1 Then
						If KT_in_array(level_value, arr_allowed_levels, false) Then
							login_redirect = Session("KT_denied_pageuri")
						Else
							' redirect to the denied page will result into another denied page, so don't redirect
						End If
					Else
						' levels array has no elements - acccess is allowed to all logged users
						login_redirect = Session("KT_denied_pageuri")
					End If
			Else
				' no levels restriction is used, so we can redirect to previously denied page
				login_redirect = Session("KT_denied_pageuri")
			End If
			Session.Contents.Remove("KT_denied_pageuri")
			Session.Contents.Remove("KT_denied_pagelevels")
		End If
		
		If login_redirect = "" Then
			Set disp = tNG.dispatcher
			If KT_isSet(disp) Then
				relPath = disp.relPath
			Else	
				relPath =  ""
			End If
			relPath = KT_makeIncludedURL(relPath)
			If tNG_login_config("level_field") <> "" Then
				level_column = tNG_login_config("level_field")
				level_value = Cstr(rs(level_column)&"")
				
				login_redirect = relPath & tNG_login_config("redirect_success")
				If tNG_login_config_redirect_success.Exists(level_value) Then
					If tNG_login_config_redirect_success(level_value) <> "" Then
						login_redirect = relPath & tNG_login_config_redirect_success(level_value)
					End If	
				End If	
			Else
				login_redirect = relPath  & tNG_login_config("redirect_success")
			End If
		End If
	End If

	tNG.addColumn "kt_login_redirect", "STRING_TYPE", "VALUE", login_redirect, ""
	Set Trigger_Login_AddDynamicFields = nothing
End Function


Function Trigger_Login_SaveDataToSession(ByRef tNG)
	' default sessions
	Set rs = tNG.transactionResult
	
	Session("kt_login_user") = tNG.getColumnValue("kt_login_user")
	Session("kt_login_id") = tNG.getColumnValue("kt_login_id")
	If tNG_login_config("level_field") <> "" Then
		Session("kt_login_level") = tNG.getColumnValue("kt_login_level")
	End If

	' user-grid session
	For Each ses_name In tNG_login_config_session
		If rs.Exists(tNG_login_config_session(ses_name)) Then
			Session(ses_name)= rs(tNG_login_config_session(ses_name))
		Else
			Session(ses_name)= rs(ses_name)
		End If 	
	Next

	Set Trigger_Login_SaveDataToSession  = nothing
End Function


Function Trigger_Login_AutoLogin (ByRef tNG)
	If tNG.loginType <> "cookie" Then
		' unset cookies for any login transaction that is not of type 'cookie'
		If Request.Cookies("kt_login_id") <> "" and Request.Cookies("kt_login_test")<>"" Then
			Response.Cookies("kt_login_id") = ""
			Response.Cookies("kt_login_id").Expires = Date - 1
			Response.Cookies("kt_login_id").Path = KT_GetSitePath()
			
			Response.Cookies("kt_login_test") = ""
			Response.Cookies("kt_login_test").Expires = Date - 1
			Response.Cookies("kt_login_test").Path = KT_GetSitePath()
		End If
	End If
	
	Set cols = tNG.columns
	If cols.Exists("kt_login_rememberme") Then
		If tNG.getColumnValue("kt_login_rememberme") <> "" Then
			Response.Cookies("kt_login_id")	= tNG.getColumnValue("kt_login_id")
			Response.Cookies("kt_login_id").Expires = Date + Cint("0" & tNG_login_config("autologin_expires"))
			Response.Cookies("kt_login_id").Path = KT_GetSitePath()

			kt_test = tNG_encryptString(tNG.getColumnValue("kt_login_password_db"))
			Response.Cookies("kt_login_test") = kt_test
			Response.Cookies("kt_login_test").Expires = Date + Cint("0" & tNG_login_config("autologin_expires"))
			Response.Cookies("kt_login_test").Path = KT_GetSitePath()
		End If
	End If
	Set Trigger_Login_AutoLogin = nothing
End Function


' Register (Insert) Transaction triggers
Function Trigger_Registration_CheckUniqueUsername (ByRef tNG)
  Dim tblFldObj: Set tblFldObj = new tNG_CheckUnique
  tblFldObj.Init tNG
  tblFldObj.setTable tNG_login_config("table")
  tblFldObj.setFieldName tNG_login_config("user_field")
  tblFldObj.setErrorMsg KT_getResource("REGISTRATION_UNIQUE_USER_FIELDERR", "tNG", null)
  Set Trigger_Registration_CheckUniqueUsername = tblFldObj.Execute()
End Function

Function Trigger_Registration_CheckPassword(ByRef tNG)
	password_field = tNG_login_config("password_field")
	Set cols = tNG.columns
	If Not cols.Exists(password_field) Then
		password = tNG_generateRandomString(6)
		tNG.addColumn password_field, "STRING_TYPE", "VALUE", password, ""
	End If
	Set Trigger_Registration_CheckPassword = nothing
End Function

Function Trigger_Registration_EncryptPassword(ByRef tNG)
	password_column = tNG_login_config("password_field")
    password = tNG.getColumnValue(password_column)
    tNG.KT_RESERVED("kt_login_password") = password
    tNG.setRawColumnValue password_column, tNG_encryptString(password)
	Set Trigger_Registration_EncryptPassword = nothing
End Function


Function Trigger_Registration_PrepareActivation(ByRef tNG)
	Set cols = tNG.columns
	If Not cols.Exists(tNG_login_config("activation_field")) Then
		tNG.addColumn tNG_login_config("activation_field"), "NUMERIC_TYPE", "VALUE", "0", ""
	End If
	If tNG_login_config("randomkey_field") <> "" Then
		random_key = tNG_generateRandomString(0)
		tNG.addColumn tNG_login_config("randomkey_field"), "STRING_TYPE", "VALUE", random_key, ""
	End If
	Set Trigger_Registration_PrepareActivation = nothing
End Function

Function Trigger_Registration_RestorePassword(ByRef tNG)
	password_field = tNG_login_config("password_field")
	tNG.setRawColumnValue password_field, tNG.KT_RESERVED("kt_login_password")
	Set Trigger_Registration_RestorePassword = nothing
End Function


Function Trigger_Registration_AddDynamicFields(ByRef tNG)
	user_field = tNG_login_config("user_field")
	tNG.addColumn "kt_login_user", "STRING_TYPE", "VALUE", tNG.getColumnValue(user_field), ""
	password_field =  tNG_login_config("password_field")
	tNG.addColumn "kt_login_password", "STRING_TYPE", "VALUE", tNG.getColumnValue(password_field), ""

	Set cols = tNG.columns
	If tNG_login_config("activation_field") <> "" And tNG_login_config("email_field") <> "" and cols.Exists(tNG_login_config("email_field")) Then
		args = "kt_login_id=" & tNG.getColumnValue(tNG_login_config("pk_field"))
		If tNG_login_config("randomkey_field") <> "" Then
			args = args &  "&kt_login_random=" & tNG.getColumnValue (tNG_login_config("randomkey_field"))
		Else
			args = args &  "&kt_login_email=" & tNG.getColumnValue(tNG_login_config("email_field"))
		End If
		activation_page = KT_dirname(KT_getUri()) & KT_makeIncludedURL("") & "activate.asp?" & args 
		tNG.addColumn "kt_activation_page", "STRING_TYPE", "VALUE", activation_page, ""
	End If

	Set disp = tNG.dispatcher
	tmpRelPath = KT_makeIncludedURL(disp.relPath)
	login_page = KT_Rel2AbsUrl(KT_getUri(),  tmpRelPath, tNG_login_config("login_page"))
	tNG.addColumn "kt_login_page", "STRING_TYPE", "VALUE", login_page, ""

	redirect_page = tmpRelPath & tNG_login_config("login_page")
	Set cols = tNG.columns
	If tNG_login_config("email_field") <> "" And cols.Exists(tNG_login_config("email_field")) Then
		If tNG_login_config("activation_field") <> "" Then
			If cols.Exists(tNG_login_config("activation_field")) Then
				If Cstr(tNG.getColumnValue(tNG_login_config("activation_field")) & "") <> "0" Then
					redirect_page = KT_addReplaceParam(redirect_page, "info", "REG")
				Else
					redirect_page = KT_addReplaceParam(redirect_page, "info", "REG_ACTIVATE")
				End If	
			Else
				redirect_page = KT_addReplaceParam(redirect_page, "info", "REG_ACTIVATE")
			End If
		Else
			redirect_page = KT_addReplaceParam(redirect_page, "info", "REG_EMAIL")
		End If
	Else
		redirect_page = KT_addReplaceParam(redirect_page, "info", "REG")
	End If
	tNG.addColumn "kt_login_redirect", "STRING_TYPE", "VALUE", redirect_page, ""
	Set Trigger_Registration_AddDynamicFields = nothing
End Function


' Activation (Update) Transaction triggers
Function Trigger_Activation_Check(ByRef tNG)
	conn_name = tNG_login_config("connection")
	Execute "conn_string = MM_" & conn_name & "_STRING"
	login_conn = false
	If conn_string = tNG.connectionString Then
		login_conn = true
	End If

	Set myErr = new tNG_error
	If tNG_login_config("activation_field") = "" Then
		myErr.Init "ACTIVATION_NOT_ENABLED", array(), array()
		Set Trigger_Activation_Check = myErr
		Exit Function
	End If

	If tNG_login_config("email_field") = "" Then
		myErr.Init "ACTIVATION_NO_EMAIL", array(), array()
		Set Trigger_Activation_Check = myErr
		Exit Function
	End If

	If Not login_conn Or tNG.getTable() <> tNG_login_config("table") = "" Then
		myErr.Init "ACTIVATION_WRONG_TABLE", array(), array()
		Set Trigger_Activation_Check = myErr
		Exit Function
	End If

	If tNG.getPrimaryKey() <> tNG_login_config("pk_field") = "" Then
		myErr.Init "ACTIVATION_WRONG_PK", array(), array()
		Set Trigger_Activation_Check = myErr
		Exit Function
	End If

	Set cols = tNG.columns
	If Not cols.Exists(tNG_login_config("activation_field")) Then
		myErr.Init "ACTIVATION_NO_ACTIVE_FIELD", array(), array()
		Set Trigger_Activation_Check = myErr
		Exit Function
	End If

	' build the sql string to check 
	if tNG_login_config("randomkey_field") <> "" Then
		random_key = tNG_getRealValue("GET", "kt_login_random")
		If random_key = "" Then
			myErr.Init "ACTIVATION_NO_PARAM_RANDOM", array(), array()
			Set Trigger_Activation_Check = myErr
			Exit Function
		End If
		random_key = KT_escapeForSql(random_key, "STRING_TYPE")
		pk_value = KT_escapeForSql(tNG.getPrimaryKeyValue(), tNG_login_config("pk_type"))
		sql = "SELECT " &  KT_escapeFieldName(tNG.getPrimaryKey()) & ", " & KT_escapeFieldName(tNG_login_config("activation_field"))  & _
			 " FROM " & tNG.getTable()  & _ 
			 " WHERE " & KT_escapeFieldName(tNG.getPrimaryKey())  &  "="  & pk_value  & _
					 " AND "  &  KT_escapeFieldName(tNG_login_config("randomkey_field")) &  "="  & random_key
		Set conn = tNG.connection
		On Error Resume Next
		Set rs = conn.Execute(sql)
		If err.Number <> 0 Then
			myErr.Init "LOGIN_RECORDSET_ERR", array(), array()
			Set Trigger_Activation_Check = myErr
			On Error GoTo  0 
			Exit Function
		End If
		On Error GoTo  0 
	Else
		email_value = tNG_getRealValue("GET","kt_login_email")
		If email_value = "" Then
			myErr.Init "ACTIVATION_NO_PARAM_EMAIL", array(), array()
			Set Trigger_Activation_Check = myErr
			Exit Function
		End If
		email_value = KT_escapeForSql(email_value, "STRING_TYPE")
		pk_value = KT_escapeForSql(tNG.getPrimaryKeyValue(), tNG_login_config("pk_type"))
		sql = "SELECT " &  KT_escapeFieldName(tNG.getPrimaryKey()) & ", " & KT_escapeFieldName(tNG_login_config("activation_field"))  & _
			 " FROM " & tNG.getTable()  & _ 
			 " WHERE " & KT_escapeFieldName(tNG.getPrimaryKey())  &  "="  & pk_value  & _
				 	" AND "  &  KT_escapeFieldName(tNG_login_config("email_field")) &  "="  & email_value
		Set conn = tNG.connection
		On Error Resume Next
		Set rs = conn.Execute(sql)
		If err.Number <> 0 Then
			myErr.Init "LOGIN_RECORDSET_ERR", array(), array()
			Set Trigger_Activation_Check = myErr
			On Error GoTo  0 
			Exit Function
		End If
		On Error GoTo  0 
	End If

	If rs.EOF Then
		myErr.Init "ACTIVATION_NO_RECORDS", array(), array()
		Set Trigger_Activation_Check = myErr
		On Error GoTo  0 
		Exit Function
	End If

	activation_value = Cstr(rs.Fields.Item(tNG_login_config("activation_field")).Value & "")
	rs.MoveNext

	If Not rs.EOF Then
		myErr.Init "ACTIVATION_TOOMANY_RECORDS", array(), array()
		Set Trigger_Activation_Check = myErr
		On Error GoTo  0 
		Exit Function
	End If

	If activation_value <> "0" Then
		myErr.Init "ACTIVATION_ALREADY_ACTIVE", array(), array()
		Set Trigger_Activation_Check = myErr
		On Error GoTo  0 
		Exit Function
	End If

	' register the AFTER trigger
	tNG.registerTrigger Array("AFTER", "Trigger_Activation_Login", -1)
	Set Trigger_Activation_Check = nothing
End Function


Function Trigger_Activation_Login(ByRef tNG)
	Set disp = tNG.dispatcher
	relPath = KT_makeIncludedURL(disp.relPath)
	If tNG_login_config("randomkey_field") <> "" Then
		Set conn = tNG.connection
		redirect_page = tNG_activationLogin(conn)
		If redirect_page = "" Then
			redirect_page = relPath  & tNG_login_config("login_page")
		Else
			redirect_page = relPath  & redirect_page
		End If
	Else
		redirect_page = KT_addReplaceParam(relPath & tNG_login_config("login_page"), "info", "ACTIVATED")
	End If
	tNG.addColumn "kt_login_redirect", "STRING_TYPE", "VALUE", redirect_page
	Set Trigger_Activation_Login = nothing
End Function



'  Forgot Password (Update) Transaction triggers
Function Trigger_ForgotPassword_CheckEmail(ByRef tNG)
	Set myErr = new tNG_error
	If tNG_login_config("email_field") = "" Then
		myErr.Init "FORGOTPASS_NO_EMAIL", array(), array()
		Set Trigger_ForgotPassword_CheckEmail = myErr
		Exit Function
	End If

	If tNG.getTable() <> tNG_login_config("table") Then
		myErr.Init "FORGOTPASS_WRONG_TABLE", array(), array()
		Set Trigger_ForgotPassword_CheckEmail = myErr
		Exit Function
	End If

	If tNG.getPrimaryKey() <> tNG_login_config("email_field") Then
		myErr.Init "FORGOTPASS_WRONG_PK", array(), array()
		Set Trigger_ForgotPassword_CheckEmail = myErr
		Exit Function
	End If

	email_field = tNG_login_config("email_field")
	pk_field = tNG_login_config("pk_field")
	user_field = tNG_login_config("user_field")
	password_field = tNG_login_config("password_field")
	table = tNG_login_config("table")
	
	email_value = tNG.getColumnValue (tNG_login_config("email_field"))
	email_value = KT_escapeForSql(email_value, "STRING_TYPE")

	sql = "SELECT * FROM " & table  & _
		 " WHERE " &  KT_escapeFieldName(email_field)  &  "=" & email_value

	Set conn = tNG.connection
	On Error Resume Next
	Set rs = conn.Execute(sql)
	If err.Number <> 0 Then
		myErr.Init "LOGIN_RECORDSET_ERR", array(), array()
		Set Trigger_ForgotPassword_CheckEmail = myErr
		On Error GoTo  0 
		Exit Function
	End If
	On Error GoTo  0 

	If rs.EOF Then
		myErr.Init "FORGOTPASS_WRONG_EMAIL", array(), array()
		myErr.setFieldError email_field, "FORGOTPASS_WRONG_EMAIL_FIELDERR", array()
		Set Trigger_ForgotPassword_CheckEmail = myErr
		Exit Function
	End If
	
	password_value = Cstr(rs.Fields.Item(password_field).Value & "")
	user_value = rs.Fields.Item(user_field).Value
	If tNG_login_config("activation_field") <> "" Then
		activation_value = Cstr(rs.Fields.Item(tNG_login_config("activation_field")).Value & "")
	End If	
	
	rs.MoveNext
	If Not rs.EOF Then
		myErr.Init "FORGOTPASS_TOOMANY_RECORDS", array(), array()
		myErr.setFieldError email_field, "FORGOTPASS_TOOMANY_RECORDS_FIELDERR", array()
		Set Trigger_ForgotPassword_CheckEmail = myErr
		Exit Function
	End If
	If tNG_login_config("activation_field") <> "" Then
		If activation_value = "0" Then
			myErr.Init "FORGOTPASS_INACTIVE_USER", array(), array()
			Set Trigger_ForgotPassword_CheckEmail = myErr
			Exit Function
		End If
	End If
	tNG.KT_RESERVED("kt_login_user") = user_value
	If tNG_login_config("password_encrypt") = "true" Then
		tNG.KT_RESERVED("kt_login_password") = tNG_generateRandomString(6)
		tNG.KT_RESERVED("kt_login_password_enc") = tNG_encryptString(tNG.KT_RESERVED("kt_login_password"))
	Else
		tNG.KT_RESERVED("kt_login_password") = password_value
		tNG.KT_RESERVED("kt_login_password_enc") = tNG.KT_RESERVED("kt_login_password") ' the same values - plain
	End If

	tNG.addColumn password_field, "STRING_TYPE", "VALUE", tNG.KT_RESERVED("kt_login_password_enc")
	tNG.registerTrigger Array("AFTER", "Trigger_ForgotPassword_AddDynamicFields", -1)
	tNG.registerTrigger Array("ERROR", "Trigger_ForgotPassword_RemoveDynamicFields", -100)
	
	Set Trigger_ForgotPassword_CheckEmail = nothing
End Function

Function Trigger_ForgotPassword_RemoveDynamicFields(ByRef tNG)
	Set cols = tNG.columns
	If cols.Exists("kt_login_user") Then
		cols.Remove "kt_login_user"
	End If

	If cols.Exists("kt_login_password") Then
		cols.Remove "kt_login_password"
	End If

	If cols.Exists("kt_login_user") Then
		cols.Remove "kt_login_user"
	End If

	If cols.Exists("kt_login_page") Then
		cols.Remove "kt_login_page"
	End If

	If cols.Exists("kt_login_redirect") Then
		cols.Remove "kt_login_redirect"
	End If
	Set Trigger_ForgotPassword_RemoveDynamicFields = nothing
End Function



Function Trigger_ForgotPassword_AddDynamicFields(ByRef tNG)
	tNG.addColumn "kt_login_user", "STRING_TYPE", "VALUE", tNG.KT_RESERVED("kt_login_user")
	tNG.addColumn "kt_login_password", "STRING_TYPE", "VALUE", tNG.KT_RESERVED("kt_login_password")

	Set disp = tNG.dispatcher
	tmpRelPath = KT_makeIncludedURL(disp.relPath)

	login_page = KT_Rel2AbsUrl(KT_getUri(),  tmpRelPath, tNG_login_config("login_page"))
	tNG.addColumn "kt_login_page", "STRING_TYPE", "VALUE", login_page
	
	redirect_page = tmpRelPath  &  tNG_login_config("login_page") 
    redirect_page  =  KT_addReplaceParam(redirect_page, "info", "FORGOT")
	tNG.addColumn "kt_login_redirect", "STRING_TYPE", "VALUE", redirect_page

	Set Trigger_ForgotPassword_AddDynamicFields = nothing
End Function


' Update User Table Transaction triggers
Function Trigger_UpdatePassword_CheckOldPassword(ByRef tNG)
	Set myErr = new tNG_error

	password_field = tNG_login_config("password_field")
	password_value = tNG.getColumnValue (password_field)
	old_password_value = tNG_DynamicData("{POST.old_" & password_field & "}", tNG, null, null, null, null)

	If  old_password_value <> ""  And password_value = "" Then
		myErr.Init "UPDATEPASS_NO_NEW_PASS", array(), array()
		myErr.setFieldError password_field, "UPDATEPASS_NO_NEW_PASS_FIELDERR", array()
		Set Trigger_UpdatePassword_CheckOldPassword = myErr
		Exit Function
	End If
	
	If password_value <> "" Then
		If old_password_value = "" Then
			tNG.addColumn "old_" &  password_field, "STRING_TYPE", "VALUE", ""
			myErr.Init "UPDATEPASS_NO_OLD_PASS", array(), array()
			myErr.setFieldError "old_" & password_field, "UPDATEPASS_NO_OLD_PASS_FIELDERR", array()
			Set Trigger_UpdatePassword_CheckOldPassword = myErr
			Exit Function
		Else
			if tNG_login_config("password_encrypt") = "true" Then
				old_password_value = tNG_encryptString(old_password_value)
			End If
			table = tNG_login_config("table")
			pk_field = tNG_login_config("pk_field")
			pk_value = KT_escapeForSql(tNG.getPrimaryKeyValue(), tNG_login_config("pk_type"))
			
			sql = "SELECT " & KT_escapeFieldName(password_field) & _
				 " FROM "  & table  & _
				 " WHERE " & KT_escapeFieldName(pk_field)  &  "="  & pk_value
				 
			Set conn = tNG.connection
			On Error Resume Next
			Set rs = conn.Execute(sql)
			If err.Number <> 0 Then
				myErr.Init "LOGIN_RECORDSET_ERR", array(), array()
				Set Trigger_UpdatePassword_CheckOldPassword = myErr
				On Error GoTo  0 
				Exit Function
			End If
			On Error GoTo  0 

			If rs.EOF Then
				myErr.Init "UPDATEPASS_NO_RECORD", array(), array()
				Set Trigger_UpdatePassword_CheckOldPassword = myErr
				On Error GoTo  0 
				Exit Function
			End If

			db_password_value = rs.Fields.Item(tNG_login_config("password_field")).Value
			rs.MoveNext
			
			If Not rs.EOF Then
				myErr.Init "UPDATEPASS_TOMANY_RECORDS", array(), array()
				Set Trigger_UpdatePassword_CheckOldPassword = myErr
				On Error GoTo  0 
				Exit Function
			End If

			If db_password_value <> old_password_value Then
				tNG.addColumn "old_" & password_field, "STRING_TYPE", "VALUE", ""
				myErr.Init "UPDATEPASS_WRONG_OLD_PASS", array(), array()
				myErr.setFieldError "old_" & password_field, "UPDATEPASS_WRONG_OLD_PASS_FIELDERR", array()
				Set Trigger_UpdatePassword_CheckOldPassword = myErr
				On Error GoTo  0 
				Exit Function
			End If
		End If
	End If
	Set Trigger_UpdatePassword_CheckOldPassword = nothing
End Function



Function Trigger_UpdatePassword_EncryptPassword (ByRef tNG)
	If tNG_login_config("password_encrypt") = "true" Then  
		password_column = tNG_login_config("password_field")
		password = tNG.getColumnValue(password_column)
		If password <> "" Then
			tNG.KT_RESERVED("kt_login_password") = password
			tNG.setRawColumnValue password_column, tNG_encryptString(password)
		End If	
	End If
	Set Trigger_UpdatePassword_EncryptPassword = nothing
End Function


Function Trigger_UpdatePassword_RemovePassword (ByRef tNG)
	password_column = tNG_login_config("password_field")
	password = tNG.getColumnValue(password_column)
	If password = "" Then
		'removes the password from the array
		Set cols = tNG.columns
		Set tNG.KT_RESERVED("KT_password_column") = cols(password_column)
		cols.Remove password_column
	End If	
	Set Trigger_UpdatePassword_RemovePassword = nothing
End Function


Function Trigger_UpdatePassword_AddPassword (ByRef tNG)
	if tNG.KT_RESERVED.Exists("KT_password_column") Then  
		' only if the password has been removed
		password_column = tNG_login_config("password_field")
		Set cols = tNG.columns
		Set cols("password_column") = tNG.KT_RESERVED("KT_password_column")
	End If	
	Set Trigger_UpdatePassword_AddPassword = nothing
End Function


Function Trigger_UpdatePassword_RestorePassword (ByRef tNG)
	if tNG_login_config("password_encrypt") = "true" Then 
		' this is a double check
		password_column = tNG_login_config("password_field")
		password = tNG.getColumnValue(password_column)
		If password <> "" Then
			password = tNG.KT_RESERVED("kt_login_password")
			tNG.setRawColumnValue password_column, password
		End If	
	End If
	Set Trigger_UpdatePassword_RestorePassword = nothing
End Function

Function Trigger_UpdatePassword_RemoveOldPassword (ByRef tNG)
	password_field = tNG_login_config("password_field")
	Set cols = tNG.columns
	If cols.Exists("old_" & password_field) Then
		cols.Remove "old_" & password_field
	End If
	Set Trigger_UpdatePassword_RemoveOldPassword = nothing
End Function


%> 
