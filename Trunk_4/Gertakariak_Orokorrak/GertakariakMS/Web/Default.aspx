<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Default.aspx.vb" Inherits="GertakariakMSWeb_Raiz._Default1" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHEAD" runat="server">
	<script type="text/javascript">
		function pageLoad(sender, args) { $(document).ready(function () { $get("<%=pnlFiltro.ClientID%>").parentElement.style.height = "auto"; }); }

		function Conmutador(chk) {
			var collapse = chk.checked;
			var components = Sys.Application.getComponents()
			for (var i in components) {
				var exp = new RegExp("cpeAcciones");
				var match = exp.exec(components[i].get_id())
				if (Sys.Extended.UI.CollapsiblePanelBehavior.isInstanceOfType(components[i])) {
					if (match && match.length > 0) {
						components[i].set_Collapsed(collapse);
					}
				}
			}
		}
		/**************************************************************************************************************/
		/* Seleccion automatica de subnodos                                                                           */
		/**************************************************************************************************************/
		function OnTreeClick(evt) {
			var src = window.event != window.undefined ? window.event.srcElement : evt.target;
			var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
			if (isChkBoxClick) {
				var parentTable = GetParentByTagName("table", src);
				var nxtSibling = parentTable.nextSibling;
				if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
				{
					if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
					{
						//check or uncheck children at all levels
						CheckUncheckChildren(parentTable.nextSibling, src.checked);
					}
				}
				//check or uncheck parents at all levels
				//CheckUncheckParents(src, src.checked);
			}
		}
		function CheckUncheckChildren(childContainer, check) {
			var childChkBoxes = childContainer.getElementsByTagName("input");
			var childChkBoxCount = childChkBoxes.length;
			for (var i = 0; i < childChkBoxCount; i++) {
				childChkBoxes[i].checked = check;
			}
		}
		//utility function to get the container of an element by tagname
		function GetParentByTagName(parentTagName, childElementObj) {
			var parent = childElementObj.parentNode;
			while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
				parent = parent.parentNode;
			}
			return parent;
		}
		/**************************************************************************************************************/
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<!-- Buscador ------------------------------------------------------------------------->
	<%--<asp:UpdatePanel ID="upBuscador" runat="server">
		<ContentTemplate>--%>
	<%--<act:CollapsiblePanelExtender ID="cpeFiltro" runat="server" TargetControlID="pnlFiltro" ExpandControlID="imgEstadoPanel" CollapseControlID="imgEstadoPanel" Collapsed="True" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblBusquedaAvanz" ExpandedImage="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Extender.png" CollapsedImage="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Contraer.png" ImageControlID="imgEstadoPanel" CollapsedText="Busqueda Avanzada" ExpandedText="Busqueda Avanzada" />--%>
	<act:CollapsiblePanelExtender ID="cpeFiltro" runat="server" TargetControlID="pnlFiltro" ExpandControlID="imgEstadoPanel" CollapseControlID="imgEstadoPanel" Collapsed="True" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblBusquedaAvanz"
		ExpandedImage="~/App_Themes/Batz/IconosAcciones/Buscador/OpcionesCerrar24.png"
		CollapsedImage="~/App_Themes/Batz/IconosAcciones/Buscador/Opciones24.png"
		ImageControlID="imgEstadoPanel" CollapsedText="Busqueda Avanzada" ExpandedText="Busqueda Avanzada" />
	<asp:Panel ID="pnlBuscador" runat="server" DefaultButton="imgFiltrar" HorizontalAlign="Center">
		<center>
			<table class="tablaBuscador">
				<tr>
					<td>
						<asp:Panel runat="server" ID="pnlFiltroCabecera">
							<table class="recuadro">
								<tr class="ImageButton">
									<td style="text-align: right;">
										<asp:TextBox ID="txtBuscar" runat="server" Width="98%" AutoCompleteType="Search" ToolTip="Buscar" Style="min-width: 100px"></asp:TextBox>
										<act:TextBoxWatermarkExtender ID="tbwe_txtBuscar" runat="server" TargetControlID="txtBuscar" WatermarkText="Buscar" WatermarkCssClass="TextBoxWatermarkExtender">
										</act:TextBoxWatermarkExtender>
									</td>
									<td style="width: 1%; white-space: nowrap;">&nbsp;<asp:ImageButton ID="imgFiltrar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Buscar" AlternateText="Buscar" ValidationGroup="imgFiltrar" />
										&nbsp;<asp:ImageButton ID="imgEliminarFiltro" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Buscador/EliminarFiltro24.png" AlternateText="eliminarFiltros" ToolTip="eliminarFiltros" />
									</td>
									<td style="width: 1%">
										<asp:ImageButton ID="imgEstadoPanel" runat="server" />
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Panel ID="pnlFiltro" runat="server" CssClass="recuadro">
							<table style="width: 100%;">
								<tr>
									<td>
										<asp:Panel ID="pnlCaracteristicas" runat="server" GroupingText="Caracteristicas" ToolTip="Caracteristicas" HorizontalAlign="Left" Width="98%">
											<asp:DataList ID="dlCaracteristicas" runat="server" GridLines="None" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4" Width="96%">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
												<ItemTemplate>
													<asp:TreeView ID="tvCaracteristicas" runat="server" ShowLines="true" ExpandDepth="1"
														onclick="OnTreeClick(event)">
														<RootNodeStyle ForeColor="WindowText" BackColor="ActiveCaption" Width="100%" />
													</asp:TreeView>
												</ItemTemplate>
											</asp:DataList>
										</asp:Panel>
									</td>
									<td>
										<asp:Panel ID="pnlEstado" runat="server" GroupingText="estadoincidencia" ToolTip="estadoincidencia" Width="98%">
											<asp:RadioButtonList ID="rblEstados" runat="server" RepeatDirection="Horizontal" />
										</asp:Panel>
										<asp:Panel ID="pnlFechas" runat="server" GroupingText="Busqueda por Fechas" ToolTip="Busqueda por Fechas" Width="98%">
											<asp:Panel ID="pnlFechaRevision" runat="server" GroupingText="Fecha de Revisión" ToolTip="Fecha Prevista de Cierre" Width="98%">
												<asp:Label ID="Label2" runat="server" Text="Fecha Inicio"></asp:Label>:
								<asp:TextBox ID="txtFechaRevision" runat="server" Width="85px"></asp:TextBox>
												<act:CalendarExtender ID="txtFechaRevision_CalendarExtender" runat="server" TargetControlID="txtFechaRevision" />
												<act:CalendarExtender ID="imgFechaRevision_CalendarExtender" runat="server" TargetControlID="txtFechaRevision" PopupButtonID="imgFechaRevision" />
												&nbsp;<asp:ImageButton ID="imgFechaRevision" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />

												&nbsp;&nbsp;&nbsp;&nbsp;

								<asp:Label ID="Label5" runat="server" Text="Fecha Fin"></asp:Label>:
								<asp:TextBox ID="txtFechaRevisionFin" runat="server" Width="85px"></asp:TextBox>
												<act:CalendarExtender ID="txtFechaRevisionFin_CalendarExtender" runat="server" TargetControlID="txtFechaRevisionFin" />
												<act:CalendarExtender ID="imgFechaRevisionFin_CalendarExtender" runat="server" TargetControlID="txtFechaRevisionFin" PopupButtonID="imgFechaRevisionFin" />
												&nbsp;<asp:ImageButton ID="imgFechaRevisionFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
											</asp:Panel>
										</asp:Panel>
									</td>
								</tr>
							</table>
							<asp:Panel ID="pnlFamilias" runat="server" GroupingText="Familias" ToolTip="Familias" HorizontalAlign="Left" Width="98%">
								<asp:DataList ID="dlFamilias" runat="server" GridLines="None" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4" Width="96%">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
									<ItemTemplate>
										<asp:TreeView ID="tvFamilias" runat="server" ShowLines="true" ExpandDepth="1" ShowCheckBoxes="Parent,Leaf"
											onclick="OnTreeClick(event)">
											<RootNodeStyle ForeColor="WindowText" BackColor="ActiveCaption" Width="100%" />
										</asp:TreeView>
									</ItemTemplate>
								</asp:DataList>
							</asp:Panel>
							<asp:Panel ID="pnlResponsables" runat="server" GroupingText="responsables" ToolTip="responsables" HorizontalAlign="Left" Width="98%">
								<asp:CheckBoxList ID="cblResponsables" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" TextAlign="Right" RepeatLayout="Table" />
							</asp:Panel>
							<hr />
							<center>
								<table style="width: auto; border-collapse: collapse;">
									<tr>
										<td>
											<asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
												<asp:ImageButton ID="imgFiltrar2" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Buscar" AlternateText="Buscar" ValidationGroup="imgFiltrar" />
												<%--<asp:ImageButton ID="btnGuardarFiltro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Floppy-Small-icon24.png" ToolTip="Guardar Filtro como predeterminado" AlternateText="Guardar Filtro como predeterminado" ValidationGroup="imgFiltrar" />--%>
											</asp:Panel>
										</td>
									</tr>
								</table>
							</center>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</center>
	</asp:Panel>
	<%--</ContentTemplate>
	</asp:UpdatePanel>--%>
	<!------------------------------------------------------------------------------------->
	<asp:Label ID="LokalizedLabel1" runat="server" Text="numRegistros" CssClass="negrita"></asp:Label>:&nbsp;
	<asp:Label runat="server" CssClass="negrita" ID="lblNumRegistros"></asp:Label>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<%--<asp:HiddenField ID="txtExpandedDivs" runat="server" />--%>
			<asp:GridView ID="gvGertakariak" runat="server" AutoGenerateColumns="False" AllowSorting="True" RowHeaderColumn="ID" DataKeyNames="ID" CssClass="GridViewASP" Caption="incidencias" GridLines="None" PagerSettings-Position="Bottom" AllowPaging="True" PageSize="20" PagerSettings-Mode="NumericFirstLast" ShowFooter="false">
				<Columns>
					<asp:TemplateField>
						<HeaderTemplate>
							<asp:CheckBox ID="chkPaneles" runat="server" Checked="true" onclick="Conmutador(this);" CssClass="chkPaneles_CSS" ToolTip="Expande/Contrae todos los paneles de acciones." />
							<act:ToggleButtonExtender ID="tbe_chkPaneles" runat="server" TargetControlID="chkPaneles"
								ImageWidth="24" ImageHeight="24"
								CheckedImageAlternateText="Expande todos los paneles de acciones." CheckedImageUrl="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Contraer.png"
								UncheckedImageAlternateText="Contrae todos los paneles de acciones." UncheckedImageUrl="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Extender.png" />
						</HeaderTemplate>
						<ItemTemplate>
							<act:CollapsiblePanelExtender ID="cpeAcciones" runat="server" TargetControlID="pnlAcciones" ExpandControlID="imgEstadoPanel_Acciones" CollapseControlID="imgEstadoPanel_Acciones" ImageControlID="imgEstadoPanel_Acciones" Collapsed="True" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true" ExpandedImage="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Extender.png" CollapsedImage="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Contraer.png" CollapsedText="acciones" ExpandedText="acciones" />
							<asp:ImageButton ID="imgEstadoPanel_Acciones" runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Width="1px" />
					<asp:TemplateField HeaderText="Estado" AccessibleHeaderText="Estado" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<asp:Image ID="imgEstado" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/Estado/lock-disabled-icon24.png" ToolTip="Cerrada" Visible="false" />
							<asp:Panel ID="pnlEstado" runat="server" Visible="false" BackColor="IndianRed">
								<asp:ImageButton ID="btnCerrar" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="~/App_Themes/Batz/Imagenes/Estado/lock-off-icon24.png" AlternateText="Cerrar Tarea" />
								<act:ConfirmButtonExtender ID="cbe_btnCerrar" runat="server" TargetControlID="btnCerrar" ConfirmText="¿Cerrar Tarea?" />
							</asp:Panel>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Instalacion" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Middle">
						<ItemTemplate>
							<asp:Label ID="lblInstalacion" runat="server"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DescripcionProblema" HeaderText="Descripcion problema" SortExpression="DescripcionProblema" ItemStyle-Width="100%" />
					<asp:TemplateField HeaderText="responsables" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Middle">
						<ItemTemplate>
							<asp:BulletedList ID="blResponsables" runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Desarrollo" AccessibleHeaderText="Desarrollo" ItemStyle-Wrap="false">
						<ItemTemplate>
							<%--<div title='<%# Eval("Avance") %>%' style="background-color: #CCFFCC; margin: 0px; padding: 0px; width: 100%; height: 10px; text-align: left;">
								<hr style="background-color: Green; height: 10px; width: <%# Eval("Avance") %>%; float: left;" />
							</div>--%>
							<%--<div title='50%' style="background-color: #CCFFCC; height:10px;width:100%; margin:0px;padding:0px;border:0px;text-align:left;">
								<hr style="background-color: Green;			   height:10px;width:50%;  margin:0px;padding:0px;border:0px;text-align:left;" />
							</div>--%>
							<asp:Panel ID="pnlDes_Cont" runat="server" BackColor="#CCFFCC" Width="100%" Height="10px">
								<asp:Panel ID="pnlDes_Bar" runat="server" BackColor="Green" Width="0%" Height="10px" BorderWidth="0" BorderStyle="None" />
							</asp:Panel>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<tr>
								<td colspan="100%">
									<asp:Panel ID="pnlAcciones" runat="server">
										<table>
											<td style="white-space: nowrap">&nbsp;&nbsp;</td>
											<td style="background-color: #E8F3FF; border: thin outset #E8E8E8">
												<asp:GridView ID="gvAcciones" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" OnRowCreated="gvAcciones_RowCreated"
													Width="100%">
													<EmptyDataTemplate>
														<asp:Label ID="Label1" runat="server" Text="Sin Datos"></asp:Label>
													</EmptyDataTemplate>
													<Columns>
														<asp:TemplateField>
															<ItemTemplate>
																<asp:CheckBox ID="chkAccion" runat="server" Visible="false" />
															</ItemTemplate>
														</asp:TemplateField>
														<asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
														<asp:BoundField DataField="FechaPrevista" HeaderText="Fecha Prevista de Cierre" DataFormatString="{0:d}" />
														<asp:BoundField DataField="FechaFin" HeaderText="FechaFin" DataFormatString="{0:d}" />
														<asp:BoundField DataField="Descripcion" HeaderText="descripcion" />
														<%--<asp:BoundField DataField="Demora" HeaderText="Demora" />--%>
														<asp:TemplateField HeaderText="Demora">
															<ItemTemplate>
																<asp:Label ID="lblDemora" runat="server" />
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField HeaderText="Observación">
															<%--<HeaderTemplate>
														<asp:Label ID="lblObservacion" runat="server" Text="Observación"></asp:Label>
														<asp:ImageButton ID="btnNuevaObservacion" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" ToolTip="Nueva Observacion" AlternateText="Nueva Observacion" ImageAlign="AbsMiddle" OnClick="btnNuevaObservacion_Click" />
													</HeaderTemplate>--%>
															<ItemTemplate>
																<asp:Label ID="lblObservacion" runat="server"></asp:Label>
																<asp:ImageButton ID="btnNuevaObservacion" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" ToolTip="Nueva Observacion" AlternateText="Nueva Observacion" ImageAlign="AbsMiddle" OnClick="btnNuevaObservacion_Click" />
															</ItemTemplate>
														</asp:TemplateField>
													</Columns>
												</asp:GridView>
											</td>
										</table>
									</asp:Panel>
								</td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							<tr>
								<td colspan="100%" style="text-align: center;">
									<asp:Button ID="btnPanelFecha" runat="server" Text="Cambiar Fecha Prevista de Cierre" />
									<asp:Panel ID="pnlSelectorUsuario" runat="server" Style="display: none;" CssClass="modalBox">
										<table style="border-width: 0px">
											<tr>
												<td>
													<fieldset style="text-align: right;">
														<asp:ImageButton ID="btnCerrarPanel" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Cancelar24.png" />
													</fieldset>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="Label2" runat="server" Text="Fecha Prevista de Cierre"></asp:Label>:
													<asp:TextBox ID="txtFechaRevision" runat="server" Width="85px"></asp:TextBox>
													<act:CalendarExtender ID="txtFechaRevision_CalendarExtender" runat="server" TargetControlID="txtFechaRevision" />
													<act:CalendarExtender ID="imgFechaRevision_CalendarExtender" runat="server" TargetControlID="txtFechaRevision" PopupButtonID="imgFechaRevision" />
													&nbsp;<asp:ImageButton ID="imgFechaRevision" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
													<!-- Fecha de Revision solo fecha ---------------------------------------------------------------------->
													<asp:CompareValidator ID="cvFechaRevision" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaRevision" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="imgFiltrar" />
													<act:ValidatorCalloutExtender ID="vce_cvFechaRevision" runat="server" TargetControlID="cvFechaRevision" />
													<!------------------------------------------------------------------------------------------------------>
												</td>
											</tr>
											<tr style="text-align: center">
												<td>
													<fieldset style="text-align: center;">
														<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" OnClick="btnFechaRevision_Click" />
													</fieldset>
												</td>
											</tr>
										</table>
									</asp:Panel>
									<act:ModalPopupExtender ID="mpe_SelectorUsuarios" runat="server" CancelControlID="btnCerrarPanel" PopupControlID="pnlSelectorUsuario" TargetControlID="btnPanelFecha" BackgroundCssClass="modalBackground" />
								</td>
							</tr>
							<tr>
								<td colspan="100%">
									<table style="border: solid 1px #000000;">
										<tr>
											<td style="border: 1px; border-color: Black; border-style: solid; background-color: #FF0000; color: #333333;">&nbsp;
											</td>
											<td>
												<asp:Label ID="Label8" runat="server" Text="Abierta" />
											</td>
											<td style="border: 1px; border-color: Black; border-style: solid; background-color: #FFFFFF; color: #333333;">&nbsp;
											</td>
											<td>
												<asp:Label ID="Label3" runat="server" Text="Cerrada" />
											</td>
											<td style="border: 1px; border-color: Black; border-style: solid; background-color: #E2DED6; color: #333333;">&nbsp;
											</td>
											<td>
												<asp:Label ID="Label4" runat="server" Text="seleccionada" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</FooterTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</ContentTemplate>
	</asp:UpdatePanel>
	<PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- Validacion de Campos ------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha de Revision solo fecha ---------------------------------------------------------------------->
	<asp:CompareValidator ID="cvFechaRevision" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaRevision" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="imgFiltrar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaRevision" runat="server" TargetControlID="cvFechaRevision" />
	<asp:CompareValidator ID="cvFechaRevisionFin" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaRevisionFin" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="imgFiltrar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaRevisionFin" runat="server" TargetControlID="cvFechaRevisionFin" />
	<!------------------------------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
</asp:Content>
