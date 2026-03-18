<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>


<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.Traducir("Pedidos nuevos (Lineas)")%> 
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent1" runat="server">
    <br />
    <%=Html.Partial("datospedido")%>
    <div class="ph1">
        <h3><%=h.Traducir("Cambios")%><br /></h3>
        <form action="aceptarcabecera" method="post">
            <fieldset>
            <%=Html.Hidden("pedido", Request("pedido"))%>
            <%=Html.Hidden("fromaction")%>
            <input type="submit" value="<%=h.Traducir("Aceptar todas las líneas")%>" />
            </fieldset>
        </form>
        <form action="proponercabecera" method="post">
            <%=Html.ValidationSummary() %>
            <fieldset>
            <%=Html.Hidden("pedido", Request("pedido"))%>
            <%=Html.Hidden("fromaction")%>
            <%=h.Traducir("Descuento")%><br />
            <%=Html.TextBox("descuento")%><br />
            <%=h.Traducir("Fecha")%><br />
            <input type="text" name="fecha" class="calendar" /><br />
            <input type="submit" value="<%=h.Traducir("Proponer todas las líneas")%>" />
            </fieldset>
        </form>
    </div>
    <div class="ph2">
        <%=Html.Partial("observaciones")%>
    </div>
    <div class="ph3">
        <%=Html.Partial("adjuntos")%>
    </div>
    <br style="clear:left;" /><br /><h3><%=h.Traducir("Lineas")%><br /></h3>
    <table class="table1">
        <thead>
            <tr>
                <th><%=h.Traducir("Nº")%> </th>
                <th><%=h.Traducir("Of")%> </th>
                <th><%=h.Traducir("OP")%> </th>
                <th><%=h.Traducir("Marca")%> </th>
                <th><%=h.Traducir("Articulo")%> </th>
                <th><%=h.Traducir("Descripción")%> </th>
                <th><%=h.Traducir("Cantidad")%> </th>
                <th><%=h.Traducir("P. unitario")%> </th>
                <th><%=h.Traducir("Desc")%> </th>
                <th><%=h.Traducir("Importe")%> </th>
                <th><%=h.Traducir("Fecha")%> </th>
                <th><%=h.Traducir("Aceptar")%> </th>
                <th><%=h.Traducir("Proponer cambios")%> </th>
            </tr>
        </thead>
        <tbody>
            <%For Each l In Model%>
                <tr>
                    <td align="center"><%=l.linea%></td>
                    <td align="center"><%=l.numord%></td>
                    <td align="center"><%=l.numope%></td>
                    <td align="center"><%=l.marca%></td>
                    <td align="center"><%=l.articulo%></td>
                    <td>
                       <%=l.descripciongcarticu%><br />
                        <%=l.descripcion0%> <br />
                        <%=l.descripcion2%>
                        <%If l.descripcion0.ToString.Length > 0 Then%>
                        <br />  
                        <%End If%>
                        <%=If(l.descripcion1.ToString.Length > 0, "Mat: " + l.descripcion1, "")%>
                        <%If l.descripcion2gcarticu.length > 0 Then%>
                        <br />
                            <%=l.descripcion2gcarticu%>
                        <%End If%>

                        <%If Not String.IsNullOrEmpty(l.ref_prov) Then%>
                        <br />
                        Ref_Prov: <%=l.ref_prov %>

                        <%End If %>
                    </td>
                    <td align="center"><%=l.cantidad%></td>
                    <td align="center"><%=l.unitario%>€</td>
                    <td align="center"><%=l.descuento%></td>
                    <td align="center"><%=l.importe%>€</td>
                    <td align="center"><%=l.fecha.toshortdatestring()%></td>
                    <td align="center">
                        <form method="post" action="<%=url.action("aceptarlinea") %>">
                            <%=Html.Hidden("pedido", Request("pedido"))%>
                            <%=Html.Hidden("linea", l.linea)%>
                            <%=Html.Hidden("fromaction")%>
                            <input type="submit" value="<%=h.Traducir("Aceptar")%>" />
                        </form>
                    </td>
                    <td>
                        <form method="post" action="<%=url.action("proponerlinea") %>">
                            <%=Html.Hidden("pedido", Request("pedido"))%>
                            <%=Html.Hidden("linea", l.linea)%>
                            <%=Html.Hidden("fromaction")%>
                            <table>
                                <tr>
                                    <td><%=h.Traducir("Precio")%></td>
                                    <td><%=Html.TextBox("precio")%></td>
                                </tr>
                                <tr>
                                    <td><%=h.Traducir("Descuento")%></td>
                                    <td><%=Html.TextBox("descuento")%></td>
                                </tr>
                                <tr>
                                    <td><%=h.Traducir("Fecha")%></td>
                                    <td><input type="text" name="fecha" class="calendar" /></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td><input type="submit" value="<%=h.Traducir("Proponer")%>" /></td>
                                </tr>
                            </table>
                            
                        </form>
                    </td>
                </tr>
            <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="9"><strong><%=h.traducir("Total") %></strong></th>
                <th><%= math.Round(CType(Model, List(Of Object)).Sum(Function(l) CDec(l.importe)),2) %></th>
                <th colspan="3"></th>
            </tr>
        </tfoot>
    </table>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
        <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-<%=(h.GetCulture().Split("-")(0)) %>.js' type="text/javascript"></script>
  
    <script type="text/javascript">
          $(document).ready(function () {
                        $('.calendar').datepicker($.datepicker.regional['<%=h.GetCulture().Split(" - ")(0)%>']);
        });
        
    </script>
</asp:Content>
