<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="Maquinas.aspx.vb" Inherits="CostesReales.Maquinas" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
                <%--<asp:Label ID="Label10" runat="server" Text="Máquinas" class="lbl lbl-primary"></asp:Label>--%>
            </asp:Panel>
    </div>
    <br />
    <div class="container">
        <asp:Panel ID="pnlMaquinaClasificada" runat="server">
            <asp:GridView ID="grdMaquinaClasificada" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png"></asp:CommandField>
                    <asp:TemplateField HeaderText="Maquina">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtMaquina" runat="server" Text='<%# Bind("Maquina") %>'></asp:TextBox>--%>
                            <asp:Label ID="txtMaquina" runat="server"  Text='<%# Bind("Maquina") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMaquina" runat="server" Text='<%# Bind("Maquina") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Descripción">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMaquina_des" runat="server" Text='<%# Bind("Maquina_des") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Maquina_des") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Proceso">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlProceso" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Proceso") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ubicación">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlPlanta" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Planta") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kwh">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtKwh" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Kwh") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Proceso_ID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%#Bind("Proceso_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtProceso" runat="server" Text='<%#Eval("Proceso_ID")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Planta_ID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%#Bind("Proceso_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPlanta" runat="server" Text='<%#Eval("Planta_ID")%>'></asp:TextBox>
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
        </asp:Panel>
        <br />
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" />
        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textMaquina">Máquina</label>
                <asp:TextBox ID="txtMaquina" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <label for="textDescripcion">Descripción</label>
                <asp:TextBox ID="txtDescripcion" class="form-control" runat="server" Width="300px" AutoPostBack="false"></asp:TextBox>
                <label for="ddlProcesos">Proceso</label>
                <asp:DropDownList ID="ddlProcesos" class="form-control" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                <label for="ddlPlantas">Planta</label>
                <asp:DropDownList ID="ddlPlantas" class="form-control" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                <label for="textKwh">Kwh</label>
                <asp:TextBox ID="txtKwh" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                <div>
                    <br />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
