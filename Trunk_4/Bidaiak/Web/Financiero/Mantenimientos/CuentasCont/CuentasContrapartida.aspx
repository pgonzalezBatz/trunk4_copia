<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="CuentasContrapartida.aspx.vb" Inherits="WebRaiz.CuentasContrapartida" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <%--<script type="text/javascript" src="../../../js/jQuery/jquery.toastmessage.js"></script>--%>    
    <asp:Label runat="server" ID="labelInfo" Text="Cada planta tendra una cuenta de contrapartida que sera utilizada para realizar el asiento contable del total de gastos de visa"></asp:Label><br /><br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <asp:GridView runat="server" ID="gvCuentas" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None">		        
		        <EmptyDataTemplate><br /><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		        <Columns>
			        <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="Planta"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label ID="lblPlanta" runat="server"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
			        <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="Contrapartida"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label ID="lblContrapartida" runat="server"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="Cuota"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label ID="lblCuota" runat="server"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
		        </Columns>
	        </asp:GridView>
            <div id="divModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Cuenta planta"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></div>
                                <div class="col-sm-10"><b><asp:Label runat="server" ID="lblPlanta"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelContrapartida" Text="Contrapartida"></asp:Label></div>
                                <div class="col-sm-10">
                                    <asp:TextBox runat="server" ID="txtContrapartida" CssClass="form-control campoObligatorio"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbContrapartida" runat="server" TargetControlID="txtContrapartida" FilterType="Numbers" />
                                    <asp:RequiredFieldValidator ID="rfvContrapartida" runat="server" Display="None" ControlToValidate="txtContrapartida" ValidationGroup="Guardar" ErrorMessage="Introduzca la cuenta"></asp:RequiredFieldValidator>
	                                <ajax:ValidatorCalloutExtender runat="server" ID="vceContrapartida" TargetControlID="rfvContrapartida" />	
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelCuota" Text="Cuota"></asp:Label></div>
                                <div class="col-sm-10">
                                    <asp:TextBox runat="server" ID="txtCuota" CssClass="form-control campoObligatorio"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbCuota" runat="server" TargetControlID="txtCuota" FilterType="Numbers" />
                                    <asp:RequiredFieldValidator ID="rfvCuota" runat="server" Display="None" ControlToValidate="txtCuota" ValidationGroup="Guardar" ErrorMessage="Introduzca la cuenta"></asp:RequiredFieldValidator>
	                                <ajax:ValidatorCalloutExtender runat="server" ID="vceCuota" TargetControlID="rfvCuota" />	
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-12"><asp:Button runat="server" ID="btnSaveM" Text="Guardar" ValidationGroup="Question" CssClass="form-control btn btn-primary" /></div>                            
                        </div> 
                    </div>
                </div>
            </div>                   
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>
