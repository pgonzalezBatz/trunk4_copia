<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Detalle.aspx.vb" Inherits="GertakariakMSWeb_Raiz.Detalle" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHEAD" runat="server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () { $get("<%=pnlContenedora.ClientID%>").parentElement.style.height = "auto"; });
		    $(document).ready(function () { $get("<%=pnlProvisional.ClientID%>").parentElement.style.height = "auto"; });
		    $(document).ready(function () { $get("<%=pnlDefinitiva.ClientID%>").parentElement.style.height = "auto"; });
		    $(document).ready(function () { $get("<%=pnlPreventiva.ClientID%>").parentElement.style.height = "auto"; });
		}
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ascx:Titulo ID="TituloPagina" runat="server" />
            <act:TabContainer ID="tcIncidencia" runat="server" ActiveTabIndex="0" Width="100%">
                <act:TabPanel ID="tpDatosGenerales" runat="server" HeaderText="datosgenerales">
                    <ContentTemplate>
                        <asp:Panel ID="pnBotones_DatosGenerales" runat="server">
                            <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" ToolTip="Editar" AlternateText="Editar" />
                            <asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Eliminar24.png" />
                            <act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" />
                            <asp:ImageButton ID="btnNuevaIncidencia" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" ToolTip="Nuevo" AlternateText="Nuevo" />
                        </asp:Panel>
                        <hr />
                        <table style="width: 1px" class="GridViewASP">
                            <caption>
                                <asp:Label ID="Label31" runat="server" Text="fechas" />
                            </caption>
                            <tr class="HeaderStyle">
                                <th style="white-space: nowrap">
                                    <asp:Label ID="Label1" runat="server" Text="FechaApertura" />
                                </th>
                                <th style="white-space: nowrap" class="GridViewASP">
                                    <asp:Label ID="Label2" runat="server" Text="Fecha de Solución" />
                                </th>
                                <th style="white-space: nowrap">
                                    <asp:Label ID="Label3" runat="server" Text="Demora" />
                                    (<asp:Label ID="Label4" runat="server" Text="semanas" />)
                                </th>
                            </tr>
                            <tr align="center" class="RowStyle">
                                <td style="white-space: nowrap">
                                    <asp:Label ID="lblFechaApertura" runat="server" />
                                </td>
                                <td style="white-space: nowrap">
                                    <asp:Label ID="lblFechaCierre" runat="server" />
                                </td>
                                <td style="white-space: nowrap">
                                    <asp:Label ID="lblDemora" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table class="GridViewASP">
                            <caption>
                                <asp:Label ID="Label29" runat="server" Text="incidencia"></asp:Label>
                            </caption>
                            <tr class="HeaderStyle">
                                <th style="white-space: nowrap">
                                    <asp:Label ID="Label6" runat="server" Text="familia" />
                                </th>
                                <td class="RowStyle" style="white-space: nowrap">
                                    <asp:Label ID="lblFamilia" runat="server" />
                                </td>
                                <th style="white-space: nowrap">
                                    <asp:Label ID="Label8" runat="server" Text="Instalación" ToolTip="Linea" />
                                </th>
                                <td class="RowStyle" style="white-space: nowrap">
                                    <asp:Label ID="lblInstalacion" runat="server" />
                                </td>
                                <th style="white-space: nowrap">
                                    <asp:Label ID="Label7" runat="server" Text="tipoIncidencia"></asp:Label>
                                </th>
                                <td class="RowStyle" style="white-space: nowrap">
                                    <asp:Label ID="lblTipoIncidencia" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="HeaderStyle">
                                <th style="white-space: nowrap" rowspan="2">
                                    <asp:Label ID="Label5" runat="server" Text="Descripcion problema"></asp:Label>
                                </th>
                                <td class="RowStyle" colspan="3" rowspan="2">
                                    <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblDescripcionProblema" runat="server" /></pre>
                                </td>
                                <th colspan="2" style="white-space: nowrap">
                                    <asp:Label ID="Label28" runat="server" Text="responsables"></asp:Label>
                                </th>
                            </tr>
                            <tr class="HeaderStyle">
                                <td class="RowStyle" style="white-space: nowrap" colspan="2">
                                    <asp:BulletedList ID="blResponsables" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tpAcciones" runat="server" HeaderText="acciones">
                    <ContentTemplate>
                        <asp:Panel ID="pnBotones_Acciones" runat="server">
                            <asp:ImageButton ID="btnNuevaAccion" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" ToolTip="Nuevo" AlternateText="Nuevo" />
                        </asp:Panel>
                        <hr />
                        <act:CollapsiblePanelExtender ID="cpeContenedora" runat="server" TargetControlID="pnlContenedora" ExpandControlID="pnlCollapsed_Contenedora" CollapseControlID="pnlCollapsed_Contenedora" Collapsed="true" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                            ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                            CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                            ImageControlID="imgEstadoPanel_Contenedora" CollapsedText="Expandir" ExpandedText="Contraer" />
                        <table class="tablaBuscador" style="width: 100%;">
                            <tr>
                                <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                                    <asp:Panel runat="server" ID="pnlCollapsed_Contenedora">
                                        <table class="recuadro">
                                            <tr class="ImageButton">
                                                <td>
                                                    <asp:Image runat="server" ID="imgEstadoPanel_Contenedora" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
													<asp:Label ID="lblEtapa_2" runat="server" Text="Contenedoras" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlContenedora" runat="server" CssClass="recuadro">
                                        <asp:ListView ID="lvAcciones_Contenedoras" runat="server" EnableModelValidation="true" DataKeyNames="Id" Enabled="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="Label11" runat="server" Text="Sin Datos" />
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <asp:Panel ID="itemPlaceholderContainer" runat="server">
                                                    <table runat="server" id="itemPlaceholder" />
                                                </asp:Panel>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <table id="lvAccion" runat="server" class="GridViewASP">
                                                    <tr class="HeaderStyle">
                                                        <td rowspan="5" style="vertical-align: top; width: 1px">
                                                            <asp:LinkButton ID="btnEditarAccion" runat="server" CommandName="Edit" ToolTip="Editar">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" />
                                                            </asp:LinkButton>
                                                        </td>
                                                        <th>
                                                            <%--<asp:Label ID="Label13" runat="server" Text="Tipo de Acción"></asp:Label>--%>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label14" runat="server" Text="FechaInicio"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label15" runat="server" Text="Fecha Prevista de Cierre"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label16" runat="server" Text="FechaFin"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label9" runat="server" Text="Demora" />
                                                            (<asp:Label ID="Label10" runat="server" Text="semanas" />)
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label12" runat="server" Text="Desarrollo"></asp:Label>(%)
                                                        </th>

                                                    </tr>
                                                    <tr class="RowStyle" style="text-align: center">
                                                        <td>
                                                            <%--<asp:Label ID="lblTipoAccion" runat="server" Text='<%# IF(Eval("IdTipoAccion")=0, Nothing ,Eval("IdTipoAccion"))%>' />--%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label18" runat="server" Text='<%#  Eval("FechaInicio", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label19" runat="server" Text='<%#  Eval("FechaPrevista", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label20" runat="server" Text='<%#  Eval("FechaFin", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDemora" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label21" runat="server" Text='<%#  Eval("Realizacion") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th>
                                                            <asp:Label ID="Label26" runat="server" Text="descripcion"></asp:Label>
                                                        </th>
                                                        <td colspan="5" class="RowStyle">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label27" runat="server" Text='<%#  Eval("Descripcion") %>'></asp:Label></pre>
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th rowspan="2">
                                                            <asp:Label ID="Label22" runat="server" Text="eficacia"></asp:Label>
                                                        </th>
                                                        <td class="RowStyle" colspan="4" rowspan="2">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label23" runat="server" Text='<%#  Eval("Eficacia") %>'></asp:Label></pre>
                                                        </td>
                                                        <th style="white-space: nowrap">
                                                            <asp:Label ID="Label25" runat="server" Text="responsables"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <td class="RowStyle">
                                                            <asp:BulletedList ID="blResponsablesAcciones" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td colspan="6">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvObservaciones" runat="server" AutoGenerateColumns="False" ShowFooter="true" AllowSorting="True" RowHeaderColumn="ID" DataKeyNames="ID" CssClass="GridViewASP" Caption="observaciones">
                                                                        <RowStyle CssClass="RowStyle" />
                                                                        <FooterStyle CssClass="FooterStyle" />
                                                                        <PagerStyle CssClass="PagerStyle" />
                                                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                                        <HeaderStyle CssClass="HeaderStyle" />
                                                                        <EditRowStyle CssClass="EditRowStyle" />
                                                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                                                        <EmptyDataRowStyle CssClass="FooterStyle" />
                                                                        <EmptyDataTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text="Sin Datos" />
                                                                        </EmptyDataTemplate>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdAccion" HeaderText="Fecha" ItemStyle-Width="1px" Visible="false" />
                                                                            <asp:BoundField DataField="Fecha" HeaderText="fecha" ItemStyle-Width="1px" DataFormatString="{0:d}" />
                                                                            <asp:TemplateField ItemStyle-Wrap="false" AccessibleHeaderText="Usuario" HeaderText="Usuario">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Usuario.NombreCompleto")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="observaciones" ItemStyle-Width="100%" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <hr />
                                                            <fieldset style="text-align: center">
                                                                <asp:LinkButton ID="btnNuevaObservacion" runat="server" CommandName="NuevaObservacion" ToolTip="Nuevo">
                                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" />
                                                                </asp:LinkButton>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemSeparatorTemplate>
                                                <hr />
                                            </ItemSeparatorTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>

                        <act:CollapsiblePanelExtender ID="cpeProvisional" runat="server" TargetControlID="pnlProvisional" ExpandControlID="pnlCollapsed_Provisional" CollapseControlID="pnlCollapsed_Provisional" Collapsed="true" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                            ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                            CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                            ImageControlID="imgEstadoPanel_Provisional" CollapsedText="Expandir" ExpandedText="Contraer" />
                        <table class="tablaBuscador" style="width: 100%;">
                            <tr>
                                <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                                    <asp:Panel runat="server" ID="pnlCollapsed_Provisional">
                                        <table class="recuadro">
                                            <tr class="ImageButton">
                                                <td>
                                                    <asp:Image runat="server" ID="imgEstadoPanel_Provisional" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
													<asp:Label ID="Label24" runat="server" Text="Provisionales" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlProvisional" runat="server" CssClass="recuadro">
                                        <asp:ListView ID="lvAcciones_Provisional" runat="server" EnableModelValidation="true" DataKeyNames="Id" Enabled="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="Label11" runat="server" Text="Sin Datos" />
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <asp:Panel ID="itemPlaceholderContainer" runat="server">
                                                    <table runat="server" id="itemPlaceholder" />
                                                </asp:Panel>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <table id="lvAccion" runat="server" class="GridViewASP">
                                                    <tr class="HeaderStyle">
                                                        <td rowspan="5" style="vertical-align: top; width: 1px">
                                                            <asp:LinkButton ID="btnEditarAccion" runat="server" CommandName="Edit" ToolTip="Editar">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" />
                                                            </asp:LinkButton>
                                                        </td>
                                                        <th>
                                                            <%--<asp:Label ID="Label13" runat="server" Text="Tipo de Acción"></asp:Label>--%>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label14" runat="server" Text="FechaInicio"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label15" runat="server" Text="Fecha Prevista de Cierre"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label16" runat="server" Text="FechaFin"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label9" runat="server" Text="Demora" />
                                                            (<asp:Label ID="Label10" runat="server" Text="semanas" />)
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label12" runat="server" Text="Desarrollo"></asp:Label>(%)
                                                        </th>
                                                    </tr>
                                                    <tr class="RowStyle" style="text-align: center">
                                                        <td>
                                                            <%--<asp:Label ID="Label17" runat="server" Text='<%# IF(Eval("IdTipoAccion")=0, Nothing ,Eval("IdTipoAccion"))%>' />--%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label18" runat="server" Text='<%#  Eval("FechaInicio", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label19" runat="server" Text='<%#  Eval("FechaPrevista", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label20" runat="server" Text='<%#  Eval("FechaFin", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDemora" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label21" runat="server" Text='<%#  Eval("Realizacion") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th>
                                                            <asp:Label ID="Label26" runat="server" Text="descripcion"></asp:Label>
                                                        </th>
                                                        <td colspan="5" class="RowStyle">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label27" runat="server" Text='<%#  Eval("Descripcion") %>'></asp:Label></pre>
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th rowspan="2">
                                                            <asp:Label ID="Label22" runat="server" Text="eficacia"></asp:Label>
                                                        </th>
                                                        <td class="RowStyle" colspan="4" rowspan="2">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label23" runat="server" Text='<%#  Eval("Eficacia") %>'></asp:Label></pre>
                                                        </td>
                                                        <th style="white-space: nowrap">
                                                            <asp:Label ID="Label25" runat="server" Text="responsables"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <td class="RowStyle">
                                                            <asp:BulletedList ID="blResponsablesAcciones" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td colspan="6">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvObservaciones_Provisional" runat="server" AutoGenerateColumns="False" ShowFooter="true" AllowSorting="True" RowHeaderColumn="ID" DataKeyNames="ID" CssClass="GridViewASP" Caption="observaciones">
                                                                        <RowStyle CssClass="RowStyle" />
                                                                        <FooterStyle CssClass="FooterStyle" />
                                                                        <PagerStyle CssClass="PagerStyle" />
                                                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                                        <HeaderStyle CssClass="HeaderStyle" />
                                                                        <EditRowStyle CssClass="EditRowStyle" />
                                                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                                                        <EmptyDataRowStyle CssClass="FooterStyle" />
                                                                        <EmptyDataTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text="Sin Datos"></asp:Label>
                                                                        </EmptyDataTemplate>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdAccion" HeaderText="Fecha" ItemStyle-Width="1px" Visible="false" />
                                                                            <asp:BoundField DataField="Fecha" HeaderText="fecha" ItemStyle-Width="1px" DataFormatString="{0:d}" />
                                                                            <asp:TemplateField ItemStyle-Wrap="false" AccessibleHeaderText="Usuario" HeaderText="Usuario">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Usuario.NombreCompleto")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="observaciones" ItemStyle-Width="100%" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <hr />
                                                            <fieldset style="text-align: center">
                                                                <asp:LinkButton ID="btnNuevaObservacion" runat="server" CommandName="NuevaObservacion" ToolTip="Nuevo">
                                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" />
                                                                </asp:LinkButton>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemSeparatorTemplate>
                                                <hr />
                                            </ItemSeparatorTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>

                        <act:CollapsiblePanelExtender ID="cpeDefinitiva" runat="server" TargetControlID="pnlDefinitiva" ExpandControlID="pnlCollapsed_Definitiva" CollapseControlID="pnlCollapsed_Definitiva" Collapsed="true" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                            ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                            CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                            ImageControlID="imgEstadoPanel_Definitiva" CollapsedText="Expandir" ExpandedText="Contraer" />
                        <table class="tablaBuscador" style="width: 100%;">
                            <tr>
                                <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                                    <asp:Panel runat="server" ID="pnlCollapsed_Definitiva">
                                        <table class="recuadro">
                                            <tr class="ImageButton">
                                                <td>
                                                    <asp:Image runat="server" ID="imgEstadoPanel_Definitiva" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
													<asp:Label ID="Label30" runat="server" Text="Definitivas" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlDefinitiva" runat="server" CssClass="recuadro">
                                        <asp:ListView ID="lvAcciones_Definitivas" runat="server" EnableModelValidation="true" DataKeyNames="Id" Enabled="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="Label11" runat="server" Text="Sin Datos" />
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <asp:Panel ID="itemPlaceholderContainer" runat="server">
                                                    <table runat="server" id="itemPlaceholder" />
                                                </asp:Panel>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <table id="lvAccion" runat="server" class="GridViewASP">
                                                    <tr class="HeaderStyle">
                                                        <td rowspan="5" style="vertical-align: top; width: 1px">
                                                            <asp:LinkButton ID="btnEditarAccion" runat="server" CommandName="Edit" ToolTip="Editar">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" />
                                                            </asp:LinkButton>
                                                        </td>
                                                        <th>
                                                            <%--<asp:Label ID="Label13" runat="server" Text="Tipo de Acción"></asp:Label>--%>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label14" runat="server" Text="FechaInicio"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label15" runat="server" Text="Fecha Prevista de Cierre"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label16" runat="server" Text="FechaFin"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label9" runat="server" Text="Demora" />
                                                            (<asp:Label ID="Label10" runat="server" Text="semanas" />)
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label12" runat="server" Text="Desarrollo"></asp:Label>(%)
                                                        </th>
                                                    </tr>
                                                    <tr class="RowStyle" style="text-align: center">
                                                        <td>
                                                            <%--<asp:Label ID="Label17" runat="server" Text='<%# IF(Eval("IdTipoAccion")=0, Nothing ,Eval("IdTipoAccion"))%>' />--%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label18" runat="server" Text='<%#  Eval("FechaInicio", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label19" runat="server" Text='<%#  Eval("FechaPrevista", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label20" runat="server" Text='<%#  Eval("FechaFin", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDemora" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label21" runat="server" Text='<%#  Eval("Realizacion") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th>
                                                            <asp:Label ID="Label26" runat="server" Text="descripcion"></asp:Label>
                                                        </th>
                                                        <td colspan="5" class="RowStyle">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label27" runat="server" Text='<%#  Eval("Descripcion") %>'></asp:Label></pre>
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th rowspan="2">
                                                            <asp:Label ID="Label22" runat="server" Text="eficacia"></asp:Label>
                                                        </th>
                                                        <td class="RowStyle" colspan="4" rowspan="2">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label23" runat="server" Text='<%#  Eval("Eficacia") %>'></asp:Label></pre>
                                                        </td>
                                                        <th style="white-space: nowrap">
                                                            <asp:Label ID="Label25" runat="server" Text="responsables"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <td class="RowStyle">
                                                            <asp:BulletedList ID="blResponsablesAcciones" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td colspan="6">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvObservaciones_Definitiva" runat="server" AutoGenerateColumns="False" ShowFooter="true" AllowSorting="True" RowHeaderColumn="ID" DataKeyNames="ID" CssClass="GridViewASP" Caption="observaciones">
                                                                        <RowStyle CssClass="RowStyle" />
                                                                        <FooterStyle CssClass="FooterStyle" />
                                                                        <PagerStyle CssClass="PagerStyle" />
                                                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                                        <HeaderStyle CssClass="HeaderStyle" />
                                                                        <EditRowStyle CssClass="EditRowStyle" />
                                                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                                                        <EmptyDataRowStyle CssClass="FooterStyle" />
                                                                        <EmptyDataTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text="Sin Datos"></asp:Label>
                                                                        </EmptyDataTemplate>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdAccion" HeaderText="Fecha" ItemStyle-Width="1px" Visible="false" />
                                                                            <asp:BoundField DataField="Fecha" HeaderText="fecha" ItemStyle-Width="1px" DataFormatString="{0:d}" />
                                                                            <asp:TemplateField ItemStyle-Wrap="false" AccessibleHeaderText="Usuario" HeaderText="Usuario">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Usuario.NombreCompleto")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="observaciones" ItemStyle-Width="100%" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <hr />
                                                            <fieldset style="text-align: center">
                                                                <asp:LinkButton ID="btnNuevaObservacion" runat="server" CommandName="NuevaObservacion" ToolTip="Nuevo">
                                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" />
                                                                </asp:LinkButton>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemSeparatorTemplate>
                                                <hr />
                                            </ItemSeparatorTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>


                        <act:CollapsiblePanelExtender ID="cpePreventiva" runat="server" TargetControlID="pnlPreventiva" ExpandControlID="pnlCollapsed_Preventiva" CollapseControlID="pnlCollapsed_Preventiva" Collapsed="true" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                            ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                            CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                            ImageControlID="imgEstadoPanel_Preventiva" CollapsedText="Expandir" ExpandedText="Contraer" />
                        <table class="tablaBuscador" style="width: 100%;">
                            <tr>
                                <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                                    <asp:Panel runat="server" ID="pnlCollapsed_Preventiva">
                                        <table class="recuadro">
                                            <tr class="ImageButton">
                                                <td>
                                                    <asp:Image runat="server" ID="imgEstadoPanel_Preventiva" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
													<asp:Label ID="Label32" runat="server" Text="Preventivas" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlPreventiva" runat="server" CssClass="recuadro">
                                        <asp:ListView ID="lvAcciones_Preventivas" runat="server" EnableModelValidation="true" DataKeyNames="Id" Enabled="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="Label11" runat="server" Text="Sin Datos" />
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <asp:Panel ID="itemPlaceholderContainer" runat="server">
                                                    <table runat="server" id="itemPlaceholder" />
                                                </asp:Panel>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <table id="lvAccion" runat="server" class="GridViewASP">
                                                    <tr class="HeaderStyle">
                                                        <td rowspan="5" style="vertical-align: top; width: 1px">
                                                            <asp:LinkButton ID="btnEditarAccion" runat="server" CommandName="Edit" ToolTip="Editar">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Editar24.png" />
                                                            </asp:LinkButton>
                                                        </td>
                                                        <th>
                                                            <%--<asp:Label ID="Label13" runat="server" Text="Tipo de Acción"></asp:Label>--%>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label14" runat="server" Text="FechaInicio"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label15" runat="server" Text="Fecha Prevista de Cierre"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label16" runat="server" Text="FechaFin"></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label9" runat="server" Text="Demora" />
                                                            (<asp:Label ID="Label10" runat="server" Text="semanas" />)
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label12" runat="server" Text="Desarrollo"></asp:Label>(%)
                                                        </th>
                                                    </tr>
                                                    <tr class="RowStyle" style="text-align: center">
                                                        <td>
                                                            <%--<asp:Label ID="Label17" runat="server" Text='<%# IF(Eval("IdTipoAccion")=0, Nothing ,Eval("IdTipoAccion"))%>' />--%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label18" runat="server" Text='<%#  Eval("FechaInicio", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label19" runat="server" Text='<%#  Eval("FechaPrevista", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label20" runat="server" Text='<%#  Eval("FechaFin", "{0:d}") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDemora" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label21" runat="server" Text='<%#  Eval("Realizacion") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th>
                                                            <asp:Label ID="Label26" runat="server" Text="descripcion"></asp:Label>
                                                        </th>
                                                        <td colspan="5" class="RowStyle">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label27" runat="server" Text='<%#  Eval("Descripcion") %>'></asp:Label></pre>
                                                        </td>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <th rowspan="2">
                                                            <asp:Label ID="Label22" runat="server" Text="eficacia"></asp:Label>
                                                        </th>
                                                        <td class="RowStyle" colspan="4" rowspan="2">
                                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="Label23" runat="server" Text='<%#  Eval("Eficacia") %>'></asp:Label></pre>
                                                        </td>
                                                        <th style="white-space: nowrap">
                                                            <asp:Label ID="Label25" runat="server" Text="responsables"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr class="HeaderStyle">
                                                        <td class="RowStyle">
                                                            <asp:BulletedList ID="blResponsablesAcciones" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td colspan="6">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvObservaciones_Preventiva" runat="server" AutoGenerateColumns="False" ShowFooter="true" AllowSorting="True" RowHeaderColumn="ID" DataKeyNames="ID" CssClass="GridViewASP" Caption="observaciones">
                                                                        <RowStyle CssClass="RowStyle" />
                                                                        <FooterStyle CssClass="FooterStyle" />
                                                                        <PagerStyle CssClass="PagerStyle" />
                                                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                                        <HeaderStyle CssClass="HeaderStyle" />
                                                                        <EditRowStyle CssClass="EditRowStyle" />
                                                                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                                                        <EmptyDataRowStyle CssClass="FooterStyle" />
                                                                        <EmptyDataTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text="Sin Datos"></asp:Label>
                                                                        </EmptyDataTemplate>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdAccion" HeaderText="Fecha" ItemStyle-Width="1px" Visible="false" />
                                                                            <asp:BoundField DataField="Fecha" HeaderText="fecha" ItemStyle-Width="1px" DataFormatString="{0:d}" />
                                                                            <asp:TemplateField ItemStyle-Wrap="false" AccessibleHeaderText="Usuario" HeaderText="Usuario">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Usuario.NombreCompleto")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="observaciones" ItemStyle-Width="100%" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <hr />
                                                            <fieldset style="text-align: center">
                                                                <asp:LinkButton ID="btnNuevaObservacion" runat="server" CommandName="NuevaObservacion" ToolTip="Nuevo">
                                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" />
                                                                </asp:LinkButton>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemSeparatorTemplate>
                                                <hr />
                                            </ItemSeparatorTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>



                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel runat="server" ID="tp_Documentos" HeaderText="Documentos Adjuntos">
                    <ContentTemplate>
                        <%--<table class="recuadro" style="width: auto;">
							<tr class="ImageButton">
								<td style="text-align: right;">--%>
                        <asp:ImageButton ID="btnNuevoDocumento" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo24.png" ToolTip="Nuevo" AlternateText="Nuevo" />
                        <%--	</td>
							</tr>
						</table>--%>
                        <asp:GridView ID="gvDocumentos" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="Documentos">
                            <Columns>
                                <asp:CommandField ShowEditButton="true" ButtonType="Link" />
                                <asp:CommandField ShowSelectButton="true" ButtonType="Link" Visible="false" />
                                <asp:TemplateField AccessibleHeaderText="" ConvertEmptyStringToNull="false" InsertVisible="true">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlDoc" runat="server" NavigateUrl="~/Incidencia/Mantenimiento/DocumentoBBDD.aspx" Target="_blank">
                                            <asp:Image ID="imgDoc" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/TipoArchivos/Document-Blank-icon24.png" BorderStyle="None" />
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                <asp:BoundField DataField="FECHACREACION" HeaderText="Fecha de Creacion" SortExpression="FECHACREACION" />
                                <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE" SortExpression="NOMBRE" />
                                <asp:BoundField DataField="TITULO" HeaderText="TITULO" SortExpression="TITULO" />
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCION" SortExpression="DESCRIPCION" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </act:TabPanel>
            </act:TabContainer>
            <%--<br />
			<fieldset style="text-align: center">
				<asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Volver" />
			</fieldset>--%>
            <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
