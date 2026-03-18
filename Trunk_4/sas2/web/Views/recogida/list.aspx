<%@ Page  Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage"   %>


<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Listado movimiento de recogidas")%>
    </title>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphMenu2" runat="server">
    <ul>
        <li><a href="<%=Url.Action("create") %>"><%= h.traducir("Crear nueva recogida")%></a></li>
    </ul>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.Content("~/help.html#recogidas") %>" target="_blank"><%= h.traducir("Ayuda")%></a>
    <% If ViewData("listOfRecogidas") Is Nothing OrElse ViewData("listOfRecogidas").count = 0 Then%>
        <h3>
            <%= h.traducir("No se han encontrado recogidas")%>
            <a href="<%=Url.Action("create") %>"><%= h.traducir("Crear nueva recogida")%></a>
        </h3>
    <%Else%>
    <h3><%= h.traducir("Solicitudes de recogidas")%></h3>
    <form action="<%=if(ViewData("idviaje") Is Nothing,url.action("create","viaje") ,  Url.action("viaje"))%>" method="post">
    <table class="table1">  
        <thead>
            <tr>
                <th></th>
                <th><%= h.traducir("Fecha")%></th>
                <th><%= h.traducir("Recogida")%></th>
                <th><%= h.traducir("Entrega")%></th>
                <th><%= h.traducir("Creado por")%></th>
                <th><%= h.traducir("Of - OP - Peso")%></th>
                <th><%= h.traducir("Observacion")%></th>
                <th><%= h.Traducir("Negocio")%></th>
                <th><%= h.traducir("Acciones")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each r As web.Recogida In ViewData("listOfRecogidas")%>
                <tr>
                    <td>
                        <input type="checkbox" name="recogida" value="<%=r.id %>" />
                    </td>
                    <td><%=r.Fecha.Value.ToShortDateString%></td>
                    <td><%=r.nombreEmpresaRecogida%>
                        <%=If(r.observacionesdireccion, "")%>    
                    </td>
                    <td><%=r.nombreEmpresaEntrega%></td>
                    <td><%=r.nombreSab%></td>
                    <td>
                        <%For Each e In r.ListOfOp%>
                            <div>
                                <%=e.Numord%> - <%=e.Numope%> - <%=e.Peso%>Kg  
<%--                                <%If IsNumeric(e.puerta) Then%>
                                <%=CType(ViewData("puertas"), IEnumerable(Of SelectListItem)).First(Function(p) p.Value = e.puerta).Text %>
                                <%End If %>--%>
                                
                            </div>
                        <%next %>
                    </td>
                    <td><%=r.Observacion%></td>
                    <td><%=r.Negocio%></td>
                    <td>
                        <a href="<%=Url.Action("divide", New With {.id = r.Id})%>"><%=h.traducir("Dividir recogida")%></a> |
                        <a href="<%=url.action("delete",new with{.id=r.id}) %>"><%= h.traducir("Eliminar")%></a> |
                        <a href="<%=url.action("edit",new with{.id=r.id}) %>"><%= h.traducir("Editar")%></a> |
                        <a href="<%=Url.Action("details", New With {.id = r.Id})%>"><%= h.traducir("Detalle")%></a>
                    </td>
                </tr>        
            <%Next%>    
        </tbody>
        <tfoot>
            <tr>
                <td colspan="9" align="left";>
                <%If ViewData("idviaje") IsNot Nothing Then%>
                    <%=Html.Hidden("idviaje")%>
                    <input type="submit" value="<%= h.Traducir("Añadir a viaje")%>" />                
                <%Else%>
                    <input type="submit" value="<%= h.Traducir("Crear nuevo viaje")%>" />                
                <%End If%>
                </td>
            </tr>
        </tfoot>
    </table>
    </form>
    <%End If%>
</asp:Content>
