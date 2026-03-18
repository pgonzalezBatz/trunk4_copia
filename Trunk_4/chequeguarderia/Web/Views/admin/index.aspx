<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Cheques guardería")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h4><%= h.traducir("Constantes de la aplicación")%></h4>
            <strong><%= h.traducir("Limite de rango")%></strong><br />
            <%=ViewData("costantes").LimiteRango%> €<br/>
            <strong><%= h.traducir("Publico mensual")%></strong><br />
            <%=ViewData("costantes").PublicoMensual%> €<br />
            <strong><%= h.traducir("Ejercicio actual")%> </strong><br />
            <%=ViewData("costantes").EjercicioActual%><br />
            <strong><%= h.traducir("Porcentaje tramite")%></strong><br />
            <%=ViewData("costantes").PorcentajeTramite%><br />
    <h4><%= h.traducir("Vision general")%></h4>
    <form action="<%=url.action("VistaEjercicio") %>" method="get">
        <fieldset>
            <legend><%= h.traducir("Ejercicio a visualizar")%></legend>
            <strong><%= h.traducir("Ejercicio")%></strong><br />
            <%=Html.DropDownList("year")%><br />
            <input type="submit" value="<%= h.traducir("Ir a vision general")%>" />
        </fieldset>
    </form>

    <h4><%= h.traducir("Exportaciones")%></h4>
    <form action="<%=url.action("list") %>" method="get">
        <fieldset>
            <legend><%= h.traducir("Rango a visualizar")%></legend>
            <strong><%= h.traducir("Ejercicio - Mes")%></strong><br />
            <%=Html.DropDownList("yearmonth")%><br />
            <input type="submit" value="<%= h.traducir("Ver solicitudes")%>" />
        </fieldset>
    </form>
</asp:Content>
