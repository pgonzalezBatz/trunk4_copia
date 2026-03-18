<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestCookies.aspx.vb" Inherits="WebLogin2.TestCookies" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <asp:Button runat="server" ID="btnGet" Text="Get cookies" />
       <asp:Button runat="server" ID="btnUp" Text="Increment cookie langile" style="margin-left:15px;" /><br /><br />
        Cookie langile: <asp:Label runat="server" ID="lblCookieLangile"></asp:Label>
    </div>
    </form>
</body>
</html>
