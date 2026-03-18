<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Remesas.aspx.vb" Inherits="WebRaiz.Remesas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">	
    <asp:UpdatePanel runat="server">
        <ContentTemplate>            
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
                            <div class="form-group"><asp:Label runat="server" ID="labelSel" Text="Seleccione entre que fechas quiere conocer el dinero de anticipos necesario. No se muestran los anticipos ya entregados"></asp:Label></div>
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
                             <div class="form-group">
                                <asp:Button runat="server" ID="btnSearchF" Text="Buscar" CssClass="form-control btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>    
            <asp:Panel runat="server" ID="pnlSinResultados" CssClass="form-group">
                <b><asp:Label runat="server" ID="labelSinResul" text="No se necesita dinero para anticipos entre las fechas seleccionadas"></asp:Label></b>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDineroRetenido">
                 <div class="panel panel-warning">
                    <div class="panel-heading">
                        <strong><asp:Label runat="server" ID="labelDivCabecera" Text="Dinero retenido (el anticipo esta marcado como preparado)" CssClass="text-uppercase"></asp:Label></strong>
                    </div>
                    <div class="panel-body lines">
                        <div class="row">
                            <div class="col-sm-6">
                                <asp:GridView runat="server" ID="gvDineroRet" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None">
				                    <Columns>                        
					                    <asp:TemplateField>
						                    <HeaderTemplate><asp:Label text="Moneda" runat="server"></asp:Label></HeaderTemplate>
						                    <ItemTemplate><asp:Label id="lblMoneda" runat="server"></asp:Label></ItemTemplate>
					                    </asp:TemplateField>							
					                    <asp:TemplateField>
						                    <HeaderTemplate><asp:Label text="Cantidad" runat="server"></asp:Label></HeaderTemplate>
						                    <ItemTemplate><asp:Label id="lblCantidad" runat="server"></asp:Label></ItemTemplate>
					                    </asp:TemplateField>
				                    </Columns>
			                    </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>                                
            </asp:Panel>
	        <table class="table table-striped">
                <asp:Repeater runat="server" ID="rptRemesas">
			        <HeaderTemplate>
				        <tr>
                            <th runat="server" visible="false">&nbsp;</th>
					        <th>&nbsp;</th>
					        <th><asp:Label runat="server" Text="moneda"></asp:Label> </th>
					        <th style="text-align:right"><asp:Label runat="server" Text="Cantidad"></asp:Label></th>
                            <th style="text-align:right"><asp:Label runat="server" Text="Saldo en caja actual"></asp:Label></th>
                            <th style="text-align:right"><asp:Label runat="server" ID="labelSaldoRetenido" Text="Saldo en caja retenido"></asp:Label></th>
                            <th style="text-align:center"><asp:Label runat="server" Text="Fecha requerida"></asp:Label></th>					                    
                            <th><asp:Label runat="server" Text="Id Viaje"></asp:Label></th>
				        </tr>
			        </HeaderTemplate>
			        <ItemTemplate>
				        <tr runat="server" id="trServer">			
                            <td runat="server" visible="false"><asp:Label runat="server" ID="lblIdMoneda"></asp:Label></td>
					        <td style="text-align:center"><asp:ImageButton runat="server" ID="imgDetalle" ImageUrl="~/App_Themes/Tema1/images/signo_mas.jpg" CommandName="Detalle" /></td>
					        <td><asp:Label ID="lblMoneda" runat="server" CssClass="text-uppercase"></asp:Label></td>
                            <td style="text-align:right"><asp:Label ID="lblCantidad" runat="server"></asp:Label></td>
                            <td style="text-align:right"><asp:Label ID="lblSaldoCaja" runat="server"></asp:Label></td>
                            <td style="text-align:right"><asp:Label ID="lblSaldoCajaRetenido" runat="server"></asp:Label></td>
                            <td style="text-align:center"><asp:Label ID="lblFechaReq" runat="server"></asp:Label></td>
                            <td><asp:Hyperlink runat="server" id="hlIdViaje" Target="_blank" ToolTip="Ver informacion"></asp:Hyperlink></td>				                    
				        </tr>
				        <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
					        <tr>
						        <td colspan="5">
                                    <table class="table table-striped">
                                        <asp:Repeater runat="server" ID="rptRemesasDet" OnItemDataBound="rptRemesasDet_ItemDataBound">			                       
			                                <ItemTemplate>
				                                <tr runat="server" id="trServer">	
                                                    <td>&nbsp;</td>
					                                <td><asp:Label ID="lblMonedaDet" runat="server" CssClass="text-uppercase"></asp:Label></td>
                                                    <td style="text-align:right"><asp:Label ID="lblCantidadDet" runat="server"></asp:Label></td>
                                                    <td style="text-align:center"><asp:Label ID="lblFechaReqDet" runat="server"></asp:Label></td>					
                                                    <td><asp:Hyperlink runat="server" id="hlIdViajeDet" Target="_blank" ToolTip="Ver informacion"></asp:Hyperlink></td>					                                            
				                                </tr>
                                            </ItemTemplate>
                                      </asp:Repeater>
                                   </table> 
                                </td>						
					        </tr>
				        </asp:Panel>
			        </ItemTemplate>
                    <AlternatingItemTemplate>
				        <tr runat="server" id="trServer">
                            <td runat="server" visible="false"><asp:Label runat="server" ID="lblIdMoneda"></asp:Label></td>
					        <td style="text-align:center"><asp:ImageButton runat="server" ID="imgDetalle" ImageUrl="~/App_Themes/Tema1/images/signo_mas.jpg" CommandName="Detalle" /></td>
					        <td><asp:Label ID="lblMoneda" runat="server" CssClass="text-uppercase"></asp:Label></td>
                            <td style="text-align:right"><asp:Label ID="lblCantidad" runat="server"></asp:Label></td>
                            <td style="text-align:right"><asp:Label ID="lblSaldoCaja" runat="server"></asp:Label></td>
                            <td style="text-align:right"><asp:Label ID="lblSaldoCajaRetenido" runat="server"></asp:Label></td>
                            <td style="text-align:center"><asp:Label ID="lblFechaReq" runat="server"></asp:Label></td>
                            <td><asp:Hyperlink runat="server" id="hlIdViaje" Target="_blank" ToolTip="Ver informacion"></asp:Hyperlink></td>								
				        </tr>
				        <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
					        <tr style="background: #eff4f7;">
						        <td colspan="5">
                                     <table class="table table-striped">
                                        <asp:Repeater runat="server" ID="rptRemesasDet" OnItemDataBound="rptRemesasDet_ItemDataBound">			                       
			                                <ItemTemplate>
				                                <tr runat="server" id="trServer">	
                                                    <td>&nbsp;</td>
					                                <td><asp:Label ID="lblMonedaDet" runat="server" CssClass="text-uppercase"></asp:Label></td>
                                                    <td style="text-align:right"><asp:Label ID="lblCantidadDet" runat="server"></asp:Label></td>
                                                    <td style="text-align:center"><asp:Label ID="lblFechaReqDet" runat="server"></asp:Label></td>					
                                                    <td><asp:Hyperlink runat="server" id="hlIdViajeDet" Target="_blank" ToolTip="Ver informacion"></asp:Hyperlink></td>					                                            
				                                </tr>
                                            </ItemTemplate>
                                      </asp:Repeater>
                                   </table>
                                </td>						
					        </tr>
				        </asp:Panel>
			        </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>
