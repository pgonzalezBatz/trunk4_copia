<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="Lantegis.aspx.vb" Inherits="CostesReales.Lantegis" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
            <%--<asp:Label ID="Label8" runat="server" Text="Lantegis" class="lbl lbl-primary"></asp:Label>--%>
        </asp:Panel>
    </div>
    <br />
    <div class="container">
        <asp:Panel ID="pnlLantegi" runat="server">
            <asp:GridView ID="grdLantegi" runat="server" AutoGenerateColumns="False" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png" Visible="False">
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="Lantegi_ID">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLantegiID" runat="server" Text='<%# Bind("LANTEGI_ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLantegiID" runat="server" Text='<%# Bind("LANTEGI_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lantegi">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlLantegis" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLantegi" runat="server" Text='<%# Bind("Lantegi") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Grupo Producto">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGrupoProducto" runat="server" Text='<%# Bind("Grupo_Producto") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGrupoProducto" runat="server" Text='<%# Bind("Grupo_Producto") %>'></asp:Label>
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
        </asp:Panel>
        <br />
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" />
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textLantegiID">Lantegi_ID</label>
                <asp:TextBox ID="txtLantegiID" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <label for="textLantegi">Lantegi</label>
                <asp:TextBox ID="txtLantegi" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <label for="textGrupoProducto">Grupo Producto</label>
                <asp:TextBox ID="txtGrupoProducto" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <div>
                    <br />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
