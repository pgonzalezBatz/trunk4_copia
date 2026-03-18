<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<asp:Content  ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Firma de contratos")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <strong><%= h.traducir("Este trabajador no ha firmado el contrato")%></strong>
    <a href="<%=url.action("contratopdf") %>?idtrabajador=<%=ViewData("idtrabajador") %>" target="_blank"><%= h.traducir("Imprimir contrato")%></a><br />
    <form action="<%=url.action("firmar") %>" method="post">
        <fieldset>
        <%=Html.Hidden("idtrabajador")%>

        <input type="checkbox" name="firmado" />
        <strong><%= h.traducir("Firmado")%></strong>
        <br />
        <input type="submit" value="<%= h.traducir("Guardar")%>" />
        </fieldset>
    </form>
</asp:Content>
