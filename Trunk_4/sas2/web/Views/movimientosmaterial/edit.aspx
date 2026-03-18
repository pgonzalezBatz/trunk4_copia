<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of web.Movimiento)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Editar movimiento de material")%>
    </title>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        .tag{color:Green;font-weight:bold;}
    </style>
</asp:Content>

<asp:Content  ContentPlaceHolderID="cphContenido2" runat="server">
    <%= Html.ValidationSummary()%>
    <form method="post" action="">
        <fieldset>
            <legend><%= h.traducir("Editar linea")%></legend>
            <strong><%= h.traducir("OF")%></strong><br />
            <%= Html.TextBoxFor(Function(m) m.Numord)%><br />
            <strong><%= h.traducir("OP")%></strong><br />
            <%= Html.DropDownListFor(Function(m) m.Numope, CType(ViewData("operaciones"), Generic.List(Of SelectListItem)))%><br />
            <strong><%= h.traducir("Código de proveedor")%></strong><br />
            <%= Html.HiddenFor(Function(m) m.CodPro)%>
            <span id="spanProveedor" class="tag"><%= Html.DisplayFor(Function(m) m.NombreProveedor)%></span><br />
            <input type="text" id="busquedaproveedor" autocomplete="off" /><br />
            <strong><%= h.traducir("Empresa de salida")%></strong><br />
            <%= Html.HiddenFor(Function(m) m.EmpresaSalida)%>
            <span id="nombreempresaslida" class="tag"><%= Html.DisplayFor(Function(m) m.NombreEmpresaSalida)%></span><br />
            <input type="text" id="busquedaempresasalida" autocomplete="off" /><br />

            <strong><%= h.traducir("Fecha prevista de entrega")%></strong><br />
            <%= Html.TextBoxFor(Function(m) m.FechaEntrega)%><br />
            
            <strong><%= h.traducir("Marca")%></strong><br />
            <%= Html.TextBoxFor(Function(m) m.Marca)%><br />
            <strong><%= h.traducir("Cantidad")%></strong><br />
            <%= Html.TextBoxFor(Function(m) m.Cantidad)%><br />
            <strong><%= h.traducir("Peso")%></strong><br />
            <%= Html.TextBoxFor(Function(m) m.Peso)%><br />
            <strong><%= h.traducir("Observación")%></strong><br />
            <%= Html.TextAreaFor(Function(m) m.Observacion)%><br />

            <input type="submit" value="<%= h.Traducir("Guardar")%>" />
        </fieldset> 
    </form>
   <script src="../Scripts/JScript1.js" type="text/javascript"></script>
   <script type="text/javascript">
       function OneElementToAppend(val) {
           f(val.Id, val.Nombre);
           $('#FechaEntrega').focus();
       };
       function OneElementToAppend2(val) {
           clickEmpresaSalida(val.Id, val.Nombre);
           $('#busquedaempresasalida').empty();
           $('input[type=submit]').focus();
       };
       function ManyElementsToAppend(el, val, fun) {
           el.append('<a id="pro' + val.Id.toString() + '" href="#">' + val.Nombre + '</a><br/>');
           fun($('#pro' + val.Id.toString()), function () { f(val.Id, val.Nombre) });
       };
       function ManyElementsToAppend2(el, val, fun) {
           el.append('<a id="emp' + val.Id.toString() + '" href="#">' + val.Nombre + '</a><br/>');
           fun($('#emp' + val.Id.toString()), function () { clickEmpresaSalida(val.Id, val.Nombre) });
       };
       function f(id, name) {
           var codproBox = $('#CodPro');
           var codNameTag = $('#spanProveedor');
           var searchBox = $('#busquedaproveedor');
           codproBox.attr("value", id);
           codNameTag.empty();
           codNameTag.append(name);
           searchBox.attr('value', '');
       };
       function clickEmpresaSalida(id, name) {
           $('#EmpresaSalida').attr("value", id);
           $('#nombreempresaslida').empty();
           $('#nombreempresaslida').append(name);
           $('#busquedaempresasalida').attr('value', '');
       };
       $(document).ready(function () {
           textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#busquedaproveedor'), OneElementToAppend, ManyElementsToAppend);
           textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#busquedaempresasalida'), OneElementToAppend2, ManyElementsToAppend2);
       });
   </script>
</asp:Content>