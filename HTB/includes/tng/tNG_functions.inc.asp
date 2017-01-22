<% 

Function tNG_fieldHasChanged(fieldName, fieldValue)
	
	ExecStr  = "" & _ 
	"tNG_fieldHasChanged_returnedVal = false" & vbNewLine & _
	"If Not IsObject(tNG_fieldHasChanged_values) Then" & vbNewLine & _ 
	"	Set tNG_fieldHasChanged_values = Server.CreateObject(""Scripting.Dictionary"")" & vbNewLine & _
	"End If" & vbNewLine & _
	"If tNG_fieldHasChanged_values.Exists(""" & fieldName & """) Then"  & vbNewLine & _
	"	If tNG_fieldHasChanged_values(""" & fieldName & """) <> """ & fieldValue & """ Then"  & vbNewLine & _
	"		tNG_fieldHasChanged_returnedVal = true"  & vbNewLine & _
	"	End If" & vbNewLine & _
	"Else" & vbNewLine & _
	"	tNG_fieldHasChanged_returnedVal = true"  & vbNewLine & _
	"End If" & vbNewLine & _
	"tNG_fieldHasChanged_values(""" & fieldName & """) = """ & fieldValue & """"
	
	ExecuteGlobal ExecStr
	'Response.write vbNewLine & vbNewLine &  ExecStr & vbNewLine &  vbNewLine
	tNG_fieldHasChanged = tNG_fieldHasChanged_returnedVal
End Function

Function tNG_fileExists(dynamicFolder, dynamicFileName)
	ret = false
	If dynamicFileName <> "" Then
		folder = tNG_DynamicData(dynamicFolder, nothing, null, null, null, null)
		fileName = tNG_DynamicData(dynamicFileName, nothing, null, null, null, null)
		If fileName <> "" Then
			folder = replace(folder, "\", "/")
			If right(folder, 1) <> "/" Then
				folder = folder & "/"
			End If
			folder = KT_makeIncludedPath(folder)
			relPath = folder & fileName
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			On Error Resume Next
			If fso.FileExists(Server.MapPath(relPath)) Then
				ret = True
			End If
			If Err.Number <> 0 Then
				ret = False
			End If
			On Error GoTo 0
			Set fso = nothing
		End If
	End If	
	tNG_fileExists = ret
End Function	

Function tNG_downloadDynamicFile(siteRootPath, dynamicFolder, dynamicFileName) 
	ret = ""
	If dynamicFileName <> "" Then
		folder = tNG_DynamicData(dynamicFolder, nothing, null, null, null, null)
		fileName = tNG_DynamicData(dynamicFileName, nothing, null, null, null, null)
		folder = KT_makeIncludedPath(folder)
		folder = Server.MapPath(folder) & "\"
		absPath = folder & fileName

		nowdt = Timer()
		If IsObject(Session("tNG_download"))Then
			Set downloadInfo = Server.CreateObject("Scripting.Dictionary")
			Set arr = Session("tNG_download")
			For each tmpId in arr
				Set details = arr(tmpId)
				If details("time") > (nowdt - 5 * 60) Then
					Set downloadInfo(tmpId) = details
				End If
			Next
			Set Session("tNG_download") = downloadInfo
		Else
			Set Session("tNG_download") = Server.CreateObject("Scripting.Dictionary")
		End If

		uniqueId = KT_md5(absPath)
		If Not Session("tNG_download").Exists(uniqueId) Then
			Set downloadInfo = Server.CreateObject("Scripting.Dictionary")
			downloadInfo("realPath") = absPath
			downloadInfo("fileName") = fileName
			downloadInfo("time") = nowdt
			Set Session("tNG_download")(uniqueId) = downloadInfo
		End If
		If siteRootPath <> "" Then
			siteRootPath = replace(siteRootPath, "\", "/")
			If right(siteRootPath, 1) <> "/" Then
				siteRootPath = siteRootPath & "/"
			End If		
		End If
		ret = siteRootPath & "includes/tng/pub/tNG_download.asp?id=" & Server.URLEncode(uniqueId)
	End If
	tNG_downloadDynamicFile	 = ret
End Function


Function tNG_showDynamicImage(siteRootPath, dynamicFolder, dynamicFileName)
	If dynamicFileName <> "" Then
		folder = tNG_DynamicData(dynamicFolder, nothing, null, null, null, null)
		fileName = tNG_DynamicData(dynamicFileName, nothing, null, null, null, null)
		If fileName <> "" Then
			folder = replace(folder, "\", "/")
			If right(folder, 1) <> "/" Then
				folder = folder & "/"
			End If
			relPath = folder & fileName
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			On Error Resume Next
			If Not fso.FileExists(Server.MapPath(KT_makeIncludedPath(folder) & fileName)) Then
				relPath = siteRootPath & "includes/tng/styles/img_not_found.gif"
			End If
			If Err.Number <> 0 Then
				relPath = siteRootPath & "includes/tng/styles/img_not_found.gif"
			End If
			On Error GoTo 0			
			Set fso = nothing
		Else 
			relPath = siteRootPath & "includes/tng/styles/img_not_found.gif"
		End If	
	Else
		relPath = siteRootPath & "includes/tng/styles/img_not_found.gif"
	End If	
	tNG_showDynamicImage = relPath
End Function



Function tNG_showDynamicThumbnail(siteRootPath, dynamicFolder, dynamicFileName, width, height, proportional)
	relPath = ""
	If dynamicFileName <> "" Then
		folder = tNG_DynamicData(dynamicFolder, nothing,  null, null, null, null)
		fileName = tNG_DynamicData(dynamicFileName, nothing, null, null, null, null)
		folder = replace(folder, "\", "/")
		If right(folder, 1) <> "/" Then
			folder = folder & "/"
		End If
		
		relPath = folder & fileName
		Set path_info = KT_pathinfo(fileName)
		thumbnailFolder = folder  &  "thumbnails/"
		thumbnailName = path_info("filename") & "_" &  Cint(width) & "x" & Cint(height) & path_info("dot") & path_info("extension")
		relPath = thumbnailFolder & thumbnailName

		Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
		
		fileExists = True
		On Error Resume Next
		fileExists = fso.FileExists(Server.MapPath(KT_makeIncludedPath(relPath)))
		If Err.Number <> 0 Then
			relPath = siteRootPath & "includes/tng/styles/img_not_found.gif"
		End If
		On Error GoTo 0
		
		If Not fileExists Then
			If fileName <> "" and fso.FileExists(Server.MapPath(KT_makeIncludedPath(folder) & fileName)) Then
				Set image = new KT_image
				image.setPreferedLib tNG_prefered_image_lib
				image.addCommand tNG_prefered_imagemagick_path
				image.resize Server.MapPath(KT_makeIncludedPath(folder) & fileName), Server.MapPath(KT_makeIncludedPath(thumbnailFolder)), thumbnailName, width, height, proportional
				If image.hasError() Then
					errorArr = image.getError()
					If tNG_debug_mode = "DEVELOPMENT" Then
						errMsg = errorArr(1)
						relPath = siteRootPath & "includes/tng/styles/cannot_thumbnail.gif"" />" & errMsg & "<img src=""" & siteRootPath & "includes/tng/styles/cannot_thumbnail.gif"
					Else
						relPath = siteRootPath & "includes/tng/styles/cannot_thumbnail.gif"
					End If
				End If
			Else
				relPath = siteRootPath & "includes/tng/styles/img_not_found.gif"
			End If
		End If
		Set fso = nothing
	End If
	tNG_showDynamicThumbnail = relPath
