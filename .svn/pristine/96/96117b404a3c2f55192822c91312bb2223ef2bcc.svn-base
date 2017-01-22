<%@LANGUAGE="VBSCRIPT" CODEPAGE="1252"%>
<!--#include file="../../../Connections/INKSYSSQL.asp" -->
<!--#include file="../globalcode/rolecheck.asp"-->
<%

grantGegner = rolecheck(31,session("MM_UserID"))
grantKlientMD = rolecheck(27,session("MM_UserID"))
grantAuftraggeberMD = rolecheck(5,session("MM_UserID"))
grantLieferantenMD = rolecheck(223,session("MM_UserID"))
grantSpoolerMD = rolecheck(204,session("MM_UserID"))
grantAutomatischeVerarbeitung = rolecheck(263,session("MM_UserID"))
grantTarifeMD = rolecheck(248,session("MM_UserID"))
grantGegnerNeu = rolecheck(32,session("MM_UserID"))
grantKlientNeu = rolecheck(28,session("MM_UserID"))
grantAuftraggeberNeu = rolecheck(6,session("MM_UserID"))
grantLieferantenNeu = rolecheck(224,session("MM_UserID"))
grantInkassoAktNeu = rolecheck(253,session("MM_UserID"))
grantInterventionsAktNeu = rolecheck(58,session("MM_UserID"))
grantTaskNeu = rolecheck(158,session("MM_UserID"))
grantTerminNeu = rolecheck(190,session("MM_UserID"))
grantStundenNeu = rolecheck(77,session("MM_UserID"))
grantInvoiceNeu = rolecheck(24,session("MM_UserID"))
grantEingangsrechnungNeu = rolecheck(228,session("MM_UserID"))
grantBugsNeu = rolecheck(264,session("MM_UserID"))
grantBonitaetMD = rolecheck(46,session("MM_UserID"))
grantInkassoAktenMD = rolecheck(62,session("MM_UserID"))
grantInterventionsAktenMD = rolecheck(49,session("MM_UserID"))
grantWorkflowMD = rolecheck(258,session("MM_UserID"))
grantInkassoImport = rolecheck(63,session("MM_UserID"))
grantInkassoImportPeli = rolecheck(266,session("MM_UserID"))
grantGruppeFakt = rolecheck(196,session("MM_UserID"))
grantGruppeFibu = rolecheck(197,session("MM_UserID"))
grantGruppeLohn = rolecheck(257,session("MM_UserID"))
grantGruppeInkasso = rolecheck(267,session("MM_UserID"))
grantGruppeDetektei = rolecheck(268,session("MM_UserID"))
grantBugReportsMD = rolecheck(194,session("MM_UserID"))
grantGruppeDatenbankwartung = rolecheck(269,session("MM_UserID"))
grantDBLeerzeichenAuftraggeber = rolecheck(270,session("MM_UserID"))
grantDBLeerzeichenKlient = rolecheck(271,session("MM_UserID"))
grantDBLeerzeichenGegner = rolecheck(272,session("MM_UserID"))
grantServerinfo = rolecheck(273,session("MM_UserID"))
grantLogdatei = rolecheck(274,session("MM_UserID"))
grantIntImportKSV = rolecheck(61,session("MM_UserID"))
grantIntImportINKIS = rolecheck(61,session("MM_UserID"))
grantIntImportScore = rolecheck(59,session("MM_UserID"))
grantIntImportAlektum = rolecheck(61,session("MM_UserID"))
grantAnschrifterhebungMD = rolecheck(35,session("MM_UserID"))
grantAnschrifterhebungImport = rolecheck(37,session("MM_UserID"))
grantHTBEinstellungen = rolecheck(173,session("MM_UserID"))
grantGruppeBenutzerverwaltung = rolecheck(287,session("MM_UserID"))
grantGruppeBenutzerkonten = rolecheck(1,session("MM_UserID"))
grantGruppeRollen = rolecheck(187,session("MM_UserID"))
grantGruppeFunktionen = rolecheck(288,session("MM_UserID"))
grantMeldeerhebungMD = rolecheck(963,session("MM_UserID"))
grantArbeitgeberNeu = rolecheck(317,session("MM_UserID"))
grantArbeitgebererhebungMD = rolecheck(303,session("MM_UserID"))
grantGebDaterhebungMD = rolecheck(295,session("MM_UserID"))
grantWiAuskMD = rolecheck(299,session("MM_UserID"))
grantArbeitgeberMD = rolecheck(316,session("MM_UserID"))
grantMeldeImport = rolecheck(322,session("MM_UserID")) 
grantGothiaImport = rolecheck(367,session("MM_UserID"))
grantIntImportCF = rolecheck(373,session("MM_UserID"))
grantIntImportIS = rolecheck(377,session("MM_UserID"))
grantIntImportAktiva = rolecheck(379,session("MM_UserID"))
grantImportBon = rolecheck(386,session("MM_UserID"))
grantImportIntAkzepta = rolecheck(391,session("MM_UserID"))
grantImportIntECP = rolecheck(390,session("MM_UserID"))
grantImportIntIDH = rolecheck(389,session("MM_UserID"))
grantImportIntIS = rolecheck(392,session("MM_UserID"))
grantImportIntOko = rolecheck(393,session("MM_UserID"))
grantImportIntCommerz = rolecheck(394,session("MM_UserID"))
grantImportIDG = rolecheck(397,session("MM_UserID"))
grantIntImportAutoeinzuege = rolecheck(398,session("MM_UserID"))

