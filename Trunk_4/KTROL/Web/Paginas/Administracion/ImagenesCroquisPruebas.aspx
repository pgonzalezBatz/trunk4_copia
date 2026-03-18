<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImagenesCroquisPruebas.aspx.vb" Inherits="WebRaiz.ImagenesCroquisPruebas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCodOperacion" runat="server" Text="Código Operación" />
                </td>
                <td>
                    <asp:TextBox ID="txtCodOperacion" runat="server" Text="8401610" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnProbarImagenCroquis" runat="server" Text="Aceptar" OnClick="btnProbarImagenCroquis_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
