<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DepartamentosPersonas.aspx.vb" Inherits="WebRaiz.DepartamentosPersonas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <div class="form-group">
        <asp:Label runat="server" ID="labelInfo" Text="Exporta el listado de la estructura de departamentos-personas de Batz a un ficheros CSV"></asp:Label><br /><br />
        <asp:Button runat="server" ID="btnExportar" Text="Exportar" CssClass="form-control btn btn-primary" />
    </div>    
</asp:Content>