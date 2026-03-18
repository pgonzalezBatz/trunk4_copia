<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(of web.SolicitudUI)" %>

<%@ Import Namespace="Web" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
<title>
        <%= h.Traducir("Formulario alta")%>
</title>
<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
<style type="text/css">
    #NifHija{text-transform:capitalize;}
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form action="" method="post">
        <fieldset>
            <legend><%= h.Traducir("Datos del empleado")%></legend>
            <strong><%= h.Traducir("Solicitante")%></strong>:<%=Html.Display("solicitante")%><br />
            <strong><%= h.Traducir("Email")%></strong>:<%=Html.Display("mail")%><br />
            <strong><%= h.Traducir("telefono")%></strong>:<%=Html.Display("telefono")%><br />
            <strong><%= h.Traducir("Nombre del niño/a")%></strong><br />
            <%=Html.DropDownListFor(Function(m) m.NifHija, ViewData("nifhija"))%><br />
        </fieldset>
        <h4><%= h.Traducir("Si los datos de arriba son incorrectos contacte con personal")%>. 
        <%= h.Traducir("De lo contrario complete el formulario")%></h4>
        <fieldset>
            <legend><%= h.Traducir("Datos de la guarderia")%></legend>
            <strong><%= h.Traducir("Tipo de guarderia")%></strong><br />
            <%=Html.DropDownListFor(Function(m) m.TipoGuarderia, CType(ViewData("tipoguarderia"), Generic.List(Of Mvc.SelectListItem)), h.Traducir("Seleccionar"))%><br />
            <div id="divcantidad">
            <%=Html.Partial("cantidad")%>
            </div>
            <strong><%=Html.ValidationMessage("mes","*")%><%= h.Traducir("Meses")%></strong><br />
            <%=Html.HiddenFor(Function(m) m.Ejercicio)%>
            <%For Each e In ViewData("mes")%>
                <%If e.Selected Then%>
                    <input type="checkbox" name="mes" value="<%=e.value %>"  checked="checked"/>
                <%Else%>
                    <input type="checkbox" name="mes" value="<%=e.value %>" />
                <%End If%>
                <%= h.Traducir(e.text)%>
            <%Next%><br />
            <strong><%= h.Traducir("Nombre de la guarderia")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.Nombre)%><br />
            <strong><%= h.Traducir("Direccion")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.Direccion)%><br />
            <strong><%= h.Traducir("codigoPostal")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.CP, New With {.maxlength = 10})%><br />
            <strong><%= h.Traducir("poblacion")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.Poblacion)%><br />
            <strong><%= h.Traducir("Provincia")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.Provincia)%><br />
            <strong><%= h.Traducir("Telefono del centro")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.TelCentro, New With {.maxlength = 15})%><br />
            <strong><%= h.Traducir("Email del centro")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.MailCentro)%><br />
            <strong><%= h.Traducir("Responsable del centro")%></strong><br />
            <%=Html.TextBoxFor(Function(m) m.ResCentro)%><br />
            <input type="submit" value="<%= h.Traducir("Darme de alta")%>" />
        </fieldset>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            var url = '<%=url.action("CantidadAjax") %>'
            $("#TipoGuarderia").change(function () {
                $.get(url + '?tipoguarderia=' + $(this).attr('value'), function (html) {
                    $('#divcantidad').empty();
                    $('#divcantidad').append(html);
                });
            });
        });
    </script>
</asp:Content>