response.write "s_hideTimeout=1;" & vbcrlf
response.write "s_subShowTimeout=1;" & vbcrlf
response.write "s_subMenuOffsetX=1;" & vbcrlf
response.write "s_subMenuOffsetY=-3;" & vbcrlf
response.write "s_keepHighlighted=true;" & vbcrlf
response.write "s_autoSELECTED=true;" & vbcrlf
response.write "s_autoSELECTEDItemsClickable=false;" & vbcrlf
response.write "s_autoSELECTEDTree=true;" & vbcrlf
response.write "s_autoSELECTEDTreeItemsClickable=true;" & vbcrlf
response.write "s_scrollingInterval=30;" & vbcrlf
response.write "s_rightToLeft=false;" & vbcrlf
response.write "s_hideSELECTsInIE=true;" & vbcrlf & vbcrlf

response.write "s_target='self';"& vbcrlf & vbcrlf


response.write "s_CSSMain=["& vbcrlf
response.write "'#999999',"& vbcrlf
response.write "'#999999',"& vbcrlf
response.write "1," & vbcrlf
response.write "'#FBFBFB',"& vbcrlf
response.write "2,"& vbcrlf
response.write "'#FBFBFB',"& vbcrlf
response.write "'#cccccc',"& vbcrlf
response.write "'#000000',"& vbcrlf
response.write "'#000000',"& vbcrlf
response.write "'verdana,arial,helvetica,sans-serif',"& vbcrlf
response.write "s_iE&&!s_iE4&&!s_mC?'0.6em':'9px',"& chr(13)
response.write "'1',"& chr(13)
response.write "'normal',"& chr(13)
response.write "'left',"& chr(13)
response.write "2,"& chr(13)
response.write "0,"& chr(13)
response.write "'#999999',"& chr(13)
response.write "'progid:DXImageTransform.Microsoft.Shadow(color=""#777777"",Direction=135,Strength=3)',"& chr(13)
response.write "true,"& chr(13)
response.write "'../menu/black_arrow.gif',"& chr(13)
response.write "'../menu/black_arrow.gif',"& chr(13)
response.write "7,"& chr(13)
response.write "7,"& chr(13)
response.write "6,"& chr(13)
response.write "'#ffffff',"& chr(13)
response.write "'#000000',"& chr(13)
response.write "'../menu/black_arrow.gif',"& chr(13)
response.write "true,"& chr(13)
response.write "'../menu/scrolltop.gif',"& chr(13)
response.write "'../menu/scrollbottom.gif',"& chr(13)
response.write "68,"& chr(13)
response.write "12," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "1," & vbcrlf		
response.write "'#FBFBFB'," & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "2," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "''" & vbcrlf
response.write "];" & vbcrlf
response.write "s_CSSMain2=[" & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "1," & vbcrlf
response.write "'#FBFBFB'," & vbcrlf
response.write "2," & vbcrlf
response.write "'#FBFBFB'," & vbcrlf
response.write "'#cccccc'," & vbcrlf
response.write "'#000000'," & vbcrlf
response.write "'#000000'," & vbcrlf
response.write "'verdana,arial,helvetica,sans-serif'," & vbcrlf
response.write "s_iE&&!s_iE4&&!s_mC?'0.6em':'9px'," & vbcrlf
response.write "'1'," & vbcrlf
response.write "'normal'," & vbcrlf
response.write "'left'," & vbcrlf
response.write "2," & vbcrlf
response.write "0," & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "'progid:DXImageTransform.Microsoft.Shadow(color=""#777777"",Direction=135,Strength=3)'," & vbcrlf
response.write "true," & vbcrlf
response.write "'../menu/black_arrow.gif'," & vbcrlf
response.write "'../menu/black_arrow.gif'," & vbcrlf
response.write "7," & vbcrlf
response.write "7," & vbcrlf
response.write "6," & vbcrlf
response.write "'#ffffff'," & vbcrlf
response.write "'#000000'," & vbcrlf
response.write "'../menu/black_arrow.gif'," & vbcrlf
response.write "true," & vbcrlf
response.write "'../menu/scrolltop.gif'," & vbcrlf
response.write "'../menu/scrollbottom.gif'," & vbcrlf
response.write "68," & vbcrlf
response.write "12," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "1," & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "'#999999'," & vbcrlf
response.write "2," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "''," & vbcrlf
response.write "''" & vbcrlf
response.write "];" & vbcrlf & vbcrlf




