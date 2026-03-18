<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorOperacion.ascx.vb" Inherits="WebRaiz.SelectorOperacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>
<%--<%@ Register Assembly="IdeaSparx.CoolControls.Web" Namespace="IdeaSparx.CoolControls.Web" TagPrefix="Cool" %>--%>

<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
	<ContentTemplate>
		<table class="table2">
			<thead>
				<tr>
					<th colspan="2">
						<asp:Label runat="server" ID="lblCodOperacion" Text="Codigo operacion" Font-Size="20px"></asp:Label>
					</th>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td>
						<asp:Label runat="server" ID="lblCod" Text="cod" Font-Size="18px"></asp:Label>.&nbsp;
						<asp:TextBox ID="txtOperacion" runat="server" Columns="30" CssClass="operacion" MaxLength="25" Font-Size="18px"></asp:TextBox>                        
						<act:AutoCompleteExtender ID="ace_txtOperacion" runat="server" Enabled="True" ServicePath="~/Servicio/ServiciosWeb.asmx" ServiceMethod="BuscarOperaciones" TargetControlID="txtOperacion" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="100" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClassGrande" CompletionListItemCssClass="CompletionListItemCssClassGrande" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClassGrande" FirstRowSelected="true" ></act:AutoCompleteExtender>
					</td>
					<td style="vertical-align:middle" >
						<asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Contraer.png" ToolTip="Buscar" AlternateText="Buscar" CausesValidation="false" />
					</td>
				</tr>
			</tbody>
			<tfoot>
				<tr>
					<th colspan="2">
						<asp:Label runat="server" ID="lblDescripcion" Font-Size="14px"></asp:Label>
					</th>
				</tr>
			</tfoot>
		</table>
		<asp:Panel ID="pnlSelectorOp" runat="server" CssClass="modalBox">
			<table>
				<tr>
					<td>
						<asp:UpdatePanel ID="pnlOperaciones" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="400px" Font-Size="18px">
									<fieldset>
										<%-- User con "CommandField" "ButtonType='Link'" en IE7 para botones de seleccion pq si no la seleccion proboca 2 PostBack --%>
										<asp:GridView ID="gv_Operaciones" runat="server" AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowPaging="false" AllowSorting="false" DataKeyNames="COD_OPERACION,OPERACION_GENERAL,OPERACION_TIPO,IDSECCION" Caption="Operaciones" EmptyDataText="Sin Datos" CssClass="GridViewASP">
											<HeaderStyle CssClass="HeaderStyle" />
											<AlternatingRowStyle CssClass="AlternatingRowStyle" />
											<RowStyle CssClass="RowStyle" />
											<SelectedRowStyle CssClass="SelectedRowStyle" />
											<PagerStyle CssClass="PagerStyle" />
											<Columns>
												<asp:CommandField ButtonType="Link" ShowSelectButton="True" />
												<asp:BoundField DataField="NUM_OP" HeaderText="Orden" SortExpression="NUM_OP" ReadOnly="True" />
												<asp:BoundField DataField="COD_OPERACION" HeaderText="Codigo" SortExpression="COD_OPERACION" ReadOnly="True" />
												<asp:BoundField DataField="OPERACION_GENERAL" HeaderText="Op. General" SortExpression="OPERACION_GENERAL" />
												<asp:BoundField DataField="OPERACION_TIPO" HeaderText="Op. Tipo" SortExpression="OPERACION_TIPO" />
											</Columns>
										</asp:GridView>
									</fieldset>
								</asp:Panel>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
					<td valign="top">
						<asp:ImageButton runat="server" ID="imgCerrar" ImageUrl="~/App_Themes/Tema1/Imagenes/cerrar.gif" ImageAlign="Right" ToolTip="cerrar" AlternateText="cerrar" />
					</td>
				</tr>
			</table>
		</asp:Panel>
		<asp:Label runat="server" ID="lblHidden" Style="display: none"></asp:Label>
		<act:ModalPopupExtender ID="mpe_SelectorOp" runat="server" TargetControlID="lblHidden" PopupControlID="pnlSelectorOp" RepositionMode="RepositionOnWindowResizeAndScroll" CancelControlID="imgCerrar" BackgroundCssClass="modalBackground">
		</act:ModalPopupExtender>
	</ContentTemplate>
</asp:UpdatePanel>
<asp:Button runat="server" ID="btnComprobar" Style="visibility: hidden; position:absolute;" OnClick="ComprobarOperacion" />