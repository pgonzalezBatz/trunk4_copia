<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="E56.aspx.vb" Inherits="GTK_Troqueleria.E56" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () { $get("<%=pnlEtapa_5.ClientID%>").parentElement.style.height = "auto"; });
            $(document).ready(function () { $get("<%=pnlEtapa_6.ClientID%>").parentElement.style.height = "auto"; });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <asp:UpdatePanel ID="upE14" runat="server">
        <ContentTemplate>
            <ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº" />
            <table class="GridViewASP">
                <thead class="HeaderStyle">
                    <tr>
                        <th>
                            <asp:Label ID="Label7" runat="server" Text="Fecha Inicio"></asp:Label></th>
                        <th>
                            <asp:Label ID="Label8" runat="server" Text="Fecha Fin (Previsto)"></asp:Label></th>
                        <th>
                            <asp:Label ID="Label9" runat="server" Text="Fecha Solicitud de Aprobacion"></asp:Label></th>
                        <th>
                            <asp:Label ID="Label32" runat="server" Text="Fecha Aprobación"></asp:Label></th>
                    </tr>
                </thead>
                <tbody class="RowStyle">
                    <tr>
                        <td style="text-align: center;">
                            <%--<asp:TextBox ID="txtFechaInicio" runat="server" Width="0" Style="visibility: hidden;" BorderStyle="None" BorderWidth="0"></asp:TextBox>
							<asp:Label ID="lblFechaInicio" runat="server"></asp:Label>--%>
                            <asp:TextBox ID="txtFechaInicio" runat="server" Width="85px"></asp:TextBox>
                            <act:CalendarExtender ID="ce_txtFechaInicio" runat="server" TargetControlID="txtFechaInicio" />
                            <act:CalendarExtender ID="ce_imgFechaInicio" runat="server" TargetControlID="txtFechaInicio" PopupButtonID="imgFechaInicio" />
                            &nbsp;<asp:ImageButton ID="imgFechaInicio" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txtFechaFin" runat="server" Width="85px"></asp:TextBox>
                            <act:CalendarExtender ID="ce_txtFechaFin" runat="server" TargetControlID="txtFechaFin" />
                            <act:CalendarExtender ID="ce_imgFechaFin" runat="server" TargetControlID="txtFechaFin" PopupButtonID="imgFechaFin" />
                            &nbsp;<asp:ImageButton ID="imgFechaFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txtFechaCierre" runat="server" Width="85px"></asp:TextBox>
                            <act:CalendarExtender ID="ce_txtFechaCierre" runat="server" TargetControlID="txtFechaCierre" />
                            <act:CalendarExtender ID="ce_imgFechaCierre" runat="server" TargetControlID="txtFechaCierre" PopupButtonID="imgFechaCierre" />
                            &nbsp;<asp:ImageButton ID="imgFechaCierre" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txtFechaValidacion" runat="server" Width="85px"></asp:TextBox>
                            <act:CalendarExtender ID="ce_txtFechaValidacion" runat="server" TargetControlID="txtFechaValidacion" />
                            <act:CalendarExtender ID="ce_imgFechaVal" runat="server" TargetControlID="txtFechaValidacion" PopupButtonID="imgFechaVal" />
                            &nbsp;<asp:ImageButton ID="imgFechaVal" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" ImageAlign="AbsMiddle" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <act:CollapsiblePanelExtender ID="cpeEtapa_5" runat="server" TargetControlID="pnlEtapa_5" ExpandControlID="pnlCollapsed_E5" CollapseControlID="pnlCollapsed_E5" Collapsed="false" CollapsedSize="0" ScrollContents="false" ExpandDirection="Vertical" SuppressPostBack="true"
                ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                ImageControlID="imgEstadoPanel_E5" CollapsedText="Expandir" ExpandedText="Contraer" />
            <act:CollapsiblePanelExtender ID="cpeEtapa_6" runat="server" TargetControlID="pnlEtapa_6" ExpandControlID="pnlCollapsed_E6" CollapseControlID="pnlCollapsed_E6" Collapsed="false" CollapsedSize="0" ScrollContents="false" ExpandDirection="Vertical" SuppressPostBack="true"
                ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                ImageControlID="imgEstadoPanel_E6" CollapsedText="Expandir" ExpandedText="Contraer" />
            <table class="tablaBuscador" style="width: 100%;">
                <tr>
                    <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                        <asp:Panel runat="server" ID="pnlCollapsed_E5">
                            <table class="recuadro">
                                <tr class="ImageButton">
                                    <td>
                                        <asp:Image runat="server" ID="imgEstadoPanel_E5" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
											<asp:Label ID="lblEtapa_5" runat="server" Text="Etapa 5 - Analisis final"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEtapa_5" runat="server" CssClass="recuadro">
                            <asp:Label ID="Label24" runat="server" Text="Indicar las causas reales sobre el conjunto del proceso:"></asp:Label>
                            <asp:Label ID="Label4" runat="server" Text="(Aplicar herramienta de análisis de causa 5 porqués)"></asp:Label>
                            <ul>
                                <li>
                                    <asp:Label ID="Label14" runat="server" Text="Hombre, Materia, Máquina, Métodos"></asp:Label></li>
                                <li>
                                    <asp:Label ID="Label13" runat="server" Text="Quién, Donde, Cuando, Porqué, Como"></asp:Label></li>
                                <li>
                                    <asp:Label ID="Label12" runat="server" Text="Cambio de fabricacion, Proceso de retoques" /></li>
                                <li>
                                    <asp:Label ID="Label11" runat="server" Text="Mantenimiento"></asp:Label></li>
                            </ul>
                            <table style="width: 100%" border="0">
                                <tr>
                                    <td style="vertical-align: top; width: 50%;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="vertical-align: middle; width: 1%;">
                                                    <asp:Panel ID="pnlBotones_5PQ" runat="server" CssClass="PanelBotones">
                                                        <asp:ImageButton ID="imgNuevo5PQ" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" AlternateText="Nuevo Porque" ToolTip="Nuevo Porque" />
                                                    </asp:Panel>
                                                </td>
                                                <td style="vertical-align: middle; width: 99%;" class="recuadro">
                                                    <asp:Label ID="Label5" runat="server" Text="¿Porqué el Proceso de Fabricación ha podido fabricar o generar ese defecto?"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="vertical-align: middle; width: 1%;">
                                                    <asp:Panel ID="pnlBotones_5PQ_PC" runat="server" CssClass="PanelBotones">
                                                        <asp:ImageButton ID="imgNuevo5PQ_PC" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" AlternateText="Nuevo Porque" ToolTip="Nuevo Porque" />
                                                    </asp:Panel>
                                                </td>
                                                <td style="vertical-align: middle; width: 99%;" class="recuadro">
                                                    <asp:Label ID="Label6" runat="server" Text="¿Porqué el Plan de control no ha detectado ese defecto?"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <asp:GridView ID="gv5PQ_PF" SkinID="GridView" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="5 Porques (Proceso Fabricacion)" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" ShowSelectButton="false" ButtonType="Link" ItemStyle-Width="1%" />
                                                <asp:BoundField DataField="REALIZACION" HeaderText="Orden" SortExpression="REALIZACION" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="¿Por que?" SortExpression="DESCRIPCION" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="Label44" runat="server" Text="Sin Datos"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <asp:GridView ID="gv5PQ_PC" SkinID="GridView" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="5 Porques (Proceso Control)" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" ShowSelectButton="false" ButtonType="Link" ItemStyle-Width="1%" />
                                                <asp:BoundField DataField="REALIZACION" HeaderText="Orden" SortExpression="REALIZACION" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="¿Por que?" SortExpression="DESCRIPCION" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="Label44" runat="server" Text="Sin Datos"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>

                                <%--<tr class="HeaderStyle">
                                    <td style="width: 1%; vertical-align: top;">
                                        <asp:Panel ID="pnlBotones_CR_LF" runat="server" CssClass="PanelBotones">
                                            <asp:ImageButton ID="btnEditar_CR_PF" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
                                        </asp:Panel>
                                    </td>
                                    <th>
                                        <asp:Label ID="Label2" runat="server" Text="Análisis final (Causa raiz)" /></th>
                                    <td style="width: 1%; vertical-align: top;" rowspan="2">
                                        <asp:Panel ID="pnlBotones_CR_PC" runat="server" CssClass="PanelBotones">
                                            <asp:ImageButton ID="btnEditar_CR_PC" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
                                        </asp:Panel>
                                    </td>
                                    <th>
                                        <asp:Label ID="Label3" runat="server" Text="Causa Raiz: (Plan de Control)" /></th>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;" colspan="2">
                                        <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblCausaRaiz_PF" runat="server" /></pre>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblCausaRaiz_PC" runat="server" /></pre>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr class="HeaderStyle">
                                                <td style="width: 1%; vertical-align: top;">
                                                    <asp:Panel ID="pnlBotones_CR_LF" runat="server" CssClass="PanelBotones">
                                                        <asp:ImageButton ID="btnEditar_CR_PF" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
                                                    </asp:Panel>
                                                </td>
                                                <th>
                                                    <asp:Label ID="Label2" runat="server" Text="Análisis final (Causa raiz)" /></th>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top;" colspan="2">
                                                    <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblCausaRaiz_PF" runat="server" /></pre>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                        <asp:Panel runat="server" ID="pnlCollapsed_E6">
                            <table class="recuadro">
                                <tr class="ImageButton">
                                    <td>
                                        <asp:Image runat="server" ID="imgEstadoPanel_E6" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
											<asp:Label ID="lblEtapa_6" runat="server" Text="Etapa 6 - Plan de acciones definitivo"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEtapa_6" runat="server" CssClass="recuadro">
                            <table style="width: 100%">
                                <tr>
                                    <td style="vertical-align: top; width: 100%;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="vertical-align: middle; width: 1%;">
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="PanelBotones">
                                                        <asp:ImageButton ID="btnNuevaAccion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" AlternateText="Nueva Acción" ToolTip="Nueva Acción" />
                                                    </asp:Panel>
                                                </td>
                                                <td style="vertical-align: middle; width: 99%;" class="recuadro">
                                                    <asp:Label ID="Label1" runat="server" Text="Las acciones deben definirse para atacar la causa raíz (final) principalmente y evitar que dicho problema se repita"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvAcciones" SkinID="GridView" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="Acciones Definitivas" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" ShowSelectButton="false" ButtonType="Link" ItemStyle-Width="1%" />
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Accion" SortExpression="DESCRIPCION" />
                                                <asp:BoundField DataField="EFICACIA" HeaderText="Responsable (Area/Departamento)" SortExpression="EFICACIA" ItemStyle-Width="1%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="FECHAPREVISTA" HeaderText="Fecha Prevista" SortExpression="FECHAPREVISTA" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%" HeaderStyle-Wrap="false" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="Label44" runat="server" Text="Sin Datos"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <div style="text-align: center">
                <asp:Panel ID="pnlBotonesDetalle" runat="server" CssClass="PanelBotones">
                    <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" />
                </asp:Panel>
            </div>

            <act:ModalPopupExtender ID="mpe_pnlCausaRaiz_PF" runat="server" TargetControlID="btnEditar_CR_PF" PopupControlID="pnlCausaRaiz_PF" CancelControlID="imgCancelar_CausaRaiz_PF" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="pnlCausaRaiz_PF" runat="server" CssClass="modalBox" Style="display: none; width: 75%;">
                <table style="border-collapse: collapse; margin: 5px; width: 99%;">
                    <tr class="BarraTitulo">
                        <th style="text-align: left">
                            <asp:Label ID="Label35" runat="server" Text="Análisis final (Causa raiz)"></asp:Label>
                        </th>
                        <th style="text-align: right">
                            <asp:Panel ID="pnlBotonesCabecera" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="imgCancelar_CausaRaiz_PF" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                            </asp:Panel>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCausaRaiz_PF" runat="server" Rows="10" Width="99%" TextMode="MultiLine"></asp:TextBox>
                                        <act:AutoCompleteExtender ID="ace_txtCausaRaiz_PF" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_CausaRaizPF" TargetControlID="txtCausaRaiz_PF" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                                            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                                        <asp:RegularExpressionValidator ID="rev_txtCausaRaiz_PF" runat="server" ControlToValidate="txtCausaRaiz_PF" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="imgAceptar_CausaRaiz_PF" />
                                        <act:ValidatorCalloutExtender ID="vce_rev_txtCausaRaiz_PF" TargetControlID="rev_txtCausaRaiz_PF" runat="server" PopupPosition="BottomLeft" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="BarraTitulo">
                        <td style="text-align: center" colspan="2">
                            <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="imgAceptar_CausaRaiz_PF" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptar_CausaRaiz_PF" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <%--            <act:ModalPopupExtender ID="mpe_pnlCausaRaiz_PC" runat="server" TargetControlID="btnEditar_CR_PC" PopupControlID="pnlCausaRaiz_PC" CancelControlID="imgCancelar_CausaRaiz_PC" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="pnlCausaRaiz_PC" runat="server" CssClass="modalBox" Style="display: none; width: 75%;">
                <table style="border-collapse: collapse; margin: 5px; width: 99%;">
                    <tr class="BarraTitulo">
                        <th style="text-align: left">
                            <asp:Label ID="Label10" runat="server" Text="Causa Raiz: (Plan de Control)"></asp:Label>
                        </th>
                        <th style="text-align: right">
                            <asp:Panel ID="Panel3" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="imgCancelar_CausaRaiz_PC" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                            </asp:Panel>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCausaRaiz_PC" runat="server" Rows="10" Width="99%" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rev_txtCausaRaiz_PC" runat="server" ControlToValidate="txtCausaRaiz_PC" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="imgAceptar_CausaRaiz_PC" />
                                        <act:ValidatorCalloutExtender ID="vce_rev_txtCausaRaiz_PC" TargetControlID="rev_txtCausaRaiz_PC" runat="server" PopupPosition="BottomLeft" />
                                        <act:AutoCompleteExtender ID="ace_txtCausaRaiz_PC" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_CausaRaizPC" TargetControlID="txtCausaRaiz_PC" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
                                            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="BarraTitulo">
                        <td style="text-align: center" colspan="2">
                            <asp:Panel ID="Panel4" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="imgAceptar_CausaRaiz_PC" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptar_CausaRaiz_PC" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion solo fecha ----------------------------------------------------------------------------->
    <asp:CompareValidator ID="cv_txtFechaInicio" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaInicio" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_cv_txtFechaInicio" runat="server" TargetControlID="cv_txtFechaInicio" />
    <asp:CompareValidator ID="cv_txtFechaFin" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_cv_txtFechaFin" runat="server" TargetControlID="cv_txtFechaFin" />
    <asp:CompareValidator ID="cv_txtFechaCierre" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_cv_txtFechaCierre" runat="server" TargetControlID="cv_txtFechaCierre" />
    <!------------------------------------------------------------------------------------------------------>

    <!-- txtFechaInicio <= txtFechaFin / txtFechaCierre ---------------------------------------------------->
    <asp:CompareValidator ID="cv_txtFechaInicio2" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Operator="GreaterThanEqual" ControlToCompare="txtFechaInicio" Display="None" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_cv_txtFechaInicio2" runat="server" TargetControlID="cv_txtFechaInicio2" />

    <asp:CompareValidator ID="cv_txtFechaInicio3" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Operator="GreaterThanEqual" ControlToCompare="txtFechaInicio" Display="None" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_cv_txtFechaInicio3" runat="server" TargetControlID="cv_txtFechaInicio3" />
    <!------------------------------------------------------------------------------------------------------>
</asp:Content>
