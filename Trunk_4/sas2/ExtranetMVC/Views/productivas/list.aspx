<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="ExtranetMVC" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.traducir("OF productivas")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <table class="table">
        <thead>
            <tr>
                <th><%=h.traducir("Nº viaje")%></th>
                <th><%=h.traducir("Salida")%></th>
                <th><%=h.traducir("Origen Destino")%></th>
                <th><%=h.traducir("Precio")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each v In Model%>
            <tr>
                <td><%=v.id%></td>
                <td><%=v.salida.toshortdatestring%></td>
                <td>
                    <%If v.listofalbaran.count > 0 Then%>
                    <strong><%=h.traducir("Envio")%></strong>
                    <%For Each e In v.listofalbaran%>
                        <div class="alert alert-info">
                        <%=CType(e.ListOfAgrupacion, List(Of Object)).Sum(Function(p As Object) p.peso)%> Kg ==> <%=e.ListOfAgrupacion(0).ListOfMovimiento(0).nombreproveedor%><br />
                        <%For Each g In e.ListOfAgrupacion%>
                            <%For Each m In g.ListOfMovimiento%>
                                <%=m.numord%>:<%=m.numope%> - <%=m.marca%> <br />
                            <%Next%>
                            
                        <%Next%>
                        </div>
                    <%Next%>
                    <%End If%>
                    <%If v.ListOfRecogida.count > 0 Then%>
                    <strong><%=h.traducir("Recogida")%></strong>
                    <%For Each r In v.ListOfRecogida%>
                        <div  class="alert alert-success">
                            <%=r.nombreEmpresaRecogida%> ==> <%= r.nombreEmpresaEntrega%>
                        </div>
                    <%Next%>
                    <%End If%>
                </td>
                <td>
                    <form action="" method="post">
                        <input type="hidden" name="viaje" value="<%=v.id %>" />
                        <label>
                            <%=h.traducir("Importe")%><br />
                            <%=Html.TextBox("importe", Nothing, New With {.class = "form-control"}) %>
                        </label><br />
                        <label>
                            <%=h.traducir("Comentario")%><br />
                            <textarea name="comentario" class="form-control"></textarea>
                        </label>
                        <br />
                        <%If ConfigurationManager.AppSettings("taxistas").Split(",").Contains(SimpleRoleProvider.GetId()) Then%>
                        <label>
                            <%=h.traducir("Kilometros")%><br />
                            <%=Html.TextBox("kilometros", Nothing, New With {.class = "form-control"})%>
                        </label>
                        <br />
                        <label>
                            <%=h.traducir("Puntos de espera")%><br />
                            <%=Html.TextBox("puntoespera", Nothing, New With {.class = "form-control"})%>
                        </label>
                        <br />
                        <label>
                            <%=h.traducir("Tiempo de espera superior 1 hora")%><br />
                            <%=Html.TextBox("esperasuperior", Nothing, New With {.class = "form-control"})%>
                        </label>
                        <br />
                        <label>
                            <%=h.traducir("Suplemento domingos y festivos")%><br />
                            <%=Html.TextBox("suplemento", Nothing, New With {.class = "form-control"})%>
                        </label>
                        <br />
                        <%End If%>
                        
                        <input type="submit" value="<%=h.traducir("Guardar") %>" class="btn btn-primary" />
                    </form>
                </td>
            </tr>
            <%Next%>
        </tbody>
    </table>
</asp:Content>
