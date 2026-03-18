<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Añadir movmimiento a grupo")%>
    </title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <table id="table1" class="table1">
        <thead>
            <tr>
                <th><%= h.Traducir("OF")%></th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Peso")%></th>
                <th><%= h.Traducir("Proveedor")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Observación")%></th>
                <th><%= h.Traducir("Negocio")%></th>
                <th><%= h.Traducir("Acciones")%></th>
            </tr>
        </thead>
        <tbody>
        <%For Each m As web.Movimiento In Model%>
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
                <td><%= m.NombreProveedor%></td>
                <td><%= m.NombreSab%></td>
                <td><%= m.Observacion%></td>
                <td><%= m.Negocio%></td>
                <td>
                    <form action="?<%=Request.QueryString.ToString() %>" method="post">
                        <%= Html.Hidden("movimiento", m.Id)%>
                        <input type="submit" value="<%= h.Traducir("Añadir marca a bulto")%>" />
                    </form>
                </td>
            </tr>
        <%Next%>
        </tbody>
    </table>
</asp:Content>
