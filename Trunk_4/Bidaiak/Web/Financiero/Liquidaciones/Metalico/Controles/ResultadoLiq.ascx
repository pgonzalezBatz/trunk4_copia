<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ResultadoLiq.ascx.vb" Inherits="WebRaiz.ResultadoLiq" %>
<asp:Panel runat="server" ID="pnlResul">
    <h3><asp:Label runat="server" ID="lblMensaje"></asp:Label></h3>
</asp:Panel>
<div class="form-group" runat="server" id="divRedirigirLiq">
    <asp:Button runat="server" ID="btnRedirigir" Text="Ir al resultado de la liquidacion" CssClass="form-control btn btn-primary" />
</div>