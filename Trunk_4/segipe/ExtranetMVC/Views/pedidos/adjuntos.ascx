<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>


<%@ Import Namespace="ExtranetMVC" %>
<div>
    <h3><%=h.traducir("Adjuntos")%> </h3>
    <%If ViewData("adjuntos") IsNot Nothing Then%>
        <% For Each o In ViewData("adjuntos")%>
                <div>
                    <a href="<%=url.action("adjunto",new with{.id=o.id,.nombre=o.nombre}) %>" target="_blank"> <%=o.nombre %></a>
                </div>
        <%Next%>
    <%End If%>
</div>


