<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Horas extra transporte")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
 <form action="" method="post">
     <fieldset>
         <legend><%=h.traducir("horas extra para fecha ")%> <%= New Date(Request("ticksFecha")).ToShortDateString%></legend>
         <label><%=h.traducir("Horas extra")%></label><br />
         <%=Html.TextBox("horasExtra")%><br />
         <input type="submit" value="<%=h.traducir("Guardar")%>" />
     </fieldset>
 </form>
</asp:Content>
