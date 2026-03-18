<%@ Page Language="vb" MasterPageFile="~/MPWeb.Master" AutoEventWireup="false" CodeBehind="SeleccionRefOp.aspx.vb" Inherits="WebRaiz._SeleccionRefOp"  %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>

<asp:Content ID="cph" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../../js/jQuery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery-ui.js"></script>

    <script type="text/javascript" src="../../js/jQuery/jquery.cookie.js"></script>
    <link type="text/css" href="../../App_Themes/Tema1/jquery-ui.css" rel="stylesheet" />

    <script type="text/javascript" src="../../js/jQuery/Keyboard/jquery.keyboard.js"></script>
    <link type="text/css" href="../../App_Themes/Tema1/Keyboard/Keyboard.css" rel="stylesheet" /> 

    <script type="text/javascript" src="../../js/jQuery/jquery.numeric.js"></script>
    <script type="text/javascript">
        Sys.Application.add_load(init);
        function init() {
            $('.datos-numericos').numeric({ negative: false });


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

            $('#' + '<%=txtSelOperacion.ClientID%>').keyboard({
            //$('.keyboardOpe').keyboard({
                openOn: null,
                stayOpen: true,
                autoAccept: true,
                usePreview: false,
                layout: 'qwerty',
                position: {
                    of: null,
                    my: 'center top',
                    at: 'center bottom'
                }/*,
                    kb.shiftActive = true;
                    kb.showKeySet(el);
                    //kb.showKeySet('shift');
                }*/
            });

            $('.keyboardNumeros').keyboard({
                openOn: null,
                autoAccept: true,
                stayOpen: true,
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

            $('#imgKeyboardTexto').click(function () {
                $('.keyboard').getkeyboard().reveal();
                $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
                $('.ui-widget-content .ui-state-default').css('color', '#000000');
                $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
                $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
            });

            $('#imgKeyboardOperacion').click(function () {
                //$('.keyboardOpe').getkeyboard().reveal();
                $('#' + '<%=txtSelOperacion.ClientID%>').getkeyboard().reveal();
                $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
                $('.ui-widget-content .ui-state-default').css('color', '#000000');
                $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
                $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
            });

            $('.verDatos').click(function () {
                var panel = $(".panel");
                var state = $.cookie("ToggleStatus");
                panel.slideToggle("slow", function () {
                    $.cookie("ToggleStatus", (state == 1 ? "0" : "1"));
                });
                if ((state == 0 && panel.is(':visible'))) {
                    panel.slideDown();
                }
            });
        }
        
        function SelectAllCheckboxes(chk) {
            $('#<%=gvCaracteristicasCalidadGestor.ClientID%>').find("input:checkbox").each(function () {
                if (this != chk) { this.checked = chk.checked; }
            });            
		}   

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin:0 auto;">
                <tr>
                    <td>
                        <table class="table2" style="margin-left: 20px">
		                    <thead>
			                    <tr>
				                    <th colspan="2">
					                    <asp:Label runat="server" ID="lblCodOperacion" Text="Codigo operación" Font-Size="20px"></asp:Label>
				                    </th>
			                    </tr>
		                    </thead>
		                    <tbody>
			                    <tr>
				                    <td>
					                    <asp:Label runat="server" ID="lblCod" Text="cod" Font-Size="20px"></asp:Label>.&nbsp;
					                    <%--<asp:TextBox ID="txtSelOperacion" runat="server" CssClass="keyboardNumeros" Font-Size="20px" />>&nbsp;&nbsp;&nbsp;  datos-numericos --%>                                        
					                    <asp:TextBox ID="txtSelOperacion" runat="server" CssClass="keyboardOpe" Font-Size="20px" />&nbsp;&nbsp;&nbsp; <%-- datos-alfanuméricos --%>                                        

                                        <img id="imgKeyboardOperacion" alt="" class="tooltip" title="Haz click para abrir el teclado virtual" src="../../App_Themes/Tema1/Keyboard/keyboard.png" />&nbsp;
				                    </td>
                                    <td style="vertical-align:middle; width:100px; text-align:center" >
					                    <asp:Button ID="btnConfirmarOperacion" runat="server" Text="Buscar" UseSubmitBehavior="true" Font-Size="18px" ToolTip="Buscar" OnClick="btnConfirmarOperacion_Click" formnovalidate="formnovalidate" />
				                    </td>
			                    </tr>
		                    </tbody>
		                    <tfoot>
			                    <tr>
				                    <th colspan="2">
					                    <asp:Label runat="server" ID="lblDescripcion" Font-Size="14px"></asp:Label>
				                    </th>
			                    </tr>
		                    </tfoot>
	                    </table>
                    </td>
                    <td valign="middle" style="padding-left: 50px">
                        <asp:Button ID="btnCambiarOperacion" runat="server" Text="Cambiar Operación" Font-Size="18px" style="display:none" OnClick="btnCambiarOperacion_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>             
    
     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>     
            <asp:Panel ID="pnlReparacion" runat="server">  
                <asp:Label ID="lblReparacion" runat="server" Text="" Font-Size="18px" Font-Bold="true" />
            </asp:Panel>             
            <div id="divInfoPieza" runat="server" style="margin-top:30px">
                <table width="90%" style="margin-left: 20px; margin-top: 30px; margin-bottom:20px">
                    <tr>
                        <td valign="middle" class="definicion15">
                            <asp:Label ID="lblInfoPieza" runat="server" Text="Trazabilidad Pieza" Font-Size="14px" CssClass="negrita" />
                        </td>
                        <td valign="middle" style="width:70%">
                            <asp:TextBox ID="txtInfoPieza" runat="server" MaxLength="200" Font-Size="18px" Width="99%" CssClass="keyboard" required="true"/>


                        </td>
                        <td valign="middle">
                            <img id="imgKeyboardTexto" alt="" class="tooltip" title="Haz click para abrir el teclado virtual" src="../../App_Themes/Tema1/Keyboard/keyboard.png" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" valign="middle">
                            
                                        <img id="infoImg" class="innerTiptext" src="../../App_Themes/Tema1/Imagenes/infoPieza.png" style="display:block;position:relative;margin:0 auto;" alt="infoIcon" />
                                        <div id="infoDiv" style="display:block;margin:0 auto;width:300px;">
                                            <p> - Estampación: semana y año</p>
                                            <p> - Soldadura: fecha, hora y mesa</p>
                                            <p> - Inyección: fecha y cavidad</p>
                                            <p> - Montaje: fecha, hora y número de serie</p>
                                            <p> Si la pieza no tiene trazabilidad unitaria hay que poner la fecha</p>
                                        </div>

                        </td>
                    </tr>
                </table>

<%--                <div class="hoverInfo"> 
                    <span>i</span>
                    <p>3
                        <br>lines
                        <br>of text
                    </p>
                </div>--%>

            </div>                            
            <asp:Panel ID="pnlGlobal" runat="server" Width="100%">
                                                                                    
                <div id="divAdministrador" runat="server" style="width:100%; margin-top: 10px;">
                    <table width="90%" style="margin-left: 20px">
                        <tr>
                            <td valign="middle" class="definicion15">
                                <asp:Label ID="lblAccederComo" runat="server" Text="Acceder como" Font-Size="14px" CssClass="negrita" />
                            </td>
                            <td style="width:70%">
                                <asp:DropDownlist ID="ddlRoles" runat="server" AutoPostBack="true" Font-Size="18px" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" />                                
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divGestor" runat="server" style="width:100%; margin-top: 10px; margin-bottom:40px">
                    <table width="90%" style="margin-left: 20px">
                        <tr>
                            <td valign="middle" class="definicion15">
                                <asp:label ID="lblGestor" runat="server" Text="Selecciona una opción" Font-Size="14px" CssClass="negrita" />
                            </td>
                            <td>
                                <asp:Button ID="btnControlesGestor" runat="server" Text="Realizar control como gestor" CssClass="controlGestorActivo" OnCommand="btnAdministracionGestor_Command" CommandName="gestor" /> &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnControlesOperario" runat="server" Text="Realizar control como operario" OnCommand="btnAdministracionGestor_Command" Commandname="operario" />                                
                                <asp:HiddenField ID="hfTipoControl" runat="server" Value="1" />
                            </td>
                        </tr>
<%--				    <tr>
							<!--  TODO: AQUÍ VAMOS A METER UN SELECTOR DE FECHA PARA LOS GESTORES (CALIDAD)  -->
                            <td valign="middle" class="definicion15">
                                <asp:label ID="lblFecha" runat="server" Text="Selecciona una fecha" Font-Size="14px" CssClass="negrita" />
                            </td>
                            <td>
								    <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged">
									</asp:Calendar>
									<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></div>
                            </td>
						</tr>--%>
                    </table>
                </div>

                <div id="divControles" runat="server" style="display:none; width:100%; margin-top: 10px; margin-left: 20px">
                    <div align="right" style="margin-top:10px; margin-bottom:10px; margin-right:20%">
                        <asp:Button ID="btnMostrarCaracteristicasOperario" runat="server" Text="Mostrar/Ocultar características del operario" CssClass="verDatos" CausesValidation="false" OnClientClick="return false" /><!-- OnClick="btnMostrarCaracteristicasOperario_Click" -->
                    </div>
                    <div>                        
                        <asp:GridView ID="gvCaracteristicasCalidadGestor" runat="server" DataKeyNames="ID_REGISTRO, METODO_CONTROL, POSICION" AllowPaging="False"
                            Width="80%" AutoGenerateColumns="false" CssClass="GridViewASP BatzFont" GridLines="None" Caption="Características">
                            <RowStyle CssClass="RowStyle" />
                            <FooterStyle CssClass="FooterStyle" />
                            <PagerStyle CssClass="PagerStyle" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" />
                            <HeaderStyle CssClass="HeaderStyle" />
                            <EditRowStyle CssClass="EditRowStyle" />
                            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                            <EmptyDataTemplate>
                                <div style="background-color: #5D7B9D;">
                                    <asp:Label ID="lblSinCaracteristicas" runat="server" Text="No hay características para este código de operación" ForeColor="White" Font-Bold="true" />
                                </div>                                
                            </EmptyDataTemplate>
                            <Columns> 
                                <asp:TemplateField HeaderText="Id" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Visible="False" Text='<%# Eval("ID_REGISTRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="POSICION" HeaderText="Pos" />                             
                                <asp:BoundField DataField="CARAC_PARAM" HeaderText="Carac/Param" />
                                <asp:BoundField DataField="ESPECIFICACION" HeaderText="Especificación" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left">
                                            <asp:CheckBox ID="cbSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ToolTip="Seleccionar/Deseleccionar todas las características" />
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Checkbox ID="chkCaracteristica" runat="server" Checked="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ver AV" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>        
                                        <asp:HyperLink ID="hlVerAyudaVisual" runat="server" Target="_blank">
                                            <asp:Image ID="imgVerAyudaVisual"  runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Ver24.png" AlternateText="Ver" ToolTip="Ver ayuda visual de la característica" />
                                        </asp:HyperLink>                                                                                                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descargar AV" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlDescargarAyudaVisual" runat="server" NavigateUrl="../DescargarAyudaVisual.aspx?idRegistro={0}" >
                                            <asp:Image ID="imgbDescargarAyudaVisual"  runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Descargar.png" AlternateText="Descargar" ToolTip="Descargar ayuda visual de la característica" />                                   
                                        </asp:HyperLink>                                                                                                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divOperario" runat="server" class="panel" style="display:none; margin-top:20px">                     
                        <asp:GridView ID="gvCaracteristicasOperario" runat="server" DataKeyNames="ID_REGISTRO" AllowPaging="False" ShowHeader="true"
                            Width="80%" AutoGenerateColumns="false" CssClass="GridViewASP BatzFont" GridLines="None" Caption="Características del operario">
                            <RowStyle CssClass="RowStyle" />
                            <FooterStyle CssClass="FooterStyle" />
                            <PagerStyle CssClass="PagerStyle" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" />
                            <HeaderStyle CssClass="HeaderStyle" />
                            <EditRowStyle CssClass="EditRowStyle" />
                            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                            <EmptyDataTemplate>
                                <div style="background-color: #5D7B9D;">
                                    <asp:Label ID="lblSinCaracteristicas" runat="server" Text="No hay características de operario para este código de operación" ForeColor="White" Font-Bold="true" />
                                </div>
                            </EmptyDataTemplate>
                            <Columns> 
                                <asp:TemplateField HeaderText="Id" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Visible="False" Text='<%# Eval("ID_REGISTRO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="POSICION" HeaderText="Pos" />                             
                                <asp:BoundField DataField="CARAC_PARAM" HeaderText="Carac/Param" />
                                <asp:BoundField DataField="ESPECIFICACION" HeaderText="Especificación" />
                                <asp:TemplateField HeaderText="Ver AV" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>        
                                        <asp:HyperLink ID="hlVerAyudaVisual" runat="server" Target="_blank">
                                            <asp:Image ID="imgVerAyudaVisual"  runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Ver24.png" AlternateText="Ver" ToolTip="Ver ayuda visual de la característica" />
                                        </asp:HyperLink>                                                                                                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descargar AV" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlDescargarAyudaVisual" runat="server" NavigateUrl="../DescargarAyudaVisual.aspx?idRegistro={0}" >
                                            <asp:Image ID="imgbDescargarAyudaVisual"  runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Descargar.png" AlternateText="Descargar" ToolTip="Descargar ayuda visual de la característica" />                                   
                                        </asp:HyperLink>                                                                                                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>          
                                                                                                                                           
                <div style="text-align:center; width:100%; margin-top:20px; margin-bottom:30px"> 
                    <asp:Button ID="btnAceptar" Text="Realizar control" runat="server" Font-Size="18px" CssClass="Botonera" Font-Bold="true" style="text-transform:uppercase" />   
                </div>
            </asp:Panel>          
            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="ddlRoles" />
            <asp:AsyncPostBackTrigger ControlID="btnConfirmarOperacion" />
            <asp:AsyncPostBackTrigger ControlID="btnCambiarOperacion" />
        </Triggers>
    </asp:UpdatePanel>
        
    <PanelCargandoDatos:PanelCargandoDatos ID="pnlCargandoDatos" runat="server" />
</asp:Content>