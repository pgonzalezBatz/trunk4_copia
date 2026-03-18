<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="HorasSerial.aspx.vb" Inherits="CostesReales.HorasSerial" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <%--<asp:ScriptReference Path="~/Scripts/jquery-3.5.1.min.js" />--%>
<%--            <asp:ScriptReference Path="~/Scripts/jquery-2.2.3.min.js" />
            <asp:ScriptReference Path="~/Scripts/jquery-ui-1.11.4.min.js" />
            <asp:ScriptReference Path="~/Scripts/jquery.validate.min.js" />
            <asp:ScriptReference Path="~/Scripts/moment-with-locales.min.js" />
            <asp:ScriptReference Path="~/Scripts/bootstrap-datetimepicker.min.js" />--%>
        </Scripts>
    </asp:ScriptManager>
<%--    <div class="container">
        <asp:Panel ID="pnlTitulo" runat="server">
                <asp:Label ID="Label10" runat="server" Text="Horas Serial" class="lbl lbl-primary"></asp:Label>
            </asp:Panel>
    </div>--%>
    <asp:Panel ID="pnlTitulo" runat="server"></asp:Panel>
    <br />
    <div class="container">

        <asp:Panel ID="pnlHorasSerial" runat="server">
            <asp:GridView ID="grdHorasSerial" runat="server" AutoGenerateColumns="False" AllowPaging="True" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <Columns>
 <%--                   <asp:CommandField ButtonType="Image" ShowEditButton="False" CancelImageUrl="~/Content/img/cancelar24.png" DeleteImageUrl="~/Content/img/eliminar24.png" EditImageUrl="~/Content/img/editar24.png" UpdateImageUrl="~/Content/img/actualizar24.png">
                    </asp:CommandField>--%>
                    <asp:TemplateField HeaderText="Portador">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtPortador" runat="server" Text='<%# Bind("PORTADOR") %>'></asp:TextBox>--%>
                            <asp:Label ID="lblPortador" runat="server" Text='<%# Bind("PORTADOR") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("PORTADOR") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Criterio_Reparto_ID" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCriterioRepartoId" runat="server" Text='<%# Bind("CID") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("CID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Criterio Reparto">
                        <EditItemTemplate>
                            <%--<asp:DropDownList ID="ddlCriterioReparto1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCriterioReparto_SelectedIndexChanged"></asp:DropDownList>--%>
                            <asp:DropDownList ID="ddlCriterioReparto1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangeCriterioGrid"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Criterio_Reparto") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
<%--                    <asp:TemplateField HeaderText="Business_ID" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBusinessID" runat="server" Text='<%# Bind("Business_ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Business_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="Negocio">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlNegocio" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("Lantegi") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Negocio/Máquina">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlNegocioMaquina" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Maquina_Lantegi") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
<%--                    <asp:TemplateField HeaderText="Maquina_ID" Visible="False">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlMaquinaID" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
<%--                    <asp:TemplateField HeaderText="Maquina_des" Visible="False">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlMaquinaDes" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("Maquina_des") %>'></asp:Label>
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
            <br />
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary" />           
            <asp:Button ID="btnListadoMaquinas" runat="server" Text="Listado Máquinas" CssClass="btn btn-primary" />  
        </asp:Panel>
        <br />
<%--        <asp:Panel ID="pnlNuevo" runat="server">
            <div class="form-group">
                <label for="textPortador">Portador</label>
                <asp:TextBox ID="txtPortador" class="form-control" runat="server" Width="250px"></asp:TextBox>
                <label for="textPortador">Criterio Reparto</label>
                <br />
                <asp:DropDownList ID="ddlCriterioReparto" runat="server" AutoPostBack="true"></asp:DropDownList>
                <br />
                <asp:Panel ID="pnlNegocio" runat="server">
                    <div class="form-group">
                        <label for="textNegocio">Negocio</label>
                    </div>
                    <asp:DropDownList ID="ddlNegocio" runat="server">
                    </asp:DropDownList>
                </asp:Panel>
                <asp:Panel ID="pnlMaquinas" runat="server">
                    <div class="form-group">
                        <label for="textMaquinas">Máquina</label>
                    </div>
                    <asp:DropDownList ID="ddlMaquinas" runat="server"></asp:DropDownList>
                </asp:Panel>
                <br />
                <div>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-primary" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-danger" Style="height: 33px" />
                </div>
            </div>
        </asp:Panel>--%>

           <div class="modal fade" id="modalNuevo" tabindex="-1" role="dialog" aria-labelledby="exampleModarCenterTitle" aria-hidden="true">
            <asp:UpdatePanel runat="server" ID="up2">
                <ContentTemplate>
                    <div class="modal-dialog modal-dialog-centered" role="dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="H1">Nuevo reparto serial</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <asp:Label ID="lblPortador" runat="server" Text="Portador"></asp:Label>
                                    <asp:TextBox ID="txtPortador" CssClass="form-control input-group-sm" runat="server"></asp:TextBox>                            
                                </div>
                                <div class="form-group" id="divCriterio" runat="server">
                                    <asp:Label ID="lblCriterioReparto" runat="server" Text="Criterio Reparto"></asp:Label>
                                    <asp:DropDownList ID="ddlCriterioRepartoForm" CssClass="form-control input-group-sm" runat="server" AutoPostBack="true"></asp:DropDownList>                            
                                    
                                </div>
                                <div class="form-group" id="divNegocio" runat="server">
                                    <asp:Label ID="lblNegocio" runat="server" Text="Negocio"></asp:Label>
                                    <asp:DropDownList ID="ddlNegocioForm" CssClass="form-control input-group-sm" runat="server" AppendDataBoundItems="False"></asp:DropDownList>
                                </div>
                                <div class="form-group" id="divMaquina" runat="server">
                                    <asp:Label ID="lblMaquina" runat="server" Text="Maquina"></asp:Label>
                                    <asp:DropDownList ID="ddlMaquinaForm" CssClass="form-control input-group-sm" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="btnGuardar" CssClass="btn btn-primary" runat="server" OnClick="btnGuardar_Click" OnClientClick="cerrarModal2();" UseSubmitBehavior="false">Guardar</asp:LinkButton>
<%--                            <button type="button" onclick="javascript:window.location.reload()" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>--%>
                                <button type="button"class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
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

<%--<script>
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
            //$(".datetype").datepicker({ dateFormat: 'dd/mm/yy' });
        });

</script>--%>

</asp:Content>