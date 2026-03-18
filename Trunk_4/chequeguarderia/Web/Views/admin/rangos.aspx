<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Cambiar rango")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h4><%= h.Traducir("Constantes de la aplicación")%></h4>

    <form action="" method="post">
        <fieldset>
            <legend><%= h.Traducir("Rango actual dentro del ejercicio")%> </legend>
            <strong><%= h.Traducir("Seleccione rango")%> </strong><br />
            <%=Html.DropDownList("rango")%><br />
            <input type="submit" value="<%= h.Traducir("Guardar cambios")%>" />
        </fieldset>
    </form>
    
</asp:Content>
