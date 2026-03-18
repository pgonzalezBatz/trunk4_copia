<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="CCGastosPorVenta.aspx.vb" Inherits="CostesReales.CCGastos" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
            <%--<asp:Label ID="Label8" runat="server" Text="Gastos de la 164 y Materia Prima" class="lbl lbl-primary"></asp:Label>--%>
        </asp:Panel>
    </div>
    <br />
    <div class="container">
        <asp:Panel ID="pnlCCPorVenta" runat="server">
            <asp:GridView ID="grdCCGastosPorVenta" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png">
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="CC">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCC" runat="server" Text='<%# Bind("CC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("CC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gastos Venta">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlGastosVenta" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Gastos_Venta") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="Excepcion_Carga" HeaderText="Excepción Carga" />
                    <asp:TemplateField HeaderText="Tipo Reparto">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlTiposReparto" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Tipo_Reparto") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField HeaderText="Gastos_Venta_id" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGastosVentaId" runat="server" Text='<%# Bind("Gastos_Venta_id") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGastosVentaId" runat="server" Text='<%# Bind("Gastos_Venta_id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo_Reparto_id" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTipoRepartoId" runat="server" Text='<%# Bind("Tipo_Reparto_id") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTipoRepartoId" runat="server" Text='<%# Bind("Tipo_Reparto_id") %>'></asp:Label>
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
                <label for="textCC">CC</label>
                <asp:TextBox ID="txtCC" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtCC" ErrorMessage="Sólo números" Operator="DataTypeCheck" Type="Integer">
                </asp:CompareValidator><br />
                <label for="ddlPartidaGasto">PartidaGasto</label>
                <asp:DropDownList ID="ddlPartidaGasto" class="form-control" runat="server" Width="300px"></asp:DropDownList>
                <br />
                <label for="textExcepcionCarga">Excepción Carga</label>
                <asp:CheckBox ID="chkExcepcionCarga" runat="server" />
                <br />        
                <label for="ddlTipoReparto">Tipo Reparto</label>
                <asp:DropDownList ID="ddlTipoReparto" class="form-control" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                <div>
                    <br />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
