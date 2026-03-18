<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="PorcentajeDepartamento.aspx.vb" Inherits="CostesReales.PorcentajeDepartamento" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
            <%--<asp:Label ID="Label8" runat="server" Text="Porcentajes por Departamento" class="lbl lbl-primary"></asp:Label>--%>
        </asp:Panel>
    </div>
    <br />
    <div class="container">
        <asp:Panel ID="pnlPorcentajeDepartamento" runat="server">
            <asp:GridView ID="grdPorcentajeDepartamento" runat="server" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png"></asp:CommandField>                                
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/Content/img/eliminar24.png" Text="Eliminar"
                                OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <%--<br />
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-primary" />
                <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textProceso">Proceso</label>
                <asp:DropDownList ID="ddlProcesos" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:DropDownList>
                <label for="textDepartamento">Departamento</label>
                <asp:DropDownList ID="ddlDepartamentos" class="form-control" runat="server" Width="300px" AutoPostBack="false"></asp:DropDownList>
                <label for="textPorcentaje">Porcentaje</label>
                <asp:TextBox ID="txtPorcentaje" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>              
                <div>
                    <br />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-danger" />
                </div>
            </div>
        </asp:Panel>--%>
    </div>
</asp:Content>
