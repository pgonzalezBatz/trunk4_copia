<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="PermisoDenegado.aspx.vb" Inherits="Nominas.PermisoDenegado" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <div class="alert alert-danger text-justify"><asp:Label id="lblMensaje" runat="server"></asp:Label></div>
    <div class="col-sm-4"><asp:Button runat="server" ID="btnVolver" Text="Volver al portal del empleado" CssClass="btn btn-primary col-xs-12" /></div>
</asp:Content>
