<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Listado agrupaciones")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMenu2" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.Content("~/help.html#grupos") %>" target="_blank"><%= h.Traducir("Ayuda")%></a>
    <% If ViewData("listOfAgrupacion").count=0 Then%>
        <h1><%= h.Traducir("No se han encontrado bultos")%></h1>
    <%Else%>
    <h3><%= h.Traducir("Listado de bultos sin albaranes")%></h3>
    <form action="<%=Url.Action("createEmptyBulto") %>" method="post">
        Peso
        <%=Html.TextBox("peso") %>
        <input type="submit" value="<%=h.traducir("Crear bulto vacio") %>" />
    </form>
    <form action="<%= Url.action("create","albaran")%>" method="get">
    <table id="table1" class="table1">
        <thead>
            <tr>
                <th><%= h.traducir("Dentro de bulto")%></th>
                <th><%= h.Traducir("Albarán")%></th>
                <th><%= h.Traducir("Bulto Nº")%></th>
                <th>
                    <%= h.Traducir("Peso bulto")%>
                </th>
                <th>
                    <%= h.Traducir("OF")%>
                </th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Peso")%></th>
                <th><%= h.Traducir("Proveedor")%></th>
                <th><%= h.Traducir("Empresa salida")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.traducir("Observación")%></th>
                <th><%= h.Traducir("Negocio")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Agrupacion In ViewData("listOfAgrupacion")%>
                <%If a.ListOfMovimiento.Count = 0 %>
                    <tr>
                        <td>
                             <%If a.idParent.HasValue %>
                            <%=a.idParent.Value %><br />
                                <a href="<%=Url.Action("UnEmbedBulto", New With {.id = a.Id}) %>"><%=h.traducir("Eliminar acumulacion") %></a>
                            <%else %>
                            <a href="<%=Url.Action("EmbedBulto", New With {.id = a.Id}) %>"><%=h.traducir("Accumular en bulto") %></a>
                            <%End If %>
                        </td>
                        <td>
                             <input type="checkbox" name="bultos" value="<%= a.Id%>" />
                        </td>
                        <td><%= a.Id%></td>
                        <td>
                            <a href="<%=Url.Action("editPeso", h.ToRouteValues(Request.QueryString, New With {.idbulto = a.Id})) %>"><%= a.Peso%></a>
                        </td>
                    </tr>
                <%End if %>
                <%Dim first = True%>
                <%For Each m In a.ListOfMovimiento%>
                        <tr>
                        <% If first Then%>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>">
                            <%If a.idParent.HasValue %>
                            <%=a.idParent.Value %><br />
                                <a href="<%=Url.Action("UnEmbedBulto", New With {.id = a.Id}) %>"><%=h.traducir("Eliminar acumulacion") %></a>
                            <%else %>
                            <a href="<%=Url.Action("EmbedBulto", New With {.id = a.Id}) %>"><%=h.traducir("Accumular en bulto") %></a>
                            <%End If %>
                        </td>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>">
                                <input type="checkbox" name="bultos" value="<%= a.Id%>" />
                                <br />
                                <%=Html.ActionLink(h.traducir("Imprimir"), "pdf", New With {.id = a.Id}) %>
                            </td>
                            <td rowspan="<%=a.ListOfMovimiento.Count() %>">
                                <%= a.Id%><br />
                                <a href="<%=Url.Action("edit",new with{.id=a.id}) %>"><%= h.Traducir("Quitar movimientos")%></a><br />
                                <a href="<%=Url.Action("addmovimiento",new with{.id=a.id}) %>"><%= h.Traducir("Añadir movimientos")%></a><br />
                            </td>
                            <td rowspan="<%=a.ListOfMovimiento.count() %>">
                                <a href="<%=Url.Action("editPeso", h.ToRouteValues(Request.QueryString, New With {.idbulto = a.Id})) %>"><%= a.Peso%></a>
                            </td>
                            <%first = False%>
                        <%End If%>
                        <td><%= m.Numord%></td>
                        <td><%= m.Numope%></td>
                        <td><%= m.Marca%> <br /> <%=m.Material %></td>
                        <td>
                        <%If m.FechaEntrega.HasValue Then%>
                        <%= m.FechaEntrega.Value.ToShortDateString%>
                        <%End If%>
                        </td>
                        <td><%= m.Cantidad%></td>
                        <td><%= m.Peso.Value.ToString("0.##")%></td>
                        <td><strong><%= m.NombreProveedor%></strong></td>
                        <td><%= m.NombreEmpresaSalida%></td>
                        <td><%= m.NombreSab%></td>
                        <td><%=m.Observacion %></td>
                        <td><%=m.Negocio %></td>
                        
                   </tr>
                <%Next%>
            <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="15" align="left">
                    <input type="submit" value="<%= h.Traducir("Agrupar bultos en albarán")%>" />
                </td>
            </tr>
        </tfoot>
    </table>
    
    </form>
    <%End If%>
</asp:Content>
