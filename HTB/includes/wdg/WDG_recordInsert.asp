<!-- #include file="WDG.asp" -->
<%
'
'	Copyright (c) InterAKT Online 2000-2005
'
	Function printErrorScript()
		Response.Write("<" & "script>var isComplete = true;var isError = true;</script" & ">")
		Response.End()
	End Function
	
	id = Request.QueryString("id")
	el = Request.QueryString("el")
	text = Request.QueryString("text")

	If id = "" OR el = "" Then Response.End()
	If TypeName(Session("WDG_sessInsTest")) <> "Dictionary" Then Response.End()

	Dim WDG_sessInsTest, vars
	Set WDG_sessInsTest = Session("WDG_sessInsTest")

	id = CInt(id)
	If NOT KT_isset(WDG_sessInsTest(id)) Then Response.End()

	Set vars = WDG_sessInsTest(id)
	KT_LoadASPFiles Array("Connections\" & vars("conn") & ".asp")

	On Error Resume Next

	Dim rs, Cnxn 
	Set Cnxn = Server.CreateObject("ADODB.Connection")
	ExecuteGlobal("strCnxn = MM_" & vars("conn") & "_STRING")
	Cnxn.Open strCnxn

	If Err Then
		printErrorScript
	End If

	sql = "insert into " & vars("table") & " (" & vars("updatefield") & ") values ('" & Replace(text, "'","''" ) & "') "
	Cnxn.Execute sql

	If Err Then
		printErrorScript
	End If

	sql = "select " & vars("idfield") & " as id FROM " & vars("table") & " where " & vars("updatefield") & " = '" & Replace(text, "'","''" ) & "'"
	Set rs=Cnxn.Execute(sql)

	If Err Then
		printErrorScript
	End If

	newid=rs("id")

	text=Replace(text,"\","\\")
	text=Replace(text,"""","\""")
%><script>
	var isComplete = true;
	var updatedDynamicInput = parent[parent.$DYS_GLOBALOBJECT]['<%=el%>'];
	updatedDynamicInput.recordset.Insert({
		"<%=vars("idfield")%>":"<%=newid%>",
		"<%=vars("updatefield")%>":"<%=text%>"
		},
		parseInt(updatedDynamicInput._firstMatch, 10) + 1
	);
	//window.setTimeout(function() {
		updatedDynamicInput.addButton.disabled = true;
		updatedDynamicInput.oldinput.options.add(new Option("<%=text%>", "<%=newid%>"));
		updatedDynamicInput.oldinput.selectedIndex = updatedDynamicInput.oldinput.options.length - 1;
		updatedDynamicInput.sel.options.add(new Option("<%=text%>", "<%=newid%>"));
		updatedDynamicInput.sel.selectedIndex = updatedDynamicInput.sel.options.length - 1;
		updatedDynamicInput.oldinput.value = "<%=newid%>";
		updatedDynamicInput.newvalue = "<%=newid%>";
		parent.MXW_DynamicObject_syncSelection('<%=el%>', false, true);
		updatedDynamicInput.edit.focus();
	//}, 10);
</script>
