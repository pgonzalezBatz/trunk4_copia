<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="TarifasServGen.aspx.vb" Inherits="WebRaiz.TarifasServGen" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">        
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
	        <asp:MultiView runat="server" ID="mvTarifas" ActiveViewIndex="0">        
		        <asp:View runat="server" ID="vListado">	                    
                    <asp:Label runat="server" ID="labelListadoServ" Text="Tarifas de servicios genericas utilizadas para los presupuestos de los viajes"></asp:Label><br /><br />                    
                    <div class="panel-group" id="divAccordion">
                        <div class="panel panel-primary">                    
                            <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                                <h4 class="panel-title">                            
                                    <span class="glyphicon glyphicon glyphicon-filter"></span>
				                    <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			                    </h4>                              
                            </div>
                            <div class="panel-collapse collapse" id="divCollapse">                           
                                    <div class="panel-body lines">
                                        <div class="input-group">
                                            <asp:Dropdownlist runat="server" id="ddlServicios" CssClass="form-control"></asp:Dropdownlist>
                                            <span class="input-group-btn">
                                                <button runat="server" id="btnSearch" class="btn btn-primary" type="button"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></button>
                                            </span>
                                        </div><br />
                                        <div class="row">
                                            <div class="col-sm-3"><asp:LinkButton runat="server" id="lnkNuevo" Text="Nuevo"></asp:LinkButton></div>
                                            <div class="col-sm-3"><asp:LinkButton runat="server" id="lnkReplicar" Text="Replicar tarifas" ToolTip="Cada cambio de año, se podran replicar todas las tarifas en el año seleccionado"></asp:LinkButton></div>
                                        </div>                                     
                                </div>
                            </div>
                        </div>
                    </div>                    
			        <asp:GridView runat="server" ID="gvTarifas" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="true" CssClass="table table-striped table-hover" PageSize="20" GridLines="None">				        
                        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                        <PagerSettings PageButtonCount="5" />
				        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				        <Columns>	
                            <asp:TemplateField HeaderText="Servicio">
                                <ItemTemplate><asp:Label runat="server" ID="lblServicio" /></ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Nivel">
                                <ItemTemplate><asp:Label runat="server" ID="lblNivel" /></ItemTemplate>
                            </asp:TemplateField>
				        </Columns>
			        </asp:GridView>
                    <div id="divModalReplicar" class="modal fade" data-keyboard="false">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalReplicar" Text="Replicacion de tarifas"></asp:Label></h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="labelInfo" Text="Seleccione el año en el que quiere replicar las tarifas. Se tomaran como origen las tarifas del ultimo año de cada tipo"></asp:Label>
                                    </div>  
                                    <div class="form-group">
                                        <asp:DropDownList runat="server" ID="ddlAnnoRep" CssClass="form-control"></asp:DropDownList>
                                    </div>                                    
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnReplicar" Text="Replicar" cssclass="btn btn-primary" /> 
                                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelar" Text="Cancelar"></asp:Label></button>                                     
                                </div> 
                            </div>
                        </div>
                    </div>
		        </asp:View>
		        <asp:View runat="server" ID="vDetalle">  
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <strong><asp:Label runat="server" ID="labelDivCabTarifa" Text="Datos tarifa"></asp:Label></strong>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelDServicio" Text="Servicio"></asp:Label></div>
                                <div class="col-sm-4"><asp:Dropdownlist runat="server" ID="ddlDServicio" CssClass="form-control"></asp:Dropdownlist></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelNivel" Text="Nivel"></asp:Label></div>
                                <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlNivel" CssClass="form-control"></asp:DropDownList></div>
                            </div>
                        </div>
                        <div class="panel-footer">                            
                            <div class="form-inline">                                
                                <asp:Button runat="server" ID="btnActivar" Text="Activar la tarifa" CssClass="form-control btn btn-primary" />                                
                                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Guardar" CssClass="form-control btn btn-primary" />
                                <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" CssClass="form-control btn btn-danger" />
                                <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" />                               
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-primary" runat="server" id="divTarifas">
                        <div class="panel-heading">
                            <strong><asp:Label runat="server" ID="labelDivCabtarifaAnyo" Text="Tarifas por año"></asp:Label></strong>
                        </div>
                        <div class="panel-body">
                            <asp:LinkButton runat="server" ID="lnkTarifaLinea" Text="Nueva linea"></asp:LinkButton><br /><br />
                             <asp:GridView runat="server" ID="gvTarifasAnno" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">				                
				                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				                <Columns>	
                                    <asp:BoundField DataField="Anno" HeaderText="Año" />
                                    <asp:BoundField DataField="Tarifa" HeaderText="Tarifa" />                                    
				                </Columns>
			                </asp:GridView>
                        </div>
                    </div>                    
                    <div id="divModal" class="modal fade" data-keyboard="false">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Tarifas"></asp:Label></h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-sm-2"><asp:Label runat="server" ID="labelLServicio" Text="Servicio"></asp:Label></div>
                                        <div class="col-sm-10"><b><asp:Label runat="server" ID="lblLServicio"></asp:Label></b></div>
                                    </div>  
                                     <div class="row">
                                        <div class="col-sm-2"><asp:Label runat="server" ID="labelLAnno" Text="Año"></asp:Label></div>
                                        <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlAnno" CssClass="form-control"></asp:DropDownList></div>
                                    </div>  
                                    <div class="row">
                                        <div class="col-sm-2"><asp:Label runat="server" ID="labelLTarifa" Text="Tarifa"></asp:Label></div>
                                        <div class="col-sm-7">
                                            <asp:Textbox runat="server" ID="txtTarifa" CssClass="form-control required" style="text-align:right"></asp:Textbox>                                            
                                            <ajax:FilteredTextBoxExtender ID="ftbTarifa" runat="server" TargetControlID="txtTarifa" FilterType="Numbers" />
                                            <asp:RequiredFieldValidator ID="rfvTarifa" runat="server" Display="None" ControlToValidate="txtTarifa" ValidationGroup="GuardarL" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>                                            
                                        </div>
                                        <div class="col-sm-3">
                                            €
                                        </div>
                                    </div>                                    
                                </div>
                                <div class="modal-footer">
                                    <div class="col-sm-6"><asp:Button runat="server" ID="btnGuardarLineaM" Text="Guardar" ValidationGroup="GuardarL" CssClass="form-control btn btn-primary" /></div>
                                    <div class="col-sm-6"><asp:Button runat="server" ID="btnEliminarLineaM" Text="Eliminar" CssClass="form-control btn btn-danger" /></div>
                                </div> 
                            </div>
                        </div>
                    </div>                    
                    <div class="modal fade" id="confirmDeleteTarifa">
                        <div class="modal-dialog modal-confirm">
                            <div class="modal-content">            
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title" id="myModalLabelTarifa"><asp:Label runat="server" ID="labelConfirmDeleteTitleTarifa" Text="Confirmar borrado"></asp:Label></h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label runat="server" ID="labelConfirmCancelTarifa" Text="confirmarEliminar"></asp:Label>
                                </div>
                                <div class="modal-footer">                          
                                    <asp:Button runat="server" ID="btnEliminarModalTarifa" Text="Eliminar" cssclass="btn btn-primary" /> 
                                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarTarifa" Text="Cancelar"></asp:Label></button>                          
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="confirmDeleteLinea">
                        <div class="modal-dialog modal-confirm">
                            <div class="modal-content">            
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title" id="myModalLabelLinea"><asp:Label runat="server" ID="labelConfirmDeleteTitleLinea" Text="Confirmar borrado"></asp:Label></h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label runat="server" ID="labelConfirmCancelLinea" Text="confirmarEliminar"></asp:Label>
                                </div>
                                <div class="modal-footer">                          
                                    <asp:Button runat="server" ID="btnEliminarModalLinea" Text="Eliminar" cssclass="btn btn-primary" /> 
                                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarLinea" Text="Cancelar"></asp:Label></button>                          
                                </div>
                            </div>
                        </div>
                    </div>
		        </asp:View>
	        </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>