End Function


Function tNG_GetDynamicDataFunctionValue( placeholder )
	tNG_GetDynamicDataFunctionValue = placeholder

	KT_getInternalTimeFormat()
	' getting date	
	Dim yyyy: yyyy = CInt(DatePart("yyyy", Date()))
	Dim m: m = CInt(DatePart("m",  Date()))
	If m < 10 Then
		m = "0" & m
	End If
	Dim d: d = CInt(DatePart("d",  Date()))
	If d < 10 Then
		d = "0" & d
	End If
	current_date = yyyy & "-" & m & "-" & d
	
	
	' getting time
	Dim h: h = Cint(Hour(Now))
	If h < 10 Then
		h = "0" & h
	End If
	Dim min: min = Cint(Minute(Now))
	If min < 10 Then
		min = "0" & min
	End If
	Dim sec: sec = Cint(Second(Now))
	If sec < 10 Then
		sec = "0" & sec
	End If	
	current_time = h & ":" & min & ":" & sec
	
	If KT_in_array(placeholder, Array("NOW()", "now()", "NOW", "now"), true) Then
		date_now = KT_convertDate(current_date, "yyyy-mm-dd", KT_screen_date_format)	
		tNG_GetDynamicDataFunctionValue = date_now
	ElseIf KT_in_array(placeholder, Array("NOW_DT()", "now_dt()", "NOW_DT", "now_dt"), true) Then
		date_dt_now = KT_convertDate(current_date & " " & current_time, "yyyy-mm-dd HH:ii:ss", KT_screen_date_format & " " & KT_screen_time_format_internal)	
		tNG_GetDynamicDataFunctionValue = date_dt_now
	ElseIf KT_in_array(placeholder, Array("NOW_T()", "now_t()", "NOW_T", "now_t"), true) Then
		date_t_now = KT_convertDate(current_time, "HH:ii:ss", KT_screen_time_format_internal)
		tNG_GetDynamicDataFunctionValue = date_t_now
	ElseIf KT_in_array(placeholder, Array("KT_REFERRER", "kt_referrer"), true) Then
		KT_referrer = Request.ServerVariables("HTTP_REFERER")
		tNG_GetDynamicDataFunctionValue = KT_referrer
	End If
