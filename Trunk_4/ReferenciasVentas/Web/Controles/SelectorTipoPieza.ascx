<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorTipoPieza.ascx.vb" Inherits="ReferenciasVentas.SelectorTipoPieza" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<script type="text/javascript">
</script>

<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2" style="width:100%; border-width:0px">
            <tr>
                <td style="width:90%">
                    <asp:TextBox ID="txtTiposPieza" runat="server" Width="99%" Columns="20" />
                    <act:AutoCompleteExtender ID="aceTiposPieza" ServiceMethod="CargarTiposPieza"
                        runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                        CompletionInterval="100" EnableCaching="false"
                        TargetControlID="txtTiposPieza" UseContextKey="true"
                        ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                        CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                        CompletionListItemCssClass="CompletionListItemCssClass" />
                        <asp:HiddenField ID="hfTiposPieza" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvTiposPieza" runat="server" ErrorMessage="Required field" ControlToValidate="txtTiposPieza" Enabled="false" Display="None" /><%--Poner esto: ValidationGroup="CamposVacios" y quitar Enabled=false--%>
                        <asp:HiddenField ID="hfEmpresa" runat="server" />
                </td>
                <td style="text-align:center; width:10%">
                    <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Find" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Find type" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlSelectorTipoPieza" runat="server" CssClass="modalBox">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="pnlTipoPieza" runat="server" EnableViewState="true" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>                                    
									<asp:GridView ID="gv_TipoPieza" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Caption="PIECE TYPES"
                                        CellPadding="4" CssClass="GridViewASP" DataKeyNames="ELTO, DENO_S" EmptyDataText="No piece types found for this plant" GridLines="None" PagerSettings-Mode="NumericFirstLast" PageIndex="10">
                                        <HeaderStyle CssClass="HeaderStyle" />
                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                        <RowStyle CssClass="RowStyle" />
                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <Columns>			
											<asp:CommandField ButtonType="Link" ShowSelectButton="True" />
                                            <asp:BoundField DataField="ELTO" HeaderText="CODE" ReadOnly="True" SortExpression="ELTO" />
                                            <asp:BoundField DataField="DENO_S" HeaderText="DESCRIPTION" SortExpression="DENO_S" />
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>                        
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top">
                        <asp:ImageButton ID="imgCerrar" runat="server" AlternateText="Close" ImageAlign="Right" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/cerrar.gif" ToolTip="Close" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <AjaxControl:ModalPopupExtender ID="mpe_SelectorTipoPieza" runat="server" BackgroundCssClass="modalBackground" CancelControlID="imgCerrar" PopupControlID="pnlSelectorTipoPieza" RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnBuscar">
        </AjaxControl:ModalPopupExtender>
        <act:ValidatorCalloutExtender ID="vceTiposPieza" runat="server" TargetControlID="rfvTiposPieza" PopupPosition="BottomRight"  />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

