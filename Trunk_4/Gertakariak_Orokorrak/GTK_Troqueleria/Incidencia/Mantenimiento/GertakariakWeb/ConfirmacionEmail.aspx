<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ConfirmacionEmail.aspx.vb" Inherits="GTK_Troqueleria.ConfirmacionEmail" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<center>
		<asp:Panel ID="Panel1" runat="server" Visible="true" BorderWidth="1" BorderColor="Gray" Width="90%">
			<asp:Label ID="Label2" runat="server" Text="Enviar correo" />
		</asp:Panel>


		<table>
			<tr>
				<td>
					<asp:CheckBox ID="chkNCProveedor" runat="server" Text="Lista de avisos (No Conformidad - Proveedores)" Checked="true" />
					<asp:ImageButton ID="img_NCProveedore" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/TipoArchivos/Mail-icon24.png" OnClientClick="return false;" ImageAlign="AbsMiddle" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:CheckBox ID="chkGestor" runat="server" Text="Gestor de Proyecto" Checked="false" />
					<asp:ImageButton ID="img_chkGestor" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/TipoArchivos/Mail-icon24.png" OnClientClick="return false;" ImageAlign="AbsMiddle" />
				</td>
			</tr>
			<%--<tr>
				<td>
					<asp:CheckBox ID="chkPedidoRetroceso" runat="server" Text="Pedido por Retroceso" Checked="true" />
					<asp:ImageButton ID="img_chkPedidoRetroceso" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/community-users-icon24.png" OnClientClick="return false;" ImageAlign="AbsMiddle"/>
				</td>
			</tr>--%>
		</table>
		<asp:UpdatePanel ID="up_pnlBotones" runat="server">
			<ContentTemplate>
				<asp:Panel ID="pnlBotones" runat="server" BorderColor="Gray" Width="90%" GroupingText="confirmarEnvio">
					<asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
					&nbsp;&nbsp;&nbsp;
					<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
				</asp:Panel>
			</ContentTemplate>
		</asp:UpdatePanel>
	</center>

	<act:PopupControlExtender ID="pce_pnl_NCProveedore" runat="server" TargetControlID="img_NCProveedore" PopupControlID="pnl_NCProveedore" Position="Right" />
	<act:PopupControlExtender ID="pce_pnl_chkGestor" runat="server" TargetControlID="img_chkGestor" PopupControlID="pnl_chkGestor" Position="Right" />
	<%--<act:PopupControlExtender ID="pce_pnl_chkPedidoRetroceso" runat="server" TargetControlID="img_chkPedidoRetroceso" PopupControlID="pnl_chkPedidoRetroceso" Position="Right" />--%>
	<asp:Panel ID="pnl_NCProveedore" runat="server" CssClass="modalBox">
		<asp:BulletedList ID="bl_chkNCProveedor" runat="server" DisplayMode="Text"></asp:BulletedList>
	</asp:Panel>
	<asp:Panel ID="pnl_chkGestor" runat="server" CssClass="modalBox" HorizontalAlign="Left">
		<asp:BulletedList ID="bl_chkGestor" runat="server" DisplayMode="Text"></asp:BulletedList>
	</asp:Panel>
	<%--<asp:Panel ID="pnl_chkPedidoRetroceso" runat="server" CssClass="modalBox">
		<asp:BulletedList ID="bl_chkPedidoRetroceso" runat="server" DisplayMode="Text"></asp:BulletedList>
	</asp:Panel>--%>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
