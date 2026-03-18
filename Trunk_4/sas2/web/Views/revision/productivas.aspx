<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Revisar productivas")%>
    </title>
</asp:Content>

<asp:Content  ContentPlaceHolderID="cphContenido2" runat="server">
    <%  If Model.count = 0 Then%>
    <h1><%= h.traducir("No se han encontrado viajes")%></h1>
 <%else %>
    <h3><%= h.traducir("Listado de viajes sin enviar")%></h3>
    <%Dim viaje, albaran, agrupacion As Boolean%>
    <ul>
        <%For Each v As web.Viaje In Model%>
                <li>
                    <%dim t= web.DBAccess.GetTransportista(v.IdTransportista, ConfigurationManager.ConnectionStrings("sas").ConnectionString)%>
                    <strong><%=v.Id.ToString%></strong> <strong><%=t.Nombre %></strong>
                    (<%= v.ListOfAlbaran.Sum(Function(a) a.ListOfAgrupacion.Sum(Function(g) g.Peso)) + v.ListOfRecogida.Sum(Function(a) a.ListOfOp.Sum(Function(lofop) lofop.Peso))%> Kg)
                    <%=v.Salida.Value.ToShortDateString%>
                     
                    <a href="<%=url.action("imprimir",new with{.id=v.id}) %>" target="_blank"><%= h.traducir("Imprimir albaranes")%></a> |
                    <br />
                    <ul>
                        <%For Each a As web.Albaran In v.ListOfAlbaran%>
                            <li>
                                <div style="float:left; width:40%;">
                                    <%Dim d = DBAccess.GetDireccionAlbaran(a.Id, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                                    <strong><%= a.ListOfAgrupacion.First().ListOfMovimiento.First().NombreProveedor%></strong><br />
                                    <%=d.calle %><br /> <%= d.codigopostal%> <%= d.poblacion%><br />
                                    Peso Albaran = <%= a.ListOfAgrupacion.Sum(Function(g) g.Peso)%> Kg
                                </div>
                                <div style="float:left; width:40%;">
                                    <table>
                                        <%For Each gu In a.ListOfAgrupacion%>
                                            <% For Each m In gu.ListOfMovimiento%>
                                                <tr>
                                                    <td>
                                                        <%=m.Numord %> : <%= m.Numope%> - <%= m.Marca%> <%=m.Material %>
                                                    </td>
                                                </tr>
                                            <%Next%>
                                        <%Next%>
                                    </table>
                                </div>
                                <div style="clear:both"></div>
                            </li>
                        <%Next%>
                        <%For Each r As web.Recogida In v.ListOfRecogida%>
                        <li class="recogida">
                            <strong><%=r.nombreEmpresaRecogida%></strong>    --->  <strong><%=r.nombreEmpresaEntrega%></strong><br />
                            <strong><%= h.traducir("Peso")%></strong>:
                            <%=r.ListOfOp.Sum(Function(h) h.Peso)%> Kg
                            <strong>OF:OP</strong>:
                            <%For Each ofop In r.ListOfOp%>
                                <%=ofop.Numord%>:<%=ofop.Numope%>
                            <%Next%>
                        </li>
                        <%Next%>
                    </ul>
                     <form method="post" action="">
                        <fieldset>
                            <legend><%= h.traducir("Precio propuesto por el transportista")%></legend>
                        <%= Html.Hidden("id", v.Id)%>
                        <%= Html.Hidden("percio", v.Precio)%>
                        <strong><%=h.traducir("Precio")%>: <%=v.Precio%>€</strong>
                        <input type="submit" value="<%=h.traducir("Aceptar y crear pedido")%>" />
                        </fieldset>
                    </form>
                </li>
        <%Next%>
    </ul>
 <%End If%>
</asp:Content>
