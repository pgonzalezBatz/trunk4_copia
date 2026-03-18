<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Cheques guardería")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <table class="table2" style="font-size:0.8em;">
        <thead>
            <tr>
                <th><%= h.Traducir("Nº Trabajador")%> </th>
                <th><%= h.Traducir("Nombre")%> </th>
                <th><%= h.Traducir("Nif hijo/a")%> </th>
                <th><%= h.Traducir("Enero")%> </th>
                <th><%= h.Traducir("Febrero")%> </th>
                <th><%= h.Traducir("Marzo")%> </th>
                <th><%= h.Traducir("Abril")%> </th>
                <th><%= h.Traducir("Mayo")%> </th>
                <th><%= h.Traducir("Junio")%> </th>
                <th><%= h.Traducir("Julio")%> </th>
                <th><%= h.Traducir("Agosto")%> </th>
                <th><%= h.Traducir("Septiembre")%> </th>
                <th><%= h.Traducir("Octubre")%> </th>
                <th><%= h.Traducir("Noviembre")%> </th>
                <th><%= h.Traducir("Diciembre")%> </th>
                <th><%= h.Traducir("Importe")%> </th>
                <th><%= h.Traducir("Tramite")%> </th>
                <th><%= h.Traducir("Total")%> </th>
            </tr>
        </thead>
        <tbody>
            <%For Each s In ViewData("solicitudes")%>
                <tr>
                    <td><%= s.idTrabajador%></td>
                    <td><%= s.nombre%> <%= s.apellido1%> <%= s.apellido2%></td>
                    <td><%= s.nifHija%></td>
                    <%For Each m In s.meses%>
                    <td>
                    <% If m.importe IsNot DBNull.Value Then%>
                        <%=m.importe%>€
                    <%End If%>
                    </td>
                    <%next %>
                    <td><%= s.importe%>€</td>
                    <td><%= s.tramite%>€</td>
                    <td><%= s.total%>€</td>
                </tr>
            <%Next%>
        </tbody>
    </table>
    
</asp:Content>
