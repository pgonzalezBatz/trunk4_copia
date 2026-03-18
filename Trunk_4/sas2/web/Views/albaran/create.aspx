<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Listado de albaranes")%>
    </title>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMenu2" runat="server">
    <ul>
        <li><a href="<%=Url.Action("list","albaran") %>"><%= h.Traducir("Listado")%></a></li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido2" runat="server">
    1.-
    <%= Html.ValidationSummary()%>
    <form method="post" action="SaveAlbaran">
        <fieldset>
            <legend><%= h.Traducir("Crear Albarán")%></legend>
            <strong><%= h.Traducir("Observaciones de albarán")%></strong><br />
            <%= Html.TextArea("observacion")%><br />
            <br />

            <strong><%= h.Traducir("Dirección de envio")%></strong><br />
            <div id="spanProveedor2">
                <%= ViewData("direcionenvio").Calle%><br />
                <%= ViewData("direcionenvio").CodigoPostal%>,<%= ViewData("direcionenvio").Poblacion%><br />
                <%= ViewData("direcionenvio").Provincia%><br />
                <%= ViewData("direcionenvio").Pais%><br />
            </div>
            <a id="acambiodireccion" href="<%=Url.Action("helbide") %>?<%= Regex.Replace(Request.QueryString.ToString, "&idhelbide=\d*", "")%>"><%= h.Traducir("Cambiar la dirección de envio")%></a>
            <%= Html.Hidden("idHelbide")%>
            <%= Html.Hidden("idproveedordestino")%>

            <div id="buscador">
                <%= h.Traducir("Buscar dirección")%>
                <%= Html.TextBox("direccion")%>
            </div>
            <br />
            <br />
            <input type="submit" value="<%= h.Traducir("Crear albarán")%>" />
        </fieldset>
        <%For Each a As web.Agrupacion In Model%>
        <input type="hidden" name="bultos" value="<%=a.Id %>" />
        <%Next%>
    </form>
    2.-
     <form method="post" action="addtoexistingalbaran">
         <fieldset>
             <legend><%= h.Traducir("Añadir bultos a albarán existente")%></legend>
             <%=Html.TextBox("albaran")%><br />
             <input type="submit" value="<%= h.Traducir("Añadir bulto a albaran existente")%>" />
         </fieldset>
         <%For Each a As web.Agrupacion In Model%>
         <input type="hidden" name="bultos" value="<%=a.Id %>" />
         <%Next%>
     </form>


    <table class="table1">
        <thead>
            <tr>
                <th><%= h.Traducir("Bulto Nº")%></th>
                <th><%= h.Traducir("Peso bulto")%></th>
                <th><%= h.Traducir("OF")%></th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Peso")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Observación")%></th>
                <th><%= h.Traducir("Negocio")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Agrupacion In Model%>
            <%If a.ListOfMovimiento.Count = 0 %>
                <tr>
                    <td><%= a.Id%></td>
                    <td><%= a.Peso%></td>
                </tr>
            <%End If %>
            <%Dim first = True%>
            <%For Each m In a.ListOfMovimiento%>
            <%If web.h.GetListOfDefaultEmpresaFromStrCn(   ConfigurationManager.ConnectionStrings("SAS").ConnectionString).Exists(Function(o) o.id = m.EmpresaSalida) Then%>
            <tr>
                <%Else%>
            <tr class="recogida">
                <%End If%>
                <% If first Then%>
                <td rowspan="<%=a.ListOfMovimiento.count() %>">
                    <%= a.Id%>
                </td>
                <td rowspan="<%=a.ListOfMovimiento.count() %>"><%= Decimal.Round(a.ListOfMovimiento.Sum(Function(o) o.Peso))%></td>
                <%first = False%>
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
                <td><%= m.Peso.Value.ToString("0.##")%></td>
                <td><%= m.NombreSab%></td>
                <td><%=m.Observacion %></td>
                <td><%=m.Negocio %></td>
            </tr>
            <%Next%>
            <%Next%>
        </tbody>
    </table>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="//intranet2.batz.es/baliabideorokorrak/textbox_search.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#buscador").hide();
            $("#acambiodireccion").click(function () {
                $("#buscador").show();
                return false;
            });
            var direccion = $("#spanProveedor2");
            function fOneElement(val) {
                clickDireccion(val);
            };
            function fManyElements(div, val, f) {
                div.append('<a id="helbi' + val.Id.toString() + '" href="#">' + val.Calle + ' ' + val.CodigoPostal + ' ' + val.Poblacion + ' ' + val.Pais + '</a><br/>');
                f($('#helbi' + val.Id.toString()), function () { clickDireccion(val) });
            };
            function clickDireccion(val) {
                $('#idHelbide').attr("value", val.Id);
                direccion.empty()
                direccion.append(val.Calle + '<br/>' + val.CodigoPostal + '<br/>' + val.Poblacion + '<br/>' + val.Pais)
                direccion.attr("style", "color:green;font-weight:bold;");
            };
            textboxSearch('<%= Url.Action("Buscar", "json")%>?q=', $("#direccion"), fOneElement, fManyElements);
                });
    </script>
</asp:Content>

