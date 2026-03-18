<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of web.MovimientoBase)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Crear movimiento de material")%>
    </title>
    <style type="text/css">
        #divProveedor{font-weight:bold;color:Gray;}
        #spanProveedor{color:Red;font-weight:bold;}
        #nombreempresaslida{font-weight:bold;color:Green;}
        input{margin-bottom:1em;}
    </style>
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMenu2" runat="server">
    <ul>
        <li><a href="<%=Url.Action("list") %>"><%= h.traducir("Listado de movimientos")%></a></li>
    </ul>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%= Html.ValidationSummary()%>
    <form action="" method="post">
        <fieldset>
            <legend><%= h.traducir("Movimientos de material")%></legend>
            <strong><%= h.Traducir("Negocio") %></strong><br />
            <%= Html.DropDownListFor(Function(m) m.IdNegocio, CType(ViewData("negocios"), List(Of SelectListItem)))%><br />

            <strong><%= h.traducir("OF")%></strong> - 
            <strong><%= h.traducir("OP")%></strong><br />
            <%= Html.TextBoxFor(Function(m) m.Numord, New With {.class = "small-box"})%>
             -
            <%= Html.DropDownListFor(Function(m) m.Numope, CType(ViewData("operaciones"), Generic.List(Of SelectListItem)), "-- Seleccionar --")%><br />
            <strong><%= h.traducir("Código de proveedor")%></strong><br />
            <span id="spanProveedor"><%= h.traducir("Selecciona un proveedor:")%></span>
            <%= Html.HiddenFor(Function(m) m.CodPro)%><br />
            <input type="text" id="busquedaproveedor" autocomplete="off" /><br />
            <strong><%= h.traducir("Fecha prevista de entrega")%></strong><br />
            <%= Html.TextBoxFor(Function(m) m.FechaEntrega,New With{.class="calendar"})%><br />
            <strong><%= h.traducir("Empresa de salida")%></strong><br />
            <%=Html.HiddenFor(Function(m) m.EmpresaSalida)%>
            <span id="nombreempresaslida"></span><br />
            <input type="text" id="busquedaempresasalida" autocomplete="off" /><br />
            <input type="submit" value="<%= h.Traducir("Continuar")%>" />
        </fieldset>
    </form>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-1.11.2.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-<%=h.GetCulture().Split("-")(0) %>.js' type="text/javascript"></script>
    <script src="../Scripts/JScript1.js" type="text/javascript"></script>
    <script type="text/javascript">
       movimientoMaterial('<%= ResolveClientUrl("~/json")%>', 'Numord', 'Numope', 'Marca', 'helper');
           function OneElementToAppend(val) {
               f(val.Id, val.Nombre);
               $('#FechaEntrega').focus();
           };
           function OneElementToAppend2(val) {
               clickEmpresaSalida(val.Id, val.Nombre);
               $('#busquedaempresasalida').empty();
               $('input[type=submit]').focus();
           };
           function ManyElementsToAppend(el,val, fun) {                                                                 
               el.append('<a id="pro' + val.Id.toString() + '" href="#">' + val.Nombre + '</a><br/>');
               fun($('#pro' + val.Id.toString()), function () { f(val.Id, val.Nombre) });
           };
           function ManyElementsToAppend2(el,val, fun) {
               el.append('<a id="emp' + val.Id.toString() + '" href="#">' + val.Nombre + '</a><br/>');
               fun($('#emp' + val.Id.toString()), function () { clickEmpresaSalida(val.Id, val.Nombre) });
           };
           function f(id, name) {
               var codproBox = $('#CodPro');
               var codNameTag = $('#spanProveedor');
               var searchBox = $('#busquedaproveedor');
               codproBox.attr("value", id);
               codNameTag.empty();
               codNameTag.attr('style', 'color:green;');
               codNameTag.append(name);
               searchBox.attr('value','');
           };
           var changeValueForHidden = function (val) {
               $("#nombreempresaslida").empty();
               $("#nombreempresaslida").append(val.Nombre);
           };
           function clickEmpresaSalida(id,name) {
               $('#EmpresaSalida').attr("value", id);
               $('#nombreempresaslida').empty();
               $('#nombreempresaslida').append(name);
               $('#busquedaempresasalida').attr('value', '');
           };
           $(document).ready(function () {
               $('.calendar').datepicker($.datepicker.regional["<%=h.GetCulture().Split(" - ")(0)%>"]);
               textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#busquedaproveedor'), OneElementToAppend, ManyElementsToAppend);
               getValueForHidden('<%= ResolveClientUrl("~/json/GetProveedor")%>' + '?id=' + $("#EmpresaSalida").attr("value"), changeValueForHidden);
               textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#busquedaempresasalida'), OneElementToAppend2, ManyElementsToAppend2);
           if ($('#Numord').attr('value') == 0) {
               $('#Numord').attr('value', '');
           };
       });
       //numord('<%=Url.Action("BuscarNumord","json") %>', 'Numord', 'divof','Numope');
       //numope('<%=Url.Action("BuscarNumope","json") %>', 'Numope','Numord')
   </script>
</asp:Content>
