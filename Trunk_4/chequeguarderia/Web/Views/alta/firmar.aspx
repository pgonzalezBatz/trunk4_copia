<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<asp:Content ID="Content1"  ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Contrato")%> 
    </title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <h3>
        <%= h.Traducir("Es necesario firmar el contrato")%> 
    </h3><br />

    <%= h.Traducir("Puedes imprimir el contrato, firmarlo y entregarlo en portería")%> 
    <a href="<%=url.action("contratopdf","contrato") %>" target="_blank"><%= h.Traducir("Imprimir contrato")%> </a> <br />

    <%= h.Traducir("Tambien puedes pedir que te lo impriman en portería")%> 
</asp:Content>
