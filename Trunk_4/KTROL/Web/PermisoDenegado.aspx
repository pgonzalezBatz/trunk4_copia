<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PermisoDenegado.aspx.vb" Inherits="WebRaiz.PermisoDenegado" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="position:absolute; top:30%; left:30%;">
                <tr>
                    <td style="width: 10%">
                        <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ToolTip="Error" ImageUrl="~/App_Themes/Tema1/Imagenes/error_big.gif" /></td>
                    <td>
                        <asp:Label runat="server" ID="lblMensaje" Style="font-weight: bold; font-size: 20px;"></asp:Label></td>
                </tr>
                <tr>
                    <%--<td colspan="2" align="center">
                        <br />
                        <table>
                            <tr>--%>
                                <%--<td style="width: 10%">
                                    
                                </td>--%>
                                
                                <td style="text-align:center" colspan="2"><%--align="center"--%>
                                    <br /><br />
                                    <asp:LinkButton runat="server" ID="lnkKtrol" Text="Volver a la página de Login de KTROL" Style="font-size: 20px;" />
                                    <asp:ImageButton runat="server" ID="imgKtrol" ToolTip="Volver a la página de Login de KTROL" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Imagenes/logo_Ktrol_grande.png" />
                                </td>
                            <%--</tr>
                        </table>
                        <br />
                        <br />
                    </td>--%>
                </tr>
                <tr>                 
                    <td style="text-align:center" colspan="2"><%--align="center"--%>
                        <br /><br />
                        <asp:LinkButton runat="server" ID="lnkHelpdesk" Text="Abrir incidencia en Helpdesk" Style="font-size: 20px;" />
                        <asp:ImageButton runat="server" ID="imgHelpdesk" ToolTip="Dar de alta una incidencia en Helpdesk" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Imagenes/logo_helpdesk_grande.png"  />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

    
<%--</asp:Content>--%>
