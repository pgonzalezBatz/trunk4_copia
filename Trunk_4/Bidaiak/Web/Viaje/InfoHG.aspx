<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="InfoHG.aspx.vb" Inherits="WebRaiz.InfoHG" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:Panel runat="server" ID="pnlHGEnviada" CssClass="alert alert-info">
        <b><asp:Label runat="server" ID="lblInfoEnviada" style="font-size:16px;"></asp:Label></b>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlHGValidada" CssClass="alert alert-success">
        <b><asp:Label runat="server" ID="lblInfoValidada" style="font-size:16px;"></asp:Label></b>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlError" CssClass="alert alert-danger">
        <b><asp:Label runat="server" ID="labelError" text="Ha ocurrido un error al finalizar la accion. Compruebe el estado de la hoja de gastos y en caso de no haberse realizado la accion, genere un helpdesk para que se lo solucionen" style="font-size:16px;" CssClass="text-uppercase"></asp:Label></b>
    </asp:Panel>    
    <div class="form-group">
        <asp:Button runat="server" ID="btnImprimir" Text="Imprimir hoja de gastos" CssClass="form-control btn btn-primary" />     
        <asp:Button runat="server" ID="btnVolverHG" Text="Volver a la hoja de gastos" CssClass="form-control btn btn-primary" />
        <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-primary" />          
    </div>
</asp:Content>
