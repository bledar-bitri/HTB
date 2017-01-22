<%
Dim res: Set res = Server.CreateObject("Scripting.Dictionary")
res("E-Mail_ERROR_SENDING") = "Fehler beim Versand der E-Mail."
res("ASP_E-Mail_MISSING_OBJECT") = "Fehler beim Versand der E-Mail."
res("ASP_E-Mail_ERROR_NOCONFIG") = "Fehler beim Versand der E-Mail."
res("E-Mail_ERROR_SENDING_D") = "E-mail couldn't be sent. Error returned: %s ."
res("ASP_E-Mail_MISSING_OBJECT_D") = "Cannot send e-mails on this server because Collaborative Data Object used for sending e-mails was not found."
res("ASP_E-Mail_ERROR_NOCONFIG_D") = "Please use InterAKT Control Panel to configure the E-mail Settings."
%>