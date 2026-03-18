<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Cheques guardería")%> 
    </title>
    <style type="text/css">
        .left{float:left;}
        .right{margin-left:17em;}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h4>
        <%= h.Traducir("Ejercicio")%>: <%=ViewData("ejercicio") %> - 
        <%= h.Traducir("Mes")%>: <%=ViewData("mes")%>
    </h4>
    <div class="left">
    <form action="<%=url.action("exportepsilon",new with{.yearmonth=Request("yearmonth")})%>" method="post">
        <fieldset>
        <input id="typeguarderia" type="checkbox" name="type" value="guarderia" /> <label for="typeguarderia"> <%= h.Traducir("Guarderia")%></label><br />
        <input id="typegourmet" type="checkbox" name="type" value="gourmet" /> <label for="typegourmet"> <%= h.Traducir("Gourmet")%></label><br />
        <input id="typelagunaro" type="checkbox" name="type" value="lagunaro" /> <label for="typelagunaro"> <%= h.traducir("Beneficiarios Lagunaro")%></label><br />
        <input id="typecargos" type="checkbox" name="type" value="cargos" /> <label for="typecargos"> <%= h.Traducir("Cargos")%></label><br />

        <input type="submit" value="<%= h.Traducir("Exportar para Epsilon")%> " />
        </fieldset>
    </form>

    <%If ViewData("existenguarderiasnuevas") Then%>
    <a class="mine-button red" href="<%=url.action("ExportGuarderiaListado",new with{.yearmonth=Request("yearmonth")})%>" ><%= h.Traducir("Listado de niños y guarderias")%></a>
    <form action="<%=url.action("import",new with{.yearmonth=Request("yearmonth")})%>" method="post" enctype="multipart/form-data">
        <input type="file"  name="filewithids" /><br />
        <input type="submit" value="<%= h.Traducir("Añadir nuevas guarderias")%> " />
    </form>
    <%Else%>
    <a class="mine-button" href="<%=url.action("ExportGuarderiaListado",new with{.yearmonth=Request("yearmonth")})%>" ><%= h.Traducir("Listado de niños y guarderias")%></a>
    <a class="mine-button" href="<%=url.action("ExportGuarderiaPedido",new with{.yearmonth=Request("yearmonth")})%>" ><%= h.Traducir("Generar pedido mensual")%></a>
    <%End If%>
    </div>
    <div class="right">
    <div class="left">
    <table class="table2">
        <caption>
            <%= h.Traducir("Cheque Guarderia")%>
        </caption>
        <thead>
            <tr>
                <th><%= h.Traducir("Nº trabajador")%> </th>
                <th><%= h.Traducir("Nombre")%> </th>
                <th><%= h.Traducir("Importe")%> </th>
                <th><%= h.Traducir("Tramite")%> </th>
                <th><%= h.Traducir("Nombre guarderia")%> </th>
            </tr>
        </thead>
        <tbody>
            <%For Each e In ViewData("solicitudesguarderia")%>
                <tr>
                    <td><%=e.idTrabajador%></td>
                    <td><%=e.nombre%></td>
                    <td><%=e.Importe%> €</td>
                    <td><%=CDec(e.Tramite).ToString("###.##")%> €</td>
                    <td><%=e.NombreGuarderia%></td>
                </tr>
            <%Next%>
        </tbody>
        <tfoot>
            <tr>
            <th colspan="2" align="center"><%=h.traducir("Total") %></th>
            <th colspan="2" align="center">
                <%= ctype(ViewData("solicitudesguarderia"), IEnumerable(Of Object)).Sum(Function(sg) sg.importe) + CType(ViewData("solicitudesguarderia"), IEnumerable(Of Object)).Sum(Function(sg) sg.tramite) %> €
            </th>
                <th></th>
                </tr>
        </tfoot>
    </table>
    </div>
    <div class="left">
    <table class="table2">
        <caption>
            <%= h.Traducir("Cheque Gourmet")%>
        </caption>
        <thead>
            <tr>
                <th><%= h.Traducir("Nº trabajador")%> </th>
                <th><%= h.Traducir("Nombre")%> </th>
                <th><%= h.Traducir("Importe")%> </th>
                <th><%= h.Traducir("Tramite")%> </th>
            </tr>
        </thead>
        <tbody>
            <%For Each e In ViewData("solicitudesgourmet")%>
                <tr>
                    <td><%=e.idTrabajador%></td>
                    <td><%=e.nombre%> <%=e.apellido1%> <%=e.apellido2%></td>
                    <td><%=e.Importe%> €</td>
                    <td><%=e.Tramite%> €</td>
                </tr>
            <%Next%>
        </tbody>
         <tfoot>
            <tr>
            <th colspan="2" align="center"><%=h.traducir("Total") %></th>
            <th colspan="2" align="center">
                <%= ctype(ViewData("solicitudesgourmet"), IEnumerable(Of Object)).Sum(Function(sg) sg.importe) + CType(ViewData("solicitudesgourmet"), IEnumerable(Of Object)).Sum(Function(sg) sg.tramite) %> €
            </th>
                </tr>
        </tfoot>
    </table>
    </div>
    </div>
    <br style="clear:left;" />
</asp:Content>
