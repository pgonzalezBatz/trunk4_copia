<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DetalleAnticipo.aspx.vb" Inherits="WebRaiz.DetalleAnticipo" %>
<%@ Register Src="~/Controles/Importes.ascx" TagName="Importes" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivCab" Text="Datos de cabecera"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelIdViaje" text="Num Viaje"></asp:Label></div>
                <div class="col-sm-4"><asp:Label ID="lblIdViaje" runat="server" CssClass="label label-info" style="font-size:18px"></asp:Label></div>
                <div class="col-sm-2"><asp:LinkButton runat="server" ID="lnkVerViaje" Text="Ver viaje"></asp:LinkButton></div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelSolicitante" text="Solicitante"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblSolicitante"></asp:Label></b></div>
                <div class="col-sm-2"><asp:Label runat="server" ID="labelLiquidador" text="Liquidador"></asp:Label></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblLiquidador"></asp:Label></b>
                    <asp:HiddenField runat="server" ID="hfEmailLiquidador" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label ID="labelCtaContable" runat="server" text="Cuenta contable"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblCtaContable" runat="server" CssClass="labelDetalle"></asp:Label></b></div>
                <div class="col-sm-2"><asp:Label runat="server" ID="labelLantegi" Text="Lantegi"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblLantegi"></asp:Label></b></div>
            </div>
            <asp:Panel runat="server" ID="pnlOrganizacion" cssClass="row">
                <div class="col-sm-2"><asp:Label ID="labelOrganiz" runat="server" text="Organizacion"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblOrganiz" runat="server"></asp:Label></b></div>                                
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlAnticSinDev">
                <div class="alert alert-danger">
                    <asp:Repeater runat="server" ID="rptAnticSinDev">
                        <ItemTemplate>                            
                            <b><asp:Label runat="server" ID="lblAnticSinDev" CssClass="text-danger"></asp:Label></b>
                            <asp:Hyperlink runat="server" ID="hlVerAntic" ToolTip="Ver anticipo" style="margin-left:20px;"></asp:Hyperlink><br />                            
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivEstados" Text="Estados"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:LinkButton runat="server" ID="lnkHistoricoEstados" Text="Ver historico"></asp:LinkButton><br /><br />
                     <table>
                        <tr>
                            <td style="text-align:center"><h6><b><asp:Label runat="server" ID="lblEtSolicitado" Text="solicitado"></asp:Label></b></h6></td>
                            <td>&nbsp;</td>
                            <td style="text-align:center"><h6><b><asp:Label runat="server" ID="lblEtPreparado" Text="Preparado"></asp:Label></b></h6></td>
                            <td>&nbsp;</td>
                            <td style="text-align:center"><h6><b><asp:Label runat="server" ID="lblEtEntregado" Text="Entregado"></asp:Label></b></h6></td>
                            <td>&nbsp;</td>
                            <td style="text-align:center"><h6><b><asp:Label runat="server" Id="lblEtCerrado" Text="Cerrado"></asp:Label></b></h6></td>
                        </tr>
                        <tr>
                            <td style="text-align:center"><asp:ImageButton runat="server" ID="imgSolicitar" ToolTip="Solicitar" /></td>
                            <td style="vertical-align:middle"><img alt="" src="../../App_Themes/Tema1/Images/Estados/flecha.gif" /></td>                            
                            <td style="text-align:center"><asp:ImageButton runat="server" ID="imgPreparar" ToolTip="Preparar" /></td>
                            <td style="vertical-align:middle"><img alt="" src="../../App_Themes/Tema1/Images/Estados/flecha.gif" /></td>
                            <td style="text-align:center"><asp:ImageButton runat="server" ID="imgEntregar" ToolTip="Entregar" /></td>
                            <td style="vertical-align:middle"><img alt="" src="../../App_Themes/Tema1/Images/Estados/flecha.gif" /></td>
                            <td style="text-align:center"><asp:ImageButton runat="server" ID="imgCerrar" ToolTip="cerrar" /></td>
                            <td>                                
                                <asp:Label id="labelCancelado" runat="server" Text="Cancelado" CssClass="text-danger text-uppercase" style="font-size:15px;margin-left:15px;"></asp:Label>
                                <asp:Button runat="server" ID="btnAnular" Text="Anular Anticipo" ToolTip="Mientras este en estado solicitado, tramitado o preparado se podra anular el anticipo" CssClass="form-control btn btn-danger" style="margin-left:40px;" />                                
                            </td>
                        </tr>
                    </table>                                                                                                                
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:HiddenField runat="server" ID="hfEstadoActual" />
        </div>
    </div> 
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivAnt" Text="Anticipos solicitados"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:Panel runat="server" ID="pnlAnticSolicitado">
                <div class="form-inline">
                    <asp:Label runat="server" ID="labelFechaReq" text="Fecha en la que requiere el anticipo"></asp:Label>
                    <b><asp:Label runat="server" id="lblFechaRequiere"></asp:Label></b>
                </div><br />
                <uc:Importes runat="server" id="selImportes" Modificable="false" GridviewWidthPercentage="60"></uc:Importes>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelEurSol" Text="Euros solicitados"></asp:Label></div>
                    <div class="col-sm-2"><b><asp:Label runat="server" ID="lblEurosSolicitados" style="font-size:15px;"></asp:Label></b></div>
                </div>
                <asp:Panel runat="server" ID="pnlEurosEntregados" CssClass="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelEurosEntreg" Text="Euros entregados"></asp:Label></div>
                    <div class="col-sm-2"><b><asp:Label runat="server" ID="lblEurosEntregados" style="font-size:15px;"></asp:Label></b></div>                     
                </asp:Panel> 
                <asp:Panel runat="server" ID="pnlNoEntregadoCancelacion" CssClass="alert alert-danger">
                    <asp:Label runat="server" ID="labelNoEntregadoCancelacion" Text="El anticipo no se ha entregado porque ha sido cancelado"></asp:Label>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlAnticPorTransferencia">
                <div class="form-group">
                    <asp:Label runat="server" ID="labelInfoTransf" text="El anticipo se ha creado por una transferencia proveniente de"></asp:Label>
                </div>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelViajeOrigTransf" Text="Viaje"></asp:Label></div>
                    <div class="col-sm-4"><asp:Label ID="lblViajeOrigTransf" runat="server" CssClass="label label-info" style="font-size:18px"></asp:Label></div>
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelPersonaTransf" Text="Persona"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblPersonaTransf"></asp:Label></b></div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivMov" Text="Resumen de movimientos"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:GridView runat="server" ID="gvResumen" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table tableWithBorder table-striped table-hover" GridLines="None" ShowFooter="true">				
				<EmptyDataTemplate><asp:Label runat="server" Text="No se ha realizado ningun movimiento"></asp:Label></EmptyDataTemplate>
				<Columns>
					<asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><asp:Linkbutton runat="server" id="lnkVer" CommandName="View" CssClass="form-control btn btn-info" OnClick="VerDetalleMovimiento" ToolTip="Ver"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></asp:Linkbutton></ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
						<HeaderTemplate><asp:Label runat="server" Text="fecha"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblFechaMov"></asp:Label></ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" ItemStyle-HorizontalAlign="Right">
						<HeaderTemplate><asp:Label runat="server" Text="Justificado"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblHaberMov"></asp:Label></ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" ItemStyle-HorizontalAlign="Right">
						<HeaderTemplate><asp:Label runat="server" Text="Gasto"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblDebeMov"></asp:Label></ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField FooterStyle-HorizontalAlign="Center">
						<HeaderTemplate><asp:Label runat="server" Text="moneda"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblMonedaMov" CssClass="text-uppercase"></asp:Label></ItemTemplate>
                        <FooterTemplate><b><asp:Label runat="server" ID="lblEtiquetaTotal" Style="text-align: right;" Text="Etiqueta"></asp:Label></b></FooterTemplate>
					</asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
						<HeaderTemplate><asp:Label runat="server" Text="Conversion en euros"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblConversionEuros"></asp:Label></ItemTemplate>
                        <FooterTemplate><b><asp:Label runat="server" ID="lblTotal" style="font-size:15px" Text="Total"></asp:Label></b></FooterTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
						<HeaderTemplate><asp:Label runat="server" Text="Usuario"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblUsuario"></asp:Label></ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<HeaderTemplate><asp:Label runat="server" Text="Modo"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblModo"></asp:Label></ItemTemplate>
					</asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
						<HeaderTemplate><asp:Label runat="server" Text="Cambio"></asp:Label></HeaderTemplate>
						<ItemTemplate><asp:Label runat="server" ID="lblCambio"></asp:Label></ItemTemplate>
					</asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
						<ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CommandName="Elim" CssClass="form-control btn btn-danger" ToolTip="Eliminar"><span aria-hidden="true" class="glyphicon glyphicon-remove-sign"></span></asp:Linkbutton></ItemTemplate>
					</asp:TemplateField>                    
				</Columns>                
			</asp:GridView>
            <asp:Panel runat="server" ID="pnlAvisosHojasGastos" CssClass="alert alert-warning">                
                <b><asp:Label runat="server" ID="lblAviso"></asp:Label></b>
             </asp:Panel>
            <asp:Panel runat="server" ID="pnlAvisoHojaLiquidada" CssClass="alert alert-info">                
                <b><asp:Label runat="server" ID="lblAvisoHGLiquidada"></asp:Label></b>                    
            </asp:Panel>
        </div>
    </div><br />
    <div class="row">
        <div class="col-sm-2"><asp:Button runat="server" ID="btnDevolucion" Text="Devolver en metalico" CssClass="form-control btn btn-primary" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnTransferencia" Text="Transferir a otro viaje" CssClass="form-control btn btn-warning" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnDifCambio" Text="Diferencia de cambio" CssClass="form-control btn btn-primary" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnMovManual" Text="Añadir movimiento" CssClass="form-control btn btn-primary" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnImprimir" Text="Imprimir" CssClass="form-control btn btn-info" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
    </div> 
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <div id="divHistorico" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleMHistorico" Text="Historico de estados"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:GridView runat="server" ID="gvEstados" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">				            
				                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				                <Columns>				
					                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
						                <HeaderTemplate><asp:Label runat="server" Text="fecha"></asp:Label></HeaderTemplate>
						                <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
					                </asp:TemplateField>								
					                <asp:TemplateField>
						                <HeaderTemplate><asp:Label runat="server" Text="estado"></asp:Label></HeaderTemplate>
						                <ItemTemplate><asp:Label runat="server" ID="lblEstado"></asp:Label></ItemTemplate>
					                </asp:TemplateField>									
				                </Columns>
			                </asp:GridView> 
                        </div>               
                    </div>
                </div>
            </div>        
            <div id="divEntrega" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleMEntrega" Text="Entrega de anticipo"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelAntEntr" Text="El anticipo se le entrega al liquidador"></asp:Label>
                                <b><asp:Label runat="server" ID="lblUserEntrega"></asp:Label></b>
                            </div>
                            <asp:CheckBox runat="server" ID="chbImprimir" text="Imprimir recibo" Checked="true" />
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-12"><asp:Button ID="btnCambioEstadoUser" runat="server" text="Aceptar" CssClass="form-control btn btn-primary" /></div>
                        </div>
                    </div>
                </div>
            </div>           
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divMovManual" class="modal fade" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalManual" Text="Movimiento manual"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelManualAccion" Text="Accion"></asp:Label></div>
                        <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlManualAccion" CssClass="form-control"></asp:DropDownList></div>
                        <div class="col-sm-3">                            
                            <asp:TextBox runat="server" ID="txtManualCantidad" MaxLength="8" style="text-align:center" CssClass="form-control required"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbManualCantidad" runat="server" TargetControlID="txtManualCantidad" FilterType="Numbers,Custom" ValidChars=".," />                                
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:TextBox runat="server" ID="txtManualComen" TextMode="MultiLine" CssClass="form-control required" Rows="3"></asp:TextBox>
                    </div>                    
                </div>
                <div class="modal-footer">
                    <div class="col-sm-12"><asp:Button ID="btnManualGuardar" runat="server" text="Aceptar" CssClass="form-control btn btn-primary" /></div>
                </div>
            </div>
        </div>
    </div>    
     <div id="divModal" class="modal fade" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal"></asp:Label></h4>
                    <asp:HiddenField runat="server" ID="hfModalAction" />
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="labelConfirmMessageModal"></asp:Label>
                    <asp:HiddenField runat="server" ID="hfModalParam" />
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnAceptarModalM" Text="Aceptar" cssclass="btn btn-primary" /> 
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModal" Text="Cancelar"></asp:Label></button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hfDeleteMovId" />
    <div class="modal fade" id="confirmDeleteMov">
        <div class="modal-dialog modal-confirm">
            <div class="modal-content">            
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabelMov"><asp:Label runat="server" ID="labelConfirmDeleteTitleMov" Text="Confirmar borrado"></asp:Label></h4>                    
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="labelConfirmMessageMov" Text="Esta seguro de que desea eliminar el movimiento"></asp:Label>
                </div>
                <div class="modal-footer">                          
                    <asp:Button runat="server" ID="btnEliminarModalMov" Text="Eliminar" cssclass="btn btn-primary" /> 
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModalMov" Text="Cancelar"></asp:Label></button>                          
                </div>
            </div>
        </div>
    </div>
    <uc:CargandoDatos runat="server" />
</asp:Content>
