<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Formulario.aspx.vb" Inherits="IstrikuWebRaiz.Formulario" %>

<%@ Register Assembly="eWorld.UI, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="~/Controles/SeleccionUsuarios.ascx" TagName="SeleccionUsuarios" TagPrefix="ControlUsuario" %>
<%@ Register Src="../Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad() { $find('<%=pce_pnlAfectado.ClientID%>').add_shown(function () { var obj = $get('<%= txtAfectado.ClientID%>'); obj.focus(); obj.select(); }); }
        /* Deteccion NC (Afectado) ****************************************************************************************/
        function Set_Afectado(source, eventArgs) {
            $find('<%=pce_pnlAfectado.ClientID%>').hidePopup();
            $("#lvAfectados_UL").append('<li><input name="hd_IdAfectados" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtAfectado.ClientID%>').value + ' <a href="#" onclick="Borrar_Afectado(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrarAfectado" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtAfectado.ClientID%>').value = "";
        }
        function Borrar_Afectado(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
        /*******************************************************************************************************************/
    </script>

    <link href="../Content/include/ui-1.10.0/ui-lightness/jquery-ui-1.10.0.custom.min.css" rel="stylesheet" />
    <link href="../Content/jquery.ui.timepicker.css?v=0.3.3" type="text/css" rel="stylesheet"/>

</asp:Content>

<asp:Content ID="Contenido_BODY" ContentPlaceHolderID="cp" runat="server">
    <table class="GridViewASP" style="width: 98%;">
        <caption>
            <asp:Label ID="lblCaptionGW" runat="server" Text="Detalle Suceso" ToolTip="Informe detallado del suceso"></asp:Label>
        </caption>
        <tr class="HeaderStyle">
            <th style="white-space: nowrap">
                <asp:Label ID="Label13" runat="server" Text="FechaApertura" ToolTip="FechaApertura" />
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label10" runat="server" Text="Finalización del Suceso" ToolTip="Finalización del Suceso" />
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label2" runat="server" Text="Hora del Suceso" ToolTip="Hora del Suceso" class="timepicker_button_trigger"/>
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label4" runat="server" Text="Hora de Trabajo" ToolTip="Hora de Trabajo"></asp:Label>
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label7" runat="server" Text="Tipo Suceso" ToolTip="Tipo Suceso"></asp:Label>
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label6" runat="server" Text="Creador" ToolTip="Creador"></asp:Label>
            </th>
<%--            <th style="white-space: nowrap">
                <asp:Label ID="Label1" runat="server" Text="Modificar Evaluacion" ToolTip="Modificar Evaluacion"></asp:Label>
            </th>--%>
<%--            <th style="white-space: nowrap">
                <asp:Label ID="Label3" runat="server" Text="Informe Final" ToolTip="Informe Final"></asp:Label>
            </th>--%>
            <th style="white-space: nowrap">
                <asp:Label ID="Label3" runat="server" Text="Riesgo" ToolTip="Riesgo"></asp:Label>
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label5" runat="server" Text="Nivel de riesgo" ToolTip="Nivel de riesgo"></asp:Label>
            </th>
        </tr>
        <tr class="RowStyle">
            <td style="white-space: nowrap; text-align: center;">
                <asp:TextBox ID="txtFecha" runat="server" Width="85px"></asp:TextBox>
                <act:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" TargetControlID="txtFecha" />
                <act:CalendarExtender ID="imgCalendario_CalendarExtender" runat="server" TargetControlID="txtFecha" PopupButtonID="imgCalendario" />
                &nbsp;<asp:ImageButton ID="imgCalendario" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
            </td>
            <td style="white-space: nowrap; text-align: center;">
                <asp:TextBox ID="txtFechaFin" runat="server" Width="85px"></asp:TextBox>
                <act:CalendarExtender ID="ce_txtFechaFin" runat="server" TargetControlID="txtFechaFin" />
                <act:CalendarExtender ID="ce_img_txtFechaFin" runat="server" TargetControlID="txtFechaFin" PopupButtonID="img_txtFechaFin" />
                &nbsp;<asp:ImageButton ID="img_txtFechaFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
            </td>
            <td style="text-align: center;">
                 <div id="content">
                    <div>
                        <asp:Image ID="imgHora" runat="server" class="timepicker_button_trigger" AlternateText="Hora" ToolTip="Hora" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/TimePicker/clock-icon_16x16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                        <asp:TextBox id="txtHoraSuceso" runat="server" style="width: 70px;" class="timepicker_button_trigger"/>
                        <script type="text/javascript">
                            $(document).ready(function () {
                                $('#<%=txtHoraSuceso.ClientID %>').timepicker({
                                    showAnim: 'blind',
                                    showOn: 'button',
                                    button: '.timepicker_button_trigger',
                                    hourText: '<%=ItzultzaileWeb.Itzuli("Hora")%>', 
                                    minuteText: '<%=ItzultzaileWeb.Itzuli("Minuto")%>',
                                });
                            });
                        </script>
                    </div>
                </div>                
            </td>
            <td style="text-align: center;">
                <asp:TextBox ID="txtHoraTrabajo" runat="server"></asp:TextBox>
                <act:NumericUpDownExtender ID="nude_txtHoraTrabajo" runat="server" Maximum="12" Minimum="0" Step="1" TargetControlID="txtHoraTrabajo" Width="90" />
            </td>
            <td style="white-space: nowrap; text-align: center;">
                <asp:RadioButtonList ID="rblLesiones" runat="server" ToolTip="Lesiones" RepeatDirection="Horizontal" />
            </td>
            <td style="white-space: nowrap; text-align: center;">
                <asp:Label ID="lblCreador" runat="server" Text="?"></asp:Label>
            </td>
<%--            <td>
                <asp:DropDownList ID="ddlInformeFinal" runat="server"></asp:DropDownList>
            </td>--%>
<%--            <td>
                <asp:DropDownList ID="ddlModificarEvaluacion" runat="server"></asp:DropDownList>
                <asp:TextBox ID="txtObservaciones_ModifEval" runat="server" TextMode="MultiLine" Rows="5" Width="100%" Columns="30" ></asp:TextBox>
            </td>--%>
            <td>
                <asp:DropDownList ID="ddlRiesgo" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlNivelderiesgo" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr align="center" class="HeaderStyle">
            <th style="white-space: nowrap" colspan="8">
                <asp:Label ID="Label14" runat="server" Text="Descripcion Suceso"></asp:Label>
            </th>
        </tr>
        <tr align="center" class="RowStyle">
            <td colspan="8">
                <asp:TextBox ID="txtDescripcion" runat="server" Rows="10" TextMode="MultiLine" MaxLength="2000" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr align="center" class="HeaderStyle">
            <th colspan="8">
                <table border="0" style="margin: 0 auto; border-collapse: collapse;">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlBucarAfectado" runat="server" CssClass="PanelBotones">
                                <asp:Image ID="imgBuscarAfectado" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="Afectados"></asp:Label>
                        </td>
                    </tr>
                </table>
            </th>
        </tr>
        <tr class="RowStyle">
            <td colspan="8">
                <asp:ListView ID="lvAfectados" runat="server" DataKeyNames="Id">
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
    <br />
    <div style="width: auto; text-align: center;">
        <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
            <asp:ImageButton ID="btnGuardar" runat="server" AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" />
        </asp:Panel>
    </div>

    <act:PopupControlExtender ID="pce_pnlAfectado" runat="server" TargetControlID="imgBuscarAfectado" PopupControlID="pnlAfectado" Position="Right">
    </act:PopupControlExtender>
    <asp:Panel ID="pnlAfectado" runat="server" Width="100%">
        <asp:TextBox ID="txtAfectado" runat="server" />
        <act:TextBoxWatermarkExtender ID="wmAfectados" runat="server" TargetControlID="txtAfectado" WatermarkText="textoabuscar" WatermarkCssClass="TextBoxWatermarkExtender" />
        <!-- Texto predictivo -->
        <act:AutoCompleteExtender ID="ace_txtAfectado" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb/ServiciosWeb.asmx" TargetControlID="txtAfectado" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
            ServiceMethod="get_Usuarios" OnClientItemSelected="Set_Afectado"
            OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
    </asp:Panel>

    <%--<PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />--%>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <asp:RequiredFieldValidator ID="rfvFecha" runat="server" ErrorMessage="requerido" ControlToValidate="txtFecha" Display="None" />
    <act:ValidatorCalloutExtender ID="vce_rfvFecha" runat="server" TargetControlID="rfvFecha" />
    <asp:CompareValidator ID="cvFecha" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFecha" Type="Date" Operator="DataTypeCheck" Display="None" />
    <act:ValidatorCalloutExtender ID="vce_cvFecha" runat="server" TargetControlID="cvFecha" />
    <asp:RequiredFieldValidator ID="rfv_txtHoraSuceso" runat="server" ErrorMessage="requerido" ControlToValidate="txtHoraSuceso" Display="None" />
    <act:ValidatorCalloutExtender ID="vce_rfv_txtHoraSuceso" runat="server" TargetControlID="rfv_txtHoraSuceso" />
    <asp:RegularExpressionValidator ID="rev_txtHoraSuceso" runat="server" ErrorMessage="formatoIncorrecto" ControlToValidate="txtHoraSuceso" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$" Display="None" />
    <act:ValidatorCalloutExtender ID="vce_rev_txtHoraSuceso" runat="server" TargetControlID="rev_txtHoraSuceso"/>    
    <asp:RangeValidator ID="rvHoraTrabajo" runat="server" ControlToValidate="txtHoraTrabajo" ErrorMessage="Fuera de rango" MinimumValue="0" MaximumValue="12" Type="Integer" Display="None" />
    <act:ValidatorCalloutExtender ID="vce_rvHoraTrabajo" runat="server" TargetControlID="rvHoraTrabajo" />
    <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ErrorMessage="requerido" ControlToValidate="txtDescripcion" Display="None" />
    <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvDescripcion" PopupPosition="TopLeft" />
    <asp:RequiredFieldValidator ID="rfvLesiones" runat="server" ErrorMessage="requerido" ControlToValidate="rblLesiones" Display="None" />
    <act:ValidatorCalloutExtender ID="vce_rfvLesiones" runat="server" TargetControlID="rfvLesiones" />
    <!------------------------------------------------------------------------------------------------------>
</asp:Content>
