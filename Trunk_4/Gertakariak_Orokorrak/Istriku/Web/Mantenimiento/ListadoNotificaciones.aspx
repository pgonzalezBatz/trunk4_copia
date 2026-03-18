<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ListadoNotificaciones.aspx.vb" Inherits="IstrikuWebRaiz.ListadoNotificaciones" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //function pageLoad(sender, args) { $(document).ready(function () { $get("<=pnlFiltro.ClientID%>").parentElement.style.height = "auto"; }); }

        /* Deteccion NC (Afectado) ****************************************************************************************/
        function Set_Afectado(source, eventArgs) {
            $("#lvAfectados_UL").append('<li><input name="hd_IdAfectados" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtBuscar.ClientID%>').value + ' <a href="#" onclick="Borrar_Afectado(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrarAfectado" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtBuscar.ClientID%>').value = "";
        }
        function Borrar_Afectado(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
        /*******************************************************************************************************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:Panel ID="pnlBuscador" runat="server">
        <ascx:titulo runat="server" id="Titulo" />
        <center>
            <table class="tablaBuscador">
        <tr>
            <td>
                <asp:Panel runat="server" ID="pnlFiltroCabecera">
                    <table  style="width: 98%;border: 1px outset #EBEBEB; background-color: #EBEBEB; margin:2px; vertical-align: middle;">
                        <tr class="ImageButton">
                            <td style="text-align: right;">
                                 <asp:TextBox ID="txtBuscar" runat="server" Width="98%" AutoCompleteType="Search" ToolTip="Buscar" Style="min-width: 100px"></asp:TextBox>
                                <act:textboxwatermarkextender id="wm_txtBuscar" runat="server" targetcontrolid="txtBuscar" watermarktext="Buscar" watermarkcssclass="TextBoxWatermarkExtender" />
                                <act:autocompleteextender id="ace_txtBuscar" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" targetcontrolid="txtBuscar" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                                    servicemethod="get_Usuarios" onclientitemselected="Set_Afectado"
                                    onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>   
         <tr>
             <td>
                 <asp:ListView ID="lvAfectados" runat="server" DataKeyNames="Id" EnableViewState="false">
                    <LayoutTemplate>
                        <ul id="lvAfectados_UL" style="display: table; margin: auto;">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li style="white-space: nowrap;">
                            <input name="hd_IdAfectados" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#" onclick="Borrar_Afectado(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrarAfectado" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvAfectados_UL" style="display: table; margin: auto;"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
             </td>
         </tr>    
    </table>
        </center>
        <div style="width: auto; text-align: center;">
            <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                <asp:ImageButton ID="btnGuardar" runat="server" AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>