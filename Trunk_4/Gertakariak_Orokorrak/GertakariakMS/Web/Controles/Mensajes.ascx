<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Mensajes.ascx.vb" Inherits="GertakariakMSWeb_Raiz.Mensajes" %>
<asp:UpdatePanel ID="upMensaje" runat="server">
	<ContentTemplate>
		<asp:HiddenField ID="hfOculto" runat="server" />
		<act:ModalPopupExtender ID="mpe_pnlMensaje" runat="server" TargetControlID="hfOculto" PopupControlID="pnlMensaje" OkControlID="imgAceptar" CancelControlID="imgCancelar" BackgroundCssClass="modalBackground">
		</act:ModalPopupExtender>

		<%--"RoundedCornersExtender" no funciona en IE8 si se combina con "ModalPopupExtender"--%>
		<%--<act:RoundedCornersExtender ID="rce_pnlMensaje" runat="server" TargetControlID="pnlMensaje" Radius="20"></act:RoundedCornersExtender>--%>

		<asp:Panel ID="pnlMensaje" runat="server" CssClass="modalBox" Style="display: none;">
			<table style="border-collapse: collapse; margin: 5px;">
				<tr class="BarraTitulo">
					<th style="text-align: left">
						<asp:Image ID="imgVentana" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon24.png" ImageAlign="Middle"/></th>
					<th style="text-align: right">
						<asp:Panel ID="pnlBotonesCabecera" runat="server" CssClass="PanelBotones">
							<asp:ImageButton ID="imgCancelar" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
						</asp:Panel>
					</th>
				</tr>
				<tr>
					<td colspan="2">
						<table>
							<tr>
								<td>
									<asp:Image ID="imgMArvin" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
								<td>
									<asp:Label ID="lblMensaje" runat="server" Text="lblMensaje" CssClass="MensajeError"></asp:Label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="BarraTitulo">
					<td style="text-align: center" colspan="2">
						<asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
							<asp:ImageButton ID="imgAceptar" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" />
						</asp:Panel>
					</td>
				</tr>
			</table>
		</asp:Panel>
	</ContentTemplate>
</asp:UpdatePanel>