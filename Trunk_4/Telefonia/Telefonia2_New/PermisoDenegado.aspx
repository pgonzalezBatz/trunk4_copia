<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.master" CodeBehind="PermisoDenegado.aspx.vb" Inherits="Telefonia.PermisoDenegado" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <div class="alert alert-danger"><asp:Label runat="server" id="labelMensaje" Text="Sin acceso al recurso"></asp:Label></div>
    <asp:Button runat="server" ID="btnVolver" Text="Volver a la intranet" CssClass="form-control btn btn-primary" />    
</asp:Content>
