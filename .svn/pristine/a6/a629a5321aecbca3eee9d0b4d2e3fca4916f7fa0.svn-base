// USE WORDWRAP AND MAXIMIZE THE WINDOW TO SEE THIS FILE
// v5
// === 1 === EXTRAS
s_hideTimeout=300;//1000=1 second
s_subShowTimeout=300;//if <=100 the menus will function like SM4.x
s_subMenuOffsetX=1;//pixels (if no subs, leave as you like)
s_subMenuOffsetY=-3;
s_keepHighlighted=true;
s_autoSELECTED=true;//make the item linking to the current page SELECTED
s_autoSELECTEDItemsClickable=false;//look at IMPORTANT NOTES 1 in the Manual
s_autoSELECTEDTree=true;//look at IMPORTANT NOTES 1 in the Manual
s_autoSELECTEDTreeItemsClickable=true;//look at IMPORTANT NOTES 1 in the Manual
s_scrollingInterval=30;//scrolling for tall menus
s_rightToLeft=false;
s_hideSELECTsInIE=false;//look at IMPORTANT HOWTOS 7 in the Manual

// === 2 === Default TARGET for all the links
// for navigation to frame, calling functions or
// different target for any link look at
// IMPORTANT HOWTOS 1 NOTES in the Manual
s_target='self';//(newWindow/self/top)


// === 3 === STYLESHEETS- you can define different arrays and then assign
// them to any menu you want with the s_add() function
s_CSSMain=[
'#999999',	// BorderColorDOM ('top right bottom left' or 'all')
'#999999',	// BorderColorNS4
1,		// BorderWidth
'#f1f1f1',	// BgColor
2,		// Padding
'#f1f1f1',	// ItemBgColor
'#cccccc',	// ItemOverBgColor
'#000000',	// ItemFontColor
'#000000',	// ItemOverFontColor
'verdana,arial,helvetica,sans-serif',	// ItemFontFamily
s_iE&&!s_iE4&&!s_mC?'1.0em':'9px',	// ItemFontSize (css)
'1',		// ItemFontSize Netscape4 (look at KNOWN BUGS 3 in the Manual)
'normal',	// ItemFontWeight (bold/normal)
'left',		// ItemTextAlign (left/center/right)
2,		// ItemPadding
0,		// ItemSeparatorSize
'#999999',	// ItemSeparatorColor
'progid:DXImageTransform.Microsoft.Shadow(color="#777777",Direction=135,Strength=3)',		// IEfilter (look at Samples\IE4(5.5)Filters dirs)
true,				// UseSubImg
'black_arrow.gif',	// SubImgSrc
'black_arrow.gif',	// OverSubImgSrc
7,				// SubImgWidth
7,				// SubImgHeight
6,				// SubImgTop px (from item top)
'#ffffff',			// SELECTED ItemBgColor
'#000000',			// SELECTED ItemFontColor
'black_arrow.gif',	// SELECTED SubImgSrc
true,				// UseScrollingForTallMenus
'scrolltop.gif',	// ScrollingImgTopSrc
'scrollbottom.gif',	// ScrollingImgBottomSrc
68,				// ScrollingImgWidth
12,				// ScrollingImgHeight
'',		// ItemClass (css)
'',		// ItemOverClass (css)
'',		// SELECTED ItemClass (css)
1,		// ItemBorderWidth
'#f1f1f1',	// ItemBorderColor ('top right bottom left' or 'all')
'#999999',	// ItemBorderOverColor ('top right bottom left' or 'all')
'#999999',	// SELECTED ItemBorderColor ('top right bottom left' or 'all')
2,		// ItemSeparatorSpacing
'',		// ItemSeparatorBgImage
'',		// ItemBgImage
'',		// ItemOnBgImage
''		// SELECTED ItemBgImage
];
s_CSSMain2=[
'#999999',	// BorderColorDOM ('top right bottom left' or 'all')
'#999999',	// BorderColorNS4
1,		// BorderWidth
'#f1f1f1',	// BgColor
2,		// Padding
'#f1f1f1',	// ItemBgColor
'#cccccc',	// ItemOverBgColor
'#000000',	// ItemFontColor
'#000000',	// ItemOverFontColor
'verdana,arial,helvetica,sans-serif',	// ItemFontFamily
s_iE&&!s_iE4&&!s_mC?'0.5em':'9px',	// ItemFontSize (css)
'1',		// ItemFontSize Netscape4 (look at KNOWN BUGS 3 in the Manual)
'normal',	// ItemFontWeight (bold/normal)
'left',		// ItemTextAlign (left/center/right)
2,		// ItemPadding
0,		// ItemSeparatorSize
'#999999',	// ItemSeparatorColor
'progid:DXImageTransform.Microsoft.Shadow(color="#777777",Direction=135,Strength=3)',		// IEfilter (look at Samples\IE4(5.5)Filters dirs)
true,				// UseSubImg
'/no/black_arrow.gif',	// SubImgSrc
'/no/black_arrow.gif',	// OverSubImgSrc
7,				// SubImgWidth
7,				// SubImgHeight
6,				// SubImgTop px (from item top)
'#ffffff',			// SELECTED ItemBgColor
'#000000',			// SELECTED ItemFontColor
'/no/black_arrow.gif',	// SELECTED SubImgSrc
true,				// UseScrollingForTallMenus
'scrolltop.gif',	// ScrollingImgTopSrc
'scrollbottom.gif',	// ScrollingImgBottomSrc
68,				// ScrollingImgWidth
12,				// ScrollingImgHeight
'',		// ItemClass (css)
'',		// ItemOverClass (css)
'',		// SELECTED ItemClass (css)
1,		// ItemBorderWidth
'#999999',	// ItemBorderColor ('top right bottom left' or 'all')
'#999999',	// ItemBorderOverColor ('top right bottom left' or 'all')
'#999999',	// SELECTED ItemBorderColor ('top right bottom left' or 'all')
2,		// ItemSeparatorSpacing
'',		// ItemSeparatorBgImage
'',		// ItemBgImage
'',		// ItemOnBgImage
''		// SELECTED ItemBgImage
];

