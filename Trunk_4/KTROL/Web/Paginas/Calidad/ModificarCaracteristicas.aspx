<%@ Page Language="vb" AutoEventWireup="true" MasterPageFile="~/MPWeb.Master" CodeBehind="ModificarCaracteristicas.aspx.vb" Inherits="WebRaiz.ModificarCaracteristicas" %>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
   <script type="text/javascript" src="../../js/jQuery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery.cookie.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery.numeric.js"></script>
    <script type="text/javascript" src="../../js/jQuery/UsuariosCalidad.js"></script>
    <script type="text/javascript">
        Sys.Application.add_load(init);
        function init() {
            $('.numerico').numeric({ decimal: false, negative: false });
            $('.datos-numericos').numeric({ decimal: ",", negative: false });

            var panel = $(".panel");
            var boton = $(".verDatos");
            var state = $.cookie("ToggleStatus");

            boton.click(function () {
                panel.slideToggle("slow", function () {
                    $.cookie("ToggleStatus", (state == 1 ? "0" : "1"));
                });
            });

            if ((state == 0 && panel.is(':visible'))) {
                panel.slideUp();
            }
        }

    </script>

   <Titulo:Titulo ID="titModificarCaracteristicas" Texto="Editar controles y características" runat="server" />
   <br />   
   <asp:Panel ID="pnlCaracteristicas" runat="server">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>               
               <asp:Panel ID="pnlGridView" runat="server">
                   <table>
                       <tr>
                           <td class="definicionNoWidth">
                               <asp:Label ID="lblIdControl" runat="server" Text="Identificador del control" />
                           </td>
                           <td class="campoTextoNormal">        
                               <asp:TextBox ID="txtIdControl" runat="server" MaxLength="6" class="numerico" />
                           </td>
                           <td style="padding-left:10px">
                               <asp:Button ID="btnBuscarControl" runat="server" Text="Buscar" OnClick="btnBuscarControl_Click" />
                           </td>
                           <td style="width:50%" align="right">
                               <asp:Button ID="btnEliminarControl" runat="server" Text="Eliminar control" OnClick="btnEliminarControl_Click" BackColor="red" ForeColor="White" />
                               <act:ConfirmButtonExtender ID="cbeEliminar" runat="server" DisplayModalPopupID="mpeEliminar"
                                    TargetControlID="btnEliminarControl"></act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminarControl" OkControlID="btnBorrar"
                                    CancelControlID="btnCancelar" BackgroundCssClass="modalBackground"></act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Confirmación" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado" runat="server" Text="¿Desea confirmar que quiere borrar el control?" />
                                    </div>
                                    <div class="footer" align="center">
                                        <asp:Button ID="btnBorrar" runat="server" CssClass="si" Text="Si" />
                                        <asp:Button ID="btnCancelar" runat="server" CssClass="no" Text="No" />
                                    </div>
                                </asp:Panel>
                           </td>
                       </tr>
                       <tr>
                           <td>
                               &nbsp;
                           </td>
                           <td>
                               <asp:RequiredFieldValidator ID="rfvIdControl" runat="server" ControlToValidate="txtIdControl" ErrorMessage="Id requerido" Display="None" />
                               <act:ValidatorCalloutExtender ID="vceIdControl" TargetControlID="rfvIdControl" runat="server" PopupPosition="Left" CssClass="vceCaracteristicas" />
                           </td>
                           <td>
                               &nbsp;
                           </td>
                       </tr>
                   </table>
                   
                   <table>
                       <tr>
                           <td>
                               <asp:LinkButton id="lnbtUsuario" text="Ver/Ocultar datos del usuario del control" style="text-decoration:underline"  runat="server" class="verDatos" CausesValidation="false" OnClientClick="return false" />
                           </td>
                       </tr>
                   </table>
                   <div class="panel">
                       <table width="35%" style="margin-top:10px">
                           <tr>
                               <td>
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
                                                    <asp:Label ID="lblCodUsuario" Text='<%# Eval("CodUsuario")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTrabajador" Text='<%# Eval("Idtipo")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Turno">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTurno" Text='<%# Eval("TurnoTrabajador")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                    </asp:DetailsView>
                               </td>
                           </tr>
                       </table>
                   </div>
                   

                   <table width="100%" style="margin-top:10px">
                       <tr>
                           <td>
                               <asp:Label ID="lblAdvertenciaCaracteristicas" runat="server" Text="* Al modificar una característica incorrecta a correcta, si esta característica es la única incorrecta de todas del control, se perderá el error asociado a este control" Font-Italic="true" />
                           </td>
                       </tr>                       
                       <tr>
                           <td>
                               <asp:GridView ID="gvControles" runat="server" DataKeyNames="IdRegistro"
                                   Width="80%" AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None"
                                   OnRowEditing="gvControles_RowEditing" OnRowUpdating="gvControles_RowUpdating" 
                                   OnRowCancelingEdit="gvControles_RowCancelingEdit">
                                   <RowStyle CssClass="RowStyle" Height="24px" />
                                   <FooterStyle CssClass="FooterStyle" />
                                   <PagerStyle CssClass="PagerStyle" />
                                   <SelectedRowStyle CssClass="SelectedRowStyle" />
                                   <HeaderStyle CssClass="HeaderStyle" />
                                   <EditRowStyle CssClass="EditRowStyle" />
                                   <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                   <EmptyDataTemplate>
                                       <asp:Label ID="lblSinRegistros" runat="server" />
                                   </EmptyDataTemplate>
                                   <Columns>
                                       <asp:TemplateField HeaderText="Id Registro" Visible="false">
                                           <ItemTemplate>
                                               <asp:Label ID="lblIdRegistro" runat="server" Visible="false" Text='<%# Eval("IdRegistro")%>'></asp:Label>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField Visible="false">
                                           <ItemTemplate>
                                               <asp:Label ID="lblOkNok" runat="server" Text='<%# Eval("OkNok")%>' />
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField Visible="false">
                                           <ItemTemplate>
                                               <asp:Label ID="lblValor" runat="server" Text='<%# Eval("Valor")%>' />
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:BoundField DataField="Posicion" HeaderText="Posición" ReadOnly="true" />
                                       <asp:BoundField DataField="CaracParam" HeaderText="Característica" ReadOnly="true" />                                       
                                       <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="center">
                                           <ItemTemplate>
                                               <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("Tipo")%>' />
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Center">
                                           <ItemTemplate>
                                               <asp:Label ID="lblCaracteristicaValor" runat="server" Text='<%#Eval("Valor")%>' Visible='<%# Eval("OkNok") <> 2 %>' />
                                               <asp:Image ID="imgCaracteristicaValor" runat="server" Width="24" Height="24" Visible='<%# Eval("OkNok") <> 2%>' />
                                           </ItemTemplate>
                                           <EditItemTemplate>                                               
                                               <asp:TextBox runat="server" ID="txtEditarValor" class="datos-numericos" Text='<%#Eval("Valor")%>' Visible='<%# Eval("Tipo") = "V"%>' />                                               
                                               <asp:RequiredFieldValidator runat="server" ID="rfvValor" ControlToValidate="txtEditarValor" ErrorMessage="Valor requerido" Display="None" ValidationGroup="ValorVacio" />
                                               <act:ValidatorCalloutExtender ID="vceValor" TargetControlID="rfvValor" runat="server" PopupPosition="Left" CssClass="vceCaracteristicas" />
                                               <asp:CheckBox ID="chkCambiarControl" Text="Característica NOK" Font-Italic="true" runat="server" Visible='<%# Eval("Tipo") = "V"%>' />
                                               <asp:DropDownList runat="server" ID="ddlEditarValor" Visible='<%# Eval("Tipo") = "A"%>'>
                                                   <asp:ListItem Text="OK" Value="1" />
                                                   <asp:ListItem Text="NOK" Value="0" />
                                                </asp:DropDownList>
                                           </EditItemTemplate>
                                       </asp:TemplateField>                                        
                                       <asp:TemplateField>
                                           <ItemTemplate>
                                               <asp:ImageButton ID="btnEditar" runat="server" Text="Editar" CommandName="Edit" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" ToolTip="Editar valor de la característica" Visible='<%# Eval("OkNok") <> 2%>' CausesValidation="false" />
                                           </ItemTemplate>
                                           <EditItemTemplate>
                                                <asp:ImageButton ID="btnGuardar" Text="Guardar" runat="server" CommandName="Update" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Guardar24.png" ToolTip="Guardar" ValidationGroup="ValorVacio" />
                                                <asp:ImageButton ID="btnCancelar" Text="Cancelar" runat="server" CommandName="Cancel" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Cancelar24.png" ToolTip="Cancelar" CausesValidation="false" />                                               
                                           </EditItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField ItemStyle-HorizontalAlign="Center" AccessibleHeaderText="imgSubirFichero">
                                           <ItemTemplate>
                                               <asp:ImageButton ID="imgSubir" runat="server" AlternateText="Subir" ImageUrl="~/App_Themes/Tema1/IconosAcciones/addFile24.png" ToolTip="Añadir/Eliminar ficheros a la característica" OnClick="imgSubir_Click" 
                                                   Visible='<%# Eval("OkNok") <> 2%>' CausesValidation="false" />
                                           </ItemTemplate>
                                       </asp:TemplateField>                               
                                   </Columns>
                               </asp:GridView>
                           </td>
                       </tr>
                   </table>
                   <asp:Panel ID="pnlErrores" runat="server" style="margin-top:20px">
                        <asp:DetailsView ID="dvErrores" runat="server" AllowPaging="false" DefaultMode="Edit"
                            AutoGenerateRows="false" GridLines="None" Width="80%" CssClass="DetailsView">
                            <HeaderStyle CssClass="HeaderStyleDetailsView" />
                            <FieldHeaderStyle CssClass="FieldHeaderStyle" Width="20%" />
                            <RowStyle HorizontalAlign="Left" CssClass="RowStyleDetailsView" />
                            <AlternatingRowStyle CssClass="RowStyleDetailsView" /><%-- CssClass="AlternatingRowStyleDetailsView" --%>
                            <CommandRowStyle CssClass="CommandRowStyleDetailsView" />
                            <HeaderTemplate>
                                <asp:Label ID="lblSinDatos" runat="server" Text="Error en el control" CssClass="negrita" />
                            </HeaderTemplate>
                            <Fields>         
                                <asp:TemplateField HeaderText="Tipo">
                                    <ItemTemplate>
                                        <asp:RadioButtonList ID="rbListTipoError" runat="server" AutoPostBack="true"  RepeatDirection="Vertical" OnSelectedIndexChanged="rbListTipoError_SelectedIndexChanged">
                                            <asp:ListItem Text="Validación" />
                                            <asp:ListItem Text="Parte a mantenimiento" />
                                            <asp:ListItem Text="Cambio Referencia" />
                                        </asp:RadioButtonList>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Validado Por">
                                    <ItemTemplate>
                                        <div style="float: left; width: 80%">
                                            <asp:TextBox ID="txtInsertUsuario" CssClass="anchoTextbox" runat="server" MaxLength="100" Width="98%" Text='<%# Eval("NombreValidacionUsuario")%>' />
                                        </div>                                        
                                        <div id="imgSeleccion" class="imagen-no-seleccionado" runat="server" style="float: left; margin-left: 2px"></div>                                        
                                        <div class="clear-float"></div>
                                        <asp:HiddenField runat="server" ID="hfIdUsuario" />
                                        <div id="helper" style="margin-top: -1px;" runat="server"></div> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comentario">
                                    <ItemTemplate>
                                        <asp:Textbox ID="txtComentario" runat="server" TextMode="MultiLine" Text='<%# Eval("Comentario")%>' Rows="4" Width="98%"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="" ItemStyle-Height="50" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Button ID="btnGuardarErrores" runat="server" Text="Guardar" OnClick="btnGuardarErrores_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>                              
                   </asp:Panel>                  
               </asp:Panel>

               <asp:Button runat="server" ID="btnModalPopUp" style="display:none"/>         
                <act:ModalPopupExtender ID="mpeAvisoGuardar" runat="server"
                        TargetControlID="btnModalPopUp" BehaviorID="modalPopup"
                        PopupControlID="pnlAvisoGuardar"
                        BackgroundCssClass="modalBackground">
                </act:ModalPopupExtender>

                <asp:Panel ID="pnlAvisoGuardar" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        <asp:Label ID="lblTituloAviso" runat="server" Text="Aviso" />
                    </div>
                    <div class="body">
                        <asp:Label ID="lblAviso" runat="server" Text="Desea confirmar la acción?" />
                    </div>
                    <div class="footer" align="center">
                        <asp:Button ID="btnOK" runat="server" Text="OK" CssClass="si"  />
                    </div>
                </asp:Panel>

               <asp:Button runat="server" ID="btnOcultoReporteErrores" style="display:none"/>         
                <act:ModalPopupExtender ID="mpeReporteErrores" runat="server"
                        TargetControlID="btnOcultoReporteErrores" BehaviorID="modalPopup2"
                        PopupControlID="pnlReporteErrores" CancelControlID="btnCancelarError"
                        BackgroundCssClass="modalBackground">
                </act:ModalPopupExtender>

                <asp:Panel ID="pnlReporteErrores" runat="server" CssClass="modalPopup" Width="70%" Style="display: none">
                    <asp:HiddenField ID="hfIdRegistro" runat="server" />
                    <asp:HiddenField ID="hfValor" runat="server" />
                    <div class="header">
                        <asp:Label ID="lblIdentificarError" runat="server" Text="Relacionar control con error" />
                    </div>
                    <div class="body">                        
                        <table width="100%" style="border-collapse:collapse" cellpadding="3">
                            <tr>
                                <td class="definicion15">
                                    <asp:Label ID="lblSeleccionarOpcion" runat="server" Text="Error" />
                                </td>
                                <td class="campoTextoNormal" align="left">
                                    <asp:DropDownList ID="ddlErrores" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="Validación" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Parte a mantenimiento" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Cambio referencia" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                 <td class="definicion15">
                                    <asp:Label ID="lblValidadoPor" runat="server" Text="Validado por" />
                                </td>
                                <td class="campoTextoNormal" align="left">
                                    <asp:TextBox ID="txtValidadoPor" runat="server" Width="30%" class="numerico" />
                                    <asp:RequiredFieldValidator ID="rfvValidacionUsuario" runat="server" ControlToValidate="txtValidadoPor" ValidationGroup="Validador" Display="None" ErrorMessage="Valor requerido" />
                                    <act:ValidatorCalloutExtender ID="vceValidacionUsuario" TargetControlID="rfvValidacionUsuario" runat="server" PopupPosition="right" CssClass="vceCaracteristicas" />
                                </td>
                            </tr>
                            <tr>
                                <td class="definicion15">
                                    <asp:Label ID="lblComentario" runat="server" Text="comentario" />
                                </td>
                                <td class="campoTextoNormal" align="left">
                                    <asp:TextBox ID="txtComentario" runat="server" Rows="4" TextMode="MultiLine" Width="98%" />
                                </td>
                            </tr>
                        </table>                         
                    </div>
                    <div class="footer" align="center">
                        <asp:Button ID="btnGuardarError" runat="server" Text="Guardar" ValidationGroup="Validador" CssClass="botonAceptar" OnClick="btnGuardarError_Click" />
                        <asp:Button ID="btnCancelarError" runat="server" Text="Cancelar" CssClass="botonAceptar" CausesValidation="false" />
                    </div>
                </asp:Panel>

               <asp:Button id="btnModalFicherosOculto" runat="server" style="display:none" />
                <act:ModalPopupExtender ID="mpeModalFicheros" BehaviorID="pnlModalFicheros" runat="server"
                TargetControlID="btnModalFicherosOculto" PopupControlID="pnlModalFicheros"
                CancelControlID="btnCancelar" BackgroundCssClass="FondoAplicacion" />

                <asp:Panel ID="pnlModalFicheros" runat="server" Style="display: none;" CssClass="modalPopup" Width="50%">
                    <div class="header">
                        <asp:Label ID="lblFicherosCaracteristica" runat="server" Text="Ficheros de la característica" />
                    </div>
                    <div class="body">                    
                        <div id="divAdjuntarArchivo" runat="server" style="margin-top: 10px">
                            <div class="cpHeader" style="padding:5px 0 5px 0; text-align: center">
                                <asp:Label ID="lblFicherosAdjuntos" Text="Ficheros adjuntos" CssClass="negrita" runat="server" />
                            </div>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Repeater runat="server" ID="rptAdjuntos">                                            
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
                                        <asp:Panel ID="pnlAdjuntosVacio" runat="server" style="margin:20px">
                                            <asp:Label ID="lblSinDatos" runat="server" Text="La característica no contiene ficheros adjuntos" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>                                
                            <div class="cpHeader" style="padding: 5px 0 5px 0; text-align: center">
                                <asp:Label ID="lblAdjuntarNuevosFicheros" Text="Adjuntar nuevos ficheros" CssClass="negrita" runat="server" />
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
                        <asp:Button ID="btnAceptarFicheros" runat="server" Text="Guardar" />
                        <p>
                            <asp:Label ID="lblNotaAdjuntos" runat="server" Text="Los ficheros adjuntos sólo se guardarán después de clicar en el botón 'Guardar'" Font-Italic="true" />
                        </p>
                    </div>
                    <div align="right" style="margin-top:10px; margin-bottom:5px">                 
                        <asp:Button ID="btnCancelarCambios" runat="server" Text="Cancelar" />                        
                    </div>  
                    <asp:HiddenField ID="hfIdCaracteristica" runat="server" />                                                                      
	            </asp:Panel>            
            
           </ContentTemplate>
           <Triggers>
               <asp:PostBackTrigger ControlID="lnbtUsuario" />
           </Triggers>
       </asp:UpdatePanel>
    </asp:Panel>

    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>

