<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="WebRaiz._Default1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript">
        function AbrirVentana() {
            var height = screen.height-10;
            var width = screen.width - 10;
            var nuevaVentana = window.open("Login.aspx?olanet=1", "window", "height=" + height + ", width=" + width + ",toolbar=0,scrollbars=0,menubar=0,fullscreen=1,resizable=0,titlebar=0");
            nuevaVentana.moveTo(0, 0);
        }
    </script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body onload="AbrirVentana();">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
