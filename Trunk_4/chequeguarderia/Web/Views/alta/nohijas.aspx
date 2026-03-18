<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
<title>
        <%= h.Traducir("Formulario alta")%>
</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h1>
    <%= h.Traducir("Partiendo de los datos que tenemos en RRHH, no tienes ningun hijo/a menor de 3 años")%>
    </h1>
</asp:Content>
