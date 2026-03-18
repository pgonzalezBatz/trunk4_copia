<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="InventarioAjusteManual.aspx.vb" Inherits="CostesReales.InventarioAjusteManual" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:Panel ID="pnlInventarioAjusteManual" runat="server">
            <asp:GridView ID="grdInventarioAjusteManual" runat="server"
                OnRowDeleting="grdInventarioAjusteManual_RowDeleting"
                OnRowEditing="grdInventarioAjusteManual_RowEditing"
                OnRowUpdating="grdInventarioAjusteManual_RowUpdating"
                OnRowCancelingEdit="grdInventarioAjusteManual_RowCancelingEdit"
                AutoGenerateColumns="False"
                DataKeyNames="Fecha_id" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                AllowPaging="True" EmptyDataText="No hay datos para mostrar">
                <Columns>
                    <asp:TemplateField HeaderText="Fecha_id">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Bind("Fecha_id")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFechaId" runat="server" Text='<%#Eval("Fecha_id")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Referencia">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%#Bind("Referencia")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtReferencia" runat="server" Text='<%#Eval("Referencia")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unidades_Ajuste">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%#Bind("Unidades_Ajuste")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnidadesAjuste" runat="server" Text='<%#Eval("Unidades_Ajuste")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ShowCancelButton="true" ShowDeleteButton="true" ButtonType="Image" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png"></asp:CommandField>
                </Columns>
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />
            </asp:GridView>
            <br />
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" />
        </asp:Panel>
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textReferencia">Referencia<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReferencia" Display="Static" ErrorMessage="* Campo obligatorio" />
                </label>
                <asp:TextBox ID="txtReferencia" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <label for="textUnidadAjuste">Unidad de ajuste<asp:RequiredFieldValidator ID="rfvUnidadesAjuste" runat="server" ControlToValidate="txtUnidadesAjuste" Display="Static" ErrorMessage="* Campo obligatorio" />
                </label>
                <asp:TextBox ID="txtUnidadesAjuste" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <div>
                    <br />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-danger" Style="height: 33px" CausesValidation="False" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
