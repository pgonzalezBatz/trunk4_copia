<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Pruebas.aspx.vb" Inherits="WebRaiz.Pruebas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 20%">
            <tr>
                <td>
                    <asp:Label ID="lblCodTrabajador" runat="server" Text=" Cod.Trabajador" />
                </td>
                <td>
                    <asp:TextBox ID="txtCodTrabajador" runat="server" Text="990399" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCodPrisma" runat="server" Text=" Cod.Prisma" />
                </td>
                <td>
                    <asp:TextBox ID="txtCodPrisma" runat="server" Text="50021" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnIncidenciaPrisma" runat="server" Text="Abrir incidencia prisma" OnClick="btnIncidenciaPrisma_Click" />
                </td>
            </tr>
        </table>        
    </div>
    </form>
</body>
</html>
