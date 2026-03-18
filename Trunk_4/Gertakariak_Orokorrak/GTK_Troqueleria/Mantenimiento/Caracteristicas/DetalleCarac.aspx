<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DetalleCarac.aspx.vb" Inherits="GTK_Troqueleria.DetalleCarac" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<table class="GridViewASP" style="width: 1%;">
		<thead class="HeaderStyle">
			<tr>
				<th>
					<asp:Label ID="Label2" runat="server" Text="Orden" ToolTip="Importancia"></asp:Label>
				</th>
				<th>
					<asp:Label ID="Label3" runat="server" Text="Descripcion"></asp:Label>
				</th>
			</tr>
		</thead>
		<tbody>
			<tr class="RowStyle">
				<td>
					<asp:TextBox ID="txtOrden" runat="server" Width="30" MaxLength="3"></asp:TextBox>
				</td>
				<td>
					<asp:TextBox ID="txtDescripcion" runat="server" Width="250" MaxLength="30"></asp:TextBox>
				</td>
			</tr>
		</tbody>
		<tfoot>
			<tr>
				<td colspan="2" style="text-align:center;">
					<asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
						<asp:ImageButton ID="btnAceptar" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="btnAceptar" />
						<asp:ImageButton ID="btnCancelar" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
					</asp:Panel>
				</td>
			</tr>
		</tfoot>
	</table>

	<!-- Campo Obligatorio -->
	<asp:RequiredFieldValidator ID="rfv_txtDescripcion" runat="server" ErrorMessage="debeRellenarDatos" ControlToValidate="txtDescripcion" Display="None" ValidationGroup="btnAceptar"></asp:RequiredFieldValidator>
	<act:ValidatorCalloutExtender ID="vce_rfv_txtDescripcion" runat="server" TargetControlID="rfv_txtDescripcion"></act:ValidatorCalloutExtender>
	<!-- Campo Numerico -->
	<asp:RegularExpressionValidator ID="rev_txtOrden" ControlToValidate="txtOrden" ValidationExpression="[0-9]*" Display="None" runat="server" ErrorMessage="advDebeSerNumerico" ValidationGroup="btnAceptar"></asp:RegularExpressionValidator>
	<act:ValidatorCalloutExtender ID="vce_rev_txtOrden" TargetControlID="rev_txtOrden" runat="server" />
</asp:Content>