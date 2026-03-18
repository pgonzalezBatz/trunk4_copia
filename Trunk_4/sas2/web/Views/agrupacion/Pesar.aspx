<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Pesar bulto")%>
    </title>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        span{color:Green;}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h2><%= h.Traducir("Que quieres hacer con los movimientos seleccionados?")%></h2>
    1.-
    <%= Html.ValidationSummary()%>
    <form action="<%=url.action("create") %>" method="post">
    <fieldset>
        <legend><%= h.Traducir("Crear bulto")%></legend>
        <strong> <%= h.Traducir("Suma de las marcas seleccionadas=")%></strong><%= ViewData("total")%> Kg<br />
        <strong><%= h.Traducir("Peso bulto")%></strong><br />
        <%= Html.TextBox("pesoBulto")%>
        <br />
        <%For Each m As web.Movimiento In Model%>
            <input type="hidden" value="<%=m.Id %>" name="agrupar" />
        <%Next%>
        <input type="submit" value="<%= h.Traducir("Guardar")%>" />
    </fieldset>
    </form>

    2.-
    <form action="<%=url.action("editlist","movimientosmaterial") %>" method="post">
    <fieldset>
        <legend><%= h.Traducir("Editar los movimientos")%></legend>
        <%For Each m As web.Movimiento In Model%>
            <input type="hidden" value="<%=m.Id %>" name="agrupar" />
        <%Next%>

        <strong><%= h.Traducir("Código de proveedor")%></strong><br />
        <span id="spanProveedor">
            <%=Model(0).NombreProveedor %>
        </span>
        <%= Html.Hidden("proveedor")%><br />
        <input type="text" id="busquedaproveedor" autocomplete="off" /><br />
        <strong><%= h.Traducir("Empresa de salida")%></strong><br />
        <%= Html.Hidden("EmpresaSalida", Model(0).EmpresaSalida)%><br />
        <span id="nombreempresaslida">
            <%=Model(0).NombreEmpresaSalida %>
        </span><br />
        <input type="text" id="busquedasalida" autocomplete="off"/><br />
        <input type="submit" value="<%= h.Traducir("Editar")%>" />
    </fieldset>
    </form>
    3.-
     <form action="<%=url.action("deletelist","movimientosmaterial") %>" method="post">
    <fieldset>
        <legend><%= h.Traducir("Eliminar los movimientos")%></legend>
        <%For Each m As web.Movimiento In Model%>
            <input type="hidden" value="<%=m.Id %>" name="agrupar" />
        <%Next%>
        <input type="submit" value="<%= h.Traducir("Eliminar Todos")%>" />
    </fieldset>
    </form>
    <table class="table1">
        <thead>
            <tr>
                <th><%= h.Traducir("OF")%></th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Peso")%></th>
                <th><%= h.Traducir("Proveedor")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Observación")%></th>
            </tr>
        </thead>
        <tbody>
        <%For Each m As web.Movimiento In Model%>
            <%If web.h.GetListOfDefaultEmpresaFromStrCn( ConfigurationManager.ConnectionStrings("SAS").ConnectionString).Exists(Function(o) o.id = m.EmpresaSalida) Then%>
                <tr>
            <%Else%>
                <tr class="recogida">
            <%End If%>
                    <td><%= m.Numord%></td>
                    <td><%= m.Numope%></td>
                    <td><%= m.Marca%></td>
                    <td>
                    <%If m.FechaEntrega.HasValue Then%>
                        <%= m.FechaEntrega.Value.ToShortDateString%>
                    <%End If%>
                    </td>
                    <td><%= m.Cantidad%></td>
                    <td><%= m.Peso%></td>
                    <td><%= m.NombreProveedor%></td>
                    <td><%= m.NombreSab%></td>
                    <td><%= m.Observacion %></td>
            </tr>
        <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="9">
                    
                </td>
            </tr>
        </tfoot>
        </table>
    <script src="../Scripts/JScript1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OneElementToAppend(val) {
            f(val.Id, val.Nombre);
            $('#busquedasalida').focus();
        };
        function OneElementToAppend2(val) {
            clickEmpresaSalida(val.Id, val.Nombre);
            $('#busquedaempresasalida').empty();
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
            var codproBox = $('#proveedor');
            var codNameTag = $('#spanProveedor');
            var searchBox = $('#busquedaproveedor');
            codproBox.attr("value", id);
            codNameTag.empty();
            codNameTag.attr('style', 'color:green;');
            codNameTag.append(name);
            searchBox.attr('value', '');
            $('#busquedasalida').focus();
        };
        var changeValueForHidden = function (val) {
            $("#busquedasalida").empty();
            $("#busquedasalida").append(val.Nombre);
        };
        function clickEmpresaSalida(id, name) {
            $('#EmpresaSalida').attr("value", id);
            $('#nombreempresaslida').empty();
            $('#nombreempresaslida').append(name);
            $('#busquedasalida').attr('value', '');
        };
        $(document).ready(function () {
            textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#busquedaproveedor'), OneElementToAppend, ManyElementsToAppend);
            //getValueForHidden('<%= ResolveClientUrl("~/json/GetProveedor")%>' + '?id=' + $("#EmpresaSalida").attr("value"), changeValueForHidden);
            textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#busquedasalida'), OneElementToAppend2, ManyElementsToAppend2);
        });
    </script>
</asp:Content>
