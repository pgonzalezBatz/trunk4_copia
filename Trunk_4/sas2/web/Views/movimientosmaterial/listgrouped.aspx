<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Listado movimiento de material")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMenu2" runat="server">
    <ul>
        <li><a href="<%=Url.Action("create0") %>"><%= h.Traducir("Crear nuevo movimiento")%></a></li>
        <li><a href="<%=Url.Action("createdesdepedido")%>"><%= h.traducir("Crear movimientos desde pedido")%></a></li>
    </ul>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.Content("~/help.html#movimientomaterial") %>" target="_blank"><%= h.Traducir("Ayuda")%></a>
    <% If ViewData("listOfMovimientos") Is Nothing OrElse ViewData("listOfMovimientos").count = 0 Then%>
        <h3>
            <%= h.Traducir("No se han encontrado nuevos movimientos")%>
            <a href="<%=Url.Action("create0") %>"><%= h.Traducir("Crear nuevo movimiento")%></a>
        </h3>
    <%Else%>
    <h3><%= h.Traducir("Solicitudes de movimientos de material")%></h3>
    <form action="<%= Url.action("pesar","agrupacion")%>" method="post">
    <table id="table1" class="table1">
        <thead>
            <tr>
                <th>
                    <a href="<%=url.action("filter") %><%=web.h.ModifiQueryString(Request.QueryString, New KeyValuePair(Of String, String)("f", "fecha"))%>"><%= h.traducir("Fecha entrega")%></a>
                </th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th>
                    <%= h.Traducir("Peso")%>
                </th>
                <th>
                    <a href="<%=url.action("filter") %><%=web.h.ModifiQueryString(Request.QueryString, New KeyValuePair(Of String, String)("f", "proveedor"))%>"><%= h.Traducir("Proveedor")%></a>
                </th>
                <th>
                    <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","empresasalida")) %>"><%= h.Traducir("Empresa salida")%></a>
                </th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Negocio")%></th>
            </tr>
        </thead>
        <tbody>
        <%For Each m As web.Movimiento In ViewData("listOfMovimientos")%>
                    <tr>
                <td>
                <% If m.FechaEntrega.HasValue Then%>
                    <%= m.FechaEntrega.Value.ToShortDateString%>
                <%End If%>
                </td>
                <td><%= m.Cantidad%></td>
                <td><%= m.Peso%></td>
                <td>
                    <a href="<%=Url.Action("list")%><%=web.h.ModifiQueryString(Request.QueryString, New KeyValuePair(Of String, String)("proveedor", Url.Encode(m.NombreProveedor)), New KeyValuePair(Of String, String)("fecha", m.FechaEntrega.Value.ToShortDateString))%>"><%= Html.Encode( m.NombreProveedor)%></a>
                </td>
                <td><%= m.NombreEmpresaSalida%></td>
                <td><%= m.NombreSab%></td>
                <td><%= m.Negocio%></td>
            </tr>
        <%Next%>
        </tbody>
    </table>
    </form>
    <%End If%>
</asp:Content>
