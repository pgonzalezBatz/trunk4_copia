<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="VerLiquidacionesFacturas.aspx.vb" Inherits="WebRaiz.VerLiquidacionesFacturas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <div class="form-group">
        <asp:Label runat="server" id="labelMensaje" Text="Se muestra las hojas de gastos de la gente que esta trabajando en otra empresa y de las cuales se ha solicitudo una factura"></asp:Label>
    </div>
    <div class="form-inline">
        <asp:Label runat="server" ID="labelSelLiq" Text="Seleccione la liquidacion"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlLiq" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
    </div>
    <asp:Panel runat="server" ID="pnlInfoLiq">
        <div class="row">
            <div class="col-sm-4"><asp:Label runat="server" ID="labelFTrans" Text="Fecha de la solicitud de factura"></asp:Label></div>
            <div class="col-sm-2"><b><asp:Label runat="server" ID="lblFTrans"></asp:Label></b></div>
            <div class="col-sm-2"><asp:Label runat="server" ID="labelEmpresa" Text="Empresa"></asp:Label></div>
            <div class="col-sm-4"><b><asp:Label runat="server" ID="lblEmpresa"></asp:Label></b></div>
        </div><br />
        <asp:GridView runat="server" ID="gvHojasLiq" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" ShowFooter="true">	        
	        <EmptyDataTemplate><asp:Label runat="server" Text="No existe ninguna hoja de gastos a liquidar"></asp:Label></EmptyDataTemplate>
	        <Columns>   
                <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblImportes"></asp:Label></ItemTemplate>
                </asp:TemplateField>                                         
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" ID="labelPersona" Text="Persona"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="labelTotal" text="Total"></asp:Label></FooterTemplate>    
		        </asp:TemplateField>                       
                <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="right">
                    <HeaderTemplate><asp:Label runat="server" ID="labelLiquidacion" Text="Liquidacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><b><asp:Label runat="server" ID="lblLiquidacion" style="font-size:13px"></asp:Label></b></ItemTemplate> 
                    <FooterTemplate><b><asp:Label runat="server" ID="lblTotal" style="font-size:15px"></asp:Label></b></FooterTemplate>
		        </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" text="Viajes/Hojas"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Hyperlink runat="server" ID="hlViajeHoja" Target="_blank" style="padding-left:15px;"></asp:Hyperlink></ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblIdUser"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" text="Fecha validacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblFVal"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" text="Cuenta contable"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblCuenta"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" Text="Organizacion"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblOrganizacion"></asp:Label></ItemTemplate>
                </asp:TemplateField>                        
                <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" Text="Lantegi"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblLantegi"></asp:Label></ItemTemplate>
                </asp:TemplateField>
	        </Columns>
        </asp:GridView><br />
        <div class="row">
            <div class="col-sm-4"><asp:Button runat="server" ID="btnDescargar" Text="Descargar fichero del banco" CssClass="form-control btn btn-primary" style="font-size:20px;" /></div>
            <div class="col-sm-4"><asp:Button runat="server" ID="btnAvisarEmail" Text="Mandar email aviso pago" CssClass="form-control btn btn-primary" style="font-size:20px;" /></div>
        </div>     
        <div class="modal fade" id="confirmEmail">
            <div class="modal-dialog modal-confirm">
                <div class="modal-content">            
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalLabel"><asp:Label runat="server" ID="labelConfirmTitle" Text="Confirmacion"></asp:Label></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label runat="server" ID="labelConfirmMessage" Text="Si continua se avisara por email a las personas de que se ha realizado el pago. ¿Desea continuar?"></asp:Label>
                    </div>
                    <div class="modal-footer">                          
                        <asp:Button runat="server" ID="btnContinuar" Text="Continuar" cssclass="btn btn-primary" /> 
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelar" Text="Cancelar"></asp:Label></button>                          
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
