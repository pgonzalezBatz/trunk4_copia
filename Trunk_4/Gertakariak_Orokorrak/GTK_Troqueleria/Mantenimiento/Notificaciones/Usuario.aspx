<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Usuario.aspx.vb" Inherits="GTK_Troqueleria.Usuario" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Set_Otros(source, eventArgs) {
            $("#lvOtros_UL").append('<li><input name="hd_IdOtros" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtOtros.ClientID%>').value + ' <a href="#hd_IdOtros" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Otros" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtOtros.ClientID%>').value = "";
        }
        function Borrar_Item(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <table class="GridViewASP">
        <thead class="HeaderStyle">
            <tr>
                <th>
                    <asp:Label ID="Label7" runat="server" Text="Tipo notificación" ToolTip="Tipo"></asp:Label>
                </th>
                <th>
                    <asp:Label ID="Label2" runat="server" Text="Usuarios"></asp:Label></th>
            </tr>
        </thead>
        <tbody class="RowStyle">
            <tr>
                <td style="width: 1%;" valign="top">
                    <asp:DropDownList runat="server" ID="ddl" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value">
                        <asp:ListItem Value="" Text="(Seleccione uno)"></asp:ListItem>
                    </asp:DropDownList>
                </td>

                <td>
                    <asp:TextBox ID="txtOtros" runat="server" />
                    <asp:Image ID="imgBuscar" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" ImageAlign="Middle" />
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

    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <!------------------------------------------------------------------------------------------------------>
    <asp:RequiredFieldValidator ID="rfv_ddl" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="ddl" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rfv_ddl" runat="server" TargetControlID="rfv_ddl" />
    <!------------------------------------------------------------------------------------------------------>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