response.write "function s_getStart(a){" & vbcrlf
'
response.write "var bodyMarginTop=0;" & vbcrlf
response.write "var bodyMarginLeft=0;" & vbcrlf
'
response.write "var o=document.images[""getStart""];if(!o)return a==""x""?-630:0;" & vbcrlf
'
response.write "if(s_nS4)return a==""x""?o.x:o.y;" & vbcrlf
response.write "var oP,oC,ieW;oP=o.offsetParent;oC=a==""x""?o.offsetLeft:o.offsetTop;" & vbcrlf
response.write "ieW=s_iE&&!s_mC?1:0;" & vbcrlf
response.write "while(oP){if(ieW&&oP.tagName&&oP.tagName.toLowerCase()==""table""&&oP.border&&oP.border>0)oC++;oC+=a==""x""?oP.offsetLeft:oP.offsetTop;oP=oP.offsetParent};" & vbcrlf
response.write "return s_oP7m||s_kN31p&&!s_kN32p||s_iE5M?a==""x""?s_oP7m?oC:oC+bodyMarginLeft:oC+bodyMarginTop:oC};" & vbcrlf
'
'
'
'
'response.write "function s_whilePageLoading(){if(typeof s_ML==""undefined""){setTimeout(""s_whilePageLoading()"",1000);return};var px=s_oP7m||s_nS4?0:""px"",os=null,x,y,i,S;for(i=0;i<s_P.length;i++){S=s_[s_P[i]][0];if(typeof(S.T)==""number""&&typeof(S.L)==""number"")continue;os=s_nS4?document.layers[""s_m""+s_P[i]]:s_getOS(""s_m""+s_P[i]);os.left=eval(S.L)+px;os.top=eval(S.T)+px};if(typeof s_Bl==""undefined"")setTimeout(""s_whilePageLoading()"",1000)};s_whilePageLoading();s_ol=window.onload?window.onload:function(){};window.onload=function(){setTimeout('s_Bl=1',3000);s_ol()}"
 lessthen = "<"
 'response.write "function s_whilePageLoading(){if(typeof s_ML==""undefined""){setTimeout(""s_whilePageLoading()"",1000);return};var px=s_oP7m||s_nS4?0:""px"",os=null,x,y,i,S;for(i=0;i" & lessthen & "s_P.length;i++){S=s_[s_P[i]][0];if(typeof(S.T)==""number""&&typeof(S.L)==""number"")continue;os=s_nS4?document.layers[""s_m""+s_P[i]]:s_getOS(""s_m""+s_P[i]);os.left=eval(S.L)+px;os.top=eval(S.T)+px};if(typeof s_Bl==""undefined"")setTimeout(""s_whilePageLoading()"",1000)};s_whilePageLoading();s_ol=window.onload?window.onload:function(){};window.onload=function(){setTimeout('s_Bl=1',3000);s_ol()}"
'-------------------------------------------------------------------------------------------------------------	
' Start
'-------------------------------------------------------------------------------------------------------------	
response.write "s_add(" & vbcrlf
response.write "{" & vbcrlf
response.write "N:'start'," & vbcrlf
response.write "LV:1," & vbcrlf
response.write "W:120," & vbcrlf
response.write "T:'s_getStart(""y"")'," & vbcrlf
response.write "L:'s_getStart(""x"")'," & vbcrlf
response.write "P:true," & vbcrlf
response.write "S:s_CSSMain2," & vbcrlf
response.write "BW:0," & vbcrlf
response.write "PD:5," & vbcrlf
response.write "IEF:''" & vbcrlf
response.write "}," & vbcrlf & vbcrlf

response.write "[" & vbcrlf
response.write "{U:'../intranet/intranetnew.asp',T:'&nbsp;Start'}" & vbcrlf
response.write "]" & vbcrlf
response.write ");" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Neu
'-------------------------------------------------------------------------------------------------------------	
response.write "s_add(" & vbcrlf
response.write "{" & vbcrlf
response.write "N:'main'," & vbcrlf
response.write "LV:1," & vbcrlf
response.write "W:120," & vbcrlf
response.write "T:'s_getStart(""y"")'," & vbcrlf
response.write "L:'s_getStart(""x"")'," & vbcrlf
response.write "P:true," & vbcrlf
response.write "S:s_CSSMain2," & vbcrlf
response.write "BW:0," & vbcrlf
response.write "PD:5," & vbcrlf
response.write "IEF:''" & vbcrlf
response.write "}," & vbcrlf & vbcrlf