// === 4 === MENU DEFINITIONS
s_add(
{
N:'main',	// NAME
LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)
W:120,		// MINIMAL WIDTH
T:200,		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)
L:5,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)
P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)
S:s_CSSMain2,	// STYLE Array to use for this menu
BW:0,		// BORDER WIDTH
PD:5,		// PADDING
IEF:''		// IEFilter
},

[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details
{Show:'new',U:'',T:'&nbsp;Neu...'}
]
);

	s_add(
	{N:'new',LV:2,MinW:140,T:205,L:5,P:false,S:s_CSSMain},
	[
	
	{U:'style_win2000.php',T:'&nbsp;Gegner'},
	{U:'style_winxp.php',T:'&nbsp;Klient',SeparatorSize:1},
	{U:'style_osx.php',T:'&nbsp;Anschrifterhebung'},
	{U:'style_osx.php',T:'&nbsp;Inkassoakt'},
	{U:'style_ms_website.php',T:'&nbsp;Interventionsakt'},
	{U:'style_bluecurve.php',T:'&nbsp;Bonitätsprüfung',SeparatorSize:1},
	{U:'style_keramik.php',T:'&nbsp;Aufgabe'},
	{U:'style_smartmenus_dot_org.php',T:'&nbsp;Termin'},
	{U:'style_smartmenus4.php',T:'&nbsp;Rechnung'}
	]
	);

	
	s_add(
{
N:'main2',	// NAME
LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)
W:120,		// MINIMAL WIDTH
T:200,		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)
L:120,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)
P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)
S:s_CSSMain2,	// STYLE Array to use for this menu
BW:0,		// BORDER WIDTH
PD:5,		// PADDING
IEF:''		// IEFilter
},

