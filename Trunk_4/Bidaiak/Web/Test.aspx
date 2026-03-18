<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Test.aspx.vb" Inherits="WebRaiz.Test" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">        
    <a    <div class="panel-group" id="divAccordion">
        <div class="panel panel-primary">                    
            <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                <h4 class="panel-title">                            
                    <span class="glyphicon glyphicon glyphicon-filter"></span>
				    <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			    </h4>                              
            </div>
            <div class="panel-collapse collapse in" id="divCollapse">                           
                <div class="panel-body lines">
                    <div class="form-group">                        
                        <asp:TextBox runat="server" ID="txtIdViajeF" MaxLength="7" CssClass="form-control"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="ftbIdViajeF" runat="server" TargetControlID="txtIdViajeF" FilterType="Numbers" />                                      
                    </div>
                    <div class="form-group">                        
                        <uc:Busqueda ID="searchUserF" runat="server" PostBack="false" SoloActivos="true" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" />
                    </div> 
                    <div class="form-group">
                        <asp:Button runat="server" ID="btnSearchF" Text="Buscar" CssClass="form-control btn btn-primary" />
                    </div>
                </div>
            </div>            
        </div>
    </div><br />
        <ajax:TabContainer ID="tabPaneles" runat="server">
		<ajax:TabPanel ID="tabP1" runat="server">
			<ContentTemplate>
				c
			</ContentTemplate>
		</ajax:TabPanel>
		<ajax:TabPanel ID="tabP2" runat="server">
			<ContentTemplate>
				b
			</ContentTemplate>
		</ajax:TabPanel>
		<ajax:TabPanel ID="tabP3" runat="server">
			<ContentTemplate>
				a
			</ContentTemplate>
		</ajax:TabPanel>
        <ajax:TabPanel ID="tabP4" runat="server">
			<ContentTemplate>                
                <uc:Busqueda ID="searchUserClosed" runat="server" SoloActivos="true" PostBack="false" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" />
                e
			</ContentTemplate>
		</ajax:TabPanel>
	</ajax:TabContainer>
</asp:Content>
