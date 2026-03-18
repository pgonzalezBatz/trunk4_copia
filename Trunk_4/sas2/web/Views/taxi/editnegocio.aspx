<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Editar negocio")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form method="post" action="">
        <%= Html.Hidden("Id") %>
        <fieldset>
            <legend><%= h.Traducir("Editar viaje taxista")%></legend>
            <strong><%= h.Traducir("Negocio") %></strong><br />
            <%= Html.DropDownList("negocios")%><br />
            <input type="submit" value="<%= h.Traducir("Guardar")%>" />
        </fieldset>
    </form>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
</asp:Content>
