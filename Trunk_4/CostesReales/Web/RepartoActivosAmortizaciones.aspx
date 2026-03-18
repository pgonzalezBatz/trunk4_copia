<%@ Page Title="" Language="vb" MasterPageFile="~/Master/MPCR.Master" CodeBehind="RepartoActivosAmortizaciones.aspx.vb" Inherits="CostesReales.RepartoActivosAmortizaciones" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
            <%--<asp:Label ID="Label8" runat="server" Text="Reparto Activos Para Amortizaciones" class="lbl lbl-primary"></asp:Label>--%>
        </asp:Panel>
    </div>
    <br />
    <div class="container">

        <asp:UpdatePanel runat="server" ID="up1">
            <ContentTemplate>

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
                            <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png"></asp:CommandField>
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
                                    <asp:DropDownList ID="ddlCriterioReparto" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangeCriterio"></asp:DropDownList>
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
                            <asp:TemplateField HeaderText="Maquina">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlMaquina" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label7b" runat="server" Text='<%# Bind("Maquina") %>'></asp:Label>
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
                </asp:Panel>
                <br />
<%--                <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" runat="server" CssClass="btn btn-primary" data-toggle="modal" data-target="#modalNuevo" UseSubmitBehavior="false">Nuevo</asp:LinkButton>--%>
                    <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" runat="server" CssClass="btn btn-primary">Nuevo</asp:LinkButton>
                <!-- Modal -->

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdAmortizacionesActivos"/>
            </Triggers>
        </asp:UpdatePanel>



        <div class="modal fade" id="modalNuevo" tabindex="-1" role="dialog" aria-labelledby="exampleModarCenterTitle" aria-hidden="true">
            <asp:UpdatePanel runat="server" ID="up2">
                <ContentTemplate>
                    <div class="modal-dialog modal-dialog-centered" role="dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="H1">Nuevo reparto de activos</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <asp:Label ID="lblNumActivo" runat="server" Text="Número de Activo"></asp:Label>
                                    <asp:TextBox ID="txtNumActivoForm" CssClass="form-control input-group-sm" runat="server"></asp:TextBox>                            
                                </div>
                                <div class="form-group" id="divCriterio" runat="server">
                                    <asp:Label ID="lblCriterioReparto" runat="server" Text="Criterio Reparto"></asp:Label>
                                    <asp:DropDownList ID="ddlCriterioRepartoForm" CssClass="form-control input-group-sm" runat="server" AutoPostBack="true"></asp:DropDownList>                            
                                    
                                </div>
                                <div class="form-group" id="divPlanta" runat="server">
                                    <asp:Label ID="lblPlanta" runat="server" Text="Planta"></asp:Label>
                                    <asp:DropDownList ID="ddlPlantaForm" CssClass="form-control input-group-sm" runat="server"></asp:DropDownList>
                                </div>
                                <div class="form-group" id="divProceso" runat="server">
                                    <asp:Label ID="lblProceso" runat="server" Text="Proceso"></asp:Label>
                                    <asp:DropDownList ID="ddlProcesoForm" CssClass="form-control input-group-sm" runat="server"></asp:DropDownList>
                                </div>
                                <div class="form-group" id="divMaquina" runat="server">
                                    <asp:Label ID="lblMaquina" runat="server" Text="Maquina"></asp:Label>
                                    <asp:DropDownList ID="ddlMaquinaForm" CssClass="form-control input-group-sm" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="btnGuardar" CssClass="btn btn-primary" runat="server" OnClick="btnGuardar_Click" OnClientClick="cerrarModal2();" UseSubmitBehavior="false">Guardar</asp:LinkButton>
                                <button type="button" onclick="javascript:window.location.reload()" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            </div>
                        </div>
                    </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCriterioRepartoForm" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            </Triggers>
        </asp:UpdatePanel>
                
                </div>

    </div>

<script type="text/javascript">
    function cerrarModal2() {
        $('#modalNuevo').modal('hide');
    }
</script>

</asp:Content>