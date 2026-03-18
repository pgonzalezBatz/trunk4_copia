<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Detalle.aspx.vb" Inherits="IstrikuWebRaiz.Resumen" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">

    <asp:Panel ID="pnlBotonesGv" runat="server" CssClass="PanelBotones" Visible="false">
		<asp:ImageButton ID="btnParteAccidente" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/application-edit-icon16.png" AlternateText="Parte de Accidente" ToolTip="Parte de Accidente" />
	</asp:Panel>

    <table class="GridViewASP">
        <caption>
            <asp:Label ID="lblCaptionGW" runat="server" Text="Detalle Suceso" ToolTip="Informe detallado del suceso"></asp:Label></caption>
        <tr class="HeaderStyle">
            <td rowspan="6" valign="top" style="width: 1px">&nbsp;<asp:ImageButton ID="imgEditar" runat="server" AlternateText="Editar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" PostBackUrl="~/Informe/Formulario.aspx" ToolTip="Editar" />
            </td>
            <th style="white-space: nowrap">
                <asp:Label ID="Label13" runat="server" Text="FechaApertura" ToolTip="FechaApertura" />
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label10" runat="server" Text="Finalización del Suceso" ToolTip="Finalización del Suceso" />
            </th>
            <th style="white-space: nowrap" class="GridViewASP">
                <asp:Label ID="Label2" runat="server" Text="Hora del Suceso" ToolTip="Hora del Suceso" />
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
                <asp:Label ID="Label19" runat="server" Text="Modificar Evaluacion" ToolTip="Modificar Evaluacion"></asp:Label>
            </th>--%>
<%--            <th style="white-space: nowrap">
                <asp:Label ID="Label20" runat="server" Text="Informe Final" ToolTip="Informe Final"></asp:Label>
            </th>--%>
            <th style="white-space: nowrap">
                <asp:Label ID="Label20" runat="server" Text="Riesgo" ToolTip="Riesgo"></asp:Label>
            </th>
            <th style="white-space: nowrap">
                <asp:Label ID="Label21" runat="server" Text="Nivel de riesgo" ToolTip="Nivel de riesgo"></asp:Label>
            </th>
        </tr>
        <tr align="center" class="RowStyle">
            <td style="white-space: nowrap">
                <asp:Label runat="server" ID="lblFechaApertura"></asp:Label>
            </td>
            <td style="white-space: nowrap">
                <asp:Label runat="server" ID="lblFechaCierre"></asp:Label>
            </td>
            <td style="white-space: nowrap">
                <asp:Label ID="lblHoraSuceso" runat="server"></asp:Label>
            </td>
            <td style="white-space: nowrap">
                <asp:Label runat="server" ID="lblHoraTrabajo"></asp:Label>
            </td>
            <td style="white-space: nowrap">
                <asp:Label ID="lblTipoSuceso" runat="server"></asp:Label>
                &nbsp;
            </td>
            <td style="white-space: nowrap">
                <asp:Label ID="lblCreador" runat="server" Text="?"></asp:Label>
            </td>
<%--            <td>
                <asp:BulletedList ID="blInformeFinal" runat="server" DisplayMode="Text" ViewStateMode="Disabled" ToolTip="Deteccion" />
            </td>--%>
