<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Rutas del viaje")%> <%=Request("idViaje") %>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%For Each p In ViewData("posibilidades") %>
        <div>
            <form method="post" action="<%=Url.Action("AddRuta", h.ToRouteValues(Request.QueryString, Nothing)) %>">
                <%=Html.Hidden("idEmpresa", p.id) %>
                <%=p.nombre %>
                <input type="submit" value="<%=h.traducir("Añadir a la ruta") %>" />
            </form>
        </div>
        
    <%next %>
    <h3><%=h.traducir("Orden de la ruta") %></h3>
    <%If Model.count > 0 Then%>

    <table class="table1">
        <thead>
            <tr>
                <th><%=h.traducir("Orden") %></th>
                <th><%=h.traducir("Destino") %></th>
                <th><%=h.traducir("Km desde detino anterior") %></th>
            </tr>
        </thead>
        <tbody>
            <%For i As Integer = 0 To Model.count - 1 %>
    <tr>
        <td><%=(i + 1).ToString %> <%=Model(i).nombreEmpresa %></td>
    <td><form action="<%=Url.Action("deleteRuta", h.ToRouteValues(Request.QueryString, Nothing)) %>" method="post">
            <%=Html.Hidden("id", Model(i).id) %>
            <input type="submit" value="<%=h.traducir("Eliminar de la ruta") %>" />
        </form></td>
        <td><%=Model(i).distancia %> Km</td>
        </tr>
    <%      Next %>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="2">Total</th>

                <th><%= ctype(Model, IEnumerable(Of Object)).Sum(Function(e) e.distancia) %> Km</th>
                
            </tr>
        </tfoot>
    </table>
    <%else %>
    <%=h.traducir("No se ha especificado ruta") %>
    <%end if %>
</asp:Content>
    