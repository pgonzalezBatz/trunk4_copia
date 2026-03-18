<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="Web" %>
<%@ Import Namespace="System.Linq" %>
<%If CType(ViewData("tipoguarderia"), Generic.List(Of Mvc.SelectListItem)).Exists(Function(e As Mvc.SelectListItem) e.Selected = True) Then%>
    <strong><%= h.Traducir("Cantidad mensual")%></strong><br />
    <%If CType(ViewData("tipoguarderia"), Generic.List(Of Mvc.SelectListItem)).Exists(Function(e As Mvc.SelectListItem) e.Value = "privada" AndAlso e.Selected = True) Then%>
            <%=Html.TextBox("importe")%><br />
            <%Else%>
            <%=Html.Hidden("importe", "160")%>160 €<br />
    <%End If %>
<%Else%>

<%End If%>