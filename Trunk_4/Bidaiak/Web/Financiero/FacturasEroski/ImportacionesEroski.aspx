<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ImportacionesEroski.aspx.vb" Inherits="WebRaiz.ImportacionesEroski" %>
<%@ Register src="../ResumenImportaciones.ascx" tagname="ResumenImp" tagprefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:Panel runat="server" ID="pnlPendienteUsuario">
        <div class="alert alert-warning">
            <b><asp:Label runat="server" ID="lblPendiente"></asp:Label></b>
        </div>
        <div class="form-group">        
            <asp:Button runat="server" ID="btnContinuar" text="Continuar" cssClass="form-control btn btn-primary" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPendienteOtroUsuario">
        <div class="alert alert-warning">
            <b><asp:Label runat="server" ID="lblPendienteOtro" Text="Otro usuario esta en proceso de importacion del fichero de facturas Eroski. Hasta que no finalice no podra continuar"></asp:Label></b>
        </div>        
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlNoPendiente">        
        <uc:ResumenImp ID="resumen" runat="server" TipoImportacion="Eroski" /> 
    </asp:Panel>
</asp:Content>
