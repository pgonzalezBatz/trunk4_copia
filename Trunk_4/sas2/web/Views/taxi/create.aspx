<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Crear viaje")%>
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form method="post" action="">
        <fieldset>
            <legend><%= h.traducir("Crear viaje taxista")%></legend>
            <strong><%= h.Traducir("Negocio") %></strong><br />
            <%= Html.DropDownList("negocios")%><br />
            <strong><%= h.traducir("Taxista")%></strong><br />
            <%=Html.DropDownList("proveedor", h.traducir("Seleccionar taxi"))%><br />
            <strong><%= h.traducir("Fecha")%></strong><br />
            <%=Html.TextBox("fecha", Nothing, New With {.class = "calendar"})%><br />
            <strong><%= h.traducir("Origen")%></strong><br />
            <%=Html.TextArea("origen")%><br />
            <strong><%= h.traducir("Destino")%></strong><br />
            <%=Html.TextArea("destino")%><br />
            
            <strong><%= h.traducir("Observacion")%></strong><br />        
            <%=Html.TextArea("observacion")%><br />
            <input type="submit" value="<%= h.Traducir("Guardar")%>" />
        </fieldset>
    </form>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/ui.datepicker.es-ES.js' type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.calendar').datepicker();
        });
    </script>
</asp:Content>
