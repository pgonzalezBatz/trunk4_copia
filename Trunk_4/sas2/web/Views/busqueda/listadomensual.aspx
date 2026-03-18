<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Listado mensual")%> <%=Request("ejercicio")%>/<%=Request("mes")%>
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <div style="margin-left:1.4em;">
    <strong><%=h.traducir("Proveedor")%></strong><br />
    <%=ViewData("transportista").nombre%><br />
    <%=ViewData("transportista").cif%><br />
    <%=ViewData("transportista").calle%><br />
    <%=ViewData("transportista").codigopostal%>, <%=ViewData("transportista").poblacion%><br />
    <%=ViewData("transportista").provincia%><br />
    <%=ViewData("transportista").pais%><br />
        </div>
    <table class="table1">
        <thead>
            <tr>
                <th><%=h.Traducir("Negocio")%></th>
                <th><%=h.traducir("Nº viaje")%></th>
                <th><%=h.traducir("Nº pedido")%></th>
                <th><%=h.traducir("Importe SAS")%></th>
                <th><%=h.traducir("Importe XBAT")%></th>
                <th><%=h.traducir("Fecha salida")%></th>
               <%If Request("taxista") Is Nothing Then%>
                <th><%=h.traducir("Salidas")%></th>
                <th><%=h.traducir("Recogidas")%></th>
                <th><%=h.traducir("Tipo")%></th>
                <th><%=h.traducir("Observaciones")%></th>
                <%Else%>
                <th><%=h.traducir("Observaciones")%></th>
                <th><%=h.traducir("Ruta")%></th>
                <% End If%>
            </tr>
        </thead>
        <tbody>
            <%For Each v In Model%>
                <tr>
                    <td><%=v.negocio%></td>
                    <td><%=v.idviaje%></td>
                    <td><%=v.npedido%></td>
                    <td><%=v.importesas%></td>
                    <td><%=v.importexbat%></td>
                    <td><%=v.fechasalida.toshortdatestring%></td>
                    <td><%=v.Otros%></td>
                    <td>
                        <%If v.Origen isnot DBNull.Value Then%>
                        <%=v.Origen%>-><%=v.Destino%>
                        <%End If%>
                    </td>
                    <td>
                        <%If IsNumeric(v.tiempo) AndAlso v.tiempo = 1 Then%>
                            Facturación por horas
                        <%End If%>
                    </td>
                    <td>
                        <%=v.comentarioAlmacen%>
                    </td>
                </tr>
            <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="4"></th>
                <th><%= ViewData("importexbat")%></th>
                <th colspan="4"></th>
            </tr>
        </tfoot>
    </table>

    <%If Request("taxista") IsNot Nothing Then%>
    <table class="table1">
        <thead>
            <tr>
                <th><%=h.Traducir("Negocio")%></th>
                <th><%=h.Traducir("Nº viaje")%></th>
                <th><%=h.traducir("Nº pedido")%></th>
                <th><%=h.traducir("Importe SAS")%></th>
                <th><%=h.traducir("Importe XBAT")%></th>
                <th><%=h.traducir("Fecha salida")%></th>
               <%If Request("taxista") Is Nothing Then%>
                <th><%=h.traducir("Salidas")%></th>
                <th><%=h.traducir("Recogidas")%></th>
                <th><%=h.traducir("Tipo")%></th>
                <th><%=h.traducir("Observaciones")%></th>
                <%Else%>
                <th><%=h.traducir("Observaciones")%></th>
                <th><%=h.traducir("Ruta")%></th>
                <% End If%>
            </tr>
        </thead>
        <tbody>
            <%For Each v In ViewData("taxiSubcontratado")%>
                <tr>
                    <td><%=v.Negocio%></td>
                    <td><%=v.idviaje%></td>
                    <td><%=v.npedido%></td>
                    <td><%=v.importesas%></td>
                    <td><%=v.importexbat%></td>
                    <td><%=v.fechasalida.toshortdatestring%></td>
                    <td><%=v.Otros%></td>
                    <td>
                        <%If v.Origen isnot DBNull.Value Then%>
                        <%=v.Origen%>-><%=v.Destino%>
                        <%End If%>
                    </td>
                    <td>
                        <%If IsNumeric(v.tiempo) AndAlso v.tiempo = 1 Then%>
                            Facturación por horas
                        <%End If%>
                    </td>
                    <td>
                        <%=v.comentarioAlmacen%>
                    </td>
                </tr>
            <%Next%>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="3"></th>
                <th><%=  ViewData("importexbatSubconstratado")%></th>
                <th colspan="4"></th>
            </tr>
        </tfoot>
    </table>
    <%End If%>
</asp:Content>