response.write "[" & vbcrlf
response.write "{Show:'new',U:'',T:'&nbsp;Neu'}" & vbcrlf
response.write "]" & vbcrlf
response.write ");" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Neu DropDown
'-------------------------------------------------------------------------------------------------------------	
response.write "	s_add(" & vbcrlf
response.write "	{N:'new',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:14,P:false,S:s_CSSMain}," & vbcrlf
response.write "	[" & vbcrlf & vbcrlf
if grantArbeitgeberNeu = 1 then	
	response.write "	{U:'../arbeitgeber/newArbeitgeber.asp',T:'&nbsp;Arbeitgeber'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Arbeitgeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantAuftraggeberNeu = 1 then	
	response.write "	{U:'../auftraggeber/newauftraggeber.asp',T:'&nbsp;Auftraggeber'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Auftraggeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantGegnerNeu = 1 then	
	response.write "	{U:'../gegner/newgegner.asp',T:'&nbsp;Gegner'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Gegner',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if 
if grantKlientNeu = 1 then
	response.write "	{U:'../klienten/newklient.asp',T:'&nbsp;Klient'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Klient',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if 
if grantLieferantenNeu = 1 then
	response.write "	{U:'../lieferanten/newLieferant.asp?',T:'&nbsp;Lieferant',SeparatorSize:1}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Lieferant',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantInkassoAktNeu = 1 then
	response.write "	{U:'../aktenink/newaktink.asp',T:'&nbsp;Inkassoakt'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Inkassoakt',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantInterventionsAktNeu = 1 then
	response.write "	{U:'../aktenint/newaktint.asp',T:'&nbsp;Interventionsakt',SeparatorSize:1}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Interventionsakt',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantTaskNeu = 1 then
	response.write "	{U:'../tasks/newtask.asp',T:'&nbsp;Aufgabe'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Aufgabe',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantTerminNeu = 1 then
	response.write "	{U:'../kalender/newKEintrag.asp',T:'&nbsp;Termin'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Termin',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantStundenNeu = 1 then
	response.write "	{U:'../hours/newhours.asp',T:'&nbsp;Stundenabrechnung',SeparatorSize:1}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Stundenabrechnung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantInvoiceNeu = 1 then
	response.write "	{U:'../invoices/newinvoice1.asp',T:'&nbsp;Ausgangsrechnung'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Ausgangsrechnung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantEingangsrechnungNeu = 1 then
	response.write "	{U:'../eingangsrechnungen/newEingRechnung.asp',T:'&nbsp;Eingangsrechnung',SeparatorSize:1}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Eingangsrechnung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if 
if grantBugsNeu = 1 then
	response.write "	{U:'../bugs/newbug.asp',T:'&nbsp;Bugreport/Änderungswunsch'}" & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Bugreport/Änderungswunsch',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
end if
response.write "	]" & vbcrlf
response.write "	);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Programme
'-------------------------------------------------------------------------------------------------------------		
response.write "	s_add(" & vbcrlf
response.write "{" & vbcrlf
response.write "N:'main2',	// NAME" & vbcrlf
response.write "LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" & vbcrlf
response.write "W:120,		// MINIMAL WIDTH" & vbcrlf
response.write "T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "L:124,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)" & vbcrlf
response.write "S:s_CSSMain2,	// STYLE Array to use for this menu" & vbcrlf
response.write "BW:0,		// BORDER WIDTH" & vbcrlf
response.write "PD:5,		// PADDING" & vbcrlf
response.write "IEF:''		// IEFilter" & vbcrlf
response.write "}," & vbcrlf & vbcrlf

response.write "[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" & vbcrlf
response.write "{Show:'programme',U:'',T:'&nbsp;Programme'}" & vbcrlf
response.write "]" & vbcrlf
response.write ");" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Programme DropDown
'-------------------------------------------------------------------------------------------------------------	
response.write "	s_add(" & vbcrlf
response.write "	{N:'programme',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:129,P:false,S:s_CSSMain}," & vbcrlf
response.write "	[" & vbcrlf
if grantGruppeInkasso = 1 then
	response.write "	{Show:'inkasso',U:'',T:'&nbsp;Inkasso'},	 " & vbcrlf
else
	response.write "	{Show:'inkasso',U:'',T:'&nbsp;Inkasso',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'},	 " & vbcrlf
end if
if grantGruppeDetektei = 1 then
	response.write "	{Show:'detektei',U:'',T:'&nbsp;Detektei',SeparatorSize:1}," & vbcrlf
else
	response.write "	{Show:'detektei',U:'',T:'&nbsp;Detektei',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB',SeparatorSize:1}," & vbcrlf
end if
if grantGruppeFakt = 1 then
	response.write "	{U:'../intranet/fakt.asp',T:'&nbsp;Fakturierung'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Fakturierung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantGruppeFibu = 1 then
	response.write "	{U:'../intranet/fibu.asp',T:'&nbsp;Finanzbuchhaltung'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Finanzbuchhaltung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantGruppeLohn = 1 then
	response.write "	{U:'../intranet/lohn.asp',T:'&nbsp;Lohnverrechnung'}" & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Lohnverrechnung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
end if

response.write "	]" & vbcrlf
response.write "	);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Programme / Inkasso
'-------------------------------------------------------------------------------------------------------------	
response.write "		s_add(" & vbcrlf
response.write "		{N:'inkasso',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},"
response.write "		[" & vbcrlf

