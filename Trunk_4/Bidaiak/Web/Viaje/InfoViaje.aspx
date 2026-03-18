<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="InfoViaje.aspx.vb" Inherits="WebRaiz.InfoViaje" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <div class="alert alert-success">
        <b><asp:Label runat="server" id="lblMensajeResul"></asp:Label></b>
    </div>
    <div class="alert alert-warning" runat="server" id="divDocsCli">
        <asp:LinkButton runat="server" ID="lnkDocsCliente" Text="Se han relacionado documentos de cliente para alguno de los proyectos"></asp:LinkButton>
    </div>                                             
    <div style="background-image:url(../App_Themes/Tema1/Images/Ticket_template.png);background-repeat:no-repeat;background-size:contain;height:250px;width:100%;">
        <div class="box-ticket">
            <b><asp:Label ID="lblIdViajeGenerado" runat="server" style="font-size:28px;color:#a52a21"></asp:Label></b>
        </div>
    </div>                  
    <asp:Panel runat="server" ID="pnlNotificaciones" CssClass="form-group">                
        <b><asp:Label runat="server" ID="labelInfo" Text="Los cambios realizados se han notificado por email a"></asp:Label>:</b><br /><br />
        <ul runat="server" id="ulNotif" class="list-group"></ul>
    </asp:Panel>     
    <div class="row">        
        <div class="col-sm-3"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
        <div class="col-sm-6"><asp:Button runat="server" ID="btnAgregarDocCliente" Text="Agregar documentos de cliente" CssClass="form-control btn btn-primary" /></div>
    </div>
</asp:Content>
