<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="OperacionKaplanPrisma.aspx.vb" Inherits="WebRaiz.OperacionKaplanPrisma" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../js/jQuery/jquery-1.7.2.min.js"></script>
    <!-- Numeric --> 
    <script src="../js/jQuery/jquery.numeric.js" type="text/javascript"></script>
    <script type="text/javascript">
        // Esto es así porque funcionamos con un updatepanel y en los postbacks no estaba funcionando
        // el código javascript    
        Sys.Application.add_load(init);
        function init() {
            $('a[id$="lnkEliminar"]').click(function () {
                return confirm(msg_confirm);
            });

            $('.datos-numericos').numeric({ decimal: false, negative: false });
        }
   </script>   
   
   <Titulo:Titulo ID="titKaplanPrisma" Texto="Relación de códigos de operación entre Kaplan y PRISMA" runat="server"  />
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
            <asp:Panel ID="pnlGridView" runat="server" Width="100%">
                <div>
                    <asp:GridView ID="gvKaplanPrisma" runat="server" DataKeyNames="Id" AllowPaging="false"
                        Width="80%" AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None">
                        <RowStyle CssClass="RowStyle" />
                        <FooterStyle CssClass="FooterStyle" />
                        <PagerStyle CssClass="PagerStyle" />
                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                        <HeaderStyle CssClass="HeaderStyle" />
                        <EditRowStyle CssClass="EditRowStyle" />
                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblSinRegistros" runat="server" Text="Sin Datos" />
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Visible="False" Text='<%# Eval("Id")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodOperacionKaplan" HeaderText="Operación Kaplan" />
                            <asp:BoundField DataField="CodOperacionPrisma" HeaderText="Operación Prisma" />
                            <%--<asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditar" CommandName="Edit" runat="server" Text="Editar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Delete" Text="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div>
                    <asp:LinkButton runat="server" ID="lbtnAgregarNuevaRelacion" Text="Añadir nueva relación" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDetailsView" runat="server" Width="80%" CssClass="panel-datos">
                <div>
                    <asp:DetailsView ID="dvKaplanPrisma" runat="server" AllowPaging="False" DefaultMode="Insert"
                        AutoGenerateRows="false" GridLines="None" Width="100%" CssClass="DetailsView">
                        <HeaderStyle CssClass="HeaderStyleDetailsView" />
                        <FieldHeaderStyle CssClass="FieldHeaderStyle" Width="25%" />
                        <RowStyle CssClass="RowStyleDetailsView" />
                        <AlternatingRowStyle CssClass="AlternatingRowStyleDetailsView" />
                        <CommandRowStyle CssClass="CommandRowStyleDetailsView" />
                        <Fields>
                            <asp:TemplateField HeaderText="Id" Visible="False" InsertVisible="True">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" Text='<%# Eval("Id")%>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cod.Operación Kaplan">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditCodOperacionKaplan" runat="server" Text='<%# Eval("CodOperacionKaplan")%>' CssClass="datos-numericos" Width="98%" MaxLength="50"></asp:TextBox>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="txtInsertCodOperacionKaplan" runat="server" Text='<%# Eval("CodOperacionKaplan")%>' CssClass="datos-numericos" Width="98%" MaxLength="50"></asp:TextBox>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cod.Operación Prisma">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditCodOperacionPrisma" runat="server" Text='<%# Eval("CodOperacionPrisma")%>' CssClass="datos-numericos" Width="98%"></asp:TextBox>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="txtInsertCodOperacionPrisma" runat="server" Text='<%# Eval("CodOperacionPrisma")%>' CssClass="datos-numericos" Width="98%"></asp:TextBox>
                                </InsertItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <div style="float: right">
                                        <asp:LinkButton ID="lnkActualizar" CommandName="Update" runat="server" Text="Guardar"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancelar" CommandName="Cancel" runat="server" Text="Cancelar"></asp:LinkButton>
                                    </div>
                                    <div class="clear-float">
                                    </div>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <div style="float: right">
                                        <asp:LinkButton ID="lnkInsertar" CommandName="Insert" runat="server" Text="Guardar"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancelar" CommandName="Cancel" runat="server" Text="Cancelar"></asp:LinkButton>
                                    </div>
                                    <div class="clear-float">
                                    </div>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                </div>
                <br />
                <div>
                    <asp:LinkButton runat="server" ID="lbtnVolver" CausesValidation="False" Text="Listado de relaciones" />
                </div>
                <br />          
            </asp:Panel>
        </ContentTemplate>
       <Triggers>
           <asp:PostBackTrigger ControlID="lbtnVolver" />
           <asp:PostBackTrigger ControlID="lbtnAgregarNuevaRelacion" />
           <asp:PostBackTrigger ControlID="gvKaplanPrisma" />
       </Triggers>
    </asp:UpdatePanel>
    <%--<act:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mdlPopup" runat="server"
               TargetControlID="pnlModal" PopupControlID="pnlModal" OkControlID="btnOk" OnOkScript="okClick();"
               CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlModal" runat="server" Style="display: none; text-align:center" CssClass="modalBox">
		<asp:Label ID="lblConfirmacion" runat="server" Text="Desea eliminar el grupo seleccionado?" />
		<br />
        <br />
        <div style="text-align: center">
            <asp:Button ID="btnOk" runat="server" Text="Sí" Width="50px" />
            <asp:Button ID="btnNo" runat="server" Text="No" Width="50px" />
        </div>
	</asp:Panel>--%>

    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>



