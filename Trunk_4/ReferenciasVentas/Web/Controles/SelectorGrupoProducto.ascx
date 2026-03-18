<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorGrupoProducto.ascx.vb" Inherits="ReferenciasVentas.SelectorGrupoProducto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<script type="text/javascript">

</script>

<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2" style="width:100%; border-width:0px">
            <tr>
                <td style="width:90%">
                    <asp:TextBox ID="txtGrupoProducto" runat="server" Width="99%" Columns="20" />
                    <act:AutoCompleteExtender ID="aceGrupoProducto" ServiceMethod="CargarGruposProducto"
                        runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                        CompletionInterval="100" EnableCaching="false"
                        TargetControlID="txtGrupoProducto" UseContextKey="true"
                        ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                        CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                        CompletionListItemCssClass="CompletionListItemCssClass" />
                        <asp:HiddenField ID="hfGrupoProducto" runat="server" />
                        <asp:HiddenField ID="hfIdGP" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvGrupoProducto" runat="server" ErrorMessage="Required field" ControlToValidate="txtGrupoProducto" Display="None" ValidationGroup="CamposVacios" />                                               
                </td>
                <td style="text-align:center; width:10%">
                    <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Find" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Find group" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlSelectorGrupoProducto" runat="server" CssClass="modalBox" style="overflow: scroll">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="pnlGrupoProducto" runat="server" EnableViewState="true" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>                                    
									<asp:GridView ID="gv_GrupoProducto" runat="server" AllowSorting="True" AutoGenerateColumns="False" Caption="PRODUCT GROUPS"
                                        CellPadding="4" CssClass="GridViewASP" DataKeyNames="ELTO, DENO_S, CodigoProducto" EmptyDataText="No product groups found" GridLines="None" PagerSettings-Mode="NumericFirstLast"><%--AllowPaging="True" PageIndex="10"--%>
                                        <HeaderStyle CssClass="HeaderStyle" />
                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                        <RowStyle CssClass="RowStyle" />
                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <Columns>			
											<%--<asp:CommandField ButtonType="Link" ShowSelectButton="True" />--%>
                                            <asp:BoundField DataField="ELTO" HeaderText="CODE"  SortExpression="ELTO" />
                                            <asp:BoundField DataField="DENO_S" HeaderText="DESCRIPTION" SortExpression="DENO_S" />
                                            <asp:BoundField DataField="CodigoProducto" HeaderText="PRODUCT CODE"  SortExpression="CodigoProducto" Visible="false" />
                                            <asp:BoundField DataField="Producto" HeaderText="DESCRIPTION" SortExpression="Producto" />
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
        <AjaxControl:ModalPopupExtender ID="mpe_SelectorGrupoProducto" runat="server" BackgroundCssClass="modalBackground" CancelControlID="imgCerrar" PopupControlID="pnlSelectorGrupoProducto" RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnBuscar">
        </AjaxControl:ModalPopupExtender>
        <act:ValidatorCalloutExtender ID="vceGrupoProducto" runat="server" TargetControlID="rfvGrupoProducto" PopupPosition="BottomRight" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

