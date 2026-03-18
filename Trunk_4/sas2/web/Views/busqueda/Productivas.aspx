<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Busqueda de viajes realizados")%>
    </title>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form method="get" action="">
        <fieldset>
            <legend><%= h.traducir("Busqueda viajes con OF productivas")%></legend>
            <strong><%= h.traducir("Desde")%> - 
            <%= h.traducir("Hasta")%></strong>
            (<%= h.traducir("Nº de viaje")%>)<br />
            <%=Html.TextBox("idfrom")%> - <%=Html.TextBox("idto")%><br />
            <input type="submit" value=" <%= h.Traducir("Buscar")%>" />
        </fieldset>
    </form>

    <%If Model.count > 0 Then%>
        <ul>
        <%For Each v As web.Viaje In Model%>
                <li>
                    <%dim t= web.DBAccess.GetTransportista(v.IdTransportista, ConfigurationManager.ConnectionStrings("sas").ConnectionString)%>
                    <strong><%=v.Id.ToString%></strong> <strong><%=t.Nombre %></strong>
                    (<%= v.ListOfAlbaran.Sum(Function(a) a.ListOfAgrupacion.Sum(Function(g) g.Peso)) + v.ListOfRecogida.Sum(Function(a) a.ListOfOp.Sum(Function(lofop) lofop.Peso))%> Kg)
                    <%=If(v.Salida Is Nothing, h.traducir("No ha salido"), v.Salida.Value.ToShortDateString)%>
                     
                    <a href="<%=url.action("imprimir","viaje",new with{.id=v.id}) %>" target="_blank"><%= h.traducir("Imprimir albaranes")%></a> | <strong>Precio:</strong> <%=v.Precio%>€
                    <br />
                        <%For Each a As web.Albaran In v.ListOfAlbaran%>
                            <div>
                                <div style="float:left; width:40%;">
                                    <%Dim d = web.DBAccess.GetDireccionAlbaran(a.Id, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                                    <strong><%= d.nombreEmpresa%></strong><br />
                                    <%=d.calle %><br /> <%= d.codigopostal%> <%= d.poblacion%><br />
                                    Peso Albaran = <%= a.ListOfAgrupacion.Sum(Function(g) g.Peso)%> Kg
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
                        </div>
                        <%Next%>
                    <%If v.comentarioAlmacen.Length > 0 Then%>
                            <strong><%=h.traducir("Comentario almacen")%>: <%=v.comentarioAlmacen%></strong><br />
                            <%End If%>
                            <%If v.comentarioProveedor.Length > 0 Then%>
                            <strong><%=h.traducir("Comentario Proveedor")%>: <%=v.comentarioProveedor%></strong><br />
                            <%End If%>
                 </li>
        <%Next%>
    </ul>
    <%End If%>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#idfrom").keyup(function () {
                $("#idto").attr('value',$(this).attr('value'))
                });
        });
    </script>
</asp:Content>
