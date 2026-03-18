<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Listado.aspx.vb" Inherits="IstrikuWebRaiz.Listado" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function pageLoad() {
			$find('<%=pce_pnlBuscarAdministrador.ClientID%>').add_shown(function () { var obj = $get('<%= txtAdministrador.ClientID%>'); obj.focus(); obj.select(); });
			$find('<%=pce_pnlBuscadorUsr.ClientID%>').add_shown(function () { var obj = $get('<%= txtUsuario.ClientID%>'); obj.focus(); obj.select(); });
			$find('<%=pce_pnlBuscadorCon.ClientID%>').add_shown(function () { var obj = $get('<%= txtConsultor.ClientID%>'); obj.focus(); obj.select(); });
			$find('<%=pce_pnlBuscadorUsrAcceso.ClientID%>').add_shown(function () { var obj = $get('<%= txtUsrAcceso.ClientID%>'); obj.focus(); obj.select(); });
		}
		function Set_Adminstrador(source, eventArgs) {
			$get('<%= hdIdAdministrador.ClientID%>').value = eventArgs.get_value();
			$get('<%= btnAgregarAdministrador.ClientID%>').click();
		}
		function Set_AdminstradorPlanta(source, eventArgs) {
			$get('<%= hdIdAdministradorPlanta.ClientID%>').value = eventArgs.get_value();
			$get('<%= btnAgregarAdministradorPlanta.ClientID%>').click();
		}
		function Set_Usuario(source, eventArgs) {
			$get('<%= hfIdUsuario.ClientID%>').value = eventArgs.get_value();
			$get('<%= btnAgregarUsuario.ClientID%>').click();
		}
		function Set_Consultor(source, eventArgs) {
			$get('<%= hfIdConsultor.ClientID%>').value = eventArgs.get_value();
			$get('<%= btnAgregarConsultor.ClientID%>').click();
		}
		function Set_UsrAcceso(source, eventArgs) {
			$get('<%= hfIdUsrAcceso.ClientID%>').value = eventArgs.get_value();
			$get('<%= btnAgregarUsrAcceso.ClientID%>').click();
		}
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:Panel ID="pnlAdmin" runat="server">
        <act:CollapsiblePanelExtender ID="cpe_pnlAdministradores" runat="server" TargetControlID="pnlAdministradores" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
		ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
		CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
		ImageControlID="imgEstadoPanel" ExpandControlID="Td" CollapseControlID="Td" />
	    <table style="border: thin outset #E8E8E8; width: 50%;">
		    <tr>
			    <td id="Td" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
				    <asp:Image ID="imgEstadoPanel" runat="server" ImageAlign="AbsMiddle" />
				    <asp:Label ID="Label1" runat="server" Text="Administradores" ToolTip="Control total sobre la aplicacion"></asp:Label>
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
						    <act:AutoCompleteExtender ID="ace_txtAdministrador" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtAdministrador" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
							    ServiceMethod="get_Usuarios" OnClientItemSelected="Set_Adminstrador"
							    OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
					    </asp:Panel>
					    <asp:GridView ID="gvAdministradores" runat="server" DataKeyNames="ID" Width="1%"
						    CssClass="GridViewASP" GridLines="None"
						    AutoGenerateColumns="False" AllowSorting="True"
						    PagerSettings-Position="Bottom"
						    AllowPaging="false"
						    PagerSettings-Mode="NumericFirstLast"
						    ShowFooter="true">
						    <RowStyle CssClass="RowStyle" />
						    <FooterStyle CssClass="FooterStyle" />
						    <SelectedRowStyle CssClass="SelectedRowStyle" />
						    <HeaderStyle CssClass="HeaderStyle" />
						    <EditRowStyle CssClass="EditRowStyle" />
						    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
						    <EmptyDataRowStyle CssClass="FooterStyle" />
						    <EmptyDataTemplate>
							    <asp:Label ID="lblSinDatos" runat="server" Text="Sin Datos"></asp:Label>
						    </EmptyDataTemplate>

						    <PagerSettings Mode="NumericFirstLast" PageButtonCount="7" Position="Bottom" />
						    <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
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

    
	    <act:CollapsiblePanelExtender ID="cpe_pnlAdministradoresPlanta" runat="server" TargetControlID="pnlAdministradoresPlanta" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
		    ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
		    CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
		    ImageControlID="imgEstadoPanel_planta" ExpandControlID="Td" CollapseControlID="Td" />
	    <table style="border: thin outset #E8E8E8; width: 50%;">
		    <tr>
			    <td id="Td_adminplanta" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
				    <asp:Image ID="imgEstadoPanel_planta" runat="server" ImageAlign="AbsMiddle" />
				    <asp:Label ID="Label5" runat="server" Text="Administradores de planta" ToolTip="Control total sobre la aplicacion para su planta"></asp:Label>
			    </td>
		    </tr>
		    <tr>
			    <td style="border: thin outset #E8E8E8">
				    <asp:Panel ID="pnlAdministradoresPlanta" runat="server">
					    <asp:Panel ID="Panel5" runat="server" CssClass="PanelBotones">
						    <asp:Image ID="imgBuscarAdministradorPlanta" runat="server" class="ImageButton" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
						    <asp:Button ID="btnAgregarAdministradorPlanta" runat="server" Text="Administrador Planta" Style="display: none;" />
					    </asp:Panel>
					    <act:PopupControlExtender ID="pce_pnlBuscarAdministradorPlanta" runat="server" TargetControlID="imgBuscarAdministradorPlanta" PopupControlID="pnlBuscarAdministradorPlanta" Position="Right">
					    </act:PopupControlExtender>
					    <asp:Panel ID="pnlBuscarAdministradorPlanta" runat="server" Width="100%">
						    <asp:HiddenField ID="hdIdAdministradorPlanta" runat="server" />
						    <asp:TextBox ID="txtAdministradorPlanta" runat="server" />
						    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtAdministradorPlanta" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
						    <!-- Texto predictivo -->
						    <act:AutoCompleteExtender ID="ace_txtAdministradorPlanta" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtAdministradorPlanta" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
							    ServiceMethod="get_Usuarios" OnClientItemSelected="Set_AdminstradorPlanta"
							    OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
					    </asp:Panel>
					    <asp:GridView ID="gvAdministradoresPlanta" runat="server" DataKeyNames="ID" Width="1%"
						    CssClass="GridViewASP" GridLines="None"
						    AutoGenerateColumns="False" AllowSorting="True"
						    PagerSettings-Position="Bottom"
						    AllowPaging="false"
						    PagerSettings-Mode="NumericFirstLast"
						    ShowFooter="true">
						    <RowStyle CssClass="RowStyle" />
						    <FooterStyle CssClass="FooterStyle" />
						    <SelectedRowStyle CssClass="SelectedRowStyle" />
						    <HeaderStyle CssClass="HeaderStyle" />
						    <EditRowStyle CssClass="EditRowStyle" />
						    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
						    <EmptyDataRowStyle CssClass="FooterStyle" />
						    <EmptyDataTemplate>
							    <asp:Label ID="lblSinDatos" runat="server" Text="Sin Datos"></asp:Label>
						    </EmptyDataTemplate>

						    <PagerSettings Mode="NumericFirstLast" PageButtonCount="7" Position="Bottom" />
						    <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
						    <Columns>
							    <asp:TemplateField ShowHeader="False" ItemStyle-Width="1%">
								    <ItemTemplate>
									    <asp:ImageButton ID="btnBorrarAdmPlanta" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Eliminar" />
									    <act:ConfirmButtonExtender ID="cbe_btnBorrar" runat="server" TargetControlID="btnBorrarAdmPlanta" ConfirmText="Desea eliminar" />
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
    </asp:Panel>

	<act:CollapsiblePanelExtender ID="cpe_pnlUsuarios" runat="server" TargetControlID="pnlUsuarios" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
		ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
		CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
		ImageControlID="imgEstadoPanel_Usr" ExpandControlID="TD_Usr" CollapseControlID="TD_Usr" />
	<table style="border: thin outset #E8E8E8; width: 50%;">
		<tr>
			<td id="TD_Usr" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
				<asp:Image ID="imgEstadoPanel_Usr" runat="server" ImageAlign="AbsMiddle" />
				<asp:Label ID="Label2" runat="server" Text="Usuarios" ToolTip="Gestiona todo excepto la parte de mantenimiento de la aplicación reservada a los Administradores"></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="border: thin outset #E8E8E8">
				<asp:Panel ID="pnlUsuarios" runat="server">
					<asp:Panel ID="Panel2" runat="server" CssClass="PanelBotones">
						<asp:Image ID="imgBuscarUsr" runat="server" class="ImageButton" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
						<asp:Button ID="btnAgregarUsuario" runat="server" Text="Usuario de Gestion" Style="display: none;" />
					</asp:Panel>
					<act:PopupControlExtender ID="pce_pnlBuscadorUsr" runat="server" TargetControlID="imgBuscarUsr" PopupControlID="pnlBuscadorUsr" Position="Right">
					</act:PopupControlExtender>
					<asp:Panel ID="pnlBuscadorUsr" runat="server" Width="100%">
						<asp:HiddenField ID="hfIdUsuario" runat="server" />
						<asp:TextBox ID="txtUsuario" runat="server" />
						<act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtUsuario" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
						<!-- Texto predictivo -->
						<act:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtUsuario" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
							ServiceMethod="get_Usuarios" OnClientItemSelected="Set_Usuario"
							OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
					</asp:Panel>
					<asp:GridView ID="gvUsuarios" runat="server" DataKeyNames="ID" Width="1%"
						CssClass="GridViewASP" GridLines="None"
						AutoGenerateColumns="False" AllowSorting="True"
						PagerSettings-Position="Bottom"
						AllowPaging="false"
						PagerSettings-Mode="NumericFirstLast"
						ShowFooter="true">
						<RowStyle CssClass="RowStyle" />
						<FooterStyle CssClass="FooterStyle" />
						<SelectedRowStyle CssClass="SelectedRowStyle" />
						<HeaderStyle CssClass="HeaderStyle" />
						<EditRowStyle CssClass="EditRowStyle" />
						<AlternatingRowStyle CssClass="AlternatingRowStyle" />
						<EmptyDataRowStyle CssClass="FooterStyle" />
						<EmptyDataTemplate>
							<asp:Label ID="lblSinDatos" runat="server" Text="Sin Datos"></asp:Label>
						</EmptyDataTemplate>

						<PagerSettings Mode="NumericFirstLast" PageButtonCount="7" Position="Bottom" />
						<PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
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

	<act:CollapsiblePanelExtender ID="cpe_pnlConsultores" runat="server" TargetControlID="pnlConsultores" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
		ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
		CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
		ImageControlID="imgEstadoPanel_Con" ExpandControlID="TD_Con" CollapseControlID="TD_Con" />
	<table style="border: thin outset #E8E8E8; width: 50%;">
		<tr>
			<td id="TD_Con" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
				<asp:Image ID="imgEstadoPanel_Con" runat="server" ImageAlign="AbsMiddle" />
				<asp:Label ID="Label3" runat="server" Text="Consultores" ToolTip="Puede verlo todo pero no puede modificar"></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="border: thin outset #E8E8E8">
				<asp:Panel ID="pnlConsultores" runat="server">
					<asp:Panel ID="Panel3" runat="server" CssClass="PanelBotones">
						<asp:Image ID="imgBuscarCon" runat="server" class="ImageButton" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
						<asp:Button ID="btnAgregarConsultor" runat="server" Text="Consultor" Style="display: none;" />
					</asp:Panel>
					<act:PopupControlExtender ID="pce_pnlBuscadorCon" runat="server" TargetControlID="imgBuscarCon" PopupControlID="pnlBuscadorCon" Position="Right">
					</act:PopupControlExtender>
					<asp:Panel ID="pnlBuscadorCon" runat="server" Width="100%">
						<asp:HiddenField ID="hfIdConsultor" runat="server" />
						<asp:TextBox ID="txtConsultor" runat="server" />
						<act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtConsultor" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
						<!-- Texto predictivo -->
						<act:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtConsultor" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
							ServiceMethod="get_Usuarios" OnClientItemSelected="Set_Consultor"
							OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
					</asp:Panel>
					<asp:GridView ID="gvConsultores" runat="server" DataKeyNames="ID" Width="1%"
						CssClass="GridViewASP" GridLines="None"
						AutoGenerateColumns="False" AllowSorting="True"
						PagerSettings-Position="Bottom"
						AllowPaging="false"
						PagerSettings-Mode="NumericFirstLast"
						ShowFooter="true">
						<RowStyle CssClass="RowStyle" />
						<FooterStyle CssClass="FooterStyle" />
						<SelectedRowStyle CssClass="SelectedRowStyle" />
						<HeaderStyle CssClass="HeaderStyle" />
						<EditRowStyle CssClass="EditRowStyle" />
						<AlternatingRowStyle CssClass="AlternatingRowStyle" />
						<EmptyDataRowStyle CssClass="FooterStyle" />
						<EmptyDataTemplate>
							<asp:Label ID="lblSinDatos" runat="server" Text="Sin Datos"></asp:Label>
						</EmptyDataTemplate>

						<PagerSettings Mode="NumericFirstLast" PageButtonCount="7" Position="Bottom" />
						<PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
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

	<act:CollapsiblePanelExtender ID="cpe_pnlUsrAcceso" runat="server" TargetControlID="pnlUsrAcceso" Collapsed="false" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
		ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
		CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
		ImageControlID="imgEstadoPanel_UsrAcceso" ExpandControlID="TD_UsrAcceso" CollapseControlID="TD_UsrAcceso" />
	<table style="border: thin outset #E8E8E8; width: 50%;">
		<tr>
			<td id="TD_UsrAcceso" style="border: thin outset #E8E8E8; cursor: pointer;" class="cabeceraIzquierda">
				<asp:Image ID="imgEstadoPanel_UsrAcceso" runat="server" ImageAlign="AbsMiddle" />
				<asp:Label ID="Label4" runat="server" Text="Usuarios con Acceso" ToolTip="Usuarios que tienen acceso a la aplicacion. Solo pueden crear nuevas incidencias y ver sus incidencias."></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="border: thin outset #E8E8E8">
				<asp:Panel ID="pnlUsrAcceso" runat="server">
					<asp:Panel ID="Panel4" runat="server" CssClass="PanelBotones">
						<asp:Image ID="imgBuscarUsrAcceso" runat="server" class="ImageButton" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
						<asp:Button ID="btnAgregarUsrAcceso" runat="server" Text="Usuario con Acceso" Style="display: none;" />
					</asp:Panel>
					<act:PopupControlExtender ID="pce_pnlBuscadorUsrAcceso" runat="server" TargetControlID="imgBuscarUsrAcceso" PopupControlID="pnlBuscadorUsrAcceso" Position="Right">
					</act:PopupControlExtender>
					<asp:Panel ID="pnlBuscadorUsrAcceso" runat="server" Width="100%">
						<asp:HiddenField ID="hfIdUsrAcceso" runat="server" />
						<asp:TextBox ID="txtUsrAcceso" runat="server" />
						<act:TextBoxWatermarkExtender ID="tbwe_txtUsrAcceso" runat="server" TargetControlID="txtUsrAcceso" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
						<!-- Texto predictivo -->
						<act:AutoCompleteExtender ID="ace_txtUsrAcceso" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtUsrAcceso" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
							ServiceMethod="get_Usuarios" OnClientItemSelected="Set_UsrAcceso"
							OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
					</asp:Panel>
					<asp:GridView ID="gvUsrAcceso" runat="server" DataKeyNames="ID" Width="1%"
						CssClass="GridViewASP" GridLines="None"
						AutoGenerateColumns="False" AllowSorting="True"
						PagerSettings-Position="Bottom"
						AllowPaging="false"
						PagerSettings-Mode="NumericFirstLast"
						ShowFooter="true">
						<RowStyle CssClass="RowStyle" />
						<FooterStyle CssClass="FooterStyle" />
						<SelectedRowStyle CssClass="SelectedRowStyle" />
						<HeaderStyle CssClass="HeaderStyle" />
						<EditRowStyle CssClass="EditRowStyle" />
						<AlternatingRowStyle CssClass="AlternatingRowStyle" />
						<EmptyDataRowStyle CssClass="FooterStyle" />
						<EmptyDataTemplate>
							<asp:Label ID="lblSinDatos" runat="server" Text="Sin Datos"></asp:Label>
						</EmptyDataTemplate>

						<PagerSettings Mode="NumericFirstLast" PageButtonCount="7" Position="Bottom" />
						<PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
						<Columns>
							<asp:TemplateField ShowHeader="False" ItemStyle-Width="1%">
								<ItemTemplate>
									<asp:ImageButton ID="btnBorrar" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Eliminar" />
									<act:ConfirmButtonExtender ID="cbe_btnBorrar" runat="server" TargetControlID="btnBorrar" ConfirmText="Desea eliminar" />
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
