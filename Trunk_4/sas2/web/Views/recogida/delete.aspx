<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(of web.recogida)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Eliminar recogida")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
<strong><%= h.traducir("Estas seguro de que quieres eliminar la recogida?")%></strong><br /><br />

    <form action="" method="post">
        <%= Html.HiddenFor(Function(m) m.Id)%>
        <strong><%= h.traducir("Nº de recogida")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.Id)%><br />

        <%= Html.HiddenFor(Function(m) m.Fecha)%>
        <strong><%= h.traducir("Fecha")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.Fecha)%><br />

        <%= Html.HiddenFor(Function(m) m.nombreEmpresaRecogida)%>
        <strong><%= h.traducir("Empresa de recogida")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.nombreEmpresaRecogida)%><br />

        <%= Html.HiddenFor(Function(m) m.nombreEmpresaEntrega)%>
        <strong><%= h.traducir("Empresa de entrega")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.nombreEmpresaEntrega)%><br />
        

        <div class="formbuttons">
            <a href="<%=url.action("list") %>"><%= h.traducir("Volver al listado de recogidas")%></a>
            <input type="submit" value="<%= h.Traducir("Eliminar")%>" />
        </div>
    </form>
</asp:Content>
