<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<asp:Content  ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Listado de Viajes")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <strong><a href="<%=url.action("list") %>"><%= h.Traducir("Volver")%></a></strong>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <strong><%= h.Traducir("Las OFs que se muestran a continuación, no tienen pedido de subcontratación")%></strong>
    <ul>
    <%For Each o In Model%>
        <li>
            <%=o.numord %>:<%= o.numope%>
        </li>
    <%Next%>
    </ul>
    <form action="<%=url.action("subcontratacion2",new with{.id=request("id")}) %>" method="post" style="float:left; margin:0 10px;">
        <input type="submit" value="<%=  h.Traducir("Notificar a Juana")%>" />
    </form>
    <strong><a href="<%=url.action("list") %>"><%= h.Traducir("Volver al listado")%></a></strong>
</asp:Content>
