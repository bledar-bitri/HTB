<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadFile.ascx.cs"
    Inherits="HTB.v2.intranetx.global_files.UploadFile" %>
<html>
<head>
    <title>File - Upload</title>
    
    <script type="text/javascript">
<!--
        function ReturnName() {
            //document.write (document.form1.fieldname.value);
            //document.write (document.form1.imagename.value);
            //window.opener.document.forms(0).item('DOCUMENTAttachment').value = document.form1.imagename.value;
            window.opener.document.forms(0).item(document.form1.fieldname.value).value = document.form1.imagename.value;
            //window.opener.document.images['ImagePreview'].src = '..\foto\upload\' + document.form1.imagename.value;
            //window.opener.document.images['imagepreview'].src = '../user/pics/' + document.form1.imagename.value;
            window.close();
        }
//-->
    </script>

    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
</head>
<body id="bdy" runat="server" background="../../intranet/images/osxback.gif" text="#000000" link="#000000" vlink="#000000" alink="#000000">
    <h2>
        <p class="docHeader"></p>
    </h2>
    <table width="100%" border="1" cellpadding="4" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td class="tblHeader">
                Datei-Upload
            </td>
        </tr>
    </table>
    <form id="form1" name="form1" enctype="MULTIPART/FORM-DATA" >
        <table width="100%" border="1" cellpadding="4" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="4" cellspacing="0">
                    <tr>
                        <td align="right" valign="top">
                            &nbsp;
                        </td>
                        <td align="left">
                            <span class="docText">Dateiname:</span><br/>
                            <asp:FileUpload id="file1" runat="server" class="docText" size="40"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;
                            <input type="hidden" name="Hid" value="Go"/>
                        </td>
                        <td align="left">
                            <asp:button id="SUB1" runat="server" text="Datei senden" class="btnSave" 
                                onclick="SUB1_Click"/>
                            <input type="button" name="SUB12" value="Abbrechen" onclick="window.close();" class="btnCancel"/>
                            <input type="hidden" name="fieldname2" value=""/>
                            <br/>
                            <span class="smallText">(Das Hochladen einer Datei kann, je nach Gr&ouml;sse und Leitungsgeschwindigkeit,
                                eine Weile dauern.)</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        <asp:HiddenField id="configname" runat="server"/>
        <asp:HiddenField id="fieldname"  runat="server"/>
    </form>
    <div align="center">
        <p>
            <span class="docText"></span>
        </p>
        <table id="tblMessage" runat="server" visible="false" width="100%" border="1" cellpadding="4" cellspacing="0" bgcolor="#FFFFFF">
            <tr>
                <td>
                    <div align="center">
                        <p>
                            <span class="smallText" id="message" runat="server"></span>
                        </p>
                        <p>
                            <span class="docText">
                                <input type="button" name="Button2" value="ok" onclick="window.close();" />
                            </span>
                        </p>
                    </div>
                </td>
            </tr>
        </table>
        <p>
            <span class="docText">
                <br/>
                <br/>
            </span>
        </p>
        <p align="left">
            <span class="docText">
                <br/>
                <br/>
            </span>
        </p>
    </div>
    <center>
        <br/>
        <input type="button" id="btnOk" runat="server" visible="false" value="OK - use filename" onclick="ReturnName();" class="docText"/>
    </center>
    <p>
    </p>
    <p align="center">
        &nbsp;
    </p>
</body>
</html>
