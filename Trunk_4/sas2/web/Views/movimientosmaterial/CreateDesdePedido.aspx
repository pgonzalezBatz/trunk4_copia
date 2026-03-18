<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

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
            <%= Html.DropDownList("idNegocio", CType(ViewData("negocios"), IEnumerable(Of SelectListItem)))%><br />
            <strong><%= h.traducir("Nº Pedido")%></strong> 
            <br />
            <%= Html.TextBox("npedido")%>
            <br />
            <strong><%= h.traducir("Código de proveedor")%></strong><br />
            <span id="spanProveedor"><%= h.traducir("Selecciona un proveedor:")%></span>
            <%= Html.Hidden("CodPro")%><br />
            <input type="text" id="busquedaproveedor" autocomplete="off" /><br />
            <strong><%= h.traducir("Fecha prevista de entrega")%></strong><br />
            <%= Html.TextBox("FechaEntrega", Nothing, New With {.class = "calendar"})%><br />
            <input type="submit" value="<%=h.traducir("Continuar")%>" />
        </fieldset>
    </form>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-1.11.2.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-<%=h.GetCulture().Split("-")(0) %>.js' type="text/javascript"></script>
      <script src="../Scripts/JScript1.js" type="text/javascript"></script>
   <script type="text/javascript">
           function OneElementToAppend(val) {
               f(val.Id, val.Nombre);
               $('#FechaEntrega').focus();
           };
           function ManyElementsToAppend(el,val, fun) {
               el.append('<a id="pro' + val.Id.toString() + '" href="#">' + val.Nombre + '</a><br/>');
               fun($('#pro' + val.Id.toString()),  function () { f(val.Id,val.Nombre) });
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
           
           $(document).ready(function () {
               $('.calendar').datepicker($.datepicker.regional["<%=h.GetCulture().Split(" - ")(0)%>"]);
           textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#busquedaproveedor'), OneElementToAppend, ManyElementsToAppend);
           if ($('#Numord').attr('value') == 0) {
               $('#Numord').attr('value', '');
           };
       });
   </script>
</asp:Content>
