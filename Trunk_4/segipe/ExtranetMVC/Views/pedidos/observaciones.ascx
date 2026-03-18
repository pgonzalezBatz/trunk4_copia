<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="ExtranetMVC" %>
    <h3><%=h.Traducir("Observaciones")%> </h3>
    
    <%If ViewData("observaciones") IsNot Nothing Then%>
    <table class="table1">
        <thead>
            <tr>
                <th>Fecha</th>
                <th>Observacion</th>
            </tr>
        </thead>
    <tbody>
        <% For Each o In ViewData("observaciones")%>
            <tr>
                <% If o.idsab = SimpleRoleProvider.GetId() Then%>
                    <td><%=o.fecha%></td>   
                    <td><%= o.texto%></td>
                <%Else%>
                    <td><strong><%=o.fecha%></strong></td>   
                    <td><strong> <%= o.texto%></strong></td>
                <%End If%>
                
            </tr>
        <%Next%>
    </tbody>
    </table>
    <%End If%>
    <form action="<%=url.action("observaciones") %>" method="post">
        <%=Html.Hidden("pedido", Request("pedido"))%>
        <%=Html.Hidden("fromaction", Request.RawUrl)%>
        <%=Html.TextBox("texto")%>
        <input type="submit" value="<%=h.Traducir("Añadir")%>" />
    </form>
