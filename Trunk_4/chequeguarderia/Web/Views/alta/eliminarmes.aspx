<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
<title>
        <%= h.Traducir("Confimar eliminación")%>
</title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <%= h.traducir("Estas seguro de que quieres eliminar la solicitud para este mes?")%>

</asp:Content>
