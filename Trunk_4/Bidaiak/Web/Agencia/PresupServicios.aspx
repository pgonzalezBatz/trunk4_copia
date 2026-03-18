<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="PresupServicios.aspx.vb" Inherits="WebRaiz.PresupServicios" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">        
    <script type="text/javascript">

        //Actualiza la tarifa del hotel dependiendo del numero de dias
        /*function ActualizarTarifaHotel() {
            var fDesde, fHasta;                        
            var FechaDesde= document.getElementById('txtHFEntrada.ClientID');
            var FechaHasta = document.getElementById('txtHFSalida.ClientID');
            if (FechaDesde.value!='' && FechaHasta.value!='')
            {
                fDesde = parseDate(FechaDesde.value);
                fHasta = parseDate(FechaHasta.value);
                var tarifaR = document.getElementById('txtHTarifaDiaR.ClientID ');
                var tarifaTotalR = document.getElementById('lblHTarifaTotalR.ClientID');
                var tarifaO = document.getElementById('lblHTarifaDiaO.ClientID>');
                var tarifaTotalO = document.getElementById('lblHTarifaTotalO.ClientID');
                var diff = Math.abs(fHasta - fDesde);                
                var one_day=1000*60*60*24; //Get 1 day in milliseconds
                var daysNumber = parseInt(Math.floor(diff / one_day));
                if (tarifaO.innerHTML != undefined)
                    tarifaTotalO.innerHTML = tarifaO.innerHTML * daysNumber;
                else
                    tarifaTotalO.innerHTML = "0";
                if (tarifaR.value != undefined)
                    tarifaTotalR.innerHTML = tarifaR.value * daysNumber;
                else
                    tarifaTotalR.innerHTML = "0";
            }
        }*/

        //Actualiza la tarifa del coche de alquiler dependiendo del numero de dias
        /*function ActualizarTarifaCocheAlq() {
            var fDesde, fHasta;
            var FechaDesde = document.getElementById('txtCDiaRecog.ClientID');
            var FechaHasta = document.getElementById('txtCDiaDev.ClientID');
            if (FechaDesde.value != '' && FechaHasta.value != '') {
                fDesde = parseDate(FechaDesde.value);
                if (document.getElementById('txtCHoraRecog.ClientID').value != '') {
                    var hora = document.getElementById('txtCHoraRecog.ClientID').value.split(":");
                    fDesde.setHours(hora[0]);
                    fDesde.setMinutes(hora[1]);
                }
                
                fHasta = parseDate(FechaHasta.value);
                if (document.getElementById('txtCHoraDev.ClientID').value != '') {
                    var hora = document.getElementById('txtCHoraDev.ClientID').value.split(":");
                    fHasta.setHours(hora[0]);
                    fHasta.setMinutes(hora[1]);
                }

                var tarifa = document.getElementById('txtCTarifa1Dia.ClientID');
                var tarifaTotal = document.getElementById('lblCTarifaTotal.ClientID');
                
                var diff = Math.abs(fHasta - fDesde);
                var one_day = 1000 * 60 * 60 * 24; //Get 1 day in milliseconds
                var daysNumber = parseInt(Math.ceil(diff / one_day));
                tarifaTotal.innerHTML = tarifa.value * daysNumber;
            }
        }*/

        function SelectFlightRate() {
            var btn = document.getElementById('<%=btnACiudadTarif.ClientID%>');
            btn.click();
        }

        function SelectHotelRate() {
            var btn = document.getElementById('<%=btnHCiudadTarif.ClientID%>');
            btn.click();
        }

        function SelectCocheRate() {
            var btn = document.getElementById('<%=btnCCiudadTarif.ClientID%>');
            btn.click();
        }

        //Parsea un string a fecha
        function parseDate(str) {
            var mdy = str.split('/');
            return new Date(mdy[2], mdy[1],mdy[0]);
        }

        $(document).ready(function () {            
            $('#<%=txtTarifDestino.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearchT.ClientID%>').click();
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            $('#<%=txtTarifDestino.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearchT.ClientID%>').click();
            });
        });

    </script>
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
                <div class="col-sm-2"><asp:Label runat="server" ID="labelViajeros" Text="Viajeros"></asp:Label></div>        
                <div class="col-sm-10 form-inline">
                    <asp:Label runat="server" ID="labelNumPlanes" Text="Seleccione el numero de planes de viaje" />
                    <asp:DropDownList runat="server" ID="ddlNumPlanViajes" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                </div>
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
			            <asp:TemplateField HeaderText="Plan de viaje">				           
				            <ItemTemplate><asp:DropDownList runat="server" ID="ddlPlanViaje" CssClass="form-control"></asp:DropDownList></ItemTemplate>
			            </asp:TemplateField>           	                       
		            </Columns>        
	            </asp:GridView>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelFLimite" Text="Validez hasta"></asp:Label></div>
                <div class="col-sm-4">
                     <div class="input-group date" id="dtDateLimit">
                        <asp:TextBox runat="server" ID="txtFechaLimite" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                    </div>
                </div>
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
            <div class="row" runat="server" id="trNewPresupState">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelNewState" Text="Nuevo estado"></asp:Label></div>
                <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlEstado" CssClass="form-control"></asp:DropDownList></div>
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
        <asp:Button runat="server" ID="btnCrearPresup" Text="Crear presupuesto" CssClass="form-control btn btn-success" />
        <asp:Panel runat="server" ID="pnlBotones">
            <asp:Button runat="server" ID="btnGuardar" ValidationGroup="Guardar" Text="Guardar" CssClass="form-control btn btn-primary" />
            <asp:Button runat="server" ID="btnEnviar" ValidationGroup="Guardar" Text="Enviar y guardar" CssClass="form-control btn btn-success" />
            <asp:Button runat="server" ID="btnPrevisualizar" Text="Previsualizar" ToolTip="Previsualizar el presupuesto como lo ve el validador" CssClass="form-control btn btn-primary" />            
            <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" />            
        </asp:Panel>
    </div>            
    <asp:Panel runat="server" ID="pnlServicios">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="form-inline">
            <asp:Label runat="server" ID="labelSelPlanViaje" Text="Seleccione el plan de viaje a configurar" />
            <asp:DropDownList runat="server" ID="ddlPlanViaje" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-inline">
            <asp:Label runat="server" ID="labelIntegrantesPlan" Text="Numero de integrantes con el plan"></asp:Label>
            <b><asp:Label runat="server" ID="lblIntegrPlan"></asp:Label></b>
        </div><br />
        <div class="panel panel-primary">
            <div class="panel-heading">
                <strong><asp:Label runat="server" ID="labelDivServAereo" Text="Servicios aereos"></asp:Label></strong>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate> 
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelADia" Text="Dia"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtAFecha">
                                    <asp:TextBox runat="server" ID="txtADia" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelAHoraSalida" Text="Hora salida a partir de"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtAHora">
                                    <asp:TextBox runat="server" ID="txtAHoraSalida" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                </div>    
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelACiudadOrigen" Text="Ciudad origen"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtACiudadOrigen" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvACO" runat="server" Display="None" ControlToValidate="txtACiudadOrigen" ValidationGroup="GuardarA" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelACiudadDestino" Text="Ciudad destino"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtACiudadDestino" CssClass="form-control" onBlur="SelectFlightRate();" />
                                <asp:RequiredFieldValidator ID="rfvACD" runat="server" Display="None" ControlToValidate="txtACiudadDestino" ValidationGroup="GuardarA" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 form-inline">
                                <asp:Button runat="server" ID="btnACiudadTarif" Text="Seleccionar tarifa" CssClass="form-control btn btn-primary" />
                                <b><asp:Label runat="server" ID="lblACiudadTarif" style="margin-left:15px;"></asp:Label></b>
                                <asp:HiddenField runat="server" ID="hfACiudadTarif" />
                            </div>
                        </div>
                        <div class="form-group">                            
                            <table class="table table-striped table-condensed">
                                <tr>
                                    <th>&nbsp;</th>
                                    <th><asp:Label runat="server" ID="labelATarifaObjCab" Text="Objetivo"></asp:Label></th>
                                    <th><asp:Label runat="server" ID="labelATarifaRealCab" Text="Real"></asp:Label></th>
                                </tr>
                                <tr>
                                    <th><asp:Label runat="server" ID="labelATarifa1Persona" Text="1 persona"></asp:Label></th>
                                    <td><asp:Label runat="server" ID="lblATarifaObj"></asp:Label></td>
                                    <td>
                                        <asp:Textbox runat="server" ID="txtATarifaReal" CssClass="form-control"></asp:Textbox>   
                                        <ajax:FilteredTextBoxExtender ID="ftbATarifaReal" runat="server" TargetControlID="txtATarifaReal" FilterType="Numbers,Custom" ValidChars=".," />  
                                    </td>
                                </tr>                            
                            </table>
                        </div>
                        <div class="form-inline">
                            <asp:Button runat="server" ID="btnAGuardar" Text="Guardar" ValidationGroup="GuardarA" CssClass="form-control btn btn-primary" />
                            <asp:Button runat="server" ID="btnACancelar" Text="Cancelar" ToolTip="Limpia las cajas de texto para introducir uno nuevo" CssClass="form-control btn btn-default" />
                        </div>
                        <div class="form-group">
                            <asp:GridView runat="server" ID="gvAviones" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" ShowFooter="true">		                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkEdit" CommandName="Sel"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>      
                                        <asp:TemplateField HeaderText="Plan viaje">				           
				                        <ItemTemplate><asp:Label runat="server" ID="lblPlanViaje"></asp:Label></ItemTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad Origen" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
				                        <ItemTemplate><asp:Label runat="server" ID="lblCiudadOrigen"></asp:Label></ItemTemplate>
			                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Ciudad Destino">
				                        <ItemTemplate><asp:Label runat="server" ID="lblCiudadDestino"></asp:Label></ItemTemplate>
			                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha">
				                        <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
                                            <FooterTemplate><asp:Label runat="server" Text="Total (n personas)"></asp:Label></FooterTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tarifa Objetivo">
				                        <ItemTemplate><asp:Label runat="server" ID="lblTarifaObj"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="lblTotalObj"></asp:Label></FooterTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tarifa Real">
				                        <ItemTemplate><asp:Label runat="server" ID="lblTarifaReal"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="lblTotalReal"></asp:Label></FooterTemplate>
			                        </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Elim" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkDel" CommandName="Del"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>          	                       
		                        </Columns>        
	                        </asp:GridView>   
                        </div>                   
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>   
        <div class="panel panel-primary">
            <div class="panel-heading">
                <strong><asp:Label runat="server" ID="labelDivHotel" Text="Hoteles"></asp:Label></strong>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate> 
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelHCiudad" Text="Ciudad"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtHCiudad" onBlur="SelectHotelRate();" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvHCiudad" runat="server" Display="None" ControlToValidate="txtHCiudad" ValidationGroup="GuardarH" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 form-inline">
                                <asp:Button runat="server" ID="btnHCiudadTarif" Text="Seleccionar tarifa" CssClass="form-control btn btn-primary" />
                                <b><asp:Label runat="server" ID="lblHCiudadTarif" style="margin-left:15px;"></asp:Label></b>
                                <asp:HiddenField runat="server" ID="hfHCiudadTarif" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelHNombre" Text="Hotel"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtHNombre" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvHNombre" runat="server" Display="None" ControlToValidate="txtHNombre" ValidationGroup="GuardarH" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>                                
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelHFEntrada" Text="Entrada"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtHFEntrada">
                                    <asp:TextBox runat="server" ID="txtHFEntrada" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelHFSalida" Text="Salida"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtHFSalida">
                                    <asp:TextBox runat="server" ID="txtHFSalida" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelHTipoHab" Text="Tipo habitacion"></asp:Label></div>
                            <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlHTipoHab" CssClass="form-control"></asp:DropDownList></div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelHRegimen" Text="Regimen"></asp:Label></div>
                            <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlHRegimen" CssClass="form-control"></asp:DropDownList></div>
                        </div>
                        <div class="form-group">                            
                            <table class="table table-striped table-condensed">
                                <tr>
                                    <th>&nbsp;</th>
                                    <th><asp:Label runat="server" ID="labelHTarifaObjCab" Text="Objetivo"></asp:Label></th>
                                    <th><asp:Label runat="server" ID="labelHTarifaRealCab" Text="Real"></asp:Label></th>
                                </tr>
                                <tr>
                                    <th><asp:Label runat="server" ID="labelHTarifaDia" Text="1 dia/persona"></asp:Label></th>
                                    <td><asp:Label runat="server" ID="lblHTarifaDiaO"></asp:Label></td>
                                    <td>
                                        <asp:Textbox runat="server" ID="txtHTarifaDiaR" CssClass="form-control"></asp:Textbox>
                                        <ajax:FilteredTextBoxExtender ID="ftbHTarifaDiaR" runat="server" TargetControlID="txtHTarifaDiaR" FilterType="Numbers,Custom" ValidChars=".," /> 
                                    </td>
                                </tr>                                
                            </table>
                        </div>
                        <div class="form-inline">
                            <asp:Button runat="server" ID="btnHGuardar" Text="Guardar" ValidationGroup="GuardarH" CssClass="form-control btn btn-primary" />
                            <asp:Button runat="server" ID="btnHCancelar" Text="Cancelar" ToolTip="Limpia las cajas de texto para introducir uno nuevo" CssClass="form-control btn btn-default" />
                        </div>                    
                        <div class="form-group">                         
                            <asp:GridView runat="server" ID="gvHoteles" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" ShowFooter="true">		                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkEdit" CommandName="Sel"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>      
                                        <asp:TemplateField HeaderText="Plan viaje">				           
				                        <ItemTemplate><asp:Label runat="server" ID="lblPlanViaje"></asp:Label></ItemTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hotel">
				                        <ItemTemplate><asp:Label runat="server" ID="lblHotel"></asp:Label></ItemTemplate>
			                        </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="F. Entrada">
				                        <ItemTemplate><asp:Label runat="server" ID="lblFEntrada"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="labelTotal" Text="Total (n personas y n dias)"></asp:Label></FooterTemplate>
			                        </asp:TemplateField>                                     
                                    <asp:TemplateField HeaderText="Tarifa Objetivo">
				                        <ItemTemplate><asp:Label runat="server" ID="lblTarifaObj"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="lblTotalObj"></asp:Label></FooterTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tarifa Real">
				                        <ItemTemplate><asp:Label runat="server" ID="lblTarifaReal"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="lblTotalReal"></asp:Label></FooterTemplate>
			                        </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Elim" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkDel" CommandName="Del"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>          	                       
		                        </Columns>        
	                        </asp:GridView>
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
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCConductor" Text="Conductor"></asp:Label></div>
                            <div class="col-sm-4"><b><asp:Label runat="server" ID="lblCConductor" CssClass="labelDetalle"></asp:Label></b></div>                                                        
                        </div>                         
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCCiudadRec" Text="Lugar recogida"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtCLugarRecogida" CssClass="form-control" onBlur="SelectCocheRate();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCLugarRec" runat="server" Display="None" ControlToValidate="txtCLugarRecogida" ValidationGroup="GuardarC" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>                           
                            <div class="col-sm-6 form-inline">
                                <asp:Button runat="server" ID="btnCCiudadTarif" Text="Seleccionar tarifa" CssClass="form-control btn btn-primary" />
                                <b><asp:Label runat="server" ID="lblCCiudadTarif" style="margin-left:15px;"></asp:Label></b>
                                <asp:HiddenField runat="server" ID="hfCCiudadTarif" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCDiaRecog" Text="Dia"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtCDiaRecog">
                                    <asp:TextBox runat="server" ID="txtCDiaRecog" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>                            
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCHoraRecog" Text="Hora"></asp:Label></div>
                            <div class="col-sm-4">
                                 <div class="input-group date" id="dtCHoraReg">
                                    <asp:TextBox runat="server" ID="txtCHoraRecog" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                </div>                                   
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCCiudadDev" Text="Lugar devolucion"></asp:Label></div>
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="txtCLugarDevolucion" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCLugarDev" runat="server" Display="None" ControlToValidate="txtCLugarDevolucion" ValidationGroup="GuardarC" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>                            
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCDiaDev" Text="Dia"></asp:Label></div>
                            <div class="col-sm-4">
                                 <div class="input-group date" id="dtCDiaDev">
                                    <asp:TextBox runat="server" ID="txtCDiaDev" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCHoraDev" Text="Hora"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtCHoraDev">
                                    <asp:TextBox runat="server" ID="txtCHoraDev" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                </div>                                
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCCategoria" Text="Categoria vehiculo"></asp:Label></div>
                            <div class="col-sm-10"><asp:TextBox runat="server" ID="txtCCategoria" CssClass="form-control"></asp:TextBox></div>
                        </div>
                        <div class="form-group">                            
                            <table class="table table-striped table-condensed">
                                <tr>
                                    <th>&nbsp;</th>
                                    <th><asp:Label runat="server" ID="labelCCabObj" Text="Objetivo"></asp:Label></th>                                    
                                    <th><asp:Label runat="server" ID="labelCCabReal" Text="Real"></asp:Label></th>                                    
                                </tr>
                                <tr>
                                    <th><asp:Label runat="server" ID="labelCCab1DiaTarifa" Text="1 dia"></asp:Label></th>
                                    <td><asp:Label runat="server" ID="lblCTarifaDiaO"></asp:Label></td>
                                    <td>
                                        <asp:Textbox runat="server" ID="txtCTarifa1Dia" CssClass="form-control"></asp:Textbox>
                                        <ajax:FilteredTextBoxExtender ID="ftbeCTarifa1Dia" runat="server" TargetControlID="txtCTarifa1Dia" FilterType="Numbers,Custom" ValidChars=".," /> 
                                    </td>                                    
                                </tr>  
                            </table>
                        </div>
                        <div class="form-inline">
                            <asp:Button runat="server" ID="btnCGuardar" Text="Guardar" ValidationGroup="GuardarC" CssClass="form-control btn btn-primary" />
                            <asp:Button runat="server" ID="btnCCancelar" Text="Cancelar" ToolTip="Limpia las cajas de texto para introducir uno nuevo" CssClass="form-control btn btn-default" />
                        </div>  
                        <div class="form-group">
                            <asp:GridView runat="server" ID="gvCoches" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" ShowFooter="true">		                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkEdit" CommandName="Sel"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>      
                                        <asp:TemplateField HeaderText="Plan viaje">				           
				                        <ItemTemplate><asp:Label runat="server" ID="lblPlanViaje"></asp:Label></ItemTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad Origen">
				                        <ItemTemplate><asp:Label runat="server" ID="lblLugarRecogida"></asp:Label></ItemTemplate>
			                        </asp:TemplateField>                                 
                                        <asp:TemplateField HeaderText="Fecha">
				                        <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
                                            <FooterTemplate><asp:Label runat="server" ID="labelTotal" Text="Total (n dias)"></asp:Label></FooterTemplate>
			                        </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Tarifa Objetivo">
				                        <ItemTemplate><asp:Label runat="server" ID="lblTarifaObj"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="lblTotalObj"></asp:Label></FooterTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tarifa Real">
				                        <ItemTemplate><asp:Label runat="server" ID="lblTarifaReal"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="lblTotalReal"></asp:Label></FooterTemplate>
			                        </asp:TemplateField>                                   
                                    <asp:TemplateField HeaderText="Elim" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkDel" CommandName="Del"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>          	                       
		                        </Columns>        
	                        </asp:GridView>
                         </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel panel-primary" runat="server" id="divTrenes">
            <div class="panel-heading">
                <strong><asp:Label runat="server" ID="labelDivTrenes" Text="Trenes"></asp:Label></strong>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate> 
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelTCiudadOrigen" Text="Ciudad origen"></asp:Label></div>
                            <div class="col-sm-4"><asp:TextBox runat="server" ID="txtTCiudadOrigen" CssClass="form-control" /></div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelTCiudadDestino" Text="Ciudad destino"></asp:Label></div>
                            <div class="col-sm-4"><asp:TextBox runat="server" ID="txtTCiudadDestino" CssClass="form-control" /></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelTFecha" Text="Dia"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtTDia">
                                    <asp:TextBox runat="server" ID="txtTDia" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelTHora" Text="Hora"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtTHora">
                                    <asp:TextBox runat="server" ID="txtTHora" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                </div>                                 
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelTClase" Text="Clase"></asp:Label></div>
                            <div class="col-sm-10"><asp:TextBox runat="server" ID="txtTClase" CssClass="form-control"></asp:TextBox></div>
                        </div>
                         <div class="form-group">                            
                            <table class="table table-striped table-condensed">                                
                                <tr>
                                    <th>&nbsp;</th>
                                    <th><asp:Label runat="server" ID="labelTCabReal" Text="Real"></asp:Label></th> 
                                </tr>
                                <tr>
                                    <th><asp:Label runat="server" ID="labelTCab1PersoTarifa" Text="1 persona"></asp:Label></th>
                                    <td>
                                        <asp:Textbox runat="server" ID="txtTTarifa1Perso" CssClass="form-control"></asp:Textbox>
                                        <ajax:FilteredTextBoxExtender ID="ftbeTTarifa1Perso" runat="server" TargetControlID="txtTTarifa1Perso" FilterType="Numbers,Custom" ValidChars=".," /> 
                                    </td>                                    
                                </tr>  
                            </table>
                        </div>
                        <div class="form-inline">
                            <asp:Button runat="server" ID="btnTGuardar" Text="Guardar" ValidationGroup="GuardarT" CssClass="form-control btn btn-primary" />
                            <asp:Button runat="server" ID="btnTCancelar" Text="Cancelar" ToolTip="Limpia las cajas de texto para introducir uno nuevo" CssClass="form-control btn btn-default" />
                        </div>
                        <div class="form-group">
                            <asp:GridView runat="server" ID="gvTrenes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" ShowFooter="true">		                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkEdit" CommandName="Sel"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>      
                                        <asp:TemplateField HeaderText="Plan viaje">
				                        <ItemTemplate><asp:Label runat="server" ID="lblPlanViaje"></asp:Label></ItemTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad origen">
				                        <ItemTemplate><asp:Label runat="server" ID="lblCiudadOrigen"></asp:Label></ItemTemplate>
			                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Ciudad destino" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
				                        <ItemTemplate><asp:Label runat="server" ID="lblCiudadDestino"></asp:Label></ItemTemplate>
			                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha">
				                        <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>                                    
                                        <FooterTemplate><asp:Label runat="server" ID="labelTotal" Text="Total (n personas)"></asp:Label></FooterTemplate>
			                        </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Tarifa">
				                        <ItemTemplate><asp:Label runat="server" ID="lblTarifaReal"></asp:Label></ItemTemplate>
                                        <FooterTemplate><asp:Label runat="server" ID="lblTotalReal"></asp:Label></FooterTemplate>
			                        </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Elim" ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center">
				                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkDel" CommandName="Del"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton></ItemTemplate>
			                        </asp:TemplateField>          	                       
		                        </Columns>        
	                        </asp:GridView>
                        </div>                                                  
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>         
   </asp:Panel>     
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
                                <asp:Label runat="server" ID="labelModalEnvioMessage" Text="Si continua se guardaran los datos y se avisara al responsable del envio del presupuesto. ¿Desea continuar?"></asp:Label>                                
                            </div>                            
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-2"><asp:Button ID="btnModalEnviarPresup" runat="server" text="Enviar" CssClass="form-control btn btn-primary" /></div>
                            <div class="col-sm-2"><button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelModalCancelEnviarPresup" Text="Cancelar"></asp:Label></button></div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divModalTarifa" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalTarifa" Text="Buscador de tarifa"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-2"><asp:Label runat="server" ID="labelTarifDestino" Text="Destino"></asp:Label></div>
                                <div class="col-sm-10 input-group">
                                    <asp:Textbox runat="server" ID="txtTarifDestino" CssClass="form-control"></asp:Textbox>                                    
                                    <span class="input-group-btn">
                                       <button runat="server" id="btnSearchT" class="btn btn-primary" type="button"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></button>
                                    </span>
                                </div>
                            </div>                                
                            <div class="form-group">
                                <asp:GridView runat="server" ID="gvCiudadesTarifas" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">				           
                                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                    <PagerSettings PageButtonCount="5" />
				                    <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				                    <Columns>	
                                        <asp:TemplateField HeaderText="Destino">
                                            <ItemTemplate><asp:Label runat="server" ID="lblDestino" /></ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Nivel">
                                            <ItemTemplate><asp:Label runat="server" ID="lblNivel" /></ItemTemplate>
                                        </asp:TemplateField>
				                    </Columns>
			                    </asp:GridView>
                            </div>                                                                        
                        </div>                        
                        <asp:HiddenField runat="server" ID="hfTipo" />
                    </div>
                </div>                
            </div>                        
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" ID="PanelCargandoDatos" />
</asp:Content>