if grantBonitaetMD = 1 then
	response.write "		{U:'../bonitaet/bonitaet.asp',T:'&nbsp;Bonitätsprüfungen'},"
else
	response.write "		{U:'',T:'&nbsp;Bonitätsprüfungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantInkassoAktenMD = 1 then
	response.write "		{U:'../aktenink/AktenStaff.asp',T:'&nbsp;Inkassoakten'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Inkassoakten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantInterventionsAktenMD = 1 then
	response.write "		{U:'../aktenint/aktenint.asp',T:'&nbsp;Interventionen',SeparatorSize:1}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Interventionen',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if
if grantWorkflowMD = 1 then
	response.write "		{U:'../workflow/default.asp',T:'&nbsp;Workflow',SeparatorSize:1}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Workflow',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if

if grantInkassoImport = 1 then
	response.write "		{Show:'import1',U:'',T:'&nbsp;Import'}" & vbcrlf & vbcrlf
else
	response.write "		{Show:'import1',U:'',T:'&nbsp;Import',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf	
end if

response.write "		]" & vbcrlf
response.write "		);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Programme / Detektei
'-------------------------------------------------------------------------------------------------------------	
response.write "		s_add(" & vbcrlf
response.write "		{N:'detektei',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},"
response.write "		[" & vbcrlf

if grantAnschrifterhebungMD = 1 then
	response.write "		{U:'../akten/GMA.asp',T:'&nbsp;Anschrifterhebung'}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Anschrifterhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantArbeitgebererhebungMD = 1 then
	response.write "		{U:'../arbeitgebererhebung/AGErh.asp',T:'&nbsp;Arbeitgebererhebung'}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Arbeitgebererhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantGebDaterhebungMD = 1 then
	response.write "		{U:'../geburtsdatenerhebung/gebdatakt.asp',T:'&nbsp;Geburtsdatenerhebung'}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Geburtsdatenerhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeerhebungMD = 1 then
	response.write "		{U:'../melde/melde.asp',T:'&nbsp;Meldeerhebung'}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Meldeerhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantWiAuskMD = 1 then
	response.write "		{U:'../wirtschaftsauskunft/WiAusk.asp',T:'&nbsp;Wirtschaftsauskunft',SeparatorSize:1}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Wirtschaftsauskunft',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if

if grantAnschrifterhebungImport = 1 then
	response.write "		{Show:'import2',U:'',T:'&nbsp;Import'}" & vbcrlf & vbcrlf
else
	response.write "		{Show:'import2',U:'',T:'&nbsp;Import',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf & vbcrlf
end if
response.write "		]" & vbcrlf
response.write "		);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Programme / Detektei / Import
'-------------------------------------------------------------------------------------------------------------		
response.write "			s_add(" & vbcrlf
response.write "			{N:'import2',LV:4,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," & vbcrlf
response.write "			[" & vbcrlf
if grantAnschrifterhebungImport = 1 then
	response.write "			{U:'../akten/Import.asp',T:'&nbsp;Anschrifterhebungen: Peli'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Anschrifterhebungen: Peli',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantAnschrifterhebungImport = 1 then
	response.write "			{U:'../akten/ImportAKAWGeneric.asp',T:'&nbsp;Anschrifterhebungen: AK',SeparatorSize:1}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Anschrifterhebungen: AK',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: ECP',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: ECP',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Infoscore',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Infoscore',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: IS Inkasso',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: IS Inkasso',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Intrum Justitia',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Intrum Justitia',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: KSV',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: KSV',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: OKO',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: OKO',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Plainfeld',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Plainfeld',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeImport = 1 then
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Universum',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
else
	response.write "			{U:'',T:'&nbsp;Meldeerhebungen: Universum',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
end if


response.write "			]" & vbcrlf
response.write "			);" & vbcrlf & vbcrlf

