<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Valorar transporte")%>
    </title>
</asp:Content>


<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <a href="<%= Url.Action("listhorasextradiario")%>">Introducir horas extra</a>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
       <a href="<%=url.Content("~/help.html#transporte") %>" target="_blank"><%= h.traducir("Ayuda")%></a> |
       <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","transportista")) %>">(<%= h.traducir("filtrar")%>)</a> 

 <%  If Model.count = 0 Then%>
    <h1><%= h.traducir("No se han encontrado viajes")%></h1>
 <%else %>
    <h3><%= h.traducir("Listado de viajes sin enviar")%></h3>
    <%Dim viaje, albaran, agrupacion As Boolean%>
    <ul>
        <%For Each v As web.Viaje In Model%>
                <li>
                    <%Dim t = web.DBAccess.GetTransportista(v.IdTransportista, ConfigurationManager.ConnectionStrings("sas").ConnectionString)%>
                    <strong><%=v.Id.ToString%></strong> <strong><%=t.Nombre %></strong>
                    (<%= v.ListOfAlbaran.Sum(Function(a) a.ListOfAgrupacion.Sum(Function(g) g.Peso)) + v.ListOfRecogida.Sum(Function(a) a.ListOfOp.Sum(Function(lofop) lofop.Peso))%> Kg)
                    <%=v.Salida.Value.ToShortDateString%>
                     
                    <a href="<%=url.action("imprimir",new with{.id=v.id}) %>" target="_blank"><%= h.traducir("Imprimir albaranes")%></a> |
                    <a href="<%=Url.Action("rutaviaje", "ruta", New With {.idviaje = v.Id}) %>"><%= h.traducir("Rutas")%></a> |
                    <%If Not v.deCamino.HasValue Then%>
                    <form action="<%=Url.Action("decamino")%>" method="post" style="display:inline;">
                        <%=Html.Hidden("idviaje", v.Id)%>
                        <input type="submit" value="<%=h.traducir("Notificar proveedores (viaje de camino)")%>" />
                    </form>
                    <%End If%>
                    <br />
                        <%For Each a As web.Albaran In v.ListOfAlbaran%>
                            <div>
                                <div style="float:left; width:40%;">
                                    <%Dim d = web.DBAccess.GetDireccionAlbaran(a.Id, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                                       <strong><%= d.nombreEmpresa%></strong><br />
                                    <%=d.calle %><br /> <%= d.codigopostal%> <%= d.poblacion%><br />
                                    <strong>Peso Albaran = <%= a.ListOfAgrupacion.Sum(Function(g) h.CalcularPesoTotal(g))%> Kg</strong>
                                </div>
                                <div style="float:left; width:40%;">
                               <% Html.RenderPartial("~/Views/Agrupacion/displayBultoPartial.vbhtml", a.ListOfAgrupacion) %>
                                </div>
                                <div style="clear:both"></div>
                            </div>
                        <%Next%>
                        <%For Each r As web.Recogida In v.ListOfRecogida%>
                        <div class="recogida">
                            <strong><%=r.nombreEmpresaRecogida%></strong>    --->  <strong><%=r.nombreEmpresaEntrega%></strong><br />
                            <strong><%= h.traducir("Peso")%></strong>:
                            <%=r.ListOfOp.Sum(Function(h) h.Peso)%> Kg
                            <strong>OF:OP</strong>:
                            <%For Each ofop In r.ListOfOp%>
                                <%=ofop.Numord%>:<%=ofop.Numope%>
                            <%Next%>
                            <br />
                            <strong>Observación:</strong> <%=r.Observacion %>
                        </div>
                        <%Next%>
                     <%If v.distancia.HasValue %>
                    <br />
                       <strong>Distancia de la ruta: </strong> <%=v.distancia%>
                    <%End If %>
                    <form method="post" action="">
                        <fieldset>
                            <legend><%= h.traducir("Introducir precio y crear pedido")%></legend>
                            <%If ConfigurationManager.AppSettings("taxistas").Split(";").Contains(v.IdTransportista) Then%>
                            <%=h.traducir("Kilometros:")%><%=v.kilometros%><br />
                            <%=h.traducir("Nº puntos espera:")%><%=v.nPuntosEspera%><br />
                            <%=h.traducir("Espera superior hora:")%><%=v.esperaSuperiorHora%><br />
                            <%=h.traducir("Domingos y festivos:")%><%=v.festivos%><br />
                            <%End If%>
                            <%If v.comentarioAlmacen.Length > 0 Then%>
                            <strong><%=h.traducir("Comentario almacen")%>: <%=v.comentarioAlmacen%></strong><br />
                            <%End If%>
                            <%If v.comentarioProveedor.Length > 0 Then%>
                            <strong><%=h.traducir("Comentario Proveedor")%>: <%=v.comentarioProveedor%></strong><br />
                            <%End If%>
                        <%=Html.Hidden("id", v.Id)%>
                        <%=Html.TextBox("precio",v.Precio)%>
                        <input type="submit" value="Guardar precio" />
                        </fieldset>
                    </form>

                </li>
        <%Next%>
    </ul>
 <%End If%>
    <div style="background-color:#C3C3C3; height:10px;width:10px; border:1px solid black;float:left;"></div>
    <span style="font-size:0.9em; margin-left:0.6em;font-style:italic;"><%= h.traducir("albaranes de recogida")%></span>
</asp:Content>
