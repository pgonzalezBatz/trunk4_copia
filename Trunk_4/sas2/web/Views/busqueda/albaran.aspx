<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Busqueda de viajes realizados")%>
    </title>
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=Html.ValidationSummary()%>
    <form method="post" action="">
        <fieldset>
            <legend><%= h.traducir("Busquedar albaran")%></legend>
           <strong> <%= h.traducir("Nº de albaran")%></strong>
            <%=Html.TextBox("id")%> 
            <br />
            <input type="submit" value=" <%= h.traducir("Buscar")%>" />
        </fieldset>
    </form>

    <%If Model IsNot Nothing %>
    <%=h.traducir("Encontrado albaran Nº") %>
    <%=Model.idAlbaran %>

    <% If Model.idviaje Is DBNull.Value%>
        El albaran no esta asignado a viaje
<%else %>
    Asignado al viaje 
    <%=Model.idviaje %>
    <%End If %>

    <%end if %>
</asp:Content>