End Function


'
' Function tNG_DynamicData
' @param string $expression The expression to be evaluated
' @param object or null $tNG The tNG context in which the expression is evaluated
' @param string $escapeMethod The string escape method for the evaluated values (rawurlencode and SQL)
' @param booolean $useSavedData Weather to use the current tNG data or the saved values
' @param array $extraParams Extra expression parameters passed when for evaluation (of form $key => $value; any encounter of key will be replaced with its value)
'
Function tNG_DynamicData(expression, ByRef tNG, escapeMethod, useSavedData, extraParams, errorIfNotFound)
	tNG_DynamicData = expression
	If isEmpty(expression) Or isnull(expression) Or expression = "" Then
		Exit Function
	End If

	If isNull(escapeMethod) Then
		escapeMethod = ""
	End If
	If isNull(useSavedData) Then
		useSavedData = false
	End If
	If Not KT_isSet(extraParams) Then
		Set extraParams = Server.CreateObject("Scripting.Dictionary")
	End If
	If isNull(errorIfNotFound) Then
		errorIfNotFound = true
	End If
	
	PB = "{"
	PE = "}"
		
	Set placeholdersArr = tNG_getReplacementsFromMessage(expression)
	dynamicDataFunctions = Array("NOW()", "now()", "NOW", "now", _
							"NOW_DT()", "now_dt()", "NOW_DT", "now_dt", _
							"NOW_T()", "now_t()", "NOW_T", "now_t", _
							"KT_REFERRER", "kt_referrer")
	Set replacementsArr = Server.CreateObject("Scripting.Dictionary")
	Select Case escapeMethod
		Case "rawurlencode"
		
		Case "expression"
		
		Case "SQL"
			If not KT_isSet(tNG) Then
				escapeMethod = false
			End If
		Case Else
			escapeMethod = false	
	End Select

	For each idx in placeholdersArr
		placeholder = placeholdersArr(idx)
		If KT_in_array(placeholder, KT_array_keys(extraParams), true) Then
			'  extra params have priority 1
			placeholderType = "tNG_DDextra"
			placeholderName = placeholder
		Else
			' functions have priority 2
			If KT_in_array(placeholder, dynamicDataFunctions, true) Then
				placeholderType = "tNG_DDfunction"
				placeholderName = placeholder
			Else
				ptpos = instr(placeholder, ".")
				If ptpos = 0 Then
					' tNG field
					If KT_isSet(tNG) Then
						' attached to a tng, replace field with value
						placeholderType = "tNG_tNGfield"
					 	placeholderName = placeholder
					Else
						' no tng, leave as is
						placeholderType = "tNG_tNGfieldLater"
					 	placeholderName = placeholder
					End If
				 Else	
					placeholderType = left(placeholder, ptpos-1)
					placeholderName = mid(placeholder, ptpos+1)
				 End If
			End If
		End If

		placeholder = PB & placeholder & PE
		Select case LCase(placeholderType)
			case "tng_ddfunction"
				replacementsArr(placeholder) = tNG_GetDynamicDataFunctionValue(placeholderName)
			case "tng_ddextra"
				replacementsArr(placeholder) = extraParams(placeholderName)
			case "tng_tngfield"
				If useSavedData Then
					placeholderValue = tNG.getSavedValue(placeholderName)
				Else
					Set cols = tNG.columns
					If cols.Exists(placeholderName) Or tNG.getPrimaryKey() = placeholderName Then
						placeholderValue = tNG.getColumnValue(placeholderName)
						placeholderType = tNG.getColumnType(placeholderName)
					Else
						If errorIfNotFound Then
							Response.write 	"tNG_DynamicData:<br />Column " & placeholderName & " is not part of the current transaction."
							Response.End()
						Else
							placeholderValue = placeholder
						End If
					End If
					If escapeMethod = "SQL" Then
						placeholderValue = KT_escapeForSql(placeholderValue, placeholderType)
					End If
				End If
				replacementsArr(placeholder) = placeholderValue
			
			case "tng_tngfieldlater"

			case "get"
				myPlaceholderName = placeholderName
				If KT_isSet(tNG) Then
					If KT_isSet(tNG.multipleIdx) Then
						myPlaceholderName = myPlaceholderName & "_" & tNG.multipleIdx
					End If
				End If
				replacementsArr(placeholder) = tNG_getRealValue("GET", myPlaceholderName)
				If Not KT_isSet(replacementsArr(placeholder)) Then
					replacementsArr(placeholder) = tNG_getRealValue("GET", placeholderName)
				End If
				
			case "post"
				myPlaceholderName = placeholderName
				If KT_isSet(tNG) Then
					If KT_isSet(tNG.multipleIdx) Then
						myPlaceholderName = myPlaceholderName & "_" & tNG.multipleIdx
					End If
				End If
				replacementsArr(placeholder) = tNG_getRealValue("POST", myPlaceholderName)
				If Not KT_isSet(replacementsArr(placeholder)) Then
					replacementsArr(placeholder) = tNG_getRealValue("POST", placeholderName)					
				End If

			case "request"
				myPlaceholderName = placeholderName
				If KT_isSet(tNG) Then
					If KT_isSet(tNG.multipleIdx) Then
						myPlaceholderName = myPlaceholderName & "_" & tNG.multipleIdx
					End If
				End If
				replacementsArr(placeholder) = tNG_getRealValue("REQUEST", myPlaceholderName)
				If Not KT_isSet(replacementsArr(placeholder)) Then
					replacementsArr(placeholder) = tNG_getRealValue("REQUEST", placeholderName)
				End If
				
			case "cookie"
				replacementsArr(placeholder) = tNG_getRealValue("COOKIE", placeholderName)
				
			case "session"
				replacementsArr(placeholder) = tNG_getRealValue("SESSION", placeholderName)

			case "application"
				replacementsArr(placeholder) = tNG_getRealValue("APPLICATION", placeholderName)
				
			case "globals"
				replacementsArr(placeholder) = tNG_getRealValue("GLOBALS", placeholderName)

			case else
				' recordset
				On Error Resume Next
				str = "Set KT_rsDD = " & placeholderType & vbNewLine & _
					"KT_valDD = KT_rsDD.Fields.Item(""" & placeholderName & """).Value"
				ExecuteGlobal str
				If Err.Number <> 0 Then
					placeholderValue = placeholder
				Else
					placeholderValue = KT_valDD
				End If
				On Error GoTo 0
				replacementsArr(placeholder) = placeholderValue
				
		End Select
	Next

	If escapeMethod = "rawurlencode" Then
		If Not replacementsArr.Exists("{kt_login_redirect}")  And Not replacementsArr.Exists("{kt_referrer}") And Not replacementsArr.Exists("{KT_REFERRER}") Then
			For each k in replacementsArr
				replacementsArr(k) = Server.URLEncode(replacementsArr(k) & "")
			Next	
		End If
	ElseIf escapeMethod = "expression" Then
		For each k in replacementsArr
			replacementsArr(k) = KT_escapeExpression(replacementsArr(k))
		Next	
	End If	
	' build the expression
	newexpression = expression
	For each k in replacementsArr
		newexpression = replace(newexpression, k, replacementsArr(k) & "")
	Next
	tNG_DynamicData = newexpression
End Function


'
' Compiles a method and a reference into a value
' Ex: method=GET and reference=test, return value=$_GET['test']
' @param string $method The method (GET, POST, etc)
' @param string $reference The reference (variable name)
' @return object unknown The compiled value
'         the return has stripped slashes if necessary
'

Public Function tNG_getRealValue (method, reference) 	
	ret	 = null
	Select case method
		case "GET"
			If KT_isSet( Request.QueryString(reference)) Then	
				ret = Request.QueryString(reference)
			End If
			
		case "POST"
			If KT_isSet( Request.Form(reference)) Then	
				ret = Request.Form(reference)
			End If

		case "REQUEST"
			If KT_isSet( Request(reference)) Then	
				ret = Request(reference)
			End If
		
		case "COOKIE"
			If KT_isSet( Request.Cookies(reference)) Then	
				ret = Request.Cookies(reference)
			End If

		case "SESSION"
			If KT_isSet(Session(reference)) Then	
				ret = Session(reference)
			End If
			
		case "APPLICATION"
			If KT_isSet(Application(reference)) Then	
				ret = Application(reference)
			End If
						
		case "GLOBALS"
			ExecuteGlobal "MYGLOBAL = " & reference
			ret = MYGLOBAL
			
		case "SERVER":
			If KT_isSet(Request.ServerVariables(reference)) Then
				ret = Request.ServerVariables(reference)
			End If	

		case "FILES"
			ret = Request.Form(reference)

		case "VALUE"
			ret = reference

		case "CURRVAL"
			ret = null
			
		case else
			Response.write "tNG_getRealValue:<br />Unknown method: " & method & "."
			Response.End()

	End Select
	tNG_getRealValue = ret
End	Function



Sub tNG_prepareValues(ByRef colDetails)
	Dim type2alt: Set type2alt = Server.CreateObject("Scripting.Dictionary")
	type2alt("CHECKBOX_1_0_TYPE")	=	 "1"
	type2alt("CHECKBOX_-1_0_TYPE")	=	 "-1"
	type2alt("CHECKBOX_YN_TYPE")	=	 "Y"
	type2alt("CHECKBOX_TF_TYPE")	=	 "t"
	If colDetails.Exists("method") And colDetails.Exists("reference") And colDetails.Exists("type") Then
		colValue = tNG_getRealValue(colDetails("method"), colDetails("reference"))
		colType = colDetails("type")

		If colDetails("method") = "VALUE" Then
			colValue = tNG_DynamicData(colValue, nothing, null, null, null, null)
			If colDetails.Exists("default") Then
				colDetails("default") = colValue
			End If
		ElseIf colDetails.Exists("default") Then
			colDetails("default") = tNG_DynamicData(colDetails("default"), nothing, null, null, null, null)	
		End If
		
		If colType = "CHECKBOX_YN_TYPE" OR colType = "CHECKBOX_1_0_TYPE" OR colType = "CHECKBOX_-1_0_TYPE" OR colType = "CHECKBOX_TF_TYPE" Then
			If KT_isSet(colValue) Then
				colValue = type2alt(colType)
			Else
				colValue = ""
			End If
		End If
		
		If colType = "DATE_TYPE" Or colType = "DATE_ACCESS_TYPE" Then
			colValue = KT_formatDate2DB(colValue)
			If colDetails.Exists("default") Then
				If colDetails("default") <> "" Then
					colDetails("default") = KT_formatDate2DB(colDetails("default"))
				End If	
			End If
		End If
	Else
		colValue = ""
	End If
	colDetails("value") = colValue
End Sub


Function tNG_getReplacementsFromMessage(str) 
	Dim ret: Set ret = Server.CreateObject("Scripting.Dictionary")
	Dim matches
	Dim indexmatches: indexmatches = 0
	KT_preg_match "\{([\w\d\.\s\(\)]+)\}", str, matches
	For each match in matches
		ret(indexmatches) = replace(replace(match.Value, "}", ""), "{", "")
		indexmatches = indexmatches + 1
	Next
	Set tNG_getReplacementsFromMessage = ret
End Function

Function tNG_getEscapedStringFromMessage(str)
	tNG_getEscapedStringFromMessage = KT_preg_replace("{[^\s}]+\}", "%s", str)
End Function



' LOGIN functions

Function tNG_generateRandomString(length)
	Randomize Timer
	newstring = KT_md5(CStr(Rnd * 999999999999))
	If length > 0 Then
		tNG_generateRandomString = Mid(newstring, 1, length)
	Else
		tNG_generateRandomString = newstring
	End If
End Function


Function tNG_encryptString (plain_string) 
	tNG_encryptString = KT_md5(plain_string)
End Function

Function tNG_activationLogin(connection)
	If Request.QueryString("kt_login_id") <> "" AND Request.QueryString("kt_login_random") <> "" Then
		' make an instance of the transaction object
		Set loginTransaction_activation = new tNG_login
		loginTransaction_activation.Init connection
		' register triggers
		' automatically start the transaction
		loginTransaction_activation.registerTrigger Array("STARTER", "Trigger_Default_Starter", 1, "VALUE", "1")
		' add columns
		loginTransaction_activation.setLoginType "activation"
		loginTransaction_activation.addColumn "kt_login_id", tNG_login_config("pk_type"), "GET", "kt_login_id", ""
		loginTransaction_activation.addColumn "kt_login_random", "STRING_TYPE", "GET", "kt_login_random", ""
		loginTransaction_activation.executeTransaction
		Set cols = loginTransaction_activation.columns
		If cols.Exists("kt_login_redirect") Then
			' return already computed redirect page
			tNG_activationLogin = loginTransaction_activation.getColumnValue("kt_login_redirect")
			Exit Function
		End If
	End If
	tNG_activationLogin = ""
End Function


Function tNG_cookieLogin(connection)
	If Session("kt_login_user") <> "" Then	
		Exit Function
	End If

	If Request.Cookies("kt_login_id") <> "" AND Request.Cookies("kt_login_test") <> "" Then
		' make an instance of the transaction object
		Set loginTransaction_cookie = new tNG_login
		loginTransaction_cookie.Init connection
		' register triggers
		' automatically start the transaction
		loginTransaction_cookie.registerTrigger Array("STARTER", "Trigger_Default_Starter", 1, "VALUE", "1")
		' add columns
		loginTransaction_cookie.setLoginType "cookie"
		loginTransaction_cookie.addColumn "kt_login_id", tNG_login_config("pk_type"), "COOKIE", "kt_login_id", ""
		loginTransaction_cookie.addColumn "kt_login_test", "STRING_TYPE", "COOKIE", "kt_login_test", ""
		loginTransaction_cookie.executeTransaction
	End If
End Function

' Used by addFields trigger
Sub tNG_addColumn (ByRef tNG, colName__param, type__param, method__param, reference__param) 
	If KT_in_array(tNG.getTransactionType(), Array("_insert", "_multipleInsert"), True) Then
		tNG.addColumn colName__param, type__param, method__param, reference__param, ""
	Else
		tNG.addColumn colName__param, type__param, method__param, reference__param
	End If	
End Sub

'wrapper for KT_getResource(resourceName, "NXT")
Function NXT_getResource(resourceName)
	NXT_getResource = KT_getResource(resourceName, "NXT", null)
End Function

%>