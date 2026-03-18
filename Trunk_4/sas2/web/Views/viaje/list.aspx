<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Listado de Viajes")%>
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" rel="Stylesheet" type="text/css" />
 
    <style type="text/css">
        .recogida{background-color:#C3C3C3;padding:5px;}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.Content("~/help.html#viaje") %>" target="_blank"><%= h.Traducir("Ayuda")%></a>
    <a href="<%=url.action("filter") %><%=Web.h.ModifiQUeryString(request.querystring, new KeyValuePair(Of string,string)("f","transportista")) %>">(<%= h.traducir("filtrar")%>)</a>
    <%=Html.ValidationSummary()%>
 <%  If Model.count = 0 Then%>
    <h1><%= h.Traducir("No se han encontrado viajes")%></h1>
 <%         Else %>
    <h3><%= h.traducir("Listado de viajes sin enviar")%></h3>
    <%Dim viaje, albaran, agrupacion As Boolean%>
    <ul>
        <%For Each v As web.Viaje In Model%>
                <li>
                    <%dim t = web.DBAccess.GetTransportista(v.IdTransportista, ConfigurationManager.ConnectionStrings("sas").ConnectionString)%>
                    <%= h.Traducir("Viaje")%> <strong><%=t.Nombre %></strong>
                    (<%= v.ListOfAlbaran.Sum(Function(a) a.ListOfAgrupacion.Sum(Function(g) g.Peso)) + v.ListOfRecogida.Sum(Function(a) a.ListOfOp.Sum(Function(lofop) lofop.Peso))%> Kg)
                    <%= v.fechaCreacion %>
                    <%If Not v.deCamino.HasValue Then%>
                    <form action="<%=Url.Action("decamino")%>" method="post" style="display:inline;">
                        <%=Html.Hidden("idviaje", v.Id)%>
                        <input type="submit" value="<%=h.traducir("Notificar proveedores (viaje de camino)")%>" />
                    </form>
                    <%End If%>
                    <br />
                    <a href="<%=url.action("imprimir", New With {.id = v.id}) %>" target="_blank"><%= h.Traducir("Imprimir albaranes")%></a> |
                    <a href="<%=url.action("packinglist", New With {.id = v.id}) %>" target="_blank"><%= h.Traducir("Imprimir packing list")%></a> |
                    <a href="<%=url.action("etiqueta", New With {.id = v.id}) %>" target="_blank"><%= h.Traducir("Imprimir etiquetas")%></a> |
                    <a href="<%=url.action("addalbaran", New With {.id = v.id}) %>"><%= h.Traducir("Añadir albaranes")%></a> |
                    <a href="<%=url.action("edit", New With {.id = v.id}) %>"><%= h.Traducir("Quitar albaranes")%></a> |
                    <a href="<%=url.action("list", "recogida", New With {.idviaje = v.id}) %>"><%= h.Traducir("Añadir recogidas")%></a>|
                    <a href="<%=Url.Action("rutaviaje", "ruta", New With {.idviaje = v.Id}) %>"><%= h.traducir("Rutas")%></a>
                    <br />
                        <%For Each a As web.Albaran In v.ListOfAlbaran%>
                            <div style="padding: 5px;border: 1px dashed;">
                                <div style="float:left; width:40%;">
                                    <%Dim d = web.DBAccess.GetDireccionAlbaran(a.Id, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                                    <strong><%= d.nombreEmpresa%></strong><br />
                                    <%=d.calle %><br /> <%= d.codigopostal%> <%= d.poblacion%><br />
                                    <strong>Peso Albaran = <%= a.ListOfAgrupacion.Sum(Function(g) h.CalcularPesoTotal(g))%> Kg</strong>
                                    <br />
                                    <strong>Negocio Albaran:</strong> <%= a.Negocio %><br />
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
                            <strong><%= h.Traducir("Peso")%></strong>:
                            <%=r.ListOfOp.Sum(Function(h) h.Peso)%> Kg
                            <br />
<%--                            <%If r.puerta.HasValue %>
                               <strong><%=h.traducir("Lugar de entrega ") %> </strong><%= CType(ViewData("puertas"), IEnumerable(Of SelectListItem)).First(Function(k) k.Value = r.puerta).Text%>
                            <% End If %>
                            <br />--%>
                            <strong>OF:OP</strong>:
                            <%For Each ofop In r.ListOfOp%>
                                <%=ofop.Numord%>:<%=ofop.Numope%>
                            <%Next%>
                            <br />
                            <strong>Observación:</strong> <%=r.Observacion %> <br />
                            <strong>Negocio Recogida:</strong> <%= r.Negocio %> <br />
                        </div>
                        <%Next%>
                    
                    <%If v.distancia.HasValue %>
                    <br />
                       <strong>Distancia de la ruta: </strong> <%=v.distancia%>
                    <%End If %>
                    <form method="post" action="<%=url.Action("Enviar") %>">
                    <fieldset>
                        <legend><%= h.Traducir("Marcar viaje como enviado")%></legend>
                        <%= Html.Hidden("id", v.Id)%>
                        <strong>Fecha:</strong><input type="text" name="fecha" class="calendar" /><br />
                        <strong>Notificar transportista:</strong><input type="checkbox" name="notificar" value="notificar" checked="checked" />
                        <strong>Transporte productivo</strong><input type="checkbox" name="productivo" value="productivo" checked="checked" /><br />
                        <br />
                        <strong>Tipo de viaje:</strong> 
                        <%If v.IdTransportista = "0509" Then%>
                            <%=Html.Hidden("listoftipoviaje", "1")%>
                            TAXI
                        <%Else%>
                            <%=Html.DropDownList("listoftipoviaje", h.traducir("Seleccionar"))%>
                        <%End If%>
                        <br />
                        <%If ConfigurationManager.AppSettings("proveedor_trp_tiempo").Split(";").Contains(v.IdTransportista) Then%>
                            <strong>Facturar por dia:</strong><input type="checkbox" name="facturarportiempo" value="true"  />
                            <strong>Precio día:</strong><%=Html.TextBox("preciotrpdia", ConfigurationManager.AppSettings("precio_trp_dia"))%>
                        <br />
                        <%End If%>
                        <strong>Comentario:</strong> <br />
                        <%=Html.TextArea("comentarioalmacen")%>
                        <br />
                        <input type="submit" value="enviar" />
                    </fieldset>
                    </form>
                   
                </li>
        <%Next%>
    </ul>
 <%End If%>
    <div style="background-color:#C3C3C3; height:10px;width:10px; border:1px solid black;float:left;"></div>
    <span style="font-size:0.9em; margin-left:0.6em;font-style:italic;"><%= h.Traducir("albaranes de recogida")%></span>
     <script src='//intranet2.batz.es/baliabideorokorrak/jquery-1.11.2.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-<%=h.GetCulture().Split("-")(0) %>.js' type="text/javascript"></script>
                       <script type="text/javascript">
        $(document).ready(function () {
            $('.calendar').datepicker();
        });
    </script>
</asp:Content>