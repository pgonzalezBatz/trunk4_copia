<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Horas extra transporte")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
 <table class="table1">
     <thead>
         <tr>
             <th><%=h.traducir("Pedido")%></th>
             <th><%=h.traducir("Fecha")%></th>
             <th><%=h.traducir("Importe")%></th>
             <th><%=h.traducir("Horas extras")%></th>
             <th></th>
         </tr>
     </thead>
     <tbody>
         <%For Each o In Model%>
         <tr>
            <td><%=o.nPedido%></td>
            <td><%=o.fecha.toshortdatestring%></td>
            <td><%=o.importe%></td>
            <td><%=o.horasExtra%></td>
            <td>
                <a href="<%=Url.Action("edithorasextradiario", New With {.nPedido = o.nPedido, .ticksFecha = CDate(o.fecha).Ticks, .horasExtra = o.horasExtra})%>"><%=h.traducir("editar horas")%></a>
            </td>
         </tr>
         <%Next%>
     </tbody>
 </table>
</asp:Content>
