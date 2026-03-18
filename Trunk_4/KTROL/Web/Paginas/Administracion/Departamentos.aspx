<%@ Page Language="vb" AutoEventWireup="true" MasterPageFile="~/MPWeb.Master" CodeBehind="Departamentos.aspx.vb" Inherits="WebRaiz.Departamentos" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../js/jQuery/jquery-1.7.2.min.js"></script>
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
   
   <Titulo:Titulo ID="titDepartamento" Texto="Departamentos" runat="server"  />
   <br />   
   <asp:Panel ID="pnlDepartamentos" runat="server">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>               
               <asp:Panel ID="pnlGridView" runat="server" Width="100%" >
                   <div>
                       <asp:Label ID="lblPlantas" runat="server" Text="Plantas" />
                       <asp:DropDownList ID="ddlPlantas" runat="server" DataTextField="Nombre" DataValueField="Id" OnSelectedIndexChanged="ddlPlantas_SelectedIndexChanged" AutoPostBack="true" />
                   </div>
                   <br /><br />
                   <div>
                       <asp:GridView ID="gvDepartamentos" runat="server" DataKeyNames="Id" AllowPaging="true" PageSize="20"
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
                                       <asp:Label ID="lblId" runat="server" Visible="False" Text='<%# Eval("Id") %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lnkEditar" CommandName="Edit" runat="server" Text="Editar"></asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClientClick="showConfirm(this); return false;"
                                           OnClick="lnkEliminar_Click" CommandArgument='<%# Eval("Id") %>' />
                                   </ItemTemplate>
                               </asp:TemplateField>
                           </Columns>
                       </asp:GridView>
                   </div>
                   <br />
                   <div>
                       <asp:LinkButton runat="server" ID="lbtnAgregarNuevoDepartamento" Text="Añadir nuevo departamento" />
                   </div>
               </asp:Panel>
               <asp:Panel ID="pnlDetailsView" runat="server" Width="50%" CssClass="panel-datos">
                   <div>
                       <asp:DetailsView ID="dvDepartamento" runat="server" AllowPaging="False" DefaultMode="Insert"
                           AutoGenerateRows="false" GridLines="None" Width="100%" CssClass="DetailsView">
                           <HeaderStyle CssClass="HeaderStyleDetailsView" />
                           <FieldHeaderStyle CssClass="FieldHeaderStyle" Width="20%" />
                           <RowStyle CssClass="RowStyleDetailsView" />
                           <AlternatingRowStyle CssClass="AlternatingRowStyleDetailsView" />
                           <CommandRowStyle CssClass="CommandRowStyleDetailsView" />
                           <Fields>         
                               <asp:TemplateField HeaderText="Id" Visible="False" InsertVisible="True">
                                   <ItemTemplate>
                                       <asp:Label ID="lblId" Text='<%# Eval("Id") %>' runat="server"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>                      
                               <asp:TemplateField HeaderText="Departamento">
                                   <EditItemTemplate>
                                       <asp:DropDownList ID="ddlEditDepartamento" DataTextField="Nombre" DataValueField="Id"
                                           runat="server" Enabled="false">
                                       </asp:DropDownList>
                                   </EditItemTemplate>
                                   <InsertItemTemplate>
                                       <asp:DropDownList ID="ddlInsertDepartamento" DataTextField="Nombre" DataValueField="Id"
                                           runat="server" Enabled="true">
                                       </asp:DropDownList>
                                   </InsertItemTemplate>
                               </asp:TemplateField>                               
                               <asp:TemplateField HeaderText="Ruta base acceso">
                                   <InsertItemTemplate>
                                       <asp:TextBox ID="txtInsertRutaAcceso" Text='<%# Eval("RutaAcceso")%>' runat="server" Width="98%"></asp:TextBox>
                                   </InsertItemTemplate>
                                   <EditItemTemplate>
                                       <asp:TextBox ID="txtEditRutaAcceso" Text='<%# Eval("RutaAcceso")%>' runat="server" Width="98%"></asp:TextBox>
                                   </EditItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Tipo">
                                   <InsertItemTemplate>
                                       <asp:RadioButtonList ID="rblInsertTipo" runat="server">
                                            <asp:ListItem Text="Calidad" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Operario" Value="1"></asp:ListItem>
                                       </asp:RadioButtonList>
                                   </InsertItemTemplate>
                                   <EditItemTemplate>
                                       <asp:RadioButtonList ID="rblEditTipo" runat="server">
                                            <asp:ListItem Text="Calidad" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Operario" Value="1"></asp:ListItem>
                                       </asp:RadioButtonList>
                                   </EditItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField>
                                   <EditItemTemplate>
                                       <div style="float: right">
                                           <asp:LinkButton ID="lnkActualizar" CommandName="Update" runat="server" Text="Guardar"></asp:LinkButton><%-- OnDataBinding="PostBackBind_DataBinding"--%>
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
                       <asp:LinkButton runat="server" ID="lbtnVolver" CausesValidation="False" Text="Listado de departamentos" />
                   </div>
               </asp:Panel>
           </ContentTemplate>
           <Triggers>
               <asp:PostBackTrigger ControlID="lbtnVolver" />
               <asp:PostBackTrigger ControlID="lbtnAgregarNuevoDepartamento" />
               <asp:PostBackTrigger ControlID="gvDepartamentos" />
           </Triggers>
       </asp:UpdatePanel>
   </asp:Panel>
    <act:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mdlPopup" runat="server"
               TargetControlID="pnlModal" PopupControlID="pnlModal" OkControlID="btnOk" OnOkScript="okClick();"
               CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlModal" runat="server" Style="display: none; text-align:center" CssClass="modalBox">
		<asp:Label ID="lblConfirmacion" runat="server" Text="¿Desea eliminar el departamento seleccionado?" />
		<br />
        <br />
        <div style="text-align: center">
            <asp:Button ID="btnOk" runat="server" Text="Sí" Width="50px" />
            <asp:Button ID="btnNo" runat="server" Text="No" Width="50px" />
        </div>
	</asp:Panel>

    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>

