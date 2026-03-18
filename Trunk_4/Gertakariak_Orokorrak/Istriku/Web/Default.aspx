<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Default.aspx.vb" Inherits="IstrikuWebRaiz._Default" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () { $get("<%=pnlFiltro.ClientID%>").parentElement.style.height = "auto"; });

            $("#<%=imgEliminarFiltro.ClientID%>").click(function () {
                $.each($('input'), function (i, val) {
                    if ($(this).attr("name") == "hd_IdAfectados") { $(this).remove(); }
                });
            });

        }

        /* Deteccion NC (Afectado) ****************************************************************************************/
        function Set_Afectado(source, eventArgs) {
            $("#lvAfectados_UL").append('<li><input name="hd_IdAfectados" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtAfectado.ClientID%>').value + ' <a href="#" onclick="Borrar_Afectado(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrarAfectado" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtAfectado.ClientID%>').value = "";
        }
        function Borrar_Afectado(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
        /*******************************************************************************************************************/
        function MutExChkList(chk) {
            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++)
                if (chks[i] != chk && chk.checked)
                    chks[i].checked = false;
        }

        /*******************************************************************************************************/
        /*Estado incidencia*/
        /*******************************************************************************************************/
        var radioButtons = document.getElementsByName("<%= rblEstados.UniqueID%>");
        function getCheckedRadio() {
            var txtFechaCierre_Origen = document.getElementById("<%= txtFechaCierre_Origen.ClientID%>");
            var txtFechaCierre_Fin = document.getElementById("<%= txtFechaCierre_Fin.ClientID%>");
            for (var x = 0; x < radioButtons.length; x++) {
                if ((radioButtons[x].checked) && radioButtons[x].value == "0") {
                    txtFechaCierre_Origen.value = "";
                    txtFechaCierre_Fin.value = "";
                }
            }
        }
        function SetCheckedRadio() {
            radioButtons[0].checked = true
        }
        /*******************************************************************************************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <!-- Buscador =================================================================================-->
    <asp:UpdatePanel ID="upBuscador" runat="server">
        <ContentTemplate>
            <%--"RoundedCornersExtender" no funciona en IE8 si se combina con "ModalPopupExtender"--%>
            <%--<act:RoundedCornersExtender ID="rce_pnlBuscador" runat="server" TargetControlID="pnlBuscador" Radius="10" Corners="All" BorderColor="ActiveBorder" Color="AliceBlue" />--%>
            <%--<act:CollapsiblePanelExtender ID="cpeFiltro" runat="server" TargetControlID="pnlFiltro" ExpandControlID="imgEstadoPanel" CollapseControlID="imgEstadoPanel" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblBusquedaAvanz" ExpandedImage="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Extender.png" CollapsedImage="~/App_Themes/Tema1/IconosAcciones/PanelExtensible/Contraer.png" ImageControlID="imgEstadoPanel" CollapsedText="Busqueda Avanzada" ExpandedText="Busqueda Avanzada" Collapsed="False" />--%>
            <act:CollapsiblePanelExtender ID="cpeFiltro" runat="server" TargetControlID="pnlFiltro" ExpandControlID="imgEstadoPanel" CollapseControlID="imgEstadoPanel" Collapsed="True" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblBusquedaAvanz"
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
                                                <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtBuscar" WatermarkText="Buscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                                            </td>
                                            <td style="width: 1%; white-space: nowrap;">&nbsp;<asp:ImageButton ID="imgFiltrar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Buscador/Filtrar24.png" ToolTip="Buscar" AlternateText="Buscar" />
                                                &nbsp;<asp:ImageButton ID="imgEliminarFiltro" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Buscador/EliminarFiltro24.png" AlternateText="eliminarFiltros" ToolTip="eliminarFiltros" />
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
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlTipoSuceso" runat="server" GroupingText="Tipo Suceso" ToolTip="Tipo Suceso" Width="98%" BackColor="#cccccc">
                                                    <asp:CheckBoxList ID="cblTipoSuceso" runat="server" RepeatDirection="Horizontal" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlEstado" runat="server" GroupingText="estadoincidencia" ToolTip="estadoincidencia" Width="98%" BackColor="#cccccc">
                                                    <asp:RadioButtonList ID="rblEstados" runat="server" RepeatDirection="Horizontal" onclick="javascript:getCheckedRadio();" />
                                                </asp:Panel>
                                            </td>

                                        </tr>
                                        <tr>
<%--                                            <td>
                                                <asp:Panel ID="pnlInformeFinal" runat="server" GroupingText="Informe Final" ToolTip="Informe Final" Width="98%" BackColor="#cccccc">
                                                    <asp:CheckBoxList ID="cblInformeFinal" runat="server" RepeatDirection="Vertical" />
                                                </asp:Panel>
                                            </td>--%>
<%--                                            <td>
                                                <asp:Panel ID="pnlModificarEvaluacion" runat="server" GroupingText="Modificar Evaluacion" ToolTip="Modificar Evaluacion" Width="98%" BackColor="#cccccc">
                                                    <asp:CheckBoxList ID="cblModificarEvaluacion" runat="server" RepeatDirection="Horizontal" />
                                                </asp:Panel>
                                            </td>--%>
                                            <td>
                                                <asp:Panel ID="pnlRiesgo" runat="server" GroupingText="Riesgo" ToolTip="Riesgo" Width="98%" BackColor="#cccccc">
                                                    <asp:CheckBoxList ID="cblRiesgo" runat="server" RepeatDirection="Vertical" />
                                                </asp:Panel>
                                            </td>                                          
                                            <td>
                                                <asp:Panel ID="pnlNivelderiesgo" runat="server" GroupingText="Nivel de riesgo" ToolTip="Nivel de riesgo" Width="98%" BackColor="#cccccc">
                                                    <asp:CheckBoxList ID="cblNivelderiesgo" runat="server" RepeatDirection="Vertical" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <asp:Panel ID="pnlFechaInicio" runat="server" GroupingText="Fecha de Apertura" ToolTip="Fecha de Apertura" Width="98%" BackColor="#cccccc">
                                                                <asp:Label ID="Label2" runat="server" Text="Desde"></asp:Label>:
																<asp:TextBox ID="txtFechaInicio_Origen" runat="server" Width="85px"></asp:TextBox>
                                                                <act:CalendarExtender ID="ce_txtFechaInicio_Origen" runat="server" TargetControlID="txtFechaInicio_Origen" />
                                                                <act:CalendarExtender ID="imgFechaRevision_CalendarExtender" runat="server" TargetControlID="txtFechaInicio_Origen" PopupButtonID="imgFechaRevision" />
                                                                &nbsp;<asp:ImageButton ID="imgFechaRevision" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                                                                <br />
                                                                <asp:Label ID="Label5" runat="server" Text="Hasta"></asp:Label>:
																<asp:TextBox ID="txtFechaInicio_Fin" runat="server" Width="85px"></asp:TextBox>
                                                                <act:CalendarExtender ID="ce_txtFechaInicio_Fin" runat="server" TargetControlID="txtFechaInicio_Fin" />
                                                                <act:CalendarExtender ID="imgFechaRevisionFin_CalendarExtender" runat="server" TargetControlID="txtFechaInicio_Fin" PopupButtonID="imgFechaRevisionFin" />
                                                                &nbsp;<asp:ImageButton ID="imgFechaRevisionFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="pnlFechaFin" runat="server" GroupingText="Fecha de Cierre" ToolTip="Fecha de Cierre" Width="98%" BackColor="#cccccc">
                                                                <asp:Label ID="Label4" runat="server" Text="Desde"></asp:Label>:
																<asp:TextBox ID="txtFechaCierre_Origen" runat="server" Width="85px" OnChange="SetCheckedRadio()"></asp:TextBox>
                                                                <act:CalendarExtender ID="ce_txtFechaCierre_Origen" runat="server" TargetControlID="txtFechaCierre_Origen" />
                                                                <act:CalendarExtender ID="ce_imgFechaFin_Origen" runat="server" TargetControlID="txtFechaCierre_Origen" PopupButtonID="imgFechaFin_Origen" />
                                                                &nbsp;<asp:ImageButton ID="imgFechaFin_Origen" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                                                                <br />
                                                                <asp:Label ID="Label6" runat="server" Text="Hasta"></asp:Label>:
																<asp:TextBox ID="txtFechaCierre_Fin" runat="server" Width="85px" OnChange="SetCheckedRadio()"></asp:TextBox>
                                                                <act:CalendarExtender ID="ce_txtFechaCierre_Fin" runat="server" TargetControlID="txtFechaCierre_Fin" />
                                                                <act:CalendarExtender ID="ce_imgFechaFin_Fin" runat="server" TargetControlID="txtFechaCierre_Fin" PopupButtonID="imgFechaFin_Fin" />
                                                                &nbsp;<asp:ImageButton ID="imgFechaFin_Fin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlAfectado" runat="server" GroupingText="Personas" ToolTip="Personas" Width="98%" BackColor="#cccccc">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtAfectado" runat="server" />
                                                                <act:TextBoxWatermarkExtender ID="wmAfectados" runat="server" TargetControlID="txtAfectado" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
                                                                <!-- Texto predictivo -->
                                                                <act:AutoCompleteExtender ID="ace_txtAfectado" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtAfectado" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                                                                    ServiceMethod="get_Usuarios" OnClientItemSelected="Set_Afectado"
                                                                    OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                                                            </td>
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
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr style="margin: 0px; padding: 0px; width: 100%;" />
                                    <center>
                                        <table style="width: auto; border-collapse: collapse;">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="Panel2" runat="server" CssClass="PanelBotones">
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
    <!--=================================================================================-->
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnBotones" runat="server" CssClass="PanelBotones">
                <asp:ImageButton ID="btnNuevaIncidencia" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo24.png" AlternateText="Nueva Incidencia" ToolTip="Nueva Incidencia" />
            </asp:Panel>
            <asp:GridView SkinID="GridView" ID="gvSucesos" runat="server" AutoGenerateColumns="False" AllowSorting="True" RowHeaderColumn="ID" DataKeyNames="ID" CssClass="GridViewASP" EmptyDataText="Sin Datos" Caption="Listado de Accidente/Incidente" GridLines="None" PagerSettings-Position="Bottom" AllowPaging="True">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ItemStyle-Width="1px" />
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" ItemStyle-Width="1px" />
                    <asp:BoundField DataField="FECHAAPERTURA" HeaderText="fecha" SortExpression="FECHAAPERTURA" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DESCRIPCIONPROBLEMA" HeaderText="descripcion" SortExpression="DESCRIPCIONPROBLEMA" />
                    <asp:TemplateField HeaderText="Hora Trabajo" SortExpression="HoraTrabajo">
                        <ItemStyle Wrap="false" Width="1px" VerticalAlign="Top" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# CDate(Eval("FECHAAPERTURA")).Second%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Creador" SortExpression="Creador">
                        <ItemStyle Wrap="false" Width="1px" VerticalAlign="Top" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblCreador" runat="server" Text="?"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Afectados" SortExpression="Afectados">
                        <ItemStyle Wrap="false" Width="1px" VerticalAlign="Top" />
                        <ItemTemplate>

                            <asp:BulletedList ID="blAfectados" runat="server" DisplayMode="Text" OnDataBound="blAfectados_DataBound" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
