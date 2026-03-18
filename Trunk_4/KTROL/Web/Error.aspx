<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Error.aspx.vb" Inherits="WebRaiz._Error1" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <table class="PermisoDenegado">
        <tr>
            <td><asp:Image ID="Image1" runat="server" ImageAlign="Middle" ToolTip="Error" ImageUrl="~/App_Themes/Tema1/Imagenes/error_big.gif" /></td>
            <td><asp:Label runat="server" ID="lblMensaje" style="font-weight:bold;font-size:15px;"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" ID="lnkKtrol" Text="Volver a la página de Login de KTROL" style="font-size:20px;" />
                        </td>
                        <td align="center">
                            <asp:ImageButton runat="server" ID="imgKtrol" ToolTip="Volver a la página de Login de KTROL" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Imagenes/logo_Ktrol_grande.png" />
                        </td>
                    </tr>                    
                </table>
                <br /><br />
            </td>
        </tr>        
    </table>                     
</asp:Content>
