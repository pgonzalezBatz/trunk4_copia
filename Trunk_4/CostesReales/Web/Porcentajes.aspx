<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CostesReales.Master" CodeBehind="Porcentajes.aspx.vb" Inherits="CostesReales.TablasPorcentajes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:Panel ID="pnlPorcentajes" runat="server">
            <asp:GridView ID="grdPorcentajes" runat="server"
                OnRowDeleting="grdPorcentajes_RowDeleting"
                OnRowEditing="grdPorcentajes_RowEditing"
                OnRowUpdating="grdPorcentajes_RowUpdating"
                OnRowCancelingEdit="grdPorcentajes_RowCancelingEdit"
                AutoGenerateColumns="False"
                BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                AllowPaging="True" EmptyDataText="No hay datos para mostrar">
                <Columns>
                    <asp:TemplateField HeaderText="DPTO">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Bind("DPTO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDpto" runat="server" Text='<%#Eval("DPTO")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Reubicados (%)">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%#Bind("DPTO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtReubicados" runat="server" Text='<%#Eval("Reubicados (%)")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ETT (%)">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%#Bind("DPTO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtETT" runat="server" Text='<%#Eval("ETT (%)")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%#Bind("Total")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTotal" runat="server" Text='<%#Eval("Total")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ShowCancelButton="true" ShowDeleteButton="true" ButtonType="Image" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png" >
                    </asp:CommandField>
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
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-primary" />
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textDepartamento">Departamento</label>
                <asp:TextBox ID="txtDepartamento" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <br />
                <label for="textReubicados">Reubicados</label>
                <asp:TextBox ID="txtReubicados" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <br />
                <label for="textETT">ETT</label>
                <asp:TextBox ID="txtETT" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <br />                
                <div>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-danger" Style="height: 33px" />
                </div>
            </div>
        </asp:Panel>       
    </div>
</asp:Content>
