<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="BuscarUsuarios.aspx.vb" Inherits="GertakariakMSWeb_Raiz.BuscarUsuarios" %>

<%@ Register Src="~/Controles/SeleccionUsuarios.ascx" TagName="SeleccionUsuarios" TagPrefix="ControlUsuario" %>
<%--<%@ Register Src="~/Controles/SeleccionUsuarios2.ascx" TagName="SeleccionUsuarios2" TagPrefix="ControlUsuario" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	Este control solo busca usuarios (no busca proveedores).
	<br />
	Deja a los proveedores fuera de esta busqueda para hacerla mas rapida.
	<ControlUsuario:SeleccionUsuarios runat="server" ID="selusuariosRespNC" Trabajador="Todos" Vigentes="true" TipoSeleccion="Multiple" />

	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:ImageButton ID="btnBuscarResponsables" runat="server" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo24.png" />
			<asp:Panel ID="pnlSelectorUsuario" runat="server" Style="display: none;" CssClass="modalBox">
				<table border="0px">
					<tr>
						<td>
							<fieldset style="text-align: right;">
								<asp:ImageButton ID="btnCerrarPanel" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
							</fieldset>
						</td>
					</tr>
					<tr>
						<td>
							<ControlUsuario:SeleccionUsuarios id="SeleccionUsuarios" runat="server" vigentes="true" trabajador="UsuariosBatz" />
						</td>
					</tr>
					<tr align="center">
						<td>
							<fieldset style="text-align: center;">
								<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" />
							</fieldset>
						</td>
					</tr>
				</table>
			</asp:Panel>
			<act:ModalPopupExtender ID="mpe_SelectorUsuarios" runat="server" CancelControlID="btnCerrarPanel" PopupControlID="pnlSelectorUsuario" TargetControlID="btnBuscarResponsables" BackgroundCssClass="modalBackground" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
