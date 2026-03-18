<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="DatosGenerales.aspx.vb" Inherits="GertakariakMSWeb_Raiz.DatosGenerales" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/SeleccionUsuarios.ascx" TagName="SeleccionUsuarios" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<asp:UpdatePanel runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<uc1:Titulo ID="TituloPagina" runat="server" Texto="Nuevo" />
			<table style="width: 1px" class="GridViewASP">
				<caption>
					<asp:Label ID="Label11" runat="server" Text="fechas" />
				</caption>
				<tr class="HeaderStyle">
					<th style="white-space: nowrap">
						<asp:Label ID="Label1" runat="server" Text="FechaApertura" />
					</th>
					<th style="white-space: nowrap" class="GridViewASP">
						<asp:Label ID="Label2" runat="server" Text="Fecha de Solución" />
					</th>
					<th style="white-space: nowrap">
						<asp:Label ID="Label3" runat="server" Text="Demora" />
						(<asp:Label ID="Label4" runat="server" Text="semanas" />)
					</th>
				</tr>
				<tr align="center" class="RowStyle">
					<td style="white-space: nowrap">
						<asp:TextBox ID="txtFechaApertura" runat="server" Width="85px"></asp:TextBox>
						<act:CalendarExtender ID="txtFechaApertura_CalendarExtender" runat="server" TargetControlID="txtFechaApertura" />
						<act:CalendarExtender ID="imgCalendario_CalendarExtender" runat="server" TargetControlID="txtFechaApertura" PopupButtonID="imgCalendario" />
						&nbsp;<asp:ImageButton ID="imgCalendario" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
					</td>
					<td style="white-space: nowrap">
						<asp:TextBox ID="txtFechaCierre" runat="server" Width="85px"></asp:TextBox>
						<act:CalendarExtender ID="txtFechaCierre_CalendarExtender" runat="server" TargetControlID="txtFechaCierre" />
						<act:CalendarExtender ID="imgCalendario2_CalendarExtender" runat="server" TargetControlID="txtFechaCierre" PopupButtonID="imgCalendario2" />
						&nbsp;<asp:ImageButton ID="imgCalendario2" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
					</td>
					<td style="white-space: nowrap">
						<asp:Label ID="lblDemora" runat="server" />
					</td>
				</tr>
			</table>
			<br />
			<table class="GridViewASP">
				<caption>
					<asp:Label ID="Label9" runat="server" Text="incidencia"></asp:Label>
				</caption>
				<tr class="HeaderStyle">
					<th style="white-space: nowrap">
						<asp:Label runat="server" Text="familia" ID="Label6" />
					</th>
					<td class="RowStyle" style="white-space: nowrap">
						<asp:DropDownList ID="ddlFamilia" runat="server" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="AssetName" DataValueField="Asset">
							<asp:ListItem></asp:ListItem>
						</asp:DropDownList>
					</td>
					<th style="white-space: nowrap">
						<asp:Label runat="server" Text="Instalación" ID="Label8" />
					</th>
					<td class="RowStyle" style="white-space: nowrap">
						<asp:DropDownList ID="ddlInstalacion" runat="server" AppendDataBoundItems="false" DataTextField="AssetName" DataValueField="Asset">
						</asp:DropDownList>
					</td>
					<th style="white-space: nowrap">
						<asp:Label ID="Label7" runat="server" Text="tipoIncidencia" />
					</th>
					<td class="RowStyle" style="white-space: nowrap">
						<asp:BulletedList ID="blTipoIncidencia" runat="server" ToolTip="tipoIncidencia" DisplayMode="LinkButton" />
						<fieldset runat="server" id="fsTipoIncidencia" style="text-align: center;">
							<asp:ImageButton ID="btnTipoIncidencia" runat="server" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" />
						</fieldset>
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th style="white-space: nowrap" rowspan="2">
						<asp:Label ID="Label5" runat="server" Text="Descripcion problema"></asp:Label>
					</th>
					<td class="RowStyle" style="white-space: nowrap" colspan="3" rowspan="2">
						<asp:TextBox ID="txtDescripcionProblema" runat="server" Rows="6" Width="99%" TextMode="MultiLine" />
					</td>
					<th colspan="2" style="white-space: nowrap">
						<asp:Label ID="Label10" runat="server" Text="responsables"></asp:Label>
					</th>
				</tr>
				<tr class="HeaderStyle">
					<td class="RowStyle" style="white-space: nowrap" colspan="2">
						<asp:BulletedList ID="blResponsables" runat="server" />
						<fieldset style="text-align: center;">
							<asp:ImageButton ID="btnBuscarResponsables" runat="server" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" />
						</fieldset>
					</td>
				</tr>
			</table>
			<fieldset style="text-align: center;">
				<asp:ImageButton ID="btnGuardar" runat="server" AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" CausesValidation="true" ValidationGroup="btnGuardar" />
			</fieldset>
			<br />
			<fieldset style="text-align: center">
				<%--<asp:Button ID="btnVolver" runat="server" Text="Volver" PostBackUrl="~/Default.aspx" />--%>
				<asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Volver" />
			</fieldset>
			<asp:Panel ID="pnlSelectorUsuario" runat="server" Style="display: none;" CssClass="modalBox">
				<table border="0px">
					<tr>
						<td>
							<fieldset style="text-align: right;">
								<asp:ImageButton ID="btnCerrarPanel" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Cancelar24.png" />
							</fieldset>
						</td>
					</tr>
					<tr>
						<td>
							<uc2:SeleccionUsuarios ID="SeleccionUsuarios" runat="server" Vigentes="true" Trabajador="UsuariosBatz" />
						</td>
					</tr>
					<tr align="center">
						<td>
							<fieldset style="text-align: center;">
								<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" />
							</fieldset>
						</td>
					</tr>
				</table>
			</asp:Panel>
			<act:ModalPopupExtender ID="mpe_SelectorUsuarios" runat="server" CancelControlID="btnCerrarPanel" PopupControlID="pnlSelectorUsuario" TargetControlID="btnBuscarResponsables" BackgroundCssClass="modalBackground" />
			<asp:Panel ID="pnlTipoIncidencia" runat="server" Style="display: none;" CssClass="modalBox">
				<table border="0px">
					<tr>
						<td>
							<fieldset style="text-align: right;">
								<asp:ImageButton ID="btnCerrarPanel_SU" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Cancelar24.png" />
							</fieldset>
						</td>
					</tr>
					<tr>
						<td>
							<asp:TreeView ID="tvEstructura" runat="server" ShowLines="True" BorderColor="Black" BorderWidth="1" BorderStyle="Solid" Width="100%" ExpandDepth="1">
								<NodeStyle ForeColor="Black" />
								<%--
								<LevelStyles>
										<asp:TreeNodeStyle Font-Underline="false" />
								</LevelStyles>
								--%>
								<RootNodeStyle ForeColor="WindowText" BackColor="ActiveCaption" Width="100%" />
								<ParentNodeStyle ForeColor="WindowText" BackColor="#C9D7E7" Width="100%" />
								<%--<LeafNodeStyle BackColor="red" />--%>
								<SelectedNodeStyle Font-Bold="true" BorderColor="#8585E2" BorderStyle="Double" BorderWidth="2" />
								<HoverNodeStyle BorderColor="ActiveBorder" ForeColor="#F4F4F4" Font-Bold="true" BorderStyle="Outset" BorderWidth="3" BackColor="#345374" />
							</asp:TreeView>
						</td>
					</tr>
					<tr align="center">
						<td>
							<fieldset style="text-align: center;">
								<%--<asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" />--%>
							</fieldset>
						</td>
					</tr>
				</table>
			</asp:Panel>
			<%--<act:ModalPopupExtender ID="mpe_TipoIncidencia" runat="server" CancelControlID="btnCerrarPanel_SU" PopupControlID="pnlTipoIncidencia" TargetControlID="btnTipoIncidencia" BackgroundCssClass="modalBackground" />--%>
			<act:ModalPopupExtender ID="mpe_TipoIncidencia" runat="server" CancelControlID="btnCerrarPanel_SU" PopupControlID="pnlTipoIncidencia" TargetControlID="btnTipoIncidencia" BackgroundCssClass="modalBackground" />
			<PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
	<!------------------------------------------------------------------------------------------------------>
	<!-- Validacion de Campos ------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha de Apertura Obligatorio --------------------------------------------------------------------->
	<asp:RequiredFieldValidator ID="rfvFechaApertura" runat="server" ErrorMessage="requerido" ControlToValidate="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rfvFechaApertura" runat="server" TargetControlID="rfvFechaApertura" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha de Apertura solo fecha ---------------------------------------------------------------------->
	<asp:CompareValidator ID="cvFechaApertura" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaApertura" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaApertura" runat="server" TargetControlID="cvFechaApertura" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha de Cierre solo fecha ------------------------------------------------------------------------>
	<asp:CompareValidator ID="cvFechaCierre" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaCierre" runat="server" TargetControlID="cvFechaCierre" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- FechaInicio <= FechaCierre ------------------------------------------------------------------------>
	<asp:CompareValidator ID="cvFechaCierre2" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Operator="GreaterThanEqual" ControlToCompare="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaCierre2" runat="server" TargetControlID="cvFechaCierre2" />
	<!------------------------------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
</asp:Content>
