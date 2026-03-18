<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LineaCoste.aspx.vb" Inherits="GTK_Troqueleria.LineaCoste" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $find('<%=pce_pnlProveedor.ClientID%>').add_shown(function () { var obj = $get('<%= txtProveedor.ClientID%>'); obj.focus(); obj.select(); });
        }

        /* Proveedor de la Procedencia *************************************************************************************/
        function Set_Proveedor(source, eventArgs) {
            $get('<%= hdIdProveedor.ClientID%>').value = eventArgs.get_value();
            $get('<%= lblProveedor.ClientID%>').innerHTML = eventArgs.get_text();
            $find('<%=pce_pnlProveedor.ClientID%>').hidePopup();
        }
        function ValidarProveedor(source, arguments) {
            arguments.IsValid = ($get('<%= hdIdProveedor.ClientID%>').value != "");
        }

        function RecalcularImporte(idHoras, idTasa) {
            var txtHoras = $get(idHoras.id);
            var txtTasa = $get(idTasa.id);
            var datoHoras = txtHoras.value;
            var datoTasa = txtTasa.value;
            var resul = datoHoras.replace(/,/g, '.') * datoTasa.replace(/,/g, '.');
            $('#ContentPlaceHolder_FORM_txtImporte').val(resul);
            $('#ContentPlaceHolder_FORM_hfTxtImporte').val(resul);
        }

        function MirarGestionGTK(idDescripcion) {
            var texto = $get(idDescripcion.id).value.toLowerCase();
            if (texto.includes('gestión de gtk') || texto.includes('gestion de gtk') || texto.includes('gestión de la gtk') || texto.includes('gestion de la gtk')) {
                $get('<%= txtHoras.ClientID %>').value = 0.5;
                $get('<%= txtHoras.ClientID %>').setAttribute('disabled', 'disabled');
            } else {
                $get('<%= txtHoras.ClientID %>').removeAttribute('disabled');
            }
        }

        /*******************************************************************************************************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº" />
    <asp:Panel ID="Panel2" runat="server">
        <table class="GridViewASP" style="width: 1%;">
            <tr class="HeaderStyle">
                <th>
                    <asp:Label ID="Label5" runat="server" Text="Nº Pedido Origen" />
                </th>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPedidosOrig" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" ToolTip="Depende del proveedor y OF-OP-Marca seleccionados en la NC.">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlDatosNoModificar" runat="server">
        <table class="GridViewASP" style="width: 1%;">
            <tr class="HeaderStyle">
                <%--<th>
                    <asp:Label ID="Label5" runat="server" Text="Nº Pedido Origen" />
                    </th>--%>
                <th>
                    <asp:Label ID="Label6" runat="server" Text="Origen" />
                </th>
                <th>
                    <asp:Label ID="Label7" runat="server" Text="ofop"></asp:Label></th>
                <th>
                    <asp:Label ID="Label8" runat="server" Text="Marca"></asp:Label>
                </th>
            </tr>
            <tr class="RowStyle">
                <%--<td>
                    <asp:DropDownList runat="server" ID="ddlPedidosOrig" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" ToolTip="Depende del proveedor y OF-OP-Marca seleccionados en la NC.">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
                <td>
                    <asp:DropDownList runat="server" ID="ddlOrigen" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="bonos" Value="B"></asp:ListItem>
                        <asp:ListItem Text="Material" Value="M"></asp:ListItem>
                        <asp:ListItem Text="subcontratacion" Value="S"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlOfOp" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-control" ToolTip="Marca"></asp:DropDownList>
                    <act:CascadingDropDown ID="cdd_ddlMarca" runat="server" TargetControlID="ddlMarca"
                        Category="" PromptText="(Seleccione uno)" LoadingText="Cargando"
                        ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_Marcas" ParentControlID="ddlOfOp" />

                </td>
            </tr>
        </table>
        <table class="GridViewASP" style="width: auto;">
            <tr class="HeaderStyle">
                <th style="width: 1%">
                    <asp:Label ID="Label1" runat="server" Text="Descripcion" /></th>
                <td class="RowStyle">
                    <asp:TextBox ID="txtDescripcion" runat="server" Width="100%" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="rev_txtDescripcion" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="Solo 2000 Caracteres" ValidationExpression="^[\s\S]{0,2000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnAceptar" />
                    <act:ValidatorCalloutExtender ID="vce_rev_txtDescripcion" TargetControlID="rev_txtDescripcion" runat="server" PopupPosition="TopLeft" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table class="GridViewASP" style="width: 1%;">
        <tr class="HeaderStyle">
            <%--<th>
                <asp:Label ID="Label4" runat="server" Text="Concepto" />
            </th>--%>
            <th style="width: 1%">
                <asp:Label ID="Label2" runat="server" Text="Nº Horas"></asp:Label>
            </th>
            <th style="width: 1%">
                <asp:Label ID="Label4" runat="server" Text="Cantidad de Material"></asp:Label>
            </th>
            <th style="width: 1%">
                <asp:Label ID="Label3" runat="server" Text="Tasa"></asp:Label>
            </th>
            <th style="width: 1%">
                <asp:Label ID="Label10" runat="server" Text="Importe"></asp:Label>
            </th>
            <th style="width: 1%">
                <asp:Label ID="Label9" runat="server" Text="Proveedor"></asp:Label>
            </th>
        </tr>
        <tr class="RowStyle">
            <%--<td>
                <asp:DropDownList ID="cbConcepto" runat="server" AppendDataBoundItems="true" DataValueField="Value" DataTextField="Text">
                    <asp:ListItem Value="" Text="(Seleccione uno)"></asp:ListItem>
                </asp:DropDownList>
            </td>--%>
            <td>
                <asp:TextBox ID="txtHoras" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txtTasa" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txtImporte" runat="server" readonly="true" style="pointer-events:none"></asp:TextBox>
                <asp:HiddenField ID="hfTxtImporte" runat="server" />
            </td>
            <td>
                <asp:Panel ID="pnlProveedores" runat="server">
                    <table>
                        <tr>
                            <td style="width: 1%">
                                <asp:Panel ID="Panel1" runat="server" CssClass="PanelBotones">
                                    <asp:Image ID="imgFiltrar_Proveedor" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                                </asp:Panel>
                                <asp:HiddenField ID="hdIdProveedor" runat="server" />
                                <act:PopupControlExtender ID="pce_pnlProveedor" runat="server" TargetControlID="imgFiltrar_Proveedor" PopupControlID="pnlProveedor" Position="Right" />
                                <asp:Panel ID="pnlProveedor" runat="server" Width="100%">
                                    <asp:TextBox ID="txtProveedor" runat="server"></asp:TextBox>
                                    <!-- Texto predictivo -->
                                    <act:AutoCompleteExtender ID="ace_txtProveedor" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_Proveedor" TargetControlID="txtProveedor" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                                        OnClientItemSelected="Set_Proveedor"
                                        OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                                </asp:Panel>
                            </td>
                            <td style="white-space: nowrap; text-align: left;">
                                <ul>
                                    <li>
                                        <asp:Label ID="lblProveedor" runat="server" Text="?"></asp:Label></li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>

        </tr>
    </table>

    <div style="text-align: center">
        <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
            <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnAceptar" />
            <asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" />
            <act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" Enabled="True" />
        </asp:Panel>
    </div>
    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <!------------------------------------------------------------------------------------------------------>
    <!-- Campo Numerico Decimal -->
    <asp:RegularExpressionValidator ID="rev_txtHoras" ControlToValidate="txtHoras" ValidationExpression="^[+-]?(?:\d+\.?\d*|\d+\,?\d*|\d*\,?\d+|\d*\.?\d+)[\r\n]*$" Display="None" runat="server" ErrorMessage="advDebeSerNumerico" ValidationGroup="btnAceptar"></asp:RegularExpressionValidator>
    <act:ValidatorCalloutExtender ID="vce_rev_txtHoras" TargetControlID="rev_txtHoras" runat="server" />
    <asp:RegularExpressionValidator ID="rev_txtImporte" ControlToValidate="txtImporte" ValidationExpression="^[+-]?(?:\d+\.?\d*|\d+\,?\d*|\d*\,?\d+|\d*\.?\d+)[\r\n]*$" Display="None" runat="server" ErrorMessage="advDebeSerNumerico" ValidationGroup="btnAceptar"></asp:RegularExpressionValidator>
    <act:ValidatorCalloutExtender ID="vce_rev_txtImporte" TargetControlID="rev_txtImporte" runat="server" PopupPosition="right" />
    <!-- Campo Numerico -->
    <asp:RegularExpressionValidator ID="rev_txtCantidad" ControlToValidate="txtCantidad" ValidationExpression="[0-9]*" Display="None" runat="server" ErrorMessage="advDebeSerNumerico" ValidationGroup="btnAceptar"></asp:RegularExpressionValidator>
    <act:ValidatorCalloutExtender ID="vce_rev_txtCantidad" TargetControlID="rev_txtCantidad" runat="server" />
    <!------------------------------------------------------------------------------------------------------>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>