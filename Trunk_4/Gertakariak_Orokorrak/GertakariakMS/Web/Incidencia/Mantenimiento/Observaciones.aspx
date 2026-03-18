<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Observaciones.aspx.vb" Inherits="GertakariakMSWeb_Raiz.Observaciones" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<uc1:Titulo ID="TituloPagina" runat="server" Texto="observaciones" />

			<table id="Table1" runat="server" class="GridViewASP" title="observaciones" summary="observaciones">
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label26" runat="server" Text="Usuario" />
					</th>
					<td class="RowStyle">
						<asp:Label ID="lblUsuario" runat="server" />
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label1" runat="server" Text="Fecha" />
					</th>
					<td class="RowStyle">
						<asp:Label ID="lblFecha" runat="server" />
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label3" runat="server" Text="Descripcion" />
					</th>
					<td class="RowStyle">
						<asp:TextBox ID="txtDescripcion" runat="server" Rows="5" TextMode="MultiLine" Width="99%" />
					</td>
				</tr>
			</table>

			<fieldset style="text-align: center;">
				<asp:ImageButton ID="btnGuardar" runat="server" AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" CausesValidation="true" ValidationGroup="btnGuardar" />
				&nbsp;<asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Eliminar24.png" />
			</fieldset>
			<br />
			<fieldset style="text-align: center">
				<asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Volver" />
			</fieldset>
			<PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>