<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Listado.aspx.vb" Inherits="GTK_Troqueleria.Listado" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $find('<%=pce_pnlBuscarAdministrador.ClientID%>').add_shown(function () { var obj = $get('<%= txtAdministrador.ClientID%>'); obj.focus(); obj.select(); });
		    $find('<%=pce_pnlBuscadorUsr.ClientID%>').add_shown(function () { var obj = $get('<%= txtUsuario.ClientID%>'); obj.focus(); obj.select(); });
		    $find('<%=pce_pnlBuscadorCon.ClientID%>').add_shown(function () { var obj = $get('<%= txtConsultor.ClientID%>'); obj.focus(); obj.select(); });
		}
		function Set_Adminstrador(source, eventArgs) {
		    $get('<%= hdIdAdministrador.ClientID%>').value = eventArgs.get_value();
		    $get('<%= btnAgregarAdministrador.ClientID%>').click();
		}
		function Set_Usuario(source, eventArgs) {
		    $get('<%= hfIdUsuario.ClientID%>').value = eventArgs.get_value();
		    $get('<%= btnAgregarUsuario.ClientID%>').click();
		}
        function Set_Consultor(source, eventArgs) {
			$get('<%= hdIdConsultor.ClientID%>').value = eventArgs.get_value();
			$get('<%= btnAgregarConsultor.ClientID%>').click();
		}
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <asp:Panel ID="Panel1" runat="server" CssClass="Comentario">
        <asp:Label ID="Label7" runat="server" Text="Al incluir o eliminar un usuario el icono de la intranet aparecerá  o se quitará automáticamente." />
    </asp:Panel>

    <act:CollapsiblePanelExtender ID="cpe_pnlAdministradores" runat="server" TargetControlID="pnlAdministradores" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
        ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
        CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
        ImageControlID="imgEstadoPanel" ExpandControlID="Td" CollapseControlID="Td" />
    <table style="border: thin outset #E8E8E8; width: 50%;">
        <tr>
            <td id="Td" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
                <asp:Image ID="imgEstadoPanel" runat="server" ImageAlign="AbsMiddle" />
                <asp:Label ID="Label1" runat="server" Text="Administradores"></asp:Label>

                <asp:Image ID="btnInfo_Adm" runat="server" class="ImageButton" AlternateText="Informacion" ToolTip="Informacion" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="AbsMiddle" />
                <act:BalloonPopupExtender ID="bpe_pnlInfo" runat="server" BalloonPopupControlID="pnlInfo_Adm" TargetControlID="btnInfo_Adm" Position="TopRight" BalloonSize="Small" DisplayOnMouseOver="true"></act:BalloonPopupExtender>
                <asp:Panel ID="pnlInfo_Adm" runat="server" HorizontalAlign="Left">
                    <asp:Label ID="lblDesc_Adm" runat="server" Text="Tiene acceso a toda la aplicación pudiendo modificar cualquier parte de la aplicación."></asp:Label>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="border: thin outset #E8E8E8">
                <asp:Panel ID="pnlAdministradores" runat="server">
                    <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                        <asp:Image ID="imgBuscarAdministrador" runat="server" class="ImageButton" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                        <asp:Button ID="btnAgregarAdministrador" runat="server" Text="Administrador" Style="display: none;" />
                    </asp:Panel>
                    <act:PopupControlExtender ID="pce_pnlBuscarAdministrador" runat="server" TargetControlID="imgBuscarAdministrador" PopupControlID="pnlBuscarAdministrador" Position="Right">
                    </act:PopupControlExtender>
                    <asp:Panel ID="pnlBuscarAdministrador" runat="server" Width="100%">
                        <asp:HiddenField ID="hdIdAdministrador" runat="server" />
                        <asp:TextBox ID="txtAdministrador" runat="server" />
                        <act:TextBoxWatermarkExtender ID="wmResponsables" runat="server" TargetControlID="txtAdministrador" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                        <!-- Texto predictivo -->
                        <act:AutoCompleteExtender ID="ace_txtAdministrador" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" TargetControlID="txtAdministrador" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                            ServiceMethod="get_Usuarios_Aplicacion" OnClientItemSelected="Set_Adminstrador"
                            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                    </asp:Panel>
                    <asp:GridView SkinID="GridView" ID="gvAdministradores" runat="server" DataKeyNames="ID" Width="1%">
                        <Columns>
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnBorrarAdm" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Eliminar" />
                                    <act:ConfirmButtonExtender ID="cbe_btnBorrar" runat="server" TargetControlID="btnBorrarAdm" ConfirmText="Desea eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                            <asp:TemplateField HeaderText="Nombre" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombreUsuario" runat="server" Text="?"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <act:CollapsiblePanelExtender ID="cpe_pnlUsuarios" runat="server" TargetControlID="pnlUsuarios" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
        ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
        CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
        ImageControlID="imgEstadoPanel_Usr" ExpandControlID="TD_Usr" CollapseControlID="TD_Usr" />
    <table style="border: thin outset #E8E8E8; width: 50%;">
        <tr>
            <td id="TD_Usr" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
                <asp:Image ID="imgEstadoPanel_Usr" runat="server" ImageAlign="AbsMiddle" />
                <asp:Label ID="Label2" runat="server" Text="Gestores"></asp:Label>

                <asp:Image ID="btnInfo_Usr" runat="server" class="ImageButton" AlternateText="Informacion" ToolTip="Informacion" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="AbsMiddle" />
                <act:BalloonPopupExtender ID="BalloonPopupExtender1" runat="server" BalloonPopupControlID="pnlInfo_Usr" TargetControlID="btnInfo_Usr" Position="TopRight" BalloonSize="Medium" DisplayOnMouseOver="true"></act:BalloonPopupExtender>
                <asp:Panel ID="pnlInfo_Usr" runat="server" HorizontalAlign="Left">
                    <asp:Label ID="Label6" runat="server" Text="Solo tienen acceso a las No Conformidades que se les ha asignado."></asp:Label>
                    <br />
                    <asp:Label ID="Label5" runat="server" Text="Podrán realizar el rol de:"></asp:Label>
                    <ul>
                        <li>
                            <asp:Label ID="Label3" runat="server" Text="Creadores"></asp:Label></li>
                        <li>
                            <asp:Label ID="Label4" runat="server" Text="Perseguidores"></asp:Label></li>
                    </ul>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="border: thin outset #E8E8E8">
                <asp:Panel ID="pnlUsuarios" runat="server">
                    <asp:Panel ID="Panel2" runat="server" CssClass="PanelBotones">
                        <asp:Image ID="imgBuscarUsr" runat="server" class="ImageButton" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                        <asp:Button ID="btnAgregarUsuario" runat="server" Text="Usuario" Style="display: none;" />
                    </asp:Panel>
                    <act:PopupControlExtender ID="pce_pnlBuscadorUsr" runat="server" TargetControlID="imgBuscarUsr" PopupControlID="pnlBuscadorUsr" Position="Right">
                    </act:PopupControlExtender>
                    <asp:Panel ID="pnlBuscadorUsr" runat="server" Width="100%">
                        <asp:HiddenField ID="hfIdUsuario" runat="server" />
                        <asp:TextBox ID="txtUsuario" runat="server" />
                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtUsuario" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                        <!-- Texto predictivo -->
                        <act:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" TargetControlID="txtUsuario" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                            ServiceMethod="get_Usuarios_Aplicacion" OnClientItemSelected="Set_Usuario"
                            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                    </asp:Panel>
                    <asp:GridView SkinID="GridView" ID="gvUsuarios" runat="server" DataKeyNames="ID" Width="1%">
                        <Columns>
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnBorrarAdm" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Eliminar" />
                                    <act:ConfirmButtonExtender ID="cbe_btnBorrar" runat="server" TargetControlID="btnBorrarAdm" ConfirmText="Desea eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                            <asp:TemplateField HeaderText="Nombre" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombreUsuario" runat="server" Text="?"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <act:CollapsiblePanelExtender ID="cpe_pnlConsultor" runat="server" TargetControlID="pnlConsultor" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
        ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
        CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
        ImageControlID="imgEstadoPanel_Con" ExpandControlID="TD_Con" CollapseControlID="TD_Con" />
    <table style="border: thin outset #E8E8E8; width: 50%;">
        <tr>
            <td id="TD_Con" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
                <asp:Image ID="imgEstadoPanel_Con" runat="server" ImageAlign="AbsMiddle" />
                <asp:Label ID="Label8" runat="server" Text="Consultores"></asp:Label>

                <asp:Image ID="btnInfo_Con" runat="server" class="ImageButton" AlternateText="Informacion" ToolTip="Informacion" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="AbsMiddle" />
                <act:BalloonPopupExtender ID="BalloonPopupExtender2" runat="server" BalloonPopupControlID="pnlInfo_Con" TargetControlID="btnInfo_Con" Position="TopRight" BalloonSize="Medium" DisplayOnMouseOver="true"></act:BalloonPopupExtender>
                <asp:Panel ID="pnlInfo_Con" runat="server" HorizontalAlign="Left">
                    <asp:Label ID="Label9" runat="server" Text="Podran consultar todas las No Conformidades."></asp:Label>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="border: thin outset #E8E8E8">
                <asp:Panel ID="pnlConsultor" runat="server">
                    <asp:Panel ID="Panel5" runat="server" CssClass="PanelBotones">
                        <asp:Image ID="imgBuscarCon" runat="server" class="ImageButton" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                        <asp:Button ID="btnAgregarConsultor" runat="server" Text="Usuario" Style="display: none;" />
                    </asp:Panel>
                    <act:PopupControlExtender ID="pce_pnlBuscadorCon" runat="server" TargetControlID="imgBuscarCon" PopupControlID="pnlBuscadorCon" Position="Right">
                    </act:PopupControlExtender>
                    <asp:Panel ID="pnlBuscadorCon" runat="server" Width="100%">
                        <asp:HiddenField ID="hdIdConsultor" runat="server" />
                        <asp:TextBox ID="txtConsultor" runat="server" />
                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtConsultor" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                        <!-- Texto predictivo -->
                        <act:AutoCompleteExtender ID="ace_txtConsultor" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" TargetControlID="txtConsultor" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                            ServiceMethod="get_Usuarios_Aplicacion" OnClientItemSelected="Set_Consultor"
                            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                    </asp:Panel>
                    <asp:GridView SkinID="GridView" ID="gvConsultores" runat="server" DataKeyNames="ID" Width="1%">
                        <Columns>
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnBorrarCon" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Eliminar" />
                                    <act:ConfirmButtonExtender ID="cbe_btnBorrar" runat="server" TargetControlID="btnBorrarCon" ConfirmText="Desea eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                            <asp:TemplateField HeaderText="Nombre" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombreUsuario" runat="server" Text="?"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>