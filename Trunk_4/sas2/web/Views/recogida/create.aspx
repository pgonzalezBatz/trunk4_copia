<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(of web.recogida)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Listado movimiento de recogidas")%>
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        span{font-weight:bold;}
        ol{padding-left:1.9em;}
        ol li input[type=text]{width:7.9em;}
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <%= Html.ValidationSummary()%>
    <form action="" method="post">
        <fieldset>
            <legend><%= h.traducir("Recogidas")%></legend>
            
            <strong><%= h.Traducir("Negocio") %></strong><br />
            <%= Html.DropDownListFor(Function(m) m.IdNegocio, CType(ViewData("negocios"), List(Of SelectListItem)))%><br />

                <strong><%= h.traducir("Empresa de recogida")%>
                <%=Html.ValidationMessage("IdEmpresaRecogida", "*")%>
                </strong><br />
                <span id="spanempresarecogida"><%= ViewData("spanempresarecogida")%></span>
                <%= Html.HiddenFor(Function(m) m.IdEmpresaRecogida)%><br />
                <input type="text" id="buscarempresarecogida" autocomplete="off" /><br />
            <strong><%= h.traducir("Dirección alternativa de recogida")%><br />
                <%=Html.TextArea("observacionesdireccion") %><br />

            <strong ><%= h.traducir("Empresa de entrega")%></strong><br />
            <span id="spanempresaentrega" style="color:Green;"><%= ViewData("spanempresaentrega")%></span>
            <%= Html.HiddenFor(Function(m) m.IdEmpresaEntrega)%><br />
            <input type="text" id="buscarempresaentraga" autocomplete="off" /><br />


            <strong><%= h.traducir("Fecha de recogida")%></strong><br />
            <%= Html.EditorFor(Function(m) m.Fecha)%><br />


            <strong><%= h.traducir("Observaciones")%></strong><br />
            <%=Html.TextAreaFor(Function(m) m.Observacion)%>
            <br /><br />    
            <strong><%= h.traducir("OF")%></strong> - 
            <strong><%= h.traducir("OP")%></strong>
            <%=Html.ValidationMessage("ofop","*")%>
            <br />
            <ol>
                <%For i = 0 To ViewData("ofopcount")%>
                    <li>
                        <%If Model.ListOfOp IsNot Nothing AndAlso Model.ListOfOp.Count > i %>
                        <%= Html.TextBox("ListOfOp[" + i.ToString + "].numord", Model.ListOfOp(i).Numord)%>
                     -
                    <%= Html.TextBox("ListOfOp[" + i.ToString + "].numope", Model.ListOfOp(i).Numope)%>
                    : <%= Html.TextBox("ListOfOp[" + i.ToString + "].peso", Model.ListOfOp(i).Peso)%>
                        <%--<%=Html.DropDownList("ListOfOp[" + i.ToString + "].puerta", New Mvc.SelectList(ViewData("puertas"), "value", "text", Model.ListOfOp(i).puerta), "Seleccionar")                             %>--%>
                        <%else %>
                    <%= Html.TextBox("ListOfOp[" + i.ToString + "].numord")%>
                    - <%= Html.TextBox("ListOfOp[" + i.ToString + "].numope")%>
                    : <%= Html.TextBox("ListOfOp[" + i.ToString + "].peso")%>

                        <%--<%=Html.DropDownList("ListOfOp[" + i.ToString + "].puerta", CType(ViewData("puertas"), IEnumerable(Of SelectListItem)), "Seleccionar")                             %>--%>
                        <%End If %>
                    
                    
                    </li>
                <%Next%>
            </ol>
            <%=Html.Hidden("ofopcount")%>
            <input type="submit" value="<%= h.Traducir("Continuar")%>" />
        </fieldset>
    </form>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-1.11.2.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-<%=h.GetCulture().Split("-")(0) %>.js' type="text/javascript"></script>
    <script src="../Scripts/JScript1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OneElementToAppend1(val) {
            f1(val.Id, val.Nombre);
            $('#buscarempresaentraga').focus();
        };
        function OneElementToAppend2(val) {
            f2(val.Id, val.Nombre);
            $('#Fecha').focus();
        };
        function ManyElementsToAppend1(el, val, fun) {
            el.append('<a id="re' + val.Id.toString() + '" href="#">' + val.Nombre + '</a><br/>');
            fun($('#re' + val.Id.toString()), function () { f1(val.Id, val.Nombre) });
        };
        function ManyElementsToAppend2(el, val, fun) {
            el.append('<a id="en' + val.Id.toString() + '" href="#">' + val.Nombre + '</a><br/>');
            fun($('#en' + val.Id.toString()), function () { f2(val.Id, val.Nombre) });
        };
        function f1(id, name) {
            $('#IdEmpresaRecogida').attr("value", id);
            $('#spanempresarecogida').empty();
            $('#spanempresarecogida').attr('style', 'color:green;');
            $('#spanempresarecogida').append(name);
            $('#buscarempresarecogida').attr('value', '');
        };
        function f2(id, name) {
            $('#IdEmpresaEntrega').attr("value", id);
            $('#spanempresaentrega').empty();
            $('#spanempresaentrega').attr('style', 'color:green;');
            $('#spanempresaentrega').append(name);
            $('#buscarempresaentraga').attr('value', '');
        };
        $(document).ready(function () {
             $('.calendar').datepicker($.datepicker.regional["<%=h.GetCulture().Split(" - ")(0)%>"]);
            textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#buscarempresarecogida'), OneElementToAppend1, ManyElementsToAppend1);
            textboxSearch('<%= ResolveClientUrl("~/json")%>' + '/BuscarProveedor?q=', $('#buscarempresaentraga'), OneElementToAppend2, ManyElementsToAppend2);
            
        });
    </script>
</asp:Content>



