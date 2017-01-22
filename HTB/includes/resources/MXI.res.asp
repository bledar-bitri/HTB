<%
Dim res: Set res = Server.CreateObject("Scripting.Dictionary")
res("SQL_ERROR") = "<strong>Fehler bei Erstellung des Dynamic Include:</strong><br/>%s<br/><strong>SQL-Fehler:</strong><br/>%s<br/>"
res("MISSING_FIELD") = "<strong>Fehler bei Erstellung des Dynamic Include:</strong><br/>Das Feld '%s' ist nicht Bestandteil des Datensatzes.<br/>"
%>