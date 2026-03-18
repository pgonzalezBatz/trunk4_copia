<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DocsIntegrViaje.aspx.vb" Inherits="WebRaiz.DocsIntegrViaje" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <script type="text/javascript" src="../Scripts/bootstrap-filestyle.min.js"></script>
    <asp:Panel runat="server" ID="pnlInfo">
        <asp:Label runat="server" ID="labelDocInfo1" Text="Como la actividad a realizar en el viaje es exenta, tiene que adjuntar toda la documentacion que acredite tal exencion(tarjetas de embarque, facturas de hotel, etc...)" CssClass="help-block"></asp:Label>
        <asp:Panel runat="server" ID="pnlInfoUploadDoc">
            <div class="row">
                <div class="col-sm-3"><asp:Label runat="server" ID="labelDocTitulo" Text="Titulo descriptivo del documento"></asp:Label></div>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtDocTitulo" MaxLength="50" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDocTit" runat="server" Display="None" ControlToValidate="txtDocTitulo" ValidationGroup="AddDoc" ErrorMessage="Introduzca el texto"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3"><asp:Label runat="server" ID="labelDocAdj" Text="Documento a adjuntar"></asp:Label></div>
                <div class="col-sm-9"><asp:FileUpload runat="server" ID="fuDocumento" CssClass="filestyle" /></div>                
            </div><br />  
            <div class="row">
                <div class="col-sm-3"><asp:Button runat="server" ID="btnSubirDoc" Text="Subir documento" CssClass="form-control btn btn-primary" ValidationGroup="AddDoc" /></div>
                <div class="col-sm-3"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
            </div>
        </asp:Panel>
        <ul id="mylist" class="list-group">
            <asp:Repeater runat="server" ID="rptDocumentos">
                <ItemTemplate>
                    <li class="list-group-item">
                        <asp:HyperLink ID="hkTitulo" runat="server" Target="_blank"></asp:HyperLink>
                        <asp:LinkButton runat="server" ID="lnkEliminar" CssClass="pull-right"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton>
                    </li>
                </ItemTemplate>                
            </asp:Repeater>
        </ul>					   
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlNoPermitido">
        <div class="alert alert-danger">
            <asp:Label runat="server" ID="lblNoPermitido" style="font-size:15px;"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnVolver2" Text="Volver" CssClass="form-control btn btn-primary" />
        </div>
    </asp:Panel>
    <div class="modal fade" id="confirmDeleteDoc">
        <div class="modal-dialog modal-confirm">
            <div class="modal-content">            
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel"><asp:Label runat="server" ID="labelConfirmDeleteTitle" Text="Confirmar borrado"></asp:Label></h4>
                    <asp:HiddenField runat="server" ID="hfIdDoc" />
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="labelConfirmDeleteMessage" Text="ConfirmarEliminar"></asp:Label>
                </div>
                <div class="modal-footer">                          
                    <asp:Button runat="server" ID="btnEliminarModal" Text="Eliminar" cssclass="btn btn-primary" /> 
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarDelete" Text="Cancelar"></asp:Label></button>                          
                </div>
            </div>
        </div>
    </div>
</asp:Content>
