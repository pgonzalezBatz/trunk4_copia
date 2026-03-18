<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="PermisoDenegado.aspx.vb" Inherits="IstrikuWebRaiz.PermisoDenegado" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .permisoDenegado {
            font: oblique bold 120% cursive; 
            text-shadow: 0.1em 0.1em 0.2em gray;
            font-size: x-large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <table>
        <tr>
            <td><img src="App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" alt="Don´t Panic" /></td>
            <td><asp:Label ID="lblMensaje" runat="server" Text="permisoDenegado" CssClass="permisoDenegado"></asp:Label></td>
        </tr>
    </table>
</asp:Content>