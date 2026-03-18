<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="CCGastosMOI.aspx.vb" Inherits="CostesReales.CCGastosMOI" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
            <%--<asp:Label ID="Label8" runat="server" Text="Cuentas de Gastos MOI" class="lbl lbl-primary"></asp:Label>--%>
        </asp:Panel>
    </div>
    <br />
    <div class="container">
        <asp:Panel ID="pnlCCGastoMOI" runat="server">     
            <asp:GridView ID="grdGastosMOI" runat="server" AutoGenerateColumns="False" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png">
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="CC">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMaquina" runat="server" Text='<%# Bind("CC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("CC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Gasto_MOI">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlGastoMOI" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Gasto_MOI") %>'></asp:Label>
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
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textCC">CC</label>
                <asp:TextBox ID="txtCC" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <label for="textGastoMOI">Gasto MOI</label>
                <asp:DropDownList ID="ddlGastoMOI" runat="server" class="form-control" Width="300px"></asp:DropDownList>       
                <%--<label for="textCC">Energía</label>
                <asp:TextBox ID="txtEnergia" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>            --%>    
                <div>
                    <br />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
