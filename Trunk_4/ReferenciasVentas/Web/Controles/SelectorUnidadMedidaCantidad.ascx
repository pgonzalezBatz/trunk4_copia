<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorUnidadMedidaCantidad.ascx.vb" Inherits="ReferenciasVentas.SelectorUnidadMedidaCantidad" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<script type="text/javascript">
    
</script>

<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2" style="width:100%; border-width:0px">
            <tr>
                <td style="width:90%">
                    <asp:TextBox ID="txtUnidadMedidaCantidad" runat="server" Width="99%" Columns="20" />
                    <act:AutoCompleteExtender ID="aceUnidadMedidaCantidad" ServiceMethod="CargarUnidadesMedida"
                        runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                        CompletionInterval="100" EnableCaching="false"
                        TargetControlID="txtUnidadMedidaCantidad" UseContextKey="true"
                        ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                        CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                        CompletionListItemCssClass="CompletionListItemCssClass" />
                        <asp:HiddenField ID="hfUnidadMedidaCantidad" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvUnidadMedidaCantidad" runat="server" ErrorMessage="Required field" ControlToValidate="txtUnidadMedidaCantidad" Display="None" ValidationGroup="CamposVacios" />                                             
                        <asp:HiddenField ID="hfEmpresa" runat="server" />
                </td>
                <td style="text-align:center; width:10%">
                    <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Find" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Find quantity" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlSelectorUnidadMedida" runat="server" CssClass="modalBox">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="pnlUnidadMedida" runat="server" EnableViewState="true" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>                                    
									<asp:GridView ID="gv_UnidadMedida" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Caption="UNITS OF MEASURE"
                                        CellPadding="4" CssClass="GridViewASP" DataKeyNames="ELTO, DENO_S" EmptyDataText="No units of measure found" GridLines="None" PagerSettings-Mode="NumericFirstLast" PageIndex="10" >
                                        <HeaderStyle CssClass="HeaderStyle" /><%-- sqldatasource="sqlDataSource_UnidadesMedida"--%>
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
<%--                                <asp:SqlDataSource ID="sqlDataSource_UnidadesMedida" runat="server" selectCommand="select * from cubos. where empresa=1" ProviderName="System.Data.OleDb">
                                </asp:SqlDataSource>  --%>                            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top">
                        <asp:ImageButton ID="imgCerrar" runat="server" AlternateText="Close" ImageAlign="Right" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/cerrar.gif" ToolTip="Close" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <AjaxControl:ModalPopupExtender ID="mpe_SelectorUnidadMedida" runat="server" BackgroundCssClass="modalBackground" CancelControlID="imgCerrar" PopupControlID="pnlSelectorUnidadMedida" RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnBuscar">
        </AjaxControl:ModalPopupExtender>
        <act:ValidatorCalloutExtender ID="vceUnidadMedidaCantidad" runat="server" TargetControlID="rfvUnidadMedidaCantidad" PopupPosition="BottomRight" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

