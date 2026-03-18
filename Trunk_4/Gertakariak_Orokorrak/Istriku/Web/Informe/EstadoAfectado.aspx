<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="EstadoAfectado.aspx.vb" Inherits="IstrikuWebRaiz.EstadoAfectado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="../Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:Panel ID="Panel1" runat="server" GroupingText="Afectado">
				<table class="GridViewASP" style="width: 1px;">
					<tr class="HeaderStyle">
						<th style="white-space: nowrap">
							<asp:Label ID="Label5" runat="server" Text="Nombre"></asp:Label>
						</th>
						<th style="white-space: nowrap">
							<asp:Label ID="Label6" runat="server" Text="Departamento"></asp:Label>
						</th>
						<th style="white-space: nowrap">
							<asp:Label ID="Label1" runat="server" Text="Responsable"></asp:Label>
						</th>
					</tr>
					<tr class="RowStyle">
						<td style="white-space: nowrap">
							<asp:Label ID="lblNombre" runat="server"></asp:Label>&nbsp;(<asp:Label ID="lblNumTrabajador" runat="server"></asp:Label>)
						</td>
						<td style="white-space: nowrap">
							<asp:Label ID="lblDepartameto" runat="server"></asp:Label>
						</td>
						<td style="white-space: nowrap">
							<asp:Label ID="lblResponsable" runat="server"></asp:Label>
						</td>
					</tr>
					<tr class="HeaderStyle">
						<th colspan="3">
							Estado
						</th>
					</tr>
					<tr class="RowStyle">
						<td colspan="3" style="text-align: center">
							<table border="0">
								<tr>
									<td style="text-align: left">
										<asp:RadioButtonList ID="rblEstado" runat="server" />
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</asp:Panel>
			<br />
			<fieldset style="text-align: center;">
				<asp:ImageButton ID="imgGuardar" runat="server" AlternateText="Guardar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" ToolTip="Guardar" />
				&nbsp;
				<asp:ImageButton ID="imgEliminar" runat="server" AlternateText="Eliminar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Eliminar24.png" ToolTip="Eliminar" />
				<act:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="imgEliminar" ConfirmText="Desea eliminar">
				</act:ConfirmButtonExtender>
			</fieldset>
			<br />
			<fieldset style="text-align: center;">
				<asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Pagina anterior" PostBackUrl="~/Informe/Detalle.aspx" />
			</fieldset>
		</ContentTemplate>
	</asp:UpdatePanel>
	<PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
	<!-- Validacion de Campos ------------------------------------------------------------------------------>
	<asp:RequiredFieldValidator ID="rfvEstado" runat="server" ErrorMessage="requerido" ControlToValidate="rblEstado" Display="None" />
	<act:ValidatorCalloutExtender ID="vce_rfvEstado" runat="server" TargetControlID="rfvEstado" />
	<!------------------------------------------------------------------------------------------------------>
</asp:Content>
