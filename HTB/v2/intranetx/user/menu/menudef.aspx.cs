using System;
using HTB.v2.intranetx.util;

namespace HTB.intranetx.menu
{
    public partial class menudef : System.Web.UI.Page
    {
        bool _grantGegner;
        bool _grantKlientMd;
        bool _grantAuftraggeberMd;
        bool _grantLieferantenMd;
        bool _grantSpoolerMd;
        bool _grantAutomatischeVerarbeitung;
        bool _grantTarifeMd;
        bool _grantGegnerNeu;
        bool _grantKlientNeu;
        bool _grantAuftraggeberNeu;
        bool _grantLieferantenNeu;
        bool _grantInkassoAktNeu;
        bool _grantInterventionsAktNeu;
        bool _grantTaskNeu;
        bool _grantTerminNeu;
        bool _grantStundenNeu;
        bool _grantInvoiceNeu;
        bool _grantEingangsrechnungNeu;
        bool _grantBugsNeu;
        bool _grantBonitaetMd;
        bool _grantInkassoAktenMd;
        bool _grantInterventionsAktenMd;
        bool _grantWorkflowMd;
        bool _grantInkassoImport;
        bool _grantInkassoImportPeli;
        bool _grantGruppeFakt;
        bool _grantGruppeFibu;
        bool _grantGruppeLohn;
        bool _grantGruppeInkasso;
        bool _grantGruppeDetektei;
        bool _grantBugReportsMd;
        bool _grantGruppeDatenbankwartung;
        bool _grantDBLeerzeichenAuftraggeber;
        bool _grantDBLeerzeichenKlient;
        bool _grantDBLeerzeichenGegner;
        bool _grantServerinfo;
        bool _grantLogdatei;
        bool _grantIntImportKSV;
        bool _grantIntImportInkis;
        bool _grantIntImportScore;
        bool _grantIntImportAlektum;
        bool _grantAnschrifterhebungMd;
        bool _grantAnschrifterhebungImport;
        bool _grantHTBEinstellungen;
        bool _grantGruppeBenutzerverwaltung;
        bool _grantGruppeBenutzerkonten;
        bool _grantGruppeRollen;
        bool _grantGruppeFunktionen;
        bool _grantMeldeerhebungMd;
        bool _grantArbeitgeberNeu;
        bool _grantArbeitgebererhebungMd;
        bool _grantGebDaterhebungMd;
        bool _grantWiAuskMd;
        bool _grantArbeitgeberMd;
        bool _grantMeldeImport;
        bool _grantGothiaImport;
        bool _grantIntImportCf;
        bool _grantIntImportIs;
        bool _grantIntImportAktiva;
        bool _grantImportBon;
        bool _grantImportIntAkzepta;
        bool _grantImportIntECP;
        bool _grantImportIntIdh;
        bool _grantImportIntIs;
        bool _grantImportIntOko;
        bool _grantImportIntCommerz;
        bool _grantImportIdg;
        bool _grantIntImportAutoeinzuege;
        private bool _grantReports;
        private bool _grantBanken;
        private const bool GrantImportIntOKO = false;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            int wuserId = GlobalUtilArea.GetUserId(Session);
            
            if (wuserId < 0) return;

