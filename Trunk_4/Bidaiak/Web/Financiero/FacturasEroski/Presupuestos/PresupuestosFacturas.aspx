<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="PresupuestosFacturas.aspx.vb" Inherits="WebRaiz.PresupuestosFacturas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <div class="panel-group" id="divAccordion">
        <div class="panel panel-primary">                    
            <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                <h4 class="panel-title">                            
                    <span class="glyphicon glyphicon glyphicon-filter"></span>
				    <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			    </h4>                              
            </div>
            <div class="panel-collapse collapse in" id="divCollapse">                           
                    <div class="panel-body lines">                        
                    <div class="form-group"><asp:Label runat="server" ID="labelSel" Text="Seleccione entre que fechas quiere comparar los presupuestos contra las facturas. A las facturas se han quitado los gastos de gestion"></asp:Label></div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelFIni" text="FechaInicio"></asp:Label></div>
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
                        <div class="row">
                            <div class="col-sm-4"><asp:CheckBox runat="server" ID="chbNoCoincidentes" Text="Mostrar solo no coincidentes" /></div>
                        </div>
                        <div class="form-group">
                        <asp:Button runat="server" ID="btnSearchF" Text="Buscar" CssClass="form-control btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>    
    <asp:panel runat="server" ID="pnlResultado">
        <div class="form-group">
            <asp:Label runat="server" ID="labelResul" Text="Registros encontrados"></asp:Label>
            <b><asp:Label runat="server" ID="lblReg"></asp:Label></b>
        </div>
        <asp:GridView runat="server" ID="gvPresupFact" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">	        
	        <EmptyDataTemplate><asp:Label runat="server" Text="Todas las facturas coinciden con los presupuestos"></asp:Label></EmptyDataTemplate>
	        <Columns>                     
               <asp:TemplateField HeaderText="Viaje">                
			        <ItemTemplate><asp:Hyperlink runat="server" ID="hlViaje" Target="_blank"></asp:Hyperlink></ItemTemplate>                
		        </asp:TemplateField>                                           
                <asp:TemplateField HeaderText="Presupuestado">                
			        <ItemTemplate><asp:Label runat="server" ID="lblPresupuestado" style="font-size:15px;"></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Facturado">                
			        <ItemTemplate><asp:Label runat="server" ID="lblFacturado" style="font-size:15px;"></asp:Label></ItemTemplate>
                </asp:TemplateField>                               
                <asp:TemplateField HeaderText="Falta por facturar">                
			        <ItemTemplate><asp:Label runat="server" ID="lblFaltaFacturar" style="font-size:15px;"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Detalle">                
			        <ItemTemplate>
                        <asp:Hyperlink runat="server" ID="hlView" CommandName="View" Target="_blank"><i class="glyphicon glyphicon-search text-info"></i></asp:Hyperlink>
			        </ItemTemplate> 
		        </asp:TemplateField>                                    
	        </Columns>
        </asp:GridView>
    </asp:panel>
</asp:Content>
