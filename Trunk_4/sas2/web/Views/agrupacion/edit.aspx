<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of Web.Agrupacion)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Editar agrupacion")%>
    </title>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <a href="<%=url.action("list") %>"><%= h.Traducir("Volver")%></a>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h3><%= h.Traducir("Bulto Nº ")%> <%= Html.DisplayFor(Function(m) m.Id)%></h3>
    <table class="table1">
        <thead>
            <tr>
                <th><%= h.Traducir("OF")%></th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Peso")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Observación")%></th>
                <th><%= h.Traducir("Acciones")%></th>
            </tr>
        </thead>
        <tbody>
            <% For Each m In Model.ListOfMovimiento%>
            <tr>
                <td><%= m.Numord%></td>
                <td><%= m.Numope%></td>
                <td><%= m.Marca%></td>
                <td>
                <% If m.FechaEntrega.HasValue Then%>
                    <%= m.FechaEntrega.Value.ToShortDateString%>
                <%End If%>
                </td>
                <td><%= m.Cantidad%></td>
                <td><%= m.Peso%></td>
                <td><%= m.IdSab%></td>
                <td><%=m.Observacion %></td>
                <td>
                    <form action="<%=Url.action("remove") %>" method="post">
                        <%= Html.Hidden("agrupacion", Model.Id)%>
                        <%= Html.Hidden("movimiento", m.Id)%>
                        <input type="submit" value="<%= h.Traducir("Quitar marca del grupo")%>" />
                    </form>
                </td>
            </tr>
            <%Next%>
        </tbody>
    </table>
    <form action="<%=url.action("removeall") %>" method="post">
        <%= Html.Hidden("agrupacion", Model.Id)%>
        <input type="submit" value="<%= h.Traducir("Eliminar bulto")%>" />
    </form>
</asp:Content>
