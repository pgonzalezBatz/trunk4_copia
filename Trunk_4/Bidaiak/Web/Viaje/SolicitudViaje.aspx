<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="SolicitudViaje.aspx.vb" Inherits="WebRaiz.SolicitudViaje" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/Importes.ascx" TagName="Importes" TagPrefix="uc" %>
<%@ Register Src="~/Controles/CalendarioAnticipo.ascx" TagName="Calendar" TagPrefix="uc" %>
<%@ Register Assembly="OptionGroupDropDownList" Namespace="WebControlsDropDown" TagPrefix="ogd" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">		
    <script type="text/javascript" src="../Scripts/bootstrap-filestyle.min.js"></script>    
	<script type="text/javascript" language="javascript">	   
		//Valida que se haya elegido un tipo de viaje
		function ValidarTipoViaje(sender, args) {
			if (document.getElementById('<%=ddlTipoViaje.ClientId%>').value == "-1")
				args.IsValid = false;
			else
				args.isValid = true;
		}

		//Valida que se haya elegido una unidad organizativa
		function ValidarUnidadOrg(sender, args) {
			if (document.getElementById('<%=ddlUnidadOrg.ClientId%>').value == "-1")
				args.IsValid = false;
			else
				args.isValid = true;
        }

        //Deschequea el resto de radiobutton
        function CheckOtherIsCheckedByGVID(spanChk) {
            var CurrentRdbID = spanChk.id;
            Parent = document.getElementById('<%=gvPersonas.ClientID%>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") { 
                    if (items[i].checked) {
                        items[i].checked = false;
                    }
                }
            }
        }

        //Quita la seleccion del radiobutton
	    function QuitarCheck(spanChk) {
	        var CurrentRdbID = spanChk.id;
	        Parent = document.getElementById('<%=gvPersonas.ClientID%>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].type == "radio") {
                    items[i].checked = false;                    
                }
            }
	    }

	    //Al hacerse click, si es un servicio que requiere persona, se fuerza un postback
        function ServicioAgenciaCoche_Click(id_check,requierePersona) {            
            var check = document.getElementById(id_check.id);
            document.getElementById('<%=hfAgencia.ClientID%>').value = check.checked;
            if (requierePersona == 1) {
                var button = document.getElementById('<%=btnAgenciaCoche.ClientID%>');
                button.click();
            }
        }		
	</script>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong>A)&nbsp;<asp:Label runat="server" ID="labelDivCabDatosIni" Text="Datos Iniciales"></asp:Label></strong>
        </div>
        <div class="panel-body lines">
            <asp:Panel runat="server" ID="pnlIdViaje" CssClass="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelIdViaje" text="Id Viaje"></asp:Label></div>
                <div class="col-sm-4"><asp:Label ID="lblIdViaje" runat="server" CssClass="label label-info" style="font-size:18px;"></asp:Label></div>
                <div class="col-sm-2"><asp:Label runat="server" ID="labelEstadoViaje" text="Estado"></asp:Label></div>
                <div class="col-sm-4"><asp:Label ID="lblEstadoViaje" runat="server" CssClass="label" style="font-size:15px;text-transform:uppercase;"></asp:Label></div>                    
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlValidacion">
                <div class="form-group">
                    <asp:Label runat="server" ID="labelIndicarComen" text="En caso de rechazar, puede indicar unos comentarios"></asp:Label><br />
                    <asp:TextBox runat="server" ID="txtComentariosVal" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-inline">
                    <asp:Button runat="server" ID="btnValidar" Text="Validar" CssClass="btn btn-primary" />
                    <asp:Button runat="server" ID="btnRechazar" Text="Rechazar" CssClass="btn btn-danger" />
                </div>
            </asp:Panel>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelFechSol" text="Fech. Solicitud"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblFechaSolicitud" runat="server"></asp:Label></b></div>
                <div class="col-sm-2"><asp:Label runat="server" ID="labelProp" Text="Propietario"></asp:Label></div>
                <div class="col-sm-4">
                    <b><asp:Label ID="lblPropietario" runat="server"></asp:Label></b>
                    <asp:HiddenField runat="server" ID="hfPropietario" />
                </div>
            </div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-12 text-warning"><asp:Label runat="server" ID="labelInfoDestino" Text="Seleccione la ciudad de su destino. Si no la encuentra, seleccione 'Resto'" CssClass="text-uppercase"></asp:Label></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelDestino" text="Destino"></asp:Label></div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlTarifaDestino" CssClass="form-control required text-uppercase" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList>
                            <asp:TextBox runat="server" ID="txtDestino" CssClass="form-control required" MaxLength="50"></asp:TextBox>                            
                        </div>
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelTipViaje" Text="Tipo viaje"></asp:Label></div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlTipoViaje" CssClass="form-control required" AutoPostBack="true"></asp:DropDownList>
                            <asp:CustomValidator ID="cvTipoViaje" runat="server" EnableClientScript="true" ClientValidationFunction="ValidarTipoViaje" ControlToValidate="ddlTipoViaje" ValidationGroup="Guardar" Display="None" ErrorMessage="Seleccione un tipo de viaje"></asp:CustomValidator>	
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelFIda" text="Fecha ida"></asp:Label></div>
		                <div class="col-sm-4">
                            <div class="input-group date" id="dtFechaIda">
                                <asp:TextBox runat="server" ID="txtFechaIda" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                <span class="input-group-addon" runat="server" id="spanFIdaCal"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
		                </div>
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelFV" text="Fecha llegada"></asp:Label></div>
                        <div class="col-sm-4">
                            <div class="input-group date" id="dtFechaVuelta">
                                <asp:TextBox runat="server" ID="txtFechaVuelta" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                <span class="input-group-addon" runat="server" id="spanFVueltaCal"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                        </div>
                    </div>
		            <asp:Label runat="server" ID="labelInfoFecha" Text="Fecha de llegada al origen (Ej: Fecha de llegada al aeropuerto de Bilbao)" cssClass="help-block text-danger"></asp:Label>
                    <asp:Panel runat="server" ID="pnlTipoDesplaz">
                         <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="lblPais" Text="Pais"></asp:Label></div>
                            <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlPais" DataTextField="Nombre" DataValueField="Id" CssClass="form-control"></asp:DropDownList></div>
                            <div class="col-sm-2"><asp:Label runat="server" ID="lblTipoDesplaz" Text="Tipo de desplazamiento"></asp:Label></div>
                            <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlTipoDesplaz" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></div>
                        </div>
                        <asp:Panel runat="server" ID="pnlPlantasFiliales" CssClass="form-group">                            
                            <div class="checkboxlist col-sm-12">
                                <asp:Repeater runat="server" ID="rptCheckFiliales">
                                    <ItemTemplate>
                                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbFilial" /></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div><br /><br />
                            <asp:Panel runat="server" ID="pnlResulGerente" CssClass="form-group"></asp:Panel>
                        </asp:Panel>                        
                         <asp:Panel runat="server" ID="pnlCliente" CssClass="panel panel-info">                            
                            <div class="panel-heading">
                                <strong><asp:Label runat="server" ID="labelSubdivClient" Text="Documentos cliente"></asp:Label></strong>
                            </div>
                            <div class="panel-body lines">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="labelInfoCli" Text="Si algun asistente se le ha asignado una actividad exenta, tendra que subir algun documento relativo al cliente"></asp:Label><br /><br />                            
                                    </div>                                                         
                                <asp:Panel runat="server" ID="pnlInfoUploadDocCli">
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
                                </asp:Panel> 
                                <ul id="mylist" class="list-group">
                                    <asp:Repeater runat="server" ID="rptDocumentosCliente">
                                        <ItemTemplate>
                                            <li class="list-group-item">
                                                <asp:HyperLink ID="hkTitulo" runat="server" Target="_blank"></asp:HyperLink>
                                                <asp:LinkButton runat="server" ID="lnkEliminar" CssClass="pull-right"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton>
                                            </li>
                                        </ItemTemplate>                
                                    </asp:Repeater>
                                </ul>
                                <b><asp:Label runat="server" ID="lblSinRegistros" Visible="false" Text="No se ha subido ningun documento"></asp:Label></b>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <!--No se puede tener dentro de un update panel por el fileupload-->                    
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSubirDoc" />
                </Triggers>
            </asp:UpdatePanel> 
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upFechasViaje">
                <ContentTemplate>                      
                    <asp:Panel runat="server" ID="pnlDiasAntelacionOK" CssClass="alert alert-success">
                        <span class="glyphicon glyphicon-ok"></span>
                        <b><asp:Label runat="server" ID="lblTextoDiasAntelacionOk" Text="El viaje se ha planificado con la antelacion suficiente" style="font-size:15px;"></asp:Label></b>                            
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlDiasAntelacionNoOK" CssClass="alert alert-danger">
                        <asp:Image runat="server" ID="imgIconoDiasAntelacionNoOk" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/advertencia.gif" />        
                        <b><asp:Label runat="server" ID="labelTextoNoOkAnt1" Text="ATENCION: La planificacion del viaje se esta realizando fuera de plazo. Planificarlo con antelacion suficiente puede suponer un ahorro de hasta el 20%." style="text-decoration: underline;"></asp:Label>
                        <asp:Label runat="server" ID="labelTextoNoOkAnt2" Text="Objetivo para planificar y emitir billetes aereos" /></b>
                        <ul style="padding-left:30px;">
                            <li><b><asp:Label runat="server" ID="labelTextoNoOkAnt3" Text="NACIONAL + EUROPA > [DIAS] DIAS"></asp:Label></b></li>
                            <li><b><asp:Label runat="server" ID="labelTextoNoOkAnt4" Text="INTERCONTINENTAL > [DIAS] DIAS"></asp:Label></b></li>
                        </ul>
                    </asp:Panel>                   
                </ContentTemplate>
            </asp:UpdatePanel>
             <div class="form-group">
                <asp:Label runat="server" ID="labelDescr" Text="Descripcion (Justifique claramente la necesidad del viaje)"></asp:Label><br />
                <asp:TextBox runat="server" ID="txtDescripcionDatosIni" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revDescripcion" runat="server" ControlToValidate="txtDescripcionDatosIni" ValidationGroup="Guardar" Display="None" ErrorMessage="500 caracteres maximo" ValidationExpression="[\s\S]{0,500}"></asp:RegularExpressionValidator>
            </div>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong>B)&nbsp;<asp:Label runat="server" ID="labelDivCabInteg" Text="Integrantes"></asp:Label></strong>
        </div>
        <div class="panel-body lines"> 
            <div class="form-group"><asp:Label runat="server" ID="labelIntegrantes" Text="Introduzca las personas que van a viajar"></asp:Label></div>
             <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlAddIntegrante">
                        <uc:Busqueda ID="searchUser" runat="server" PostBack="true" SoloActivos="true" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" /><br /><br />
                    </asp:Panel>
	                <asp:GridView runat="server" ID="gvPersonas" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-condensed table-hover" GridLines="None">		                
		                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                <Columns>				
			                <asp:TemplateField Visible="false">
				                <ItemTemplate><asp:Label runat="server" ID="lblIdSab"></asp:Label></ItemTemplate>
			                </asp:TemplateField>
                            <asp:TemplateField>
				                <HeaderTemplate><asp:Label runat="server" Text="Liq"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Radiobutton runat="server" ID="rbtLiq" GroupName="Liquidador" onclick="javascript:CheckOtherIsCheckedByGVID(this);" onDblClick="javascript:QuitarCheck(this);"></asp:Radiobutton></ItemTemplate>
			                </asp:TemplateField>					
			                <asp:TemplateField ItemStyle-Wrap="false">
				                <HeaderTemplate><asp:Label runat="server" Text="Nombre"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
			                </asp:TemplateField>
                            <asp:TemplateField>
				                <HeaderTemplate><asp:Label runat="server" Text="actividad"></asp:Label></HeaderTemplate>
				                <ItemTemplate>
                                    <ogd:OptionGroupDropDownList runat="server" id="ogddlActiv" CssClass="form-control" ToolTip="seleccioneUno"></ogd:OptionGroupDropDownList>					                
                                    <asp:Panel runat="server" ID="pnlDesarraigo">
                                        <br /><div class="row">
                                            <div class="col-sm-2"><asp:Label runat="server" ID="labelPaP" Text="PaP"></asp:Label></div>
                                            <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlPaP" CssClass="form-control"></asp:DropDownList></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCondEsp" Text="Cond. especiales"></asp:Label></div>
                                            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlCondEsp" CssClass="form-control"></asp:DropDownList></div>
                                        </div>                                                                                                                        
                                    </asp:Panel>                                   
				                </ItemTemplate>
			                </asp:TemplateField>
                            <asp:TemplateField>
				                <HeaderTemplate><asp:Label runat="server" Text="Observacion"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:TextBox runat="server" ID="txtObservacion" MaxLength="100" CssClass="form-control"></asp:TextBox></ItemTemplate>
			                </asp:TemplateField>									
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <HeaderTemplate><asp:Label runat="server" text="Fecha ida"></asp:Label></HeaderTemplate>
                                <ItemTemplate>
                                    <div class="input-group date" id="dtFechaIntIda" runat="server">
                                        <asp:TextBox runat="server" ID="txtFIda" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                    </div>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <HeaderTemplate><asp:Label runat="server" Text="Fecha vuelta"></asp:Label></HeaderTemplate>
                                <ItemTemplate>
                                    <div class="input-group date" id="dtFechaIntVuelta" runat="server">
                                        <asp:TextBox runat="server" ID="txtFVuelta" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                    </div>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate><asp:Label runat="server" Text="Desarraigado"></asp:Label></HeaderTemplate>
                                <ItemTemplate><asp:CheckBox runat="server" id="chbDesarraigado" AutoPostBack="true" OnCheckedChanged="CambioDesarraigo" /></ItemTemplate> 
                            </asp:TemplateField>
			                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
				                <HeaderTemplate><asp:Label runat="server" Text="quitar"></asp:Label></HeaderTemplate>
				                <ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CssClass="form-control btn btn-danger" data-toggle="modal"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></asp:Linkbutton></ItemTemplate>
			                </asp:TemplateField>
		                </Columns>
	                </asp:GridView>
                    <asp:Panel runat="server" ID="pnlUserSinDpto" CssClass="alert alert-warning">
                        <br /><b><asp:Label runat="server" ID="lblSinDpto"></asp:Label></b>
                    </asp:Panel>                     
                     <asp:HiddenField runat="server" ID="hfIdFila" />
                     <asp:HiddenField runat="server" ID="hfPropagar" />
                     <asp:Button runat="server" ID="btnCambioFechasIda" style="display:none;" />
                     <asp:Button runat="server" ID="btnCambioFechasVuelta" style="display:none;" />
                </ContentTemplate>
             </asp:UpdatePanel>                                           
             <asp:HiddenField runat="server" ID="hfIdLiq" />
            <div class="form-group text-warning"><asp:Label runat="server" ID="label1" Text="Los subcontratados/autonomos ya no pueden ser añadidos como integrantes. En caso de que tengan que viajar, solicite el viaje y a continuacion contacte con Travel Air para indicarle que añada a estas personas al viaje creado" CssClass="text-uppercase"></asp:Label></div>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlModuloAgencia">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <strong>C)&nbsp;<asp:Label runat="server" ID="labelDivCabAgencia" Text="Datos agencia"></asp:Label></strong>
            </div>
            <div class="panel-body lines">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>  
                        <asp:Panel runat="server" ID="pnlAgenciaDisabled" CssClass="alert alert-warning">
                            <b><asp:Label runat="server" ID="labelInfoAgen" Text="En estos momentos, la solicitud de agencia no se puede modificar ya que el estado ha cambiado. Si quisiera realizar algún cambio, pongase en contacto con el responsable de la agencia"></asp:Label></b>
                        </asp:Panel>                        
                        <asp:Panel runat="server" ID="pnlEstadoAgencia" CssClass="form-inline">
                            <asp:Label runat="server" ID="labelEstado" text="estado"></asp:Label>
                            <b><asp:Label runat="server" ID="lblEstadoAgencia" style="font-size:15px"></asp:Label></b><br /><br />
                        </asp:Panel>
                        <div class="row">
                            <div class="checkboxlist col-sm-10">
                                <asp:Repeater runat="server" ID="rptCheckServAgen">
                                    <ItemTemplate>
                                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbServicio" /></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="pnlCocheAlquiler">
                            <div class="form-inline">
                                <asp:Label runat="server" ID="labelConductor" Text="Seleccione un conductor"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlConductores" CssClass="form-control forceInline"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelMensaGPS" CssClass="mensajeImportante" Text="Atencion: En caso de necesitar un GPS para el coche, pasad a recogerlo por informatica de BATZ" Visible="false"></asp:Label>
                            </div>
                        </asp:Panel>                        
                        <div class="form-group">
                            <asp:Label runat="server" ID="labelComen" Text="Comentarios (Especifique la preferencia de horarios de avión y la ciudad donde desea alojarse)"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtComentariosAgencia" TextMode="MultiLine" Rows="5" CssClass="form-control required"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revComenAgencia" runat="server" ControlToValidate="txtComentariosAgencia" ValidationGroup="Guardar" Display="None" ErrorMessage="350 caracteres maximo" ValidationExpression="[\s\S]{0,500}"></asp:RegularExpressionValidator>
                        </div>	                        	                        
                        <asp:Panel runat="server" ID="pnlComenAgencia" CssClass="form-group">
                            <asp:Label runat="server" ID="labelComenAgencia" Text="Comentarios de la agencia"></asp:Label><br />
                            <div class="well"><b><asp:Label runat="server" ID="lblComenAgencia"></asp:Label></b></div>
                        </asp:Panel>
                        <asp:Button runat="server" ID="btnAgenciaCoche" style="display:none;" />
                        <asp:HiddenField runat="server" ID="hfAgencia" />                        
                    </ContentTemplate>                    
                </asp:UpdatePanel>
            </div>
        </div>
    </asp:Panel>            	            
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong>D)&nbsp;<asp:Label runat="server" ID="labelDivCabProyecto" Text="Proyecto"></asp:Label></strong>
        </div>
        <div class="panel-body lines">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>  
                    <div class="form-inline">
                        <asp:Label runat="server" ID="labelUO" Text="Unidad organizativa"></asp:Label>
                        <asp:DropDownList runat="server" id="ddlUnidadOrg" AutoPostBack="true" CssClass="form-control required forceInline"></asp:DropDownList>
                        <asp:CustomValidator ID="cvUnidadOrg" runat="server" EnableClientScript="true" ClientValidationFunction="ValidarUnidadOrg" ControlToValidate="ddlUnidadOrg" ValidationGroup="Guardar" Display="None" ErrorMessage="Seleccione una unidad organizativa"></asp:CustomValidator>	
                    </div>	                
                    <asp:Panel runat="server" ID="pnlRequiereConSinProyecto" CssClass="form-group">
		                <asp:RadioButtonList runat="server" ID="rblConSinProyecto" AutoPostBack="true" CssClass="checkboxlist" RepeatDirection="Horizontal" RepeatLayout="Table">
                            <asp:ListItem Text="Con proyecto" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Sin proyecto" Value="1"></asp:ListItem>
                       </asp:RadioButtonList>
	                </asp:Panel>
	                <asp:Panel runat="server" ID="pnlRequiereProyCli">		                
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelCliente" Text="cliente"></asp:Label></div>
                            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlCliente" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"><asp:Label runat="server" ID="labelProyecto" Text="proyecto"></asp:Label></div>
                            <div class="col-sm-4"><asp:DropDownList runat="server" id="ddlProyecto" CssClass="form-control"></asp:DropDownList></div>
                            <div class="col-sm-3">
                                <asp:TextBox runat="server" ID="txtPorcentajeProyCli" Tooltip="Porcentaje para la reparticion de costos" MaxLength="3" CssClass="form-control text-center"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbPorcentajeProyCli" runat="server" TargetControlID="txtPorcentajeProyCli" FilterType="Numbers" />  
                            </div>
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnAddProyCli" Text="Añadir proyecto" CssClass="form-control btn btn-primary" /></div>                                                                                            
                        </div>		                
	                </asp:Panel>
	                <asp:Panel runat="server" ID="pnlOF" CssClass="form-inline">
                        <asp:Label runat="server" ID="labelOF" Text="OF"></asp:Label>
		                <asp:DropDownList runat="server" id="ddlOF" AppendDataBoundItems="true" CssClass="form-control forceInline"></asp:DropDownList>                        
	                </asp:Panel>
                    <asp:Panel runat="server" ID="pnlOFValidar">
                        <div class="row">                            
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="txtOF" ValidationGroup="InsertOF" CssClass="form-control text-uppercase"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOF" runat="server" Display="None" ControlToValidate="txtOF" ValidationGroup="InsertOF" ErrorMessage="Introduzca una of"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox runat="server" ID="txtPorcentajeOF" Tooltip="Porcentaje para la reparticion de costos" MaxLength="3" CssClass="form-control text-center"></asp:TextBox>                        
                                <ajax:FilteredTextBoxExtender ID="ftbPorcentajeOF" runat="server" TargetControlID="txtPorcentajeOF" FilterType="Numbers" />  
                            </div>
                            <div class="col-sm-3"><asp:Button runat="server" ID="btnAddOFTxt" Text="Añadir proyecto" ValidationGroup="InsertOF" CssClass="form-control btn btn-primary" /></div>
                        </div>                        		                                       
                        <asp:Panel runat="server" ID="pnlResulOFValidar">
                            <asp:Label runat="server" ID="lblOFValidacion"></asp:Label>
                        </asp:Panel>                        
	                </asp:Panel>
                    <asp:Panel runat="server" ID="pnlProyectos">
                        <div class="well">
                            <asp:Repeater runat="server" ID="rptProyectos">
			                    <ItemTemplate>
				                    <div runat="server" id="trServer" class="row">			
                                        <div class="col-sm-6">
                                            <asp:Label runat="server" ID="lblIdprograma"></asp:Label>
                                            <asp:Label runat="server" ID="lblNumOF"></asp:Label>
                                            <asp:Label ID="lblNombre" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-xs-10">
                                            <asp:TextBox runat="server" id="txtPorcentaje" MaxLength="3" CssClass="form-control text-center"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftbPorcentaje" runat="server" TargetControlID="txtPorcentaje" FilterType="Numbers" />  
                                        </div>
                                        <div class="col-sm-2 col-xs-2">                                            
                                            <asp:LinkButton runat="server" ID="lnkDel" CommandName="Del"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton>
                                        </div>                                        					                    					                                                            
				                    </div>				                    
			                    </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="alert alert-warning" runat="server" id="divSinProyectos">
                            <b><asp:Label runat="server" ID="labelSinProyectos" Text="No se ha añadido ningun proyecto"></asp:Label></b>
                        </div>                                                
                    </asp:Panel>
	            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
	<asp:Panel runat="server" ID="pnlModuloAnticipos">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <strong>E)&nbsp;<asp:Label runat="server" ID="labelDivCabAnticipos" Text="Anticipos"></asp:Label></strong>
            </div>
            <div class="panel-body lines">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>     
                        <asp:Panel runat="server" ID="pnlAnticiposDisabled" CssClass="alert alert-warning">
                            <b><asp:Label runat="server" ID="lblMensajeInfo"></asp:Label></b>
                        </asp:Panel>            
                        <asp:Panel runat="server" ID="pnlAnticipos">
                            <asp:Panel runat="server" ID="pnlAnticiposInfo" CssClass="alert alert-info">
                                <asp:Label runat="server" id="labelAnticInfo"></asp:Label>                                                   
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlEstadoAnticipos" CssClass="form-inline">
                                <asp:HiddenField runat="server" ID="hfIdEstAnti"></asp:HiddenField> 
                                <asp:Label runat="server" ID="labelEstAnti" text="estado"></asp:Label>
                                <b><asp:Label runat="server" ID="lblEstadoAnticipo" style="font-size:15px"></asp:Label></b><br />
                            </asp:Panel>
                            <div class="form-inline">                              		                                            		                
                                <asp:Label runat="server" ID="labelFechaNec" text="Fecha necesidad"></asp:Label>
                                <uc:Calendar runat="server" ID="calFechaNec" />
                            </div>                
                            <uc:Importes runat="server" id="selImportes" Modificable="true"></uc:Importes>                                                
                            <asp:Repeater runat="server" ID="rptTransfAnticipos">
                                <ItemTemplate>
                                    <div class="alert alert-warning">
                                        <b><asp:Label runat="server" ID="lblTransfAnticipo"></asp:Label></b>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:Panel>                        
                    </ContentTemplate>                    
                </asp:UpdatePanel>
            </div>
        </div>
    </asp:Panel>                        
	<div id="btn-group">
        <asp:Button runat="server" ID="btnGuardar" ValidationGroup="Guardar" CssClass="btn btn-primary" />        
        <asp:Button runat="server" ID="btnCancelar" Text="Cancelar viaje" CssClass="btn btn-danger" />
        <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="btn btn-default" />
	</div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
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
