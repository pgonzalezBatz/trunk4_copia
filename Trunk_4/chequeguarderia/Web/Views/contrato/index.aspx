<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Firma de contratos")%> 
    </title>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    
    <form action="" method="post">
    
        <fieldset>
            <legend><%= h.Traducir("Usuario")%> </legend>
            <strong><%= h.Traducir("Código de trabajador")%> </strong><br />
            <%=Html.TextBox("idtrabajador")%><br />
            <input type="submit" value="<%= h.Traducir("Ir")%> " />

        </fieldset>
    </form>
</asp:Content>
