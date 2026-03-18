<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="TransferenciaAnticipo.aspx.vb" Inherits="WebRaiz.TransferenciaAnticipo" %>
<%@ Register Src="~/Controles/SeleccionViaje.ascx" TagName="SelViajes" TagPrefix="uc" %>
<%@ Register Src="~/Controles/Importes.ascx" TagName="Importes" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">	
    <div class="form-group"><asp:Label runat="server" ID="labelInfo"></asp:Label></div>    
     <asp:UpdatePanel runat="server" ID="upDatosTransf" UpdateMode="Conditional">
        <ContentTemplate> 
            <asp:Panel runat="server" ID="pnlFiltro">
                <div class="panel-group" id="divAccordion">
                    <div class="panel panel-primary">                    
                        <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#<%=divCollapse.ClientID %>" data-parent="#divAccordion">                                                
                            <h4 class="panel-title">                            
                                <span class="glyphicon glyphicon glyphicon-filter"></span>
				                <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			                </h4>                              
                        </div>
                        <div class="panel-collapse collapse in" id="divCollapse" runat="server">                           
                            <div class="panel-body lines" style="padding-bottom:0px;">
                                <div class="form-inline">
                                    <asp:DropDownList runat="server" ID="ddlAño" CssClass="form-control"></asp:DropDownList>
                                    <asp:DropDownList runat="server" ID="ddlMes" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtIdViajeF" MaxLength="7" CssClass="form-control"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbIdViaje" runat="server" TargetControlID="txtIdViajeF" FilterType="Numbers" />
                                </div>
                                <div class="form-group">                        
                                    <uc:Busqueda ID="searchUserF" runat="server" PostBack="false" SoloActivos="true" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" />
                                </div>                   
                                <div class="row">
                                    <div class="col-sm-3"><asp:Button runat="server" ID="btnSearchF" Text="Buscar" CssClass="form-control btn btn-primary" /></div>
                                    <div class="col-sm-3"><asp:Button runat="server" ID="btnResetF" Text="Resetear filtro" CssClass="form-control btn btn-default" /></div>
                                    <div class="col-sm-3"><asp:Button runat="server" ID="btnVolverF" Text="Volver" CssClass="form-control btn btn-default" /></div>
                                </div>
                            </div>                
                        </div>            
                    </div>
                </div>
             </asp:Panel>
            <div class="panel panel-primary" runat="server" id="divSelViajes">
                <div class="panel-heading">
                    <strong><asp:Label runat="server" ID="labelDivCab" Text="Datos de cabecera"></asp:Label></strong>
                </div>
                <div class="panel-body">
                    <div class="form-inline">
                        <asp:Label runat="server" id="labelSelViaje" Text="Seleccione el viaje en el que se encontraba"></asp:Label>&nbsp;
                        <b><asp:Label runat="server" id="lblPersonaTransf"></asp:Label></b>
                    </div>
                    <uc:SelViajes runat="server" ID="selViajes"></uc:SelViajes>
                </div>
            </div>                       
            <asp:Panel runat="server" ID="pnlSinAcabar" CssClass="alert alert-warning">
                <b><asp:Label runat="server" ID="lblSinAcabar"></asp:Label></b>
            </asp:Panel>            
            <asp:Panel runat="server" ID="pnlDatosTransf">  
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <strong><asp:Label runat="server" ID="labelDivViajeOrig" Text="Informacion del viaje origen"></asp:Label></strong>
                    </div>
                    <div class="panel-body lines">
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" id="labelViajeO" text="Viaje"></asp:Label></div>
                            <div class="col-sm-10"><asp:Label runat="server" id="lblViajeO" CssClass="label label-info" style="font-size:16px;"></asp:Label></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" id="labelFechasO" text="Fechas"></asp:Label></div>
                            <div class="col-sm-10"><b><asp:Label runat="server" id="lblFechasO"></asp:Label></b></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" id="labelLiqO" text="Liquidador"></asp:Label></div>
                            <div class="col-sm-10">
                                <b><asp:Label runat="server" id="lblLiqO"></asp:Label></b>
                                <asp:HiddenField runat="server" ID="hfUsuarioTransf" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <strong><asp:Label runat="server" ID="labelDivViajeDest" Text="Informacion del viaje destino"></asp:Label></strong>
                    </div>
                    <div class="panel-body lines">
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" id="labelViajeD" text="Viaje"></asp:Label></div>
                            <div class="col-sm-10"><asp:Label runat="server" id="lblViajeD" CssClass="label label-warning" style="font-size:16px;"></asp:Label></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" id="labelFechasD" text="Fechas"></asp:Label></div>
                            <div class="col-sm-10"><b><asp:Label runat="server" id="lblFechasD"></asp:Label></b></div>
                        </div>
                        <div class="form-inline">
                            <asp:Label runat="server" id="labelSelLiq" text="Persona a la que se le entrega el dinero"></asp:Label>
                            <asp:DropDownList runat="server" id="ddlLiqDest" CssClass="form-control"></asp:DropDownList> 
                        </div>
                        <div class="form-group alert alert-warning" runat="server" id="divUserOmitidos">
                            <asp:Label runat="server" ID="labelUserOmitidos" Text="Se han quitado usuarios que no pueden recibir una transferencia ya que no pueden tener un anticipo"></asp:Label>
                        </div>
                        <asp:Panel runat="server" ID="pnlLiquidadorDist" CssClass="form-group">
                            <asp:Label runat="server" id="labelLiqD" text="El liquidador del viaje es"></asp:Label>    
                            <b><asp:Label runat="server" id="lblLiqD"></asp:Label></b>
                        </asp:Panel> 
                        <div class="form-inline">
                            <asp:Label runat="server" id="labelFecha" text="Fecha en la que se realizo la transferencia"></asp:Label>                            
                            <div class="input-group date" id="dtFechaTransf" runat="server">
                                <asp:TextBox runat="server" ID="txtFechaTransf" style="background-color:#EEE" contenteditable="false" CssClass="form-control"></asp:TextBox>    
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>                            
                            <b><asp:Label runat="server" id="lblFechaTransf"></asp:Label></b>
                        </div>
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <strong><asp:Label runat="server" ID="labelDivImp" Text="Importes"></asp:Label></strong>
                    </div>
                    <div class="panel-body lines">
                         <div class="form-inline">
                             <asp:Label runat="server" ID="labelEurosPend" Text="Importe en euros pendiente de justificar"></asp:Label>
                             <b><asp:Label runat="server" ID="lblEurosPend"></asp:Label></b>
                         </div>
                        <uc:Importes runat="server" id="selImportes" Modificable="true"></uc:Importes>
                        <div class="form-group">
                            <asp:Label runat="server" id="labelObserv" Text="observaciones"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>	
                        </div>
                    </div>
                </div>
                <div class="row">		
                    <div class="col-sm-3"><asp:Button runat="server" ID="btnTransferir" Text="Transferir" CssClass="form-control btn btn-primary" /></div>
                    <div class="col-sm-3"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>                    
	            </div>
            </asp:Panel>  
            <div id="divModalTransf" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalTransf" Text="Transferencia"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <b><asp:Label runat="server" ID="labelConfirm" Text="Mensaje de confirmacion"></asp:Label></b>
                            </div>
                            <asp:Label runat="server" ID="lblTextoTransf"></asp:Label>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnTransferirM" Text="Transferir" CssClass="form-control btn btn-primary" /></div>                            
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnCancelM" text="Cancelar" CssClass="form-control btn btn-default" data-dismiss="modal" /></div>                                 
                        </div> 
                    </div>
                </div>
            </div> 
            <div id="divModalReimp" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalReimp" Text="Impresion de hojas de gastos"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelReimp" Text="La transferencia se ha completado pero es necesario imprimir las siguientes hojas de gastos ya que estaban validadas y no incluian el movimiento de transferencia efectuado"></asp:Label>
                            </div>
                            <div class="form-group">
                                <asp:Repeater runat="server" ID="rptHojas">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hlHG" Target="_blank" ToolTip="Imprimir"></asp:HyperLink><br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnVolverReimp" Text="Volver" CssClass="form-control btn btn-default" /></div>                            
                        </div> 
                    </div>
                </div>
            </div>                                                                                
        </ContentTemplate>
    </asp:UpdatePanel>   
    <uc:CargandoDatos runat="server" /> 
</asp:Content>
