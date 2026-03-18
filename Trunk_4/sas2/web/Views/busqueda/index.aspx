<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Busqueda de viajes realizados")%>
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.action("productivas") %>"><%= h.traducir("Busqueda viajes productivos")%></a>
    <br />
    <a href="<%=url.action("noproductivas") %>"><%= h.traducir("Busqueda viajes no productivos")%></a>
    <br />
    <a href="<%=Url.Action("albaran") %>"><%= h.traducir("Busqueda de albaranes")%></a>
    <h3><%=h.traducir("Listado mensual por proveedor")%></h3>
    <form action="<%=Url.Action("listadomensual")%>" method="get">
        <fieldset>
            <legend><%=h.traducir("Elegir año, mes y proveedor")%></legend>
            <label>
                <%=h.traducir("Año")%>
                <br />
                <%=Html.TextBox("ejercicio")%>
            </label>
            <br />
            <label>
                <%=h.traducir("Mes")%>
                <br />
                <%=Html.TextBox("mes")%>
            </label>
            <br />
            <label>
                <%=h.Traducir("Negocio")%>
                <br />
                <%=Html.DropDownList("negocios")%>
            </label>
            <br />
            <label>
                <%=h.traducir("Proveedor")%>
                <br />
                <%=Html.DropDownList("transportista")%>
            </label>
            <br />
             <label>
                <%=h.traducir("Viajes tipo taxi")%>
                <br />
                <input type="checkbox" name="taxista" /><%=h.traducir("Si") %>
            </label>
            <br />
            <input type="submit" value="<%=h.traducir("Ver listado") %>" />
        </fieldset>
    </form>
</asp:Content>
