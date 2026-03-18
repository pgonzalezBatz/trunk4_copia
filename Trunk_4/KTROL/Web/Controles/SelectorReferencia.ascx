<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorReferencia.ascx.vb" Inherits="WebRaiz.SelectorReferencia" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>
<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2">
            <thead>
                <tr>
                    <th colspan="2">
                        <asp:Label ID="lblTitulo" runat="server" Font-Size="20px"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <asp:Label ID="lblRef" runat="server" Text="ref" Font-Size="18px"></asp:Label>
                        &nbsp;
                        <asp:TextBox ID="txtReferencia" runat="server" Columns="30" MaxLength="25" Font-Size="18px"></asp:TextBox>
                        <AjaxControl:AutoCompleteExtender ID="ace_txtReferencia" runat="server" CompletionInterval="100" CompletionListCssClass="CompletionListCssClassGrande" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClassGrande" CompletionListItemCssClass="CompletionListItemCssClassGrande" CompletionSetCount="0" EnableCaching="true" Enabled="True" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="BuscarReferencias" ServicePath="~/Servicio/ServiciosWeb.asmx" TargetControlID="txtReferencia" UseContextKey="True">
                        </AjaxControl:AutoCompleteExtender>
                    </td>
                    <td style="text-align:center">
                        <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Buscar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Contraer.png" ToolTip="Buscar" />
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <th colspan="2">
                        <asp:Label ID="lblDescripcion" runat="server" Font-Size="14px"></asp:Label>
                    </th>
                </tr>
            </tfoot>
        </table>
        <asp:Panel ID="pnlSelectorRef" runat="server" CssClass="modalBox">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="pnlPiezaUso" runat="server" EnableViewState="true" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>
                                    <asp:Label ID="Label2" runat="server" Text="Planta"></asp:Label>
                                    :
                                    <asp:DropDownList ID="ddl_Lantegis" runat="server" AppendDataBoundItems="true" AutoPostBack="True" DataSourceID="LinqDataSource_Lantegis" DataTextField="LANTEGI" DataValueField="LANTEGI">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:LinqDataSource ID="LinqDataSource_Lantegis" runat="server" ContextTypeName="KaPlanLib.DAL.ELL" OrderBy="LANTEGI, TODOS" TableName="M_LANTEGI">
                                    </asp:LinqDataSource>
									<hr />
									<asp:GridView ID="gv_Articulos" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Caption="Articulos" CellPadding="4" CssClass="GridViewASP" DataKeyNames="CODIGO,DENOMINACION" EmptyDataText="Sin Datos" GridLines="None" PagerSettings-Mode="NumericFirstLast" Font-Size="14px">
                                        <HeaderStyle CssClass="HeaderStyle" />
                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                        <RowStyle CssClass="RowStyle" />
                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <Columns>
											<%-- User con "CommandField" "ButtonType='Link'" en IE7 para botones de seleccion pq si no la seleccion proboca 2 PostBack --%>
											<asp:CommandField ButtonType="Link" ShowSelectButton="True" />
                                            <asp:BoundField DataField="CODIGO" HeaderText="Codigo" ReadOnly="True" SortExpression="CODIGO" />
                                            <asp:BoundField DataField="REFERENCIA_CLIENTE" HeaderText="Ref. Cliente" SortExpression="REFERENCIA_CLIENTE" />
                                            <asp:BoundField DataField="DENOMINACION" HeaderText="Denominacion" SortExpression="DENOMINACION" />
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>
                                <asp:LinqDataSource ID="LinqDataSource_Articulos" runat="server" ContextTypeName="KaPlanLib.DAL.ELL" TableName="MAESTRO_ARTICULOS" Where="LANTEGI == @LANTEGI">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="ddl_Lantegis" ConvertEmptyStringToNull="true" DefaultValue="" Name="LANTEGI" PropertyName="SelectedValue" Type="String" />
                                    </WhereParameters>
                                </asp:LinqDataSource>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top">
                        <asp:ImageButton ID="imgCerrar" runat="server" AlternateText="cerrar" ImageAlign="Right" ImageUrl="~/App_Themes/Tema1/Imagenes/cerrar.gif" ToolTip="cerrar" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <AjaxControl:ModalPopupExtender ID="mpe_SelectorRef" runat="server" BackgroundCssClass="modalBackground" CancelControlID="imgCerrar" PopupControlID="pnlSelectorRef" RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnBuscar">
        </AjaxControl:ModalPopupExtender>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

