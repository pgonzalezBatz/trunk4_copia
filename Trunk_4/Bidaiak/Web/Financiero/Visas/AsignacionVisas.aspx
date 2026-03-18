<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="AsignacionVisas.aspx.vb" Inherits="WebRaiz.AsignacionVisas" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">	
	<asp:MultiView runat="server" ID="mvVisas" ActiveViewIndex="0">
		<asp:View runat="server" ID="vListado">	
            <div class="panel-group" id="divAccordion">
                <div class="panel panel-primary">                    
                    <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                        <h4 class="panel-title">                            
                            <span class="glyphicon glyphicon glyphicon-filter"></span>
				            <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			            </h4>                              
                     </div>
                    <div class="panel-collapse collapse in" id="divCollapse">                           
                         <div class="panel-body">
                             <uc:Busqueda ID="searchUserF" runat="server" PostBack="true" SoloActivos="true" RutaPaginaBusqueda="../../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="userVisas" />                             
                            <asp:LinkButton runat="server" id="lnkAsignacion" Text="Nueva asignacion"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>                         
			<asp:GridView runat="server" ID="gvVisas" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">
				<PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />
				<EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				<Columns>								
					<asp:BoundField HeaderText="Num Tarjeta" DataField="NumTarjeta" SortExpression="NumTarjeta" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center" />
					<asp:TemplateField SortExpression="Nombre">
						<HeaderTemplate><asp:Linkbutton runat="server" Text="Asignado a" CommandArgument="Nombre" CommandName="Sort"></asp:Linkbutton></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblAsignadoA"></asp:Label></ItemTemplate>
					</asp:TemplateField>														
				</Columns>
			</asp:GridView>
		</asp:View>
		<asp:View runat="server" ID="vDetalle">
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelNumTarj" Text="Num Tarjeta"></asp:Label></div>
                <div class="col-sm-10">
                    <asp:Panel runat="server" ID="pnlNuevo1">
						<asp:TextBox runat="server" ID="txtNumTarjeta" CssClass="form-control" ValidationGroup="Guardar"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="ftbNumTarjeta" runat="server" TargetControlID="txtNumTarjeta" FilterType="Numbers" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEdicion1">
                        <b><asp:Label runat="server" ID="lblNumTarjeta"></asp:Label></b>
                    </asp:Panel>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelAsiga" Text="Asignado a"></asp:Label></div>
                <div class="col-sm-10">
                    <asp:Panel runat="server" ID="pnlNuevo2">                                                                                      
                        <uc:Busqueda ID="searchUserNew" runat="server" PostBack="false" SoloActivos="true" RutaPaginaBusqueda="../../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" />            						                                                                       	                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEdicion2">
                        <b><asp:Label runat="server" ID="lblNombrePersona"></asp:Label></b>
                    </asp:Panel>
                </div>
            </div>
            <div class="row" runat="server" id="divObsoleto">
                <div class="col-sm-3"><asp:Checkbox ID="chbObsoleto" runat="server" Text="obsoleto" TextAlign="Right"></asp:Checkbox></div>                
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="form-control btn btn-primary" ValidationGroup="Guardar" /></div>
                <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
            </div>            
		</asp:View>
	</asp:MultiView>
    <uc:CargandoDatos runat="server" />    
</asp:Content>
