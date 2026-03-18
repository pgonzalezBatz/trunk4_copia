<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="RPGenericos.aspx.vb" Inherits="CostesReales.RPGenericos" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
            <%--<asp:Label ID="Label8" runat="server" Text="RP Genéricos" class="lbl lbl-primary"></asp:Label>--%>
        </asp:Panel>
    </div>
    <br />
    <div class="container">
        <asp:Panel ID="pnlRPGenerico" runat="server">
                <asp:GridView ID="grdRPGenericos" runat="server" AutoGenerateColumns="False" AllowPaging="True">
                    <Columns>
                        <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png">
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="RP">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRP" runat="server" Text='<%# Bind("RP") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("RP") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lantegi_id" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLantegiId" runat="server" Text='<%# Bind("Lantegi_id") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Lantegi_id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lantegi">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlLantegis" runat="server"></asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Lantegi") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/Content/img/eliminar24.png" Text="Eliminar" 
                                    OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" />
            <br />
            <br />
        </asp:Panel>
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textRP">
                    RP<asp:RequiredFieldValidator ID="rfvRP" runat="server" ControlToValidate="txtRP" Display="Static" ErrorMessage="* Campo obligatorio" />
                </label>
                <asp:TextBox ID="txtRP" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <label for="textLantegi">
                    Lantegi<asp:RequiredFieldValidator ID="rfvLantegi" runat="server" ControlToValidate="ddlLantegis" InitialValue="Seleccione lantegi..." Display="Dynamic" ErrorMessage="* Campo obligatorio" />
                </label>
                <asp:DropDownList ID="ddlLantegis" class="form-control" runat="server" Width="200px" AutoPostBack="false"></asp:DropDownList>
                <div>
                    <br />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" Style="height: 33px" CausesValidation="False" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
