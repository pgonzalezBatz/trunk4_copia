<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DetPresupuesto.aspx.vb" Inherits="WebRaiz.DetPresupuesto" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelViaje" Text="Viaje"></asp:Label></div>
        <div class="col-sm-4"><b><asp:Label runat="server" ID="lblIdViaje"></asp:Label></b></div>       
        <div class="col-sm-6">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:LinkButton runat="server" ID="lnkHistoricoEstados" Text="Ver historico"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelSolicitante" Text="Solicitante"></asp:Label></div>
        <div class="col-sm-4"><b><asp:Label runat="server" ID="lblSolicitante"></asp:Label></b></div>       
        <div class="col-sm-2"><asp:Label runat="server" ID="labelRespVal" Text="Responsable"></asp:Label></div>
        <div class="col-sm-4"><b><asp:Label runat="server" ID="lblRespVal"></asp:Label></b></div>       
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelDescViaje" Text="Descripción"></asp:Label></div>
        <div class="col-sm-10"><b><asp:Label runat="server" ID="lblDescViaje"></asp:Label></b></div>       
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelViajeros" Text="Viajeros"></asp:Label></div>
        <div class="col-sm-10">
            <asp:GridView runat="server" ID="gvIntegrantes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		      
		        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		        <Columns>                             
                    <asp:TemplateField HeaderText="Integrante">				           
				        <ItemTemplate><asp:Label runat="server" ID="lblIntegrante"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:Templatefield HeaderText="Fechas viaje">
				        <ItemTemplate><asp:Label runat="server" ID="lblFechasViaje"></asp:Label></ItemTemplate>
                    </asp:Templatefield>
			        <asp:TemplateField HeaderText="Plan de viaje" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				        <ItemTemplate><asp:Label runat="server" ID="lblPlanViaje"></asp:Label></ItemTemplate>
			        </asp:TemplateField>           	                       
		        </Columns>        
	        </asp:GridView>
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" ID="labelInfoPlan" Text="Cada plan de viaje incluye los gastos de un grupo de personas que viajan en fechas distintas" CssClass="help-block"></asp:Label>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelEstado" Text="Estado"></asp:Label></div>
        <div class="col-sm-4"><b><asp:Label runat="server" ID="lblEstado" style="font-size:16px"></asp:Label></b></div>
        <div class="col-sm-6 form-inline">
            <b>
                <asp:Label runat="server" ID="labelRespondidoPor" Text="Respondido por"></asp:Label>
                <asp:Label runat="server" ID="lblUserRespuesta"></asp:Label>
            </b>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelFLimite" Text="Validez hasta"></asp:Label></div>
        <div class="col-sm-4"><b><asp:Label runat="server" ID="lblFLimite"></asp:Label></b></div>
    </div>    
    <div class="row well">
        <div class="col-sm-2"><b><asp:Label runat="server" ID="labelObjetivoTotal" Text="Objetivo" style="font-size:16px;"></asp:Label></b></div>
        <div class="col-sm-4">
            <b><asp:Label runat="server" ID="lblObjTotal" style="font-size:18px"></asp:Label>
            <asp:Label runat="server" Text="€" style="font-size:15px;"></asp:Label>
            </b>
        </div>
        <div class="col-sm-2"><b><asp:Label runat="server" ID="labelPresupTotal" Text="Presupuesto total" style="font-size:16px;"></asp:Label></b></div>
        <div class="col-sm-4">
            <b><asp:Label runat="server" ID="lblTotal" style="font-size:18px"></asp:Label></b>
            <asp:Label runat="server" Text="€" style="font-size:15px;"></asp:Label>
        </div>        
    </div>
    <asp:Panel runat="server" ID="pnlDiasAntelacionSolicitud">
        <b><asp:Label runat="server" ID="lblTextoDiasAntelacionSol" style="font-size:15px;"></asp:Label></b>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlDiasAntelacionOK" CssClass="alert alert-success">
        <span class="glyphicon glyphicon-ok"></span>
        <b><asp:Label runat="server" ID="lblTextoDiasAntelacionOk" Text="El viaje se ha validado con la antelacion suficiente" style="font-size:15px;"></asp:Label></b>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlDiasAntelacionNoOK" CssClass="alert alert-danger">
        <asp:Image runat="server" ID="imgIconoDiasAntelacionNoOk" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/advertencia.gif" />        
        <b><asp:Label runat="server" ID="labelTextoNoOkAnt1" Text="ATENCION: La validacion del viaje se esta realizando fuera de plazo. Validarlo con antelacion suficiente puede suponer un ahorro de hasta el 20%." style="text-decoration: underline;"></asp:Label>
        <asp:Label runat="server" ID="labelTextoNoOkAnt2" Text="Objetivo para planificar y emitir billetes aereos" /></b>
        <ul style="padding-left:30px;">
            <li><b><asp:Label runat="server" ID="labelTextoNoOkAnt3" Text="NACIONAL + EUROPA > [DIAS] DIAS"></asp:Label></b></li>
            <li><b><asp:Label runat="server" ID="labelTextoNoOkAnt4" Text="INTERCONTINENTAL > [DIAS] DIAS"></asp:Label></b></li>
        </ul>
    </asp:Panel>
    <div class="form-group"><asp:Label runat="server" ID="labelDesglose" Text="A continuacion se desglosa el presupuesto en sus diferentes conceptos"></asp:Label></div>    
    <asp:PlaceHolder runat="server" ID="phDetalle">
    </asp:PlaceHolder>
    <div class="form-group">
        <asp:Label runat="server" ID="labelObserAg" Text="Observaciones de la agencia" /><br />
        <b><asp:Label runat="server" ID="lblObservArg"></asp:Label></b>
    </div>
    <div class="form-group">
        <asp:Label runat="server" ID="labelObserVal" Text="Añada aqui sus observaciones" /><br />
        <asp:Textbox runat="server" ID="txtObservVal" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:Textbox>
    </div>    
    <asp:Panel runat="server" ID="pnlBotones" CssClass="row">
        <div class="col-sm-2"><asp:Button runat="server" ID="btnAprobar" Text="Aprobar" CommandName="A" CssClass="form-control btn btn-success" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnRechazar" Text="Rechazar" CommandName="R" CssClass="form-control btn btn-danger" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>        
    </asp:Panel>  
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
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
             <div id="divModalHistorico" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalHistorico" Text="Historico de estados"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField runat="server" ID="hfIdViaje" />
                            <asp:GridView runat="server" ID="gvEstados" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped" GridLines="None">				            
				                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				                <Columns>				
					                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
						                <HeaderTemplate><asp:Label runat="server" Text="Fecha"></asp:Label></HeaderTemplate>
						                <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
					                </asp:TemplateField>								
					                <asp:TemplateField>
						                <HeaderTemplate><asp:Label runat="server" Text="Estado"></asp:Label></HeaderTemplate>
						                <ItemTemplate><asp:Label runat="server" ID="lblEstado"></asp:Label></ItemTemplate>
					                </asp:TemplateField>
                                    <asp:TemplateField>
						                <HeaderTemplate><asp:Label runat="server" Text="Usuario"></asp:Label></HeaderTemplate>
						                <ItemTemplate><asp:Label runat="server" ID="lblUsuario"></asp:Label></ItemTemplate>
					                </asp:TemplateField>
				                </Columns>
			                </asp:GridView> 
                        </div>               
                    </div>
                </div>
            </div> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
