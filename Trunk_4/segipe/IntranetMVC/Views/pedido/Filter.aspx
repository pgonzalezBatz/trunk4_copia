<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="IntranetMVC" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title><%=h.traducir("Filtrar Responsable")%></title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <ul>
    <%For Each f In Model%>
        <li>
            <%If ViewData("tipofiltro") = "responsable" %>
                    <a href="<%=Url.Action("listcabecera", h.ToRouteValues(Request.QueryString, New With {.responsable = f.id})) %>">
            <%Else %>
        <a href="<%=Url.Action("listcabecera", h.ToRouteValues(Request.QueryString, New With {.proveedor = f.id})) %>">
<%end if%>

            <%=f.nombre%>
</a>
    <%Next%>
    </ul>
</asp:Content>
