<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of Web.albaran)" %>


<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Editar albarán")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <a href="<%=url.action("list") %>"><%= h.Traducir("Volver")%></a>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h3><%= h.Traducir("Albarán Nº ")%> <%= Html.DisplayFor(Function(m) m.Id)%></h3>
    <table class="table1">
         <thead>
            <tr>
                <th><%= h.Traducir("Bulto Nº")%></th>
                <th><%= h.Traducir("Peso bulto")%></th>
                <th><%= h.Traducir("Peso Marcas")%></th>
                <th><%= h.Traducir("OF")%></th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Peso")%></th>
                <th><%= h.Traducir("Proveedor")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Observación")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Agrupacion In Model.ListOfAgrupacion%>
                <%Dim first = True%>
                <%For Each m In a.ListOfMovimiento%>
                    <tr>
                        <% If first Then%>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>">
                                <%= a.Id%><br />
                                <form action="<%=Url.Action("removebulto", h.ToRouteValues(Request.QueryString, Nothing)) %>" method="post">
                                <%= Html.Hidden("agrupacion", a.Id)%>
                                <input type="submit" value="<%= h.Traducir("Quitar bulto del albarán")%>" />
                            </form>
                            </td>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>"><%= a.Peso%></td>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>"><%= Decimal.Round(a.ListOfMovimiento.Sum(Function(o) o.Peso))%></td>
                            <%first = False%>
                        <%End If%>
                        <td><%= m.Numord%></td>
                        <td><%= m.Numope%></td>
                        <td>
                            <%= m.Marca%>
                            <form action="<%=Url.Action("removemarca", h.ToRouteValues(Request.QueryString, Nothing)) %>" method="post">
                                <%= Html.Hidden("agrupacion", a.Id)%>
                                <%= Html.Hidden("movimiento", m.Id)%>
                                <input type="submit" value="<%= h.Traducir("Quitar marca del albarán")%>" />
                            </form>
                        </td>
                        <td>
                        <%If m.FechaEntrega.HasValue Then%>
                        <%= m.FechaEntrega.Value.ToShortDateString%>
                        <%End If%>
                        </td>
                        <td><%= m.Cantidad%></td>
                        <td><%= m.Peso.Value.ToString("0.##")%></td>
                        <td><%= m.NombreProveedor%></td>
                        <td><%= m.NombreSab%></td>
                        <td><%=m.Observacion %></td>
                   </tr>
                <%Next%>
            <%Next%>
        </tbody>
    </table>
    <br />
    <form action="<%=Url.action("removealbaran") %>" method="post">
        <%= Html.Hidden("albaran", Model.Id)%>
        <input type="submit" value="<%= h.Traducir("Eliminar albarán")%>" />
    </form>
</asp:Content>
