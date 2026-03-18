<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="IntranetMVC" %>
    <div>
    <h3><%= h.traducir("Observaciones Externas")%> </h3>
    <%If ViewData("observacionesexternas") IsNot Nothing Then%>
    <table>
    <tbody>
        <% For Each o In ViewData("observacionesexternas")%>
            <tr>
                <% If o.idsab = SimpleRoleProvider.GetId() Then%>
                    <td><%=o.fecha%></td>  
                     <td><%=o.usuario%></td>  
                    <td><%= o.texto%></td>
                <%Else%>
                    <td><strong><%=o.fecha%></strong></td>   
                    <td><strong><%=o.usuario%></strong></td>  
                    <td><%= o.texto%></td>
                <%End If%>
                
            </tr>
        <%Next%>
    </tbody>
    </table>
    <%End If%>
    <form action="<%=url.action("observacionesexternas") %>" method="post">
        <%=Html.Hidden("idpedido", Request("idpedido"))%>
        <%=Html.Hidden("fromaction", Request.RawUrl)%>
        <%=Html.TextBox("texto")%>
        <input type="submit" value="<%= h.traducir("Añadir")%>" />
    </form>
    </div>
    <div>
    <h3><%= h.traducir("Observaciones Internas")%> </h3>
    <%If ViewData("observacionesinternas") IsNot Nothing Then%>
    <table>
    <tbody>
        <% For Each o In ViewData("observacionesinternas")%>
            <tr>
                <% If o.idsab = SimpleRoleProvider.GetId() Then%>
                    <td><%=o.fecha%></td>   
                    <td><%=o.usuario%></td>  
                    <td><%= o.texto%></td>
                <%Else%>
                    <td><strong><%=o.fecha%></strong></td>   
                    <td><strong><%=o.usuario%></strong></td>  
                    <td><%= o.texto%></td>
                <%End If%>
                
            </tr>
        <%Next%>
    </tbody>
    </table>
    <%End If%>
    <form action="<%=url.action("observacionesinternas") %>" method="post">
        <%=Html.Hidden("idpedido", Request("idpedido"))%>
        <%=Html.Hidden("fromaction", Request.RawUrl)%>
        <%=Html.TextBox("texto")%>
        <input type="submit" value="<%= h.traducir("Añadir")%>" />
    </form>
    </div>
    