            _grantGegner = GlobalUtilArea.Rolecheck(31, wuserId);
            _grantKlientMd = GlobalUtilArea.Rolecheck(27, wuserId);
            _grantAuftraggeberMd = GlobalUtilArea.Rolecheck(5, wuserId);
            _grantLieferantenMd = GlobalUtilArea.Rolecheck(223, wuserId);
            _grantSpoolerMd = GlobalUtilArea.Rolecheck(204, wuserId);
            _grantAutomatischeVerarbeitung = GlobalUtilArea.Rolecheck(263, wuserId);
            _grantTarifeMd = GlobalUtilArea.Rolecheck(248, wuserId);
            _grantGegnerNeu = GlobalUtilArea.Rolecheck(32, wuserId);
            _grantKlientNeu = GlobalUtilArea.Rolecheck(28, wuserId);
            _grantAuftraggeberNeu = GlobalUtilArea.Rolecheck(6, wuserId);
            _grantLieferantenNeu = GlobalUtilArea.Rolecheck(224, wuserId);
            _grantInkassoAktNeu = GlobalUtilArea.Rolecheck(253, wuserId);
            _grantInterventionsAktNeu = GlobalUtilArea.Rolecheck(58, wuserId);
            _grantTaskNeu = GlobalUtilArea.Rolecheck(158, wuserId);
            _grantTerminNeu = GlobalUtilArea.Rolecheck(190, wuserId);
            _grantStundenNeu = GlobalUtilArea.Rolecheck(77, wuserId);
            _grantInvoiceNeu = GlobalUtilArea.Rolecheck(24, wuserId);
            _grantEingangsrechnungNeu = GlobalUtilArea.Rolecheck(228, wuserId);
            _grantBugsNeu = GlobalUtilArea.Rolecheck(264, wuserId);
            _grantBonitaetMd = GlobalUtilArea.Rolecheck(46, wuserId);
            _grantInkassoAktenMd = GlobalUtilArea.Rolecheck(62, wuserId);
            _grantInterventionsAktenMd = GlobalUtilArea.Rolecheck(49, wuserId);
            _grantWorkflowMd = GlobalUtilArea.Rolecheck(258, wuserId);
            _grantInkassoImport = GlobalUtilArea.Rolecheck(63, wuserId);
            _grantInkassoImportPeli = GlobalUtilArea.Rolecheck(266, wuserId);
            _grantGruppeFakt = GlobalUtilArea.Rolecheck(196, wuserId);
            _grantGruppeFibu = GlobalUtilArea.Rolecheck(197, wuserId);
            _grantGruppeLohn = GlobalUtilArea.Rolecheck(257, wuserId);
            _grantGruppeInkasso = GlobalUtilArea.Rolecheck(267, wuserId);
            _grantGruppeDetektei = GlobalUtilArea.Rolecheck(268, wuserId);
            _grantBugReportsMd = GlobalUtilArea.Rolecheck(194, wuserId);
            _grantGruppeDatenbankwartung = GlobalUtilArea.Rolecheck(269, wuserId);
            _grantDBLeerzeichenAuftraggeber = GlobalUtilArea.Rolecheck(270, wuserId);
            _grantDBLeerzeichenKlient = GlobalUtilArea.Rolecheck(271, wuserId);
            _grantDBLeerzeichenGegner = GlobalUtilArea.Rolecheck(272, wuserId);
            _grantServerinfo = GlobalUtilArea.Rolecheck(273, wuserId);
            _grantLogdatei = GlobalUtilArea.Rolecheck(274, wuserId);
            _grantIntImportKSV = GlobalUtilArea.Rolecheck(61, wuserId);
            _grantIntImportInkis = GlobalUtilArea.Rolecheck(61, wuserId);
            _grantIntImportScore = GlobalUtilArea.Rolecheck(59, wuserId);
            _grantIntImportAlektum = GlobalUtilArea.Rolecheck(61, wuserId);
            _grantAnschrifterhebungMd = GlobalUtilArea.Rolecheck(35, wuserId);
            _grantAnschrifterhebungImport = GlobalUtilArea.Rolecheck(37, wuserId);
            _grantHTBEinstellungen = GlobalUtilArea.Rolecheck(173, wuserId);
            _grantGruppeBenutzerverwaltung = GlobalUtilArea.Rolecheck(287, wuserId);
            _grantGruppeBenutzerkonten = GlobalUtilArea.Rolecheck(1, wuserId);
            _grantGruppeRollen = GlobalUtilArea.Rolecheck(187, wuserId);
            _grantGruppeFunktionen = GlobalUtilArea.Rolecheck(288, wuserId);
            _grantMeldeerhebungMd = GlobalUtilArea.Rolecheck(291, wuserId);
            _grantArbeitgeberNeu = GlobalUtilArea.Rolecheck(317, wuserId);
            _grantArbeitgebererhebungMd = GlobalUtilArea.Rolecheck(303, wuserId);
            _grantGebDaterhebungMd = GlobalUtilArea.Rolecheck(295, wuserId);
            _grantWiAuskMd = GlobalUtilArea.Rolecheck(299, wuserId);
            _grantArbeitgeberMd = GlobalUtilArea.Rolecheck(316, wuserId);
            _grantMeldeImport = GlobalUtilArea.Rolecheck(322, wuserId);
            _grantGothiaImport = GlobalUtilArea.Rolecheck(367, wuserId);
            _grantIntImportCf = GlobalUtilArea.Rolecheck(373, wuserId);
            _grantIntImportIs = GlobalUtilArea.Rolecheck(377, wuserId);
            _grantIntImportAktiva = GlobalUtilArea.Rolecheck(379, wuserId);
            _grantImportBon = GlobalUtilArea.Rolecheck(386, wuserId);
            _grantImportIntAkzepta = GlobalUtilArea.Rolecheck(391, wuserId);
            _grantImportIntECP = GlobalUtilArea.Rolecheck(390, wuserId);
            _grantImportIntIdh = GlobalUtilArea.Rolecheck(389, wuserId);
            _grantImportIntIs = GlobalUtilArea.Rolecheck(392, wuserId);
            _grantImportIntOko = GlobalUtilArea.Rolecheck(393, wuserId);
            _grantImportIntCommerz = GlobalUtilArea.Rolecheck(394, wuserId);
            _grantImportIdg = GlobalUtilArea.Rolecheck(397, wuserId);
            _grantIntImportAutoeinzuege = GlobalUtilArea.Rolecheck(398, wuserId);
            _grantReports = GlobalUtilArea.Rolecheck(409, wuserId);
            _grantBanken = GlobalUtilArea.Rolecheck(142, wuserId);

            WriteScript();
        }

