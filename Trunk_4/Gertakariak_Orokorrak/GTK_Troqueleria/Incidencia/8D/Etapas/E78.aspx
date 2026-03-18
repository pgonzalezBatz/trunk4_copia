<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="E78.aspx.vb" Inherits="GTK_Troqueleria.E78" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () { $get("<%=pnlEtapa_7.ClientID%>").parentElement.style.height = "auto"; });
		    $(document).ready(function () { $get("<%=pnlEtapa_8.ClientID%>").parentElement.style.height = "auto"; });
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

            <act:CollapsiblePanelExtender ID="cpeEtapa_7" runat="server" TargetControlID="pnlEtapa_7" ExpandControlID="pnlCollapsed_E7" CollapseControlID="pnlCollapsed_E7" Collapsed="false" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                ImageControlID="imgEstadoPanel_E7" CollapsedText="Expandir" ExpandedText="Contraer" />
            <act:CollapsiblePanelExtender ID="cpeEtapa_8" runat="server" TargetControlID="pnlEtapa_8" ExpandControlID="pnlCollapsed_E8" CollapseControlID="pnlCollapsed_E8" Collapsed="false" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                ImageControlID="imgEstadoPanel_E8" CollapsedText="Expandir" ExpandedText="Contraer" />
            <table class="tablaBuscador" border="0">
                <tr>
                    <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                        <asp:Panel runat="server" ID="pnlCollapsed_E7">
                            <table class="recuadro">
                                <tr class="ImageButton">
                                    <td>
                                        <asp:Image runat="server" ID="imgEstadoPanel_E7" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
											<asp:Label ID="lblEtapa_7" runat="server" Text="Etapa 7 - Confirmacion de los planes de accion definitivos"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEtapa_7" runat="server" CssClass="recuadro">
                            <table class="GridViewASP">
                                <tr class="HeaderStyle">
                                    <th rowspan="2" style="white-space: nowrap;">
                                        <asp:Label ID="Label79" runat="server" Text="¿Las acciones emprendidas han sido confirmadas como eficaces?"></asp:Label></th>
                                    <th>
                                        <asp:Label ID="Label80" runat="server" Text="Si"></asp:Label></th>
                                    <th>
                                        <asp:Label ID="Label81" runat="server" Text="No"></asp:Label></th>
                                </tr>
                                <tr class="RowStyle">
                                    <td>
                                        <asp:CheckBox ID="cb_E7_ACCIONES_S" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="cb_E7_ACCIONES_N" runat="server" /></td>
                                </tr>
                                <tr class="HeaderStyle">
                                    <th colspan="3">
                                        <asp:Label ID="Label82" runat="server" Text="¿Como?"></asp:Label></th>
                                </tr>
                                <tr class="RowStyle">
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_E7_ACCIONES_DESC" runat="server" TextMode="MultiLine" Width="100%" Rows="4"></asp:TextBox></td>
                                </tr>
                                <tr class="FooterStyle">
                                    <td colspan="3">
                                        <asp:Label ID="Label83" runat="server" Text="Adjuntar a este documento las pruebas, como indicadores de control, informes dimensional, fotos…."></asp:Label></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                        <asp:Panel runat="server" ID="pnlCollapsed_E8">
                            <table class="recuadro">
                                <tr class="ImageButton">
                                    <td>
                                        <asp:Image runat="server" ID="imgEstadoPanel_E8" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
											<asp:Label ID="lblEtapa_8" runat="server" Text="Etapa 8 - Acciones de seguimiento y Capitalizacion"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEtapa_8" runat="server" CssClass="recuadro">
                            <table class="GridViewASP">
                                <caption>
                                    <asp:Label ID="Label66" runat="server" Text="Tras la implementación de las acciones, ¿los siguientes temas necesitan una actualización?"></asp:Label></caption>
                                <tr class="HeaderStyle">
                                    <th style="width: 1%;"></th>
                                    <th>
                                        <asp:Label ID="Label67" runat="server" Text="Si"></asp:Label></th>
                                    <th>
                                        <asp:Label ID="Label1" runat="server" Text="No"></asp:Label></th>
                                    <th>
                                        <asp:Label ID="Label68" runat="server" Text="Resp."></asp:Label></th>
                                    <th>
                                        <asp:Label ID="Label69" runat="server" Text="Plazos"></asp:Label></th>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label70" runat="server" Text="Instrucciones/procedimientos de trabajo"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES1" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES1_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES1_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES1_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label71" runat="server" Text="Normas Técnicas/especificaciones"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES2" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES2_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES2_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES2_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label72" runat="server" Text="Procesos (flujogramas…)"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES3" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES3_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES3_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES3_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label73" runat="server" Text="Check list PM"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES4" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES4_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES4_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES4_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label74" runat="server" Text="Check list Diseño"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES5" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES5_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES5_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES5_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label75" runat="server" Text="Check list modelos"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES6" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES6_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES6_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES6_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label76" runat="server" Text="Check list Homologación TC"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES7" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES7_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES7_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES7_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label77" runat="server" Text="Check list Homologación final"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES8" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES8_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES8_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES8_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="RowStyle">
                                    <th style="text-align: left; white-space: nowrap;">
                                        <asp:Label ID="Label78" runat="server" Text="Otros"></asp:Label></th>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES9" runat="server" /></td>
                                    <td style="text-align: center;">
                                        <asp:CheckBox ID="cb_E8_ACCIONES9_N" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES9_RESP" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_E8_ACCIONES9_PLAZO" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <div style="text-align: center">
                <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                    <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" />
                </asp:Panel>
            </div>

            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E7_ACCIONES_S" runat="server" TargetControlID="cb_E7_ACCIONES_S" Key="E7_ACCIONES" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E7_ACCIONES_N" runat="server" TargetControlID="cb_E7_ACCIONES_N" Key="E7_ACCIONES" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES1" runat="server" TargetControlID="cb_E8_ACCIONES1" Key="E8_ACCIONES1" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES1_N" runat="server" TargetControlID="cb_E8_ACCIONES1_N" Key="E8_ACCIONES1" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES2" runat="server" TargetControlID="cb_E8_ACCIONES2" Key="E8_ACCIONES2" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES2_N" runat="server" TargetControlID="cb_E8_ACCIONES2_N" Key="E8_ACCIONES2" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES3" runat="server" TargetControlID="cb_E8_ACCIONES3" Key="E8_ACCIONES3" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES3_N" runat="server" TargetControlID="cb_E8_ACCIONES3_N" Key="E8_ACCIONES3" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES4" runat="server" TargetControlID="cb_E8_ACCIONES4" Key="E8_ACCIONES4" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES4_N" runat="server" TargetControlID="cb_E8_ACCIONES4_N" Key="E8_ACCIONES4" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES5" runat="server" TargetControlID="cb_E8_ACCIONES5" Key="E8_ACCIONES5" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES5_N" runat="server" TargetControlID="cb_E8_ACCIONES5_N" Key="E8_ACCIONES5" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES6" runat="server" TargetControlID="cb_E8_ACCIONES6" Key="E8_ACCIONES6" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES6_N" runat="server" TargetControlID="cb_E8_ACCIONES6_N" Key="E8_ACCIONES6" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES7" runat="server" TargetControlID="cb_E8_ACCIONES7" Key="E8_ACCIONES7" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES7_N" runat="server" TargetControlID="cb_E8_ACCIONES7_N" Key="E8_ACCIONES7" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES8" runat="server" TargetControlID="cb_E8_ACCIONES8" Key="E8_ACCIONES8" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES8_N" runat="server" TargetControlID="cb_E8_ACCIONES8_N" Key="E8_ACCIONES8" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES9" runat="server" TargetControlID="cb_E8_ACCIONES9" Key="E8_ACCIONES9" />
            <act:MutuallyExclusiveCheckBoxExtender ID="mecbe_cb_E8_ACCIONES9_N" runat="server" TargetControlID="cb_E8_ACCIONES9_N" Key="E8_ACCIONES9" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlMensaje_LA" runat="server" CssClass="modalBox" Style="display: none;">
        <table style="border-collapse: collapse; margin: 5px;">
            <tr class="BarraTitulo">
                <th style="text-align: left">
                    <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/LeccionesAprendidas16.png" AlternateText="Leccion Aprendida" ToolTip="Leccion Aprendida" ImageAlign="Middle" />
                    <asp:Label ID="Label102" runat="server" Text="Leccion Aprendida"></asp:Label>
                </th>
                <th style="text-align: right">

                    <asp:Panel ID="Panel4" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgCancelar_LC" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                    </asp:Panel>
                </th>
            </tr>
            <tr>
                <td colspan="2" class="MensajeError">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblMensaje" runat="server" Text="¿Desea crear una 'Leccion Aprendida'?" />
                                <br />
                                <asp:Image ID="Image6" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/LeccionesAprendidas48.png" AlternateText="Leccion Aprendida" ToolTip="Leccion Aprendida" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<tr class="BarraTitulo">
                <td style="text-align: center" colspan="2">
                    <asp:Panel ID="Panel9" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="btnAceptar_LA" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" />
                    </asp:Panel>
                </td>
            </tr>--%>
        </table>
    </asp:Panel>
    <%--<asp:HiddenField ID="hf_pnlMensaje_LA" runat="server" />
    <act:ModalPopupExtender ID="mpe_pnlMensaje_LA" runat="server" TargetControlID="hf_pnlMensaje_LA" PopupControlID="pnlMensaje_LA" CancelControlID="imgCancelar_LC" BackgroundCssClass="modalBackground" />--%>


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
    <!-- Validacion MaxLength ------------------------------------------------------------------------------>
    <asp:RegularExpressionValidator ID="rev_txt_E7_ACCIONES_DESC" runat="server" ControlToValidate="txt_E7_ACCIONES_DESC" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_rev_txt_E7_ACCIONES_DESC" TargetControlID="rev_txt_E7_ACCIONES_DESC" runat="server" PopupPosition="TopLeft" />
    <!------------------------------------------------------------------------------------------------------>


</asp:Content>
