<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorTipoProducto.ascx.vb" Inherits="ReferenciasVentas.SelectorTipoProducto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<script type="text/javascript">
    function DisponenteElegido(source, eventArgs) {
        var hdnValueId = document.getElementById('<%=hfTipoProducto.ClientID%>');
        hdnValueId.value = eventArgs.get_value();      
    };
</script>

<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2" style="width:100%; border-width:0px">
            <tr>
                <td style="width:90%">
                    <asp:TextBox ID="txtTipoProducto" runat="server" Width="98%" />
                    <act:AutoCompleteExtender ID="aceDisponente" ServiceMethod="CargarTiposProducto"
                        runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                        CompletionInterval="100" EnableCaching="false" OnClientItemSelected="DisponenteElegido"
                        TargetControlID="txtTipoProducto" UseContextKey="true"
                        ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                        CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                        CompletionListItemCssClass="CompletionListItemCssClass" />
                        <asp:HiddenField ID="hfTipoProducto" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvTipoProducto" runat="server" ErrorMessage="Required field" ControlToValidate="txtTipoProducto" Display="None" ValidationGroup="CamposVacios" />                                               
                </td>
                <td style="text-align:center; width:10%">
                    <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Find" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Find product type" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlSelectorTipoProducto" runat="server" CssClass="modalBox" style="height:50%; overflow:scroll">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="pnlTipoProducto" runat="server" EnableViewState="true" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>                                    
									<asp:GridView ID="gv_TipoProducto" runat="server" AllowSorting="True" AutoGenerateColumns="False" Caption="PRODUCT TYPES"
                                        CellPadding="4" CssClass="GridViewASP" DataKeyNames="ELTO, DENO_S" EmptyDataText="No product types found" GridLines="None" PagerSettings-Mode="NumericFirstLast"><%--AllowPaging="True" PageIndex="10"--%>
                                        <HeaderStyle CssClass="HeaderStyle" />
                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                        <RowStyle CssClass="RowStyle" />
                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <Columns>			
											<%--<asp:CommandField ButtonType="Link" ShowSelectButton="True" />--%>
                                            <asp:BoundField DataField="ELTO" HeaderText="CODE"  SortExpression="ELTO" />
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
        <AjaxControl:ModalPopupExtender ID="mpe_SelectorTipoProducto" runat="server" BackgroundCssClass="modalBackground" CancelControlID="imgCerrar" PopupControlID="pnlSelectorTipoProducto" RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnBuscar">
        </AjaxControl:ModalPopupExtender>
        <act:ValidatorCalloutExtender ID="vceTipoProducto" runat="server" TargetControlID="rfvTipoProducto" PopupPosition="BottomRight" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

