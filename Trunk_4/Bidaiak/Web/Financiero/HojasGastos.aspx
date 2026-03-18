<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="HojasGastos.aspx.vb" Inherits="WebRaiz.HojasGastos" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server"> 
    <div class="form-group"><b><asp:Label runat="server" ID="labelInfo" Text="Se muestran todas las hojas de gastos validadas"></asp:Label></b></div>
     <div class="panel-group" id="divAccordion">
        <div class="panel panel-primary">                    
            <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                <h4 class="panel-title">                            
                    <span class="glyphicon glyphicon glyphicon-filter"></span>
				    <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			    </h4>                              
            </div>
            <asp:Panel runat="server" DefaultButton="btnSearchF">
                <div class="panel-collapse collapse in" id="divCollapse">                           
                    <div class="panel-body lines" style="padding-bottom:0px;">
                        <div class="row">
                            <div class="col-sm-2 form-inline">
                                <asp:CheckBox runat="server" ID="chbUsarFechas" ToolTip="Se toman en cuenta las fechas para el filtro" />
                                <asp:Label runat="server" ID="labelFIni" text="FechaInicio"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtFechaIni">
                                    <asp:TextBox runat="server" ID="txtFechaInicio" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelFFin" text="FechaFin" CssClass="custom-label-control"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtFechaFin">
                                    <asp:TextBox runat="server" ID="txtFechaFin" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtIdViajeHojaF" MaxLength="7" CssClass="form-control"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbIdViajeHoja" runat="server" TargetControlID="txtIdViajeHojaF" FilterType="Numbers" />
                        </div>
                        <div class="form-group">                        
                            <uc:Busqueda ID="searchUserF" runat="server" PostBack="false" SoloActivos="false" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" />
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
    <asp:GridView runat="server" ID="gvHojasGastos" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table tableWithBorder table-striped" GridLines="None" PageSize="20">
		<PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
        <PagerSettings PageButtonCount="5" />
		<EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		<Columns>	
            <asp:TemplateField>				
				<ItemTemplate><asp:Image runat="server" ID="imgHGSinEnviar"></asp:Image></ItemTemplate>
			</asp:TemplateField>
            <asp:TemplateField SortExpression="FechaEnvio">
				<HeaderTemplate><asp:LinkButton runat="server" Text="FechaEnvio" CommandName="Sort" CommandArgument="FechaEnvio"></asp:LinkButton></HeaderTemplate>
				<ItemTemplate><asp:Label runat="server" ID="lblFechaEnvio"></asp:Label></ItemTemplate>
			</asp:TemplateField>			
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				<HeaderTemplate><asp:Label runat="server" Text="Num Viaje/Hoja"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:Label runat="server" ID="lblIdViaje"></asp:Label></ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField SortExpression="Persona">
				<HeaderTemplate><asp:LinkButton runat="server" Text="persona" CommandName="Sort" CommandArgument="Persona"></asp:LinkButton></HeaderTemplate>
				<ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-HorizontalAlign="Center">
				<HeaderTemplate><asp:Label runat="server" Text="Anticipo"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:CheckBox runat="server" ID="chbAnticipo" Enabled="false" /></ItemTemplate>
			</asp:TemplateField> 
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
				<HeaderTemplate><asp:Label runat="server" Text="Ver hoja"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:LinkButton runat="server" ID="lnkVerHoja" CommandName="VH" ToolTip="Ver hojas de gastos"><i class="glyphicon glyphicon-search text-primary"></i></asp:LinkButton></ItemTemplate>
			</asp:TemplateField>           		
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
				<HeaderTemplate><asp:Label runat="server" Text="Ver anticipo"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:LinkButton runat="server" ID="lnkVerAnticipo" CommandName="VA" ToolTip="Anticipos"><i class="glyphicon glyphicon-search text-primary"></i></asp:LinkButton></ItemTemplate>
			</asp:TemplateField>
            <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" Text="Entregado"></asp:Label></HeaderTemplate>
                <ItemTemplate>
                    <asp:Panel runat="server" ID="pnlSinEntregar" CssClass="form-inline">                        
                        <asp:Panel runat="server" class="input-group date" id="pnlFechaEnt">
                            <asp:TextBox runat="server" ID="txtFechaEntrega" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                        </asp:Panel>                        
                        <asp:Button runat="server" ID="btnEntregar" Text="Entregar" CssClass="form-control btn btn-primary" OnClick="btnEntregar_Click" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEntregado" style="text-align:center;" CssClass="form-group">
                        <asp:Label runat="server" ID="lblFechaEntrega"></asp:Label>
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>           		
		</Columns>
	</asp:GridView>
</asp:Content>
