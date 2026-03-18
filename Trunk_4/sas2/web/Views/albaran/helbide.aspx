<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title><%= h.Traducir("Direccion de envio")%></title>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <form action="<%=Url.Action("create")%>" method="get">
         <label>
            <%= h.Traducir("Buscar dirección")%>
            <%= Html.TextBox("direccion")%>
            <%= Html.Hidden("idHelbide")%>
             <%= Html.Hidden("bultos",Request("bultos"))%>
            <br />
             <input type="submit" value="<%=h.traducir("Continuar")%>" />
        </label>
        </form>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
            <script type="text/javascript" src="//intranet2.batz.es/baliabideorokorrak/textbox_search.js"></script>
            <script type="text/javascript">
                $(function () {
                    var direccion = $("#direccion");
                    function fOneElement(val) {
                        clickUser(val);
                    };
                    function fManyElements(div, val, f) {
                        div.append('<a id="helbi' + val.Id.toString() + '" href="#">' + val.Calle + ' ' + val.CodigoPostal + ' ' + val.Poblacion + ' ' + val.Pais + '</a><br/>');
                        f($('#helbi' + val.Id.toString()), function () { clickDireccion(val) });
                    };
                    function clickDireccion(val) {
                        $('#idHelbide').attr("value", val.Id);
                        direccion.attr("value", val.Calle + ' ' + val.CodigoPostal + ' ' + val.Poblacion + ' ' + val.Pais);
                        direccion.attr("style", "color:green;font-weight:bold;");
                    };
                    textboxSearch('<%= Url.Action("Buscar", "json")%>?q=', direccion, fOneElement, fManyElements);
                });
            </script>
</asp:Content>