<%--            <td>
                <asp:BulletedList ID="blModificarEvaluacion" runat="server" DisplayMode="Text" ViewStateMode="Disabled" ToolTip="Deteccion" />
                <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblObservaciones_ModifEval" runat="server"></asp:Label></pre>
            </td>--%>
            <td>
                <asp:BulletedList ID="blRiesgo" runat="server" DisplayMode="Text" ViewStateMode="Disabled" ToolTip="Riesgo" />
            </td>
            <td>
                <asp:BulletedList ID="blNivelderiesgo" runat="server" DisplayMode="Text" ViewStateMode="Disabled" ToolTip="Nivel de riesgo" />
            </td>
        </tr>
        <tr align="center" class="HeaderStyle">
            <th style="white-space: nowrap" colspan="8">
                <asp:Label ID="Label14" runat="server" Text="Descripcion Suceso"></asp:Label>
            </th>
        </tr>
        <tr align="center" class="RowStyle">
            <td style="white-space: nowrap" colspan="8">
                <pre style="white-space: pre-wrap; font: inherit; text-align: left;"><asp:Label ID="lblDescripcion" runat="server"></asp:Label></pre>
            </td>
        </tr>
        <tr align="center" class="HeaderStyle">
            <th style="white-space: nowrap" colspan="8">
                <asp:Label ID="Label15" runat="server" Text="Afectados"></asp:Label>
            </th>
        </tr>
        <tr align="center" class="RowStyle">
            <td style="white-space: nowrap" colspan="8">
                <table style="border: 0px; margin: 0px; padding: 0px;">
                    <tr>
                        <td align="left">
                            <asp:ListView ID="lvAfectados" runat="server" EnableModelValidation="True" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <table style="white-space: nowrap; border-width: 0px; padding: 0px; margin: 0px; border-spacing: 0px; empty-cells: show">
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <li>
                                                <asp:Image ID="imgEstado" runat="server" ImageAlign="AbsMiddle" />&nbsp;
                                                <asp:Label ID="lblNobreCompleto" runat="server" Text='<%# Eval("NombreCompleto") %>'></asp:Label>
                                            </li>
                                        </td>
                                        <td>
                                            <asp:Image ID="btnAfectadoInfo" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon16.png" ImageAlign="AbsMiddle" onmouseover="this.style.cursor='pointer'" />
                                            <act:HoverMenuExtender ID="hme_btnAfectadoInfo" runat="server" TargetControlID="btnAfectadoInfo"
                                                PopupControlID="pnlAfectado"
                                                PopupPosition="Left "
                                                OffsetX="0"
                                                OffsetY="0"
                                                PopDelay="50">
                                            </act:HoverMenuExtender>
                                            <asp:Panel ID="pnlAfectado" runat="server" CssClass="CompletionListCssClass">
                                                <table class="GridViewASP">
                                                    <tr class="HeaderStyle">
                                                        <th colspan="2">
                                                            <asp:Label ID="Label8" runat="server" Text="Trabajador"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr class="RowStyle">
                                                        <td colspan="2">
                                                            <asp:Label ID="lblNombreTrabajador" runat="server" Text='<%# Eval("NombreCompleto") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th>
                                                            <asp:Label ID="Label9" runat="server" Text="Convenio"></asp:Label></th>
                                                        <th>
                                                            <asp:Label ID="Label11" runat="server" Text="Categoria"></asp:Label></th>
                                                    </tr>
                                                    <tr class="RowStyle">
                                                        <td>
                                                            <asp:Label ID="lblConvenio" runat="server" Text="?"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblCategoria" runat="server" Text="?"></asp:Label></td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th><asp:Label ID="Label12" runat="server" Text="Nº.Trabajador"></asp:Label></th>
                                                        <td class="RowStyle"><asp:Label ID="lblNTrabajador" runat="server" Text="?"></asp:Label></td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th><asp:Label ID="Label16" runat="server" Text="Fecha Nacimiento"></asp:Label></th>
                                                        <td class="RowStyle"><asp:Label ID="lblFechaNacimiento" runat="server" Text="?"></asp:Label></td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th><asp:Label ID="Label17" runat="server" Text="Antiguedad"></asp:Label></th>
                                                        <td class="RowStyle"><asp:Label ID="lblAntiguedad" runat="server" Text="?"></asp:Label></td>
                                                    </tr>
													<tr class="HeaderStyle">
                                                        <th><asp:Label ID="Label18" runat="server" Text="Maquina"></asp:Label></th>
                                                        <td class="RowStyle"><asp:Label ID="lblMaquina" runat="server" Text="?" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="imgEditar" runat="server" CommandName="Select">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" />
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Table ID="Leyenda" runat="server" CssClass="Leyenda recuadro">
                                <asp:TableRow>
                                    <asp:TableCell ID="tcAfectado" Visible="false">
                                        <img src="../App_Themes/Tema1/Imagenes/EstadoParte/Aceptado-icon.png" alt="Aceptado" style="vertical-align: middle" />
                                        -
										<asp:Label ID="Label1" runat="server" Text="Aceptado" />
                                    </asp:TableCell><asp:TableCell ID="tcDenegado" Visible="false">
                                        <img src="../App_Themes/Tema1/Imagenes/EstadoParte/Denegado-icon.png" alt="Denegado" style="vertical-align: middle" />
                                        -
										<asp:Label ID="Label3" runat="server" Text="Denegado" />
                                    </asp:TableCell><asp:TableCell ID="tcPendiente" Visible="false">
                                        <img src="../App_Themes/Tema1/Imagenes/EstadoParte/Pendiente-icon.png" alt="Pendiente de validar" style="vertical-align: middle" />
                                        -
										<asp:Label ID="Label5" runat="server" Text="Pendiente de validar" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <fieldset style="text-align: center;">
        <asp:ImageButton ID="imgEliminar" runat="server" AlternateText="Eliminar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Eliminar24.png" ToolTip="Eliminar" />
    </fieldset>
    <br />
    <fieldset style="text-align: center;">
        <asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Volver a la página anterior" PostBackUrl="~/Default.aspx" />
    </fieldset>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <act:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="imgEliminar" ConfirmText="Desea eliminar" />
    <!------------------------------------------------------------------------------------------------------>
</asp:Content>
