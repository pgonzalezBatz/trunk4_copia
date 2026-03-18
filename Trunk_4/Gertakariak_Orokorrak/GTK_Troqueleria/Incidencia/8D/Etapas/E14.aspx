<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="E14.aspx.vb" Inherits="GTK_Troqueleria.E14" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function pageLoad(sender, args) {
			//$(document).ready(function () { $get("<=pnlEtapa_1.ClientID%>").parentElement.style.height = "auto"; });
			$(document).ready(function () { $get("<%=pnlEtapa_2.ClientID%>").parentElement.style.height = "auto"; });
			$(document).ready(function () { $get("<%=pnlEtapa_3.ClientID%>").parentElement.style.height = "auto"; });
<%--			$(document).ready(function () { $get("<%=pnlEtapa_4.ClientID%>").parentElement.style.height = "auto"; });--%>
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<asp:UpdatePanel ID="upE14" runat="server">
		<ContentTemplate>
			<ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº" />
			<table class="GridViewASP">
				<thead class="HeaderStyle">
					<tr>
						<th>
							<asp:Label ID="Label7" runat="server" Text="Fecha Inicio"></asp:Label></th>
						<th>
							<asp:Label ID="Label8" runat="server" Text="Fecha Fin (Previsto)"></asp:Label></th>
						<th>
							<asp:Label ID="Label9" runat="server" Text="Fecha Solicitud de Aprobacion"></asp:Label></th>
						<th>
							<asp:Label ID="Label32" runat="server" Text="Fecha Aprobación"></asp:Label></th>
					</tr>
				</thead>
				<tbody class="RowStyle">
					<tr>
						<td style="text-align: center;">
							<%--<asp:TextBox ID="txtFechaInicio" runat="server" Width="0" Style="visibility: hidden;" BorderStyle="None" BorderWidth="0"></asp:TextBox>
							<asp:Label ID="lblFechaInicio" runat="server"></asp:Label>--%>
                            <asp:TextBox ID="txtFechaInicio" runat="server" Width="85px"></asp:TextBox>
                            <act:CalendarExtender ID="ce_txtFechaInicio" runat="server" TargetControlID="txtFechaInicio" />
                            <act:CalendarExtender ID="ce_imgFechaInicio" runat="server" TargetControlID="txtFechaInicio" PopupButtonID="imgFechaInicio" />
                            &nbsp;<asp:ImageButton ID="imgFechaInicio" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
						</td>
						<td style="text-align: center;">
							<asp:TextBox ID="txtFechaFin" runat="server" Width="85px"></asp:TextBox>
							<act:CalendarExtender ID="ce_txtFechaFin" runat="server" TargetControlID="txtFechaFin" />
							<act:CalendarExtender ID="ce_imgFechaFin" runat="server" TargetControlID="txtFechaFin" PopupButtonID="imgFechaFin" />
							&nbsp;<asp:ImageButton ID="imgFechaFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
						</td>
						<td style="text-align: center;">
							<asp:TextBox ID="txtFechaCierre" runat="server" Width="85px"></asp:TextBox>
							<act:CalendarExtender ID="ce_txtFechaCierre" runat="server" TargetControlID="txtFechaCierre" />
							<act:CalendarExtender ID="ce_imgFechaCierre" runat="server" TargetControlID="txtFechaCierre" PopupButtonID="imgFechaCierre" />
							&nbsp;<asp:ImageButton ID="imgFechaCierre" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
						</td>
						<td style="text-align: center;">
							<asp:TextBox ID="txtFechaValidacion" runat="server" Width="85px"></asp:TextBox>
							<act:CalendarExtender ID="ce_txtFechaValidacion" runat="server" TargetControlID="txtFechaValidacion" />
							<act:CalendarExtender ID="ce_imgFechaVal" runat="server" TargetControlID="txtFechaValidacion" PopupButtonID="imgFechaVal" />
							&nbsp;<asp:ImageButton ID="imgFechaVal" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
						</td>
					</tr>
				</tbody>
			</table>

			<act:CollapsiblePanelExtender ID="cpeEtapa_2" runat="server" TargetControlID="pnlEtapa_2" ExpandControlID="pnlCollapsed_E2" CollapseControlID="pnlCollapsed_E2" Collapsed="false" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
				ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
				CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
				ImageControlID="imgEstadoPanel_E2" CollapsedText="Expandir" ExpandedText="Contraer" />
			<act:CollapsiblePanelExtender ID="cpeEtapa_3" runat="server" TargetControlID="pnlEtapa_3" ExpandControlID="pnlCollapsed_E3" CollapseControlID="pnlCollapsed_E3" Collapsed="false" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
				ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
				CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
				ImageControlID="imgEstadoPanel_E3" CollapsedText="Expandir" ExpandedText="Contraer" />
			<act:CollapsiblePanelExtender ID="cpeEtapa_4" runat="server" TargetControlID="pnlEtapa_4" ExpandControlID="pnlCollapsed_E4" CollapseControlID="pnlCollapsed_E4" Collapsed="false" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
				ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
				CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
				ImageControlID="imgEstadoPanel_E4" CollapsedText="Expandir" ExpandedText="Contraer" />
			<table class="tablaBuscador" style="width: 100%;">

                <%--<tr>
					<th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
						<asp:Panel runat="server" ID="pnlCollapsed_E4">
							<table class="recuadro">
								<tr class="ImageButton">
									<td>
										<asp:Image runat="server" ID="imgEstadoPanel_E4" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
										<asp:Label ID="lblEtapa_4" runat="server" Text="Etapa 2 - Acciones de contención/inmediatas"></asp:Label>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</th>
				</tr>
				<tr>
					<td>
						<asp:Panel ID="pnlEtapa_4" runat="server" CssClass="recuadro">
							<table class="GridViewASP">
								<caption>
									<asp:Label ID="Label53" runat="server" Text="Acciones de contencion/inmediatas"></asp:Label></caption>
								<tr class="HeaderStyle">
									<th colspan="3">
										<asp:Label ID="Label54" runat="server" Text="¿Qué acciones de contención/inmediatas han sido llevadas a cabo para garantizar la continuidad de la fabricación del troquel?"></asp:Label></th>
								</tr>
								<tr class="HeaderStyle">
									<th style="width: 1%; white-space: nowrap;">
										<asp:Label ID="Label55" runat="server" Text="Considerar:"></asp:Label></th>
									<th colspan="2">
										<asp:Label ID="Label56" runat="server" Text="Acciones tomadas"></asp:Label></th>
								</tr>
								<tr class="AlternatingRowStyle">
									<th align="left" style="white-space: nowrap;">
										<asp:Label ID="Label61" runat="server" Text="Detalles de las acciones de contención/inmediatas:"></asp:Label></th>
									<th align="left" style="white-space: nowrap; width: 1%;">
										<asp:Label ID="Label62" runat="server" Text="Fecha de cierre de acciones inmediatas"></asp:Label></th>
									<td>
										<asp:TextBox ID="txt_E4_DESCRIPCION_6" runat="server" Width="99%" MaxLength="1000" /></td>
								</tr>
								<tr class="RowStyle">
									<td rowspan="3">
										<asp:TextBox ID="txt_E4_DESCRIPCION_5" runat="server" Width="99%" TextMode="MultiLine" Rows="5" /></td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>--%>

				<tr>
					<th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
						<asp:Panel runat="server" ID="pnlCollapsed_E2">
							<table class="recuadro">
								<tr class="ImageButton">
									<td>
										<asp:Image runat="server" ID="imgEstadoPanel_E2" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
										<asp:Label ID="lblEtapa_2" runat="server" Text="Etapa 3 - Otros productos o procesos afectados"></asp:Label>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</th>
				</tr>
				<tr>
					<td>
						<asp:Panel ID="pnlEtapa_2" runat="server" CssClass="recuadro">
							<table class="GridViewASP">
								<caption>
									<asp:Label ID="Label41" runat="server" Text="¿Puede este problema afectar a otros proyectos u OFs?"></asp:Label>
								</caption>
								<tr class="HeaderStyle">
									<th style="width: 1%;"></th>
									<th style="width: 1%;">
										<asp:Label ID="Label42" runat="server" Text="Si"></asp:Label></th>
									<th style="width: 1%;">
										<asp:Label ID="Label43" runat="server" Text="No"></asp:Label></th>
									<th align="center">
										<asp:Label ID="Label45" runat="server" Text="Comentarios/Resultados"></asp:Label></th>
								</tr>
								<tr class="RowStyle">
									<th align="left" style="white-space: nowrap;">
										<asp:Label ID="Label46" runat="server" Text="Otras Plantas/Clientes afectados"></asp:Label></th>
									<td align="center">
										<asp:CheckBox ID="cb_E2_AFECTAR1_S" runat="server" /></td>
									<td align="center">
										<asp:CheckBox ID="cb_E2_AFECTAR1_N" runat="server" /></td>
									<td>
										<asp:TextBox ID="txt_E2_AFECTAR1" runat="server" MaxLength="1000" Width="99%"></asp:TextBox></td>
								</tr>
								<tr class="RowStyle">
									<th align="left" style="white-space: nowrap;">
										<asp:Label ID="Label48" runat="server" Text="Otras OF del mismo proyecto"></asp:Label></th>
									<td align="center">
										<asp:CheckBox ID="cb_E2_AFECTAR2_S" runat="server" /></td>
									<td align="center">
										<asp:CheckBox ID="cb_E2_AFECTAR2_N" runat="server" /></td>
									<td style="white-space: nowrap;">
										<asp:TextBox ID="txt_E2_AFECTAR2" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>
								<tr class="RowStyle">
									<th align="left" style="white-space: nowrap;">
										<asp:Label ID="Label51" runat="server" Text="Otros proyectos"></asp:Label></th>
									<td align="center">
										<asp:CheckBox ID="cb_E2_AFECTAR3_S" runat="server" /></td>
									<td align="center">
										<asp:CheckBox ID="cb_E2_AFECTAR3_N" runat="server" /></td>
									<td style="white-space: nowrap;">
										<asp:TextBox ID="txt_E2_AFECTAR3" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
						<asp:Panel runat="server" ID="pnlCollapsed_E3">
							<table class="recuadro">
								<tr class="ImageButton">
									<td>
										<asp:Image runat="server" ID="imgEstadoPanel_E3" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
										<asp:Label ID="lblEtapa_3" runat="server" Text="Etapa 4 - Primer análisis del problema"></asp:Label>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</th>
				</tr>
				<tr>
					<td>
						<asp:Panel ID="pnlEtapa_3" runat="server" CssClass="recuadro">
							<table class="GridViewASP" style="width: 100%;">
								<tr class="HeaderStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label86" runat="server" Text="¿Dónde se debería haber detectado esta No Conformidad?"></asp:Label></th>
									<th>
										<asp:Label ID="Label87" runat="server" Text="Si"></asp:Label></th>
									<th>
										<asp:Label ID="Label88" runat="server" Text="No"></asp:Label></th>
                                    <th align="center">
										<asp:Label ID="Label4" runat="server" Text="Comentarios/Resultados"></asp:Label></th>
								</tr>
								<tr class="RowStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label89" runat="server" Text="Simulación"></asp:Label></th>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS1_S" runat="server"/></td>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS1_N" runat="server"/></td>
                                    <td style="white-space: nowrap; width:100%;">
										<asp:TextBox ID="txt_E3_ANALISIS_DESC_1" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>
								<tr class="AlternatingRowStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label90" runat="server" Text="Revision Modelos"></asp:Label></th>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS2_S" runat="server"/></td>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS2_N" runat="server"/></td>
                                    <td style="white-space: nowrap; width:100%;">
										<asp:TextBox ID="txt_E3_ANALISIS_DESC_2" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>
								<tr class="RowStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label91" runat="server" Text="Revisión Diseños"></asp:Label></th>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS3_S" runat="server"/></td>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS3_N" runat="server"/></td>
                                    <td style="white-space: nowrap; width:100%;">
										<asp:TextBox ID="txt_E3_ANALISIS_DESC_3" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>
								<tr class="AlternatingRowStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label92" runat="server" Text="Proveedor TC / Homologación TC"></asp:Label></th>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS4_S" runat="server"/></td>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS4_N" runat="server"/></td>
                                    <td style="white-space: nowrap; width:100%;">
										<asp:TextBox ID="txt_E3_ANALISIS_DESC_4" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>

                                <tr class="AlternatingRowStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label1" runat="server" Text="Fabricación / Mecanizado"></asp:Label></th>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS5_S" runat="server"/></td>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS5_N" runat="server"/></td>
                                    <td style="white-space: nowrap; width:100%;">
										<asp:TextBox ID="txt_E3_ANALISIS_DESC_5" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>
                                <tr class="AlternatingRowStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label2" runat="server" Text="Control de Calidad (medición de pieza)"></asp:Label></th>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS6_S" runat="server"/></td>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS6_N" runat="server"/></td>
                                    <td style="white-space: nowrap; width:100%;">
										<asp:TextBox ID="txt_E3_ANALISIS_DESC_6" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>
                                <tr class="AlternatingRowStyle">
									<th style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label3" runat="server" Text="Homologación final"></asp:Label></th>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS7_S" runat="server"/></td>
									<td>
										<asp:CheckBox ID="cb_E3_ANALISIS7_N" runat="server"/></td>
                                    <td style="white-space: nowrap; width:100%;">
										<asp:TextBox ID="txt_E3_ANALISIS_DESC_7" runat="server" MaxLength="1000" Width="99%" /></td>
								</tr>

								<tr class="HeaderStyle">
									<th colspan="3" style="text-align: left; white-space: nowrap;">
										<asp:Label ID="Label93" runat="server" Text="¿Cuáles son las razones de la no detección?"></asp:Label></th>
								</tr>
								<tr class="AlternatingRowStyle">
									<td colspan="3">
										<asp:TextBox ID="txt_E3_DESCRIPCION" runat="server" Width="99%" TextMode="MultiLine" Rows="5" />
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>				
			</table>

			<div style="text-align: center">
				<asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
					<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" />
				</asp:Panel>
			</div>

			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E2_AFECTAR1_S" runat="server" TargetControlID="cb_E2_AFECTAR1_S" Key="E2_AFECTAR1" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E2_AFECTAR1_N" runat="server" TargetControlID="cb_E2_AFECTAR1_N" Key="E2_AFECTAR1" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E2_AFECTAR2_S" runat="server" TargetControlID="cb_E2_AFECTAR2_S" Key="E2_AFECTAR2" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E2_AFECTAR2_N" runat="server" TargetControlID="cb_E2_AFECTAR2_N" Key="E2_AFECTAR2" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E2_AFECTAR3_S" runat="server" TargetControlID="cb_E2_AFECTAR3_S" Key="E2_AFECTAR3" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E2_AFECTAR3_N" runat="server" TargetControlID="cb_E2_AFECTAR3_N" Key="E2_AFECTAR3" />

			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS1_S" runat="server" TargetControlID="cb_E3_ANALISIS1_S" Key="E3_ANALISIS1" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS1_N" runat="server" TargetControlID="cb_E3_ANALISIS1_N" Key="E3_ANALISIS1" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS2_S" runat="server" TargetControlID="cb_E3_ANALISIS2_S" Key="E3_ANALISIS2" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS2_N" runat="server" TargetControlID="cb_E3_ANALISIS2_N" Key="E3_ANALISIS2" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS3_S" runat="server" TargetControlID="cb_E3_ANALISIS3_S" Key="E3_ANALISIS3" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS3_N" runat="server" TargetControlID="cb_E3_ANALISIS3_N" Key="E3_ANALISIS3" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS4_S" runat="server" TargetControlID="cb_E3_ANALISIS4_S" Key="E3_ANALISIS4" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS4_N" runat="server" TargetControlID="cb_E3_ANALISIS4_N" Key="E3_ANALISIS4" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS5_S" runat="server" TargetControlID="cb_E3_ANALISIS5_S" Key="E3_ANALISIS5" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS5_N" runat="server" TargetControlID="cb_E3_ANALISIS5_N" Key="E3_ANALISIS5" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS6_S" runat="server" TargetControlID="cb_E3_ANALISIS6_S" Key="E3_ANALISIS6" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS6_N" runat="server" TargetControlID="cb_E3_ANALISIS6_N" Key="E3_ANALISIS6" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS7_S" runat="server" TargetControlID="cb_E3_ANALISIS7_S" Key="E3_ANALISIS7" />
			<act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E3_ANALISIS7_N" runat="server" TargetControlID="cb_E3_ANALISIS7_N" Key="E3_ANALISIS7" />
		</ContentTemplate>
	</asp:UpdatePanel>

	<!------------------------------------------------------------------------------------------------------>
	<!-- Validacion de Campos ------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
	<!-- Validacion solo fecha ----------------------------------------------------------------------------->
	<%--<asp:CompareValidator ID="cv_txtFechaInicio" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaInicio" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_cv_txtFechaInicio" runat="server" TargetControlID="cv_txtFechaInicio" />--%>
	<asp:CompareValidator ID="cv_txtFechaFin" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cv_txtFechaFin" runat="server" TargetControlID="cv_txtFechaFin" />
	<asp:CompareValidator ID="cv_txtFechaCierre" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cv_txtFechaCierre" runat="server" TargetControlID="cv_txtFechaCierre" />
	<!------------------------------------------------------------------------------------------------------>

	<!-- txtFechaInicio <= txtFechaFin / txtFechaCierre ---------------------------------------------------->
	<asp:CompareValidator ID="cv_txtFechaInicio2" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Operator="GreaterThanEqual" ControlToCompare="txtFechaInicio" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cv_txtFechaInicio2" runat="server" TargetControlID="cv_txtFechaInicio2" />

	<asp:CompareValidator ID="cv_txtFechaInicio3" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Operator="GreaterThanEqual" ControlToCompare="txtFechaInicio" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cv_txtFechaInicio3" runat="server" TargetControlID="cv_txtFechaInicio3" />
	<!------------------------------------------------------------------------------------------------------>

	<!-- Validacion MaxLength ------------------------------------------------------------------------------>
	<asp:RegularExpressionValidator ID="rev_txt_E4_DESCRIPCION_5" runat="server" ControlToValidate="txt_E4_DESCRIPCION_5" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rev_txt_E4_DESCRIPCION_5" TargetControlID="rev_txt_E4_DESCRIPCION_5" runat="server" PopupPosition="TopLeft" />
	<asp:RegularExpressionValidator ID="rev_txt_E3_DESCRIPCION" runat="server" ControlToValidate="txt_E3_DESCRIPCION" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rev_txt_E3_DESCRIPCION" TargetControlID="rev_txt_E3_DESCRIPCION" runat="server" PopupPosition="TopLeft" />
	<!------------------------------------------------------------------------------------------------------>
</asp:Content>
