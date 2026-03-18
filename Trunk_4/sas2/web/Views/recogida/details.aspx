<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(of web.recogida)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Listado movimiento de recogidas")%>
    </title>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/ui.datepicker.es-ES.js' type="text/javascript"></script>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('.calendar').datepicker();
        });
    </script>
    <style type="text/css">
        span{font-weight:bold;}
        ol{padding-left:1.9em;}
        ol li input[type=text]{width:7.9em;}
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <h3><%=h.traducir("Recogida")%> (<%=h.traducir("Facturar a troquelería")%>)</h3>
    <strong><%=h.traducir("Empresa de recogida")%></strong><br />
    <%=Model.nombreEmpresaRecogida%><br />
    <%=ViewData("proveedorRecogida").Calle%><br />
    <%=ViewData("proveedorRecogida").CodigoPostal%> <%=ViewData("proveedorRecogida").poblacion%><br />
    <%=ViewData("proveedorRecogida").Provincia%><br />
    <%=ViewData("proveedorRecogida").Pais%><br />
    <%=h.traducir("Telf")%>: <%=ViewData("proveedorRecogida").Telefono%><br />
    <strong><%=h.traducir("Empresa de entrega")%></strong><br />
    <%=Model.nombreEmpresaEntrega%><br />
    <%=ViewData("proveedorEntrega").Calle%><br />
    <%=ViewData("proveedorEntrega").CodigoPostal%> <%=ViewData("proveedorEntrega").poblacion%><br />
    <%=ViewData("proveedorEntrega").Provincia%><br />
    <%=ViewData("proveedorEntrega").Pais%><br />
    <%=h.traducir("Telf")%>: <%=ViewData("proveedorEntrega").Telefono%><br />
    <strong><%=h.traducir("Fecha de recogida")%></strong><br />
    <%=Model.Fecha.Value.ToShortDateString%><br />
    <strong><%=h.traducir("Observaciones")%></strong><br />
<pre><%=Model.Observacion%></pre><br />

    <table class="table1">
        <thead>
            <tr>
                <th>Of</th>
                <th>OP</th>
                <th>Peso</th>
            </tr>
        </thead>
        <tbody>
            <%For Each o In Model.ListOfOp%>
        <tr>
            <td><%=o.Numord%></td>
            <td><%=o.Numope%></td>
            <td><%=o.Peso%> Kg</td>
        </tr>
    <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="2">Peso total</th>
                <th><%=Model.ListOfOp.Sum(Function(o) o.Peso)%> Kg</th>
            </tr>
        </tfoot>
    </table>
    
</asp:Content>