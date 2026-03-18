<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Inicio.aspx.vb" Inherits="GTK_Troqueleria.Inicio" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <table class="GridViewASP" style="width:1%;">
        <tr class="HeaderStyle">
            <th><asp:Label ID="Label1" runat="server" Text="Relacion de Caracteristicas"></asp:Label></th>
        </tr>
        <tr class="RowStyle">
            <td>
                <asp:Button ID="btnOrigen_Producto" runat="server" Text="Origen -> Producto" /></td>
        </tr>
        <tr class="AlternatingRowStyle">
            <td><asp:Button ID="btnCapacidad_Producto" runat="server" Text="Capacidad -> Producto" /></td>            
        </tr>
        <tr class="RowStyle">
            <td><asp:Button ID="btnProducto_Caracteristica" runat="server" Text="Producto -> Caracteristica" /></td>            
        </tr>
    </table>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
