<%
Class KT_Image
	Private userErrorMessage		'array		-	error message to be displayed as User Error
	Private develErrorMessage		'array		-	error message to be displayed as Developer Error
	
	Private available_DotNET
	Private available_IMKShell
	
	Private URLToDotNETImageComponent		' will keep the url to the aspx page
	Private PathToIMKInstalationFolder  	' will keep the absolute path to Image Magick Instalation folder
	
	Private IMKShell				' shell object used to call Image Magick
	Private HTTPObj					' used to comunicate with aspx page

	Private initialized_DotNET
	Private initialized_IMKShell
			
	Private InitErrors_DotNET
	Private InitErrors_IMKShell
	

	Private orderLib
	Private PathToSecurityFile
	Private UseSecurityCheck
	Private Sub Class_Initialize()
		orderLib = Array(".net", "imagemagick")
		PathToSecurityFile = ""
		UseSecurityCheck = True
		
		PathToIMKInstalationFolder = "C:\Program Files\ImageMagick\"

		userErrorMessage	= Array()
		develErrorMessage	= Array()
		Set IMKShell = nothing
		Set HTTPObj = nothing
		
		available_DotNET = false
		available_IMKShell = false
		
		initialized_DotNET = false
		initialized_IMKShell = false
	End Sub

	Private Sub Class_terminate()
		Set ShellObj = nothing
		Set IMKObj = nothing
		Set HTTPObj = nothing
	End Sub
	
	Public Sub addCommand(path__param)
		path = trim(path__param)
		If path <> "" Then
			If right(path, 1) <> "\" And right(path, 1) <> "/" Then
				PathToIMKInstalationFolder = trim(path) & "\"
			Else
				PathToIMKInstalationFolder = trim(path)
			End If
		End If
	End Sub

	Public Sub setPreferedLib(lib)
		If Not KT_in_array(LCase(lib), orderLib, True) Then
			Exit Sub
		End If
		lib = LCase(lib)
		newOrder = array()
		Dim i
		newOrder = KT_array_push(newOrder, lib)
		For i=0 to ubound(orderLib)
			If orderLib(i) <> lib Then
				newOrder = KT_array_push(newOrder, orderLib(i))
			End If	
		Next
		orderLib = newOrder
	End Sub


	' ====== IMAGE SIZE =====	
	Public Function ImageSize (sourceFilePath) 
		ImageSize = Array (-1,-1)

		Dim i
		For i=0 to ubound(orderLib)
			lib = orderLib(i)

			Select Case lib
			Case ".net"
				If IsAvailableDotNET() Then
					GenerateAndWriteSecurityFile sourceFilePath
					post = "command=imagesize"
					post = post & "&PathToSecurityFile=" & Server.URLEncode(PathToSecurityFile)
					post = post & "&source=" & Server.URLEncode(sourceFilePath)
					output = ExecCommand_DotNET (post)
					DeleteSecurityFile
					If Not hasError Then
						If instr(output, "###") <> 0 Then
							arrSize = Split(output, "###")
							On Error resume Next
							ImageSize = Array (Cint(arrSize(0)), Cint(arrSize(1)))	
							On Error GoTo 0
						End If	
					End If
					Exit Function							
				End If
				
			Case "imagemagick"
			
				If IsAvailableIMKShell() Then
					output = ExecCommand_IMKShell ("identify.exe", Array("-format ""%w###%h""", """" & sourceFilePath & """"))
					If Not hasError Then
						If instr(output, "###") <> 0 Then
							arrSize = Split(output, "###")
							On Error resume Next
							ImageSize = Array (Cint(arrSize(0)), Cint(arrSize(1)))	
							On Error GoTo 0
						End If	
					End If
					Exit Function
				End If
			End Select
		Next
		
		If Not available_DotNET and Not available_IMKShell Then
			clearError
			setError "ASP_IMAGE_COMPONENTS_NOT_AVAILABLE", Array(), Array(InitErrors_DotNET & "<br />" & InitErrors_IMKShell)
			Exit Function
		End If			
	End Function


	' ====== RESIZE =====	
	Public Sub Resize (sourceFilePath, destinationFolder, destinationFileName, width, height, keepProportion) 
		If destinationFolder <> "" Then
			Dim fld: Set fld = new KT_Folder
			fld.createFolder destinationFolder
			
			If fld.hasError Then
				errors = fld.getError
				setError "%s", Array(errors(0)), Array(errors(1))
				Set fld = nothing	
				Exit Sub
			End If
			Set fld = nothing
		
			If right(destinationFolder,1) <> "\" Then
				destinationFolder = destinationFolder & "\"
			End If
		End If
		outputPath = destinationFolder  & destinationFileName	

		Dim i
		For i=0 to ubound(orderLib)
			lib = orderLib(i)

			Select Case lib
			Case ".net"
				If isAvailableDotNET() Then
					GenerateAndWriteSecurityFile outputPath
					post = "command=resize"
					post = post & "&PathToSecurityFile=" & Server.URLEncode(PathToSecurityFile)
					post = post & "&source=" & Server.URLEncode(sourceFilePath)
					post = post & "&destination=" & Server.URLEncode(outputPath)
					post = post & "&width=" & Server.URLEncode(width)
					post = post & "&height=" & Server.URLEncode(height)
					post = post & "&keepproportion=" & Server.URLEncode(keepProportion)
					ExecCommand_DotNET (post)
					DeleteSecurityFile
					Exit Sub
				End If		

			Case "imagemagick"
				If IsAvailableIMKShell() Then
					If Cstr(width) = "0" Then
						width =""
					End If
					If Cstr(height) = "0" Then
						height =""
					End If
					If keepProportion Then
						propSign = ">"
					Else
						propSign = "!"
					End If
					ExecCommand_IMKShell "convert.exe", Array("-sample", width & "x" & height & propSign,   """" & sourceFilePath & """",  """" & outputPath & """")
					Exit Sub
				End If
			End Select
		Next

		If Not available_DotNET and Not available_IMKShell Then
			clearError
			setError "ASP_IMAGE_COMPONENTS_NOT_AVAILABLE", Array(), Array(InitErrors_DotNET & "<br />" & InitErrors_IMKShell)
			Exit Sub
		End If		
	End Sub

	' ====== CROP =====	
	Public Sub Crop (sourceFilePath, x, y,  width, height) 
		Dim i
		For i=0 to ubound(orderLib)
			lib = orderLib(i)

			Select Case lib
			Case ".net"
				If isAvailableDotNET() Then
					GenerateAndWriteSecurityFile sourceFilePath
					post = "command=crop"
					post = post & "&PathToSecurityFile=" & Server.URLEncode(PathToSecurityFile)
					post = post & "&source=" & Server.URLEncode(sourceFilePath)
					post = post & "&destination=" & Server.URLEncode(sourceFilePath)
					post = post & "&x=" & Server.URLEncode(x)
					post = post & "&y=" & Server.URLEncode(y)
					post = post & "&width=" & Server.URLEncode(width)
					post = post & "&height=" & Server.URLEncode(height)
					ExecCommand_DotNET (post)
					DeleteSecurityFile
					Exit Sub
				End If	

			Case "imagemagick"
				If IsAvailableIMKShell() Then
					ExecCommand_IMKShell "convert.exe", Array("-crop", width & "x" & height & "+" & x & "+" & y, """" & sourceFilePath & """",  """" & sourceFilePath & """")
					Exit Sub
				End If
			End Select
		Next
		If Not available_DotNET and Not available_IMKShell Then
			clearError
			setError "ASP_IMAGE_COMPONENTS_NOT_AVAILABLE", Array(), Array(InitErrors_DotNET & "<br />" & InitErrors_IMKShell)
			Exit Sub
		End If		
	End Sub


	' ====== ADJUST QUALITY =====	
	Public Sub AdjustQuality (sourceFilePath, quality) 
		Dim i
		For i=0 to ubound(orderLib)
			lib = orderLib(i)

			Select Case lib
			Case ".net"
				If isAvailableDotNET() Then
					GenerateAndWriteSecurityFile sourceFilePath
					post = "command=adjustquality"
					post = post & "&PathToSecurityFile=" & Server.URLEncode(PathToSecurityFile)
					post = post & "&source=" & Server.URLEncode(sourceFilePath)
					post = post & "&destination=" & Server.URLEncode(sourceFilePath)
					post = post & "&quality=" & Server.URLEncode(quality)
					ExecCommand_DotNET (post)
					DeleteSecurityFile
					Exit Sub
				End If			
			Case "imagemagick"
				If IsAvailableIMKShell() Then
					ExecCommand_IMKShell "convert.exe", Array("-quality", quality , """" & sourceFilePath & """",  """" & sourceFilePath & """")
					Exit Sub
				End If
			End Select
		Next
		If Not available_DotNET and Not available_IMKShell Then
			clearError
			setError "ASP_IMAGE_COMPONENTS_NOT_AVAILABLE", Array(), Array(InitErrors_DotNET & "<br />" & InitErrors_IMKShell)
			Exit Sub
		End If		
	End Sub


	' ====== ROTATE =====	
	Public Sub Rotate (sourceFilePath, degree) 
		Dim i
		For i=0 to ubound(orderLib)
			lib = orderLib(i)

			Select Case lib
			Case ".net"
				If isAvailableDotNET() Then
					GenerateAndWriteSecurityFile sourceFilePath
					post = "command=rotate"
					post = post & "&PathToSecurityFile=" & Server.URLEncode(PathToSecurityFile)
					post = post & "&source=" & Server.URLEncode(sourceFilePath)
					post = post & "&destination=" & Server.URLEncode(sourceFilePath)
					post = post & "&degree=" & Server.URLEncode(degree)
					ExecCommand_DotNET (post)
					DeleteSecurityFile
					Exit Sub
				End If			
			Case "imagemagick"
				If IsAvailableIMKShell() Then
					ExecCommand_IMKShell "convert.exe", Array("-rotate", degree , """" & sourceFilePath & """",  """" & sourceFilePath & """")
					Exit Sub
				End If
			End Select
		Next
		If Not available_DotNET and Not available_IMKShell Then
			clearError
			setError "ASP_IMAGE_COMPONENTS_NOT_AVAILABLE", Array(), Array(InitErrors_DotNET & "<br />" & InitErrors_IMKShell)
			Exit Sub
		End If		
	End Sub


	' ====== FLIP =====	
	Public Sub Flip (sourceFilePath, direction) 
		Dim i
		For i=0 to ubound(orderLib)
			lib = orderLib(i)

			Select Case lib
			Case ".net"
				If isAvailableDotNET() Then
					GenerateAndWriteSecurityFile sourceFilePath
					post = "command=flip"
					post = post & "&PathToSecurityFile=" & Server.URLEncode(PathToSecurityFile)
					post = post & "&source=" & Server.URLEncode(sourceFilePath)
					post = post & "&destination=" & Server.URLEncode(sourceFilePath)
					post = post & "&direction=" & Server.URLEncode(direction)
					ExecCommand_DotNET (post)
					DeleteSecurityFile
					Exit Sub
				End If

			Case "imagemagick"
				If IsAvailableIMKShell() Then
					If lcase(direction) = "vertical" Then
						ExecCommand_IMKShell "convert.exe", Array("-flip", """" & sourceFilePath & """",  """" & sourceFilePath & """")
					Else
						ExecCommand_IMKShell "convert.exe", Array("-flop", """" & sourceFilePath & """",  """" & sourceFilePath & """")					
					End If
					Exit Sub
				End If
			End Select
		Next
		If Not available_DotNET and Not available_IMKShell Then
			clearError
			setError "ASP_IMAGE_COMPONENTS_NOT_AVAILABLE", Array(), Array(InitErrors_DotNET & "<br />" & InitErrors_IMKShell)
			Exit Sub
		End If		
	End Sub


	' ====== Sharpen =====	
	Public Sub Sharpen (sourceFilePath) 
		If Not IsAvailableIMKShell() Then
			If hasError() Then
				develErrorMessage = KT_array_push(develErrorMessage, KT_getResource("ASP_IMAGE_SHARPEN_NOT_AVAILABLE_D", "Image", array(InitErrors_IMKShell)))
			Else
				setError "ASP_IMAGE_SHARPEN_NOT_AVAILABLE", array(), Array(InitErrors_IMKShell & "<br>")
			End If	
			Exit Sub
		End If
		ExecCommand_IMKShell "convert.exe", Array("-sharpen", "3x1", """" & sourceFilePath & """",  """" & sourceFilePath & """")
	End Sub

	
	Private Sub GenerateAndWriteSecurityFile(path)
		If UseSecurityCheck Then
			Dim fso: Set fso = Server.CreateObject("Scripting.FileSystemObject")
			PathToSecurityFile = Replace(path,".","_") & "_" & Session.SessionID & "_" & fso.GetTempName & ".sec"
			On Error Resume Next
			Dim textStreamObject: Set textStreamObject = fso.CreateTextFile(PathToSecurityFile ,true) 
			textStreamObject.WriteLine(Cstr(len(PathToSecurityFile))) 
			textStreamObject.Close
			Set textStreamObject = Nothing 
			If err.number <> 0 Then
				error = err.Description
				setError "ASP_IMAGE_DOTNET_ERR_WRITE_SEC_FILE", array() , Array(PathToSecurityFile, error)
			End If
			On Error GoTo 0
			Set fso = Nothing 
		End If	
	End Sub		

	Private Sub DeleteSecurityFile
		If UseSecurityCheck Then
			Dim fso: Set fso = CreateObject("Scripting.FileSystemObject") 
			On Error Resume Next
			If fso.FileExists(PathToSecurityFile) Then
				fso.DeleteFile PathToSecurityFile
			End If
			If err.number <> 0 Then
				error = err.Description
				setError "ASP_IMAGE_DOTNET_ERR_DELETE_SEC_FILE", array() , Array(PathToSecurityFile, error)
			End If
			On Error GoTo 0
			Set fso = Nothing 
		End If	
	End Sub

	'---  generic command execution using .NET framework ---
	Private Function ExecCommand_DotNET (PostData)
		ExecCommand_DotNET = ""
		On Error Resume Next
		HTTPObj.open "POST", URLToDotNETImageComponent , false
		HTTPObj.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"

		HTTPObj.send PostData			
		If err.number <> 0  Then
			error = err.Description
			setError "ASP_IMAGE_DOTNET_EXEC_ERROR", Array(), Array(URLToDotNETImageComponent, error)
			userErrorMessage = Array(KT_getResource("ASP_IMAGE_CMD_GENERIC_ERROR", "Image", array()))			
			On Error GoTo 0
			Exit Function
		End If
		On Error GoTo 0

		strRetText = HTTPObj.ResponseText
		intRetStatus = HTTPObj.Status
		If intRetStatus = 200 Then
			If instr(strRetText, "ERROR") <> 0 Then
				errorparams = split(strRetText, "###")
				errorArr = array()
				errorKey = errorparams(0)
				If ubound(errorparams) = 1 Then
					errorArr = split(errorparams(1), "##")
				End If
				setError errorKey, Array(), errorArr
				userErrorMessage = Array(KT_getResource("ASP_IMAGE_CMD_GENERIC_ERROR", "Image", array()))
			Else
				ExecCommand_DotNET = strRetText		 
			End If			
		Else
			setError "ASP_IMAGE_DOTNET_EXEC_NON200STATUS", Array(), Array(intRetStatus, URLToDotNETImageComponent)		
			userErrorMessage = Array(KT_getResource("ASP_IMAGE_CMD_GENERIC_ERROR", "Image", array()))
		End If
	End Function
	
	'---  generic command execution Image Magick from Shell ---
	Private Function ExecCommand_IMKShell(cmd, ByRef params)
		IMKShell.clearError
		output = IMKShell.execute (Array(PathToIMKInstalationFolder & cmd), params)
		If IMKShell.hasError Then
			errors  = IMKShell.getError
            missingRightsError = KT_getResource("ASP_IMAGE_IMK_MISSING_RIGHTS_D", "Image", Array(PathToIMKInstalationFolder))
			setError "%s", array(errors(0)), array(errors(1) & "<br />" & missingRightsError)
			userErrorMessage = Array(KT_getResource("ASP_IMAGE_CMD_GENERIC_ERROR", "Image", array()))
		End If
		ExecCommand_IMKShell = output
	End Function


	Private Function IsAvailableIMKShell()
		IsAvailableIMKShell	= false
		If Not initialized_IMKShell Then
			initialized_IMKShell = true
			Set	IMKShell = new KT_Shell
			output = IMKShell.execute (Array(PathToIMKInstalationFolder & "convert.exe"), Array("-version"))
			If IMKShell.hasError Then
				available_IMKShell = false
				errors  = IMKShell.getError
				InitErrors_IMKShell = KT_getResource("ASP_IMAGE_IMK_INIT_ERROR_D", "Image", Array(PathToIMKInstalationFolder))
			Else
				If instr(1,output,"ImageMagick",1) <> 0 Then
					available_IMKShell = true
					IsAvailableIMKShell = true
				End If	
			End If
		Else
			IsAvailableIMKShell = available_IMKShell	
		End If
	End Function		

	
	Private Function IsAvailableDotNET()
		IsAvailableDotNET = false
		If Not initialized_DotNET Then
			initialized_DotNET = true
			' must init URLToImageComponent
			URLToDotNETImageComponent = KT_GetURLToResource("includes/common/lib/image/KT_ImageUtil.aspx")
			
			On Error resume Next
			Set HTTPObj = Server.CreateObject("MSXML2.ServerXMLHTTP")
			If err.number <> 0 Then
				Set HTTPObj = Server.CreateObject("Microsoft.XMLHTTP")
				err.Clear
			End If

			HTTPObj.open "GET", URLToDotNETImageComponent & "?command=TestImageComponent", false
			HTTPObj.send 
			' GET the response
			If err.number <> 0  Then
				' Something bad happend
				error  = err.Description
				InitErrors_DotNET = KT_getResource("ASP_IMAGE_DOTNET_INIT_ERROR_D", "Image", Array(RLToDotNETImageComponent, error))
				availableDotNET = false
				On Error GoTo 0
				Exit Function
			End If
			On Error GoTo 0

			strRetText = HTTPObj.ResponseText
			intRetStatus = HTTPObj.Status
			If intRetStatus = 200 Then
				If instr(strRetText, "TestImageComponent") <> 0 Then
					available_DotNET = true
					IsAvailableDotNET = true
					Exit Function
				End If
			End If
			
			If trim(strRetText) <> "" and instr(1,strRetText, "@ page language=""VB""",1) <> 0 Then
				InitErrors_DotNET = KT_getResource("ASP_IMAGE_DOTNET_NOT_INSTALLED_D", "Image", Array())
				available_DotNET = false
				Exit Function
			Else
				InitErrors_DotNET = KT_getResource("ASP_IMAGE_DOTNET_INIT_NON200STATUS_D", "Image", Array(intRetStatus, URLToDotNETImageComponent))
				available_DotNET = false
				Exit Function
			End If
		Else
			IsAvailableDotNET = available_DotNET
		End If	
	End Function




	Private Sub setError (errorCode, arrArgsUsr, arrArgsDev)
		errorCodeDev = errorCode
		If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
			errorCodeDev = errorCodeDev & "_D"
		End If
		If errorCode <> "" Then
			userErrorMessage = KT_array_push (userErrorMessage, KT_getResource(errorCode, "Image", arrArgsUsr))
		Else
			userErrorMessage = array()
		End If
		
		If errorCodeDev <> "" Then
			develErrorMessage = KT_array_push (develErrorMessage, KT_getResource(errorCodeDev, "Image", arrArgsDev))
		Else
			develErrorMessage = array()
		End If			
	End Sub

	Public Function hasError
		If ubound(userErrorMessage) > -1 Then
			hasError = True
		Else
			hasError = False	
		End If
	End Function
	

	Public Function getError
		getError = Array ( join(userErrorMessage,"<br />"), join (develErrorMessage, "<br />"))
	End Function

	Public Function clearError
		userErrorMessage = Array()
		develErrorMessage = Array()
	End Function
End Class
%>
