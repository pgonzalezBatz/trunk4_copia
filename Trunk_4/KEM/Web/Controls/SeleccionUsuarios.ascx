<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SeleccionUsuarios.ascx.vb" Inherits="KEM.SeleccionUsuarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type="text/javascript" language="javascript">
	function CargarDatos(Item) {
		var listaResp = document.getElementById(Item);
		for (var idx = 0; idx < listaResp.length; ) {
			listaResp.remove(0);
		}
		listaResp.options[0] = new Option('Cargando datos...', '0');
	}

	function CargarDatos_lsbFiltroUsuarios() { CargarDatos("<%=Me.lsbFiltroUsuarios.ClientID%>"); }

	//Funcion para selencionar los Item de la lista.
	function Seleccionar() {
		var ListaPrincipal = document.getElementById("<%=Me.lsbFiltroUsuarios.ClientID%>");
		var ListaSecundaria = document.getElementById("<%=Me.lsbUsuariosElegidos.ClientID%>");
		while (ListaPrincipal.selectedIndex != -1) {
			if (ListaPrincipal.selectedIndex != -1) {
				var oSelect = ListaSecundaria;
				var oOption = document.createElement("OPTION");
				oOption.text = ListaPrincipal.options[ListaPrincipal.selectedIndex].text;
				oOption.value = ListaPrincipal.options[ListaPrincipal.selectedIndex].value;
				oSelect.add(oOption);
				ListaPrincipal.remove(ListaPrincipal.selectedIndex);
			}
		}
	}
</script>
<asp:Panel ID="pnlBusquedaUsuarios" runat="server" DefaultButton="btnBilatu">
	<asp:UpdatePanel runat="server" ID="upBusquedaUsuarios">
		<ContentTemplate>
			<table style="background-color: #F7F7F7;" >
				<tr>
					<td style="text-align:center;" >
						<asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<asp:Label ID="lblMensaje" runat="server" CssClass="MensajeError"></asp:Label>
							</ContentTemplate>
						</asp:UpdatePanel>
						<asp:Label ID="labelUsuario" Text="usuario" runat="server"></asp:Label>:&nbsp;&nbsp;
						<asp:TextBox ID="txtResponsable" runat="server" AutoPostBack="false" Width="200px" />
						<asp:TextBoxWatermarkExtender ID="wmResponsables" runat="server" TargetControlID="txtResponsable" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
						<asp:Button ID="btnBilatu" runat="server" Text="Buscar" ToolTip="buscarUsuarios" />
					</td>
				</tr>
				<tr>
					<td style="height: 113px">
						<table>
							<tr>
								<td>
									<asp:ListBox runat="server" ID="lsbFiltroUsuarios" Rows="6" Width="300px" AutoPostBack="false" SelectionMode="Multiple" />
									<asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="lsbFiltroUsuarios" PromptCssClass="ListSearchExtenderPrompt" PromptText="Buscar" QueryPattern="Contains" IsSorted="true"/>
								</td>
								<td style="width: 50px; vertical-align: middle; text-align: center;">
									<table>
										<tr>
											<td><asp:Button runat="server" ID="btnDcha" Text="-->" ToolTip="agregar"></asp:Button></td>
										</tr>
										<tr>
											<td><asp:Button runat="server" ID="btnIzqda" Text="<--" ToolTip="quitar"></asp:Button></td>
										</tr>
									</table>
								</td>
								<td>
									<asp:ListBox runat="server" ID="lsbUsuariosElegidos" Rows="6" Width="300px" EnableViewState="true" SelectionMode="Multiple"></asp:ListBox>
									<asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="lsbUsuariosElegidos" PromptCssClass="ListSearchExtenderPrompt" PromptText="Buscar" QueryPattern="Contains" IsSorted="true"/>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Panel>
