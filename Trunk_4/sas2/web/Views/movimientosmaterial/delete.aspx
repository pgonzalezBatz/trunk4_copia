<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of web.Movimiento)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Eliminar movimiento de material")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <strong><%= h.traducir("Estas seguro de que quieres eliminar el movimiento?")%></strong><br /><br />
    <strong><%= h.traducir("OF")%></strong><br />
    <%= Html.DisplayFor(Function(m) m.Numord)%><br />
    <strong><%= h.traducir("OP")%></strong><br />
    <%= Html.DisplayFor(Function(m) m.Numope)%><br />
    <strong><%= h.traducir("Marca")%></strong><br />
    <%= Html.DisplayFor(Function(m) m.Marca)%><br />
    <strong><%= h.traducir("Proveedor")%></strong><br />
    <%= Html.DisplayFor(Function(m) m.NombreProveedor)%><br />
    <strong><%= h.traducir("Fecha de entrega")%></strong><br />
    <%= Html.DisplayFor(Function(m) m.FechaEntrega)%><br />
    <strong><%= h.traducir("Cantidad")%></strong><br />
    <%= Html.DisplayFor(Function(m) m.Cantidad)%><br />
    <strong><%= h.traducir("Peso")%></strong><br />
    <%= Html.DisplayFor(Function(m) m.Peso)%><br /><br />

    <form action="" method="post">
        <%= Html.HiddenFor(Function(m) m.Id)%>
        <div class="formbuttons">
            <a href="<%=url.action("list") %>"><%= h.traducir("Volver al listado de movimientos")%></a>
            <input type="submit" value="<%= h.Traducir("Eliminar")%>" />
        </div>
    </form>
</asp:Content>

