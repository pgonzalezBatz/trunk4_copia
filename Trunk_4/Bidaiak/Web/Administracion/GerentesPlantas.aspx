<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="GerentesPlantas.aspx.vb" Inherits="WebRaiz.GerentesPlantas" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:Label runat="server" ID="labelPlantas" Text="Las plantas filiales tendran asociado una gerente"></asp:Label><br /><br />    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>                   
            <asp:GridView runat="server" ID="gvGerentes" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None">		        
		        <EmptyDataTemplate><br /><asp:Label runat="server" Text="noExisteNingunRegistro" style="margin-left:15px;"></asp:Label></EmptyDataTemplate>
		        <Columns>
			        <asp:TemplateField>
				        <HeaderTemplate>
					        <asp:Label runat="server" Text="Planta"></asp:Label>
				        </HeaderTemplate>
				        <ItemTemplate>                    
					        <asp:Label ID="lblPlanta" runat="server"></asp:Label>
				        </ItemTemplate>
			        </asp:TemplateField>
			        <asp:TemplateField>
				        <HeaderTemplate>
					        <asp:Label runat="server" Text="Gerente"></asp:Label>
				        </HeaderTemplate>
				        <ItemTemplate>
					        <asp:Label ID="lblGerente" runat="server"></asp:Label>
				        </ItemTemplate>
			        </asp:TemplateField>                   
		        </Columns>
	        </asp:GridView>
            <div id="pageModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Gerente"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></div>
                                <div class="col-sm-10"><b><asp:Label runat="server" ID="lblPlanta"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelGerente" Text="Gerente"></asp:Label></div>
                                <div class="col-sm-10"><uc:Busqueda ID="searchUser" runat="server" PostBack="false" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" /><br />    	                                        					</div>
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
</asp:Content>
