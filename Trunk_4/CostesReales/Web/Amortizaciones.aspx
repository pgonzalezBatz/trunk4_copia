<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CostesReales.Master" CodeBehind="Amortizaciones.aspx.vb" Inherits="CostesReales.Amortizaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:Panel ID="pnlAmortizaciones" runat="server">
            <asp:GridView ID="grdAmortizacionesActivos" runat="server"
                OnRowDeleting="grdAmortizacionesActivos_RowDeleting"
                OnRowEditing="grdAmortizacionesActivos_RowEditing"
                OnRowUpdating="grdAmortizacionesActivos_RowUpdating"
                OnRowCancelingEdit="grdAmortizacionesActivos_RowCancelingEdit"
                AutoGenerateColumns="False"
                DataKeyNames="NUM_ACTIVO" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                AllowPaging="True" EmptyDataText="No hay datos para mostrar">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png">
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="Num Activo">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Bind("NUM_ACTIVO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNumActivo" runat="server" Text='<%#Eval("NUM_ACTIVO")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>                   
                    <asp:TemplateField HeaderText="Criterio Reparto">                        
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlCriterioReparto" runat="server"></asp:DropDownList>
                        </EditItemTemplate> 
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Criterio_Reparto") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Planta">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlPlanta" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Planta") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Proceso">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlProceso" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("Proceso") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Criterio_Reparto_ID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%#Bind("Criterio_Reparto_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCriterioReparto" runat="server" Text='<%#Eval("Criterio_Reparto_ID")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Planta_ID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%#Bind("Planta_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPlanta" runat="server" Text='<%#Eval("Planta_ID")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Proceso_ID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%#Bind("Proceso_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtProceso" runat="server" Text='<%#Eval("Proceso_ID")%>'></asp:TextBox>
                        </EditItemTemplate>
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
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-primary" />
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textNumActivo">Número de Activo</label>
                <asp:TextBox ID="txtNumActivo" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <label for="textCriterioReparto">Lantegi</label>
                <br />
                <asp:DropDownList ID="ddlCriteriosReparto" class="form-control" runat="server" Width="300px"></asp:DropDownList>
                <label for="textPlanta">Planta</label>
                <br />
                <asp:DropDownList ID="ddlPlantas" class="form-control" runat="server" Width="300px"></asp:DropDownList>
                <label for="textProceso">Proceso</label>
                <br />
                <asp:DropDownList ID="ddlProcesos" class="form-control" runat="server" Width="300px"></asp:DropDownList>
                <br />
                <div>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-danger" Style="height: 33px" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
