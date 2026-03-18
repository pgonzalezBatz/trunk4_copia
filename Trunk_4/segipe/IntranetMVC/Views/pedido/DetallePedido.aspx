<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="IntranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">

    <title><%=h.traducir("Detalle pedido")%></title>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <form action="<%=Url.Action("buscar", h.ToRouteValuesDelete(Request.QueryString, "idpedido")) %>" method="post">
        <fieldset>
            <legend><%=h.traducir("Buscar pedido")%></legend>
            <%=Html.TextBox("pedido")%><br />
            <input type="submit" value="<%=h.traducir("Buscar") %>" />
        </fieldset>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <div class="ph1">
    <%=Html.Partial("datospedido")%>
    </div>
    <div class="ph2">
        <%=Html.Partial("observaciones")%>
    </div>
    <div class="ph3">
        <%=Html.Partial("adjuntos")%>
    </div>
    <br style="clear:left;" />
    <br />
    <h2><%=h.traducir("Lineas de pedido")%></h2>
    <%=html.ValidationSummary() %>
    <form action="" method="post">
     <table class="table3">
        <thead>
            <tr>
                <th><%= h.traducir("Nº")%> </th>
                <th><%= h.traducir("Of")%> </th>
                <th><%= h.traducir("OP")%> </th>
                <th><%= h.traducir("Marca")%> </th>
                <th><%= h.traducir("Articulo")%> </th>
                <th><%= h.traducir("Descripción")%> </th>
                <th><%= h.traducir("Cantidad")%> </th>
                <th><%= h.traducir("P. unitario")%> </th>
                <th><%= h.traducir("Desc")%> </th>
                <th><%= h.traducir("Importe")%> </th>
                <th><%= h.traducir("F. Entrega")%> </th>
                <th><%= h.traducir("F. Límite")%> </th>
                <th><%= h.traducir("F. Fundición")%> </th>
                <%If Request("idestado") = 4 OrElse Request("idestado") = 8 Then%>
                <th><%= h.traducir("Fecha p.")%> </th>
                <th><%= h.traducir("Precio p.")%> </th>
                <th><%= h.traducir("Descuento p.")%> </th>
                <th><%= h.traducir("Selec.")%> <br /><input type="checkbox" id="allselect" /></th>
                <th><%= h.traducir("Precio concer.")%><br /><input type="checkbox" id="allconcert" /> </th>
                <th><%= h.traducir("Resp. prov.")%><br /><input type="checkbox" id="allprov" /> </th>
                <%End If%>
                <th><%= h.traducir("Histórico")%> </th>
            </tr>
        </thead>
        <tbody>
        <% For Each l In ViewData("lineas")%>
            <tr>
                <td align="center"><%=l.linea%></td>
                <td align="center"><%=l.numord%></td>
                <td align="center"><%=l.numope%></td>
                <td align="center"><%=l.marca%></td>
                <td align="center"><%=l.articulo%></td>
                <td>
                    <%=l.descripciongcarticu%><br />
                    <%=l.descripcion0%><br /> 
                    <%=l.descripcion2%>
                    <%If l.descripcion0.ToString.Length > 0 Then%>
                    <br />  
                    <%End If%>
                    <%=If(l.descripcion1.ToString.Length > 0, "Mat: " + l.descripcion1, "")%>
                    <%If l.diametro Then%>
                        <strong>Diametro: <%=l.diametro %></strong>
                    <%End If%>
                    <br />
                    <%=l.descripcion2gcarticu%>


                    <%If IsNumeric(l.largo) %>
                                        <%=l.largo %> X <%=l.ancho %> X <%=l.grueso %>
                    <%End if %>
                     <%If Not String.IsNullOrEmpty(l.ref_prov) Then%>
                        <br />
                        Ref_Prov: <%=l.ref_prov %>

                        <%End If %>
                </td>
                <td align="center"><%=l.cantidad%></td>
                <td align="center"><%=l.unitario%>€</td>
                <td align="center"><%=l.descuento%></td>
                <td align="right"><%=l.importe%>€</td>
                <td>
                        <% If l.fecha < Now Then%>
                        <span style="color:red; font-weight:bold;"><%=l.fecha.toshortdatestring()%></span>
                        <%Else%>
                        <%=l.fecha.toshortdatestring()%>
                        <%End If%>
                </td>
                <td align="center">
                <%If IsDate(l.fechaLimite) Then%>
                    <%=l.fechaLimite.toshortdatestring()%>
                <%End If%>
                </td>
                <td align="center">
                <%If IsDate(l.fechaFundicion) Then%>
                <%= l.fechaFundicion.toshortdatestring()%>
                <%End If%>
                </td>
                <%If Request("idestado") = 4 OrElse Request("idestado") = 8 Then%>
                <td align="center">
                    <%If IsDate(l.fechaPropuesta) Then%>
                    <%=Html.TextBox("fecha_" + l.linea.ToString, l.fechaPropuesta.toshortdatestring(),New With{.class="calendar"})%>
                    <%End If%>
                </td>
                <td align="center">
                    <%If IsNumeric(l.preciopropuesto) Then%>
                        <%=Html.TextBox("precio_" + l.linea.ToString, l.precioPropuesto)%>
                    <%End If%>
                </td>
                <td align="center">
                    <% If IsNumeric(l.descuentoPropuesto) Then%>
                        <%=Html.TextBox("descuento_" + l.linea.ToString, l.descuentoPropuesto)%>
                    <%end if %>
                </td>
                <td>
                    <input type="checkbox" name="seleccionar" value="<%=l.linea %>" />
                </td>
                <td>
                    <input type="checkbox" name="concertado" value="<%=l.linea %>" />
                </td>
                <td>
                    <input type="checkbox" name="proveedor" value="<%=l.linea %>" />
                </td>
                <%End If%>
                <td>
                    <a href="<%=Url.Action("historico", New With {.idpedido = Request("idpedido"), .idestado = Request("idestado"), .idlinea = l.linea.ToString})%>">
                        <%=h.traducir("ver historico")%>
                    </a>
                </td>
            </tr>
        <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="9"><%=h.traducir("Total") %></th>
                <%If Request("idestado") = 4 OrElse Request("idestado") = 8 Then%>
                <th><%= math.Round(CType(ViewData("lineas"), List(Of Object)).Sum(Function(l) CDec(l.importe)), 2) %></th>
                <th colspan="3"></th>
                <th colspan="7">
                    <input type="submit" value="<%=h.traducir("Aceptar") %>" />
                </th>
                <%else %>
                <th><%= math.Round(CType(ViewData("lineas"), List(Of Object)).Sum(Function(l) CDec(l.importe)), 2) %></th>
                <th colspan="4"></th>
                <%End If%>
            </tr>
        </tfoot>
    </table>
    </form>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/ui.datepicker.es-ES.js' type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.calendar').datepicker();
            $("#allselect").click(function () {
                $("input[name=seleccionar]").attr("checked", $(this).attr("checked"));
            });
            $("#allconcert").click(function () {
                $("input[name=concertado]").attr("checked", $(this).attr("checked"));
            });
            $("#allprov").click(function () {
                $("input[name=proveedor]").attr("checked", $(this).attr("checked"));
            });
        });
    </script>
</asp:Content>
