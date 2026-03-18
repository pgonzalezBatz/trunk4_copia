<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="CCGastosPorNegocio.aspx.vb" Inherits="CostesReales.CCPorNegocio" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
            <%--<asp:Label ID="Label8" runat="server" Text="Cuentas de Venta E Inventario PT y PC " class="lbl lbl-primary"></asp:Label>--%>
        </asp:Panel>
    </div>
    <br />
    <div class="container">
        <asp:Panel ID="pnlCCPorNegocio" runat="server">
            <asp:GridView ID="grdCCPorNegocio" runat="server" AutoGenerateColumns="False" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png"></asp:CommandField>
                    <asp:TemplateField HeaderText="CC">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCC" runat="server" Text='<%# Bind("CC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("CC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lantegi_id" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLantegi_id" runat="server" Text='<%# Bind("Lantegi_id") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Lantegi_id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="Aplica_Ventas" HeaderText="Aplica_Ventas" />
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
        </asp:Panel>
        <br />
        <asp:LinkButton ID="btnNuevo" CssClass="btn btn-primary" data-toggle="modal" data-target="#modalNuevo" runat="server" UseSubmitBehavior="false">Nuevo</asp:LinkButton>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="modalNuevo" tabindex="-1" role="dialog" aria-labelledby="exampleModarCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="H1">Nueva cuenta de venta e inventario PT y PC</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <asp:Label ID="lblCC" runat="server" Text="Cuenta Contable"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtCCForm" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblLantegi" runat="server" Text="Lantegi"></asp:Label>
                        <asp:DropDownList ID="ddlLantegiForm" CssClass="form-control input-group-sm" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label ID="lblAplicaVnetas" runat="server" Text="Aplica Ventas"></asp:Label>
                    <br />
                    <asp:CheckBox ID="chkAplicaVentas" runat="server" />
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnGuardar" CssClass="btn btn-primary" runat="server" OnClick="btnGuardar_Click" UseSubmitBehavior="false">Guardar</asp:LinkButton>
                        <button type="button" onclick="javascript:window.location.reload()" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
