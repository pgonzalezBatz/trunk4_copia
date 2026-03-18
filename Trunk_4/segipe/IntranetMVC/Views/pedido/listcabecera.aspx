<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage"  %>

<%@ Import Namespace="IntranetMVC" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title><%=h.traducir("Listado de pedidos")%></title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <form action="<%=Url.Action("buscar", h.ToRouteValuesDelete(Request.QueryString, "idpedido"))%>" method="post">
        <fieldset>
            <legend><%=h.traducir("Buscar pedido")%></legend>
            <%=Html.TextBox("pedido")%><br />
            <input type="submit" value="<%=h.traducir("Buscar") %>" />
        </fieldset>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%Dim lstFilter As Specialized.NameValueCollection = ViewData("listOfFilters")%>
    <%If lstFilter.Count>0 Then%>
    <h2><%=h.traducir("Filtros que se estan aplicando")%></h2>
    <% For Each k In lstFilter.AllKeys%>
        <a href="<%=Url.Action("listcabecera", h.ToRouteValuesDelete(Request.QueryString, k))  %>" style="border:1px solid #A8A8A8; padding:2px; margin:1px;" 
            title="<%= h.traducir("Quitar filtro")%>"><%= k%> = <%= lstFilter(k)%> </a>
    <% Next%>
    <%End If%>
    <h2><%=h.traducir("Listado de cabeceras")%></h2>
    <table class="table3">
        <thead>
            <tr>
                <th><%=h.traducir("Nº Pedido")%></th>
                <th>
                <a href="<%=Url.Action("FilterProveedor", h.ToRouteValuesDelete(Request.QueryString, "idpedido")) %>"><%=h.traducir("Proveedor")%></a>
                </th>
                <th><%=h.traducir("OF")%></th>
                <th><%=h.traducir("OP")%></th>
                <th><%=h.traducir("F. Pedido")%></th>
                <th>
                    <a href="<%=Url.Action("listcabecera", h.ToRouteValues(Request.QueryString, New With {.s = "up"})) %> ">
                    <%=h.traducir("F. Minima linea")%>
                     </a>
                </th>
                <th>
                    <%=h.traducir("F. cambio estado")%>
                </th>
                <th>
                    <a href="<%=Url.Action("filterresponsable", h.ToRouteValuesDelete(Request.QueryString, "idpedido")) %>"><%=h.traducir("Responsable")%></a>
                </th>
                <th><%=h.traducir("Programa")%></th>
                <th><%=h.traducir("Nº Lineas")%></th>
                <th><%=h.traducir("Precio")%></th>
                <th colspan="2"></th>
                <%If ViewData("enviar") Then%>
                <th></th>
                <%End If%>
            </tr>
        </thead>
        <tbody>
            <%For Each p In Model%>
                <tr>
                    <td>
                        <a href="<%=Url.Action(ViewData("action"), h.ToRouteValues(Request.QueryString, New With {.idpedido = p.numpedcab})) %>">
                        <%=p.numpedcab%>
                        <%If p.urgente = "1" Then%>
                            <span class="warn"><%= h.traducir("Urgente")%></span>
                        <%End If%>
                        </a>
                        <%If p.disconformidad.ToString.Length > 0 Then%>
                             <span class="warn"><%=h.traducir("disconformidad")%></span>
                        <%End If%>
                    </td>
                    <td><%=p.nombreProveedor%></td>
                    <td><%=p.numord%></td>
                    <td><%=p.numope%></td>
                    <td><%=p.fechaPedido.toshortdatestring()%></td>
                    <td>
                        <% If p.fechaMinimaLinea < Now Then%>
                        <span style="color:red; font-weight:bold;"><%=p.fechaMinimaLinea.toshortdatestring()%></span>
                        <%Else%>
                        <%=p.fechaMinimaLinea.toshortdatestring()%>
                        <%End If%>
                    </td>
                    <td><%=p.fechaEntradaEstado%></td>
                    <td><%=p.responsable%></td>
                    <td><%=p.programa%></td>
                    <td align="center"><%=p.nLineas%></td>
                    <td align="right"><%=p.precio %>€</td>
                    <td><%=p.texto1 %></td>
                    <td>
                        <%If p.comentarios.length = 0 Then%>
                            <%= h.traducir("No tiene comentarios")%>
                        <%ElseIf p.comentarios.length < 40 Then%>
                            <%=p.comentarios%>
                        <%Else%>
                            <%=p.comentarios.substring(0, 40)%> ...
                        <%End If%>
                    </td>
                    <%If ViewData("enviar") Then%>
                        <td>
                            <form method="post" action="">
                                <input type="checkbox" name="urgente" value="true" /><%=h.traducir("Urgente")%>
                                <input type="checkbox" name="notificar" value="true" /><%=h.traducir("Notificar")%>
                                <input type="hidden" name="idpedido" value="<%=p.numpedcab %>" />
                                <input type="hidden" name="idestado" value="<%=request("idestado") %>" />
                                <input type="submit" value="<%=h.traducir("Enviar") %>" />
                            </form>
                        </td>
                    <%End If%>
                </tr>
            <%Next%>
        </tbody>
    </table>
</asp:Content>
