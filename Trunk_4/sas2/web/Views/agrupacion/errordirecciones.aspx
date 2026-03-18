<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Error en la agrupación")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h2 style="color:Red;">Error el la agrupacion</h2>
    <%= h.Traducir("Las los movimientos tienen que pertener al mismo poveedor para poder agruparlos")%> <br />

    <a href="<%=url.Action("list","movimientosmaterial") %>"><%= h.Traducir("Volver al listado de movimientos")%></a>

</asp:Content>