'-------------------------------------------------------------------------------------------------------------	
' Programme / Inkasso / Import
'-------------------------------------------------------------------------------------------------------------		
response.write "			s_add(" & vbcrlf
response.write "			{N:'import1',LV:4,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," & vbcrlf
response.write "			[" & vbcrlf
if grantIntImportAktiva = 1 then
	response.write "			{U:'../aktenint/ImportAktivaGeneric.asp',T:'&nbsp;Interventionen: Aktiva Inkasso GmbH.'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: Aktiva Inkasso GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantImportIntAkzepta = 1 then
	response.write "			{U:'../aktenint/ImportAkzeptaGeneric.asp',T:'&nbsp;Interventionen: Akzepta Inkasso GmbH.'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: Akzepta Inkasso GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantIntImportAlektum= 1 then
	response.write "			{U:'../aktenint/ImportAlektumGeneric.asp',SeparatorSize:1,T:'&nbsp;Interventionen: Alektum Inkasso'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',SeparatorSize:1,T:'&nbsp;Interventionen: Alektum Inkasso',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantIntImportAutoeinzuege = 1 then
	response.write "			{U:'../aktenint/ImportAutoeinzuege.asp',T:'&nbsp;Interventionen: Autoeinzuege'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: Autoeinzuege',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantImportIntCommerz = 1 then
	response.write "			{U:'../aktenint/ImportCommerzGeneric.asp',T:'&nbsp;Interventionen: Commerz Inkasso GmbH.'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: Commerz Inkasso GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantIntImportCF = 1 then
	response.write "			{U:'../aktenint/ImportCreditReform.asp',T:'&nbsp;Interventionen: Creditreform Kubicki KG'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: Creditreform Kubicki KG',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantImportIntECP = 1 then
	response.write "			{U:'../aktenint/ImportECPGeneric.asp',T:'&nbsp;Interventionen: E.C.P. OG'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: E.C.P. OG',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantImportIDG = 1 then
	response.write "			{U:'../aktenint/ImportIDGGeneric.asp',T:'&nbsp;Interventionen: IDG Inkasso Direkt'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: IDG Inkasso Direkt',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantImportIntIDH = 1 then
	response.write "			{U:'../aktenint/ImportIDHGeneric.asp',T:'&nbsp;Interventionen: IDH Inkassodienst GmbH.'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: IDH Inkassodienst GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantIntImportIS = 1 then
	response.write "			{U:'../aktenint/ImportInfoScoreNewGeneric.asp',T:'&nbsp;Interventionen: Infoscore Austria GmbH.'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: Infoscore Austria GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantImportIntIS= 1 then
	response.write "			{U:'../aktenint/ImportISGeneric.asp',T:'&nbsp;Interventionen: IS Inkasso Service GmbH.'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: IS Inkasso Service GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantIntImportKSV = 1 then
	response.write "			{U:'../aktenint/ImportKSVGeneric.asp',T:'&nbsp;Interventionen: KSV Allgemein'}," & vbcrlf & vbcrlf	
	response.write "			{U:'../aktenint/ImportKSVGenericDub.asp',T:'&nbsp;Interventionen: KSV Allgemein (Dubiosen)'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Interventionen: KSV Allgemein',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
	response.write "			{U:'',T:'&nbsp;Interventionen: KSV Allgemein (Dubiosen)',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if	
if grantImportIntOKO= 1 then
	response.write "			{U:'../aktenint/ImportOKOGeneric.asp',SeparatorSize:1,T:'&nbsp;Interventionen: OKO GmbH & Co.KG'}," & vbcrlf & vbcrlf	
else
	response.write "			{U:'',SeparatorSize:1,T:'&nbsp;Interventionen: OKO GmbH & Co.KG',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf & vbcrlf
end if
if grantImportBon = 1 then
	response.write "			{U:'../bonitaet/importBonOKO.asp',T:'&nbsp;Bonitätsprüfungen: OKO'}" & vbcrlf & vbcrlf	
else
	response.write "			{U:'',T:'&nbsp;Bonitätsprüfungen: OKO',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf & vbcrlf
end if

response.write "			]" & vbcrlf
response.write "			);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Akten
'-------------------------------------------------------------------------------------------------------------		
response.write "	s_add(" & vbcrlf
response.write "{		  " & vbcrlf
response.write "N:'main3',	// NAME" & vbcrlf
response.write "LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" & vbcrlf
response.write "W:120,		// MINIMAL WIDTH" & vbcrlf
response.write "T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "L:239,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)" & vbcrlf
response.write "S:s_CSSMain2,	// STYLE Array to use for this menu" & vbcrlf
response.write "BW:0,		// BORDER WIDTH" & vbcrlf
response.write "PD:5,		// PADDING" & vbcrlf
response.write "IEF:''		// IEFilter" & vbcrlf
response.write "}," & vbcrlf & vbcrlf

response.write "[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" & vbcrlf
response.write "{Show:'akten',U:'',T:'&nbsp;Akten'}" & vbcrlf
response.write "]" & vbcrlf
response.write ");" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Akten DropDown
'-------------------------------------------------------------------------------------------------------------	
response.write "	s_add(" & vbcrlf
response.write "	{N:'akten',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:244,P:false,S:s_CSSMain}," & vbcrlf
response.write "	[" & vbcrlf
if grantAnschrifterhebungMD = 1 then
	response.write "	{U:'../akten/GMA.asp',T:'&nbsp;Anschrifterhebungen'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Anschrifterhebungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantMeldeerhebungMD = 1 then
	response.write "	{U:'../melde/melde.asp',T:'&nbsp;Meldeerhebungen'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Meldeerhebungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantBonitaetMD = 1 then
	response.write "	{U:'../bonitaet/bonitaet.asp',T:'&nbsp;Bonitätsprüfungen'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Bonitätsprüfungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantInkassoAktenMD = 1 then
	response.write "	{U:'../aktenink/AktenStaff.asp',T:'&nbsp;Inkassoakten'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Inkassoakten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantInterventionsAktenMD = 1 then
	response.write "	{U:'../aktenint/aktenint.asp',T:'&nbsp;Interventionsakten'}" & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Interventionsakten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
