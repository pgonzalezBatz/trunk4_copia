<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UsuarioPE.aspx.vb" Inherits="GTK_Troqueleria.UsuarioPE" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        
        function Set_Otros(source, eventArgs) {
            $("#lvOtros_UL").append('<li><input name="hd_IdOtros" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtOtros.ClientID%>').value + ' <a href="#hd_IdOtros" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Otros" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtOtros.ClientID%>').value = "";
        }
        function Set_OF(source, eventArgs) {
            $("#ContentPlaceHolder_FORM_txt_OFOPM").attr("disabled", "disabled");
            $("#linkBorrarOf").css("display", "inline");
        }
        function Borrar_Item(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
        function Borrar_ItemOF(e) {
            $("#ContentPlaceHolder_FORM_txt_OFOPM").removeAttr("disabled");
            $("#ContentPlaceHolder_FORM_txt_OFOPM").val("");
            $("#linkBorrarOf").css("display", "none");

        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <table class="GridViewASP">
        <thead class="HeaderStyle">
            <tr>
                <th style="min-width:200px;">
                    <asp:Label ID="Label7" runat="server" Text="OF" ToolTip="Tipo"></asp:Label>
                </th>
                <th>
                    <asp:Label ID="Label2" runat="server" Text="Usuarios"></asp:Label></th>
            </tr>
        </thead>
        <tbody class="RowStyle">
            <tr>
                <td style="width: 1%;" valign="top">
                        <asp:TextBox ID="txt_OFOPM" runat="server" />
                        <act:textboxwatermarkextender id="TextBoxWatermarkExtender9" runat="server" targetcontrolid="txt_OFOPM" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                        <!-- Texto predictivo -->
                        <act:autocompleteextender id="ace_txt_OFOPM" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txt_OFOPM" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="10" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                            servicemethod="get_OFOPM" onclientitemselected="Set_OF"
                            onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                        <a id="linkBorrarOf" href="#hd_IdOF" onclick="Borrar_ItemOF(this)" style="display: none; vertical-align: middle;">
                            <asp:Image ID="imgBorrar_OF" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                        </a>
                </td>
                <td>
                    <asp:TextBox ID="txtOtros" runat="server" />
                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" TargetControlID="txtOtros" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" TargetControlID="txtOtros" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                        ServiceMethod="get_Usuarios_Aplicacion" OnClientItemSelected="Set_Otros"
                        OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />

                    <asp:ListView ID="lvOtros" runat="server" DataKeyNames="Id">
                        <LayoutTemplate>
                            <ul id="lvOtros_UL">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li>
                                <input name="hd_IdOtros" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                                <a href="#hd_IdOtros" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                    <asp:Image ID="imgBorrar_Otros" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                                </a>
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <ul id="lvOtros_UL"></ul>
                        </EmptyDataTemplate>
                        <EmptyItemTemplate>
                            ??
                        </EmptyItemTemplate>
                    </asp:ListView>

                </td>
            </tr>
        </tbody>
    </table>

     <div style="text-align: center">
        <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
            <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" />
        </asp:Panel>
    </div>
</asp:Content>
