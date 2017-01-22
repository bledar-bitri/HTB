<%

'NAME:
'	KT_Shell
'DESCRIPTION:
'	Executes commands on server	

	Class KT_Shell
		Private userErrorMessage		'array		-	error message to be displayed as User Error
		Private develErrorMessage		'array		-	error message to be displayed as Developer Error
		
		Private commands
		Private arguments
		
		Private WshShell			 	'object that will be used to execute commands
		Private TimeOut
		
		Private Sub Class_Initialize()
			commands = Array()
			arguments = Array()
			userErrorMessage	= Array()
			develErrorMessage	= Array()
			Set WshShell  = nothing
			
			TimeOut = 5.0 ' seconds 
		End Sub

		Private Sub Class_terminate()
			Set WshShell = nothing
		End Sub
	
		
		Public Function execute(cmds, args)
			setCommands (cmds)
			setArguments (args)
			If hasError Then
				Exit Function
			End If	
			
			initShellObject
			If hasError Then
				Exit Function
			End If	
			
			u_error = ""
			d_error	= ""
			NoneExecuted = true
			
			On Error resume next
			For i=0 to ubound(commands) 
				command = commands(i) & " " & getArguments

				Err.Clear
				Set oExec    = WshShell.exec (command)
				If err.number = 0 Then
					' no error means the command was found and can be executed
	
					' wait for the program to finish.. busy waiting
					EndTime = Now() + TimeOut / (3600.0 * 24.0)
					Do While Now() < EndTime and oExec.Status = 0
						' loop 
					Loop
					If oExec.Status = 0 Then
						' still running.. must terminate it 
						If Not oExec.StdOut.AtEndOfStream Then
							execute = execute & oExec.StdOut.ReadAll
						End If

						If execute = "" Then
							oExec.Terminate
							u_error = KT_getResource("ASP_SHELL_EXEC_TERMINATE", "Shell", array())
							d_error = d_error & KT_getResource("ASP_SHELL_EXEC_TERMINATE_D", "Shell", Array(command, TimeOut))
						Else
							oExec.Terminate
							On Error GoTo 0
							Exit Function	
						End If		
					Else
						' command executed, check its status
						' check how it ended
 						 NoneExecuted = false
						 If Not oExec.StdOut.AtEndOfStream Then
							  execute = oExec.StdOut.ReadAll
							  On Error GoTo 0
							  Exit Function
						 End If
					
						 If Not oExec.StdErr.AtEndOfStream Then
							error = oExec.StdErr.ReadAll
							u_error = KT_getResource("ASP_SHELL_EXEC_ERROR", "Shell", Array())
							d_error = d_error & KT_getResource("ASP_SHELL_EXEC_ERROR_D", "Shell", Array(command, error))
						 End If
					End If
					
					Exit For ' stop from executing other commands from commands array
				Else
					' an error occured while trying to execute command. probably command was not found for the path given or invalid path format
					' log this error
					error = err.Description
					d_error = d_error & KT_getResource("ASP_SHELL_TRY_EXEC_ERROR_D", "Shell", Array(command, error)) 					
					Err.Clear
				End If
			Next
			On Error GoTo 0
			
			If u_error = "" Then
				If NoneExecuted Then
					u_error  = KT_getResource("ASP_SHELL_NOCMD_EXEC_OK", "Shell", Array())
					d_error  = KT_getResource("ASP_SHELL_NOCMD_EXEC_OK_D", "Shell", Array(d_error))
					setError "%s", Array(u_error), Array(d_error)
				End If	
			Else
				setError "%s", Array(u_error), Array(d_error)
			End If	
		End Function
		
		
		Private Sub initShellObject
			On Error resume next
			Set WshShell = Server.CreateObject("WScript.Shell") 
			If Err.Number<>0 Then
				error = err.Description
				setError "ASP_SHELL_MISSING_OBJ", Array(), Array(error) 						
			End If
			On Error GoTo 0 
		End Sub 

		Private Function getArguments
			getArguments = join (arguments, " ")
		End Function
				
		Private Sub setArguments (args)
			If isArray(args) Then
				arguments = args
			End If	
		End Sub
		
		Private Sub setCommands (cmds)
			If isArray(cmds) Then
				commands = cmds
			Else
				commands = Array(cmds)
			End If
		End Sub
	
		Private Sub setError (errorCode, arrArgsUsr, arrArgsDev)
			errorCodeDev = errorCode
			If Not KT_in_array(errorCodeDev, array("", "%s"), false) Then
				errorCodeDev = errorCodeDev & "_D"
			End If
			If errorCode <> "" Then
				userErrorMessage = KT_array_push (userErrorMessage, KT_getResource(errorCode, "Shell", arrArgsUsr))
			Else
				userErrorMessage = array()
			End If
			
			If errorCodeDev <> "" Then
				develErrorMessage = KT_array_push (develErrorMessage, KT_getResource(errorCodeDev, "Shell", arrArgsDev))
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
