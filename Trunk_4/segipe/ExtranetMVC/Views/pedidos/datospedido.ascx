<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="ExtranetMVC" %>


<div class="summarypedido">
    <%=h.Traducir("Pedido Nº")%>  
     <strong> <%= ViewData("cabecera").pedido%> </strong> 
    | <%=h.Traducir("Responsable")%> 
    <strong> <%= ViewData("cabecera").responsable%></strong>
    <%If ViewData("cabecera").proyecto.length > 0 Then%>
        | <%=h.Traducir("Proyecto")%> 
        <b><%= ViewData("cabecera").proyecto%></b>
    <%End If%>
    <%If ViewData("cabecera").cliente.length > 0 Then%>
        |  <%=h.Traducir("Cliente")%> 
        <b><%= ViewData("cabecera").cliente%></b>
    <%End If%>
    <% If ViewData("cabecera").urgente = 1 Then%>
        <span class="mark1"><%=h.Traducir("Este pedido esta marcado como urgente")%> </span>
    <% End If%>
</div>
