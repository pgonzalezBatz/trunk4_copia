<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(of web.recogida)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Eliminar recogida")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
<strong><%= h.traducir("Estas seguro de que quieres eliminar la recogida?")%></strong><br /><br />

        <strong><%= h.traducir("Nº de recogida")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.Id)%><br />

        <strong><%= h.traducir("Fecha")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.Fecha)%><br />

        <strong><%= h.traducir("Empresa de recogida")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.nombreEmpresaRecogida)%><br />

        <strong><%= h.traducir("Empresa de entrega")%></strong><br />
        <%= Html.DisplayFor(Function(m) m.nombreEmpresaEntrega)%><br />
        

        <%=html.ValidationSummary() %>
        <form action="" method="post">
            <%For Each e In Model.ListOfOp%>
                <input type="checkbox" name="lineas" value="<%= e.Numord%>|<%=e.Numope%>|<%=e.Peso%>" />
                <strong><%=h.traducir("Numero de orden")%>:</strong> <%=e.Numord%>
                <strong><%=h.traducir("Operación")%></strong><%=e.Numope%>
                <strong><%=h.traducir("Peso")%></strong><%=e.Peso%><br />
            <%next %>
        
        
            <div class="formbuttons">
                <a href="<%=url.action("list") %>"><%= h.traducir("Volver al listado de recogidas")%></a>
                <input type="submit" value="<%= h.traducir("Llevar a otra recogida")%>" />
            </div>
        </form>
</asp:Content>
