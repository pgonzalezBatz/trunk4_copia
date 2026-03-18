<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>Página de acceso</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h3>TODO: Acceso</h3>

    <a href="<%=Url.Action("list","albaran") %>">Albaranes</a>
</asp:Content>
