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
    <%If ViewData("lstFiltrosAplicados").count > 0 Then%>
        <h3><%=h.traducir("Filtros aplicados")%></h3>
        <% For Each f In ViewData("lstFiltrosAplicados")%>
            <strong><%=f.type.ToString.ToUpper%>:</strong> <%=f.nombre%>
            (<a href="<%=Url.Action("list")%><%=h.RemoveFromQueryString(Request.QueryString, f.type, "f")%>"><%=h.traducir("eliminar") %></a>)
        <%Next%>
    <%End If%>
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
                    <input type="checkbox" id="all" />
                </th>
                <th>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","of"),new KeyValuePair(Of string,string)("d","up")) %>">&uarr;</a>
                    <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","of")) %>"><%= h.Traducir("OF")%></a>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","of"),new KeyValuePair(Of string,string)("d","down")) %>">&darr;</a>
                </th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","fecha"),new KeyValuePair(Of string,string)("d","up")) %>">&uarr;</a>
                    <a href="<%=url.action("filter") %><%=web.h.ModifiQueryString(Request.QueryString, New KeyValuePair(Of String, String)("f", "fecha"))%>"><%= h.traducir("Fecha entrega")%></a>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","fecha"),new KeyValuePair(Of string,string)("d","down")) %>">&darr;</a>
                </th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","peso"),new KeyValuePair(Of string,string)("d","up")) %>">&uarr;</a>
                    <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","peso")) %>"><%= h.Traducir("Peso")%></a>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","peso"),new KeyValuePair(Of string,string)("d","down")) %>">&darr;</a>
                </th>
                <th>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","proveedor"),new KeyValuePair(Of string,string)("d","up")) %>">&uarr;</a>
                    <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","proveedor")) %>"><%= h.Traducir("Proveedor")%></a>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","proveedor"),new KeyValuePair(Of string,string)("d","down")) %>">&darr;</a>
                </th>
                <th>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","empresasalida"),new KeyValuePair(Of string,string)("d","up")) %>">&uarr;</a>
                    <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","empresasalida")) %>"><%= h.Traducir("Empresa salida")%></a>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","empresasalida"),new KeyValuePair(Of string,string)("d","down")) %>">&darr;</a>
                
                </th>
                <th><%= h.Traducir("Creado")%></th>
                <th>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","observacion"),new KeyValuePair(Of string,string)("d","up")) %>">&uarr;</a>
                    <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","observacion")) %>"><%= h.Traducir("Observación")%></a>
                    <a href="<%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("o","observacion"),new KeyValuePair(Of string,string)("d","down")) %>">&darr;</a>
                </th>
                <th><%= h.Traducir("Negocio")%></th>
                <th><%= h.Traducir("Acciones")%></th>
            </tr>
        </thead>
        <tbody>
        <%For Each m As web.Movimiento In ViewData("listOfMovimientos")%>
                    <tr>
                <td>
                    <input type="checkbox" name="agrupar" value="<%= m.Id%>" />
                </td>
                <td><%= m.Numord%></td>
                <td><%= m.Numope%></td>
                <td><%= m.Marca%> - <%=m.Material %></td>
                <td>
                <% If m.FechaEntrega.HasValue Then%>
                    <%= m.FechaEntrega.Value.ToShortDateString%>
                <%End If%>
                </td>
                <td><%= m.Cantidad%></td>
                <td><%= m.Peso%></td>
                <td><%= m.NombreProveedor%></td>
                <td><%= m.NombreEmpresaSalida%></td>
                <td><%= m.NombreSab%></td>
                <td><%= m.Observacion%></td>
                <td><%= m.Negocio%></td>
                <td>
                    <a href="<%=url.action("edit","movimientosmaterial",new with{.id=m.Id}) %>"><%= h.Traducir("Editar")%></a>
                    --
                    <a href="<%=url.action("delete","movimientosmaterial",new with{.id=m.Id}) %>"><%= h.Traducir("Eliminar")%></a>
                </td>
            </tr>
        <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="13" align="left";>
                    <input type="submit" value="<%= h.Traducir("Grupo")%>" />                
                </td>
            </tr>
        </tfoot>
    </table>
    </form>
    <%End If%>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#all").click(function () {
                $("input[type=checkbox]").attr("checked", $(this).attr("checked"));
                
            });
        });
    </script>
</asp:Content>
