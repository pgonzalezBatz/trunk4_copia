<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="CuentasOtrasPlantas.aspx.vb" Inherits="WebRaiz.CuentasOtrasPlantas" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:Label runat="server" ID="labelPlantas" Text="Las plantas filiales, tendran asociado una cuenta contable para cada tipo de IVA"></asp:Label><br /><br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <asp:GridView runat="server" ID="gvCuentas" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None">		        
		        <EmptyDataTemplate><br /><asp:Label runat="server" Text="noExisteNingunRegistro" style="margin-left:15px;"></asp:Label></EmptyDataTemplate>
		        <Columns>
			        <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="Planta"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label ID="lblPlanta" runat="server"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
			        <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="Cuenta iva normal"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label ID="lblCuenta18" runat="server"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="Cuenta iva reducido"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label ID="lblCuenta8" runat="server"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="Cuenta exento iva"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label ID="lblCuenta0" runat="server"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
		        </Columns>
	        </asp:GridView>
            <div id="pageModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Cuentas contables"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-4"><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></div>
                                <div class="col-sm-8"><b><asp:Label runat="server" ID="lblPlanta"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4"><asp:Label runat="server" ID="labelCtaNormal" Text="Cuenta iva normal"></asp:Label></div>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCuenta18" CssClass="form-control campoObligatorio"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbCuenta18" runat="server" TargetControlID="txtCuenta18" FilterType="Numbers" />
                                    <asp:RequiredFieldValidator ID="rfvCuenta18" runat="server" Display="None" ControlToValidate="txtCuenta18" ValidationGroup="Guardar" ErrorMessage="Introduzca la cuenta"></asp:RequiredFieldValidator>
	                                <ajax:ValidatorCalloutExtender runat="server" ID="vceCuenta18" TargetControlID="rfvCuenta18" />	
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4"><asp:Label runat="server" ID="labelCtaReducida" Text="Cuenta iva reducido"></asp:Label></div>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCuenta8" CssClass="form-control campoObligatorio"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbCuenta8" runat="server" TargetControlID="txtCuenta8" FilterType="Numbers" />
                                    <asp:RequiredFieldValidator ID="rfvCuenta8" runat="server" Display="None" ControlToValidate="txtCuenta8" ValidationGroup="Guardar" ErrorMessage="Introduzca la cuenta"></asp:RequiredFieldValidator>
	                                <ajax:ValidatorCalloutExtender runat="server" ID="vceCuenta8" TargetControlID="rfvCuenta8" />		
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4"><asp:Label runat="server" ID="labelCtaExenta" Text="Cuenta exento iva"></asp:Label></div>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCuenta0" CssClass="form-control campoObligatorio"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbCuenta0" runat="server" TargetControlID="txtCuenta0" FilterType="Numbers" />
                                    <asp:RequiredFieldValidator ID="rfvCuenta0" runat="server" Display="None" ControlToValidate="txtCuenta0" ValidationGroup="Guardar" ErrorMessage="Introduzca la cuenta"></asp:RequiredFieldValidator>
	                                <ajax:ValidatorCalloutExtender runat="server" ID="vceCuenta0" TargetControlID="rfvCuenta0" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnSaveM" Text="Guardar" CssClass="form-control btn btn-primary" /></div>
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnCancelM" text="Cancelar" CssClass="form-control btn btn-default" data-dismiss="modal" /></div>                                 
                        </div> 
                    </div>
                </div>
            </div>                 
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>
