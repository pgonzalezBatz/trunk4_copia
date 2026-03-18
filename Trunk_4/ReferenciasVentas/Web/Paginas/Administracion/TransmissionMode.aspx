<%@ Page Language="vb" CodeBehind="TransmissionMode.aspx.vb"  Inherits="ReferenciasVentas.TransmissionMode" MasterPageFile="~/RefSis.Master"  %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">
              
    <Titulo:Titulo ID="titgvTransmissionMode" Texto="Transmission Mode maintenance" runat="server" />
    
    <asp:UpdatePanel ID="upTransmissionMode" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvTransmissionMode" runat="Server" AutoGenerateColumns="False" CssClass="GridViewASP"         
                GridLines="None" Width="80%" OnRowCommand="gvTransmissionMode_OnRowCommand" DataKeyNames="Id" ShowFooter="true"
                onRowCancelingEdit="gvTransmissionMode_RowCancelingEdit" OnRowEditing="gvTransmissionMode_RowEditing" onRowUpdating="gvTransmissionMode_RowUpdating">
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
                    <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="left">
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
                    <asp:TemplateField HeaderText="Obsolete">
                        <ItemTemplate>
                            <asp:Checkbox ID="chkObsoleto" Checked='<%# Eval("Obsoleto")%>' Enabled="false" runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Checkbox ID="chkObsoleto" Checked='<%# Eval("Obsoleto")%>' Enabled="true" runat="server" />                            
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Checkbox ID="chkObsoleto" Enabled="true" runat="server" />
                        </FooterTemplate> 
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <FooterTemplate>
                            <asp:Button ID="btnNuevo" CommandName="Add" runat="server" Text="Add new" ValidationGroup="CamposVacios" />
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
                        <EditItemTemplate>
                            <asp:LinkButton ID="lbCancelar" runat="server" Text="Cancel"  CommandName="Cancel"/>
                        </EditItemTemplate>
                    </asp:TemplateField>            
                </Columns> 
                <EmptyDataTemplate>                                        
                    <asp:Label ID="lblNombre" runat="server" Text="Name" />&nbsp
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="20" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Add new" OnClick="btnNuevo_Click" />
                </EmptyDataTemplate>       
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>    
    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>
