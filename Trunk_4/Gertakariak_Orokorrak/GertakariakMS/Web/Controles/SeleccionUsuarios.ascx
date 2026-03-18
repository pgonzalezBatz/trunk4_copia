<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SeleccionUsuarios.ascx.vb" Inherits="GertakariakMSWeb_Raiz.SeleccionUsuarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="LocalizationLib" Namespace="LocalizationLib" TagPrefix="Loc" %>
<script type="text/javascript">
	function CargarDatos(Item) {
		var listaResp = document.getElementById(Item);
		for (var idx = 0; idx < listaResp.length;) {
			listaResp.remove(0);
		}
		listaResp.options[0] = new Option('<%="cargandoDatos".Itzuli %>...', '0');
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

	/*******************************************************/
	/*Funciones para definer el "Doble Click" en las listas*/
	/*******************************************************/
	function dobleClick_FiltroUsuarios()
	{
		var Boton = document.getElementById("<%=btnDcha.ClientID%>").name;
		__doPostBack(Boton, "");
	}
	function dobleClick_UsuariosElegidos() {
		var Boton = document.getElementById("<%=btnIzqda.ClientID%>").name;
		__doPostBack(Boton, "");
	}
	/*******************************************************/
</script>
<asp:Panel ID="pnlBusquedaUsuarios" runat="server" DefaultButton="btnBilatu">
	<asp:UpdatePanel runat="server" ID="upBusquedaUsuarios">
		<ContentTemplate>
			<table style="background-color: #F7F7F7;">
				<tr>
					<td style="width: 675px;">
						<asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<asp:Label ID="lblMensaje" runat="server" CssClass="MensajeError"></asp:Label>
							</ContentTemplate>
						</asp:UpdatePanel>
						<table style="border: 0; width: 100%">
							<tr>
								<td colspan="3">
									<asp:Label ID="Label2" CssClass="nota" Text="(Introduzca los usuarios a buscar separados por comas. Puede indicar el nombre y/o apellidos, el nombre de usuario o el codigo de trabajador)" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td style="width: 99%">
									<asp:TextBox ID="txtResponsable" runat="server" AutoCompleteType="Search" AutoPostBack="false" Width="99%" />
									<act:TextBoxWatermarkExtender ID="wmResponsables" runat="server" TargetControlID="txtResponsable" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
									<!-- Texto predictivo -->
									<act:AutoCompleteExtender ID="txtResponsable_AutoCompleteExtender" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/swSAB.asmx" ServiceMethod="Usuarios" TargetControlID="txtResponsable" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true" 
									DelimiterCharacters=";,:" ShowOnlyCurrentWordInCompletionListItem="true"/>
								</td>
								<td>&nbsp;
								</td>
								<td>
									<asp:ImageButton ID="btnBilatu" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Buscador/Flitrar.png" ToolTip="Buscar" AlternateText="Buscar" />
								</td>
							</tr>
						</table>
						<asp:Label ID="Label1" runat="server" CssClass="nota" Text="(Se pueden seleccionar varios usuarios manteniendo pulsada la tecla 'Control')"></asp:Label>
					</td>
				</tr>
				<tr>
					<td style="height: 113px">
						<table>
							<tr>
								<td>
									<asp:ListBox runat="server" ID="lsbFiltroUsuarios" Rows="6" Width="300px" AutoPostBack="false" SelectionMode="Multiple" onDblClick="dobleClick_FiltroUsuarios()" />
									<act:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="lsbFiltroUsuarios" PromptCssClass="ListSearchExtenderPrompt" PromptText="Buscar" QueryPattern="Contains" IsSorted="true" />
								</td>
								<td style="width: 50px; vertical-align: middle; text-align: center;">
									<table>
										<tr>
											<td>
												<asp:Button runat="server" ID="btnDcha" Text="-->" ToolTip="agregar"></asp:Button>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Button runat="server" ID="btnIzqda" Text="<--" ToolTip="quitar"></asp:Button>
											</td>
										</tr>
									</table>
								</td>
								<td>
									<asp:ListBox runat="server" ID="lsbUsuariosElegidos" Rows="6" Width="300px" EnableViewState="true" SelectionMode="Multiple" onDblClick="dobleClick_UsuariosElegidos()"></asp:ListBox>
									<act:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="lsbUsuariosElegidos" PromptCssClass="ListSearchExtenderPrompt" PromptText="Buscar" QueryPattern="Contains" IsSorted="true" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</ContentTemplate>
	</asp:UpdatePanel>
	<act:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server" TargetControlID="upBusquedaUsuarios">
		<Animations>
			<OnUpdating> 
				<Sequence>
					<ScriptAction Script="CargarDatos_lsbFiltroUsuarios();" />
					<EnableAction AnimationTarget="txtResponsable" Enabled="false" />
					<EnableAction AnimationTarget="upBusquedaUsuarios" Enabled="false" />					
					<FadeOut Duration=".2" Fps="30" minimumOpacity=".2" />
				</Sequence>
			</OnUpdating>
			<OnUpdated>
				<Sequence>
					<EnableAction AnimationTarget="upBusquedaUsuarios" Enabled="true" />
					<EnableAction AnimationTarget="txtResponsable" Enabled="true" />					
					<FadeIn Duration=".2" Fps="30" />
				</Sequence>
			</OnUpdated>
		</Animations>
	</act:UpdatePanelAnimationExtender>
</asp:Panel>
