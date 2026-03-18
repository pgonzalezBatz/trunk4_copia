<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="DiferenciaInventario.aspx.vb" Inherits="CostesReales.DiferenciaInventario" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="container">
        <div>
            <%--<asp:Button ID="btnImportarListado" runat="server" Text="Ver archivo" CssClass="btn btn-primary" OnClientClick="alert('recuerda que al importar estos datos se borrará la información existente relativa a las mismas fechas');"/>--%>

            <asp:Button ID="btnConsultar" runat="server" Text="Consultar Listado de BD" CssClass="btn btn-primary" />
            <asp:Button ID="btnInventarioManual" runat="server" Text="Inventario Manual" CssClass="btn btn-primary" />
        </div>
        <br />

        <div class="mb-3">
            <asp:Button ID="btnCargarDatosArchivo" runat="server" Text="Ver datos archivo" CssClass="btn btn-primary" />
            <asp:FileUpload ID="FileUpload1" runat="server" style="display:inline-block"/>
        </div>

        <br />
        <%--        <asp:Label ID="avisoImportacion" runat="server" Text="Atención! Se va a importar la siguiente información a base de datos:"></asp:Label>--%>
        <asp:Button ID="importarABD" runat="server" CssClass="btn btn-success" Text="Importar a BD" />
        <br />
        <br />
        <br />
        <asp:Panel ID="filtroTabla" runat="server" CssClass="container" style="margin-bottom:10px;background-color:#e7e7e7;border-radius:10px;">
            <h2>Filtros</h2>
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
            <br />
            <br />
        <asp:Panel ID="pnlDiferenciaInventario" runat="server">
            <asp:Label runat="server" ID="titleBD" Text="Base de datos" CssClass="col-sm-6 text-center" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="titleFile" Text="Archivo" CssClass="col-sm-6 text-center" Visible="false"></asp:Label>
            <asp:GridView ID="grdDiferenciaInventario" runat="server"
                OnRowDeleting="grdDiferenciaInventario_RowDeleting"
                OnRowEditing="grdDiferenciaInventario_RowEditing"
                OnRowUpdating="grdDiferenciaInventario_RowUpdating"
                OnRowCancelingEdit="grdDiferenciaInventario_RowCancelingEdit"
                AutoGenerateColumns="False"
                DataKeyNames="FECHA_ID" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                AllowPaging="True" EmptyDataText="No hay datos para mostrar"
                CssClass="col-sm-6">
                <Columns>
                    <asp:TemplateField HeaderText="FECHA_ID">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Bind("FECHA_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFechaID" runat="server" Text='<%#Eval("FECHA_ID")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CATEGORIA">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%#Bind("CATEGORIA")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCategoria" runat="server" Text='<%#Eval("CATEGORIA")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="REFERENCIA">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%#Bind("REFERENCIA")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtReferencia" runat="server" Text='<%#Eval("REFERENCIA")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PRECIO_INVENTARIO">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%#Bind("PRECIO_INVENTARIO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPrecioInventario" runat="server" Text='<%#Eval("PRECIO_INVENTARIO")%>'></asp:TextBox>
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
            <%--<div class="col-sm-1"></div>--%>
            <asp:GridView ID="grdImportarListado" runat="server" CssClass="col-sm-5 col-sm-offset-1"></asp:GridView>
            <br />
        </asp:Panel>
        </asp:Panel>


    </div>

    <script>
          $('.myDatePicker').datetimepicker({
            viewMode: 'years',
            format: 'YYYY',
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
