<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorDisponente.ascx.vb" Inherits="ReferenciasVentas.SelectorDisponente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<script type="text/javascript">
</script>

<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2" style="width:100%; border-width:0px">
            <tr>
                <td style="width:90%">
                    <asp:TextBox ID="txtDisponente" runat="server" Width="99%" Columns="20" />
                    <act:AutoCompleteExtender ID="aceDisponente" ServiceMethod="CargarDisponentes"
                        runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                        CompletionInterval="100" EnableCaching="false"
                        TargetControlID="txtDisponente" UseContextKey="true"
                        ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                        CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                        CompletionListItemCssClass="CompletionListItemCssClass" />
                        <asp:HiddenField ID="hfDisponente" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvDisponente" runat="server" ErrorMessage="Required field" ControlToValidate="txtDisponente" Display="None" ValidationGroup="CamposVacios" />                                               
                        <asp:HiddenField ID="hfEmpresa" runat="server" />
                </td>
                <td style="text-align:center; width:10%">
                    <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Find" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Find disponent" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlSelectorDisponente" runat="server" CssClass="modalBox">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="pnlDisponente" runat="server" EnableViewState="true" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>                                    
									<asp:GridView ID="gv_Disponente" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Caption="DISPATCHERS"
                                        CellPadding="4" CssClass="GridViewASP" DataKeyNames="ELTO, DENO_S" EmptyDataText="No dispatchers found for this plant" GridLines="None" PagerSettings-Mode="NumericFirstLast" PageIndex="10" >
                                        <HeaderStyle CssClass="HeaderStyle" />
                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                        <RowStyle CssClass="RowStyle" />
                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <Columns>			
											<asp:CommandField ButtonType="Link" ShowSelectButton="True" />
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
        <AjaxControl:ModalPopupExtender ID="mpe_SelectorDisponente" runat="server" BackgroundCssClass="modalBackground" CancelControlID="imgCerrar" PopupControlID="pnlSelectorDisponente" RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnBuscar">
        </AjaxControl:ModalPopupExtender>
        <act:ValidatorCalloutExtender ID="vceDisponente" runat="server" TargetControlID="rfvDisponente" PopupPosition="Right" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

