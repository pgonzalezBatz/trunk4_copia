<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content  ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Viajes taxi")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
<a href="<%=url.action("create") %>">
    <%= h.traducir("Create viaje de taxi")%>
</a>
<table class="table1">
    <thead>
        <tr>
            <th><%= h.traducir("Id")%></th>
            <th><%= h.Traducir("Negocio")%></th>
            <th><%= h.traducir("Taxista")%></th>
            <th><%= h.traducir("Origen")%></th>
            <th><%= h.traducir("Destino")%></th>
            <th><%= h.traducir("Fecha")%></th>
            <th><%= h.traducir("Observaciones")%></th>
            <th><%= h.traducir("Kilometros")%></th>
            <th><%= h.traducir("Nº puntos espera")%></th>
            <th><%= h.traducir("Espera superior 1 hora")%></th>
            <th><%= h.traducir("Domingos y festivos")%></th>
            <th><%= h.traducir("subcontratado")%></th>
            <th><%= h.traducir("Acciones")%></th>
            <th><%= h.traducir("Precio")%></th>
        </tr>
    </thead>
    <tbody>
        <%For Each t In Model%>
            <tr>
                <td><%=t.id%></td>
                <td>
                        <% If (t.negocio Is Nothing OrElse String.IsNullOrEmpty(t.negocio)) AndAlso ConfigurationManager.AppSettings("modificadorNegocio").Split(";").Contains(SimpleRoleProvider.GetId()) Then %>
                            <a href="<%= Url.Action("editNegocio", New With {.Id = t.id}) %>">
                                <%= h.traducir("Asignar Negocio") %>
                            </a>
                        <% Else If t.negocio Is Nothing  OrElse String.IsNullOrEmpty(t.negocio) %>
                        
                        <% Else %>
                            <%= t.negocio %>
                        <% End If %>
                </td>
                <td><%=t.nombreProveedor%></td>
                <td><%=t.origen%></td>
                <td><%=t.destino%></td>
                <td><%=t.fecha.toshortdatestring %></td>
                <td><%=t.observacion%></td>
                <td><%=t.kilometros%></td>
                <td><%=t.nPuntosEspera%></td>
                <td><%=t.esperaSuperiorHora%></td>
                <td><%=t.festivos%></td>
                <td><%=t.subcontratado%></td>
                <td>
                    <a href="<%=url.action("edit",new with{.id=t.id}) %>"><%= h.traducir("Editar")%></a>
                </td>
                <td>
                    <form action="<%=url.action("UpdatePrecio") %>" method="post">
                        <%=Html.Hidden("id", t.id)%>
                        <%=Html.TextBox("precio",t.precio)%>
                        <input type="submit" value="<%= h.Traducir("Guardar")%>" />
                    </form>
                </td>
            </tr>
        <%Next%>
    </tbody>
</table>
</asp:Content>
