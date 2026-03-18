<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Index.aspx.vb" Inherits="WebRaiz.Index" %>
<%@ Register src="Index.ascx" tagname="Index" tagprefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bidaiak</title>
</head>
<body>
    <form id="form1" runat="server">
        <div><uc:Index runat="server" id="ctrlIndex"></uc:Index></div>
    </form>
</body>
</html>
