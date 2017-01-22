<%
Dim my_rs, KT_dependentDropdownSW
Dim KT_dynamic_value

dependentDropdown_jsscript_includes=false
KT_dd=0

function stripslashes(sir)
	retu = Replace(sir,"\'","'")
	retu = Replace(retu,"\""","""")
	retu = Replace(retu,"\\","\")
	stripslashes = retu
end function

Class browser
	public Name,Version,Platform
Public function init()
	HTTP_USER_AGENT=Request.ServerVariables("HTTP_USER_AGENT")
	Name="unknown"
	Version="unknown"
	Platform="unknown"
	ua=LCase(HTTP_USER_AGENT)

	If InStr(ua, "opera") then
		Name="Opera"
		Version = CInt(Mid(ua, InStr(ua, "opera") + 6, 1))
	else 
		If InStr(ua, "msie") then
			Name="msie"
			Version = CInt(Mid(ua, InStr(ua, "msie") + 5, 1))
		else 
			If InStr(ua, "safari") then
				name="safari"
			else 
				If InStr(ua, "gecko") then
					name="gecko"
				else 
					If InStr(ua, "konqueror") then
						name="konqueror"
					end if
				end if
			end if
		end if
	end if

	If InStr(ua, "windows") then
		Platform="windows"
	else 
		If InStr(ua, "linux") then
			Platform="linux"
	else 
		If InStr(ua, "mac") then
			Platform="mac"
	else 
		If InStr(ua, "unix") then
			Platform="unix"
	end if
	end if
	end if
	end if

End Function

End Class

Dim br
set br=new browser
br.init()

'indicates whether the scripts for the numericInput had been written or not to avoid multiple inclusion
numericInput_jsscript_includes=false
sub numericInput(widget_name, negative, allowfloat)
	ret = """ onkeyup=""return numericInput(this, event, '" & negative & "', '" & allowfloat & "');"" "
	ret = ret & " onblur=""return numericInput(this, event, '" & negative & "', '" & allowfloat & "');"" "
	ret = ret & " autocomplete=""off"" null="""
	if not numericInput_jsscript_includes then
		numericInput_jsscript_includes = true
		ret = ret & """><script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/numericInput.js""></script><z z=""" '<input type=""hidden"" b="""
		
	end if

	Response.Write ret
End sub

smartdate_jsscript_includes=false
sub smartdate(widget_name, mask)
	'browser dependent
	if br.Name = "msie" OR br.Name = "gecko" OR br.Name = "safari" THEN
		KT_smartdateSW=true
	else
		KT_smartdateSW=false
	end if
	
	mask = Replace(mask,"\","\\")
	mask = Replace(mask,"'","\'")
	ret = ""
	if KT_smartdateSW then 
		ret = ret & """ onblur=""editDateBlur(this, '" & mask & "')"" "
		'fix date format on focus, problem a first keypress and resulting date is not valid format -> value is deleted
		'ret = ret & " onfocus=""editDateBlur(this, '" & mask & "')"" "
		ret = ret & " onkeypress=""return editDatePre(this, '" & mask & "', event)"" "
		ret = ret & " onkeyup=""return editDate(this, '" & mask & "', event);"" "
		ret = ret & " autocomplete=""off"" "
		'output include jscript only once and only if browser suports it
		if not smartdate_jsscript_includes then
			smartdate_jsscript_includes = true
			ret = ret & "/><script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/smartdate.js""></script><input type=""hidden"" b="""
		else
			ret = ret & """"
		end if
	end if



	Response.Write ret
End sub

mask_jsscript_includes=false
sub mask(widget_name, maska)
	'browser dependent
	if br.Name = "msie" OR br.Name = "gecko" OR br.Name = "safari" THEN
		KT_maskSW=true
	else
		KT_maskSW=false
	end if
	'KT_maskSW=false
	ret = ""
	if KT_maskSW then 
		ret = ret & """ onkeypress=""return editMaskPre(this, '" & maska & "', event)"" "
		ret = ret & " onkeyup=""return editMask(this, '" & maska & "', event);"" "
		ret = ret & " onblur=""return editMask(this, '" & maska & "', event);"" "
		ret = ret & " autocomplete=""off"
		'output include jscript only once and only if browser suports it
		if (not mask_jsscript_includes) AND KT_maskSW then
			mask_jsscript_includes = true
			ret = ret & """/><script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/mask.js""></script><input type=""hidden"" b="""
		end if

	else
		ret = ret & "not compatible browser"
	end if

	Response.Write ret
End sub

'''''''''''''''''
'DPO 26-March-2004 NOTE TO SELF
'do not forget to strip the ID attribute  from the originating input tag in calendar.js, because otherwise it will be duplicated
''''''''''''''''
wcalendar_jsscript_includes=false
KT_cid=0
Sub wcalendar(widget_name, p_format, skin, p_language, p_label, mondayfirst, singleclick)
	sw = true
	if (br.Name <> "unknown") Then
		if (br.Name = "opera") Then
			if (br.Version < 7) Then
				sw = false
			End if
		else 
		if (br.Name = "msie") Then
			if (br.Platform = "mac") Then
				sw = false
			End if
		else 
			if (br.Name = "konqueror") Then
				sw = false
			end if
		end if
		end if
	else 
		sw = false
	end if

	if not sw then exit sub
	ret = ""
	
	if p_label="" then p_label = "..."
	if p_language="" then p_language = "en"
	if skin="" then skin = "system"
	if mondayFirst="" then mondayFirst = "false"
	if singleClick="" then singleClick = "true"

	Id = "wcal_" & KT_cid
	KT_cid = KT_cid + 1

	ret = ret & """ id=""" & Id & """/>"

	if  not wcalendar_jsscript_includes then
		ret = ret & "<script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/calendar.js""></script>"
		ret = ret & "<script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/calendar-setup.js""></script>"
		wcalendar_jsscript_includes = true
	end if

	ret = ret & "<script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/lang/calendar-" & p_language & ".js""></script>"
	ret = ret & "<link rel=""stylesheet"" type=""text/css"" media=""all"" href=""" & KT_relPath & "includes/widgets/calendar-" & skin & ".css"" title=""" & skin & """/>"
	ret = ret & "<input class=""smallText"" type=""button"" id=""" & Id & "_btn"" class=""mxw_cld"" value=""" & stripslashes(p_label) & """ >"

	ret = ret & "	<script type=""text/javascript"">" & vbNewLine
	ret = ret & "		Calendar.setup({" & vbNewLine
	ret = ret & "			inputField     :    """ & Id & """,      // id of the input field" & vbNewLine
	ret = ret & "			ifFormat       :    """ & p_format & """,  // format of the input field (even if hidden, this format will be honored)" & vbNewLine
	ret = ret & "			button         :    """ & Id & "_btn"",  // trigger button (well, IMG in our case)" & vbNewLine
	ret = ret & "			align          :    ""Bl"",           // alignment (defaults to ""Bl"")" & vbNewLine
	ret = ret & "			singleClick    :    " & singleClick & "," & vbNewLine
	ret = ret & "			mondayFirst	   :    " & mondayFirst & vbNewLine
	ret = ret & "		});" & vbNewLine
	ret = ret & "	</script>"

	ret = ret & "<input type=""hidden"" disabled=""true"
	Response.Write ret

End Sub


'''''''''''''''''''''''''''
' Dependent dropdown
'''''''''''''''''''''''''''
'''''''''''''''''
'DPO 29-March-2004 NOTE TO SELF
'do not forget to strip the ID attribute  from the originating input tag in dependentDropdown.js, because otherwise it will be duplicated
''''''''''''''''


sub dependentDropdown(widget_name)

	'browser dependent - disabled SB for unknown browsers or opera smaller than 7
	KT_dependentDropdownSW = true
	if br.Name <>"unknown" then
		if br.Name="opera" then 
			if br.Version<7 then
				KT_dependentDropdownSW=false
			end if
		end if
	else
		KT_dependentDropdownSW=false
	end if
	
	ret = ""
	if KT_dependentDropdownSW then 
		KT_dd = KT_dd + 1
		ret = ret & """ id=""" & widget_name & """ "
		ret = ret & " j="""
	end if

	'output include jscript only once and only if browser suports it
	if (not dependentDropdown_jsscript_includes) AND KT_dependentDropdownSW then
		dependentDropdown_jsscript_includes = true
		ret = ret & """><script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/dependentDropdown.js""></script>"
	end if

	if KT_dependentDropdownSW then
		ret = ret & "<z b="""
	end if

	Response.Write ret
End sub
Dim DynamicMarkup_BEGIN, DynamicMarkup_END
DynamicMarkup_BEGIN = "#exec#"
DynamicMarkup_END = "#/exec#"

function hasMyDynamicMarkup(value)
	if instr(value,DynamicMarkup_BEGIN) = 0 OR instr(value,DynamicMarkup_END) = 0 then
		stripMyDynamicMarkup = false
	else
		start_v = Len(DynamicMarkup_BEGIN)
		end_v = Len(value)-Len(DynamicMarkup_END)-start_v
		value = Mid(value,start_v+1, end_v)
		hasMyDynamicMarkup = true
	end if
end function

sub dependentDropdown_end(widget_name, boundto, p_recordset, pkey, fkey, p_display, p_selected)
	ret = ""
	KT_dynamic_value = ""
	
	ExecuteGlobal("set my_rs=" & p_recordset)
	my_rs.MoveFirst()
	
	if hasMyDynamicMarkup(p_selected) then	'hasMyDynamicMarkup(p_selected) will change the p_selected if markup found
		on error resume next
		ExecuteGlobal("KT_dynamic_value = " & p_selected)
		if err.number <> 0 then
			'Response.Write(err.description)
			KT_dynamic_value = "KT_dynamic_value error:" & err.description
		end if
		on error goto 0
	else
		KT_dynamic_value = p_selected
	end if
	
	if KT_dependentDropdownSW then
		ret = ret & "<script>" & vbNewLine
		ret = ret & "dddefaults_" & widget_name & " = new Array();" & vbNewLine
		ret = ret & "ddfks_" & widget_name & " = new Array();" & vbNewLine
		ret = ret & "ddnames_" & widget_name & " = new Array();" & vbNewLine
		js_value = Replace(KT_dynamic_value,"\","\\")
		js_value = Replace(js_value,"""","\""")
		ret = ret & "dddefval_" & widget_name & " = """ & js_value & """;" & vbNewLine
		
		while not my_rs.EOF
			ret = ret & "ddfks_" & widget_name & "[""" & my_rs.Fields(pkey) & """]=""" & my_rs.Fields(fkey) & """;" & vbNewLine
			vvalue = my_rs.Fields(p_display)
			vvalue = Replace(vvalue,"\", "\\")
			vvalue = Replace(vvalue, """", "\""")
			ret = ret & "ddnames_" & widget_name & "[""" & my_rs.Fields(pkey) & """]=""" & vvalue & """;" & vbNewLine
			my_rs.MoveNext()
		wend
		ret = ret & "var objsel=document.getElementById(""" & widget_name & """);" & vbNewLine
		ret = ret & "for(var i=0;i<objsel.options.length;i++){opti=objsel.options[i];dddefaults_" & widget_name & "[opti.value+""""]=opti.text+"""";}" & vbNewLine
		ret = ret & "initMenu(""" & widget_name & """, """ & boundto & """);" & vbNewLine
		ret = ret & "</script>" & vbNewLine
	else 
		while not my_rs.EOF
			vvalue = my_rs.Fields(pkey) & ""
			if p_selected=vvalue then ::s_selected="selected"::else ::s_selected=""::end if

			vvalue = my_rs.Fields(p_display) & ""
			vvalue = Replace(vvalue,"\", "\\")
			vvalue = Replace(vvalue, """", "\""")

			ret = ret & "<option value=""" & my_rs.Fields(pkey) & """ " & s_selected & ">" & vvalue & "</option>" & vbNewLine
			my_rs.MoveNext()
		wend
	end if

	my_rs.MoveFirst()
	Response.Write(ret)
end sub


'''''''''''''''''''''''''''
' n1dependentDropdown
'''''''''''''''''''''''''''
'''''''''''''''''
'DPO 29-March-2004 NOTE TO SELF
'do not forget to strip the ID attribute  from the originating input tag in n1dependentDropdown.js, because otherwise it may be duplicated
''''''''''''''''
n1dependentDropdown_jsscript_includes=false
sub n1dependentDropdown(widget_name, boundto, p_recordset, triggerrs, tpkey, tfkey, pkey, p_display)
	ret = ""

	ret = ret & """ id=""" & widget_name & """>" 
	if not n1dependentDropdown_jsscript_includes then
		n1dependentDropdown_jsscript_includes = true
		ret = ret & "<script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/n1dependentDropdown.js""></script>"
	end if

	ret = ret & "<script>" & vbNewLine
	ret = ret & "ddfks_" & widget_name & " = new Array();" & vbNewLine
	ret = ret & "ddnames_" & widget_name & " = new Array();" & vbNewLine
	p_selected=""
	ret = ret & "dddefval_" & widget_name & " = """ & p_selected & """;" & vbNewLine

	ExecuteGlobal("set my_rs=" & triggerrs)
	my_rs.MoveFirst()
	while NOT my_rs.EOF
		ret = ret & "ddfks_" & widget_name & "[""" & my_rs.Fields(tpkey) & """]=""" & my_rs.Fields(tfkey) & """;" & vbNewLine
		my_rs.MoveNext()
	wend


	ExecuteGlobal("set my_rs=" & p_recordset)
	my_rs.MoveFirst()
	while NOT my_rs.EOF
		vvalue = my_rs.Fields(p_display)
		vvalue = Replace(vvalue, "\", "\\")
		vvalue = Replace(vvalue, """", "\""")
		ret = ret & "ddnames_" & widget_name & "[""" & my_rs.Fields(pkey) & """]=""" & vvalue & """;" & vbNewLine
		my_rs.MoveNext()
	wend
	my_rs.MoveFirst()
	ret = ret & "registerN1Menu(""" & widget_name & """, """ & boundto & """);" & vbNewLine
	ret = ret & "</script><z z="""

	Response.Write(ret)
end sub

'''''''''''''''''''''''''''
' n1dependentDropdown
'''''''''''''''''''''''''''
'''''''''''''''''
'DPO 29-March-2004 NOTE TO SELF
'do not forget to strip the ID attribute  from the originating input tag in n1dependentDropdown.js, because otherwise it may be duplicated
''''''''''''''''
Dim sessData(3)
Dim sessInsTest(0)
dynamicInput_jsscript_includes=false
sub dynamicInput(widget_name, widget_id, p_style, p_connection, table_name, datasource, idfield, p_field, p_value, p_restrict, norec, addlabel, areyousuretext, submittext)
	on error resume next

	KT_dynamic_value = ""
	ret = ""
	'browser dependent
	if br.Name = "msie" AND br.Platform="windows" OR br.Name = "gecko" THEN
		KT_dynamicInputSW=true
	else
		KT_dynamicInputSW=false
	end if
	if (not dynamicInput_jsscript_includes) AND KT_dynamicInputSW then
		'initializare sessInsTest
		Session("sessInsTest")=sessInsTest
	end if

	if p_style="" then p_style="width:150px;"
	if widget_id="" then widget_id=widget_name 
	widget_id = Replace(widget_id,"[", "_")
	widget_id = Replace(widget_id,"]", "_")
	sqlTable = table_name
	
	sessInsTest2 =	Session("sessInsTest")

	if p_restrict<>"Yes" then
		'0 conn, 1 table, 2 idfield, 3 field
		sessData(0) = p_connection
		sessData(1) = sqlTable
		sessData(2) = idfield
		sessData(3) = p_field

		count_ses = UBound(sessInsTest2) + 1
		Redim Preserve sessInsTest2(count_ses-1)
		sessInsTest2(count_ses-1)=sessData
		Session("sessInsTest")=sessInsTest2
	end if

	if err.number <>0 then
		ret = ret & err.description 
	end if

	ExecuteGlobal("set my_rs=" & datasource)
	my_rs.MoveFirst()

	if hasMyDynamicMarkup(p_value) then	'hasMyDynamicMarkup(p_value) will change the p_value if markup found
		on error resume next
		ExecuteGlobal("a = " & p_value)
		if err.number <> 0 then
			'Response.Write(err.description)
			KT_dynamic_value = "KT_dynamic_value error."
		else
			KT_dynamic_value = a
		end if

		on error goto 0
	else
		KT_dynamic_value = p_value
	end if

if KT_dynamicInputSW then 
	if not dynamicInput_jsscript_includes then
		dynamicInput_jsscript_includes = true
		ret = ret & """ style=""display:none"" name="""" id="""" disabled></select><script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/dynamicInput.js""></script>"
	else 
		ret = ret & """ style=""display:none"" name="""" id="""" disabled></select>"
	end if
	if norec="" then norec=1000000

	ret = ret & "<script>" & VbNewLine
	ret = ret & "	var del_el=document.getElementById('" & widget_id & "');del_el.parentNode.removeChild(del_el);" & VbNewLine
	ret = ret & "	var " & widget_id & "_restrict = '" & p_restrict & "';" & VbNewLine
	ret = ret & "	var " & widget_id & "_norec = '" & norec & "';" & VbNewLine
	ret = ret & "	var " & widget_id & "_style = '" & p_style & "';" & VbNewLine
	ret = ret & "	var " & widget_id & "_edittype = 'E';" & VbNewLine
	ret = ret & "	var dynImp_submittext = '" & submittext & "';" & VbNewLine
	ret = ret & "	var dynImp_ext = 'asp';" & VbNewLine
	ret = ret & "	var table = '" & table_name & "';" & VbNewLine
	ret = ret & "	var " & widget_id & "_el = new Array(''" & VbNewLine


	dvvalue = KT_dynamic_value & ""
	dvtext = ""

	while not my_rs.EOF
		rs_id = my_rs.Fields(idfield) & ""
		rs_value = my_rs.Fields(p_field)
		rs_value = Replace(rs_value, "\", "\\")
		rs_value = Replace(rs_value, """", "\""")
		rs_value = Replace(rs_value, "<!--", "<! --")
		ret = ret & ", """ & rs_id & """, """ & rs_value & """"

		if rs_id = dvvalue OR dvvalue = "" then
			dvvalue = rs_id
			dvtext = my_rs.Fields(p_field)
		end if
		my_rs.MoveNext()
	wend
	dvvalue = Server.HTMLEncode(dvvalue)
	dvtext = Server.HTMLEncode(dvtext)
	
	ret = ret & ");" & VbNewLine & "</script>"

	ret = ret & "<input type=""text"" name=""" & widget_name & "_edit"" id=""" & widget_id & "_edit"" style=""" & p_style & """"
	ret = ret & " value=""" & dvtext & """ onblur=""di_onBlur(this, event);"" onKeyDown=""return di_inputKeyDown(this, event);"" onKeyPress=""return di_inputKeyPress(this, event);"" onKeyUp=""autoComplete(this, event);"" autocomplete=""off"">"

	ret = ret & "<input type=""button"" class=""mxw_v"" name=""" & widget_id & "_v"" id=""" & widget_id & "_v"" tabIndex=""-1"" value="""
	if br.Name = "msie" then
		ret = ret & "6"" style=""font-family: webdings; "
	else
		ret = ret & "v"" style="""
	end if
	ret = ret & "position:relative; left:-1px; top: -1px; height: 21px; width: 18px"""
	ret = ret & " onFocus=""di_vFocused('" & widget_id & "')"""
	ret = ret & " onClick=""return di_buttonPressed('" & widget_id & "')"">"

	if dvvalue = "" AND dvtext="" then:: bdisabled="" else :: bdisabled="disabled"::end if
	if p_restrict="Yes" then::bdisplay="none"::else::bdisplay=""::end if
	areyousuretext=Replace(areyousuretext,"'","\'")
	ret = ret & "<input type=""button"" class=""mxw_add"" id=""" & widget_id & "_add"" value=""" & addlabel & """ " & bdisabled & " onClick=""di_addElement(this, '" & KT_relPath & "', '" & UBound(sessInsTest2) & "', '" & areyousuretext & "')"" style=""display:" & bdisplay & """>"
	ret = ret & "<iframe id=""" & widget_id & "_iframe"" style=""display:none"" src=""" & KT_relPath & "includes/widgets/dynamicInput.asp""></iframe>"
	ret = ret & "<input name=""" & widget_name & """ id=""" & widget_id & """ type=""hidden"" value=""" & dvvalue & """>"
	if p_restrict <> "Yes" then
		ret = ret & "<script>di_updateForm(""" & widget_id & "_add"")</script>"
	end if

	ret = ret & "<select id="""" name="""" style=""display:none"" disabled=""true"" z="""
else
	ret = ret & """ name=""" & widget_name & """ id=""" & widget_id & """ style=""" & p_style & """>"
	dvvalue = KT_dynamic_value & ""
	while not my_rs.EOF
		rs_id = my_rs.Fields(idfield) & ""
		rs_value = my_rs.Fields(p_field) & ""
		if p_value=rs_id then ::s_selected="selected"::else ::s_selected=""::end if
		ret = ret & "<option " & s_selected & " value=""" & rs_id & """>" & rs_value & "</option>" & vbNewLine
		my_rs.MoveNext()
	wend
end if

	my_rs.MoveFirst()
	Response.Write(ret)
end sub

sub dynamicSearch(widget_name, widget_id, conn, datasource, p_field, p_value, norec, p_style)
	KT_dynamic_value = ""

	ret = ""
	'browser dependent
	if br.Name = "msie" AND br.Platform="windows" OR br.Name = "gecko" THEN
		KT_dynamicSearchSW=true
	else
		KT_dynamicSearchSW=false
	end if

if KT_dynamicSearchSW then 
	if p_style="" then p_style="width:150px;"
	if widget_id="" then widget_id=widget_name 
	widget_id = Replace(widget_id,"[", "_")
	widget_id = Replace(widget_id,"]", "_")

	if hasMyDynamicMarkup(p_value) then	'hasMyDynamicMarkup(p_selected) will change the p_selected if markup found
		on error resume next
		ExecuteGlobal("KT_dynamic_value = " & p_value)
		if err.number <> 0 then
			'Response.Write(err.description)
			KT_dynamic_value = "KT_dynamic_value error:" & err.description
		end if
		on error goto 0
	else
		KT_dynamic_value = p_value
	end if
	
	ret = ret & """ disabled=""true"" style=""display:none""><input type=""text"" name=""" & widget_id & "_edit"" id=""" & widget_id & "_edit""" & vbNewLine
	ret = ret & "				onblur=""di_onBlur(this, event);"" "  & vbNewLine
	ret = ret & "				onKeyDown=""return di_inputKeyDown(this, event);""" & vbNewLine
	ret = ret & "				onKeyPress=""return di_inputKeyPress(this, event);""" & vbNewLine
	ret = ret & "				onKeyUp=""autoComplete(this, event);""" & vbNewLine
	ret = ret & "				autocomplete=""off""" & vbNewLine
	ret = ret & "				style=""" & p_style & """" & vbNewLine
	ret = ret & "				value=""" & KT_dynamic_value & """>"
	if not dynamicInput_jsscript_includes then
		'yes, here is dynamicInput_jsscript_includes because it uses same js
		dynamicInput_jsscript_includes = true
		ret = ret & "<script language=""JavaScript"" src=""" & KT_relPath & "includes/widgets/dynamicInput.js""></script>"
	else 
		ret = ret & ""
	end if

	if norec="" then norec=100000

	ret = ret & "<script>" & vbNewLine
	ret = ret & "	var " & widget_id & "_drm = document.getElementById('" & widget_id & "');" & vbNewLine
	ret = ret & "	" & widget_id & "_drm.parentNode.removeChild(" & widget_id & "_drm);" & vbNewLine
	ret = ret & "	var " & widget_id & "_restrict = 'No';" & vbNewLine
	ret = ret & "	var " & widget_id & "_norec = '" & norec & "';" & vbNewLine
	ret = ret & "	var " & widget_id & "_style = '" & p_style & "';" & vbNewLine
	ret = ret & "	var " & widget_id & "_edittype = 'S';" & vbNewLine
	ret = ret & "	var " & widget_id & "_el = new Array(""""" & vbNewLine

	ExecuteGlobal("set my_rs=" & datasource)
	my_rs.MoveFirst()

	while not my_rs.EOF
		rs_value = my_rs.Fields(p_field)
		rs_value = Replace(rs_value, "\", "\\")
		rs_value = Replace(rs_value, """", "\""")
		rs_value = Replace(rs_value, "<!--", "<! --")
		ret = ret & ", """", """ & rs_value & """"
		my_rs.MoveNext()
	wend
	my_rs.MoveFirst()
	ret = ret & ");" & vbNewLine & "</script>"


	ret = ret & "<input type=""button"" class=""mxw_v"" name=""" & widget_id & "_v"" id=""" & widget_id & "_v"" tabIndex=""-1"" value="""
	if br.Name = "msie" then
		ret = ret & "6"" style=""font-family: webdings; "
	else
		ret = ret & "v"" style="""
	end if
	ret = ret & "position:relative; left:-1px; top: -1px; height: 21px; width: 18px"""
	ret = ret & " onFocus=""di_vFocused('" & widget_id & "')"""
	ret = ret & " onClick=""return di_buttonPressed('" & widget_id & "')"">"

	ret = ret & "<input type=""button"" class=""mxw_add"" id=""" & widget_id & "_add"" value=""fake"" disabled=""true"" style=""display:none"">" & vbNewLine
	ret = ret & "<input name=""" & widget_name & """ id=""" & widget_id & """ type=""hidden"" value=""" & KT_dynamic_value & """>"
	
else

end if
	
	ret = ret & "<z z="""
	Response.Write(ret)
end sub


%>