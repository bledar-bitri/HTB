<%
Dim res: Set res = Server.CreateObject("Scripting.Dictionary")
res("ASP_SHELL_TRY_EXEC_ERROR") = "Der Befehl konnte nicht ausgeführt werden."
res("ASP_SHELL_EXEC_ERROR") = "Der Befehl konnte nicht ausgeführt werden."
res("ASP_SHELL_EXEC_TERMINATE") = "Der Befehl konnte nicht ausgeführt werden."
res("ASP_SHELL_NOCMD_EXEC_OK") = "Der Befehl konnte nicht ausgeführt werden."
res("ASP_SHELL_MISSING_OBJ") = "Der Befehl konnte nicht ausgeführt werden."
res("ASP_SHELL_TRY_EXEC_ERROR_D") = "Error trying to execute shell command '%s'.<br />Error description: %s<br />"
res("ASP_SHELL_EXEC_ERROR_D") = "Error executing shell command '%s'.<br />Command returned: <pre>%s</pre><br />"
res("ASP_SHELL_EXEC_TERMINATE_D") = "Execution of shell command '%s' took more than %s seconds and has been stoped."
res("ASP_SHELL_NOCMD_EXEC_OK_D") = "No shell command could be executed. Here is a trace of the execution: <br />%s"
res("ASP_SHELL_MISSING_OBJ_D") = "Missing WScript.Shell object on server.<br />The error returned is: '%s'."
%>