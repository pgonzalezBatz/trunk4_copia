<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of Web.Agrupacion)" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Editar agrupacion")%>
    </title>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <a href="<%=url.action("list") %>"><%= h.Traducir("Volver")%></a>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h3><%= h.Traducir("Bulto Nº ")%> <%= Html.DisplayFor(Function(m) m.Id)%></h3>

  <form action="" method="post">
      <%= Html.Hidden("idbulto", Model.Id)%>
        <%= Html.TextBox("peso")%>
        <input type="submit" value="<%= h.traducir("Cambiar peso")%>" />
    </form>
</asp:Content>
