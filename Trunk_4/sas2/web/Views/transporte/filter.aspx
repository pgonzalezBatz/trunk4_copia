<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Filtros")%>
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido2" runat="server">
    <ul>
        <%For Each f In ViewData("list")%>
            <li>
                <a href="<%=url.action("list") %><%=web.h.ModifiQueryString(Request.QueryString, New KeyValuePair(Of String, String)("v", f.codpro)).TrimEnd(" ")%>">
                <%= f.nombre%>
                </a>
            </li>
        <%Next%>
    </ul>
</asp:Content>