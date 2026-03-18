<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReferenciaVentaBrain.aspx.vb" MasterPageFile="~/RefSis.Master" Inherits="ReferenciasVentas.ReferenciaVentaBrain" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>
<%@ Register Src="~/Controles/SelectorTipoProducto.ascx" TagName="TipoProducto" TagPrefix="TipoProducto" %>
<%@ Register Src="~/Controles/SelectorGrupoProducto.ascx" TagName="GrupoProducto" TagPrefix="GrupoProducto" %>
<%@ Register Src="~/Controles/SelectorGrupoMaterial.ascx" TagName="GrupoMaterial" TagPrefix="GrupoMaterial" %>
<%@ Register Src="~/Controles/SelectorTipoPieza.ascx" TagName="TipoPieza" TagPrefix="TipoPieza" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
   
    </script>
</asp:Content>

<asp:Content ID="Contenido_PRINCIPAL" ContentPlaceHolderID="cuerpoPrincipal" runat="server">    
   <%-- <script src="../../js/jquery-1.11.1.js"></script>--%>

    <script type="text/javascript">
        Sys.Application.add_load(init);
        function init() {
            $('textarea').keypress(function (e) {
                var maxlength = $(this).attr('longMax');
                if ($(this).val().length >= maxlength) {
                    e.preventDefault();                    
                }
            });

            $('textarea').bind('paste', function () {
                var self = this
                setTimeout(function (){ 
                    var maxlength = $(self).attr('longMax');
                    if ($(self).val().length >= maxlength) {
                        $(self).val($(self).val().substr(0, maxlength));
                    }
                }, 0);
            });

            $('#' + '<%= btnGuardarBrain.ClientID%>').bind('click', function () {
                // Para que esto funcione tanto el validador como el extender deben tener 
                //el mismo nombre excepto por el prefijo de tres letras (rfv -> requiered field validator, rev -> regular expression validator, vce -> validator callout extender)
                for (i = 0; i < Page_Validators.length; i++) {
                    // Este es el ID del required field validator
                    var validatorID = Page_Validators[i].id;

                    // Con este if cogemos sólo los validadores dentro del las tabs
                    if (validatorID.indexOf('tab') != -1) {
                        // Este es el prefijo del validador. Puede ser rfv, rev
                        var prefix = validatorID.substr(validatorID.lastIndexOf("_") + 1, 3)
                        // Este es el ID del validator callout extender
                        var vceID = validatorID.replace(prefix, "vce");
                        // Este es el control validator callout extender
                        var vceControl = $find(vceID);
                        // Este es el ID de la Tab contenedora
                        var tabID = vceID.substring(vceID.indexOf('tab'), vceID.indexOf('_', vceID.indexOf('tab')));
                        // Del control tab sacamos su indice dentro del tabContainer. Seleccionamos el div con ese estilo y que contenga cierta cadena
                        var indexTab = $find($("div.ajax__tab_panel[id$='" + tabID + "']")[0].id)._tabIndex;

                        if (vceControl && vceControl._invalid) {
                            var tabContainer = $find('<%=tcPlantas.ClientID %>');
                            tabContainer.set_activeTab(tabContainer.get_tabs()[indexTab]);
                            Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout = vceControl;
                            Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout.show(true);
                            return;
                        }
                    }
                }
            });
        };
    </script>

    <asp:UpdatePanel ID="upGlobal" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlNuevaSolicitud" runat="server" CssClass="nuevaSolicitud">
                <table style="width: 100%; border:1px solid #5D7B9D; margin-bottom: 10px; text-align:center" class="tablaNuevaSolicitud">
                    <tr style="background-color: #AAF6C3">
                        <td>
                            <asp:Label ID="lblReferenciaBrain" runat="server" Text="Selling Part Number in Brain" style="text-transform:uppercase" ForeColor="Black" Font-Bold="true" />
                        </td>
                    </tr>
                </table>

                <asp:FormView ID="fvDatosReferencia" runat="server" Width="100%" OnDataBound="fvDatosReferencia_DataBound" DefaultMode="ReadOnly" DataKeyNames="Id" CellPadding="5">                    
                    <ItemTemplate>
                        <tr style="background-color: #F9E1D2; text-align:center">
                            <td colspan="8">
                                <asp:Label ID="lblDatosBasicosReferencia" runat="server" Text="Information entered by the user" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr style="background-color: #91A0A7; text-align:center; border-top:1px dotted #91A0A7">
                            <td colspan="6" style="background-color: #91A0A7; text-align:center; border-top:1px dotted #91A0A7">
                                <asp:Label ID="lblTitCustomerPartNumber" Text="Part Number" runat="server" ForeColor="white" />
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblPlantToCharge" Text="Plants to charge" runat="server" ForeColor="white" />
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblTitType" Text="Part Type" runat="server" />
                            </td>
                            <td style="width:10%; text-align:left">
                                <asp:Label ID="txtTipoReferencia" runat="server" Text='<%# Eval("TipoReferenciaNombre")%>' />
                            </td>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblPartNumber" Text="Part No." runat="server" />
                            </td>      
                            <td style="width:10%; text-align:left">
                                <asp:Label ID="txtCustomerPN" runat="server" Width="99%" Text='<%# Eval("CustomerPartNumber")%>' />                                                     
                            </td>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblDrawingNumber" Text="Drawing No." runat="server" />
                            </td>      
                            <td  style="width:10%; text-align:left;border-right:1px solid #91A0A7">
                                <asp:Label ID="txtDrawingNumber" runat="server" Width="99%" Text='<%# Eval("DrawingNumber")%>' />                                                                              
                            </td> 
                            <td rowspan="2" style="width:40%; vertical-align:middle" colspan="2">
                                <asp:CheckBoxList ID="chkPlantToCharge" runat="server" DataTextField="Nombre" DataValueField="Codigo" Enabled="false" RepeatDirection="Horizontal" Width="100%" RepeatColumns="3" />                                         
                            </td>                                                       
                        </tr>
                        <tr>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblNumberType" Text="No. Type" runat="server" />
                            </td>
                            <td style="width:10%; text-align:left">
                                <asp:Label ID="txtNumberType" runat="server" Text='<%# Eval("TipoNumeroNombre")%>' />
                            </td>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblPlanoWeb" Text="Drawing No." runat="server" />
                            </td>      
                            <td style="width:10%; text-align:left">
                                <asp:Label ID="txtPlanoWeb" runat="server" Width="99%" Text='<%# Eval("PlanoWeb")%>' />                                                     
                            </td>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblNivelIngenieria" Text="Engineering Level" runat="server" />
                            </td>      
                            <td style="width:10%; text-align:left; border-right:1px solid #91A0A7">
                                <asp:Label ID="txtNivelIngenieria" runat="server" Width="99%" Text='<%# Eval("NivelIngenieria")%>' />                                                                              
                            </td> 
                        </tr>
                        <tr id="filaTipoEvolucion" runat="server">
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblPreviousBatzPartNumber" Text="Previous Batz Part Number" runat="server" />
                            </td>
                            <td style="width: 10%">
                                <asp:Label ID="txtPrevBatzPN" runat="server" Text='<%# Eval("PreviousBatzPartNumber")%>' />
                             </td>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblEvolutionChanges" Text="Evolution Changes" runat="server" />
                            </td>
                            <td  style="width: 20%">
                                <asp:Label ID="txtEvolutionChanges" runat="server" Width="100%" Text='<%# Eval("EvolutionChanges")%>' />                            
                            </td>                 
                            <td colspan="2" style="border-right:1px solid #91A0A7">&nbsp</td>
                            <td colspan="4"></td>
                        </tr>
                        <tr style="background-color: #91A0A7; border-top:1px dotted #5D7B9D; text-align:center">
                            <td colspan="8">
                                <asp:Label ID="lblTitPartName" Text="Part Name" runat="server" ForeColor="white" />
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblProduct" Text="Product" runat="server" />
                            </td>
                            <td style="width:10%">
                                <asp:Label ID="txtProduct" runat="server" Width="100%" Text='<%# Eval("NameProduct")%>' />
                            </td>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblType" Text="Type" runat="server" />
                            </td>
                            <td style="width:10%">
                                <asp:Label ID="txtType" runat="server" Width="100%" Text='<%# Eval("NameType")%>' />
                            </td>                                
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblTransmissionMode" Text="Transmission Mode" runat="server" />
                            </td>
                            <td style="width:10%">
                                <asp:Label ID="txtTransmissionMode" runat="server" Width="100%" Text='<%# Eval("NameTransmissionMode")%>' />
                            </td>
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center" rowspan="2">
                                <asp:Label ID="lblComentario" Text="Comment" runat="server" />
                            </td>
                            <td rowspan="2" style="width:30%"> 
                                <asp:Label ID="txtComentario" runat="server" Width="99%" Height="100%" Text='<%# If(Eval("Comentario") = String.Empty, "No comments", Eval("Comentario"))%>' />
                            </td>                
                        </tr>
                        <tr>                
                            <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                                <asp:Label ID="lblCustProjectName" Text="Customer´s Project Name" runat="server" />
                            </td>
                            <td style="width: 10%"> 
                                 <asp:Label ID="txtCPN" runat="server" Width="100%" Text='<%# Eval("NameCustomerProjectName")%>' />
                            </td>
                            <td style="background-color:#EBEFF0; width:10%; text-align:center">
                                <asp:Label ID="lblNombreReferencia" runat="server" Text="Part Name" />
                            </td>
                            <td style="width: 10%">
                                <asp:Label ID="txtNombreReferencia" runat="server" Width="99%" Text='<%# Eval("FinalNameBrain")%>' />
                            </td>
                            <td style="background-color:#EBEFF0; width:10%; text-align:center">
                                <asp:Label ID="lblSpecification" runat="server" Text="Specification" />
                            </td>
                            <td style="width: 10%">
                                <asp:Label ID="txtSpecification" runat="server" Width="99%" Text='<%# Eval("Specification")%>' />
                            </td>                            
                        </tr>
                    </ItemTemplate>                                               
                </asp:FormView>    
                
                <table id="tablaImportarDatos" runat="server" style="width: auto; display:none; margin-top:20px; border:1px solid black">
                    <tr>
                        <td>
                            <img id="imgAdvertencia" src="../../App_Themes/Batz/Imagenes/advertencia.gif" />
                        </td>
                        <td style="margin-right: 10px; margin-left: 10px">
                            <asp:Label ID="lblMensaje" runat="server" Text="The entered Batz part already exist in another plant or plants" Font-Size="16px" Font-Bold="true" style="margin-right: 10px; margin-left: 10px" />
                        </td>
                        <td>
                            <asp:Button ID="btnImportarDatos" runat="server" Text="Import datas" OnClick="btnImportarDatos_Click" />
                        </td>
                    </tr>
                </table>

                <table style="width:100%;background-color: #91A0A7; text-align:center;margin-top:20px;color:#fff;font-weight:bold">
                    <tr runat="server" id="filaIntegracionBrain" style="margin-top:20px;">
                        <td style="width:40%">
                            <asp:Label ID="lblTextoInformativo" runat="server" Text="* The part number will be definitely integrated when you press F5 in RH depending on this value" />
                        </td>
                        <td style="width:60%">
                            <asp:RadioButtonList ID="rblIntegracionBrain" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1" Selected="true"/>
                                <asp:ListItem Text="No" Value="0" />                                
                            </asp:RadioButtonList>
                        </td>    
                    </tr>
                </table>
                <table style="width:100%; margin-bottom:20px">                    
                    <tr>
                        <td style="width:10%">&nbsp</td>
                        <td style="width:30%">&nbsp</td>
                        <td style="width:10%">&nbsp</td>
                        <td style="width:10%">&nbsp</td>
                        <td style="width:10%">&nbsp</td>
                        <td style="width:10%">&nbsp</td>
                        <td style="width:10%">&nbsp</td>
                        <td style="width:10%">&nbsp</td>
                    </tr>
                    <tr style="background-color: #F9E1D2; text-align:center">
                        <td colspan="8">
                            <asp:Label ID="lblDatosComunPlantas" runat="server" Text="Common information for plants" Font-Bold="true" />
                        </td>
                    </tr>                    
                    <tr>                                                                    
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblReferenciaPieza" Text="Batz Part Number" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenciaPieza" runat="server" MaxLength="13" Width="90%" style="text-transform:uppercase" AutoPostBack="true" OnTextChanged="txtReferenciaPieza_TextChanged" />
                            <act:TextBoxWatermarkExtender ID="tweReferenciaPieza" runat="server" TargetControlID="txtReferenciaPieza" WatermarkText="Select a product group to get the last part number" WatermarkCssClass="watermark" />
                            <asp:Image ID="imgReferenciaPieza" runat="server" ImageUrl = "~/App_Themes/Batz/Imagenes/warning.png" />
                            <asp:RequiredFieldValidator ID="rfvReferenciaPieza" runat="server" ErrorMessage="Required field" ControlToValidate="txtReferenciaPieza" ValidationGroup="CamposVacios" Display="None" />                            
                        </td>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblTipoPieza" runat="server" Text="Piece Type" />
                        </td>
                        <td colspan="3">
                            <TipoPieza:TipoPieza ID="txtTipoPieza" runat="server" />
                        </td>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblEstado" Text="State" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEstado" runat="server">
                                <asp:ListItem Text="Active" Value="1" />
                                <asp:ListItem Text="Inactive" Value="0" Selected="True" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblGrupoMaterial" runat="server" Text="Material Group" />
                        </td>
                        <td>
                            <GrupoMaterial:GrupoMaterial id="txtGrupoMaterial" runat="server" />
                        </td>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblGrupoProducto" runat="server" Text="Product Group" />
                        </td>
                        <td colspan="3">
                            <GrupoProducto:GrupoProducto id="txtGrupoProducto" runat="server" />
                        </td>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblPiezaCompraDirigida" runat="server" Text="Direct Purchasing Part" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPiezaCompraDirigida" runat="server">
                                <asp:ListItem Text="Yes" Value="1" />
                                <asp:ListItem Text="No" Value="" Selected="True" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblTipoProducto" runat="server" Text="Product Type" />
                        </td>
                        <td>
                            <TipoProducto:TipoProducto id="txtTipoProducto" runat="server" />
                        </td>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblNivelIngenieria" runat="server" Text="Engineering Level" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNivelIngenieria" runat="server" Width="99%" MaxLength="16" />
                            <%--<asp:RegularExpressionValidator id="revNivelIngenieria" 
                                 ControlToValidate="txtNivelIngenieria"
                                 ValidationExpression="^[0-9]{4}\.[0-9]{2}\.[0-9]{2}$"
                                 Display="None"
                                 ErrorMessage="Wrong format. Valid: YYYY.MM.DD"
                                 runat="server" ValidationGroup="CamposVacios"/>--%>
                        </td>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblPasarDespieceWeb" runat="server" Text="Translate structure to WEB" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPasarDespieceWeb" runat="server">
                                <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                <asp:ListItem Text="No" Value="" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblMatchCode" runat="server" Text="Match Code" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMatchCode" runat="server" Width="99%" MaxLength="10" />
                            <asp:RequiredFieldValidator ID="rfvMatchCode" runat="server" ErrorMessage="Required field" ControlToValidate="txtMatchCode" ValidationGroup="CamposVacios" Display="None" />                            
                        </td>                                                                        
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblPesoNeto" runat="server" Text="Net Weight" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPesoNeto" runat="server" Width="80%" />
                            <act:FilteredTextBoxExtender ID="ftePesoNeto" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtPesoNeto" />
                            <%--<asp:RegularExpressionValidator id="revPesoNeto" 
                                 ControlToValidate="txtPesoNeto"
                                 ValidationExpression="^\d*[\.]\d*$"
                                 Display="Dynamic"
                                 ErrorMessage="Wrong format"
                                 runat="server" ValidationGroup="CamposVacios"/>--%>
                            <%-- ValidationExpression="^\d*[\.\,]\d*$" --%>
                            <%-- ^[0-9]{1,6}([,.][0-9]{1,4})? --%>
                        </td>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblNumDin" runat="server" Text="DIN No." />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumDin" runat="server" Width="99%" MaxLength="5" />
                        </td>
                        <td style="background-color:#EBEFF0; width:10%; text-align:center">
                            <asp:Label ID="lblPseudoSubconjunto" runat="server" Text="Pseudo subassembly" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPseudoSubconjunto" runat="server" Enabled="false">
                                <asp:ListItem Text="Yes" Value="1" />
                                <asp:ListItem Text="No" Value="" Selected="True" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblPlanoWeb" runat="server" Text="Drawing No." />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlanoWeb" runat="server" Width="99%" MaxLength="10" />
                        </td>                        
                        <td style="background-color:#EBEFF0; text-align:center">
                            <asp:Label ID="lblObservaciones" runat="server" Text="Remarks" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtObservaciones" runat="server" Width="99%" MaxLength="10" />
                        </td> 
                        <td colspan="2">&nbsp</td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp
                        </td>
                    </tr>
                    <tr style="background-color: #F9E1D2; text-align:center">
                        <td colspan="8">
                            <asp:Label ID="lblDatosPorPlantas" runat="server" Text="Information per plants" Font-Bold="true" />
                        </td>
                    </tr>                           
                    <tr>                        
                        <td colspan="8">
                            <act:TabContainer ID="tcPlantas" runat="server" Width="100%" CssClass="MyTabStyle" BorderWidth="1px">                               
                            </act:TabContainer>                            
                        </td>
                    </tr>  
                    <tr>
                        <td style="padding-top:20px; text-align:left">
                           <asp:Button ID="btnVolver" runat="server" Text="Back" OnClick="btnVolver_Click" />
                        </td>
                        <td colspan="6" style="text-align:center; padding-top:20px">
                            <asp:Button ID="btnGuardar" runat="server" Text="Save" BackColor="#ebf3de" Font-Size="16px" BorderColor="black" ValidationGroup="CamposVacios" style="margin-right:20%" OnClick="btnGuardar_Click" />
                            <act:ConfirmButtonExtender ID="cbeGuardar" runat="server" DisplayModalPopupID="mpeGuardar" TargetControlID="btnGuardar"></act:ConfirmButtonExtender>                    
                            <act:ModalPopupExtender ID="mpeGuardar" runat="server" PopupControlID="pnlConfirmarAprobacion" TargetControlID="btnGuardar" OkControlID="btnSi"
                                CancelControlID="btnNo" BackgroundCssClass="modalBackground"></act:ModalPopupExtender>
                            <asp:Panel ID="pnlConfirmarAprobacion" runat="server" CssClass="modalPopup" Style="display: none" Width="40%">
                                <div class="header">
                                    <asp:Label ID="lblConfirmacion" runat="server" Text="Approve request" />
                                </div>
                                <div class="body">
                                    <asp:Label ID="lblConfirmar" runat="server" Text="The part number will not be saved in Brain and will be sent to the requester.<br />Are you sure you want to approve the request?<br />" />
                                </div>
                                <div class="footer" align="center">
                                    <asp:Button ID="btnSi" runat="server" CssClass="si" Text="Yes" />
                                    <asp:Button ID="btnNo" runat="server" CssClass="no" Text="No" />
                                </div>
                            </asp:Panel>
                            <asp:Button ID="btnGuardarBrain" runat="server" Text="Save in Brain" BackColor="#ebf3de" Font-Size="16px" BorderColor="black" ValidationGroup="CamposVacios" OnClick="btnGuardarBrain_Click" />
                        </td>                       
                        <td style="padding-top:20px; text-align:right">
                            <asp:Button ID="btnLimpiarCampos" runat="server" CausesValidation="false" Text="Clean data" OnClick="btnLimpiarCampos_Click" />
                        </td>
                    </tr>
                </table>         
            </asp:Panel> 
            <act:ValidatorCalloutExtender ID="vceReferenciaPieza" runat="server" TargetControlID="rfvReferenciaPieza" PopupPosition="BottomRight" />
            <act:ValidatorCalloutExtender ID="vceMatchCode" runat="server" TargetControlID="rfvMatchCode" PopupPosition="BottomRight" /> 
            <%--<act:ValidatorCalloutExtender ID="vcePesoNeto" runat="server" TargetControlID="revPesoNeto" PopupPosition="BottomRight" />  --%>                        
        </ContentTemplate>
    </asp:UpdatePanel>
    <PanelCargandoDatos:PanelCargandoDatos ID="panelCargandoDatos1" runat="server" />
</asp:Content>

