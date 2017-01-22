<!-- #include file="../../common/lib/Resources/KT_Resources.asp"-->
<%
	d = "NXT"
%>
//Javascript NXT Resources
NXT_Messages = {};
NXT_Messages['are_you_sure_move'] = '<%= KT_escapeJS(KT_getResource("ARE_YOU_SURE_MOVE", d, null))%>';
NXT_Messages['Record_FH'] = '<%= KT_escapeJS(KT_getResource("Record_FH", d, null))%>';
