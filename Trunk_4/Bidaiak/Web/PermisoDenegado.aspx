<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="PermisoDenegado.aspx.vb" Inherits="WebRaiz.PermisoDenegado" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <div class="alert alert-danger"><asp:Label runat="server" id="labelMensaje"></asp:Label></div>
    <asp:Button runat="server" ID="btnVolver" Text="Volver a la intranet" CssClass="form-control btn btn-primary" />
</asp:Content>