<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="PresupServiciosNew.aspx.vb" Inherits="WebRaiz.PresupServiciosNew" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">        
    <script type="text/javascript" src="../Scripts/utiles.js"></script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Timer runat="server" ID="temporizador"></asp:Timer>		
        </ContentTemplate>
    </asp:UpdatePanel> 
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivInfo" Text="Informacion del viaje"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelViaje" Text="Viaje"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblIdViaje" CssClass="label label-info" style="font-size:18px"></asp:Label></b></div>                   
                <div class="col-sm-2"><asp:Label runat="server" ID="labelFechas" Text="Fechas"></asp:Label></div>
                <div class="col-sm-4 form-inline">
                    <b>
                        <asp:Label runat="server" ID="lblFIda"></asp:Label>-
                        <asp:Label runat="server" ID="lblFVuelta"></asp:Label>
                    </b>
                </div>
            </div>
             <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelSolicitante" Text="Solicitante"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblSolicitante"></asp:Label></b></div>       
                <div class="col-sm-2"><asp:Label runat="server" ID="labelRespVal" Text="Responsable"></asp:Label></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblRespVal"></asp:Label></b>
                    <asp:HiddenField runat="server" ID="hfRespVal" />
                </div>       
            </div>
            <div class="row">                
                <div class="col-sm-2"><asp:Label runat="server" ID="labelNivel" Text="Tipo de viaje"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblNivel"></asp:Label></b></div>
             </div>
            <div class="form-group">                
                <asp:GridView runat="server" ID="gvIntegrantes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None">		            
		            <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		            <Columns>
                            <asp:TemplateField Visible="false">
				            <itemTemplate><asp:Label runat="server" ID="lblIdUser"></asp:Label></ItemTemplate>
			            </asp:TemplateField>      
                            <asp:TemplateField HeaderText="Integrante">				           
				            <ItemTemplate><asp:Label runat="server" ID="lblIntegrante"></asp:Label></ItemTemplate>
			            </asp:TemplateField>
                        <asp:Templatefield HeaderText="Fechas viaje">                           
				            <ItemTemplate><asp:Label runat="server" ID="lblFechasViaje"></asp:Label></ItemTemplate>
                        </asp:Templatefield>       	                       
		            </Columns>        
	            </asp:GridView>
                <asp:HiddenField runat="server" ID="hfNumInt" />
            </div>            
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelEstado" Text="Estado"></asp:Label></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblEstado" style="font-size:16px"></asp:Label></b>
                    <asp:HiddenField runat="server" ID="hfEstado" />
                </div>
                <div class="col-sm-6 form-inline">
                    <asp:Label runat="server" ID="labelRespondidoPor" Text="Validado por"></asp:Label>
                    <b><asp:Label runat="server" ID="lblUserRespuesta"></asp:Label></b>
                </div>
            </div>            
        </div>
     </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivObserv" Text="Observaciones"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:TextBox runat="server" ID="txtObservaciones" Rows="5" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-inline">
        <asp:Button runat="server" ID="btnCrearPresupTop" Text="Crear presupuesto" CssClass="form-control btn btn-success" />
        <asp:Panel runat="server" ID="pnlBotonesTop">
            <asp:Button runat="server" ID="btnGuardarTop" ValidationGroup="Guardar" Text="Guardar" CssClass="form-control btn btn-primary" />
            <asp:Button runat="server" ID="btnEnviarTop" ValidationGroup="Guardar" Text="Enviar y guardar" CssClass="form-control btn btn-success" />
            <asp:Button runat="server" ID="btnPrevisualizarTop" Text="Previsualizar" ToolTip="Previsualizar el presupuesto como lo ve el validador" CssClass="form-control btn btn-primary" />            
            <asp:Button runat="server" ID="btnVolverTop" Text="Volver" CssClass="form-control btn btn-default" />            
        </asp:Panel>
    </div>                
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row well">
                <div class="col-sm-2"><b><asp:Label runat="server" ID="labelObjetivoTotal" Text="Objetivo" style="font-size:16px;"></asp:Label></b></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblObjTotal" style="font-size:18px"></asp:Label>
                    <asp:Label runat="server" Text="€" style="font-size:15px;"></asp:Label></b>
                    <asp:HiddenField runat="server" ID="hfObjetivo" />
                </div>
                <div class="col-sm-2"><b><asp:Label runat="server" ID="labelPresupTotal" Text="Presupuesto total" style="font-size:16px;"></asp:Label></b></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblTotal" style="font-size:18px"></asp:Label>
                    <asp:Label runat="server" Text="€" style="font-size:15px;"></asp:Label></b>
                    <asp:HiddenField runat="server" ID="hfReal" />
                </div>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="panel panel-primary" runat="server" id="divAvion">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivServAereo" Text="Servicios aereos"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-12"><asp:Label runat="server" ID="lblAOrigenTarifa" CssClass="help-block"></asp:Label></div>
                    </div>
                    <div class="form-group">                            
                        <table class="table table-striped table-condensed">
                            <tr>
                                <th style="width:20%">&nbsp;</th>
                                <th><asp:Label runat="server" ID="labelATarifaObjCab" Text="Objetivo"></asp:Label></th>
                                <th><asp:Label runat="server" ID="labelATarifaRealCab" Text="Real"></asp:Label></th>
                            </tr>
                            <tr>
                                <th style="width:20%"><asp:Label runat="server" ID="labelATarifaPerso" Text="1 persona"></asp:Label></th>
                                <td><asp:Label runat="server" ID="lblATarifaObjPerso"></asp:Label></td>
                                <td>
                                    <asp:Textbox runat="server" ID="txtATarifaRealPerso" CssClass="form-control required"></asp:Textbox>   
                                    <asp:RequiredFieldValidator ID="rfvATarifaRealPerso" runat="server" Display="None" ControlToValidate="txtATarifaRealPerso" ValidationGroup="Guardar" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbATarifaRealPerso" runat="server" TargetControlID="txtATarifaRealPerso" FilterType="Numbers,Custom" ValidChars=".," />  
                                </td>
                            </tr> 
                            <tr>
                                <th style="width:20%"><asp:Label runat="server" ID="labelATarifaTotal"></asp:Label></th>
                                <td><asp:Label runat="server" ID="lblATarifaObjTotal"></asp:Label></td>
                                <td><asp:Label runat="server" ID="lblATarifaRealTotal"></asp:Label></td>                                    
                            </tr>
                        </table>
                    </div>                        
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>   
    <div class="panel panel-primary" runat="server" id="divHotel">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivHotel" Text="Hoteles"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate> 
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelHDias" Text="Num noches"></asp:Label></div>
                        <div class="col-sm-2">
                            <asp:TextBox runat="server" ID="txtHNumDias" CssClass="form-control text-right required"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvHNumDias" runat="server" Display="None" ControlToValidate="txtHNumDias" ValidationGroup="Guardar" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeHNumDias" runat="server" TargetControlID="txtHNumDias" FilterType="Numbers" />  
                        </div>
                        <div class="col-sm-6 col-sm-offset-2"><asp:Label runat="server" ID="lblHOrigenTarifa" CssClass="help-block"></asp:Label></div>
                    </div>                        
                    <div class="form-group">                            
                        <table class="table table-striped table-condensed">
                            <tr>
                                <th style="width:20%">&nbsp;</th>
                                <th><asp:Label runat="server" ID="labelHTarifaObjCab" Text="Objetivo"></asp:Label></th>
                                <th><asp:Label runat="server" ID="labelHTarifaRealCab" Text="Real"></asp:Label></th>
                            </tr>
                            <tr>
                                <th style="width:20%"><asp:Label runat="server" ID="labelHTarifaDiaPerso" Text="1 noche/persona"></asp:Label></th>
                                <td><asp:Label runat="server" ID="lblHTarifaObjDiaPerso"></asp:Label></td>
                                <td>
                                    <asp:Textbox runat="server" ID="txtHTarifaRealDiaPerso" CssClass="form-control required"></asp:Textbox>
                                    <ajax:FilteredTextBoxExtender ID="ftbHTarifaRealDiaPerso" runat="server" TargetControlID="txtHTarifaRealDiaPerso" FilterType="Numbers,Custom" ValidChars=".," /> 
                                </td>
                            </tr> 
                            <tr>
                                <th style="width:20%"><asp:Label runat="server" ID="labelHTarifaTotal"></asp:Label></th>
                                <td><asp:Label runat="server" ID="lblHTarifaObjTotal"></asp:Label></td>
                                <td><asp:Label runat="server" ID="lblHTarifaRealTotal"></asp:Label></td>                                    
                            </tr>
                        </table>
                    </div>                                                     
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>        
    <div class="panel panel-primary" runat="server" id="divCocheAlq">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivCoche" Text="Coches de alquiler"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate> 
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelCNumDias" Text="Num dias"></asp:Label></div>
                        <div class="col-sm-2">
                            <asp:TextBox runat="server" ID="txtCNumDias" CssClass="form-control text-right required"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCNumDias" runat="server" Display="None" ControlToValidate="txtCNumDias" ValidationGroup="Guardar" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeCNumDias" runat="server" TargetControlID="txtCNumDias" FilterType="Numbers" />  
                        </div> 
                        <div class="col-sm-6 col-sm-offset-2"><asp:Label runat="server" ID="lblCOrigenTarifa" CssClass="help-block"></asp:Label></div>
                    </div>                                                 
                    <div class="form-group">                            
                        <table class="table table-striped table-condensed">
                            <tr>
                                <th style="width:20%">&nbsp;</th>
                                <th><asp:Label runat="server" ID="labelCTarifaObjCab" Text="Objetivo"></asp:Label></th>                                    
                                <th><asp:Label runat="server" ID="labelCTarifaRealCab" Text="Real"></asp:Label></th>                                    
                            </tr>
                            <tr>
                                <th style="width:20%"><asp:Label runat="server" ID="labelCTarifaObjDia" Text="1 dia"></asp:Label></th>
                                <td><asp:Label runat="server" ID="lblCTarifaObjDia"></asp:Label></td>
                                <td>
                                    <asp:Textbox runat="server" ID="txtCTarifaRealDia" CssClass="form-control required"></asp:Textbox>
                                    <ajax:FilteredTextBoxExtender ID="ftbeCTarifaRealDia" runat="server" TargetControlID="txtCTarifaRealDia" FilterType="Numbers,Custom" ValidChars=".," /> 
                                </td>                                    
                            </tr>
                                <tr>
                                <th style="width:20%"><asp:Label runat="server" ID="labelCTarifaTotal"></asp:Label></th>
                                <td><asp:Label runat="server" ID="lblCTarifaObjTotal"></asp:Label></td>
                                <td><asp:Label runat="server" ID="lblCTarifaRealTotal"></asp:Label></td>                                    
                            </tr>
                        </table>
                    </div>                        
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div><br />
    <div class="form-inline">
        <asp:Button runat="server" ID="btnCrearPresupBottom" Text="Crear presupuesto" CssClass="form-control btn btn-success" />
        <asp:Panel runat="server" ID="pnlBotonesBottom">
            <asp:Button runat="server" ID="btnGuardarBottom" ValidationGroup="Guardar" Text="Guardar" CssClass="form-control btn btn-primary" />
            <asp:Button runat="server" ID="btnEnviarBottom" ValidationGroup="Guardar" Text="Enviar y guardar" CssClass="form-control btn btn-success" />
            <asp:Button runat="server" ID="btnPrevisualizarBottom" Text="Previsualizar" ToolTip="Previsualizar el presupuesto como lo ve el validador" CssClass="form-control btn btn-primary" />            
            <asp:Button runat="server" ID="btnVolverBottom" Text="Volver" CssClass="form-control btn btn-default" />            
        </asp:Panel>
    </div> 
    <asp:UpdatePanel runat="server">
        <ContentTemplate>  
            <div id="divModalDel" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalDelete" Text="Confirmar borrado"></asp:Label></h4>
                            <asp:HiddenField runat="server" ID="hfModalAction" />
                        </div>
                        <div class="modal-body">
                            <asp:Label runat="server" ID="labelConfirmMessageModal" Text="confirmarEliminar"></asp:Label>
                            <asp:HiddenField runat="server" ID="hfModalParam" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnAceptarModalDel" Text="Aceptar" cssclass="btn btn-primary" /> 
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModalDel" Text="Cancelar"></asp:Label></button>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divEnviar" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleConfirmEnvio" Text="Envio de presupuesto"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelModalEnvioMessage"></asp:Label>                                
                            </div>                            
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-2"><asp:Button ID="btnModalEnviarPresup" runat="server" text="Enviar" CssClass="form-control btn btn-primary" /></div>
                            <div class="col-sm-2"><button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelModalCancelEnviarPresup" Text="Cancelar"></asp:Label></button></div>
                        </div>
                    </div>
                </div>
            </div>                       
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" ID="PanelCargandoDatos" />
    <script>
        $().ready(function () {
            $('[id$=txtATarifaRealPerso],[id$=txtHTarifaRealDiaPerso],[id$=txtCTarifaRealDia],[id$=txtHNumDias],[id$=txtCNumDias]').change(function () {
                var totalReal = 0;
                var totalObj = 0;                
                var numInteg = $("#" + '<%=hfNumInt.ClientID%>').val();
                //Avion
                var lblObjA = $("#" + '<%=lblATarifaObjPerso.ClientID%>');
                if (typeof lblObjA !== "undefined") { //Se comprueba si esta visible el panel de avion
                    var objA = parse_float(lblObjA.text()) * numInteg;
                    var realA = parse_float($("#" + '<%=txtATarifaRealPerso.ClientID%>').val()) * numInteg;
                    $("#" + '<%=lblATarifaObjTotal.ClientID%>').text(objA);
                    $("#" + '<%=lblATarifaRealTotal.ClientID%>').text(realA);
                    totalObj += objA;
                    totalReal += realA;
                }
                //Hotel            
                var numDiasH = $("#" + '<%=txtHNumDias.ClientID%>').val();
                if (typeof numDiasH !== "undefined") { //Se comprueba si esta visible el panel de hotel
                    var objH = parse_float($("#" + '<%=lblHTarifaObjDiaPerso.ClientID%>').text()) * numInteg * numDiasH;
                    var realH = parse_float($("#" + '<%=txtHTarifaRealDiaPerso.ClientID%>').val()) * numInteg * numDiasH;;
                    $("#" + '<%=lblHTarifaObjTotal.ClientID%>').text(objH);
                    $("#" + '<%=lblHTarifaRealTotal.ClientID%>').text(realH);
                    totalObj += objH;
                    totalReal += realH;
                }
                //Coche alquiler            
                var numDiasC = $("#" + '<%=txtCNumDias.ClientID%>').val();
                if (typeof numDiasC !== "undefined") { //Se comprueba si esta visible el panel de coche de alquiler
                    var objC = parse_float($("#" + '<%=lblCTarifaObjDia.ClientID%>').text()) * numDiasC;
                    var realC = parse_float($("#" + '<%=txtCTarifaRealDia.ClientID%>').val()) * numDiasC;
                    $("#" + '<%=lblCTarifaObjTotal.ClientID%>').text(objC);
                    $("#" + '<%=lblCTarifaRealTotal.ClientID%>').text(realC);
                    totalObj += objC;
                    totalReal += realC;
                }
                $("#" + '<%=lblObjTotal.ClientID%>').text(totalObj);
                var lblTotal = $("#" + '<%=lblTotal.ClientID%>');
                lblTotal.text(totalReal);
                if (totalReal <= totalObj)
                    lblTotal.attr("class", "label label-success");
                else
                    lblTotal.attr("class", "label label-danger");
                $("#" + '<%=hfObjetivo.ClientID%>').val(totalObj);
                $("#" + '<%=hfReal.ClientID%>').val(totalReal);
            });
        });
    </script>
</asp:Content>