        private void WriteScript()
        {

            Response.Write(@"s_hideTimeout=1;" + "\r\n");

            Response.Write(@"s_subShowTimeout=1;" + "\r\n");
            Response.Write(@"s_subMenuOffsetX=1;" + "\r\n");
            Response.Write(@"s_subMenuOffsetY=-3;" + "\r\n");
            Response.Write(@"s_keepHighlighted=true;" + "\r\n");
            Response.Write(@"s_autoSELECTED=true;" + "\r\n");
            Response.Write(@"s_autoSELECTEDItemsClickable=false;" + "\r\n");
            Response.Write(@"s_autoSELECTEDTree=true;" + "\r\n");
            Response.Write(@"s_autoSELECTEDTreeItemsClickable=true;" + "\r\n");
            Response.Write(@"s_scrollingInterval=30;" + "\r\n");
            Response.Write(@"s_rightToLeft=false;" + "\r\n");
            Response.Write(@"s_hideSELECTsInIE=true;" + "\r\n" + "\r\n");

            Response.Write(@"s_target='self';" + "\r\n" + "\r\n");


            Response.Write(@"s_CSSMain=[" + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"1," + "\r\n");
            Response.Write(@"'#FBFBFB'," + "\r\n");
            Response.Write(@"2," + "\r\n");
            Response.Write(@"'#FBFBFB'," + "\r\n");
            Response.Write(@"'#cccccc'," + "\r\n");
            Response.Write(@"'#000000'," + "\r\n");
            Response.Write(@"'#000000'," + "\r\n");
            Response.Write(@"'verdana,arial,helvetica,sans-serif'," + "\r\n");
            Response.Write(@"s_iE&&!s_iE4&&!s_mC?'0.6em':'9px'," + "\r\n");
            Response.Write(@"'1'," + "\r\n");
            Response.Write(@"'normal'," + "\r\n");
            Response.Write(@"'left'," + "\r\n");
            Response.Write(@"2," + "\r\n");
            Response.Write(@"0," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"'progid:DXImageTransform.Microsoft.Shadow(color=""#777777"",Direction=135,Strength=3)'," + "\r\n");
            Response.Write(@"true," + "\r\n");
            Response.Write(@"'../menu/black_arrow.gif'," + "\r\n");
            Response.Write(@"'../menu/black_arrow.gif'," + "\r\n");
            Response.Write(@"7," + "\r\n");
            Response.Write(@"7," + "\r\n");
            Response.Write(@"6," + "\r\n");
            Response.Write(@"'#ffffff'," + "\r\n");
            Response.Write(@"'#000000'," + "\r\n");
            Response.Write(@"'../menu/black_arrow.gif'," + "\r\n");
            Response.Write(@"true," + "\r\n");
            Response.Write(@"'../menu/scrolltop.gif'," + "\r\n");
            Response.Write(@"'../menu/scrollbottom.gif'," + "\r\n");
            Response.Write(@"68," + "\r\n");
            Response.Write(@"12," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"1," + "\r\n");
            Response.Write(@"'#FBFBFB'," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"2," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''" + "\r\n");
            Response.Write(@"];" + "\r\n");
            Response.Write(@"s_CSSMain2=[" + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"1," + "\r\n");
            Response.Write(@"'#FBFBFB'," + "\r\n");
            Response.Write(@"2," + "\r\n");
            Response.Write(@"'#FBFBFB'," + "\r\n");
            Response.Write(@"'#cccccc'," + "\r\n");
            Response.Write(@"'#000000'," + "\r\n");
            Response.Write(@"'#000000'," + "\r\n");
            Response.Write(@"'verdana,arial,helvetica,sans-serif'," + "\r\n");
            Response.Write(@"s_iE&&!s_iE4&&!s_mC?'0.6em':'9px'," + "\r\n");
            Response.Write(@"'1'," + "\r\n");
            Response.Write(@"'normal'," + "\r\n");
            Response.Write(@"'left'," + "\r\n");
            Response.Write(@"2," + "\r\n");
            Response.Write(@"0," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"'progid:DXImageTransform.Microsoft.Shadow(color=""#777777"",Direction=135,Strength=3)'," + "\r\n");
            Response.Write(@"true," + "\r\n");
            Response.Write(@"'../menu/black_arrow.gif'," + "\r\n");
            Response.Write(@"'../menu/black_arrow.gif'," + "\r\n");
            Response.Write(@"7," + "\r\n");
            Response.Write(@"7," + "\r\n");
            Response.Write(@"6," + "\r\n");
            Response.Write(@"'#ffffff'," + "\r\n");
            Response.Write(@"'#000000'," + "\r\n");
            Response.Write(@"'../../intranet/menu/black_arrow.gif'," + "\r\n");
            Response.Write(@"true," + "\r\n");
            Response.Write(@"'../menu/scrolltop.gif'," + "\r\n");
            Response.Write(@"'../menu/scrollbottom.gif'," + "\r\n");
            Response.Write(@"68," + "\r\n");
            Response.Write(@"12," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"1," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"'#999999'," + "\r\n");
            Response.Write(@"2," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''," + "\r\n");
            Response.Write(@"''" + "\r\n");
            Response.Write(@"];" + "\r\n" + "\r\n");

            Response.Write(@"function s_getStart(a){" + "\r\n");

            Response.Write(@"var bodyMarginTop=0;" + "\r\n");
            Response.Write(@"var bodyMarginLeft=0;" + "\r\n");

            Response.Write(@"var o=document.images[""getStart""];if(!o)return a==""x""?-630:0;" + "\r\n");

            Response.Write(@"if(s_nS4)return a==""x""?o.x:o.y;" + "\r\n");
            Response.Write(@"var oP,oC,ieW;oP=o.offsetParent;oC=a==""x""?o.offsetLeft:o.offsetTop;" + "\r\n");
            Response.Write(@"ieW=s_iE&&!s_mC?1:0;" + "\r\n");
            Response.Write(@"while(oP){if(ieW&&oP.tagName&&oP.tagName.toLowerCase()==""table""&&oP.border&&oP.border>0)oC++;oC+=a==""x""?oP.offsetLeft:oP.offsetTop;oP=oP.offsetParent};" + "\r\n");
            Response.Write(@"return s_oP7m||s_kN31p&&!s_kN32p||s_iE5M?a==""x""?s_oP7m?oC:oC+bodyMarginLeft:oC+bodyMarginTop:oC};" + "\r\n");

            //-------------------------------------------------------------------------------------------------------------	
            // Start
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"s_add(" + "\r\n");
            Response.Write(@"{" + "\r\n");
            Response.Write(@"N:'start'," + "\r\n");
            Response.Write(@"LV:1," + "\r\n");
            Response.Write(@"W:120," + "\r\n");
            Response.Write(@"T:'s_getStart(""y"")'," + "\r\n");
            Response.Write(@"L:'s_getStart(""x"")'," + "\r\n");
            Response.Write(@"P:true," + "\r\n");
            Response.Write(@"S:s_CSSMain2," + "\r\n");
            Response.Write(@"BW:0," + "\r\n");
            Response.Write(@"PD:5," + "\r\n");
            Response.Write(@"IEF:''" + "\r\n");
            Response.Write(@"}," + "\r\n" + "\r\n");

            Response.Write(@"[" + "\r\n");
            Response.Write(@"{U:'../../intranet/intranet/intranetnew.asp',T:'&nbsp;Start'}" + "\r\n");
            Response.Write(@"]" + "\r\n");
            Response.Write(@");" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Neu
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"s_add(" + "\r\n");
            Response.Write(@"{" + "\r\n");
            Response.Write(@"N:'main'," + "\r\n");
            Response.Write(@"LV:1," + "\r\n");
            Response.Write(@"W:120," + "\r\n");
            Response.Write(@"T:'s_getStart(""y"")'," + "\r\n");
            Response.Write(@"L:'s_getStart(""x"")'," + "\r\n");
            Response.Write(@"P:true," + "\r\n");
            Response.Write(@"S:s_CSSMain2," + "\r\n");
            Response.Write(@"BW:0," + "\r\n");
            Response.Write(@"PD:5," + "\r\n");
            Response.Write(@"IEF:''" + "\r\n");
            Response.Write(@"}," + "\r\n" + "\r\n");

            Response.Write(@"[" + "\r\n");
            Response.Write(@"{Show:'new',U:'',T:'&nbsp;Neu'}" + "\r\n");
            Response.Write(@"]" + "\r\n");
            Response.Write(@");" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            //  Neu DropDown
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"	{N:'new',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:14,P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"	[" + "\r\n" + "\r\n");
            if (_grantArbeitgeberNeu)
                Response.Write(@"	{U:'../../intranet/arbeitgeber/newArbeitgeber.asp',T:'&nbsp;Arbeitgeber'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Arbeitgeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantAuftraggeberNeu)
                Response.Write(@"	{U:'../auftraggeber/EditAuftraggeber.aspx',T:'&nbsp;Auftraggeber'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Auftraggeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGegnerNeu)
                Response.Write(@"	{U:'../gegner/NewGegner.aspx',T:'&nbsp;Gegner'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Gegner',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantKlientNeu)
                Response.Write(@"	{U:'../klienten/NewKlient.aspx',T:'&nbsp;Klient'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Klient',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantLieferantenNeu)
                Response.Write(@"	{U:'../../intranet/lieferanten/newLieferant.asp?',T:'&nbsp;Lieferant'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Lieferant',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGegnerNeu)
                Response.Write(@"	{U:'../lawyer/EditLawyer.aspx',T:'&nbsp;Rechtsanwalt',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Rechtsanwalt',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB',SeparatorSize:1}," + "\r\n");


            if (_grantInkassoAktNeu)
                Response.Write(@"	{U:'../../intranetx/aktenink/NewAktInk.aspx?RfrInkWrkAkt=Y',T:'&nbsp;Inkassoakt'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Inkassoakt',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantInterventionsAktNeu)
            {
                Response.Write(@"	{U:'../../intranet/aktenint/newaktint.asp',T:'&nbsp;Interventionsakt'}," + "\r\n");
                Response.Write(@"	{U:'../../intranetx/aktenint/EditAktIntAuto.aspx?RfrIntWrkAkt=Y',T:'&nbsp;Autoakt',SeparatorSize:1}," + "\r\n");
            }
            else
            {
                Response.Write(@"	{U:'',T:'&nbsp;Interventionsakt',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
                Response.Write(@"	{U:'',T:'&nbsp;Autoakt',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            }
            if (_grantTaskNeu)
                Response.Write(@"	{U:'../../intranet/tasks/newtask.asp',T:'&nbsp;Aufgabe'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Aufgabe',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantTerminNeu)
                Response.Write(@"	{U:'../../intranet/kalender/newKEintrag.asp',T:'&nbsp;Termin'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Termin',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantStundenNeu)
                Response.Write(@"	{U:'../../intranet/hours/newhours.asp',T:'&nbsp;Stundenabrechnung',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Stundenabrechnung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantInvoiceNeu)
                Response.Write(@"	{U:'../../intranet/invoices/newinvoice1.asp',T:'&nbsp;Ausgangsrechnung'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Ausgangsrechnung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantEingangsrechnungNeu)
                Response.Write(@"	{U:'../../intranet/eingangsrechnungen/newEingRechnung.asp',T:'&nbsp;Eingangsrechnung',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Eingangsrechnung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantBugsNeu)
                Response.Write(@"	{U:'../../intranet/bugs/newbug.asp',T:'&nbsp;Bugreport/Änderungswunsch'}" + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Bugreport/Änderungswunsch',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");

            Response.Write(@"	]" + "\r\n");
            Response.Write(@"	);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Programme
            //-------------------------------------------------------------------------------------------------------------		
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"{" + "\r\n");
            Response.Write(@"N:'main2',	// NAME" + "\r\n");
            Response.Write(@"LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" + "\r\n");
            Response.Write(@"W:120,		// MINIMAL WIDTH" + "\r\n");
            Response.Write(@"T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"L:124,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"P:true,		// menu is PERMANENT (you can only set true if (this is LEVEL 1 menu)" + "\r\n");
            Response.Write(@"S:s_CSSMain2,	// STYLE Array to use for this menu" + "\r\n");
            Response.Write(@"BW:0,		// BORDER WIDTH" + "\r\n");
            Response.Write(@"PD:5,		// PADDING" + "\r\n");
            Response.Write(@"IEF:''		// IEFilter" + "\r\n");
            Response.Write(@"}," + "\r\n" + "\r\n");

            Response.Write(@"[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" + "\r\n");
            Response.Write(@"{Show:'programme',U:'',T:'&nbsp;Programme'}" + "\r\n");
            Response.Write(@"]" + "\r\n");
            Response.Write(@");" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Programme DropDown
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"	{N:'programme',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:129,P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"	[" + "\r\n");
            if (_grantGruppeInkasso)
                Response.Write(@"	{Show:'inkasso',U:'',T:'&nbsp;CollectionInvoice'},	 " + "\r\n");
            else
                Response.Write(@"	{Show:'inkasso',U:'',T:'&nbsp;CollectionInvoice',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'},	 " + "\r\n");

            if (_grantGruppeDetektei)
                Response.Write(@"	{Show:'detektei',U:'',T:'&nbsp;Detektei',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"	{Show:'detektei',U:'',T:'&nbsp;Detektei',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB',SeparatorSize:1}," + "\r\n");
            
            if (_grantReports)
                Response.Write(@"	{Show:'reports',U:'',T:'&nbsp;Reports'},	 " + "\r\n");
            else
                Response.Write(@"	{Show:'reports',U:'',T:'&nbsp;Reports',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'},	 " + "\r\n");

            if (_grantGruppeFakt)
                Response.Write(@"	{U:'../../intranet/intranet/fakt.asp',T:'&nbsp;Fakturierung'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Fakturierung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGruppeFibu)
                Response.Write(@"	{U:'../../intranet/intranet/fibu.asp',T:'&nbsp;Finanzbuchhaltung'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Finanzbuchhaltung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGruppeLohn)
                Response.Write(@"	{U:'../../intranet/intranet/lohn.asp',T:'&nbsp;Lohnverrechnung'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Lohnverrechnung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantBanken)
                Response.Write(@"	{U:'/v2/intranetx/bank/Bank.aspx',T:'&nbsp;Bankkonto'}" + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Bankkonto',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");
            

            Response.Write(@"	]" + "\r\n");
            Response.Write(@"	);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Programme / CollectionInvoice
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"		s_add(" + "\r\n");
            Response.Write(@"		{N:'inkasso',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},");
            Response.Write(@"		[" + "\r\n");
            
            if (_grantBonitaetMd)
                Response.Write(@"		{U:'../../intranet/bonitaet/bonitaet.asp',T:'&nbsp;Bonitätsprüfungen'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Bonitätsprüfungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantInkassoAktenMd)
                Response.Write(@"		{U:'../../intranet/aktenink/AktenStaff.asp',T:'&nbsp;Inkassoakten'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Inkassoakten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantInterventionsAktenMd)
                Response.Write(@"		{U:'../../intranet/aktenint/aktenint.asp',T:'&nbsp;Interventionen',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Interventionen',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantWorkflowMd)
                Response.Write(@"		{U:'../../intranet/workflow/default.asp',T:'&nbsp;Workflow',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Workflow',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantInkassoImport)
                Response.Write(@"		{Show:'import1',U:'',T:'&nbsp;Import'}" + "\r\n" + "\r\n");
            else
                Response.Write(@"		{Show:'import1',U:'',T:'&nbsp;Import',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");

            Response.Write(@"		]" + "\r\n");
            Response.Write(@"		);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Programme / Detektei
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"		s_add(" + "\r\n");
            Response.Write(@"		{N:'detektei',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},");
            Response.Write(@"		[" + "\r\n");

            if (_grantAnschrifterhebungMd)
                Response.Write(@"		{U:'../../intranet/akten/GMA.asp',T:'&nbsp;Anschrifterhebung'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Anschrifterhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantArbeitgebererhebungMd)
                Response.Write(@"		{U:'../../intranet/arbeitgebererhebung/AGErh.asp',T:'&nbsp;Arbeitgebererhebung'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Arbeitgebererhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGebDaterhebungMd)
                Response.Write(@"		{U:'../../intranet/geburtsdatenerhebung/gebdatakt.asp',T:'&nbsp;Geburtsdatenerhebung'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Geburtsdatenerhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeerhebungMd)
                Response.Write(@"		{U:'../../intranet/melde/melde.asp',T:'&nbsp;Meldeerhebung'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Meldeerhebung',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantWiAuskMd)
                Response.Write(@"		{U:'../../intranet/wirtschaftsauskunft/WiAusk.asp',T:'&nbsp;Wirtschaftsauskunft',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Wirtschaftsauskunft',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");


            if (_grantAnschrifterhebungImport)
                Response.Write(@"		{Show:'import2',U:'',T:'&nbsp;Import'}" + "\r\n" + "\r\n");
            else
                Response.Write(@"		{Show:'import2',U:'',T:'&nbsp;Import',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n" + "\r\n");

            Response.Write(@"		]" + "\r\n");
            Response.Write(@"		);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Programme / Reports
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"		s_add(" + "\r\n");
            Response.Write(@"		{N:'reports',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},");
            Response.Write(@"		[" + "\r\n");
            
            if (_grantReports)
                Response.Write(@"		{U:'../reports/ADStatistic.aspx',T:'&nbsp;Aussendienststatistik'}" + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Aussendienststatistik',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");

            /*
            if (grantReports)
                Response.Write(@"		{U:'../../intranet/reports/Aussendienststatistik.aspx',T:'&nbsp;Aussendienststatistik'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Aussendienststatistik',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (grantReports)
                Response.Write(@"		{U:'../../intranet/aktenink/AktenStaff.asp',T:'&nbsp;Inkassoakten'}" + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Inkassoakten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");
            */
            Response.Write(@"		]" + "\r\n");
            Response.Write(@"		);" + "\r\n" + "\r\n");

            //-------------------------------------------------------------------------------------------------------------	
            // Programme / Detektei / Import
            //-------------------------------------------------------------------------------------------------------------		
            Response.Write(@"			s_add(" + "\r\n");
            Response.Write(@"			{N:'import2',LV:4,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"			[" + "\r\n");
            if (_grantAnschrifterhebungImport)
                Response.Write(@"			{U:'../../intranet/akten/Import.asp',T:'&nbsp;Anschrifterhebungen: Peli'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Anschrifterhebungen: Peli',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantAnschrifterhebungImport)
                Response.Write(@"			{U:'../../intranet/akten/ImportAKAWGeneric.asp',T:'&nbsp;Anschrifterhebungen: AK',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Anschrifterhebungen: AK',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: ECP',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: ECP',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Infoscore',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Infoscore',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: IS CollectionInvoice',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: IS CollectionInvoice',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Intrum Justitia',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Intrum Justitia',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: KSV',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: KSV',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: OKO',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: OKO',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Plainfeld',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Plainfeld',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeImport)
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Universum',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Meldeerhebungen: Universum',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");



            Response.Write(@"			]" + "\r\n");
            Response.Write(@"			);" + "\r\n" + "\r\n");

            //-------------------------------------------------------------------------------------------------------------	
            // Programme / CollectionInvoice / Import
            //-------------------------------------------------------------------------------------------------------------		
            Response.Write(@"			s_add(" + "\r\n");
            Response.Write(@"			{N:'import1',LV:4,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"			[" + "\r\n");
            if (_grantIntImportAktiva)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportAktivaGeneric.asp',T:'&nbsp;Interventionen: Aktiva CollectionInvoice GmbH.'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: Aktiva CollectionInvoice GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantImportIntAkzepta)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportAkzeptaGeneric.asp',T:'&nbsp;Interventionen: Akzepta CollectionInvoice GmbH.'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: Akzepta CollectionInvoice GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantIntImportAlektum)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportAlektumGeneric.asp',SeparatorSize:1,T:'&nbsp;Interventionen: Alektum CollectionInvoice'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',SeparatorSize:1,T:'&nbsp;Interventionen: Alektum CollectionInvoice',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantIntImportAutoeinzuege)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportAutoeinzuege.asp',T:'&nbsp;Interventionen: Autoeinzuege'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: Autoeinzuege',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantImportIntCommerz)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportCommerzGeneric.asp',T:'&nbsp;Interventionen: Commerz CollectionInvoice GmbH.'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: Commerz CollectionInvoice GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantIntImportCf)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportCreditReform.asp',T:'&nbsp;Interventionen: Creditreform Kubicki KG'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: Creditreform Kubicki KG',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantImportIntECP)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportECPGeneric.asp',T:'&nbsp;Interventionen: E.C.P. OG'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: E.C.P. OG',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantImportIdg)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportIDGGeneric.asp',T:'&nbsp;Interventionen: IDG CollectionInvoice Direkt'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: IDG CollectionInvoice Direkt',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantImportIntIdh)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportIDHGeneric.asp',T:'&nbsp;Interventionen: IDH Inkassodienst GmbH.'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: IDH Inkassodienst GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantIntImportIs)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportInfoScoreNewGeneric.asp',T:'&nbsp;Interventionen: Infoscore Austria GmbH.'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: Infoscore Austria GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantImportIntIs)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportISGeneric.asp',T:'&nbsp;Interventionen: IS CollectionInvoice Service GmbH.'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: IS CollectionInvoice Service GmbH.',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantIntImportKSV)
            {
                Response.Write(@"			{U:'../aktenint/ImportKsv.aspx',T:'&nbsp;Interventionen: KSV Allgemein'}," + "\r\n" + "\r\n");
            }
            else
            {
                Response.Write(@"			{U:'',T:'&nbsp;Interventionen: KSV Allgemein',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");
            }
            if (GrantImportIntOKO)
                Response.Write(@"			{U:'../../intranet/aktenint/ImportOKOGeneric.asp',SeparatorSize:1,T:'&nbsp;Interventionen: OKO GmbH & Co.KG'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',SeparatorSize:1,T:'&nbsp;Interventionen: OKO GmbH & Co.KG',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantImportBon)
                Response.Write(@"			{U:'../../intranet/bonitaet/importBonOKO.asp',T:'&nbsp;Bonitätsprüfungen: OKO'}," + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;Bonitätsprüfungen: OKO',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n" + "\r\n");

            if (_grantIntImportKSV)
                Response.Write(@"			{U:'../aktenink/ImportFromXL.aspx',T:'&nbsp;CollectionInvoice: Excel Allgemein'}" + "\r\n" + "\r\n");
            else
                Response.Write(@"			{U:'',T:'&nbsp;CollectionInvoice: Excel Allgemein',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n" + "\r\n");
            

            Response.Write(@"			]" + "\r\n");
            Response.Write(@"			);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Akten
            //-------------------------------------------------------------------------------------------------------------		
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"{		  " + "\r\n");
            Response.Write(@"N:'main3',	// NAME" + "\r\n");
            Response.Write(@"LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" + "\r\n");
            Response.Write(@"W:120,		// MINIMAL WIDTH" + "\r\n");
            Response.Write(@"T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"L:239,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"P:true,		// menu is PERMANENT (you can only set true if (this is LEVEL 1 menu)" + "\r\n");
            Response.Write(@"S:s_CSSMain2,	// STYLE Array to use for this menu" + "\r\n");
            Response.Write(@"BW:0,		// BORDER WIDTH" + "\r\n");
            Response.Write(@"PD:5,		// PADDING" + "\r\n");
            Response.Write(@"IEF:''		// IEFilter" + "\r\n");
            Response.Write(@"}," + "\r\n" + "\r\n");

            Response.Write(@"[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" + "\r\n");
            Response.Write(@"{Show:'akten',U:'',T:'&nbsp;Akten'}" + "\r\n");
            Response.Write(@"]" + "\r\n");
            Response.Write(@");" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Akten DropDown
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"	{N:'akten',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:244,P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"	[" + "\r\n");
            if (_grantAnschrifterhebungMd)
                Response.Write(@"	{U:'../../intranet/akten/GMA.asp',T:'&nbsp;Anschrifterhebungen'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Anschrifterhebungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantMeldeerhebungMd)
                Response.Write(@"	{U:'../../intranet/melde/melde.asp',T:'&nbsp;Meldeerhebungen'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Meldeerhebungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantBonitaetMd)
                Response.Write(@"	{U:'../../intranet/bonitaet/bonitaet.asp',T:'&nbsp;Bonitätsprüfungen'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Bonitätsprüfungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantInkassoAktenMd)
                Response.Write(@"	{U:'../../intranet/aktenink/AktenStaff.asp',T:'&nbsp;Inkassoakten'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Inkassoakten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantInterventionsAktenMd)
                Response.Write(@"	{U:'../../intranet/aktenint/aktenint.asp',T:'&nbsp;Interventionsakten'}" + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Interventionsakten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");
            
            Response.Write(@"	]" + "\r\n");
            Response.Write(@"	);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Stammdaten
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"{		  " + "\r\n");
            Response.Write(@"N:'main4',	// NAME" + "\r\n");
            Response.Write(@"LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" + "\r\n");
            Response.Write(@"W:120,		// MINIMAL WIDTH" + "\r\n");
            Response.Write(@"T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"L:354,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"P:true,		// menu is PERMANENT (you can only set true if (this is LEVEL 1 menu)" + "\r\n");
            Response.Write(@"S:s_CSSMain2,	// STYLE Array to use for this menu" + "\r\n");
            Response.Write(@"BW:0,		// BORDER WIDTH" + "\r\n");
            Response.Write(@"PD:5,		// PADDING" + "\r\n");
            Response.Write(@"IEF:''		// IEFilter" + "\r\n");
            Response.Write(@"}," + "\r\n" + "\r\n");
            Response.Write(@"[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" + "\r\n");
            Response.Write(@"{Show:'stamm',U:'',T:'&nbsp;Stammdaten'}" + "\r\n");
            Response.Write(@"]" + "\r\n");
            Response.Write(@");" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Stammdaten Dropdown
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"	{N:'stamm',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:359,P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"	[" + "\r\n");
            if (_grantGruppeBenutzerverwaltung)
                Response.Write(@"	{Show:'benutzer',U:'',T:'&nbsp;Benutzerverwaltung',SeparatorSize:1},	 " + "\r\n");
            else
                Response.Write(@"	{Show:'benutzer',U:'',T:'&nbsp;Benutzerverwaltung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'},	 " + "\r\n");

            if (_grantArbeitgeberMd)
                Response.Write(@"	{U:'../../intranet/arbeitgeber/arbeitgeber.asp',T:'&nbsp;Arbeitgeber'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Arbeitgeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantAuftraggeberMd)
                Response.Write(@"	{U:'../../intranet/auftraggeber/auftraggeber.asp',T:'&nbsp;Auftraggeber'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Auftraggeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantKlientMd)
                Response.Write(@"	{U:'../../intranet/klienten/klienten.asp',T:'&nbsp;Klienten'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Klienten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGegner)
                Response.Write(@"	{U:'../../intranet/gegner/gegner.asp',T:'&nbsp;Gegner'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Gegner',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantLieferantenMd)
                Response.Write(@"	{U:'../../intranet/lieferanten/lieferanten.asp',T:'&nbsp;Lieferanten'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Lieferanten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGegner)
                Response.Write(@"	{U:'../lawyer/EditLawyer.aspx',T:'&nbsp;Rechtsanwalt',SeparatorSize:1}" + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Rechtsanwalt',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB',SeparatorSize:1}" + "\r\n");


            Response.Write(@"	]" + "\r\n");
            Response.Write(@"	);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Stammdaten / Benutzerverwaltung
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"		s_add(" + "\r\n");
            Response.Write(@"		{N:'benutzer',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"		[" + "\r\n");
            if (_grantGruppeBenutzerkonten)
                Response.Write(@"	{U:'../../intranet/user/users.asp',T:'&nbsp;Benutzerkonten'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Benutzerkonten',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGruppeRollen)
                Response.Write(@"		{U:'../../intranet/rollen/rollen.asp',T:'&nbsp;Rollen'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Rollen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGruppeFunktionen)
                Response.Write(@"		{U:'../../intranet/rollenFunkt/rollenFunktionen.asp',T:'&nbsp;Funktionen/Programme'}" + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Funktionen/Programme',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");


            Response.Write(@"		]" + "\r\n");
            Response.Write(@"		);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Einstellungen
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"s_add(" + "\r\n");
            Response.Write(@"{		  " + "\r\n");
            Response.Write(@"N:'main5',	// NAME" + "\r\n");
            Response.Write(@"LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" + "\r\n");
            Response.Write(@"W:120,		// MINIMAL WIDTH" + "\r\n");
            Response.Write(@"T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"L:469,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"P:true,		// menu is PERMANENT (you can only set true if (this is LEVEL 1 menu)" + "\r\n");
            Response.Write(@"S:s_CSSMain2,	// STYLE Array to use for this menu" + "\r\n");
            Response.Write(@"BW:0,		// BORDER WIDTH" + "\r\n");
            Response.Write(@"PD:5,		// PADDING" + "\r\n");
            Response.Write(@"IEF:''		// IEFilter" + "\r\n");
            Response.Write(@"}," + "\r\n");
            Response.Write(@"[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" + "\r\n");
            Response.Write(@"{Show:'setting',U:'',T:'&nbsp;Einstellungen'}" + "\r\n");
            Response.Write(@"]" + "\r\n");
            Response.Write(@");" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Einstellungen Drop
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"	{N:'setting',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:474,P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"	[" + "\r\n");
            Response.Write(@"	{Show:'my',U:'',T:'&nbsp;Meine Einstellungen',SeparatorSize:1},	" + "\r\n");
            if (_grantTarifeMd)
                Response.Write(@"	{U:'../../intranet/tarife/tarife.asp',T:'&nbsp;Tarife/Gebühren'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Tarife/Gebühren',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantHTBEinstellungen)
                Response.Write(@"	{U:'../../intranet/server/editinksys.asp',T:'&nbsp;HTB Einstellungen'}" + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;HTB Einstellungen',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");

            Response.Write(@"	]" + "\r\n");
            Response.Write(@"	);" + "\r\n");
            Response.Write(@"		s_add(" + "\r\n");
            Response.Write(@"		{N:'my',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"		[" + "\r\n");
            Response.Write(@"		{U:'../../intranet/mysettings/personalsettings.asp',T:'&nbsp;Meine persönlichen Einstellungen'}," + "\r\n");
            Response.Write(@"		{U:'../../intranet/mysettings/mail.asp',T:'&nbsp;Meine Mail Einstellungen'}," + "\r\n");
            Response.Write(@"		{U:'../../intranet/mysettings/inksys.asp',T:'&nbsp;Meine HTB Einstellungen',SeparatorSize:1}," + "\r\n");
            Response.Write(@"		{U:'../../intranet/mysettings/changepw.asp',T:'&nbsp;Mein Passwort ändern'}" + "\r\n");
            Response.Write(@"		]" + "\r\n");
            Response.Write(@"		);" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Zubehör
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"s_add(" + "\r\n");
            Response.Write(@"{		  " + "\r\n");
            Response.Write(@"N:'main6',	// NAME" + "\r\n");
            Response.Write(@"LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)" + "\r\n");
            Response.Write(@"W:120,		// MINIMAL WIDTH" + "\r\n");
            Response.Write(@"T:'s_getStart(""y"")',		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"L:584,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)" + "\r\n");
            Response.Write(@"P:true,		// menu is PERMANENT (you can only set true if (this is LEVEL 1 menu)" + "\r\n");
            Response.Write(@"S:s_CSSMain2,	// STYLE Array to use for this menu" + "\r\n");
            Response.Write(@"BW:0,		// BORDER WIDTH" + "\r\n");
            Response.Write(@"PD:5,		// PADDING" + "\r\n");
            Response.Write(@"IEF:''		// IEFilter" + "\r\n");
            Response.Write(@"}," + "\r\n" + "\r\n");
            Response.Write(@"[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details" + "\r\n");
            Response.Write(@"{Show:'tools',U:'',T:'&nbsp;Zubehör'}" + "\r\n");
            Response.Write(@"]" + "\r\n");
            Response.Write(@");" + "\r\n" + "\r\n");
            //-------------------------------------------------------------------------------------------------------------	
            // Zubehör Drop
            //-------------------------------------------------------------------------------------------------------------	
            Response.Write(@"	s_add(" + "\r\n");
            Response.Write(@"	{N:'tools',LV:2,MinW:140,T:'s_getStart(""y"")+25',L:589,P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"	[" + "\r\n");
            if (_grantSpoolerMd)
                Response.Write(@"	{U:'../../intranet/spooler/spooler.asp',T:'&nbsp;Druckdateien..'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Druckdateien..',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantAutomatischeVerarbeitung)
                Response.Write(@"	{U:'style_win2000.php',T:'&nbsp;Autom. Verarbeitungen..',SeparatorSize:1}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Autom. Verarbeitungen..',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantGruppeDatenbankwartung)
                Response.Write(@"	{Show:'db',U:'',T:'&nbsp;Datenbankwartung',SeparatorSize:1},	" + "\r\n");
            else
                Response.Write(@"	{Show:'db',U:'',T:'&nbsp;Datenbankwartung',SeparatorSize:1,FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'},	" + "\r\n");

            if (_grantBugReportsMd)
                Response.Write(@"	{U:'../../intranet/bugs/bugs.asp',T:'&nbsp;Bugreports/Änderungswünsche'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Bugreports/Änderungswünsche',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantServerinfo)
                Response.Write(@"	{U:'../../intranet/server/editserver.asp',T:'&nbsp;Server Info'}," + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Server Info',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantLogdatei)
                Response.Write(@"	{U:'../../intranet/log/eventlog.txt',T:'&nbsp;Logdatei'}" + "\r\n");
            else
                Response.Write(@"	{U:'',T:'&nbsp;Logdatei',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");

            Response.Write(@"	]" + "\r\n");
            Response.Write(@"	);" + "\r\n");
            Response.Write(@"		s_add(" + "\r\n");
            Response.Write(@"		{N:'db',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain}," + "\r\n");
            Response.Write(@"		[" + "\r\n");
            if (_grantDBLeerzeichenAuftraggeber)
                Response.Write(@"		{U:'../../intranet/wartung/trailingspacesAuftragg.asp',T:'&nbsp;Leerzeichen entfernen Auftraggeber'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Leerzeichen entfernen Auftraggeber',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantDBLeerzeichenGegner)
                Response.Write(@"		{U:'../../intranet/wartung/trailingspaces.asp',T:'&nbsp;Leerzeichen entfernen Gegner'}," + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Leerzeichen entfernen Gegner',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}," + "\r\n");

            if (_grantDBLeerzeichenKlient)
                Response.Write(@"		{U:'../../intranet/wartung/trailingspacesKlienten.asp',T:'&nbsp;Leerzeichen entfernen Klient'}" + "\r\n");
            else
                Response.Write(@"		{U:'',T:'&nbsp;Leerzeichen entfernen Klient',FontColor:'#DBDBDB',OverFontColor:'#DBDBDB'}" + "\r\n");

            Response.Write(@"		]" + "\r\n");
            Response.Write(@"		);" + "\r\n");
        }
    }
}