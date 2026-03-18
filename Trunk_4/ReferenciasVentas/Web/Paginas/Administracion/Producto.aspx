<%@ Page Language="vb" CodeBehind="Producto.aspx.vb"  Inherits="ReferenciasVentas.Producto" MasterPageFile="~/RefSis.Master"  %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">
    <link type="text/css" href="../../App_Themes/Batz/style.css" rel="stylesheet" media="screen" />         
    <Titulo:Titulo ID="titProducto" Texto="Products maintenance" runat="server" />
    
    <asp:UpdatePanel ID="upProducto" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvProducto" runat="Server" AutoGenerateColumns="False" CssClass="GridViewASP"       
                GridLines="None" Width="100%"  DataKeyNames="Id" OnRowDataBound="gvProducto_RowDataBound">
                <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" Height="40" VerticalAlign="Middle" />
                <PagerStyle CssClass="PagerStyle" />
                <SelectedRowStyle CssClass="SelectedRowStyle" />
                <HeaderStyle CssClass="HeaderStyle" />
                <EditRowStyle CssClass="EditRowStyle" />
                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                <EmptyDataRowStyle CssClass="EmptyRowStyle" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                    <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" Text='<%# Eval("Nombre")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblDescripcion" Text='<%# Eval("Descripcion")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Relationed Types">
                        <ItemTemplate>
                            <asp:Label ID="lblRelationedTypes" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Transmission Mode Visible">
                        <ItemTemplate>
                            <asp:Checkbox ID="chkTransmissionModeVisible" Checked='<%# Eval("TransmissionModeVisible")%>' Enabled="false" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Obsolete">
                        <ItemTemplate>
                            <asp:Checkbox ID="chkObsoleto" Checked='<%# Eval("Obsoleto")%>' Enabled="false" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEditar" runat="server" Text="Edit" OnClick="lbEditar_Click" />                            
                        </ItemTemplate>              
                    </asp:TemplateField>
                </Columns>        
            </asp:GridView>

            <asp:Button ID="btnNuevoProducto" runat="server" Text="Add new product" OnClick="btnNuevoProducto_Click" style="margin-top:20px; margin-bottom:10px" />

            <asp:Button ID="btnMpe_Open" runat="server" Style="display: none" />
            <act:ModalPopupExtender ID="mpe_Type" runat="server" BackgroundCssClass="modalBackground" CancelControlID="btnCancelar" PopupControlID="pnlType" TargetControlID="btnMpe_Open">
            </act:ModalPopupExtender>

            <asp:Panel ID="pnlType" runat="server" CssClass="modalPopup" Style="display: none" Width="60%">
                <asp:HiddenField ID="hfIdProducto" runat="server" />
                <div class="header">
                    <asp:Label ID="lblTituloType" runat="server" Text="New Type" />
                </div>
                <div class="body">
                    <table style="width: 100%">
                        <tr>
                            <td class="definicion15">
                                <asp:Label ID="lblNombreProducto" Text="Name" runat="server" />
                            </td>
                            <td class="campoTextoNormal" style="text-align:left">
                                <asp:TextBox ID="txtNombreProducto" runat="server" Width="95%" />
                                <asp:RequiredFieldValidator ID="rfvNombreProducto" runat="server" Text="*" ErrorMessage="* Required field" Display="None" ControlToValidate="txtNombreProducto" ValidationGroup="CamposVacios" />                       
                                <act:ValidatorCalloutExtender ID="vceNombreProducto" runat="server" TargetControlID="rfvNombreProducto" PopupPosition="Right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="definicion15">
                                <asp:Label ID="lblDescripcionProducto" Text="Description" runat="server" />
                            </td>
                            <td class="campoTextoNormal" style="text-align:left">
                                <asp:TextBox ID="txtDescripcionProducto" runat="server" Width="95%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="definicion15">
                                <asp:Label ID="lblTransmissionModeVisible" Text="Transmission Mode Visible" runat="server" />
                            </td>
                            <td class="campoTextoNormal" style="text-align:left">
                                <asp:CheckBox ID="chkTransmissionModeVisible" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="definicion15">
                                <asp:Label ID="lblTiposRelacionados" Text="Relationed Types" runat="server" />
                            </td>
                            <td class="campoTextoNormal">
                                <asp:CheckBoxList ID="chklTiposRelacionados" runat="server" DataValueField="Id" DataTextField="Nombre" RepeatColumns="4" />
                            </td>
                        </tr>
                        <tr>
                            <td class="definicion15">
                                <asp:Label ID="lblObsoleto" Text="Obsolete" runat="server" />
                            </td>
                            <td class="campoTextoNormal" style="text-align:left">
                                <asp:CheckBox ID="chkObsoleto" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center" colspan="2">
                                <asp:Button ID="btnGuardar" runat="server" Text="Save" style="margin-top:20px" OnClick="btnGuardar_Click" ValidationGroup="CamposVacios" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" colspan="2">
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancel" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>    
    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>
