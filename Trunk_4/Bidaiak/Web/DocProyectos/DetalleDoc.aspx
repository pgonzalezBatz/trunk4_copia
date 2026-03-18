<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DetalleDoc.aspx.vb" Inherits="WebRaiz.DetalleDoc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">        
    <script type="text/javascript" src="../Scripts/bootstrap-filestyle.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.toastmessage.js"></script>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelCliente" Text="Cliente"></asp:Label></div>
        <div class="col-sm-4"><b><asp:Label runat="server" ID="lblCliente"></asp:Label></b></div>
        <div class="col-sm-2"><asp:Label runat="server" ID="labelProy" Text="Proyecto"></asp:Label></div>
        <div class="col-sm-4"><b><asp:Label runat="server" ID="lblProy"></asp:Label></b></div>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelDescrip" Text="Descripcion"></asp:Label></div>
        <div class="col-sm-10"><asp:TextBox runat="server" ID="txtDescrip" MaxLength="100" CssClass="form-control"></asp:TextBox></div>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelDoc" Text="Documento"></asp:Label></div>
        <div class="col-sm-10"><asp:FileUpload runat="server" ID="fuDocumento" CssClass="filestyle"/></div>
    </div>
    <div class="row" runat="server" id="divDoc">
        <div class="col-sm-10 col-sm-offset-2"><asp:Hyperlink runat="server" id="hkDocumento" Text="Ver documento" Target="_blank"></asp:Hyperlink></div>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Guardar" CssClass="form-control btn btn-primary" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnEliminar" Text="Eliminar" CssClass="form-control btn btn-danger" /></div>        
    </div>
    <div class="modal fade" id="confirmDelete">
        <div class="modal-dialog modal-confirm">
            <div class="modal-content">            
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel"><asp:Label runat="server" ID="labelConfirmDeleteTitle" Text="Confirmar borrado"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="labelConfirmCancel" Text="confirmarEliminar"></asp:Label>
                </div>
                <div class="modal-footer">                          
                    <asp:Button runat="server" ID="btnEliminarModal" Text="Eliminar" cssclass="btn btn-primary" /> 
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelar" Text="Cancelar"></asp:Label></button>                          
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
