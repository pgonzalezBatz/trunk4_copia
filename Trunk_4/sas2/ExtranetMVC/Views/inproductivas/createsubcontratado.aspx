<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.traducir("OF inproductivas")%>
    </title>
    <link href="//intranet.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form action="" method="post">
        <h3><%= h.traducir("Crear viaje taxista")%></h3>
        <div class="form-group">
            <label><%= h.traducir("Fecha")%></label>
            <%=Html.TextBox("fecha", Nothing, New With {.class = "calendar form-control"})%>
        </div>
        <div class="form-group">
            <label><%= h.traducir("Origen")%></label>
            <%=Html.TextArea("origen", New With {.class = "form-control"})%>
        </div>
        <div class="form-group">
            <label><%= h.traducir("Destino")%></label>
            <%=Html.TextArea("destino", New With {.class = "form-control"})%>
        </div>
        <div class="form-group">
            <label><%= h.traducir("Observacion")%></label>
            <%=Html.TextArea("observacion", New With {.class = "form-control"})%>
        </div>
        <div class="form-group">
            <label><%= h.traducir("Importe")%></label>
            <%=Html.TextBox("importe", Nothing, New With {.class = "form-control"})%>
        </div>
        <div class="form-group">
            <label><%= h.traducir("Taxista subcontratado")%></label>
            <%=Html.TextBox("subcontratado", Nothing, New With {.class = "form-control"})%>
        </div>
        <input type="submit" value="<%= h.Traducir("Guardar")%>" class="btn btn-primary" />
    </form>
    <script src="//intranet.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src='//intranet.batz.es/baliabideorokorrak/ui.datepicker.es-ES.js' type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.calendar').datepicker();
        });
    </script>
</asp:Content>
