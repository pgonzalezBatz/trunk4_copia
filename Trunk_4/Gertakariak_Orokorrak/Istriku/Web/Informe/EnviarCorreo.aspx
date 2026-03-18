<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="EnviarCorreo.aspx.vb" Inherits="IstrikuWebRaiz.EnviarCorreo" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad() { $find('<%=pce_pnlUsuarios.ClientID%>').add_shown(function () { var obj = $get('<%= txtUsuario.ClientID%>'); obj.focus(); obj.select(); }); }
        /* Selector de Usuarios ********************************************************************************************/
        function Set_Usuario(source, eventArgs) {
            $find('<%=pce_pnlUsuarios.ClientID%>').hidePopup();
            $("#lvUsuarios_UL").append('<li><input name="hd_IdUsuarios" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtUsuario.ClientID%>').value + ' <a href="#" onclick="Borrar_Usuario(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrarUsuario" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtUsuario.ClientID%>').value = "";
        }
        function Borrar_Usuario(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
        /*******************************************************************************************************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <ascx:Titulo runat="server" ID="Titulo" Texto="Notificacion de Incidente/Accidente" />

    <asp:Panel ID="pnlListaUsuarios" runat="server" GroupingText="Lista de Usuarios">
        <table class="GridViewASP">
            <tr class="HeaderStyle">
                <th>
                    <asp:Panel ID="pnlBucarUsuario" runat="server" CssClass="PanelBotones">
                        <asp:Image ID="imgBuscarUsuario" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                    </asp:Panel>
                    <asp:Label ID="Label1" runat="server" Text="Usuarios de Notificacion"></asp:Label>
                </th>
            </tr>
            <tr class="RowStyle">
                <td>
                    <asp:ListView ID="lvUsuarios" runat="server" DataKeyNames="Id">
                        <LayoutTemplate>
                            <ul id="lvUsuarios_UL" style="display: table; margin: auto;">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li style="white-space: nowrap;">
                                <input name="hd_IdUsuarios" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                                <a href="#" onclick="Borrar_Usuario(this)" style="display: inline; vertical-align: middle;">
                                    <asp:Image ID="imgBorrarUsuario" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                                </a>
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <ul id="lvUsuarios_UL" style="display: table; margin: auto;"></ul>
                        </EmptyDataTemplate>
                        <EmptyItemTemplate>
                            ??
                        </EmptyItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </asp:Panel>


    <div style="width: auto; text-align: center;">
        <asp:Panel ID="Panel1" runat="server" GroupingText="Confirmar Notificacion">
            <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" />
                <asp:ImageButton ID="btnCancelar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" AlternateText="Cancelar" ToolTip="Cancelar" />
            </asp:Panel>
        </asp:Panel>
    </div>

    <act:PopupControlExtender ID="pce_pnlUsuarios" runat="server" TargetControlID="imgBuscarUsuario" PopupControlID="pnlUsuarios" Position="Right">
    </act:PopupControlExtender>
    <asp:Panel ID="pnlUsuarios" runat="server" Width="100%">
        <asp:TextBox ID="txtUsuario" runat="server" />
        <act:TextBoxWatermarkExtender ID="wmUsuarios" runat="server" TargetControlID="txtUsuario" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
        <!-- Texto predictivo -->
        <act:AutoCompleteExtender ID="ace_txtUsuario" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtUsuario" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
            ServiceMethod="get_Usuarios" OnClientItemSelected="Set_Usuario" 
            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado"/>
    </asp:Panel>
</asp:Content>

<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