end if
response.write "	]" & vbcrlf
response.write "	);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Stammdaten
'-------------------------------------------------------------------------------------------------------------	
response.write "	s_add(" & vbcrlf
response.write "{		  " & vbcrlf
response.write "N:'main4',	// NAME" & vbcrlf
response.write "LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" & vbcrlf
response.write "W:120,		// MINIMAL WIDTH" & vbcrlf
response.write "T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "L:354,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)" & vbcrlf
response.write "S:s_CSSMain2,	// STYLE Array to use for this menu" & vbcrlf
response.write "BW:0,		// BORDER WIDTH" & vbcrlf
response.write "PD:5,		// PADDING" & vbcrlf
response.write "IEF:''		// IEFilter" & vbcrlf
response.write "}," & vbcrlf & vbcrlf
response.write "[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" & vbcrlf
response.write "{Show:'stamm',U:'',T:'&nbsp;Stammdaten'}" & vbcrlf
response.write "]" & vbcrlf
response.write ");" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Stammdaten Dropdown
'-------------------------------------------------------------------------------------------------------------	
response.write "	s_add(" & vbcrlf
response.write "	{N:'stamm',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:359,P:false,S:s_CSSMain}," & vbcrlf
response.write "	[" & vbcrlf
if grantGruppeBenutzerverwaltung = 1 then
	response.write "	{Show:'benutzer',U:'',T:'&nbsp;Benutzerverwaltung',SeparatorSize:1},	 " & vbcrlf
else
	response.write "	{Show:'benutzer',U:'',T:'&nbsp;Benutzerverwaltung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'},	 " & vbcrlf
end if
if grantArbeitgeberMD = 1 then
	response.write "	{U:'../arbeitgeber/arbeitgeber.asp',T:'&nbsp;Arbeitgeber'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Arbeitgeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantAuftraggeberMD = 1 then
	response.write "	{U:'../auftraggeber/auftraggeber.asp',T:'&nbsp;Auftraggeber'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Auftraggeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantKlientMD = 1 then
	response.write "	{U:'../klienten/klienten.asp',T:'&nbsp;Klienten'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Klienten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantGegner = 1 then
	response.write "	{U:'../gegner/gegner.asp',T:'&nbsp;Gegner'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Gegner',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantLieferantenMD = 1 then
	response.write "	{U:'../lieferanten/lieferanten.asp',T:'&nbsp;Lieferanten',SeparatorSize:1}" & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Lieferanten',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
end if
response.write "	]" & vbcrlf
response.write "	);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Stammdaten / Benutzerverwaltung
'-------------------------------------------------------------------------------------------------------------	
response.write "		s_add(" & vbcrlf
response.write "		{N:'benutzer',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," & vbcrlf
response.write "		[" & vbcrlf
if grantGruppeBenutzerkonten = 1 then
	response.write "	{U:'../user/users.asp',T:'&nbsp;Benutzerkonten'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Benutzerkonten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantGruppeRollen = 1 then
	response.write "		{U:'../rollen/rollen.asp',T:'&nbsp;Rollen'}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Rollen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf
end if
if grantGruppeFunktionen = 1 then
	response.write "		{U:'../rollenFunkt/rollenFunktionen.asp',T:'&nbsp;Funktionen/Programme'}" & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Funktionen/Programme',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
end if

response.write "		]" & vbcrlf
response.write "		);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Einstellungen
'-------------------------------------------------------------------------------------------------------------	
response.write "s_add(" & vbcrlf
response.write "{		  " & vbcrlf
response.write "N:'main5',	// NAME" & vbcrlf
response.write "LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" & vbcrlf
response.write "W:120,		// MINIMAL WIDTH" & vbcrlf
response.write "T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "L:469,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)" & vbcrlf
response.write "S:s_CSSMain2,	// STYLE Array to use for this menu" & vbcrlf
response.write "BW:0,		// BORDER WIDTH" & vbcrlf
response.write "PD:5,		// PADDING" & vbcrlf
response.write "IEF:''		// IEFilter" & vbcrlf
response.write "}," & vbcrlf
response.write "[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" & vbcrlf
response.write "{Show:'setting',U:'',T:'&nbsp;Einstellungen'}" & vbcrlf
response.write "]" & vbcrlf
response.write ");" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Einstellungen Drop
'-------------------------------------------------------------------------------------------------------------	
response.write "	s_add(" & vbcrlf
response.write "	{N:'setting',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:474,P:false,S:s_CSSMain}," & vbcrlf
response.write "	[" & vbcrlf
response.write "	{Show:'my',U:'',T:'&nbsp;Meine Einstellungen',SeparatorSize:1},	" & vbcrlf
if grantTarifeMD = 1 then
	response.write "	{U:'../tarife/tarife.asp',T:'&nbsp;Tarife/Gebühren'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Tarife/Gebühren',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf		
