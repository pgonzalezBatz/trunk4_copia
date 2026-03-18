<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Usuarios.aspx.vb" Inherits="WebRaiz.Usuarios" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../../js/jQuery/jquery-1.7.2.min.js"></script>
    <script src="../../js/jQuery/usuarios.js" type="text/javascript"></script>
    <script type="text/javascript">
        var _source;
        var _popup;

        function showConfirm(source) {
            this._source = source;
            this._popup = $find('mdlPopup');
            this._popup.show();
        }
        function okClick() {
            this._popup.hide();
            var mensaje = this._source.href;
            var posicion1 = mensaje.indexOf("'");
            var posicion2 = mensaje.indexOf(',');
            var id = mensaje.substring(posicion1, posicion2);
            id = id.substring(1, id.length - 1);
            __doPostBack(id, '');
        }

        function cancelClick() {
            this._popup.hide();
            this._source = null;
            this._popup = null;
        }
   </script>
   
   <Titulo:Titulo ID="titUsuario" Texto="usuarios" runat="server" />   
   <asp:Panel ID="pnlUsuarios" runat="server">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
               <asp:Panel ID="pnlGridView" runat="server" Width="100%">                                                     
                   <div>
                       <asp:GridView ID="gvUsuarios" runat="server" DataKeyNames="Id" AllowPaging="true" PageSize="20"
                           Width="40%" AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None">
                           <RowStyle CssClass="RowStyle" />
                           <FooterStyle CssClass="FooterStyle" />
                           <PagerStyle CssClass="PagerStyle" />
                           <HeaderStyle CssClass="HeaderStyle" />
                           <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                           <EmptyDataTemplate>
                               <asp:Label ID="lblSinRegistros" runat="server" Text="Sin usuarios" />
                           </EmptyDataTemplate>
                           <Columns>
                               <asp:TemplateField HeaderText="Id" Visible="False">
                                   <ItemTemplate>
                                       <asp:Label ID="lblId" runat="server" Visible="False" Text='<%# Eval("Id") %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Nombre" >
                                   <ItemTemplate>
                                       <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("NombreUsuario").ToString().ToUpper() %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lnkEditar" CommandName="Edit" runat="server" Text="Editar"></asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClientClick="showConfirm(this); return false;" OnClick="lnkEliminar_Click" CommandArgument='<%# Eval("Id") %>' />
                                   </ItemTemplate>
                               </asp:TemplateField>
                           </Columns>
                       </asp:GridView>
                   </div>
                   <br />
                   <div>
                       <asp:LinkButton runat="server" ID="lbtnAgregarNuevoUsuario" Text="Añadir usuario nuevo" />
                   </div>
               </asp:Panel>
               <asp:Panel ID="pnlDetailsView" runat="server" Width="50%" CssClass="panel-datos">
                   <div>
                       <asp:DetailsView ID="dvUsuario" runat="server" AllowPaging="False" DefaultMode="Insert"
                           AutoGenerateRows="false" GridLines="None" Width="100%" CssClass="DetailsView" Caption="Editar usuario">
                           <HeaderStyle CssClass="HeaderStyleDetailsView" />
                           <FieldHeaderStyle CssClass="FieldHeaderStyle" Width="20%" />
                           <RowStyle CssClass="RowStyleDetailsView" />
                           <AlternatingRowStyle CssClass="AlternatingRowStyleDetailsView" />
                           <CommandRowStyle CssClass="CommandRowStyleDetailsView" />
                           <Fields>
                               <asp:TemplateField HeaderText="Id" Visible="False" InsertVisible="True">
                                   <ItemTemplate>
                                       <asp:Label ID="lblId" Text='<%# Eval("CodPersona") %>' runat="server"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText=" Nombre Usuario">                                   
                                   <InsertItemTemplate>
                                       <div id="imgSeleccion" class="imagen-no-seleccionado" runat="server">
                                       </div>
                                       <div style="float: left; width: 95%">
                                           <asp:TextBox ID="txtInsertUsuario" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                       </div>
                                       <div class="clear-float">
                                       </div>
                                       <asp:HiddenField runat="server" ID="hfIdUsuario" />
                                       <div id="helper" style="margin-top: -1px;" runat="server">
                                       </div>
                                   </InsertItemTemplate>
                                   <EditItemTemplate>
                                       <asp:TextBox ID="txtUsuario" runat="server" Width="100%" Text='<%# Eval("NombreUsuario")%>' ReadOnly="true"></asp:TextBox>
                                   </EditItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Rol Usuario">                                   
                                   <InsertItemTemplate>
                                       <asp:Dropdownlist ID="ddlInsertRolUsuario" runat="server">
                                       
                                       </asp:Dropdownlist>                                       
                                   </InsertItemTemplate>
                                   <EditItemTemplate>
                                       <asp:DropDownList ID="ddlEditRolUsuario" runat="server" />
                                   </EditItemTemplate>
                               </asp:TemplateField>                                                    
                               <asp:TemplateField>                                   
                                   <InsertItemTemplate>
                                       <div style="float: right">
                                           <asp:LinkButton ID="lnkInsertar" CommandName="Insert" runat="server" Text="Guardar"></asp:LinkButton>
                                           <asp:LinkButton ID="lnkCancelar" CommandName="Cancel" runat="server" Text="Cancelar"></asp:LinkButton>
                                       </div>
                                       <div class="clear-float">
                                       </div>
                                   </InsertItemTemplate>
                                   <EditItemTemplate>
                                       <div style="float: right">
                                           <asp:LinkButton ID="lnkModificar" CommandName="Update" runat="server" Text="Modificar"></asp:LinkButton>
                                           <asp:LinkButton ID="lnkCancelar" CommandName="Cancel" runat="server" Text="Cancelar"></asp:LinkButton>
                                       </div>
                                       <div class="clear-float">
                                       </div>
                                   </EditItemTemplate>
                               </asp:TemplateField>
                           </Fields>
                       </asp:DetailsView>
                   </div>
                   <br />
                   <div>
                       <asp:LinkButton runat="server" ID="lbtnVolver" CausesValidation="False" Text="Listado de usuarios" />
                   </div>
               </asp:Panel>
           </ContentTemplate>
           <Triggers>
               <asp:PostBackTrigger ControlID="lbtnVolver" />
               <asp:PostBackTrigger ControlID="lbtnAgregarNuevoUsuario" />
               <asp:PostBackTrigger ControlID="gvUsuarios" />
           </Triggers>
       </asp:UpdatePanel>
   </asp:Panel>
    <act:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mdlPopup" runat="server"
               TargetControlID="pnlModal" PopupControlID="pnlModal" OkControlID="btnOk" OnOkScript="okClick();"
               CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlModal" runat="server" Style="display: none; text-align:center" CssClass="modalBox">
		<asp:Label ID="lblConfirmacion" runat="server" Text="Desea eliminar el usuario seleccionado?" />
		<br />
        <br />
        <div style="text-align: center">
            <asp:Button ID="btnOk" runat="server" Text="Sí" Width="50px" />
            <asp:Button ID="btnNo" runat="server" Text="No" Width="50px" />
        </div>
	</asp:Panel>

    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>