[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details
{Show:'programme',U:'',T:'&nbsp;Programme'}
]
);

	s_add(
	{N:'programme',LV:2,MinW:140,T:204,L:125,P:false,S:s_CSSMain},
	[
	{Show:'inkasso',U:'',T:'&nbsp;CollectionInvoice'},	 
	{U:'style_win2000.php',T:'&nbsp;Detektei',SeparatorSize:1},
	{U:'style_win2000.php',T:'&nbsp;FAKT'},
	{U:'style_winxp.php',T:'&nbsp;FIBU'},
	{U:'style_osx.php',T:'&nbsp;LOHN'}
	]
	);
	
		s_add(
		{N:'inkasso',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},
		[
		{U:'../bonitaet/bonitaet.asp',T:'&nbsp;Bonitätsprüfungen'},
		{U:'../aktenink/Akten.asp',T:'&nbsp;Inkassoakten'},
		{U:'../aktenint/aktenint.asp',T:'&nbsp;Interventionen',SeparatorSize:1},
		{U:'../workflow/default.asp',T:'&nbsp;Workflow'},
		{Show:'import1',U:'',T:'&nbsp;Import'}

		]
		);
	
			s_add(
			{N:'import1',LV:4,MinW:140,T:'',L:'',P:false,S:s_CSSMain},
			[
			{U:'../bonitaet/bonitaet.asp',T:'&nbsp;Inkassoakten: Inkis'},
			{U:'../aktenink/Akten.asp',T:'&nbsp;Interventionen: A&M'},
			{U:'../aktenint/aktenint.asp',T:'&nbsp;Interventionen: Inkis'},
			{U:'../workflow/default.asp',T:'&nbsp;Interventionen: KSV Allianz'},
			{U:'../workflow/default.asp',T:'&nbsp;Interventionen: KSV T-Mobile'}
	
			]
			);
	
	s_add(
{		  
N:'main3',	// NAME
LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)
W:120,		// MINIMAL WIDTH
T:200,		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)
L:235,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)
P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)
S:s_CSSMain2,	// STYLE Array to use for this menu
BW:0,		// BORDER WIDTH
PD:5,		// PADDING
IEF:''		// IEFilter
},

[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details
{Show:'akten',U:'',T:'&nbsp;Akten'}
]
);

	s_add(
	{N:'akten',LV:2,MinW:140,T:204,L:240,P:false,S:s_CSSMain},
	[
	{U:'style_win2000.php',T:'&nbsp;Anschrifterhebungen'},
	{U:'style_win2000.php',T:'&nbsp;Bonitätsprüfungen'},
	{U:'style_win2000.php',T:'&nbsp;Inkassoakten'},
	{U:'style_winxp.php',T:'&nbsp;Interventionsakten'}
	]
	);

	s_add(
{		  
N:'main4',	// NAME
LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)
W:120,		// MINIMAL WIDTH
T:200,		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)
L:350,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)
P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)
S:s_CSSMain2,	// STYLE Array to use for this menu
BW:0,		// BORDER WIDTH
PD:5,		// PADDING
IEF:''		// IEFilter
},

[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details
{Show:'stamm',U:'',T:'&nbsp;Stammdaten'}
]
);

	s_add(
	{N:'stamm',LV:2,MinW:140,T:204,L:355,P:false,S:s_CSSMain},
	[
	{Show:'benutzer',U:'',T:'&nbsp;Benutzerverwaltung',SeparatorSize:1},	 
	{U:'style_win2000.php',T:'&nbsp;Auftraggeber'},
	{U:'style_win2000.php',T:'&nbsp;Klienten'},
	{U:'style_win2000.php',T:'&nbsp;Gegner'},
	{U:'style_win2000.php',T:'&nbsp;Lieferanten',SeparatorSize:1},
	{U:'style_win2000.php',T:'&nbsp;Klienten'},
	{U:'style_win2000.php',T:'&nbsp;Klienten'},
	{U:'style_win2000.php',T:'&nbsp;Klienten'},
	{U:'style_winxp.php',T:'&nbsp;Interventionsakten'}
	]
	);

		s_add(
		{N:'benutzer',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},
		[
		{U:'style_win2000.php',T:'&nbsp;Benutzerkonten'},
		{U:'style_win2000.php',T:'&nbsp;Rollen'},
		{U:'style_win2000.php',T:'&nbsp;Funktionen/Programme'}
		]
		);

