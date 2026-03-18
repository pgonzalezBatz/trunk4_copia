<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorProyectoPTKSIS.ascx.vb" Inherits="ReferenciasVentas.SelectorProyectoPTKSIS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<table class="table2" style="width:100%; border-width:0px">
    <tr>
        <td style="width:90%">
            <asp:TextBox ID="txtCustProjectName" runat="server" Width="99%" Columns="20" />
            <act:AutoCompleteExtender ID="aceCustProjectName" ServiceMethod="CargarProyectosPTKSis"
                runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="ProyectoPtksisElegido"
                TargetControlID="txtCustProjectName" UseContextKey="true"
                ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                CompletionListItemCssClass="CompletionListItemCssClass" />
                <asp:HiddenField ID="hfCustProjectName" runat="server" />
                <asp:HiddenField ID="hfIdProjectPtksis" runat="server" />
                <asp:RequiredFieldValidator ID="rfvCustPN" runat="server" ErrorMessage="Required field" ControlToValidate="txtCustProjectName" ValidationGroup="CamposVacios" Display="None" />                       
                        
                <asp:HiddenField ID="hfModo" runat="server" />
        </td>
        <td style="text-align:center; width:10%">
            <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Find" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Find project" OnClientClick="return showPopup()" />
        </td>
    </tr>
</table>
<asp:Panel ID="pnlSelectorProyecto" runat="server" CssClass="modalBox" style="height:50%; overflow:scroll; display: none">
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="pnlProyecto" runat="server" EnableViewState="true" UpdateMode="Always">
                    <ContentTemplate>
                        <fieldset>                                    
							<asp:GridView ID="gv_Proyectos" runat="server"  AllowSorting="True" AutoGenerateColumns="False" Caption="PROJECTS IN PTKSIS"
                                CellPadding="4" CssClass="GridViewASP" DataKeyNames="Id, Nombre, IdPtksis" EmptyDataText="No projects found" GridLines="None" >
                                <HeaderStyle CssClass="HeaderStyle" />
                                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                <RowStyle CssClass="RowStyle" />
                                <SelectedRowStyle CssClass="SelectedRowStyle" />
                                <PagerStyle CssClass="PagerStyle" />
                                <Columns>
                                    <asp:BoundField DataField="Id" Visible="true" />
                                    <asp:BoundField DataField="Nombre" HeaderText="NAME" SortExpression="NOMBRE" />
                                    <asp:BoundField DataField="IdPtksis" Visible="false" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>                            
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td valign="top">
                <asp:ImageButton ID="imgCerrar" runat="server" AlternateText="Close" ImageAlign="Right" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/cerrar.gif" ToolTip="Close" OnClientClick="return HidePopup()" />
            </td>
        </tr>
    </table>
</asp:Panel>
<AjaxControl:ModalPopupExtender ID="mpe_SelectorProyecto" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mpe"  PopupControlID="pnlSelectorProyecto" TargetControlID="lnkDummy" RepositionMode="RepositionOnWindowResizeAndScroll" >
</AjaxControl:ModalPopupExtender>
    <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
<act:ValidatorCalloutExtender ID="vceCustPN" runat="server" TargetControlID="rfvCustPN" PopupPosition="TopLeft"  />

<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />
