<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="HojaGastos.aspx.vb" Inherits="WebRaiz.HojaGastos" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server"> 
    <script type="text/javascript" src="../Scripts/bootstrap-filestyle.min.js"></script>    
     <script type="text/javascript">
         //Comprueba si el elemento seleccionado, requiere texto adicional para los gastos con recibo       
         function ChequearSeleccionadoConRecibo() {
             var dropConRecibo = document.getElementById('<%=ddlConceptosConReciboMet.ClientID %>');
             var txtConceptoRecibo = document.getElementById('<%=txtConceptoConReciboMet.ClientID %>');
             var dropSinRecibo = document.getElementById('<%=ddlConceptosSinReciboMet.ClientID %>');
             var txtConceptoSinRecibo = document.getElementById('<%=txtConceptoSinReciboMet.ClientID %>');
             var reqDet = dropConRecibo.options[dropConRecibo.selectedIndex].value.split('|')[1];             
             if (reqDet == 'True')
                 txtConceptoRecibo.style.display = 'inline';                 
             else
                 txtConceptoRecibo.style.display = 'none';             
             txtConceptoSinRecibo.value = '';
             txtConceptoSinRecibo.style.display = 'none';
             dropSinRecibo.selectedIndex = 0;
         }   

         //Comprueba si el elemento seleccionado, requiere texto adicional para los gastos sin recibo       
         function ChequearSeleccionadoSinRecibo() {
             var dropConRecibo = document.getElementById('<%=ddlConceptosConReciboMet.ClientID %>');
             var txtConceptoRecibo = document.getElementById('<%=txtConceptoConReciboMet.ClientID %>');
             var dropSinRecibo = document.getElementById('<%=ddlConceptosSinReciboMet.ClientID %>');
             var txtConceptoSinRecibo = document.getElementById('<%=txtConceptoSinReciboMet.ClientID %>');
             var reqDet = dropSinRecibo.options[dropSinRecibo.selectedIndex].value.split('|')[1];             
             if (reqDet == 'True')
                 txtConceptoSinRecibo.style.display = 'inline';                 
             else
                 txtConceptoSinRecibo.style.display = 'none';             
             txtConceptoRecibo.value = '';
             txtConceptoRecibo.style.display = 'none';         
             dropConRecibo.selectedIndex = 0;
         }    
     </script>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivCabInfo" Text="Informacion" CssClass="text-uppercase"></asp:Label></strong>
        </div>
        <div class="panel-body lines">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>                
                    <asp:Panel runat="server" ID="pnlInfoViaje">
                        <div class="row">
                            <div class="col-sm-1"><asp:Label runat="server" ID="labelViaje" Text="Viaje"></asp:Label></div>
                            <div class="col-sm-6">
                                <b>
                                    <asp:Label runat="server" Id="lblDestino"></asp:Label>
                                    <asp:Label runat="server" ID="lblIdViaje" CssClass="label label-info" style="font-size:18px;"></asp:Label>
                                </b>
                            </div>
                            <div class="col-sm-5"><asp:LinkButton runat="server" ID="lnkVerEstados1" Text="Historico de estados"></asp:LinkButton></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-1"><asp:Label runat="server" ID="labelFechas" Text="Fechas"></asp:Label></div>
                            <div class="col-sm-11">
                                <b>
                                    <asp:Label runat="server" ID="lblFechaDesde"></asp:Label>-
                                    <asp:Label runat="server" ID="lblFechaHasta"></asp:Label>
                                </b>
                            </div>
                        </div>                                                                
                     </asp:Panel>
                    <asp:Panel runat="server" ID="pnlSinIdViaje">
                        <div class="row">
                            <div class="col-sm-1"><asp:Label runat="server" ID="label" Text="Hoja"></asp:Label></div>
                            <div class="col-sm-6"><b><asp:Label runat="server" Id="lblSinIdViaje" CssClass="label label-info" style="font-size:18px;"></asp:Label></b></div>
                            <div class="col-sm-5"><asp:LinkButton runat="server" ID="lnkVerEstados2" Text="Historico de estados"></asp:LinkButton></div>
                        </div>                                
                        <div class="row">
                            <div class="col-sm-1"><asp:Label runat="server" ID="labelFechas2" Text="Fechas"></asp:Label></div>
                            <div class="col-sm-11">
                                <b>
                                    <asp:Label runat="server" ID="lblFechasDesde2"></asp:Label>-
                                    <asp:Label runat="server" ID="lblFechasHasta2"></asp:Label>
                                </b>
                            </div>
                        </div>                                             
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlInfoUsuario" CssClass="row">
                        <div class="col-sm-1"><asp:Label runat="server" ID="labelUsuario" Text="Usuario"></asp:Label></div>
                        <div class="col-sm-11"><b><asp:Label ID="lblNombreUsuario" runat="server"></asp:Label></b></div>                
                    </asp:Panel>            
                    <asp:Panel runat="server" ID="pnlEstado">
                        <div runat="server" id="divEstado">
                            <b><asp:Label runat="server" ID="lblEstado"></asp:Label></b>
                        </div>                
                        <asp:Panel runat="server" ID="pnlCambioEstado" CssClass="form-inline">
                            <asp:Button runat="server" ID="btnValidarHoja" Text="Validar hoja" CommandName="V" CssClass="form-control btn btn-success" />
                            <asp:Button runat="server" ID="btnRechazarHoja" Text="Rechazar o desvalidar hoja" CommandName="R" CssClass="form-control btn btn-danger" />                    
                        </asp:Panel>       
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlMultiUser" CssClass="form-inline">
                        <asp:Label runat="server" ID="labelSelUsu" text="Seleccione el usuario del que quiera ver la hoja"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlUsuarios" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" style="text-transform:uppercase"></asp:DropDownList>
                     </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivCabMetalico" Text="Gastos en metalico" CssClass="text-uppercase"></asp:Label></strong>
        </div>
        <div class="panel-body lines">
             <asp:UpdatePanel runat="server" ID="upMetalico">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlFiltroMet">
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelFMet" text="Fecha"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtFechaMet">
                                    <asp:TextBox runat="server" ID="txtFechaMet" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <asp:RequiredFieldValidator ID="rfvFechaMet" runat="server" Display="None" ControlToValidate="txtFechaMet" ValidationGroup="M" ErrorMessage="Introduzca la fecha"></asp:RequiredFieldValidator>
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>                                                
                            </div>                            
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-warning"><asp:Label runat="server" ID="labelInfoRecibo" Text="Si tiene recibo, seleccione una opción de 'Concepto con recibo'. En caso contrario, de 'Concepto sin recibo'. Si aún así no encuentra un concepto donde encaje el gasto, contacte con administración"></asp:Label></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" id="labelConceptoConReciboMet" Text="Concepto con recibo"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlConceptosConReciboMet" runat="server" DataTextField="text" DataValueField="value" CssClass="form-control"></asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtConceptoConReciboMet" MaxLength="75" style="display:none" CssClass="form-control" ToolTip="Introduzca una descripcion del gasto"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConceptoConReciboMet" runat="server" Display="None" ControlToValidate="txtConceptoConReciboMet" ValidationGroup="M" ErrorMessage="Introduzca el dato" Enabled="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" id="labelConceptoSinReciboMet" Text="Concepto sin recibo"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlConceptosSinReciboMet" runat="server" DataTextField="text" DataValueField="value" CssClass="form-control"></asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtConceptoSinReciboMet" MaxLength="75" style="display:none" CssClass="form-control" ToolTip="Introduzca una descripcion del gasto"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConceptoSinReciboMet" runat="server" Display="None" ControlToValidate="txtConceptoSinReciboMet" ValidationGroup="M" ErrorMessage="Introduzca el dato" Enabled="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" id="labelImporteMet" Text="importe"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtImporteMet" CssClass="form-control text-center"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbImporteMet" runat="server" TargetControlID="txtImporteMet" FilterType="Numbers,Custom" ValidChars=".," /> 
		                        <asp:RequiredFieldValidator ID="rfvImporteMet" runat="server" Display="None" ControlToValidate="txtImporteMet" ValidationGroup="M" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rgvImporteMet" runat="server" Display="None" ControlToValidate="txtImporteMet" ValidationGroup="M" ErrorMessage="Introduzca un valor mayor que 0" Type="Double" CultureInvariantValues="true" MinimumValue="0.1" MaximumValue="100000000" ></asp:RangeValidator>                        
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelMonedaMet" Text="moneda"></asp:Label></div>
                            <div class="col-sm-4"><asp:Dropdownlist runat="server" ID="ddlMonedaMet" CssClass="form-control"></asp:Dropdownlist></div>
                        </div>
                        <div class="row">                            
                            <div class="col-sm-4 col-sm-offset-2"><asp:Button runat="server" ID="btnAddLineaMetalico" Text="Añadir linea" ValidationGroup="M" CommandName="M" CssClass="form-control btn btn-primary" /></div>
                        </div>       			        
                    </asp:Panel>
                    <asp:GridView runat="server" ID="gvGastosMet" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" ShowFooter="true">		        
		                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                <Columns>
                             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				                <HeaderTemplate><asp:Label runat="server" Text="fecha"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
			                </asp:TemplateField>      
                             <asp:TemplateField>
				                <HeaderTemplate><asp:Label runat="server" Text="Usuario"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblUsuario"></asp:Label></ItemTemplate>
			                </asp:TemplateField>
                            <asp:Templatefield>
                                <HeaderTemplate><asp:Label runat="server" Text="concepto"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblConcepto"></asp:Label></ItemTemplate>
                            </asp:Templatefield>            		           	                 
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs" FooterStyle-CssClass="hidden-xs">
				                <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblImporteMon"></asp:Label></ItemTemplate>                                
			                </asp:TemplateField>
                             <asp:TemplateField ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-CssClass="gridview-header-center">
				                <HeaderTemplate><asp:Label runat="server" Text="Importe (EUR)"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>
                                <FooterTemplate><b><asp:Label runat="server" ID="lblImporteTotalMet" style="font-size:15px"></asp:Label></b></FooterTemplate>
			                </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs" FooterStyle-CssClass="hidden-xs">
				                <HeaderTemplate><asp:Label runat="server" Text="Recibo"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Checkbox runat="server" ID="chRecibo" Enabled="false"></asp:Checkbox></ItemTemplate>
			                </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs">
				                <HeaderTemplate><asp:Label runat="server" Text="Cambio"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblCambioMoneda"></asp:Label></ItemTemplate>                                
			                </asp:TemplateField>
			                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
				                <ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CssClass="form-control btn btn-danger" data-toggle="modal" CommandName="M"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></asp:Linkbutton></ItemTemplate>
			                </asp:TemplateField>           
		                </Columns>        
	                </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>     
    <asp:Panel runat="server" ID="pnlDivVisas">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <strong><asp:Label runat="server" ID="labelDivCabVisa" Text="Gastos de visa" CssClass="text-uppercase"></asp:Label></strong>
            </div>
            <div class="panel-body lines">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlDescripVisas">
                            <div class="form-group">
                                <b><asp:Label runat="server" ID="lblDescripVisas"></asp:Label></b>
                            </div>
                            <div runat="server" id="divGastosVisaCarga">
                                <asp:Label runat="server" ID="labelMensaVisa"></asp:Label>
                            </div>                            
                            <div class="alert alert-info" runat="server" id="divSoloGastosVisa">
                                <asp:Label runat="server" ID="labelMensaVisa2" Text="Si solo ha realizado gastos con visa, espere a que el mes que viene se carguen los gastos de visa. En ese momento podra enviar la hoja a su validador y una vez validada, imprimirla y adjuntar los tickets"></asp:Label>
                            </div>
                            <div class="alert alert-warning" runat="server" id="divGastosVisaSinCom">
                                <asp:Label runat="server" ID="labelMensaVisa3" Text="Existen gastos de visa sin comentar"></asp:Label>
                            </div>
                            <div class="alert alert-warning" runat="server" id="divGastosVisaDespues">
                                <asp:Label runat="server" ID="labelMensaVisa4" Text="Algunos gastos de visa han llegado despues de que la hoja de gastos se validara"></asp:Label>
                            </div>                            
                        </asp:Panel>
                        <asp:GridView runat="server" ID="gvGastosV" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" ShowFooter="true">		                    		                    
		                    <Columns>
                               <asp:TemplateField ItemStyle-HorizontalAlign="Center">
				                    <ItemTemplate><asp:Image runat="server" ID="imgWarning" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/warning16.png"/></ItemTemplate>
			                    </asp:TemplateField> 
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				                    <HeaderTemplate><asp:Label runat="server" Text="fecha"></asp:Label></HeaderTemplate>
				                    <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
			                    </asp:TemplateField>      
                                 <asp:TemplateField>
				                    <HeaderTemplate><asp:Label runat="server" Text="Usuario"></asp:Label></HeaderTemplate>
				                    <ItemTemplate><asp:Label runat="server" ID="lblUsuario"></asp:Label></ItemTemplate>
			                    </asp:TemplateField> 
                                <asp:BoundField DataField="Sector" HeaderText="Sector" />
                                <asp:TemplateField>
				                    <HeaderTemplate><asp:Label runat="server" Text="Comentario del usuario"></asp:Label></HeaderTemplate>
				                    <ItemTemplate>
					                    <asp:Label runat="server" ID="lblComentario"></asp:Label>
                                        <asp:Image runat="server" ID="imgMasComentarios" style="cursor:pointer;margin-left:10px;" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/info16.png" />
				                    </ItemTemplate>
			                    </asp:TemplateField> 
                                <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" FooterStyle-CssClass="hidden-xs" />
                                <asp:TemplateField  ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs" FooterStyle-CssClass="hidden-xs">
				                    <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
				                    <ItemTemplate><asp:Label runat="server" ID="lblImporteMonedaGasto"></asp:Label></ItemTemplate> 
			                    </asp:TemplateField>
                                 <asp:TemplateField  ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-CssClass="gridview-header-center">
				                    <HeaderTemplate><asp:Label runat="server" Text="Importe (EUR)"></asp:Label></HeaderTemplate>
				                    <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>
                                    <FooterTemplate><b><asp:Label runat="server" ID="lblImporteTotalVisa" style="font-size:15px"></asp:Label></b></FooterTemplate>
			                    </asp:TemplateField>                                
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				                    <ItemTemplate>
					                    <asp:ImageButton runat="server" ID="imgEstado" ImageAlign="AbsMiddle" CommandName="Justificar"/>
                                        <asp:LinkButton runat="server" ID="lnkJustificar" Text="Comentar gasto" ToolTip="Los movimientos de visa deben ser justificados (escribir un pequeño comentario explicativo)" CommandName="Justificar" style="margin-left:10px" Visible="false" CssClass="hidden-xs"></asp:LinkButton>                                
				                    </ItemTemplate>
			                    </asp:TemplateField>      
		                    </Columns>
	                    </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>                                   
    </asp:Panel>     
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivCabKilometraje" Text="Kilometraje" CssClass="text-uppercase"></asp:Label></strong>
        </div>
        <div class="panel-body lines">
            <asp:UpdatePanel runat="server" ID="upKilometraje">
                <ContentTemplate>   
                   <asp:Panel runat="server" ID="pnlFiltroKm">
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelFechaKm" text="fecha"></asp:Label></div>
                            <div class="col-sm-4">
                                <div class="input-group date" id="dtFechaKm">
                                    <asp:TextBox runat="server" ID="txtFechaKm" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                    <asp:RequiredFieldValidator ID="rfvFechaKm" runat="server" Display="None" ControlToValidate="txtFechaKm" ValidationGroup="KM" ErrorMessage="Introduzca la fecha"></asp:RequiredFieldValidator>
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelKm" Text="Kilometros"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtKm" CssClass="form-control text-center"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbKm" runat="server" TargetControlID="txtKm" FilterType="Numbers,Custom" ValidChars=".," /> 
                                <asp:RequiredFieldValidator ID="rfvKm" runat="server" Display="None" ControlToValidate="txtKm" ValidationGroup="KM" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>		
                                <asp:RangeValidator ID="rgvKm" runat="server" Display="None" ControlToValidate="txtKm" ValidationGroup="KM" ErrorMessage="Introduzca un valor mayor que 0" Type="Double" MinimumValue="1" MaximumValue="100000"></asp:RangeValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelOrigen" Text="origen"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtOrigenKm" MaxLength="75" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOrigenKm" runat="server" Display="None" ControlToValidate="txtOrigenKm" ValidationGroup="Km" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelDestino" Text="Destino"></asp:Label></div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtDestinoKm" MaxLength="75" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDestinoKm" runat="server" Display="None" ControlToValidate="txtDestinoKm" ValidationGroup="Km" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            </div>                        
                        </div>
                        <div class="row">
                            <div class="col-sm-4 col-sm-offset-2"><asp:Button runat="server" ID="btnAddLineaKm" Text="Añadir linea" ValidationGroup="KM" CommandName="KM" CssClass="form-control btn btn-primary" /></div>
                        </div>                    
                        <asp:Panel runat="server" ID="pnlTrayectos">
                            <div class="panel-group" id="divAccordion">
                                <div class="panel panel-primary">                    
                                    <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                                        <h4 class="panel-title">                            
                                            <span class="glyphicon glyphicon glyphicon-filter"></span>
				                            <asp:Label runat="server" id="labelTitleKm" Text="Trayectos de otras hojas de gastos"></asp:Label>                            
			                            </h4>                              
                                    </div>
                                    <div class="panel-collapse collapse" id="divCollapse">                           
                                        <div class="panel-body lines" style="padding-bottom:0px;">
                                            <div class="form-group">                        
                                                <asp:Label runat="server" ID="labelSelFechaTray" Text="Seleccione primero la fecha y despues en el trayecto deseado para insertar la linea"></asp:Label>                                     
                                            </div>
                                            <div style="overflow: auto; height: 150px;">                                                
                                                <asp:GridView runat="server" ID="gvTrayectosKM" AutoGenerateColumns="false" AllowSorting="false" CssClass="table table-striped table-condensed table-hover" GridLines="None">		                                        
		                                            <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                                            <Columns>
                                                        <asp:BoundField DataField="LugarOrigen" HeaderText="Origen"/>			
                                                        <asp:BoundField DataField="LugarDestino" HeaderText="Destino"/>
                                                        <asp:BoundField DataField="Kilometros" HeaderText="Km" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center"/>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>                                                                                
                                        </div>                
                                    </div>            
                                </div>
                            </div>              
                        </asp:Panel>
                    </asp:Panel>
                    <asp:GridView runat="server" ID="gvGastosKM" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" ShowFooter="true">		                
		                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				                <HeaderTemplate><asp:Label runat="server" Text="fecha"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
			                </asp:TemplateField>      
                             <asp:TemplateField>
				                <HeaderTemplate><asp:Label runat="server" Text="Usuario"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblUsuario"></asp:Label></ItemTemplate>
			                </asp:TemplateField>
                            <asp:BoundField DataField="LugarOrigen" HeaderText="Origen"/>			
                            <asp:BoundField DataField="LugarDestino" HeaderText="Destino"/>
                            <asp:BoundField DataField="Kilometros" HeaderText="Km" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center" />
			                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs" FooterStyle-CssClass="hidden-xs">
				                <HeaderTemplate><asp:Label runat="server" Text="Importe Km"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblImporteKM"></asp:Label></ItemTemplate>
			                </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-CssClass="gridview-header-center">
				                <HeaderTemplate><asp:Label runat="server" Text="Importe (EUR)"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblImporteEUR"></asp:Label></ItemTemplate>
                                <FooterTemplate><b><asp:Label runat="server" ID="lblImporteTotalKM" style="font-size:15px"></asp:Label></b></FooterTemplate>
			                </asp:TemplateField>  
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs" FooterStyle-CssClass="hidden-xs">
				                <ItemTemplate><asp:ImageButton runat="server" ID="imgInvertir" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/Invert.png" OnClick="InvertirLinea"/></ItemTemplate>
			                </asp:TemplateField>         	 						             
			                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center" ItemStyle-Width="1%">
				                <ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CssClass="form-control btn btn-danger" data-toggle="modal" CommandName="KM"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></asp:Linkbutton></ItemTemplate>
			                </asp:TemplateField>
		                </Columns>
	                </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
    <asp:Panel runat="server" ID="pnlDivDocumentos">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <strong><asp:Label runat="server" ID="labelDivCabDocumentacion" Text="Documentacion" CssClass="text-uppercase"></asp:Label></strong>
            </div>
            <div class="panel-body lines">
                <div class="form-group">
                    <asp:Label runat="server" ID="labelDocInfo1" Text="Como la actividad a realizar en el viaje es exenta, tiene que adjuntar toda la documentacion que acredite tal exencion(tarjetas de embarque, facturas de hotel, etc...)"></asp:Label>
                </div>
                 <asp:Panel runat="server" ID="pnlInfoUploadDoc">
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelDocTitulo" Text="Titulo descriptivo del documento"></asp:Label></div>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtDocTitulo" MaxLength="50" CssClass="form-control required"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDocTit" runat="server" Display="None" ControlToValidate="txtDocTitulo" ValidationGroup="AddDoc" ErrorMessage="Introduzca el texto"></asp:RequiredFieldValidator>
                        </div>                                    
                    </div>
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelDocAdj" Text="Documento a adjuntar"></asp:Label></div>
                        <div class="col-sm-8"><asp:FileUpload runat="server" ID="fuDocumento" CssClass="filestyle" /></div>
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnSubirDoc" Text="Subir documento" CssClass="form-control btn btn-primary" ValidationGroup="AddDoc" /></div>
                    </div>  
                     <asp:UpdatePanel runat="server">
                         <ContentTemplate>
                             <ul id="mylist" class="list-group">
                                <asp:Repeater runat="server" ID="rptDocumentos">
                                    <ItemTemplate>
                                        <li class="list-group-item">
                                            <asp:HyperLink ID="hkTitulo" runat="server" Target="_blank"></asp:HyperLink>
                                            <asp:LinkButton runat="server" ID="lnkEliminar" CssClass="pull-right"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton>
                                        </li>
                                    </ItemTemplate>                
                                </asp:Repeater>
                            </ul>
                            <div class="alert alert-warning" runat="server" id="divDocsNoSubidos">
                                <b><asp:Label runat="server" ID="lblSinRegistros" Text="No se ha subido ningun documento"></asp:Label></b>
                            </div>
                         </ContentTemplate>
                     </asp:UpdatePanel>                    
                </asp:Panel> 
            </div>
        </div>      
    </asp:Panel>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivTotal" Text="Total" CssClass="text-uppercase"></asp:Label></strong>
        </div>
        <div class="panel-body lines">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>                
                    <div class="form-group">
                        <b><asp:Label runat="server" ID="lblTotalUsuario" cssClass="text-primary" style="font-size:16px;"></asp:Label></b><br />
                        <b><asp:Label runat="server" ID="lblResumenUser" style="font-size:16px;"></asp:Label></b>
                    </div>
                    <asp:Panel runat="server" ID="pnlTotalTodos">
                        <div runat="server" id="divDiferencia"><b><asp:Label runat="server" ID="lblDiferenciaTexto"></asp:Label></b></div><br />
                        <div class="row">
                            <div class="col-sm-6">                    
                                <div class="well">
                                    <div class="row">
                                        <div class="col-xs-8"><b><asp:Label runat="server" Text="Anticipo recibido" ID="lblLabelAntRec"></asp:Label></b></div>
                                        <div class="col-xs-4 text-right"><asp:Label runat="server" id="lblAnticipo" cssClass="text-primary" style="font-size:15px;"></asp:Label></div>
                                    </div>
                                    <div class="row" runat="server" id="divAntEntregado">
                                        <div class="col-xs-8"><b><asp:Label runat="server" Text="Anticipo devuelto" ID="labelAntDevuelto"></asp:Label></b></div>
                                        <div class="col-xs-4 text-right"><asp:Label runat="server" id="lblAntDevuelto" CssClass="text-primary" style="font-size:16px;"></asp:Label></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-8"><b><asp:Label runat="server" ID="labelTotGastosMet" Text="Gastos en metalico"></asp:Label></b></div>
                                        <div class="col-xs-4 text-right"><asp:Label runat="server" id="lblTotalGastos" CssClass="text-primary" style="font-size:16px"></asp:Label></div>
                                    </div>
                                    <asp:Repeater runat="server" ID="rptTransf">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-xs-8"><asp:LinkButton runat="server" ID="lnkTransfEdit"></asp:LinkButton></div>
                                                <div class="col-xs-4 text-right"><asp:Label runat="server" id="lblTransf" cssClass="text-primary" style="font-size:16px;"></asp:Label></div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div class="row" runat="server" id="divMovAjuste">
                                        <div class="col-xs-8"><b><asp:Label runat="server" Text="Movimientos de ajuste" ID="labelMovAjuste"></asp:Label></b></div>
                                        <div class="col-xs-4 text-right"><asp:Label runat="server" id="lblMovAjuste" CssClass="text-primary" style="font-size:16px;"></asp:Label></div>
                                    </div>
                                    <div class="row" runat="server" id="divImpAbonado">
                                        <div class="col-xs-8"><b><asp:Label runat="server" Text="Importe abonado" ID="labelImpAbonado"></asp:Label></b></div>
                                        <div class="col-xs-4 text-right"><asp:Label runat="server" id="lblImpAbonado" cssClass="text-primary" style="font-size:16px;"></asp:Label></div>
                                    </div><br />
                                    <div class="row">
                                        <div class="col-xs-8"><b><asp:Label runat="server" ID="labelDiferencia" Text="Diferencia"></asp:Label></b></div>
                                        <div class="col-xs-4 text-right"><b><asp:Label runat="server" id="lblDiferencia" style="font-size:16px;"></asp:Label></b></div>
                                    </div>
                                </div>                               
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>            
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <div class="form-inline">         
                <asp:Button runat="server" id="btnEnviar" Text="Enviar" ToolTip="Enviar hoja al validador" CssClass="form-control btn btn-success" />
                <asp:Button runat="server" id="btnImprimir" Text="Imprimir" CssClass="form-control btn btn-info" />
                <asp:Button runat="server" ID="btnTransferir" Text="Transferir anticipo" CssClass="form-control btn-warning" />
                <asp:Button runat="server" ID="btnVerAnticipo" Text="Ver anticipo" CssClass="form-control btn-primary" />
                <asp:Button runat="server" id="btnEliminar" Text="Eliminar" CssClass="form-control btn btn-danger" />
                <asp:Button runat="server" id="btnVolver" Text="Volver" CssClass="form-control btn btn-default" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEnviar" />
            <asp:PostBackTrigger ControlID="btnImprimir" />
            <asp:PostBackTrigger ControlID="btnTransferir" />
            <asp:PostBackTrigger ControlID="btnVerAnticipo" />
            <asp:PostBackTrigger ControlID="btnEliminar" />
            <asp:PostBackTrigger ControlID="btnVolver" />            
        </Triggers>
    </asp:UpdatePanel>
    <div class="well">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>            
                <b>
                    <asp:Label runat="server" ID="labelInfo3" CssClass="text-danger" Text="EL ICONO DE ENVIAR PUEDE NO APARECER POR TRES RAZONES:" /><br />
                    <asp:Label runat="server" ID="labelInfo31" CssClass="text-danger" Text="1)No ha llegado el ultimo dia del mes (solo se puede enviar una por mes)" /><br />
                    <asp:Label runat="server" ID="labelInfo32" CssClass="text-danger" text="2)Tienes una visa asignada y todavia no han cargado el fichero de visas (debes esperar a que lo carguen aunque no hayas realizado ningun gasto con la tarjeta)" /><br />
                    <asp:Label runat="server" id="labelInfo33" CssClass="text-danger" Text="3)Todavia no has añadido ningun gasto"></asp:Label><br />
                </b>
                <asp:Label runat="server" ID="labelInfo1" CssClass="help-block" Text="1) Las lineas de gastos se guardan automaticamente al añadir la linea"></asp:Label>    
                <asp:Label runat="server" ID="labelInfo2" CssClass="help-block" Text="2) Enviar la hoja al validador cuando ya se hayan introducido todas las lineas. Una vez enviado, no se podran realizar mas cambios"></asp:Label>    
                <asp:Label runat="server" ID="labelInfo4" CssClass="help-block" Text="3) El boton para transferencias solo se visualiza en las hojas de viajes con anticipos en los que se haya metido algun gasto y que todavia no haya sido enviada a validar"></asp:Label>    
                <asp:Label runat="server" ID="lblComentario" CssClass="help-block"></asp:Label>       
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfEuroId" />            
            <asp:HiddenField runat="server" ID="hfLastHGStateDate" />
            <asp:HiddenField runat="server" ID="hfGastosVSinJustif" />                        
        </ContentTemplate>
    </asp:UpdatePanel>    
    <asp:UpdatePanel runat="server">
        <ContentTemplate> 
            <div id="divModalEstados" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalEstados" Text="Historico de estados"></asp:Label></h4>                            
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelInfoEstados" Text="Se muestran las fechas en las que ha habido un cambio de estado de la hoja"></asp:Label>
                            </div>
                            <asp:GridView runat="server" ID="gvEstados" AutoGenerateColumns="false" CssClass="table table-striped table-condensed table-hover" GridLines="None">				                				            
				                <Columns>				
					                <asp:TemplateField>
						                <HeaderTemplate><asp:Label ID="labelFecha" runat="server" Text="fecha"></asp:Label></HeaderTemplate>
						                <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
					                </asp:TemplateField>								
					                <asp:TemplateField>
						                <HeaderTemplate><asp:Label ID="labelEstado" runat="server" Text="estado"></asp:Label></HeaderTemplate>
						                <ItemTemplate><asp:Label runat="server" ID="lblEstado"></asp:Label></ItemTemplate>
					                </asp:TemplateField>									
				                </Columns>
			                </asp:GridView>                            
                        </div>
                        <div class="modal-footer">                            
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModalEstados" Text="Cancelar"></asp:Label></button>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divModalJustificar" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModalJustificar" Text="Justificacion del gasto"></asp:Label></h4>                            
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelInfoJust" Text="Introduzca una pequeña descripcion del gasto"></asp:Label>
                                <asp:TextBox runat="server" ID="txtJustificacion" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revJustifi" runat="server" ControlToValidate="txtJustificacion" ValidationGroup="GuardarJustif" Display="None" ErrorMessage="300 caracteres maximo" ValidationExpression="[\s\S]{0,300}"></asp:RegularExpressionValidator>                                
                            </div>
                        </div>
                         <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSaveModalJust" Text="Guardar" ValidationGroup="GuardarJustif" CssClass="btn btn-primary" /> 
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModalJusfit" Text="Cancelar"></asp:Label></button>
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
                            <asp:Label runat="server" ID="labelMessageModal"></asp:Label>                    
                            <asp:HiddenField runat="server" ID="hfModalParam" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnAceptarModal" Text="Aceptar" cssclass="btn btn-primary" /> 
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModal" Text="Cancelar"></asp:Label></button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>   
    <uc:CargandoDatos runat="server" />
</asp:Content>