end if
if grantHTBEinstellungen = 1 then
	response.write "	{U:'../server/editinksys.asp',T:'&nbsp;HTB Einstellungen'}" & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;HTB Einstellungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf
end if
response.write "	]" & vbcrlf
response.write "	);" & vbcrlf
response.write "		s_add(" & vbcrlf
response.write "		{N:'my',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," & vbcrlf
response.write "		[" & vbcrlf
response.write "		{U:'../mysettings/personalsettings.asp',T:'&nbsp;Meine persönlichen Einstellungen'}," & vbcrlf
response.write "		{U:'../mysettings/mail.asp',T:'&nbsp;Meine Mail Einstellungen'}," & vbcrlf
response.write "		{U:'../mysettings/inksys.asp',T:'&nbsp;Meine HTB Einstellungen',SeparatorSize:1}," & vbcrlf
response.write "		{U:'../mysettings/changepw.asp',T:'&nbsp;Mein Passwort ändern'}" & vbcrlf
response.write "		]" & vbcrlf
response.write "		);" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Zubehör
'-------------------------------------------------------------------------------------------------------------	
response.write "s_add(" & vbcrlf
response.write "{		  " & vbcrlf
response.write "N:'main6',	// NAME" & vbcrlf
response.write "LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" & vbcrlf
response.write "W:120,		// MINIMAL WIDTH" & vbcrlf
response.write "T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "L:584,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" & vbcrlf
response.write "P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)" & vbcrlf
response.write "S:s_CSSMain2,	// STYLE Array to use for this menu" & vbcrlf
response.write "BW:0,		// BORDER WIDTH" & vbcrlf
response.write "PD:5,		// PADDING" & vbcrlf
response.write "IEF:''		// IEFilter" & vbcrlf
response.write "}," & vbcrlf & vbcrlf
response.write "[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" & vbcrlf
response.write "{Show:'tools',U:'',T:'&nbsp;Zubehör'}" & vbcrlf
response.write "]" & vbcrlf
response.write ");" & vbcrlf & vbcrlf
'-------------------------------------------------------------------------------------------------------------	
' Zubehör Drop
'-------------------------------------------------------------------------------------------------------------	
response.write "	s_add(" & vbcrlf
response.write "	{N:'tools',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:589,P:false,S:s_CSSMain}," & vbcrlf
response.write "	[" & vbcrlf
if grantSpoolerMD = 1 then
	response.write "	{U:'../spooler/spooler.asp',T:'&nbsp;Druckdateien..'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Druckdateien..',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if
if grantAutomatischeVerarbeitung = 1 then
	response.write "	{U:'style_win2000.php',T:'&nbsp;Autom. Verarbeitungen..',SeparatorSize:1}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Autom. Verarbeitungen..',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if 
if grantGruppeDatenbankwartung = 1 then
	response.write "	{Show:'db',U:'',T:'&nbsp;Datenbankwartung',SeparatorSize:1},	" & vbcrlf
else
	response.write "	{Show:'db',U:'',T:'&nbsp;Datenbankwartung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'},	" & vbcrlf
end if
if grantBugReportsMD = 1 then
	response.write "	{U:'../bugs/bugs.asp',T:'&nbsp;Bugreports/Änderungswünsche'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Bugreports/Änderungswünsche',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if
if grantServerinfo = 1 then
	response.write "	{U:'../server/editserver.asp',T:'&nbsp;Server Info'}," & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Server Info',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if
if grantLogdatei = 1 then
	response.write "	{U:'../log/eventlog.txt',T:'&nbsp;Logdatei'}" & vbcrlf
else
	response.write "	{U:'',T:'&nbsp;Logdatei',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf	
end if
response.write "	]" & vbcrlf
response.write "	);" & vbcrlf
response.write "		s_add(" & vbcrlf
response.write "		{N:'db',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," & vbcrlf
response.write "		[" & vbcrlf
if grantDBLeerzeichenAuftraggeber = 1 then
	response.write "		{U:'../wartung/trailingspacesAuftragg.asp',T:'&nbsp;Leerzeichen entfernen Auftraggeber'}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Leerzeichen entfernen Auftraggeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if
if grantDBLeerzeichenGegner = 1 then
	response.write "		{U:'../wartung/trailingspaces.asp',T:'&nbsp;Leerzeichen entfernen Gegner'}," & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Leerzeichen entfernen Gegner',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," & vbcrlf	
end if
if grantDBLeerzeichenKlient = 1 then
	response.write "		{U:'../wartung/trailingspacesKlienten.asp',T:'&nbsp;Leerzeichen entfernen Klient'}" & vbcrlf
else
	response.write "		{U:'',T:'&nbsp;Leerzeichen entfernen Klient',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" & vbcrlf	
end if
response.write "		]" & vbcrlf
response.write "		);" & vbcrlf
%>