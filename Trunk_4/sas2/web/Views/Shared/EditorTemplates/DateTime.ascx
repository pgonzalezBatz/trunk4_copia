<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl(Of nullable(Of DateTime))" %>

<%= Html.TextBox("", If(Model.HasValue, Model.Value.ToShortDateString, ""), New With {.class = "calendar form-control", .style = "z-index:2;position:relative;"})%>