<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="IntranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title><%=h.traducir("Histórico de linea")%></title>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <form action="<%=Url.Action("buscar", h.ToRouteValuesDelete(Request.QueryString, "idpedido", "idlinea"))%>" method="post">
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
    <h2><%=h.traducir("Cambios propuestos")%></h2>
    <%If ViewData("cambiospropuestos").count > 0 Then%>
    <table class="table3">
        <thead>
            <tr>
                <th><%=h.traducir("Fecha de solicitud")%></th>
                <th><%=h.traducir("Fecha propuesta")%></th>
                <th><%=h.traducir("Precio propuesto")%></th>
                <th><%=h.traducir("Descuento propuesto")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each c In ViewData("cambiospropuestos")%>
                <tr>
                    <td><%=c.fechaCreacion %></td>
                    <td><%=c.fechaPropuesta%></td>
                    <td><%=c.precioPropuesto%></td>
                    <td><%=c.descuentoPropuesto%></td>
                </tr>
            <%Next%>
        </tbody>
    </table>
    <%Else%>
        <%=h.traducir("Esta línea no ha tenido cambios propuestos")%>
    <%End If%>
    <h2><%=h.traducir("Cambios de estado")%></h2>
    <table class="table3">
        <thead>
            <tr>
                <th><%=h.traducir("Estado")%></th>
                <th><%=h.traducir("Fecha")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each c In ViewData("historico")%>
                <tr>
                    <td><%=c.nombre%></td>
                    <td><%=c.creado%></td>
                </tr>
            <%Next%>
        </tbody>
    </table>
</asp:Content>
