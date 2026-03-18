<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.Traducir("Pedidos propuestos (Lineas)")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent1" runat="server">
    <br />
    <%=Html.Partial("datospedido")%>
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
                <th><%=h.Traducir("Descuento")%> </th>
                <th><%=h.Traducir("Descuento Propuesto")%> </th>
                <th><%=h.Traducir("Importe")%> </th>
                <th><%=h.Traducir("Importe propuesto")%> </th>
                <th><%=h.Traducir("Fecha")%> </th>
                <th><%=h.Traducir("Fecha propuesta")%> </th>
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
                        <%=l.descripcion0%> <br />
                        <%=l.descripcion2%>
                        <%If l.descripcion0.ToString.Length > 0 Then%>
                        <br />  
                        <%End If%>
                        <%=If(l.descripcion1.ToString.Length > 0, "Mat: " + l.descripcion1, "")%>
                        <%If l.diametro.length > 0 Then %>
                        <strong>Diametro: <%=l.diametro %></strong>
                        <%End if %>
                         <%If l.descripcion2gcarticu.length > 0 AndAlso l.descripcion2 <> l.descripcion2gcarticu Then%>
                            <br />
                            <%=l.descripcion2gcarticu%>
                        <%End If%>
                    </td>
                    <td align="center"><%=l.cantidad%></td>
                    <td align="center"><%=l.unitario%>€</td>
                    <td align="center"><%=l.descuento%></td>
                    <td>
                        <%If IsNumeric(l.descuentoPropuesto) Then%>
                            <%=l.descuentoPropuesto%>
                        <%End If%>
                    </td>
                    <td><%=l.importe%></td>
                    <td>
                        <%If IsNumeric(l.precioPropuesto) Then%>
                            <%=l.precioPropuesto%>
                        <%End If%>
                    </td>
                    <td><%=l.fecha.toshortdatestring()%></td>
                    <td>
                        <%If IsDate(l.fechapropuesta) Then%>
                            <%=l.fechapropuesta.toshortdatestring() %>
                        <%End If%>
                    </td>
                     <%If Not String.IsNullOrEmpty(l.ref_prov) Then%>
                        <br />
                        Ref_Prov: <%=l.ref_prov %>

                        <%End If %>
                </tr>
            <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="10"><strong><%=h.traducir("Total") %></strong></th>
                <th><%= math.Round(CType(Model, List(Of Object)).Sum(Function(l) CDec(l.importe)),2) %></th>
                <th colspan="3"></th>
            </tr>
        </tfoot>
    </table>
</asp:Content>
