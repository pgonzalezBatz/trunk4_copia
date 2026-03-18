<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="PorcentajeETTReubicados.aspx.vb" Inherits="CostesReales.PorcentajeETTReubicados" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <br />
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
<%--            <asp:Label ID="Label8" runat="server" Text="Porcentajes por departamento para el ejercicio vigente" class="lbl lbl-primary"></asp:Label><!-- TODO: PREGUNTAR A ANA POR EL TITULO -->--%>
        </asp:Panel>
    </div>
    <br />        
    
    <asp:Panel ID="filtroTabla" runat="server" CssClass="container">
        <h2>Filtros</h2>
        <asp:Label ID="dptoFiltro" runat="server">Departamento:</asp:Label><br />
        <asp:TextBox ID="dptoFiltroData" runat="server"></asp:TextBox><br />
        <asp:Label ID="fechaFiltro" runat="server">Fecha:</asp:Label>
        <div class='input-group date myDatePicker' id='datetimepicker_1'>
            <input type="text" id="fechaFiltroData" runat="server" />
            <span class="input-group-addon" style="width:auto;padding:4px 12px;">
                <span class="glyphicon glyphicon-calendar"></span>
            </span>
        </div>
        <br />
        <asp:Button ID="btnFiltro" runat="server" CssClass="btn btn-info" Text="Filtrar" />
        <asp:Button ID="btnFiltroOff" runat="server" CssClass="btn btn-info" Text="Quitar filtros" />
    </asp:Panel>
    <br />
    <div class="container">
        <%--<asp:Panel ID="filtroTabla" runat="server" GroupingText="Filtro">--%>



        <asp:Panel ID="pnlDepartamentos" runat="server">
            <asp:GridView ID="grdPorcentajeETTReubicados" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" AllowPaging="True">
                <Columns>
                    <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png">
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="Dpto_ID" Visible="false">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDptoId" runat="server" Text='<%# Bind("DPTO_ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDptoId" runat="server" Text='<%# Bind("DPTO_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dpto">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDpto" runat="server" Text='<%# Bind("DPTO") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDpto" runat="server" Text='<%# Bind("DPTO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reubicados (%)">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtReubicados" runat="server" Text='<%# Bind("Reubicados")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReubicados" runat="server" Text='<%# Bind("Reubicados") %>'></asp:Label>
                        </ItemTemplate>                       
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ETT (%)">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtETT" runat="server" Text='<%# Bind("ETT") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblETT" runat="server" Text='<%# Bind("ETT") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" ConvertEmptyStringToNull="False">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtTotal" runat="server" Text='<%# Bind("Total") %>'></asp:TextBox>--%>
                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFecha" runat="server" Text='<%# Bind("fecha_id") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFecha" runat="server" Text='<%# Bind("fecha_id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Fecha Formateada">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFecha" runat="server" Text='<%# Bind("Fecha") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFecha" runat="server" Text='<%# Bind("Fecha") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="Nombre">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNombre" runat="server" Text='<%# Bind("Nombre") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
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
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" />
                <asp:Panel ID="pnlNuevo" runat="server">
                    <div class="form-group">
                        <label for="ddlDepartamentos">Departamento</label>
                        <asp:DropDownList ID="ddlDepartamentos" class="form-control" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                        <label for="textReubicados">Reubicados</label>
                        <asp:TextBox ID="txtReubicadosForm" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                        <label for="textETT">ETT</label>
                        <asp:TextBox ID="txtETTForm" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                        <label for="textFecha">Fecha</label>
                        <asp:TextBox ID="txtFecha" class="form-control" runat="server" Width="150px" AutoPostBack="false"></asp:TextBox>
                    </div>
                    <div>
                        <br />
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

<script>
     $('.myDatePicker').datetimepicker({
            viewMode: 'months',
            format: 'YYYY/MM',
            maxDate: moment(),
            useCurrent: false
        });


        $(function () {
            $.validator.addMethod('date',
                function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }
                    var valid = true;
                    try {
                        $.datepicker.parseDate('dd/mm/yy', value);
                    }
                    catch (err) {
                        valid = false;
                    }
                    return valid;
                });
        });

</script>
</asp:Content>

