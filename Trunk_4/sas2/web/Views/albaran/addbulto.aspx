<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Añadir bulto")%>
    </title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
<table id="table1" class="table1">
        <thead>
            <tr>
                <th><%= h.Traducir("Bulto Nº")%></th>
                <th><%= h.Traducir("Peso bulto")%></th>
                <th><%= h.Traducir("OF")%></th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Proveedor")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Observación")%></th>
                <th><%= h.Traducir("Acciones")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Agrupacion In Model%>
                <%Dim first = True%>
                <%For Each m In a.ListOfMovimiento%>
                    <tr>
                        <% If first Then%>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>">
                                <%= a.Id%><br />
                            </td>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>"><%= a.Peso%></td>
                            <%first = False%>
                        <%End If%>
                        <td><%= m.Numord%></td>
                        <td><%= m.Numope%></td>
                        <td><%= m.Marca%></td>
                        <td>
                        <%If m.FechaEntrega.HasValue Then%>
                        <%= m.FechaEntrega.Value.ToShortDateString%>
                        <%End If%>
                        </td>
                        <td><%= m.Cantidad%></td>
                        <td><%= m.NombreProveedor%></td>
                        <td><%= m.NombreSab%></td>
                        <td><%=m.Observacion %></td>
                        <td>
                            <form action="?<%=Request.QueryString.ToString() %>" method="post">
                                <%= Html.Hidden("idGrupo", a.Id)%>
                                <input type="submit" value="<%= h.Traducir("Añadir bulto a albaran")%>" />
                            </form>
                        </td>
                   </tr>
                <%Next%>
            <%Next%>
        </tbody>
    </table>
</asp:Content>
