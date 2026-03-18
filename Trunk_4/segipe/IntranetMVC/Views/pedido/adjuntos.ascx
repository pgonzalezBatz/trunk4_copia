<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="IntranetMVC" %>
<div>
    <h3><%= h.traducir("Adjuntos")%> </h3>
    <%If ViewData("adjuntos") IsNot Nothing Then%>
        <% For Each o In ViewData("adjuntos")%>
                <div>
                    <a href="<%=url.action("adjunto",new with{.id=o.id,.nombre=o.nombre}) %>" target="_blank"> <%=o.nombre %></a>
                    <form action="<%=url.action("deleteAdjunto") %>" method="post" style="display:inline;">
                        <input type="hidden" name="id" value="<%=o.id %>" />
                        <%=Html.Hidden("fromaction", Request.RawUrl)%>
                        <input type="submit" value="<%=h.traducir("Eliminar") %>" />
                    </form>
                </div>
        <%Next%>
    <%End If%>
     <form action="<%=url.action("addadjunto") %>" method="post" enctype="multipart/form-data">
        <%=Html.Hidden("idpedido", Request("idpedido"))%>
        <%=Html.Hidden("fromaction", Request.RawUrl)%>
        <input type="file" name="adjunto" />
        <input type="submit" value="<%= h.traducir("Añadir")%>" />
    </form>
</div>


