<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CostesReales.Master" CodeBehind="Departamentos.aspx.vb" Inherits="CostesReales.Departamentos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:Panel ID="pnlDepartamentos" runat="server">
            <asp:GridView ID="grdDepartamentos" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png">
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="id" Visible ="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("id") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dpto">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Dpto") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Dpto") %>'></asp:Label>
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
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-primary" />
        <asp:Button ID="btnETTReubicados" runat="server" Text="% ETT y reubicados" CssClass="btn-primary" />
        <asp:Button ID="btnDepartamento" runat="server" Text="% Reparto por departamento" CssClass="btn-primary" />
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textDepartamento">Departamento</label>
                <asp:TextBox ID="txtDepartamento" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
            </div>
            <div>
                <br />
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-primary" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-danger" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
