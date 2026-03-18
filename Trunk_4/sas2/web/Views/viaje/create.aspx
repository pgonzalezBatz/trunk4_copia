<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Crear viaje")%>
    </title>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    1.-
    <form action="saveenvio" method="post">
        <fieldset>
            <legend><%= h.traducir("crear viaje")%></legend>
            <div style="float:left;">
                <strong><%= h.traducir("Transportista")%></strong><br />
                <select name="transportista">
                    <%For Each m In ViewData("Transportista")%>
                        <%If m.codprov = 8581 Then%>
                            <option value='<%= m.codProv%>' selected="selected"><%= m.nomProv%></option>
                        <%Else%>
                            <option value='<%= m.codProv%>'><%= m.nomProv%></option>
                        <%End If%>
                    <%Next%>                 
                </select><br />
                <strong><%= h.traducir("Matrícula 1")%></strong><br />
                <%= Html.TextBox("matricula1")%><br />
                <strong><%= h.traducir("Matrícula 2")%></strong><br />
                <%= Html.TextBox("matricula2")%><br />

                <%If ViewData("listOfAlbaran") IsNot Nothing Then%>
                    <%For Each a As web.Albaran In ViewData("listOfAlbaran")%>
                        <input type="hidden" name="albaran" value="<%= a.Id%>" />
                    <%Next%>
                <%End If%>
                <%If ViewData("listOfRecogida") IsNot Nothing Then%>
                <%For Each a As web.Recogida In ViewData("listOfRecogida")%>
                        <input type="hidden" name="recogida" value="<%= a.Id%>" />
                    <%Next%>
                    <%End If%>

                <input type="submit" value="<%= h.Traducir("Guardar viaje")%>" />
            </div>
            <div style="float:left;margin-left:2%;">
                <table id="matriculas" class="table1">
                    <thead>
                        <tr>
                            <th><%= h.traducir("Matricula 1")%></th>
                            <th><%= h.traducir("Matricula 2")%></th>
                            <th><%= h.traducir("PMA")%></th>
                        </tr>
                    </thead>
                    <tbody>
                        <%For Each m In ViewData("matriculas")%>
                            <tr>
                                <td><%= m.matricula1%></td> 
                                <td><%= m.matricula2%></td>
                                <td><%= m.pma%></td>
                            </tr>
                        <%Next%>        
                    </tbody>
                </table>
                
            </div>
        </fieldset>
    </form>
    2.-
    <form action="addtoexistingenvio" method="post">
        <fieldset>
            <legend><%= h.traducir("Añadir albaranes a viaje ya existente")%></legend>

           <%If ViewData("listOfAlbaran") IsNot Nothing Then%>
                    <%For Each a As web.Albaran In ViewData("listOfAlbaran")%>
                        <input type="hidden" name="albaran" value="<%= a.Id%>" />
                    <%Next%>
                <%End If%>
                <%If ViewData("listOfRecogida") IsNot Nothing Then%>
                <%For Each a As web.Recogida In ViewData("listOfRecogida")%>
                        <input type="hidden" name="recogida" value="<%= a.Id%>" />
                    <%Next%>
                    <%End If%>
            <%=Html.TextBox("idViaje")%>
            <input type="submit" value="<%= h.Traducir("Añadir albarán a viaje existente")%>" />
        </fieldset>
    </form>
    <% If ViewData("listOfAlbaran") IsNot Nothing Then%>
    <table class="table1">
        <thead>
            <tr>
            <th><%= h.traducir("Albaran Nº")%></th>
            <th><%= h.traducir("Bulto Nº")%></th>
            <th><%= h.traducir("Peso bulto")%></th>
            <th><%= h.traducir("OF")%></th>
            <th><%= h.traducir("OP")%></th>
            <th><%= h.traducir("Marca")%></th>
            <th><%= h.traducir("Fecha de entrega")%></th>
            <th><%= h.traducir("Cantidad")%></th>
            <th><%= h.traducir("Peso")%></th>
            <th><%= h.traducir("Creado")%></th>
            <th><%= h.traducir("Observación")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Albaran In ViewData("listOfAlbaran")%>
            <%Dim Albaran = True%>
            <%For Each g As web.Agrupacion In a.ListOfAgrupacion%>
            <%Dim first = True%>
            <%For Each m In g.ListOfMovimiento%>
                    <tr>
                    <%If Albaran Then%>
                        <td rowspan="<%=a.ListOfAgrupacion.sum(function(gr) gr.ListOfMovimiento.count())  %>">
                            <%= a.Id%>
                            <input type="hidden" name="albaran" value="<%= a.Id%>" />
                        </td>
                        <%albaran = False%>
                    <%End If%>
                    <% If first Then%>
                        <td rowspan="<%=g.ListOfMovimiento.count() %>"><%= g.Id%></td>
                        <td rowspan="<%=g.ListOfMovimiento.count() %>"><%= Decimal.Round(g.ListOfMovimiento.Sum(Function(o) o.Peso))%></td>
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
                </tr>
            <%Next%>
        <%Next%>
        <%Next%>
        </tbody>
    </table>
    <%End If%>
    <%If ViewData("listOfRecogida") IsNot Nothing Then%>
        <table class="table1">  
        <thead>
            <tr>
                <th><%= h.traducir("Fecha")%></th>
                <th><%= h.traducir("Recogida")%></th>
                <th><%= h.traducir("Entrega")%></th>
                <th><%= h.traducir("Creado por")%></th>
                <th><%= h.traducir("Of - OP - Peso")%></th>
                <th><%= h.traducir("Observacion")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each r As web.Recogida In ViewData("listOfRecogida")%>
                <tr>
                    <td><%=r.Fecha.Value.ToShortDateString%>
                        <input type="hidden" name="recogida" value="<%= r.Id%>" />
                    </td>
                    <td><%=r.nombreEmpresaRecogida%></td>
                    <td><%=r.nombreEmpresaEntrega%></td>
                    <td><%=r.nombreSab%></td>
                    <td>
                        <%For Each e In r.ListOfOp%>
                            <div>
                                <%=e.Numord%> - <%=e.Numope%> - <%=e.Peso%>Kg
                            </div>
                        <%next %>
                    </td>
                    <td><%=r.Observacion%></td>
                </tr>        
            <%Next%>    
        </tbody>
    </table>
    <%End If%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#matriculas tr").each(function () {
                $(this).find("td:first").click(function () {
                    $("#matricula1").val($(this).text());
                });
                $(this).find("td:eq(1)").click(function () {
                    $("#matricula2").val($(this).text());
                });
            });
        });
    </script>
</asp:Content>
