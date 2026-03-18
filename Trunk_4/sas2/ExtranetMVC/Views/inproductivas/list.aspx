<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.traducir("OF inproductivas")%>
    </title>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.action("create") %>">Nuevo viaje</a>
    <table class="table">
        <thead>
            <tr>
                <th><%=h.traducir("Fecha")%></th>
                <th><%=h.traducir("Origen")%></th>
                <th><%=h.traducir("Destino")%></th>
                <th><%=h.traducir("Observaciones")%></th>
                <th><%=h.traducir("Importe")%></th>
                <th><%=h.traducir("Kilometros")%></th>
                <th><%=h.traducir("Nº puntos espera")%></th>
                <th><%=h.traducir("Espera superior 1 hora")%></th>
                <th><%=h.traducir("Domingos y festivos")%></th>
                <th><%=h.traducir("Acciones")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each v In Model%>
                <tr>
                    <td><%=v.fecha.toshortdatestring()%></td>
                    <td><%=v.origen%></td>
                    <td><%=v.destino%></td>
                    <td><%=v.observacion%></td>
                    <td align="right"><%=v.importe%>€</td>
                    <td><%=v.kilometros%></td>
                    <td><%=v.nPuntosEspera%></td>
                    <td><%=v.esperaSuperiorHora%></td>
                    <td><%=v.festivos%></td>
                    <td>
                        <a href="<%=Url.Action("delete", New With {.id = v.id})%>"><%=h.traducir("Eliminar")%></a>
                    </td>
                </tr>
            <%Next%>
        </tbody>
    </table>
</asp:Content>
