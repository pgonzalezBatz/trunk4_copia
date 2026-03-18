<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Filtros")%>
    </title>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.action("list") %><%=Web.h.RemoveFromQueryString(request.querystring,"f","v") %>"><%= h.traducir("Eliminar filtro")%></a>
    <ul>
        <%For Each f In ViewData("list")%>
            <li>
                <a href="<%=Url.Action("list") + web.h.ModifiQueryString(Request.QueryString, New KeyValuePair(Of String, String)(Request("f"), Url.Encode(f.key)))%>">
                <%If f.key.ToString.Length > 0 Then%>
                    <%= f.key%>
                <%Else%>
                    *<%= h.traducir("Sin nombre")%>*
                <%End If%>
                </a>
            </li>
        <%Next%>
    </ul>
</asp:Content>
