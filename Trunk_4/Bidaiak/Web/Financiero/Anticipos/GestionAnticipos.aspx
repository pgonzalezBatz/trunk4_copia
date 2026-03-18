<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="GestionAnticipos.aspx.vb" Inherits="WebRaiz.GestionAnticipos" EnableEventValidation="false" %>
<%@ Register src="Controles/GridViewAnticipo.ascx" tagname="GridViewAnticipo" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
     <div class="panel-group" id="divAccordion">
        <div class="panel panel-primary">                    
            <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                <h4 class="panel-title">                            
                    <span class="glyphicon glyphicon glyphicon-filter"></span>
				    <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			    </h4>                              
            </div>
            <asp:Panel runat="server" DefaultButton="btnSearchF">
                <div class="panel-collapse collapse" id="divCollapse">                           
                    <div class="panel-body lines" style="padding-bottom:0px;">
                        <div class="form-group">                        
                            <asp:TextBox runat="server" ID="txtIdViajeF" MaxLength="7" CssClass="form-control"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbIdViajeF" runat="server" TargetControlID="txtIdViajeF" FilterType="Numbers" />
                        </div>
                        <div class="form-group">                        
                            <uc:Busqueda ID="searchUserF" runat="server" PostBack="false" SoloActivos="false" RutaPaginaBusqueda="../../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" />
                        </div>
                        <div class="row">
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnSearchF" Text="Buscar" CssClass="form-control btn btn-primary" /></div>
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnResetF" Text="Resetear filtro" CssClass="form-control btn btn-default" /></div>
                        </div>
                    </div>                
                </div>
            </asp:Panel>
        </div>
    </div>     
    <asp:Panel runat="server" ID="pnlAnticiposCancelados" CssClass="alert alert-danger">
        <asp:Label runat="server" id="labelAnticCancel" Text="Anticipos entregados de viajes que han sido cancelados por el usuario pero que no han sido devueltos"></asp:Label>        
            <asp:Repeater runat="server" ID="rptAnticCancel">
                <ItemTemplate>
				    <div class="form-group">
                        <asp:LinkButton runat="server" ID="lnkAnticCancel" OnClick="LinkCancelacion"></asp:LinkButton>
				    </div>                    
                </ItemTemplate>
            </asp:Repeater>        
    </asp:Panel>
	<ajax:TabContainer ID="tabPaneles" runat="server">
		<ajax:TabPanel ID="tabP1" runat="server">
			<ContentTemplate>
				<uc1:GridViewAnticipo ID="gvaSolicitados" runat="server" />
			</ContentTemplate>
		</ajax:TabPanel>                
		<ajax:TabPanel ID="tabP2" runat="server">
			<ContentTemplate>
				<uc1:GridViewAnticipo ID="gvaPreparados" runat="server" />
			</ContentTemplate>
		</ajax:TabPanel>
		<ajax:TabPanel ID="tabP3" runat="server">
			<ContentTemplate>
				<uc1:GridViewAnticipo ID="gvaEntregados" runat="server" />
			</ContentTemplate>
		</ajax:TabPanel>
		<ajax:TabPanel ID="tabP4" runat="server">
			<ContentTemplate>  
                <b><asp:Label runat="server" ID="labelResulCerradoSinFilter" Text="Para mostrar resultados, tienes que buscar con un filtro"></asp:Label></b>
				<uc1:GridViewAnticipo ID="gvaCerrados" runat="server" />
			</ContentTemplate>
		</ajax:TabPanel>
	</ajax:TabContainer>	
</asp:Content>