s_add(
{		  
N:'main5',	// NAME
LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)
W:120,		// MINIMAL WIDTH
T:200,		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)
L:465,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)
P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)
S:s_CSSMain2,	// STYLE Array to use for this menu
BW:0,		// BORDER WIDTH
PD:5,		// PADDING
IEF:''		// IEFilter
},

[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details
{Show:'setting',U:'',T:'&nbsp;Einstellungen'}
]
);

	s_add(
	{N:'setting',LV:2,MinW:140,T:204,L:470,P:false,S:s_CSSMain},
	[
	{Show:'my',U:'',T:'&nbsp;Meine Einstellungen',SeparatorSize:1},	
	{U:'../tarife/tarife.asp',T:'&nbsp;Tarife/Gebühren'},
	{U: '../server/editinksys.asp', T: '&nbsp;HTB Einstellungen' }
	]
	);
		s_add(
		{N:'my',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},
		[
		{U:'style_win2000.php',T:'&nbsp;Meine persönlichen Einstellungen'},
		{U:'style_win2000.php',T:'&nbsp;Meine Mail Einstellungen'},
		{U: 'style_win2000.php', T: '&nbsp;Meine HTB Einstellungen', SeparatorSize: 1 },
		{U:'style_winxp.php',T:'&nbsp;Mein Passwort ändern'}
		]
		);
		
s_add(
{		  
N:'main6',	// NAME
LV:1,		// LEVEL (look at IMPORTANT NOTES 1 in the Manual)
W:120,		// MINIMAL WIDTH
T:200,		// TOP (look at IMPORTANT HOWTOS 6 in the Manual)
L:580,		// LEFT (look at IMPORTANT HOWTOS 6 in the Manual)
P:true,		// menu is PERMANENT (you can only set true if this is LEVEL 1 menu)
S:s_CSSMain2,	// STYLE Array to use for this menu
BW:0,		// BORDER WIDTH
PD:5,		// PADDING
IEF:''		// IEFilter
},

[		// define items {U:'url',T:'&nbsp; text' ...} look at the Manual for details
{Show:'tools',U:'',T:'&nbsp;Zubehör'}
]
);

	s_add(
	{N:'tools',LV:2,MinW:140,T:204,L:585,P:false,S:s_CSSMain},
	[
	{U:'style_win2000.php',T:'&nbsp;Druckdateien..'},
	{U:'style_win2000.php',T:'&nbsp;Autom. Verarbeitungen..',SeparatorSize:1},
	{Show:'db',U:'',T:'&nbsp;Datenbankwartung'},	
	{U:'../server/editserver.asp',T:'&nbsp;Server Info'},
	{U:'../log/eventlog.txt',T:'&nbsp;Logdatei'}
	]
	);
		s_add(
		{N:'db',LV:3,MinW:140,T:'',L:'',P:false,S:s_CSSMain},
		[
		{U:'../wartung/trailingspacesAuftragg.asp',T:'&nbsp;Leerzeichen entfernen Auftraggeber'},
		{U:'../wartung/trailingspaces.asp',T:'&nbsp;Leerzeichen entfernen Gegner'},
		{U:'../wartung/trailingspacesKlienten.asp',T:'&nbsp;Leerzeichen entfernen Klient'}
		]
		);
