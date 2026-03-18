<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Busqueda de viajes no productivas")%>
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form method="get" action="">
        <fieldset>
            <legend><%= h.traducir("Busqueda viajes con OF productivas")%></legend>
            <strong><%= h.traducir("Desde")%> - 
            <%= h.traducir("Hasta")%></strong>
            (<%= h.traducir("Nº de viaje")%>)
            <br />
            <%=Html.TextBox("idfrom")%> - <%=Html.TextBox("idto")%><br />
            <input type="submit" value=" <%= h.Traducir("Buscar")%>" />
        </fieldset>
    </form>

    <%If Model.count > 0 Then%>
       <table class="table1">
    <thead>
        <tr>
            <th><%= h.traducir("Id")%></th>
            <th><%= h.traducir("Taxista")%></th>
            <th><%= h.traducir("Origen")%></th>
            <th><%= h.traducir("Destino")%></th>
            <th><%= h.traducir("Fecha")%></th>
            <th><%= h.traducir("Observaciones")%></th>
        </tr>
    </thead>
    <tbody>
        <%For Each t In Model%>
            <tr>
                <td><%=t.id%></td>
                <td><%=t.nombreProveedor%></td>
                <td><%=t.origen%></td>
                <td><%=t.destino%></td>
                <td><%=t.fecha.toshortdatestring %></td>
                <td><%=t.observacion%></td>
            </tr>
        <%Next%>
    </tbody>
</table>
    <%End If%>
</asp:Content>
