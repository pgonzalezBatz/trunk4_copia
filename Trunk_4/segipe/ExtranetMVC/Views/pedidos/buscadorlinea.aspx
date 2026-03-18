<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.Traducir("Buscador (lineas)")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent1" runat="server">
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
                <th><%=h.Traducir("Desc")%> </th>
                <th><%=h.Traducir("Importe")%> </th>
                <th><%=h.Traducir("Fecha")%> </th>
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
                         <%if l.descripciongcarticu <> l.descripcion2 Then%>
                        <%=l.descripcion2%><br />
                        <%End If %>
                        <%=If(l.descripcion1.ToString.Length > 0, "Mat: " + l.descripcion1, "")%>
                            <%If l.diametro.length > 0 Then %>
                        <strong>Diametro: <%=l.diametro %></strong>
                        <%End if %>
                         <%If l.descripcion2gcarticu.length > 0 AndAlso l.descripcion2 <> l.descripcion2gcarticu Then%>
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
                </tr>
            <%Next%>
        </tbody>
           <tfoot>
            <tr>
                <th colspan="9"><strong><%=h.traducir("Total") %></strong></th>
                <th><%= math.Round(CType(Model, List(Of Object)).Sum(Function(l) CDec(l.importe)),2) %></th>
                <th></th>
            </tr>
        </tfoot>
    </table>

</asp:Content>
