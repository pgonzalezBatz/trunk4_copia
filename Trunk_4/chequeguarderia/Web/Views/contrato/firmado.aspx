<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<asp:Content ID="Content1"  ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Contrato")%> 
    </title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <h3>
        <%= h.Traducir("El trabajador ya tiene el contrato firmado")%> 
    </h3><br />
    <a href="<%=url.action("index") %>"><%= h.Traducir("Volver")%> </a>
</asp:Content>
