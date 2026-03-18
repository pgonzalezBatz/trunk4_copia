<%@ Page Language="vb" AutoEventWireup="true" MasterPageFile="~/MPWeb.Master" CodeBehind="ResumenCalidad.aspx.vb" Inherits="WebRaiz.ResumenCalidad" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">   
        .FondoAplicacion
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
    <script type="text/javascript" src="../../js/jQuery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery-ui.js"></script>
    <link type="text/css" href="../../App_Themes/Tema1/jquery-ui.css" rel="stylesheet" />
    <link type="text/css" href="../../App_Themes/Tema1/style.css" rel="stylesheet" />
    <!--Css y Js del teclado virtual -->
    <link type="text/css" href="../../App_Themes/Tema1/Keyboard/Keyboard.css" rel="stylesheet" /> 
    <script type="text/javascript" src="../../js/jQuery/Keyboard/jquery.keyboard.js"></script>   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <!--Js de usuarios -->
    <script src="../../js/jQuery/usuarios.js" type="text/javascript"></script>

    <!-- Js de numeric -->
    <script type="text/javascript" src="../../js/jQuery/jquery.numeric.js"></script>

    <script type="text/javascript">
       $(function () {

           $('.datos-numericos').numeric({ negative: false });

           $("#" + '<%=imgbCambioReferencia.ClientID%>').bind("click", function () {
               $("#modal_Guardar_Errores_Cambio_Referencia").dialog({
                   resizable: false,
                   draggable: false,
                   height: 150,
                   width: 700,
                   buttons: {
                       SI: function () {
                           $(this).dialog("close");
                           var btnGuardar = $("[id$='btnGuardarErroresCambioReferencia_Oculto']").attr('id');
                           $('#' + btnGuardar).click();
                       },
                       NO: function () {
                           $(this).dialog('close');
                       }
                   },
                   modal: true
               });

               $('.ui-dialog-buttonset>button:first-child').css('float', 'left');
               $('.ui-dialog-buttonset>button:first-child').css('margin-left', '20%');
               $('.ui-dialog-buttonset>button:last-child').css('float', 'right');
               $('.ui-dialog-buttonset>button:last-child').css('margin-right', '20%');
               return false;
           });                     

           $('.keyboard').keyboard({
               openOn: null,
               stayOpen: true,
               autoAccept: true,
               usePreview: false,
               layout: 'qwerty',
               position: {
                   of: null,
                   my: 'center top',
                   at: 'center bottom'
               }
           });

           $('.keyboardReparacion').keyboard({
               openOn: null,
               stayOpen: true,
               autoAccept: true,
               usePreview: false,
               layout: 'qwerty',
               position: {
                   of: null,
                   my: 'center top',
                   at: 'center bottom'
               }
           });

           $('.keyboardNumeros').keyboard({
               openOn: null,
               stayOpen: true,
               autoAccept: true,
               layout: 'custom',
               usePreview: false,
               customLayout: {
                   //'default': ['7 8 9', '4 5 6', '1 2 3', '0 {clear} {bksp}', '{a} {c}']
                   'default': ['7 8 9', '4 5 6', '1 2 3', '{empty} 0 {empty}', '{clear} {bksp}', '{a} {c}']
               },
               position: {
                   of: null,
                   my: 'center top',
                   at: 'center bottom'
               }
           });

           $('#imgKeyboardComentario').click(function () {
               $('.keyboard').getkeyboard().reveal();
               $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
               $('.ui-widget-content .ui-state-default').css('color', '#000000');
               $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
               $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
           });

           $('#imgKeyboardReparacion').click(function () {
               $('.keyboardReparacion').getkeyboard().reveal();
               $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
               $('.ui-widget-content .ui-state-default').css('color', '#000000');
               $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
               $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
           });

           $('#imgKeyboardUsuario').click(function () {
               $('.keyboardNumeros').getkeyboard().reveal();
               $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
               $('.ui-widget-content .ui-state-default').css('color', '#000000');
               $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
               $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
           });          
       });

        function ComprobarCampos() {
            var txtComentario = $("[id$='txtComentario']").attr('id');
            var comentario = $('#' + txtComentario).val();
            var txtUsuario = $("[id$='txtUsuario']").attr('id');
            var usuario = $('#' + txtUsuario).val();
            if (comentario.length != 0 & usuario.length != 0) {
                var modal = $find('modalPopup');
                modal.hide();
                return true;
            }
            else {
                var comentarioError = $("[id$='lblComentarioError']").attr('id');
                $('#' + comentarioError).css('display', 'inline');
                return false;
            }
        };
 </script> 

    <Titulo:Titulo ID="titResumen" Texto="Resumen" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlResumen" runat="server">
                <div style="float: left; width: 100%">
                    <div style="float: left; width: 35%">
                        <asp:DetailsView ID="dvResumen" runat="server" AllowPaging="false" DefaultMode="ReadOnly"
                            AutoGenerateRows="false" GridLines="None" Width="100%" CssClass="DetailsView">
                            <HeaderStyle CssClass="HeaderStyleDetailsView" />
                            <FieldHeaderStyle CssClass="FieldHeaderStyle" Width="30%" />
                            <RowStyle CssClass="RowStyleDetailsView" />
                            <AlternatingRowStyle CssClass="AlternatingRowStyleDetailsView" />
                            <CommandRowStyle CssClass="CommandRowStyleDetailsView" />
                            <HeaderTemplate>
                                <asp:Label ID="lblSinDatos" runat="server" Text="Datos generales" CssClass="negrita" />
                            </HeaderTemplate>
                            <Fields>                                
                                <asp:TemplateField HeaderText="Cód. Op.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodOperacion" Text='<%# Eval("CodOperacion")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Usuario">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsuario" Text='<%# Eval("Usuario")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cod. Usuario">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsuario" Text='<%# Eval("CodUsuario")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrabajador" Text='<%# Eval("TipoTrabajador")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Turno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTurno" Text='<%# Eval("Turno")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>
                        <br />
                        <br />
                        <asp:Panel ID="pnlInformacion" runat="server" GroupingText="Resultado" Font-Bold="true" Width="95%">
                            <table style="margin-top: 10px; margin-bottom: 10px; text-align: center">
                                <tr>
                                    <td valign="middle">
                                        <asp:Image ID="imgInfo" runat="server" AlternateText="Warning" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="lblInfo" runat="server" Text="Hay características NOK" CssClass="negrita" Font-Size="24px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="float: left; width: 60%; margin-left: 4%; height: 200px; overflow-x: hidden; overflow-y: auto">
                        <asp:GridView ID="gvCaracteristicas" runat="server" DataKeyNames="IdRegistro" Width="100%"
                            AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None" Caption="Características"
                            OnRowDataBound="gvCaracteristicas_RowDataBound">
                            <RowStyle CssClass="RowStyle" />
                            <FooterStyle CssClass="FooterStyle" />
                            <PagerStyle CssClass="PagerStyle" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" />
                            <HeaderStyle CssClass="HeaderStyle" />
                            <EditRowStyle CssClass="EditRowStyle" />
                            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                            <EmptyDataTemplate>
                                <asp:Label ID="lblSinRegistros" runat="server" Text="No hay características" />
                            </EmptyDataTemplate>
                            <Columns>                               
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <act:CollapsiblePanelExtender ID="cpeAcciones" runat="server" TargetControlID="pnlControlValores"
                                            ExpandControlID="imgEstadoPanel_ControlValores"
                                            CollapseControlID="imgEstadoPanel_ControlValores" ImageControlID="imgEstadoPanel_ControlValores"
                                            Collapsed="True" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                                            ExpandedImage="~/App_Themes/Tema1/Imagenes/minus.png"
                                            CollapsedImage="~/App_Themes/Tema1/Imagenes/plus.png"
                                            CollapsedText="Ver" ExpandedText="Ver" />
                                        <asp:ImageButton ID="imgEstadoPanel_ControlValores" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Caracteristica" HeaderText="Característica"  />
                                <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("Tipo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor" ItemStyle-Height="36px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Image ID="imgCaracteristicaValor" runat="server" Width="24" Height="24" />
                                        <asp:Label ID="lblCaracteristicaValor" runat="server" Text='<%# Eval("Valor")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSubir" runat="server" AlternateText="Subir" ImageUrl="~/App_Themes/Tema1/IconosAcciones/addFile24.png" OnClick="imgSubir_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <tr>
                                            <td colspan="100%">
                                                <asp:Panel ID="pnlControlValores" runat="server">
                                                    <asp:Repeater runat="server" ID="rptAdjuntos" OnItemDataBound="rptAdjuntos_ItemDataBound" OnItemCommand="rptAdjuntos_ItemCommand">
                                                        <ItemTemplate>
						                                    <div style="float:left">
                                                                <asp:Label runat="server" ID="lblAdjunto" />
                                                            </div>
                                                            <div style="float:left; padding-left:10px">
                                                                <asp:ImageButton ID="imgQuitarAdj" runat="server" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Imagenes/borrar.gif" />
                                                            </div>
                                                            <div class="clear-float"></div>
					                                    </ItemTemplate>                    
                                                    </asp:Repeater>                                                    
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="clear-float"></div>

                <div id="divError" runat="server" style="float: left; width: 100%; margin-top: 15px">
                    <!-- Los 3 botones cuando hay error -->
                    <table width="100%">
                        <tr>
                            <td style="width: 33%; text-align: center">
                                <asp:ImageButton ID="imgbValidacion" runat="server" AlternateText="Validar" ImageUrl="~/App_Themes/Tema1/Imagenes/boton_validacion.png" />
                            </td>
                            <td style="width: 33%; text-align: center">
                                <asp:ImageButton ID="imgbReparacion_MultiOperacion" runat="server" AlternateText="Reparar" ImageUrl="~/App_Themes/Tema1/Imagenes/boton_reparacion.png" />
                            </td>
                            <td style="width: 33%; text-align: center">
                                <asp:ImageButton ID="imgbCambioReferencia" runat="server" AlternateText="Cambiar referencia" ImageUrl="~/App_Themes/Tema1/Imagenes/boton_cambio_referencia.png" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divSinErrores" runat="server" style="width: 100%; text-align: center; margin-top: 15px">
                    <asp:Panel ID="pnlGuardar" runat="server" Width="95%">
                        <asp:ImageButton ID="imgbGuardar" runat="server" AlternateText="Guardar" ImageUrl="~/App_Themes/Tema1/Imagenes/boton_guardar.png" OnClick="btnGuardar_Click" />
                    </asp:Panel>
                </div>

                <div id="modal_Guardar_Errores_Cambio_Referencia" title="Guardar" style="display: none; text-align: center">
                    <div style="float: left; width: 70%">
                        <asp:Label ID="lblCambioReferencia" runat="server" Text="Se va a cambiar de referencia" Font-Size="16px" />:
                    </div>
                    <div style="float: left; width: 29%">
                        <br />
                        <asp:Label ID="lblConfirmarReferencia" runat="server" Text="¿CONFIRMAR?" Font-Size="16px" />
                    </div>
                </div>

                <div id="modal_Rellenar_Campos" title="Validación" style="display: none; text-align: center">
                    <asp:Label ID="lblRellenarCampos" runat="server" Text="Debe rellenar los campos obligatorios" />
                </div>

                <asp:Button ID="btnGuardarErroresValidacion_Oculto" runat="server" Text="Button" style="display: none" OnClick="btnGuardarErroresValidacion_Click" />
                <asp:Button ID="btnGuardarErroresReparacionMultiOperacion_Oculto" runat="server" Text="Button" style="display: none" OnClick="btnGuardarErroresReparacionMultiOperacion_Click" />
                <asp:Button ID="btnGuardarErroresCambioReferencia_Oculto" runat="server" Text="Button" style="display: none" OnClick="btnGuardarErroresCambioReferencia_Click" />                

                <asp:Panel ID="pnlValidacion" runat="server" style="display: none" Width="90%" BackColor="white" BorderWidth="2px" BorderStyle="Solid" BorderColor="Black">
                    <table width="100%" cellpadding="10">
                        <tr>
                            <td colspan="2" style="background-color: #9BBB59; padding-left: 10px; border: 2px solid #953735;">
                                <asp:Label ID="lblValidacion" runat="server" Text="Validación" Font-Size="16px" ForeColor="White" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblComentarioValidacion" runat="server" Text="Comentario" Font-Size="14px" />:                  
                                <asp:TextBox ID="txtComentario" runat="server" TextMode="MultiLine" Rows="4" Width="93%" Font-Size="16px" CssClass="keyboard" BackColor="#F2F2F2" BorderWidth="2px" BorderStyle="Solid" BorderColor="#A6A6A6" />
                                <img id="imgKeyboardComentario" alt="" class="tooltip" title="Haz click para abrir el teclado virtual" src="../../App_Themes/Tema1/Keyboard/keyboard.png" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblUsuario" runat="server" Text="Validado por" Font-Size="14px" />:
                                <asp:TextBox ID="txtUsuario" runat="server" Width="93%" Font-Size="16px" BackColor="#F2F2F2" CssClass="datos-numericos keyboardNumeros" BorderWidth="2px" BorderStyle="Solid" BorderColor="#A6A6A6" />
                                <img id="imgKeyboardUsuario" alt="" class="tooltip" title="Haz click para abrir el teclado virtual" src="../../App_Themes/Tema1/Keyboard/keyboard.png" />
                                <br />
                                <br />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Label ID="lblComentarioError" runat="server" Text="Los campos no pueden estar vacíos" Font-Size="20px" ForeColor="Red" Font-Bold="true" Style="display: none" />
                            </td>
                        </tr>
                        <tr>
                            <td width="50%" align="center">
                                <asp:ImageButton ID="btnConfirmar" runat="server" AlternateText="Guardar" ImageUrl="~/App_Themes/Tema1/Imagenes/btnGuardarValidacion.png" OnClientClick="return ComprobarCampos();" />
                            </td>
                            <td width="50%" align="center">
                                <asp:ImageButton ID="btnSalir" runat="server" AlternateText="Cancelar" ImageUrl="~/App_Themes/Tema1/Imagenes/btnCancelarValidacion.png" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel ID="pnlReparacion" runat="server" style="display: none" Width="90%" BackColor="white" BorderWidth="2px" BorderStyle="Solid" BorderColor="Black">
                    <table width="100%" cellpadding="10" style="border-collapse: collapse">
                        <tr>
                            <td colspan="2" style="background-color: #9BBB59; padding-left: 10px; border: 2px solid #953735;">
                                <asp:Label ID="lblParteMantenimiento" runat="server" Text="ABRIR PARTE DESDE UN ORDENADOR" Font-Size="16px" ForeColor="White" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblComentario" runat="server" Text="Se guardará el control realizado y tendrás que abrir el parte desde un ordenador" Font-Size="16px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="50%" align="center">
                                <asp:ImageButton ID="btnConfirmarReparacion" runat="server" AlternateText="Aceptar" ImageUrl="~/App_Themes/Tema1/Imagenes/btnAceptar.png" />
                            </td>
                            <td width="50%" align="center">
                                <asp:ImageButton ID="btnSalirReparacion" runat="server" AlternateText="Cancelar" ImageUrl="~/App_Themes/Tema1/Imagenes/btnCancelarValidacion.png" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <act:ModalPopupExtender ID="mpeSalir" runat="server" BehaviorID="modalPopup" PopupControlID="pnlValidacion" BackgroundCssClass="FondoAplicacion"
                    TargetControlID="imgbValidacion" CancelControlID="btnSalir">
                </act:ModalPopupExtender>

                <act:ModalPopupExtender ID="mpeReparacion" runat="server" BehaviorID="modalPopupReparacion" PopupControlID="pnlReparacion" BackgroundCssClass="FondoAplicacion"
                    TargetControlID="imgbReparacion_MultiOperacion" CancelControlID="btnSalirReparacion">
                </act:ModalPopupExtender>
            
                <asp:Button id="btnModalFicherosOculto" runat="server" style="display:none" />
                <act:ModalPopupExtender ID="mpeModalFicheros" BehaviorID="pnlModalFicheros" runat="server"
                TargetControlID="btnModalFicherosOculto" PopupControlID="pnlModalFicheros"
                CancelControlID="btnAceptarFicheros" BackgroundCssClass="FondoAplicacion" />

                <asp:Panel ID="pnlModalFicheros" runat="server" Style="display: none;" CssClass="modalPopup" Width="50%">
                    <div class="header">
                        <asp:Label ID="lblAccionNueva" runat="server" Text="Ficheros de la característica" />
                    </div>
                    <div class="body">                    
                        <div id="divAdjuntarArchivo" runat="server" style="margin-top: 10px">                                
                            <div class="cpHeader" style="padding: 5px 0 5px 0; text-align: center">
                                <asp:Label ID="lblAdjuntarArchivo" Text="Adjuntar ficheros" CssClass="negrita" runat="server" />
                            </div>
                            <table width="100%">
                                <tr>
                                    <td class="campoTextoNormal" style="width:90%">
                                        <asp:FileUpload ID="fUpload" runat="server" Width="100%" />                                           
                                    </td>
                                    <td style="width:10%">
                                        <asp:Button ID="btnAdjuntar" runat="server" Text="Adjuntar" ValidationGroup="FicheroVacio" OnClick="btnAdjuntar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:RequiredFieldValidator ID="rfvAdjuntarArchivo" Display="Dynamic" runat="server" ControlToValidate="fUpload" ValidationGroup="FicheroVacio" Font-Italic="true" ErrorMessage="Debe seleccionar el fichero a subir" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Repeater runat="server" ID="rptAdjuntosSubir" >
                                            <ItemTemplate>
                                                <div style="float:left">
                                                    <asp:Image runat="server" ID="imgBullet" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Imagenes/bullet_hl.gif" />
                                                </div>
                                                <div style="float:left">
                                                    <asp:Label runat="server" ID="lblAdjunto" />
                                                </div>
                                                <div style="float:left; padding-left:10px">
                                                    <asp:ImageButton ID="imgQuitarAdj" runat="server" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Imagenes/borrar.gif" />
                                                </div>
                                                <div class="clear-float"></div>
					                        </ItemTemplate>                    
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>                                
                        </div>
                    </div>                                                                                                                                
 
                    <div align="center" style="margin-top:20px; margin-bottom:5px">                 
                        <asp:Button ID="btnAceptarFicheros" runat="server" Text="Aceptar" />
                    </div>  
                    <asp:HiddenField ID="hfIdCaracteristica" runat="server" />                                                                      
	            </asp:Panel>            
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardarErroresValidacion_Oculto" />
            <asp:PostBackTrigger ControlID="btnGuardarErroresReparacionMultiOperacion_Oculto" />
            <asp:PostBackTrigger ControlID="btnGuardarErroresCambioReferencia_Oculto" />
        </Triggers>
    </asp:UpdatePanel>
    <PanelCargandoDatos:PanelCargandoDatos ID="pnlCargandoDatos" runat="server" />  
</asp:Content>


