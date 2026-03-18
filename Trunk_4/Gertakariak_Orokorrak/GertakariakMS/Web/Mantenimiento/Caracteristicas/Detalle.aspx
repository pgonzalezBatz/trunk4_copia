<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Detalle.aspx.vb" Inherits="GertakariakMSWeb_Raiz.Detalle1" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="../../Controles/Titulo.ascx" TagName="Titulo" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<uc1:Titulo ID="Titulo1" runat="server" Texto="Detalle" />
	<asp:ImageButton ID="btnGuardar" runat="server" AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" />
	<asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Eliminar24.png" />
	<act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" />
	<asp:ImageButton ID="btnNuevaIncidencia" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" ToolTip="Nuevo" AlternateText="Nuevo" />
	<hr />
	<asp:Panel ID="Panel1" runat="server" Width="100%" HorizontalAlign="Center">
		<table style="width: 1px;" class="GridViewASP">
			<caption>
				<asp:Label ID="Label31" runat="server" Text="Caracteristica" />
			</caption>
			<tr class="HeaderStyle">
				<th style="white-space: nowrap">
					<asp:Label ID="Label1" runat="server" Text="Descripcion" />
				</th>
			</tr>
			<tr align="center" class="RowStyle">
				<td style="white-space: nowrap">
					<asp:TextBox ID="txtCaracteristica" runat="server" MaxLength="30" Width="390"></asp:TextBox>
				</td>
			</tr>
		</table>
	</asp:Panel>
	<br />
	<fieldset style="text-align: center">
		<asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Volver" />
	</fieldset>
</asp:Content>
