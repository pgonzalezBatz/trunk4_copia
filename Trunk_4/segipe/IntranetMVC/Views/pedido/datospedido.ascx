<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="IntranetMVC" %>
<div class="summarypedido">
    <%= h.traducir("Pedido Nº")%>  
     <strong> <%= ViewData("cabecera").pedido%> </strong> 
     | <%= h.traducir("Proveedor")%>  
    <strong> <%= ViewData("cabecera").nombreProveedor%></strong>
    <%If ViewData("cabecera").proyecto.length > 0 Then%>
        | <%= h.traducir("Proyecto")%>  
        <strong><%= ViewData("cabecera").proyecto%></strong>
    <%End If%>
    <%If ViewData("cabecera").cliente.length > 0 Then%>
        |  <%= h.traducir("Cliente")%>  
        <strong><%= ViewData("cabecera").cliente%></strong>
    <%End If%>
    | <%= h.traducir("Responsable")%>  
     <%= ViewData("cabecera").responsable%>
    <% If ViewData("cabecera").urgente = 1 Then%>
        <span class="mark1"><%= h.traducir("Este pedido esta marcado como urgente")%> </span>
    <% End If%>
</div>
