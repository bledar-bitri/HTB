<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadAkten.aspx.cs" Inherits="HTB.v2.intranetx.global_forms.UploadAkten" %>
<%@ Register TagPrefix="upl" TagName="upload" Src="~/v2/intranetx/global_files/UploadFile.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <upl:upload id="uploadFile" runat="server"/> &nbsp;
    </div>
    </form>
</body>
</html>
