<%@ Page Language="vb" CodeBehind="TiposReferenciaVenta.aspx.vb"  Inherits="ReferenciasVentas.TiposReferenciaVenta" MasterPageFile="~/RefSis.Master"  %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">
              
    <Titulo:Titulo ID="titMantenimientoTiposReferenciaVenta" Texto="Maintenance for Types of Selling part numbers" runat="server" />
    
    <asp:UpdatePanel ID="upTiposReferencia" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvTiposReferenciaVenta" runat="Server" AutoGenerateColumns="False" CssClass="GridViewASP"     
                GridLines="None" Width="80%" OnRowCommand="gvTiposReferenciaVenta_OnRowCommand" DataKeyNames="Id" ShowFooter="true"
                onRowCancelingEdit="gvTiposReferenciaVenta_RowCancelingEdit" onRowDeleting="gvTiposReferenciaVenta_RowDeleting" 
                OnRowEditing="gvTiposReferenciaVenta_RowEditing" onRowUpdating="gvTiposReferenciaVenta_RowUpdating">
                <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" Height="40" VerticalAlign="Middle"/>
                <PagerStyle CssClass="PagerStyle" />
                <SelectedRowStyle CssClass="SelectedRowStyle" />
                <HeaderStyle CssClass="HeaderStyle" />
                <EditRowStyle CssClass="EditRowStyle" />
                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                    <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" Text='<%# Eval("Nombre")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNombre" runat="server" Text='<%# Eval("Nombre")%>' MaxLength="20" Width="95%" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="*" ErrorMessage="* Required field" ControlToValidate="txtNombre" ValidationGroup="CamposVaciosEdicion" />                       
                            <act:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="BottomLeft" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="20" Width="50%" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="*" ErrorMessage="* Required field" ControlToValidate="txtNombre" ValidationGroup="CamposVacios" />                       
                            <act:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="BottomLeft" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Descripction" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lblDescripcion" Text='<%# Eval("Descripcion")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Eval("Descripcion")%>' MaxLength="50" Width="95%" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="50" Width="90%" />                      
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <FooterTemplate>
                            <asp:Button ID="btnNuevo" CommandName="Add" runat="server" Text="Add new type" ValidationGroup="CamposVacios" />
                        </FooterTemplate> 
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEditar" runat="server" Text="Edit" CommandName="Edit" />                            
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lbModificar" runat="server" Text="Save" CommandName="Update" ValidationGroup="CamposVaciosEdicion" />
                        </EditItemTemplate>               
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton id="btnEliminar" runat="server" commandname="Delete" text="Delete" />
                            <act:ConfirmButtonExtender ID="cbeEliminar" runat="server" DisplayModalPopupID="mpeEliminar"
                                TargetControlID="btnEliminar"></act:ConfirmButtonExtender>                    
                            <act:ModalPopupExtender ID="mpeEliminar" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminar" OkControlID="btnBorrar"
                                CancelControlID="btnCancelar" BackgroundCssClass="modalBackground"></act:ModalPopupExtender>
                            <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                <div class="header">
                                    <asp:Label ID="lblConfirmacion" runat="server" Text="Confirmation" />
                                </div>
                                <div class="body">
                                    <asp:Label ID="lblConfirmarBorrado" runat="server" Text="Are you sure you want to delete the record?" />
                                </div>
                                <div class="footer" align="center">
                                    <asp:Button ID="btnBorrar" runat="server" CssClass="si" Text="Yes" />
                                    <asp:Button ID="btnCancelar" runat="server" CssClass="no" Text="No" />
                                </div>
                            </asp:Panel>                       
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lbCancelar" runat="server" Text="Cancel"  CommandName="Cancel"/>
                        </EditItemTemplate>
                    </asp:TemplateField>            
                </Columns>        
                <EmptyDataTemplate>                                        
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre" />&nbsp
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="20" Columns="20" />
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripction" />&nbsp
                    <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="50" Columns="50" />&nbsp
                    <asp:Button ID="btnNuevo" runat="server" Text="Add new" OnClick="btnNuevo_Click" />
                </EmptyDataTemplate> 
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>    
    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>
