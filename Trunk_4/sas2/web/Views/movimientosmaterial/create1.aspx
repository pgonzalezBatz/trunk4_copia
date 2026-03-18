<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of Web.MovimientoBase)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
        <title>
            <%= h.traducir("Crear movimiento de material")%>
        </title>
        <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            function UpdateTableHeaders() {
                $("div.divTableWithFloatingHeader").each(function () {
                    var originalHeaderRow = $(".tableFloatingHeaderOriginal", this);
                    var floatingHeaderRow = $(".tableFloatingHeader", this);
                    var offset = $(this).offset();
                    var scrollTop = $(window).scrollTop();
                    if ((scrollTop > offset.top) && (scrollTop < offset.top + $(this).height())) {
                        floatingHeaderRow.css("visibility", "visible");
                        floatingHeaderRow.css("top", Math.min(scrollTop - offset.top, $(this).height() - floatingHeaderRow.height()) + "px");

                        // Copy cell widths from original header
                        $("th", floatingHeaderRow).each(function (index) {
                            var cellWidth = $("th", originalHeaderRow).eq(index).css('width');
                            $(this).css('width', cellWidth);
                        });

                        // Copy row width from whole table
                        floatingHeaderRow.css("width", $(this).css("width"));
                    }
                    else {
                        floatingHeaderRow.css("visibility", "hidden");
                        floatingHeaderRow.css("top", "0px");
                    }
                });
            }

            $(document).ready(function () {
                $("table.table1").each(function () {
                    $(this).wrap("<div class=\"divTableWithFloatingHeader\" style=\"position:relative\"></div>");

                    var originalHeaderRow = $("tr:first", this)
                    originalHeaderRow.before(originalHeaderRow.clone());
                    var clonedHeaderRow = $("tr:first", this)

                    clonedHeaderRow.addClass("tableFloatingHeader");
                    clonedHeaderRow.css("position", "absolute");
                    clonedHeaderRow.css("top", "0px");
                    clonedHeaderRow.css("left", $(this).css("margin-left"));
                    clonedHeaderRow.css("visibility", "hidden");

                    originalHeaderRow.addClass("tableFloatingHeaderOriginal");
                });
                UpdateTableHeaders();
                $(window).scroll(UpdateTableHeaders);
                $(window).resize(UpdateTableHeaders);
            });
    </script>
    <style type="text/css">
        .table1 tbody tr td input{width:50px;}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMenu2" runat="server">
    <ul>
        <li><a href="<%=Url.Action("list") %>"><%= h.traducir("Listado de movimientos")%></a></li>
    </ul>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <a href="<%=Url.Action("create0") %>"><%= h.traducir("Volver")%></a>
    <strong><%= h.Traducir("Negocio")%></strong>:
    <%= Html.DisplayFor(Function(m) m.Negocio)%>
    <strong><%= h.traducir("OF")%></strong>:
    <%= Html.DisplayFor(Function(m) m.Numord)%>
    <strong><%= h.traducir("OP")%></strong>:
    <%= Html.DisplayFor(Function(m) m.Numope)%>
    <strong><%= h.traducir("Fecha")%></strong>:
    <%= Html.DisplayFor(Function(m) m.FechaEntrega)%>
    <strong><%= h.traducir("Código de proveedor")%></strong>:
    <%= Html.DisplayFor(Function(m) m.CodPro)%>
    <strong><%= h.traducir("Empresa salida")%></strong>:
    <%= Html.DisplayFor(Function(m) m.EmpresaSalida)%>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form action="<%=Url.action("create1") %>" method="post">
        <fieldset>
            <legend><%= h.traducir("Movimientos de material")%></legend>            
            <%= Html.HiddenFor(Function(m) m.IdNegocio)%>
            <%= Html.HiddenFor(Function(m) m.Numord)%>
            <%= Html.HiddenFor(Function(m) m.Numope)%>
            <%= Html.HiddenFor(Function(m) m.CodPro)%>
            <%= Html.HiddenFor(Function(m) m.FechaEntrega)%>
            <%= Html.HiddenFor(Function(m) m.EmpresaSalida)%>
            <strong><%= h.traducir("Descripcion general")%></strong>(<%= h.traducir("la descripción que se escripba aqui, sera aplicada a todas las marcas que se seleccionen")%>)<br />
            <%= Html.TextBox("descripciongeneral")%>
            <table class="table1">
                <thead>
                    <tr>
                        <th><%= h.traducir("Marca")%> </th>
                        <th><%= h.traducir("Cantidad sugerida")%></th>
                        <th><%= h.traducir("Cantidad")%></th>
                        <th><%= h.traducir("Peso")%></th>
                        <th><%= h.traducir("Ancho x Alto x Largo")%></th>
                        <th><%= h.traducir("Diametro")%></th>
                        <th><%= h.traducir("Observación")%></th>
                        <th><%= h.traducir("Anteriores")%></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <%= Html.Hidden("marca_troquelcompleto", "ZZZZ")%>
                            Troquel completo
                        </td>
                        <td>0</td>
                        <td><%= Html.TextBox("cantidad_troquelcompleto")%></td>
                        <td><%= Html.TextBox("peso_troquelcompleto", "1")%></td>
                        <td>
                            <%= Html.TextBox("ancho_troquelcompleto")%> x
                            <%= Html.TextBox("alto_troquelcompleto")%> x
                            <%= Html.TextBox("largo_troquelcompleto")%>    
                        </td>
                        <td>
                            <%= Html.TextBox("diametro_troquelcompleto")%> 
                        </td>
                        <td>
                            <%= Html.TextBox("observacion_troquelcompleto", " Troquel completo", New With {.style = "width:300px;"})%>    
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <%= Html.Hidden("marca_utilcontrol", "ZZZZ")%>
                            Util de control
                        </td>
                        <td>0</td>
                        <td><%= Html.TextBox("cantidad_utilcontrol")%></td>
                        <td><%= Html.TextBox("peso_utilcontrol", "1")%></td>
                        <td>
                            <%= Html.TextBox("ancho_utilcontrol")%> x
                            <%= Html.TextBox("alto_utilcontrol")%> x
                            <%= Html.TextBox("largo_utilcontrol")%>    
                        </td>
                        <td>
                            <%= Html.TextBox("diametro_utilcontrol")%> 
                        </td>
                        <td>
                            <%= Html.TextBox("observacion_utilcontrol", "Util de control", New With {.style = "width:300px;"})%>    
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <%= Html.Hidden("marca_chapa", "ZZZZ")%>
                            Chapa
                        </td>
                        <td>0</td>
                        <td><%= Html.TextBox("cantidad_chapa")%></td>
                        <td><%= Html.TextBox("peso_chapa", "1")%></td>
                        <td>
                            <%= Html.TextBox("ancho_chapa")%> x
                            <%= Html.TextBox("alto_chapa")%> x
                            <%= Html.TextBox("largo_chapa")%>    
                        </td>
                        <td>
                            <%= Html.TextBox("diametro_chapa")%> 
                        </td>
                        <td>
                            <%= Html.TextBox("observacion_chapa", "Chapa", New With {.style = "width:300px;"})%>    
                        </td>
                        <td></td>
                    </tr>
                    <%For Each e As web.Movimiento In ViewData("marcas")%>
                        <tr>
                            <td>
                                <%= Html.Hidden("marca_" + e.Marca, e.Marca)%>
                                <%=e.Marca %><br /><%= e.Material%>
                            </td>
                            <td><%= e.Cantidad%></td>
                            <td><%= Html.TextBox("cantidad_" + e.Marca)%></td>
                            <td><%= Html.TextBox("peso_" + e.Marca, e.Peso)%></td>
                            <td>
                                <%= Html.TextBox("ancho_" + e.Marca, e.Ancho)%> x
                                <%= Html.TextBox("alto_" + e.Marca, e.Alto)%> x
                                <%= Html.TextBox("largo_" + e.Marca, e.Largo)%>    
                            </td>
                            <td>
                                <%= Html.TextBox("diametro_" + e.Marca, e.Diametro)%> 
                            </td>
                            <td>
                                <%= Html.TextBox("observacion_" + e.Marca, e.Observacion,New With{.style="width:300px;"})%>    
                            </td>
                            <td>
                                <%=e.Otros.Salida %> 
                            </td>
                        </tr>
                    <%Next%>
                </tbody>
            </table>
            <input type="submit" value="<%= h.Traducir("Guardar")%>" />
        </fieldset>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table1 tr").each(function () {
                $(this).find("td:eq(2)").keyup(function (e) {
                    switch (e.keyCode) {
                        case 38:
                            $(this).parent('tr').prev('tr').find('td:eq(2) input').focus();
                            break;
                        case 40:
                            $(this).parent('tr').next('tr').find('td:eq(2) input').focus();
                            break;
                    };
                });
                $(this).find("td:eq(3)").keyup(function (e) {
                    switch (e.keyCode) {
                        case 38:
                            $(this).parent('tr').prev('tr').find('td:eq(2) input').focus();
                            break;
                        case 40:
                            $(this).parent('tr').next('tr').find('td:eq(2) input').focus();
                            break;
                    };
                });
            });
        });
    </script>
</asp:Content>
