<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.Traducir("Cabeceras de pedido ")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent1" runat="server">
    <br />
    <table class="table1 cabecera">
        <thead>
            <tr>
                <th><%=h.Traducir("Nº Pedido")%> </th>
                <th><%=h.Traducir("Responsable")%> </th>
                <th><%=h.Traducir("Fecha Entrega")%> </th>
                <th><%=h.Traducir("Proyecto")%> </th>
                <th><%=h.Traducir("Of")%> </th>
                <th><%=h.Traducir("Cliente")%> </th>
                <th><%=h.Traducir("Observaciones")%> </th>
                <th><%=h.Traducir("adjuntos")%> </th>
                <th><%=h.Traducir("PDF")%> </th>
            </tr>
        </thead>
        <tbody>
            <%For Each p In Model%>
                <tr>
                    <td>
                        <a href="<%=url.action(ViewData("actiontolineas"),new with{.pedido=p.pedido}) %>">
                        <%=p.pedido %> 
                        <%If p.urgente = 1 Then%>
                            <span class="mark1"><%=h.Traducir("Urgente")%></span>
                        <%End If%>
                        </a>
                    </td>
                    <td><%=p.responsable%></td>
                    <td>
                        <%If p.fechaEntrega < Now Then%>
                            <span class="mark1">
                        <%Else%>
                            <span>
                        <%End If%>
                        <%=p.fechaEntrega.toshortdatestring()%>
                        </span>
                    </td>
                    <td><%=p.proyecto%></td>
                    <td><%=p.numord%></td>
                    <td><%=p.cliente %></td>
                    <td>
                        <%If p.comentario.length = 0 Then%>
                            <%=h.Traducir("No tiene comentarios")%>
                        <%ElseIf p.comentario.length < 40 And p.responsableComentario <> SimpleRoleProvider.GetId() Then%>
                            <span class="mark1"> <%=p.comentario%></span>
                        <%ElseIf p.responsableComentario <> SimpleRoleProvider.GetId() Then%>
                            <span class="mark1"><%=p.comentario.ToString.Substring(0, 40) %>...</span>
                        <%Else%>
                            ...
                        <%End If%>
                    </td>
                    <td>
                        <%If p.adjuntos > 0 Then%>
                            <%=h.Traducir("Tiene adjuntos")%>
                        <%End If%>
                    </td>
                    <td>
                        <a href="<%=url.action("pdf",new with{.pedido=p.pedido}) %>">Pdf</a>
                    </td>
                </tr>
            <%Next%>
        </tbody>
    </table>
</asp:Content>
