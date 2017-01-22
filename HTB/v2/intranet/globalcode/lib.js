
	/*	---------------------------------------------------------------------------------------------
		function: 	GBInsertTimeStamp
		parameters:	ctrlMemo as control
					strText as string
		author:		gerhard kittel
		date:		30.11.2005
		purpose:	inserts a given String into a control on a form
		---------------------------------------------------------------------------------------------	*/	
		
	function GBInsertTimeStamp(ctrlMemo,strText)
		{
			ctrlMemo.focus(); 
			ctrlMemo.value = ctrlMemo.value + "\n\n" + "[" + strText + "]" + "\n";			
			ctrlMemo.focus();	
		}

	/*	---------------------------------------------------------------------------------------------
		function: 	GBDelSingleRecord
		parameters:	
			@ -dtRole [Role to access DelPage, int]
			@ -dtTable [Table, string]
			@ -dtRecID [Record ID, int]
			@ -dtIDCol [ID Colum, string]
			@ -dtConf [Confirm PopUp, smallint] 0 = no confirmation; ! 1 = confirmation
			@ -dtTableConf [confirm Table, string]
			@ -dtTableConfField [confirm Field 1, string]
			@ -dtTableConfField2 [confirm Fild 2, string]
			@ -dtLang [Language, smallInt ] not implemeted yet: go for standart 1
			@ -dtRefresh [Page Refresh, int]** automatic refresh of the parent window
		author:		michael blümel
		date:		15/12/2005
		purpose:	deletes a singe record; opens a ModalDialog Confirm Window; refresh site
		---------------------------------------------------------------------------------------------	*/	

	function GBDelSingleRecord(dtRole,dtTable, dtRecID, dtIDCol, dtConf, dtTableConf, dtTableConfField, dtTableConfField2, dtLang, dtRefresh)
		{
			ret = window.showModalDialog("../global_forms/GBDelSingleRec.asp?dtRole=" + dtRole + "&dtTable=" + dtTable + "&dtRecID=" + dtRecID + "&dtIDCol=" + dtIDCol + "&dtConf=" + dtConf + "&dtTableConf=" + dtTableConf + "&dtTableConfField=" + dtTableConfField + "&dtTableConfField2=" + dtTableConfField2 + "&dtLang=" + dtLang + "&dtRefresh=" + dtRefresh + "","","center:yes; dialogHeight=300px; dialogWidth=500px; status:no; resizable:no; scroll:no");
			if (dtRefresh == 1)
				{
				window.location.reload();
				}
		}
		
	/*	---------------------------------------------------------------------------------------------
		functions: Show/hide/setHider Functions: Extend view "Filter", "Sortierung", "Aktionen"
		parameters:	
			@ -area []
			@ -sessionVar []
			@ -val []
		author:		michael blümel
		date:		27/12/2005
		purpose:	
		---------------------------------------------------------------------------------------------	*/	
		

	function show(area)
		{
			document.getElementById(area).style.display = 'inline';
		}
	
	function hide(area) {
			document.getElementById(area).style.display = 'none';
		}
	
	function setSessionVar(sessionVar,val)
		{
			ret = window.showModalDialog("../global_forms/setSessionVar.asp?varName=" + sessionVar + "&hiderValue=" + val + "",1,"status:yes; resizable:yes; dialogHeight=10px");
		}
		
	/*	---------------------------------------------------------------------------------------------
		functions: setCookie, getCookie, delCookie
		parameters:	
			@ -c_name []
			@ -value []
			@ -expiredays []
		author:		michael blümel
		date:		02/03/2007
		purpose: set & retrieve Cookie	
		---------------------------------------------------------------------------------------------	*/	
		
	function setCookie(c_name,value,expiredays)
		{
		var exdate=new Date()
		exdate.setDate(exdate.getDate()+expiredays)
		document.cookie=c_name+ "=" +escape(value)+
		((expiredays==null) ? "" : "; expires="+exdate.toGMTString())
		}
		
	function getCookie(c_name)
		{
			if (document.cookie.length>0)
			  {
			  c_start=document.cookie.indexOf(c_name + "=")
			  if (c_start!=-1)
				{ 
				c_start=c_start + c_name.length+1 
				c_end=document.cookie.indexOf(";",c_start)
				if (c_end==-1) c_end=document.cookie.length
				return unescape(document.cookie.substring(c_start,c_end))
				} 
			  }
			return ""
		}

	function delCookie(c_name)
		{
		  var EndDatum = new Date();
		  var Heute = EndDatum.getDate();
		  EndDatum.setDate(Heute - 2);
		  document.cookie = c_name + "=" + '' + ';expires=' + EndDatum.toGMTString();
		}	