<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="SelectorUsuarios.aspx.vb" Inherits="GertakariakMSWeb_Raiz.SelectorUsuarios" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/SeleccionUsuarios.ascx" TagName="SeleccionUsuarios" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<uc1:Titulo ID="Titulo1" runat="server" />
	<table style="width: 100%;">
		<tr>
			<td></td>
			<td style="width: 1px;">
			<!-- No se puede meter directamente el control de usuario "SeleccionUsuarios" en la celda pq para IE7/IE8 el "width: 1px;" hace desaparecer al control. -->
				<table>
					<tr>
						<td>
							<uc2:SeleccionUsuarios ID="suAdmin" runat="server" Vigentes="true" Trabajador="UsuariosBatz" />
						</td>
					</tr>
				</table>
			</td>
			<td></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<fieldset style="text-align: center;">
					<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" />
				</fieldset>
			</td>
			<td></td>
		</tr>
	</table>
	<fieldset style="text-align: center">
		<asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Volver" />
	</fieldset>
</asp:Content>
