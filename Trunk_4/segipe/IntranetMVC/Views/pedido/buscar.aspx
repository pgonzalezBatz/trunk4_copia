<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="IntranetMVC" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content  ContentPlaceHolderID="cphHead" runat="server">
    <title><%=h.traducir("Buscar")%></title>
</asp:Content>


<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
<table class="table3">
        <thead>
            <tr>
                <th><%=h.traducir("Nº Pedido")%></th>
                <th><%=h.traducir("Estado")%></th>
                <th><%=h.traducir("Proveedor")%></th>
                <th><%=h.traducir("OF")%></th>
                <th><%=h.traducir("OP")%></th>
                <th><%=h.traducir("F. Pedido")%></th>
                <th><%=h.traducir("F. Entrega")%></th>
                <th><%=h.traducir("F. Minima linea")%></th>
                <th><%=h.traducir("Responsable")%></th>
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
                        <a href="<%=Url.Action(ViewData("action"), h.ToRouteValues(Request.QueryString, New With {.idpedido = p.numpedcab, .idestado = p.idestado})) %>">
                        <%=p.numpedcab%>
                        <%If p.urgente = "1" Then%>
                            <span class="warn"><%= h.traducir("Urgente")%></span>
                        <%End If%>
                        </a>
                        <%If p.disconformidad.ToString.Length > 0 Then%>
                             <span class="warn"><%=h.traducir("disconformidad")%></span>
                        <%End If%>
                    </td>
                    <td><%=p.nombreEstado%></td>
                    <td><%=p.nombreProveedor%></td>
                    <td><%=p.numord%></td>
                    <td><%=p.numope%></td>
                    <td><%=p.fechaPedido.toshortdatestring()%></td>
                    <td><%=p.fechaEntrega.toshortdatestring()%></td>
                    <td><%=p.fechaMinimaLinea.toshortdatestring()%></td>
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
                                <input type="submit" value="<%=h.traducir("Enviar") %>" />
                            </form>
                        </td>
                    <%End If%>
                </tr>
            <%Next%>
        </tbody>
    </table>

</asp:Content>
