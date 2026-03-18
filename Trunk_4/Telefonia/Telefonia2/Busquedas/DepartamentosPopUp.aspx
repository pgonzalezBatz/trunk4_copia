<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DepartamentosPopUp.aspx.vb" Inherits="Telefonia.DepartamentosPopUp" %>
<%@ Register Src="~/Controls/flash.ascx" TagName="Flash" TagPrefix="fls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Departamento</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-left:50px;">
        <fls:Flash runat="server" ID="flsSituacion" />
    </div>
    <br /><br />
    <div align="center">
        <a href="#" onclick="window.close();"><asp:Label runat="server" text="cerrar"></asp:Label></a>
    </div>
    </form>
</body>
</html>
