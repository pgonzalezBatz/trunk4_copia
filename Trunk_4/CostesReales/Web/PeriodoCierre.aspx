<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="PeriodoCierre.aspx.vb" Inherits="CostesReales.PeriodoCierre" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="up01">
            <ContentTemplate>
                <asp:Panel ID="pnlPlanta" runat="server" EmptyDataText="No hay datos para mostrar">
                    <div class="form-group">
                        <label for="selectPlanta">Planta</label>
                        <asp:DropDownList ID="ddlPlantas" class="form-control" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlEjercicio" runat="server">
                    <div class="form-group">
                        <label for="selectEjercicio">Ejercicio</label>
                        <asp:DropDownList ID="ddlEjercicios" class="form-control" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                        <asp:LinkButton ID="lnkNuevoEjercicio" CssClass="btn btn-primary" data-toggle="modal" data-target="#modalNuevoEjercicio" runat="server" UseSubmitBehavior="false">Nuevo</asp:LinkButton>
                    </div>
                </asp:Panel>
        
        <asp:Panel ID="pnlPeriodoCierre" runat="server">
            <div>
                <asp:Panel ID="pnlTitulo" runat="server" HorizontalAlign="Center">
                    <%--<asp:Label ID="Label10" runat="server" Text="PERIODO DE CIERRE" class="lbl lbl-primary"></asp:Label>--%>
                </asp:Panel>
                <br />
                <asp:GridView ID="grdPeriodoCierre" runat="server" AutoGenerateColumns="False" DataKeyNames="id" CssClass="table table-responsive"
                    OnRowDeleting="grdPeriodoCierre_RowDeleting"
                    OnRowEditing="grdPeriodoCierre_RowEditing"
                    OnRowUpdating="grdPeriodoCierre_RowUpdating"
                    OnRowCancelingEdit="grdPeriodoCierre_RowCancelingEdit"
                    AllowPaging="True"
                    OnPageIndexChanging="grdPeriodoCierre_PageIndexChanging"
                    PageSize="10">
                    <EmptyDataTemplate>
                        No hay datos para mostrar 
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:CommandField ButtonType="Image" ShowEditButton="True" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png"></asp:CommandField>
                        <asp:TemplateField HeaderText="id" Visible="false">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtId" runat="server" Text='<%# Bind("id") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Anyo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAnyo" runat="server" Text='<%# Bind("Anyo") %>' Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAnyo" runat="server" ControlToValidate="txtAnyo" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Anyo") %>' Enabled="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mes">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlMeses" runat="server" Enabled="false">
                                    <asp:ListItem Value="1">Enero</asp:ListItem>
                                    <asp:ListItem Value="2">Febrero</asp:ListItem>
                                    <asp:ListItem Value="3">Marzo</asp:ListItem>
                                    <asp:ListItem Value="4">Abril</asp:ListItem>
                                    <asp:ListItem Value="5">Mayo</asp:ListItem>
                                    <asp:ListItem Value="6">Junio</asp:ListItem>
                                    <asp:ListItem Value="7">Julio</asp:ListItem>
                                    <asp:ListItem Value="8">Agosto</asp:ListItem>
                                    <asp:ListItem Value="9">Septiembre</asp:ListItem>
                                    <asp:ListItem Value="10">Octubre</asp:ListItem>
                                    <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                    <asp:ListItem Value="12">Diciembre</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Mes") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha_cierre">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFechaCierre" runat="server" Text='<%# Bind("Fecha_cierre") %>' Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFechaCierre" runat="server" ControlToValidate="txtFechaCierre" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Fecha_cierre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Anyo_AA">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAnyoAA" runat="server" Text='<%# Bind("Anyo_AA") %>' Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAnyoAA" runat="server" ControlToValidate="txtAnyoAA" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Anyo_AA") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MesAA">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMesAA" runat="server" Text='<%# Bind("Mes_AA") %>' Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMesAA" runat="server" ControlToValidate="txtMesAA" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Mes_AA") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha_cierre_inicio_mes">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFechaCierreInicioMes" runat="server" Text='<%# Bind("Fecha_cierre_inicio_mes") %>' Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFechaCierreInicioMe" runat="server" ControlToValidate="txtFechaCierreInicioMes" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Fecha_cierre_inicio_mes") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha_TM">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFechaTM" runat="server" Text='<%# Bind("Fecha_TM") %>' Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFecha_TM" runat="server" ControlToValidate="txtFechaTM" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("Fecha_TM") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tasa_chatarra">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTasaChatarra" runat="server" Text='<%# Bind("Tasa_chatarra") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTasaChatarra" runat="server" ControlToValidate="txtTasaChatarra" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("Tasa_chatarra") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                        <asp:TemplateField HeaderText="PYG">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPYG" runat="server" Text='<%# Bind("PYG") %>' Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPYG" runat="server" ControlToValidate="txtPYG" Display="Static" ErrorMessage="* Campo obligatorio" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("PYG") %>'></asp:Label>
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
            </div>
        </asp:Panel>
        <asp:LinkButton ID="btnNuevo" CssClass="btn btn-primary" data-toggle="modal" data-target="#modalNuevo" runat="server" UseSubmitBehavior="false">Nuevo</asp:LinkButton>
                
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Modal -->
        <div class="modal fade" id="modalNuevo" tabindex="-1" role="dialog" aria-labelledby="exampleModarCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="H1">Nuevo periodo de cierre</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <asp:Label ID="lblAnyo" runat="server" Text="Año"></asp:Label>
                            <asp:DropDownList ID="ddlAnyo" runat="server" CssClass="form-control input-group-sm">
                                <asp:ListItem Value="2019">2019</asp:ListItem>
                                <asp:ListItem Value="2020">2020</asp:ListItem>
                                <asp:ListItem Value="2021">2021</asp:ListItem>
                                <asp:ListItem Value="2022">2022</asp:ListItem>
                                <asp:ListItem Value="2023">2023</asp:ListItem>
                                <asp:ListItem Value="2024">2024</asp:ListItem>
                                <asp:ListItem Value="2025">2025</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblMes" runat="server" Text="Mes"></asp:Label>
                            <asp:DropDownList ID="ddlMes" runat="server" CssClass="form-control input-group-sm">
                                <asp:ListItem Value="1">Enero</asp:ListItem>
                                <asp:ListItem Value="2">Febrero</asp:ListItem>
                                <asp:ListItem Value="3">Marzo</asp:ListItem>
                                <asp:ListItem Value="4">Abril</asp:ListItem>
                                <asp:ListItem Value="5">Mayo</asp:ListItem>
                                <asp:ListItem Value="6">Junio</asp:ListItem>
                                <asp:ListItem Value="7">Julio</asp:ListItem>
                                <asp:ListItem Value="8">Agosto</asp:ListItem>
                                <asp:ListItem Value="9">Septiembre</asp:ListItem>
                                <asp:ListItem Value="10">Octubre</asp:ListItem>
                                <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                <asp:ListItem Value="12">Diciembre</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group-required">
                            <asp:Label ID="lblTasaChatarra" runat="server" Text="Tasa Chatarra"></asp:Label>
                            <asp:TextBox ID="txtTasaChatarra" CssClass="form-control input-group-sm" runat="server" class="form-control required"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtTasaChatarra"
                                ErrorMessage="Campo Requerido"
                                ValidationGroup="Validate">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnGuardar" CssClass="btn btn-primary" runat="server" OnClick="btnGuardar_Click" UseSubmitBehavior="false">Guardar</asp:LinkButton>
                        <button type="button" onclick="javascript:window.location.reload()" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
    </div>

    <div class="modal fade" id="modalNuevoEjercicio" tabindex="-1" role="dialog" aria-labelledby="exampleModarCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="H1">Nuevo ejercicio</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Se va a inicializar el ejercicio del año"></asp:Label>:                            
                            <b><asp:Label runat="server" ID="lblAnoEjer"></asp:Label></b>
                        </div>                        
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lnkAnadirEjercicio" CssClass="btn btn-primary" runat="server" UseSubmitBehavior="false">Guardar</asp:LinkButton>
                        <button type="button" onclick="javascript:window.location.reload()" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
    </div>
</asp:Content>
