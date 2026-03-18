<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ListadoLC.aspx.vb" Inherits="GTK_Troqueleria.ListadoLC" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    	<script type="text/javascript">
		function SelectAll(e) {
			var SeleccionarTodo;
			var gvId = e.parentElement.parentElement.parentElement.parentElement.parentElement.id;
			//get reference of GridView control
			var grid = document.getElementById(gvId);
			//variable to contain the cell of the grid
			var cell;

			if (grid.rows.length > 0) {
				//loop starts from 1. rows[0] points to the header.
				for (i = 1; i < grid.rows.length; i++) {
					//get the reference of first column
					cell = grid.rows[i].cells[0];

					//loop according to the number of childNodes in the cell
					for (j = 0; j < cell.childNodes.length; j++) {
						//if childNode type is CheckBox                 
						if (cell.childNodes[j].type == "checkbox") {
							//assign the status of the Select All checkbox to the cell checkbox within the grid
							if (typeof (SeleccionarTodo) == "undefined") { SeleccionarTodo = !(cell.childNodes[j].checked); }
							cell.childNodes[j].checked = SeleccionarTodo;
						}
					}
				}
			}
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº" />

	<table class="GridViewASP" style="width: 1%;">
		<tr class="HeaderStyle">
			<th style="white-space: nowrap; width: 1%; text-align: right;">
				<asp:Label ID="Label3" runat="server" Text="Nº Pedido Origen" />:</th>
			<td class="RowStyle">
				<asp:DropDownList runat="server" ID="ddlPedidosOrig" AppendDataBoundItems="true" DataValueField="Value" DataTextField="Text" ToolTip="Depende del proveedor y OF-OP-Marca seleccionados en la NC.">
					<asp:ListItem Text="(Seleccione uno)" Value=""></asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<tr class="HeaderStyle">
			<th style="white-space: nowrap; width: 1%; text-align: right;">
				<asp:Label ID="Label1" runat="server" Text="OFM" ToolTip="Orden de Fabricacion - Marca" />:</th>
			<td class="RowStyle" style="white-space: nowrap; width: 1%; text-align: left;">
				<asp:Label ID="lblOFM" runat="server" /></td>
		</tr>
	</table>

	<!-- Buscador ------------------------------------------------------------------------->
	<asp:UpdatePanel ID="upBuscador" runat="server">
		<ContentTemplate>
			<asp:Panel ID="pnlBuscador" runat="server" DefaultButton="imgFiltrar">
				<center>
					<table class="tablaBuscador">
						<tr>
							<td>
								<asp:Panel runat="server" ID="pnlFiltroCabecera">
									<table class="recuadro">
										<tr class="ImageButton">
											<td style="text-align: right;">
												<asp:TextBox ID="txtBuscar" runat="server" Width="98%" AutoCompleteType="Search" ToolTip="Buscar" Style="min-width: 100px"></asp:TextBox>
												<act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtBuscar" WatermarkText="Buscar" WatermarkCssClass="TextBoxWatermarkExtender">
												</act:TextBoxWatermarkExtender>
											</td>
											<td style="width: 1%; white-space: nowrap;">&nbsp;<asp:ImageButton ID="imgFiltrar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Buscar" AlternateText="Buscar" />
												&nbsp;<asp:ImageButton ID="imgEliminarFiltro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/EliminarFlitro24.png" AlternateText="eliminarFiltros" ToolTip="eliminarFiltros" />
											</td>
										</tr>
									</table>
								</asp:Panel>
							</td>
						</tr>
					</table>
				</center>
			</asp:Panel>
		</ContentTemplate>
	</asp:UpdatePanel>
	<!------------------------------------------------------------------------------------->
	<hr />

	<act:TabContainer ID="tabC" runat="server" ActiveTabIndex="0">
		<act:TabPanel runat="server" ID="tp_Materiales" HeaderText="costeMaterial">
			<ContentTemplate>
				<asp:Panel ID="pnlMateriales" runat="server" CssClass="PanelBotones">
					<asp:ImageButton ID="btnAgregarMateriales" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Agregar seleccionados" ToolTip="Agregar seleccionados" />
				</asp:Panel>
				<asp:GridView ID="gvMaterial" SkinID="GridView" runat="server" DataKeyNames="NUMPEDLIN,NUMLINLIN" Caption="costeMaterial">
					<Columns>
						<asp:TemplateField>
							<ItemStyle HorizontalAlign="Center" />
							<HeaderTemplate>
								<asp:Panel ID="pnlMateriales" runat="server" CssClass="PanelBotones">
									<asp:ImageButton ID="btnMarcarTodo" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/CheckBox/check-box-icon16.png" ToolTip="Marcar todo" AlternateText="Marcar todo"
										OnClientClick="SelectAll(this); return false;" />
								</asp:Panel>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox ID="chkRow" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="OFM" AccessibleHeaderText="OFM" SortExpression="OFM">
							<ItemTemplate>
								<asp:Label ID="lblOFM" runat="server" />
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Left" Wrap="False" />
						</asp:TemplateField>
						<asp:BoundField DataField="CODPROLIN" HeaderText="Proveedor" SortExpression="CODPROLIN">
							<ItemStyle HorizontalAlign="Center" />
						</asp:BoundField>
						<asp:TemplateField HeaderText="Proveedor" AccessibleHeaderText="Proveedor" SortExpression="Proveedor">
							<ItemTemplate>
								<asp:Label ID="lblProveedor" runat="server" />
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Left" />
						</asp:TemplateField>
						<asp:BoundField DataField="DESCART" HeaderText="Descripcion" SortExpression="DESCART">
							<ItemStyle HorizontalAlign="Left" />
						</asp:BoundField>
						<asp:BoundField DataField="numpedlin" HeaderText="numPed" SortExpression="numpedlin">
							<ItemStyle HorizontalAlign="Center" />
						</asp:BoundField>
						<asp:TemplateField HeaderText="albaran" AccessibleHeaderText="albaran">
							<ItemTemplate>
								<asp:Label ID="lblAlbaran" runat="server" />
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" />
						</asp:TemplateField>
						<asp:BoundField DataField="NUMLINLIN" HeaderText="numLin" SortExpression="NUMLINLIN">
							<ItemStyle HorizontalAlign="Center" />
						</asp:BoundField>
						<asp:BoundField DataField="CANPED" HeaderText="Cantidad" SortExpression="CANPED">
							<ItemStyle HorizontalAlign="Right" />
						</asp:BoundField>
						<asp:BoundField DataField="FECENTVIG" HeaderText="fecha" SortExpression="FECENTVIG" DataFormatString="{0:d}" />
					</Columns>
				</asp:GridView>
			</ContentTemplate>
		</act:TabPanel>
		<act:TabPanel runat="server" ID="tp_Bonos" HeaderText="costeBonosInternos">
			<ContentTemplate>
				<asp:Panel ID="pnlBonos" runat="server" CssClass="PanelBotones">
					<asp:ImageButton ID="btnAgregarBonos" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Agregar seleccionados" ToolTip="Agregar seleccionados" />
				</asp:Panel>
				<asp:GridView ID="gvBonos" SkinID="GridView" runat="server" DataKeyNames="NUMBON" Caption="costeBonosInternos">
					<Columns>
						<asp:TemplateField>
							<ItemStyle HorizontalAlign="Center" />
							<HeaderTemplate>
								<asp:Panel ID="pnlBonos" runat="server" CssClass="PanelBotones">
									<asp:ImageButton ID="btnMarcarTodo" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/CheckBox/check-box-icon16.png" ToolTip="Marcar todo" AlternateText="Marcar todo"
										OnClientClick="SelectAll(this); return false;" />
								</asp:Panel>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox ID="chkRow" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="OFM" AccessibleHeaderText="OFM" SortExpression="OFM" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
							<ItemTemplate>
								<asp:Label ID="lblOFM" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<%--<asp:BoundField DataField="CPSECCIO.DESCSECCIO" Visible="TRUE" HeaderText="Descripcion" SortExpression="CPSECCIO.DESCSECCIO" />--%>
						<asp:TemplateField HeaderText="Descripcion" AccessibleHeaderText="Descripcion" SortExpression="CPSECCIO.DESCSECCIO" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Label ID="lblDESCSECCIO" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="TIEMPO" Visible="TRUE" HeaderText="Horas" SortExpression="TIEMPO" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="ECOSBON" Visible="TRUE" HeaderText="Importe" SortExpression="ECOSBON" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="FECHA" Visible="TRUE" HeaderText="fecha" SortExpression="FECHA" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
					</Columns>
				</asp:GridView>
			</ContentTemplate>
		</act:TabPanel>
		<act:TabPanel runat="server" ID="tp_SubContratacion" HeaderText="costeSubcontratacion">
			<ContentTemplate>
				<asp:Panel ID="pnlSubContratacion" runat="server" CssClass="PanelBotones">
					<asp:ImageButton ID="btnAgregarSubContratacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Agregar seleccionados" ToolTip="Agregar seleccionados" />
				</asp:Panel>
				<asp:GridView ID="gvSubcontratacion" SkinID="GridView" runat="server" DataKeyNames="NUMPEDLIN,NUMLINLIN" Caption="costeSubcontratacion">
					<Columns>
						<asp:TemplateField>
							<ItemStyle HorizontalAlign="Center" />
							<HeaderTemplate>
								<asp:Panel ID="pnlSubContratacion" runat="server" CssClass="PanelBotones">
									<asp:ImageButton ID="btnMarcarTodo" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/CheckBox/check-box-icon16.png" ToolTip="Marcar todo" AlternateText="Marcar todo"
										OnClientClick="SelectAll(this); return false;" />
								</asp:Panel>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox ID="chkRow" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="OFM" AccessibleHeaderText="OFM" SortExpression="OFM">
							<ItemTemplate>
								<asp:Label ID="lblOFM" runat="server" />
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Left" Wrap="False" />
						</asp:TemplateField>
						<%--<asp:BoundField DataField="scpedcab.codproext" HeaderText="Proveedor" SortExpression="scpedcab.codproext" ItemStyle-HorizontalAlign="Center" />--%>
						<asp:TemplateField HeaderText="Proveedor" AccessibleHeaderText="Proveedor" SortExpression="scpedcab.codproext" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Label ID="lblcodproext" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<%--<asp:BoundField DataField="scpedcab.GCPROVEE.NOMPROV" HeaderText="Proveedor" SortExpression="scpedcab.GCPROVEE.NOMPROV" />--%>
						<asp:TemplateField HeaderText="Proveedor" AccessibleHeaderText="Proveedor" SortExpression="scpedcab.GCPROVEE.NOMPROV">
							<ItemTemplate>
								<asp:Label ID="lblNOMPROV" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="DESCRILIN" HeaderText="descripcion" SortExpression="DESCRILIN" />
						<asp:BoundField DataField="numpedlin" HeaderText="numPed" SortExpression="numpedlin" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="horaped" HeaderText="horas" SortExpression="horaped" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="eimpreclin" HeaderText="Importe Recibido" SortExpression="eimpreclin" ItemStyle-HorizontalAlign="Center" />
						<%--<asp:BoundField DataField="scpedcab.FECENTEXT" HeaderText="fecha" SortExpression="scpedcab.FECENTEXT" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />--%>
						<asp:TemplateField HeaderText="fecha" AccessibleHeaderText="fecha" SortExpression="scpedcab.FECENTEXT" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Label ID="lblFECENTEXT" runat="server" DataFormatString="{0:d}" />
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>
			</ContentTemplate>
		</act:TabPanel>
	</act:TabContainer>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
