<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.vb" Inherits="GTK_Troqueleria._Default" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%--   <script src="Scripts/defaultaspxScript.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () { $get("<%=pnlFiltro.ClientID%>").parentElement.style.height = "auto"; });
            $find('<%=pce_pnl_Usuarios_Filtro.ClientID%>').add_shown(function () { var obj = $get('<%= txt_Usuarios_Filtro.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnl_Proveedores_Filtro.ClientID%>').add_shown(function () { var obj = $get('<%= txt_Proveedor_Filtro.ClientID%>'); obj.focus(); obj.select(); });
        }

        /**************************************************************************************************************/
        /* Seleccion automatica de subnodos                                                                           */
        /**************************************************************************************************************/
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                //CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
        /**************************************************************************************************************/

        /* Responsable NC (Perseguidor) ************************************************************************************/
        function Set_Usuarios_Filtro(source, eventArgs) {
            $find('<%=pce_pnl_Usuarios_Filtro.ClientID%>').hidePopup();
            $("#lvUsuarios_Filtro_UL").append('<li><input name="hd_IdUsuarios_Filtro" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txt_Usuarios_Filtro.ClientID%>').value + ' <a href="#hd_IdUsuarios_Filtro" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Usuario_Filtro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txt_Usuarios_Filtro.ClientID%>').value = "";
        }
        /* Proveedores NC **************************************************************************************************/
        function Set_Proveedores_Filtro(source, eventArgs) {
            $find('<%=pce_pnl_Proveedores_Filtro.ClientID%>').hidePopup();
            $("#lvProveedores_Filtro_UL").append('<li><input name="hd_IdProveedores_Filtro" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txt_Proveedor_Filtro.ClientID%>').value + ' <a href="#hd_IdProveedores_Filtro" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Proveedor_Filtro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txt_Proveedor_Filtro.ClientID%>').value = "";
        }
        /*******************************************************************************************************************/
        function Borrar_Item(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
        /*******************************************************************************************************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <asp:RadioButtonList ID="rblUsuarios" runat="server" Visible="false" AutoPostBack="true">
        <asp:ListItem Value="batznt\diglesias" Text="Dani Iglesias (Adm)"></asp:ListItem>
        <asp:ListItem Value="batznt\ayuste" Text="Alberto Yuste (Creador)"></asp:ListItem>
        <asp:ListItem Value="batznt\illanos" Text="Iker Llanos (Perseguidor)"></asp:ListItem>
        <asp:ListItem Value="batznt\jzarraga" Text="Jon Zarraga (Gestor)"></asp:ListItem>
        <%--<asp:ListItem Value="mexicana\erodriguez" Text="Evangelina Rodriguez (Responsable NC)"></asp:ListItem>--%>
        <%--<asp:ListItem Value="57745" Text="Asier Arrondo (Proveedor)"></asp:ListItem>--%>
    </asp:RadioButtonList>
    <!-- Buscador ------------------------------------------------------------------------->
    <asp:UpdatePanel ID="upBuscador" runat="server">
        <ContentTemplate>
            <act:CollapsiblePanelExtender ID="cpeFiltro" runat="server" TargetControlID="pnlFiltro" ExpandControlID="imgEstadoPanel" CollapseControlID="imgEstadoPanel" Collapsed="true" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblBusquedaAvanz"
                ExpandedImage="~/App_Themes/Batz/IconosAcciones/Buscador/OpcionesCerrar24.png"
                CollapsedImage="~/App_Themes/Batz/IconosAcciones/Buscador/Opciones24.png"
                ImageControlID="imgEstadoPanel" CollapsedText="Busqueda Avanzada" ExpandedText="Busqueda Avanzada" />
            <asp:Panel ID="pnlBuscador" runat="server" DefaultButton="imgFiltrar">
                <center>
                    <table class="tablaBuscador">
                        <tr>
                            <td>
                                <asp:Panel runat="server" ID="pnlFiltroCabecera">
                                    <table class="recuadro">
                                        <tr class="ImageButton">
                                            <td style="text-align: right;">
                                                <asp:TextBox ID="txtBuscar" runat="server" Width="98%" AutoCompleteType="Search" ToolTip="Buscar" Style="min-width: 100px"></asp:TextBox>
                                                <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtBuscar" WatermarkText="Buscar" WatermarkCssClass="TextBoxWatermarkExtender"></act:TextBoxWatermarkExtender>
                                            </td>
                                            <td style="width: 1%; white-space: nowrap;">&nbsp;<asp:ImageButton ID="imgFiltrar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Buscar" AlternateText="Buscar" ValidationGroup="imgFiltrar" />
                                                &nbsp;<asp:ImageButton ID="imgEliminarFiltro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/EliminarFlitro24.png" AlternateText="eliminarFiltros" ToolTip="eliminarFiltros" />
                                            </td>
                                            <td style="width: 1%">
                                                <asp:ImageButton ID="imgEstadoPanel" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0px;">
                                <asp:Panel ID="pnlFiltro" runat="server" CssClass="recuadro">
                                    <table>
                                        <tr>
                                            <td style="white-space: nowrap;">
                                                <asp:Panel ID="pnlEstado" runat="server" GroupingText="estadoincidencia" ToolTip="estadoincidencia" Width="98%">
                                                    <asp:RadioButtonList ID="rblEstados" runat="server" RepeatDirection="Horizontal" />
                                                </asp:Panel>
                                            </td>
                                            <td style="white-space: nowrap;">
                                                <asp:Panel ID="pnlOrigen" runat="server" GroupingText="Origen No Conformidad" ToolTip="Origen No Conformidad" Width="98%">
                                                    <asp:CheckBoxList ID="cblProcedenciaNC" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Interna (Torre)" Value="1"></asp:ListItem>
                                                        <%--<asp:ListItem Text="Interna (Araluce)" Value="4"></asp:ListItem>--%>
                                                        <asp:ListItem Text="A proveedor" Value="2"></asp:ListItem>
                                                        <%--<asp:ListItem Text="A planta Batz" Value="3" Enabled="false"></asp:ListItem>--%>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlFechaInicio" runat="server" GroupingText="Fecha de Apertura" ToolTip="Fecha de Apertura" Width="98%">
                                                    <asp:Label ID="Label2" runat="server" Text="Desde"></asp:Label>:
                                                    <asp:TextBox ID="txtFechaInicio_Origen" runat="server" Width="85px"></asp:TextBox>
                                                    <act:CalendarExtender ID="ce_txtFechaInicio_Origen" runat="server" TargetControlID="txtFechaInicio_Origen" />
                                                    <act:CalendarExtender ID="imgFechaRevision_CalendarExtender" runat="server" TargetControlID="txtFechaInicio_Origen" PopupButtonID="imgFechaRevision" />
                                                    &nbsp;<asp:ImageButton ID="imgFechaRevision" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                                                    &nbsp;&nbsp;&nbsp;
								                    <asp:Label ID="Label5" runat="server" Text="Hasta"></asp:Label>:
								                    <asp:TextBox ID="txtFechaInicio_Fin" runat="server" Width="85px"></asp:TextBox>
                                                    <act:CalendarExtender ID="ce_txtFechaInicio_Fin" runat="server" TargetControlID="txtFechaInicio_Fin" />
                                                    <act:CalendarExtender ID="imgFechaRevisionFin_CalendarExtender" runat="server" TargetControlID="txtFechaInicio_Fin" PopupButtonID="imgFechaRevisionFin" />
                                                    &nbsp;<asp:ImageButton ID="imgFechaRevisionFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap;">
                                                <asp:Panel ID="Panel1" runat="server" GroupingText="OF" ToolTip="OF" Width="98%">
                                                    <asp:TextBox ID="filtroOF" runat="server"></asp:TextBox>
                                                </asp:Panel>
                                            </td>
                                            <td style="white-space: nowrap;">
                                                <asp:Panel ID="Panel2" runat="server" GroupingText="OP" ToolTip="OP" Width="98%">
                                                    <asp:TextBox ID="filtroOP" runat="server"></asp:TextBox>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>

                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlEstructuras" runat="server" GroupingText="Caracteristicas" ToolTip="Caracteristicas" HorizontalAlign="Left" Width="98%">
                                                    <asp:DataList ID="dlEstructuras" runat="server" GridLines="None" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4" Width="96%">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
                                                        <ItemTemplate>
                                                            <asp:TreeView ID="tvEstructura" runat="server" SkinID="TreeView"
                                                                onclick="OnTreeClick(event)">
                                                            </asp:TreeView>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="vertical-align: top; white-space: nowrap;">
                                                <fieldset style="width: 94%">
                                                    <legend>
                                                        <asp:Label ID="Label1" runat="server" Text="Usuarios"></asp:Label>
                                                        <asp:Image ID="imgBuscar_Usuarios_Filtro" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                                                    </legend>
                                                    <act:PopupControlExtender ID="pce_pnl_Usuarios_Filtro" runat="server" TargetControlID="imgBuscar_Usuarios_Filtro" PopupControlID="pnl_Usuarios_Filtro" Position="Bottom" />
                                                    <asp:Panel ID="pnl_Usuarios_Filtro" runat="server" Width="100%">
                                                        <asp:TextBox ID="txt_Usuarios_Filtro" runat="server" AutoCompleteType="Search" />
                                                        <act:TextBoxWatermarkExtender ID="wm_Usuarios_Filtro" runat="server" TargetControlID="txt_Usuarios_Filtro" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                                                        <!-- Texto predictivo -->
                                                        <act:AutoCompleteExtender ID="ace_txt_Usuarios_Filtro" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" TargetControlID="txt_Usuarios_Filtro" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                                                            ServiceMethod="get_Usuarios_NC" OnClientItemSelected="Set_Usuarios_Filtro"
                                                            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                                                    </asp:Panel>
                                                    <asp:ListView ID="lvUsuarios_Filtro" runat="server" DataKeyNames="Id">
                                                        <LayoutTemplate>
                                                            <ul id="lvUsuarios_Filtro_UL">
                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                            </ul>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <li>
                                                                <%--<input name="hd_IdUsuarios_Filtro" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>--%>
                                                                <input name="hd_IdUsuarios_Filtro" type="hidden" value='<%#Eval("Id")%>'><%# String.Format("{0} {1} {2}", Eval("Nombre"), Eval("Apellido1"), Eval("Apellido2")) %>
                                                                <a href="#hd_IdUsuarios_Filtro" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                                                    <asp:Image ID="imgBorrar_Usuario_Filtro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                                                                </a>
                                                            </li>
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <ul id="lvUsuarios_Filtro_UL"></ul>
                                                        </EmptyDataTemplate>
                                                        <EmptyItemTemplate>
                                                            ??
                                                        </EmptyItemTemplate>
                                                    </asp:ListView>
                                                </fieldset>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label ID="Label4" runat="server" Text="Cliente"></asp:Label>
                                                        -->
                                                        <asp:Label ID="Label6" runat="server" Text="Proyecto"></asp:Label>
                                                    </legend>
                                                    <table class="GridViewASP">
                                                        <tr class="HeaderStyle">
                                                            <th>
                                                                <asp:Label ID="Label9" runat="server" Text="Cliente"></asp:Label></th>
                                                            <th>
                                                                <asp:Label ID="Label10" runat="server" Text="Proyecto"></asp:Label></th>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddl_Clientes" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" ToolTip="Cliente">
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_Proyecto" runat="server" CssClass="form-control" ToolTip="Proyecto"></asp:DropDownList>
                                                                <act:CascadingDropDown ID="cdd_ddl_Proyecto" runat="server" TargetControlID="ddl_Proyecto"
                                                                    Category="Proyecto" PromptText="(Seleccione uno)" LoadingText="Cargando"
                                                                    ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_Proyecto_Cliente" ParentControlID="ddl_Clientes" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset style="width: 94%">
                                                    <legend>
                                                        <asp:Label ID="Label7" runat="server" Text="Unidad de Negocio" ToolTip="UG"></asp:Label></legend>
                                                    <asp:DropDownList ID="ddl_UG" runat="server" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" ToolTip="UG"></asp:DropDownList>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset style="width: 94%">
                                                    <legend>
                                                        <asp:Label ID="Label8" runat="server" Text="Proveedor" ToolTip="Proveedor de la NC"></asp:Label>
                                                        <asp:Image ID="imgBuscar_Proveedor" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                                                    </legend>
                                                    <act:PopupControlExtender ID="pce_pnl_Proveedores_Filtro" runat="server" TargetControlID="imgBuscar_Proveedor" PopupControlID="pnl_Proveedores_Filtro" Position="Bottom" />
                                                    <asp:Panel ID="pnl_Proveedores_Filtro" runat="server" Width="100%">
                                                        <asp:TextBox ID="txt_Proveedor_Filtro" runat="server" AutoCompleteType="Search" />
                                                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txt_Proveedor_Filtro" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                                                        <!-- Texto predictivo -->
                                                        <act:AutoCompleteExtender ID="ace_txt_Proveedor_Filtro" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" TargetControlID="txt_Proveedor_Filtro" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                                                            ServiceMethod="get_Proveedor_Filtro" OnClientItemSelected="Set_Proveedores_Filtro"
                                                            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                                                    </asp:Panel>
                                                    <asp:ListView ID="lvProveedores_Filtro" runat="server" DataKeyNames="Id">
                                                        <LayoutTemplate>
                                                            <ul id="lvProveedores_Filtro_UL">
                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                            </ul>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <li>
                                                                <input name="hd_IdProveedores_Filtro" type="hidden" value='<%#Eval("IDTROQUELERIA")%>'><%# String.Format("{0} ({1} - {2})", Eval("NOMBRE"), Eval("LOCALIDAD"), Eval("PROVINCIA")) %>
                                                                <a href="#hd_IdProveedores_Filtro" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                                                    <asp:Image ID="imgBorrar_Proveedor_Filtro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                                                                </a>
                                                            </li>
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <ul id="lvProveedores_Filtro_UL"></ul>
                                                        </EmptyDataTemplate>
                                                        <EmptyItemTemplate>
                                                            ??
                                                        </EmptyItemTemplate>
                                                    </asp:ListView>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset style="width: 94%">
                                                    <legend>
                                                        <asp:Label ID="Label11" runat="server" Text="Capacidades" ToolTip="Capacidades de proveedores"></asp:Label>
                                                    </legend>
                                                    <asp:DropDownList ID="ddl_Capacidades_Filtro" runat="server" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" ToolTip="Capacidades de proveedores"></asp:DropDownList>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                    <center>
                                        <table style="width: auto; border-collapse: collapse;">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                                                        <asp:ImageButton ID="imgFiltrar2" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Buscar" AlternateText="Buscar" ValidationGroup="imgFiltrar" />
                                                        <asp:ImageButton ID="btnGuardarFiltro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Floppy-Small-icon24.png" ToolTip="Guardar Filtro como predeterminado" AlternateText="Guardar Filtro como predeterminado" ValidationGroup="imgFiltrar" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!------------------------------------------------------------------------------------->
    <hr />

    <asp:UpdatePanel ID="upListadoIncidencias" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlBotonesGv" runat="server" CssClass="PanelBotones">
                <asp:ImageButton ID="btnNuevaIncidencia" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo24.png" AlternateText="Nueva No Conformidad" ToolTip="Nueva No Conformidad" />
                <asp:ImageButton ID="btnImprimir" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Imprimir24.png" AlternateText="INFORME INCIDENCIAS" ToolTip="INFORME INCIDENCIAS" />
            </asp:Panel>
            <asp:Label ID="mensajeOK" runat="server" style="color:green;font-weight:700"></asp:Label>
            <asp:GridView SkinID="GridView" ID="gvGertakariak" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="No Conformidades" AutoGenerateColumns="false">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ItemStyle-Width="1%" />
                    <asp:TemplateField ConvertEmptyStringToNull="false" InsertVisible="true" ItemStyle-Width="1%" ItemStyle-Wrap="false" HeaderText="ID" SortExpression="ID">
                        <ItemTemplate>
                            <asp:Label ID="lblProcedenciaNC" runat="server" Text="?"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" FooterStyle-Wrap="false" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left" />--%>
                    <asp:TemplateField HeaderText="Estado" AccessibleHeaderText="Estado" ItemStyle-Width="1%" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Panel ID="pnlEstado" runat="server" HorizontalAlign="Center">
                                <asp:Image ID="imgEstado" runat="server" />
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Notificacion" AccessibleHeaderText="Notificacion" ItemStyle-Width="1%" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Panel ID="pnlNotificacion" runat="server" HorizontalAlign="Center" Visible="false">
                                <asp:Image ID="ingNotificacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/warning-icon24.png" ToolTip="Sin Notificar" AlternateText="Sin Notificar" />
                            </asp:Panel>
                            <asp:Panel ID="pnlPendientes" runat="server" HorizontalAlign="Center" Visible="false">
                                <asp:Image ID="imgPendientes" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon24.png" ToolTip="Acciones Pendientes" AlternateText="Acciones Pendientes" />
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Etapa" AccessibleHeaderText="Etapa" ItemStyle-Width="1%" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEtapa" runat="server"></asp:Label>
                            <asp:Image ID="imgEtapa" ImageUrl="~/App_Themes/Batz/Imagenes/info.gif" runat="server" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField ConvertEmptyStringToNull="false" InsertVisible="true" HeaderText="Origen" ItemStyle-Width="1%" ItemStyle-Wrap="false">
						<ItemTemplate>
							<asp:Label ID="lblProcedenciaNC" runat="server" Text="?"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>--%>
                    <asp:BoundField DataField="FECHAAPERTURA" HeaderText="FechaApertura" SortExpression="FECHAAPERTURA" DataFormatString="{0:d}"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%" />
                    <%--<asp:BoundField DataField="DESCRIPCIONPROBLEMA" HeaderText="Descripcion" SortExpression="DESCRIPCIONPROBLEMA" />--%>
                    <asp:TemplateField HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderText="Descripcion" SortExpression="DESCRIPCIONPROBLEMA">
                        <ItemTemplate>
                            <%--<div class="Truncar">KAIXO This is some long text that will not fit in the box</div>--%>
                            <%--<span class="span"><%#Eval("DESCRIPCIONPROBLEMA") %></span>--%>
                            <%--<div class="test" style="text-overflow: ellipsis;">This is some long text that will not fit in the box</div>--%>
                            <%--<span class="test" style="text-overflow: ellipsis;">This is some long text that will not fit in the box</span>--%>
                            <%--<asp:Panel ID="Panel1" runat="server" CssClass="TruncarTexto">
                                <asp:Label runat="server" ID="lbl_DESCRIPCIONPROBLEMA2" Text='<%#Eval("DESCRIPCIONPROBLEMA") %>' ></asp:Label>
                            </asp:Panel>
                            <br />--%>
                            <%--<asp:Label runat="server" ID="lbl_DESCRIPCIONPROBLEMA" Text='<%#Eval("DESCRIPCIONPROBLEMA") %>' CssClass="TruncarTexto"></asp:Label>--%>
                            <div class="tooltip">
                                <asp:Label runat="server" ID="lbl_DESCRIPCIONPROBLEMA2" Text='<%#Eval("DESCRIPCIONPROBLEMA") %>' CssClass="TruncarTexto"></asp:Label>
                                <span class="tooltiptext">
                                    <asp:Label runat="server" ID="lbl_DESCRIPCIONPROBLEMA_tooltip" Text='<%#Eval("DESCRIPCIONPROBLEMA") %>'></asp:Label></span>
                            </div>

                            <%--<a class="tooltips" href="#">
                                CSS Tooltips  
                                <asp:Label runat="server" ID="Label12" Text='<%#Eval("DESCRIPCIONPROBLEMA") %>' ></asp:Label></a>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                        <HeaderTemplate>
                            <asp:Label ID="Label3" runat="server" Text="€" ToolTip="Coste de NC"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblImporteNC" ToolTip="Coste de NC"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RETRASO_SEMANAS" HeaderText="Retraso" SortExpression="RETRASO_SEMANAS" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderStyle-Wrap="false">
                        <HeaderTemplate>
                            <asp:Label ID="Label2" runat="server" Text="OF"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblOFs" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="false">
                        <HeaderTemplate>
                            <asp:Label ID="Label3" runat="server" Text="OP"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblOPs" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderText="Proyecto">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblProyecto"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderText="Responsable">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblResponsable"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
<%--                    <asp:TemplateField HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderText="Reabrir">
                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnReabrir" Text="Reabrir incidencia"></asp:Button>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <!------------------------------------------------------------------------------------------------------>
    <!-- Fecha de Revision solo fecha ---------------------------------------------------------------------->
    <asp:CompareValidator ID="cvFechaRevision" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaInicio_Origen" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="imgFiltrar" />
    <act:ValidatorCalloutExtender ID="vce_cvFechaRevision" runat="server" TargetControlID="cvFechaRevision" />
    <asp:CompareValidator ID="cvFechaRevisionFin" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaInicio_Fin" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="imgFiltrar" />
    <act:ValidatorCalloutExtender ID="vce_cvFechaRevisionFin" runat="server" TargetControlID="cvFechaRevisionFin" />
    <!------------------------------------------------------------------------------------------------------>
</asp:Content>
