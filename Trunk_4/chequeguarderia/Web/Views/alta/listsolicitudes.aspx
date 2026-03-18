<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
<title>
        <%= h.Traducir("Listado de solicitudes")%>
</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <strong>
        <%= h.Traducir("Abajo te mostramos los meses e importes que cheque guareria te proporcionara")%>.<br />
        <%= h.Traducir("En caso de que veas algún error o quieres realizar algún cambio, comunicaselo a Recursos humanos")%>
    </strong><br /><br />
    <%For Each g In Model%>
    <strong><%= h.Traducir("Ejercicio")%></strong>
    <%=g.ejercicio%>
    <table class="table2">
        <thead>
                <tr>
                    <th><%= h.Traducir("mes")%></th>
                    <th><%= h.Traducir("importe")%></th>
                    <th><%= h.Traducir("Tramite")%></th>
                    <th><%= h.Traducir("Guardería")%></th>
                </tr>
            </thead>
            <tbody>
    <%For Each i In g.lst%>
                <tr>
                    <td><%=i.mes%></td>
                    <td><%=i.importe%></td>
                    <td><%=i.tramite%></td>
                    <td><%=i.nombreGuarderia%></td>
                </tr>
        <%Next%>
        </tbody>
        <tfoot>
            <th><%= h.Traducir("Total")%></th>
            <th><%=g.total%></th>
            <th colspan="2"></th>
        </tfoot>
    </table>
    <%Next%>
</asp:Content>
