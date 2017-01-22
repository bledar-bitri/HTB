<%@LANGUAGE="VBSCRIPT" CODEPAGE="1252"%>
<!--#include file="utils.asp" -->
<%
	on error resume next
	id=Request.QueryString("id")
	el = Request.QueryString("el")
	text = Request.QueryString("text")

	if id="" or el="" or text="" then 
		Response.End()
	end if

	sessInsTest = Session("sessInsTest")

	vars = sessInsTest(CInt(id))
	
	file_path=Server.MapPath("../../Connections/" & vars(0) & ".asp")
	IncludeDynamicFile(file_path)

	if err then
		Response.Write(err.description)
		Response.End()
	end if

	Dim rs, Cnxn 
	Set Cnxn = Server.CreateObject("ADODB.Connection")
	ExecuteGlobal("strCnxn = MM_" & vars(0) & "_STRING")
	Cnxn.Open strCnxn


	sql = "insert into " & vars(1) & " (" & vars(3) & ") values ('" & Replace(text, "'","''" ) & "') "
	Cnxn.Execute sql

	if err then
		Response.Write(err.description)
		Response.End()
	end if

	sql = "select " & vars(2) & " as id FROM " & vars(1) & " where " & vars(3) & " = '" & Replace(text, "'","''" ) & "'"
	set rs=Cnxn.Execute(sql)
	rs.MoveFirst()
	newid=rs("id")

	if err then
		Response.Write(err.description)
		Response.End()
	end if
	text=Replace(text,"\","\\")
	text=Replace(text,"'","\'")
	
%>
<script>
	var el = parent.document.getElementById('<% Response.Write(el) %>');
	datasource = eval("parent." + el.NUghURlDXXp);
	datasource[datasource.length] = '<% Response.Write(newid) %>';
	datasource[datasource.length] = '<% Response.Write(text) %>';
	parent.di_sortDatasource(datasource);

	el.addButton.disabled = 'true';
	parent.di_syncSelection(el);
	el.focus();
</script>
