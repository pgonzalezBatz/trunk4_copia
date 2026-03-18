<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PermisoDenegado.aspx.vb" Inherits="ReferenciasVentas.PermisoDenegado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Referencias de venta</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="position:absolute; width:auto; top:30%; left:40%;">
            <tr>
                <td style="width: 10%">
                    <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ToolTip="Error" ImageUrl="~/App_Themes/Batz/Imagenes/error_big.gif" /></td>
                <td>
                    <asp:Label runat="server" ID="lblMensaje" Style="font-weight: bold; font-size: 28px;" Text="Access denied"></asp:Label>
                </td>
            </tr>
            <tr>                                
                <td style="text-align:left" colspan="2">
                    <br /><br />
                    <asp:LinkButton runat="server" ID="lnkHelpdesk" Text="Open an issue in Helpdesk" Font-Size="28px" />
                    <asp:ImageButton runat="server" ID="imgHelpdesk" ToolTip="Open an issue in Helpdesk" ImageAlign="Middle" ImageUrl="~/App_Themes/Batz/Imagenes/logo_helpdesk_grande.png" />
                </td>                          
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
