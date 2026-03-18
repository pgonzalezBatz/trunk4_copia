<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="imagenDinamica.aspx.vb" Inherits="Telefonia.imagenDinamica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
             <%--Para mostrar los errores o los mensajes de la pagina--%>
             <asp:Label runat="server" ID="lblMensaje" CssClass="mensajeInformativo"></asp:Label>            
        </center>
    </div>
    </form>
</body>
</html>
