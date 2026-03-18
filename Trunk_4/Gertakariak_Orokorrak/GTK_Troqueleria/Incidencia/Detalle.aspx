<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Detalle.aspx.vb" Inherits="GTK_Troqueleria.Detalle" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="App_Themes/Batz/theme.css" />
    <link rel="Stylesheet" type="text/css" href="App_Themes/Batz/dialog.css" />
    <link rel="Stylesheet" type="text/css" href="App_Themes/Batz/core.css" />
    <script type="text/javascript">
        window.onload = pageLoad

        function pageLoad(sender, args) {
<%--            $(document).ready(function () { $get("<%=pnlEtapa_2.ClientID%>").parentElement.style.height = "auto"; });
            $(document).ready(function () { $get("<%=pnlEtapa_3.ClientID%>").parentElement.style.height = "auto"; });
            $(document).ready(function () { $get("<%=pnlEtapa_5.ClientID%>").parentElement.style.height = "auto"; });
            $(document).ready(function () { $get("<%=pnlEtapa_6.ClientID%>").parentElement.style.height = "auto"; });
            $(document).ready(function () { $get("<%=pnlEtapa_7.ClientID%>").parentElement.style.height = "auto"; });
            $(document).ready(function () { $get("<%=pnlEtapa_8.ClientID%>").parentElement.style.height = "auto"; });
        --%>
        }
        //$(document).ready(function () {
        //    var modal = document.getElementById("modalCognos");
        //    alert(modal);
        //    var btn = document.getElementById("btnImprimir8D");
        //    alert(btn);
        //    btn.onclick = function () {
        //        modal.style.display = "block";
        //    }
        //});
        
        function btnImprimir8D() {
            var modal = document.getElementById("<%=modalCognos.ClientID%>");
            modal.style.display = "block";
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <asp:Panel ID="pnlBotonesDetalle" runat="server" CssClass="PanelBotones">
        <asp:ImageButton ID="btnNuevaIncidencia" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" AlternateText="Nueva Incidencia" ToolTip="Nueva Incidencia" />
        <asp:ImageButton ID="btnBorrar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" AlternateText="Eliminar" ToolTip="Eliminar" />
        <act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnBorrar" ConfirmText="Desea eliminar" Enabled="True" />
        <%--<asp:ImageButton ID="btnImprimir" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/printer-icon16.png" AlternateText="8D" ToolTip="8D" />--%>
        <asp:ImageButton ID="btnImprimir8D" CssClass="btnImprimir8D" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/printer-icon16.png" AlternateText="Nuevo 8D" ToolTip="Nuevo 8D" OnClientClick="btnImprimir8D();return false;" />
    </asp:Panel>
    <%--<asp:Panel ID="pnlLeccionAprendida" runat="server" CssClass="PanelBotones">
        <asp:ImageButton ID="btnLeccionAprendida" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/LeccionesAprendidas16.png" AlternateText="Leccion Aprendida" ToolTip="Leccion Aprendida" />
    </asp:Panel>--%>
    <ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº" />
    <act:TabContainer ID="tc_Detalle" runat="server" ActiveTabIndex="0" ScrollBars="Auto" >
        <act:TabPanel runat="server" ID="tp_DatosGenerales" HeaderText="Datos Generales (Etapa 1-2)">
            <ContentTemplate>
                <asp:Panel ID="pnlDatosGenerales" runat="server" CssClass="PanelBotones">
                    <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
                    <asp:ImageButton ID="btnNotificarNC" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Send-icon16.png" AlternateText="Notificar" ToolTip="Notificar" />
                    <act:ConfirmButtonExtender ID="cbe_btnNotificarNC" runat="server" TargetControlID="btnNotificarNC" ConfirmText="Notificar" Enabled="True" />
                    <asp:Label ID="lblSinNotificar" runat="server" Text="Sin Notificar" CssClass="MensajeError"></asp:Label>
                </asp:Panel>
                
<%--                <asp:Panel id="pnlCierre" runat="server">
                    <asp:Label id="lblCierre" runat="server" Text="Cierre"/>
                    <asp:Button id="aceptarCierre" runat="server" Text="Aceptar"/>
                    <asp:Button id="rechazarCierre" runat="server" Text="Rechazar"/>
                </asp:Panel>--%>

                <table class="GridViewASP" id="pnlCierre2" runat="server" style="width:auto;">
                    <tr class="HeaderStyle">
                        <th colspan="2"><asp:Label ID="Label114" runat="server" Text="Cierre"></asp:Label></th>
                    </tr>
                    <tr class="RowStyle">
                        <td><asp:Button id="aceptarCierre" runat="server" Text="Aceptar"/></td>
<%--                        <td><asp:Button id="rechazarCierre" runat="server" Text="Rechazar" OnClientClick="rechazarCierre()"/></td>--%>
                            <td><input type="button" onclick="rechazarCierre()" value="Rechazar" runat="server" /></td>
                    </tr>
                </table>


                <asp:Panel id="pnlCierre5" runat="server">
                    <asp:Button id="reabrirIncidencia" runat="server" Text="Reabrir incidencia"/>
                </asp:Panel>

                <table class="GridViewASP" style="width: 1%;">
                    <tr class="HeaderStyle">
                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label31" runat="server" Text="OF-OP (Marca)"></asp:Label>
                        </th>
                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label60" runat="server" Text="Proyecto"></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label21" runat="server" Text="Deteccion"></asp:Label>
                        </th>
                    </tr>
                    <tr class="RowStyle">
                        <td>
                            <asp:ListView ID="lv_OFOPM" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lv_OFOPM_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li style="white-space: nowrap;">
                                        <input name="hd_OFOPM" type="hidden" value='<%#Eval("Id")%>'><%#Eval("Descripcion")%>
                                    </li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lv_OFOPM_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:Label runat="server" ID="lblProyecto"></asp:Label>
                        </td>
                        <td style="text-wrap: none; white-space: nowrap;">
                            <asp:BulletedList ID="blDeteccion" runat="server" DisplayMode="Text" ViewStateMode="Disabled" ToolTip="Deteccion" />
                        </td>
                    </tr>
                </table>


                <table class="GridViewASP" style="width: 1%;">
                    <tr class="HeaderStyle">
                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label5" runat="server" Text="Procedencia"></asp:Label>
                        </th>
                        <th style="white-space: nowrap;">
                            <asp:Label ID="lblOrigen" runat="server" Text="?"></asp:Label>
                        </th>
                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label104" runat="server" Text="Producto"></asp:Label>
                        </th>
                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label106" runat="server" Text="Caracteristicas / Tipo Error"></asp:Label>
                        </th>

                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label2" runat="server" Text="Fecha Inicio"></asp:Label>
                        </th>
                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label4" runat="server" Text="Fecha Cierre"></asp:Label>
                        </th>
                        <th style="white-space: nowrap;">
                            <asp:Label ID="Label40" runat="server" Text="Retraso" ToolTip="Semanas"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProcedencia" runat="server"></asp:Label></td>
                                    <td style="text-wrap: none; white-space: nowrap;">
                                        <asp:BulletedList ID="blProveedor" runat="server" DisplayMode="Text" ViewStateMode="Disabled" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                            <asp:BulletedList ID="blProcedencia" runat="server" DisplayMode="Text" ViewStateMode="Disabled" />
                        </td>
                        <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                            <asp:BulletedList ID="blProducto" runat="server" DisplayMode="Text" ViewStateMode="Disabled" />
                        </td>
                        <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                            <asp:BulletedList ID="blCaracteristica" runat="server" DisplayMode="Text" ViewStateMode="Disabled" />
                        </td>

                        <td class="RowStyle">
                            <asp:Label ID="lblFechaInicio" runat="server"></asp:Label>
                        </td>
                        <td class="RowStyle">
                            <asp:Label ID="lblFechaCierre" runat="server"></asp:Label>
                        </td>
                        <td class="RowStyle">
                            <asp:Label ID="lblRetraso" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>

                <table class="GridViewASP">
                    <tr class="HeaderStyle" style="width: 1%; white-space: nowrap;">
                        <th>
                            <asp:Label ID="Label1" runat="server" Text="Creador"></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label16" runat="server" Text="Perseguidor" ToolTip="Encargado de Validar las etapas del 8D"></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label22" runat="server" Text="Responsable" ToolTip="Responsable de Resolucion"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td class="RowStyle" style="width: 1%; white-space: nowrap;">
                            <ul>
                                <li>
                                    <asp:Label ID="lblCreador" runat="server"></asp:Label></li>
                            </ul>
                        </td>
                        <td class="RowStyle">
                            <asp:ListView ID="lvResponsables" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvResponsables_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li>
                                        <%#Eval("NombreCompleto")%>
                                    </li>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                        <td class="RowStyle">
                            <asp:BulletedList ID="blRespResolucion" runat="server" DataTextField="NombreCompleto"></asp:BulletedList>
                        </td>
                    </tr>
                </table>
                <table class="GridViewASP">
                    <tr class="HeaderStyle">
                        <th style="width: 1%;">
                            <asp:Label ID="Label3" runat="server" Text="Descripcion"></asp:Label>
                        </th>
                        <td class="RowStyle">
                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblDescripcion" runat="server"></asp:Label></pre>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr class="HeaderStyle">
                        <th style="width: 1%;">
                            <asp:Label ID="Label38" runat="server" Text="Caracteristicas"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataList ID="dlEstructuras" runat="server" GridLines="None" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4" Width="96%"
                                EnableViewState="false" ViewStateMode="Disabled">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
                                <ItemTemplate>
                                    <asp:TreeView ID="tvEstructura" runat="server" SkinID="TreeView" />
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>

                <!-- AQUÍ METEREMOS LA ETAPA 2 -->                
                <table style="width:100%">
                    <tr>
                        <td>
                            <asp:Panel ID="Panel10" runat="server" CssClass="recuadro">
                                <table class="GridViewASP">
                                    <caption>
                                        <asp:Label ID="Label108" runat="server" Text="Acciones de contencion/inmediatas"></asp:Label></caption>
                                    <tr class="HeaderStyle">
                                        <th colspan="3">
                                            <asp:Label ID="Label109" runat="server" Text="¿Qué acciones de contención/inmediatas han sido llevadas a cabo para garantizar la continuidad de la fabricación del troquel?"></asp:Label></th>
                                    </tr>
                                    <tr class="HeaderStyle">
                                        <th style="width: 1%; white-space: nowrap;">
                                            <asp:Label ID="Label110" runat="server" Text="Considerar:"></asp:Label></th>
                                        <th colspan="2">
                                            <asp:Label ID="Label111" runat="server" Text="Acciones tomadas"></asp:Label></th>
                                    </tr>
                                    <tr class="AlternatingRowStyle">
                                        <th align="left" style="white-space: nowrap;">
                                            <asp:Label ID="Label112" runat="server" Text="Detalles de las acciones de contención/inmediatas:"></asp:Label></th>
                                        <th align="left" style="white-space: nowrap; width: 1%;">
                                            <asp:Label ID="Label113" runat="server" Text="Fecha de cierre de acciones inmediatas"></asp:Label></th>
                                        <td>
                                            <asp:Label ID="lbl_E1_DESCRIPCION_6" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <td rowspan="3">
                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lbl_E1_DESCRIPCION_5" runat="server" /></pre>
                                        </td>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <!-- -->

                <table class="GridViewASP" style="width: 1%;">
                    <caption>
                        <asp:Label ID="Label27" runat="server" Text="PERSONAS A INFORMAR" />
                        <asp:Panel ID="pnlPersonasNotificar" runat="server" CssClass="PanelBotones">
                            <asp:ImageButton ID="btnPersonasInformar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Send-icon16.png" AlternateText="Notificar" ToolTip="Notificar" />
                            <act:ConfirmButtonExtender ID="cbe_btnPersonasInformar" runat="server" TargetControlID="btnPersonasInformar" ConfirmText="Notificar" Enabled="True" />
                        </asp:Panel>
                    </caption>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label30" runat="server" Text="Gestor" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvGestor" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvGestor_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvGestor_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label94" runat="server" Text="PROVEEDOR (T cerrado, Mec, diseños…)" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvCoordinador_Fabricacion" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvCoordinador_Fabricacion_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvCoordinador_Fabricacion_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label95" runat="server" Text="ALMACÉN DE MATERIALES /CHAPA" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvCalidad_Fabricacion" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvCalidad_Fabricacion_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvCalidad_Fabricacion_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label96" runat="server" Text="EXPEDICIONES/LOGÍSTICA" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvCalidad_proveedores" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvCalidad_proveedores_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvCalidad_proveedores_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label97" runat="server" Text="DISEÑO" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvCalidad_Cliente" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvCalidad_Cliente_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvCalidad_Cliente_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label98" runat="server" Text="COMPRAR" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvAlmacen" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvAlmacen_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvAlmacen_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label99" runat="server" Text="MÁQUINAS" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvIngenieriaFabricacion" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvIngenieriaFabricacion_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvIngenieriaFabricacion_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label6" runat="server" Text="AJUSTE" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvAjuste" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvAjuste_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvAjuste_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label20" runat="server" Text="SEGUIMIENTO" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvSeguimiento" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvSeguimiento_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvSeguimiento_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label25" runat="server" Text="MEDICIÓN" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvMedicion" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvMedicion_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvMedicion_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label26" runat="server" Text="HOMOLOGACIÓN" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvHomologacion" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvHomologacion_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <ul id="lvHomologacion_UL"></ul>
                                </EmptyDataTemplate>
                                <EmptyItemTemplate>
                                    ??
                                </EmptyItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width: 1%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="Label100" runat="server" Text="Otros" />
                        </th>
                        <td class="RowStyle" style="white-space: nowrap;">
                            <asp:ListView ID="lvOtros" runat="server" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <ul id="lvOtros_UL">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("NombreCompleto")%></li>
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
                </table>
            </ContentTemplate>
        </act:TabPanel>

<%--        <act:TabPanel runat="server" ID="tp_E14" HeaderText="Etapa 2-4">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlBotones_E14" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="btnEditar_E14" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
                                <asp:ImageButton ID="btnSolicitarAprobacion_E14" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/EtapasEstado/application-edit-icon16.png" AlternateText="Solicitar Aprobacion" ToolTip="Solicitar Aprobacion" />
                                <act:ConfirmButtonExtender ID="cbe_btnSolicitarAprobacion_E14" runat="server" TargetControlID="btnSolicitarAprobacion_E14" ConfirmText="Solicitar Aprobacion" Enabled="True" />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="pnlAprobacion2_E14" runat="server" CssClass="PanelBotones" Visible="false">
                                <asp:ImageButton ID="btnAprobacion_E14" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar16.png" AlternateText="Aprobar" ToolTip="Aprobar" />
                                <asp:ImageButton ID="btnDesaprobacion_E14" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar16.png" AlternateText="Desaprobar" ToolTip="Desaprobar" />
                                <act:ModalPopupExtender ID="mpe_btnDesaprobacion_E14" runat="server" TargetControlID="btnDesaprobacion_E14" PopupControlID="pnlDescRechazo_E14" CancelControlID="imgCancelar_E14" BackgroundCssClass="modalBackground" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlRechazo_E14" runat="server" CssClass="recuadro" Visible="false" GroupingText="Razones de Desaprobacion">
                    <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblDescRechazo_E14" runat="server"></asp:Label></pre>
                </asp:Panel>
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
                                <asp:Label ID="lblE14_FI" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE14_FF" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE14_FC" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE14_FV" runat="server" Text="?" /></td>
                        </tr>
                    </tbody>
                </table>
                <act:CollapsiblePanelExtender ID="cpeEtapa_2" runat="server" TargetControlID="pnlEtapa_2" ExpandControlID="pnlCollapsed_E2" CollapseControlID="pnlCollapsed_E2" Collapsed="false" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                    ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                    CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                    ImageControlID="imgEstadoPanel_E2" CollapsedText="Expandir" ExpandedText="Contraer" />
                <act:CollapsiblePanelExtender ID="cpeEtapa_3" runat="server" TargetControlID="pnlEtapa_3" ExpandControlID="pnlCollapsed_E3" CollapseControlID="pnlCollapsed_E3" Collapsed="false" CollapsedSize="0" ScrollContents="False" ExpandDirection="Vertical" SuppressPostBack="true"
                    ExpandedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg"
                    CollapsedImage="~/App_Themes/Batz/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                    ImageControlID="imgEstadoPanel_E3" CollapsedText="Expandir" ExpandedText="Contraer" />
                <table class="tablaBuscador" style="width: 100%;">
                    <tr>
                        <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                            <asp:Panel runat="server" ID="pnlCollapsed_E2">
                                <table class="recuadro">
                                    <tr class="ImageButton">
                                        <td>
                                            <asp:Image runat="server" ID="imgEstadoPanel_E2" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
											<asp:Label ID="lblEtapa_2" runat="server" Text="Etapa 3 - Otros productos o procesos afectados"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlEtapa_2" runat="server" CssClass="recuadro">
                                <table class="GridViewASP">
                                    <caption>
                                        <asp:Label ID="Label41" runat="server" Text="¿Puede este problema afectar a otros proyectos u OFs?"></asp:Label>
                                    </caption>
                                    <tr class="HeaderStyle">
                                        <th style="width: 1%;"></th>
                                        <th style="width: 1%;">
                                            <asp:Label ID="Label42" runat="server" Text="Si"></asp:Label></th>
                                        <th style="width: 1%;" align="center">
                                            <asp:Label ID="Label43" runat="server" Text="No"></asp:Label></th>
                                        <th align="center">
                                            <asp:Label ID="Label45" runat="server" Text="Comentarios/Resultados"></asp:Label></th>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th align="left" style="white-space: nowrap;">
                                            <asp:Label ID="Label46" runat="server" Text="Otras Plantas/Clientes afectados"></asp:Label></th>
                                        <td align="center">
                                            <asp:CheckBox ID="cb_E2_AFECTAR1_S" runat="server" Enabled="false" /></td>
                                        <td align="center">
                                            <asp:CheckBox ID="cb_E2_AFECTAR1_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E2_AFECTAR1" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="AlternatingRowStyle">
                                        <th align="left" style="white-space: nowrap;">
                                            <asp:Label ID="Label48" runat="server" Text="Otras OF del mismo proyecto"></asp:Label></th>
                                        <td align="center">
                                            <asp:CheckBox ID="cb_E2_AFECTAR2_S" runat="server" Enabled="false" /></td>
                                        <td align="center">
                                            <asp:CheckBox ID="cb_E2_AFECTAR2_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E2_AFECTAR2" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th align="left" style="white-space: nowrap;">
                                            <asp:Label ID="Label51" runat="server" Text="Otros proyectos"></asp:Label></th>
                                        <td align="center">
                                            <asp:CheckBox ID="cb_E2_AFECTAR3_S" runat="server" Enabled="false" /></td>
                                        <td align="center">
                                            <asp:CheckBox ID="cb_E2_AFECTAR3_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E2_AFECTAR3" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <th onmouseover="this.style.cursor='pointer'" style="text-align: left;">
                            <asp:Panel runat="server" ID="pnlCollapsed_E3">
                                <table class="recuadro">
                                    <tr class="ImageButton">
                                        <td>
                                            <asp:Image runat="server" ID="imgEstadoPanel_E3" ImageAlign="AbsMiddle" BorderWidth="0"></asp:Image>&nbsp; 
											<asp:Label ID="lblEtapa_3" runat="server" Text="Etapa 4 - Primer análisis del problema"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlEtapa_3" runat="server" CssClass="recuadro">
                                <table class="GridViewASP" style="width: 100%;">
                                    <tr class="HeaderStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label86" runat="server" Text="¿Dónde se debería haber detectado esta No Conformidad?"></asp:Label></th>
                                        <th>
                                            <asp:Label ID="Label87" runat="server" Text="Si"></asp:Label></th>
                                        <th>
                                            <asp:Label ID="Label88" runat="server" Text="No"></asp:Label></th>
                                        <th style="width: 100%;" align="center">
                                            <asp:Label ID="Label59" runat="server" Text="Comentarios/Resultados"></asp:Label></th>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label89" runat="server" Text="Simulación"></asp:Label></th>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS1_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS1_N" runat="server" Enabled="false" /></td>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lbl_E3_ANALISIS_DESC_1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="AlternatingRowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label90" runat="server" Text="Revision Modelos"></asp:Label></th>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS2_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS2_N" runat="server" Enabled="false" /></td>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lbl_E3_ANALISIS_DESC_2" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label91" runat="server" Text="Revisión Diseños"></asp:Label></th>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS3_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS3_N" runat="server" Enabled="false" /></td>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lbl_E3_ANALISIS_DESC_3" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="AlternatingRowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label92" runat="server" Text="Proveedor TC / Homologación TC"></asp:Label></th>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS4_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS4_N" runat="server" Enabled="false" /></td>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lbl_E3_ANALISIS_DESC_4" runat="server" />
                                        </td>
                                    </tr>

                                    <tr class="AlternatingRowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label50" runat="server" Text="Fabricación / Mecanizado"></asp:Label></th>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS5_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS5_N" runat="server" Enabled="false" /></td>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lbl_E3_ANALISIS_DESC_5" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="AlternatingRowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label57" runat="server" Text="Control de Calidad (medición de pieza)"></asp:Label></th>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS6_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS6_N" runat="server" Enabled="false" /></td>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lbl_E3_ANALISIS_DESC_6" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="AlternatingRowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label58" runat="server" Text="Homologación final"></asp:Label></th>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS7_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E3_ANALISIS7_N" runat="server" Enabled="false" /></td>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lbl_E3_ANALISIS_DESC_7" runat="server" />
                                        </td>
                                    </tr>

                                    <tr class="HeaderStyle">
                                        <th colspan="3" style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label93" runat="server" Text="¿Cuáles son las razones de la no detección?"></asp:Label></th>
                                    </tr>
                                    <tr class="AlternatingRowStyle">
                                        <td colspan="3">
                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lbl_E3_DESCRIPCION" runat="server"/></pre>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>

                </table>
            </ContentTemplate>
        </act:TabPanel>
        <act:TabPanel runat="server" ID="tp_E56" HeaderText="Etapa 5-6">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlBotones_E56" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="btnEditar_E56" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
                                <asp:ImageButton ID="btnSolicitarAprobacion_E56" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/EtapasEstado/application-edit-icon16.png" AlternateText="Solicitar Aprobacion" ToolTip="Solicitar Aprobacion" />
                                <act:ConfirmButtonExtender ID="cbe_btnSolicitarAprobacion_E56" runat="server" TargetControlID="btnSolicitarAprobacion_E56" ConfirmText="Solicitar Aprobacion" Enabled="True" />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="pnlAprobacion2_E56" runat="server" CssClass="PanelBotones" Visible="false">
                                <asp:ImageButton ID="btnAprobacion_E56" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar16.png" AlternateText="Aprobar" ToolTip="Aprobar" />
                                <asp:ImageButton ID="btnDesaprobacion_E56" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar16.png" AlternateText="Desaprobar" ToolTip="Desaprobar" />
                                <act:ModalPopupExtender ID="mpe_btnDesaprobacion_E56" runat="server" TargetControlID="btnDesaprobacion_E56" PopupControlID="pnlDescRechazo_E56" CancelControlID="imgCancelar_E56" BackgroundCssClass="modalBackground" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlRechazo_E56" runat="server" CssClass="recuadro" Visible="false" GroupingText="Razones de Desaprobacion">
                    <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblDescRechazo_E56" runat="server"></asp:Label></pre>
                </asp:Panel>
                <table class="GridViewASP">
                    <thead class="HeaderStyle">
                        <tr>
                            <th>
                                <asp:Label ID="Label10" runat="server" Text="Fecha Inicio"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label11" runat="server" Text="Fecha Fin (Previsto)"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label12" runat="server" Text="Fecha Solicitud de Aprobacion"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label33" runat="server" Text="Fecha Aprobación"></asp:Label></th>
                        </tr>
                    </thead>
                    <tbody class="RowStyle">
                        <tr>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE56_FI" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE56_FF" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE56_FC" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE56_FV" runat="server" Text="?" /></td>
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
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="vertical-align: middle; width: 1%;">
                                                        <asp:Panel ID="pnlBotones_5PQ" runat="server" CssClass="PanelBotones">
                                                            <asp:ImageButton ID="imgNuevo5PQ" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" AlternateText="Nuevo Porque" ToolTip="Nuevo Porque" />
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle; width: 99%;" class="recuadro">
                                                        <asp:Label ID="Label49" runat="server" Text="¿Porqué el Proceso de Fabricación ha podido fabricar o generar ese defecto?"></asp:Label>
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
                                                        <asp:Label ID="Label47" runat="server" Text="¿Porqué el Plan de control no ha detectado ese defecto?"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; width: 50%;">
                                            <asp:GridView SkinID="GridView" ID="gv5PQ_PF" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="5 Porques (Proceso Fabricacion)" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:BoundField DataField="REALIZACION" HeaderText="Orden" SortExpression="REALIZACION" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="¿Por que?" SortExpression="DESCRIPCION" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="Label44" runat="server" Text="Sin Datos"></asp:Label>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <asp:GridView SkinID="GridView" ID="gv5PQ_PC" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="5 Porques (Proceso Control)" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:BoundField DataField="REALIZACION" HeaderText="Orden" SortExpression="REALIZACION" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="¿Por que?" SortExpression="DESCRIPCION" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="Label44" runat="server" Text="Sin Datos"></asp:Label>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr class="HeaderStyle">
                                        <th colspan="2">
                                            <asp:Label ID="Label24" runat="server" Text="Análisis final (Causa raiz)" /></th>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;" colspan="2">
                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblCausaRaiz_PF" runat="server" /></pre>
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
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="vertical-align: middle; width: 1%;">
                                                        <asp:Panel ID="pnlAcciones" runat="server" CssClass="PanelBotones">
                                                            <asp:ImageButton ID="btnNuevaAccion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" AlternateText="Nueva Acción" ToolTip="Nueva Acción" />
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle; width: 99%;" class="recuadro">
                                                        <asp:Label ID="Label52" runat="server" Text="Las acciones deben definirse para atacar la causa raíz (final) principalmente y evitar que dicho problema se repita"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView SkinID="GridView" ID="gvAcciones" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="Acciones Definitivas" AutoGenerateColumns="false">
                                                <Columns>
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
            </ContentTemplate>
        </act:TabPanel>
        <act:TabPanel runat="server" ID="tp_E78" HeaderText="Etapa 7-8">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlBotones_E78" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="btnEditar_E78" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
                                <asp:ImageButton ID="btnSolicitarAprobacion_E78" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/EtapasEstado/application-edit-icon16.png" AlternateText="Solicitar Aprobacion" ToolTip="Solicitar Aprobacion" />
                                <act:ConfirmButtonExtender ID="cbe_btnSolicitarAprobacion_E78" runat="server" TargetControlID="btnSolicitarAprobacion_E78" ConfirmText="Solicitar Aprobacion" Enabled="True" />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="pnlAprobacion2_E78" runat="server" CssClass="PanelBotones" Visible="false">
                                <asp:ImageButton ID="btnAprobacion_E78" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar16.png" AlternateText="Aprobar" ToolTip="Aprobar" />
                                <asp:ImageButton ID="btnDesaprobacion_E78" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar16.png" AlternateText="Desaprobar" ToolTip="Desaprobar" />
                                <act:ModalPopupExtender ID="mpe_btnDesaprobacion_E78" runat="server" TargetControlID="btnDesaprobacion_E78" PopupControlID="pnlDescRechazo_E78" CancelControlID="imgCancelar_E78" BackgroundCssClass="modalBackground" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlRechazo_E78" runat="server" CssClass="recuadro" Visible="false" GroupingText="Razones de Desaprobacion">
                    <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lblDescRechazo_E78" runat="server"></asp:Label></pre>
                </asp:Panel>
                <table class="GridViewASP">
                    <thead class="HeaderStyle">
                        <tr>
                            <th>
                                <asp:Label ID="Label13" runat="server" Text="Fecha Inicio"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label14" runat="server" Text="Fecha Fin (Previsto)"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label15" runat="server" Text="Fecha Solicitud de Aprobacion"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label34" runat="server" Text="Fecha Aprobación"></asp:Label></th>
                        </tr>
                    </thead>
                    <tbody class="RowStyle">
                        <tr>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE78_FI" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE78_FF" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE78_FC" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblE78_FV" runat="server" Text="?" /></td>
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
                                            <asp:CheckBox ID="cb_E7_ACCIONES_S" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E7_ACCIONES_N" runat="server" Enabled="false" /></td>
                                    </tr>
                                    <tr class="HeaderStyle">
                                        <th colspan="3">
                                            <asp:Label ID="Label82" runat="server" Text="¿Como?"></asp:Label></th>
                                    </tr>
                                    <tr class="RowStyle">
                                        <td colspan="3">
                                            <pre style="white-space: pre-wrap; font: inherit;"><asp:Label ID="lbl_E7_ACCIONES_DESC" runat="server"/></pre>
                                        </td>
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
                                            <asp:Label ID="Label85" runat="server" Text="No"></asp:Label></th>
                                        <th>
                                            <asp:Label ID="Label68" runat="server" Text="Resp."></asp:Label></th>
                                        <th>
                                            <asp:Label ID="Label69" runat="server" Text="Plazos"></asp:Label></th>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label70" runat="server" Text="Instrucciones/procedimientos de trabajo"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES1" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES1_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES1_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES1_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label71" runat="server" Text="Normas Técnicas/especificaciones"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES2" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES2_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES2_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES2_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label72" runat="server" Text="Procesos (flujogramas…)"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES3" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES3_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES3_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES3_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label73" runat="server" Text="Check list PM"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES4" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES4_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES4_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES4_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label74" runat="server" Text="Check list Diseño"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES5" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES5_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES5_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES5_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label75" runat="server" Text="Check list modelos"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES6" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES6_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES6_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES6_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label76" runat="server" Text="Check list Homologación TC"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES7" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES7_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES7_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES7_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label77" runat="server" Text="Check list Homologación final"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES8" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES8_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES8_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES8_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr class="RowStyle">
                                        <th style="text-align: left; white-space: nowrap;">
                                            <asp:Label ID="Label78" runat="server" Text="Otros"></asp:Label></th>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES9" runat="server" Enabled="false" /></td>
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="cb_E8_ACCIONES9_N" runat="server" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES9_RESP" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lbl_E8_ACCIONES9_PLAZO" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </act:TabPanel>--%>
        <act:TabPanel runat="server" ID="tp_Documentos">
            <HeaderTemplate>
                <asp:Label ID="Label39" runat="server" Text="Documentos Adjuntos"></asp:Label><asp:Image ID="imgAdvDocumentos" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/TipoArchivos/Document-Blank-icon16.png" ImageAlign="Middle" Visible="false" ToolTip="Documentos Adjuntos" AlternateText="Documentos Adjuntos" />
            </HeaderTemplate>
            <ContentTemplate>
                <asp:Panel ID="pnl_BtnDocumentos" runat="server" CssClass="PanelBotones">
                    <asp:ImageButton ID="btnNuevoDocumento" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" ToolTip="Nuevo" AlternateText="Nuevo" />
                </asp:Panel>
                <asp:GridView SkinID="GridView" ID="gvDocumentos" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="Documentos">
                    <Columns>
                        <asp:CommandField ShowEditButton="true" ButtonType="Link" />
                        <asp:CommandField ShowSelectButton="true" ButtonType="Link" Visible="false" />
                        <asp:TemplateField AccessibleHeaderText="" ConvertEmptyStringToNull="false" InsertVisible="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlDoc" runat="server" NavigateUrl="~/Controles/DocumentoBBDD.aspx" Target="_blank">
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
        <act:TabPanel runat="server" ID="tp_LineasCoste">
            <HeaderTemplate>
                <asp:Label ID="Label84" runat="server" Text="Coste" ToolTip="Coste" />
                <asp:Label ID="lblCoste" runat="server" Text="€" ToolTip="Coste" Visible="false" />
            </HeaderTemplate>
            <ContentTemplate>
                <asp:Panel ID="pnlCoste" runat="server" CssClass="PanelBotones">
                    <asp:ImageButton ID="btnNuevoCoste" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" ToolTip="Nuevo" AlternateText="Nuevo" />
                    <asp:ImageButton ID="btnBuscarLineaCoste" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" AlternateText="Buscar Linea de Coste" ToolTip="Buscar Linea de Coste" />
                    <asp:ImageButton ID="btnTramitar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Euro-icon16.png" ToolTip="Tramitar Coste" AlternateText="Tramitar Coste" />
                </asp:Panel>
                <asp:GridView SkinID="GridView" ID="gvLineasCostes" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="Lineas de Costes" AutoGenerateColumns="false">
                    <Columns>
                        <asp:CommandField ShowEditButton="true" ButtonType="Link" ItemStyle-Width="1%" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label65" runat="server" Text="Proveedor"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProveedor" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label66" runat="server" Text="OF-OP (Marca)"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOFOPM" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NUMPED" HeaderText="NUMPED" SortExpression="NUMPED" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField SortExpression="NUMPEDORIGEN" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton5" runat="server" Text="Nº Pedido Origen" ToolTip="Nº del pedido del que parte la NC o pedido al que se le va a descontar la linea" CommandName="Sort" CommandArgument="NUMPEDORIGEN"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label64" runat="server" Text='<%#Eval("NUMPEDORIGEN") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION" />
                        <asp:TemplateField SortExpression="HORAS" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Right">
                            <HeaderTemplate>
                                <asp:Label ID="Label64" runat="server" Text="Horas" ToolTip="Horas Bonos"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblHORAS" runat="server" Text='<%#Eval("HORAS") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterStyle Font-Overline="true" Font-Underline="true" />
                            <FooterTemplate>
                                <asp:Label ID="lblHORAS_F" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="TASA" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label ID="Label63" runat="server" Text="Tasa" ToolTip="Tasa (por defecto: 55)"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTASA" runat="server" Text='<%#Eval("TASA") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterStyle Font-Overline="true" Font-Underline="true" />
                            <FooterTemplate>
                                <asp:Label ID="lblTASA_F" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="IMPORTE" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Right">
                            <HeaderTemplate>
                                <asp:LinkButton ID="btnImporte" runat="server" Text='Importe' CommandName="Sort" CommandArgument="IMPORTE"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblIMPORTE" runat="server" Text='<%#Eval("IMPORTE") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterStyle Font-Overline="true" Font-Underline="true" />
                            <FooterTemplate>
                                <asp:Label ID="lblIMPORTE_F" runat="server"></asp:Label>

                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label44" runat="server" Text="Sin Datos"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:Panel ID="pnlTotalAcordado" runat="server">
                    <table class="GridViewASP">
                        <tr class="HeaderStyle" style="text-align: center;">
                            <th>
                                <asp:Label ID="Label103" runat="server" Text="totalAcordado"></asp:Label>
                            </th>
                        </tr>
                        <tr class="AlternatingRowStyle ">
                            <td class="negrita" style="text-align: center;">
                                <asp:Label ID="lblAcordado" runat="server" Font-Overline="true" Font-Underline="true"></asp:Label><br />
                                <asp:CheckBox ID="chkCompensado" runat="server" Text="compensado" TextAlign="Right" />
                            </td>
                        </tr>
                        <tr class="RowStyle">
                            <td>
                                <pre><asp:Label ID="lblObservacionesCoste" runat="server"></asp:Label></pre>
                            </td>
                        </tr>
                        <tr class="RowStyle">
                            <td class="FooterStyle">
                                <asp:Panel ID="Panel1" runat="server" CssClass="PanelBotones">
                                    <asp:ImageButton runat="server" ID="btnTotalAcordado" ToolTip="Editar Total Acordado" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" ImageAlign="AbsMiddle" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:ListView ID="lvAgrupacionesLC" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <hr />
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <asp:Panel ID="pnlAgrupacionLC" runat="server" CssClass="recuadro">
                            <asp:Panel ID="pnlBotones_AgrupacionLC" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="btnCancelar_AgrupacionLC" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" AlternateText="Cancelar" ToolTip="Cancelar" CommandName="Delete" />
                                <act:ConfirmButtonExtender ID="cfe_btnCancelar_AgrupacionLC" runat="server" TargetControlID="btnCancelar_AgrupacionLC" ConfirmText="Desea eliminar" Enabled="True" />
                            </asp:Panel>
                            <asp:Panel ID="pnlBotones_RespNC" runat="server" CssClass="PanelBotones">
                                <asp:ImageButton ID="btnAceptarAgrupacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar16.png" AlternateText="Aceptar" ToolTip="Aceptar" CommandName="Update" />
                                <asp:ImageButton ID="btnRechazarAgrupacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar16.png" AlternateText="Rechazar" ToolTip="Rechazar" />
                                <act:ModalPopupExtender ID="mpe_btnRechazarAgrupacion" runat="server" TargetControlID="btnRechazarAgrupacion" PopupControlID="pnlDesaprobacionLC" CancelControlID="imgCancelarRechazoLC" BackgroundCssClass="modalBackground" />
                            </asp:Panel>
                            <asp:GridView SkinID="GridView" ID="gvAgrupacionLC" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" Caption="Factura Preliminar" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Concepto" SortExpression="Concepto">
                                        <ItemTemplate>
                                            <asp:Label ID="lblConcepto" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION" />
                                    <asp:BoundField DataField="HORAS" HeaderText="Cantidad/Horas" SortExpression="HORAS" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="IMPORTE" HeaderText="Importe" SortExpression="IMPORTE" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="Label44" runat="server" Text="Sin Datos"></asp:Label>
                                </EmptyDataTemplate>
                            </asp:GridView>

                            <asp:Panel ID="pnlDesaprobacionLC" runat="server" CssClass="modalBox" Style="display: none;">
                                <table style="border-collapse: collapse; margin: 5px;">
                                    <tr class="BarraTitulo">
                                        <th style="text-align: left">
                                            <asp:Label ID="Label101" runat="server" Text="Razones del rechazo"></asp:Label>
                                        </th>
                                        <th style="text-align: right">
                                            <asp:Panel ID="Panel2" runat="server" CssClass="PanelBotones">
                                                <asp:ImageButton ID="imgCancelarRechazoLC" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                                            </asp:Panel>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtRechazoLC" runat="server" Columns="100" Rows="10" TextMode="MultiLine" ToolTip="Razones del rechazo" />
                                                        <asp:RegularExpressionValidator ID="rev_txtRechazoLC" runat="server" ControlToValidate="txtRechazoLC" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnAceptarRechazoLC" />
                                                        <act:ValidatorCalloutExtender ID="vce_rev_txtRechazoLC" TargetControlID="rev_txtRechazoLC" runat="server" PopupPosition="TopLeft" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="BarraTitulo">
                                        <td style="text-align: center" colspan="2">
                                            <asp:Panel ID="Panel3" runat="server" CssClass="PanelBotones">
                                                <asp:ImageButton ID="btnAceptarRechazoLC" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="btnAceptarRechazoLC" CommandName="Edit" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <hr />
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        <hr />
                    </EmptyItemTemplate>
                </asp:ListView>

                <asp:Panel ID="pnlTotalCoste" runat="server" CssClass="recuadro" GroupingText="Total Coste Aceptado" Visible="false">
                    <table class="GridViewASP" style="width: 30%; margin: auto;">
                        <tr class="HeaderStyle">
                            <th>
                                <asp:Label ID="Label105" runat="server" Text="Coste"></asp:Label></th>
                        </tr>
                        <tr class="RowStyle" style="text-align: center;">
                            <td>
                                <asp:Label ID="lblTotalCoste" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </act:TabPanel>
<%--        <act:TabPanel runat="server" ID="tp_8D" HeaderText="8D">
            <ContentTemplate>
                <table class="GridViewASP">
                    <thead class="HeaderStyle">
                        <tr>
                            <th></th>
                            <th>
                                <asp:Label ID="Label29" runat="server" Text="Estado"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label23" runat="server" Text="Etapa"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label17" runat="server" Text="Fecha Inicio"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label18" runat="server" Text="Fecha Fin (Previsto)"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label19" runat="server" Text="Fecha Solicitud de Aprobacion"></asp:Label></th>
                            <th>
                                <asp:Label ID="Label28" runat="server" Text="Fecha Aprobación"></asp:Label></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="RowStyle">
                            <td style="text-align: center;">
                                <asp:Panel ID="pnlAprobacion_E14" runat="server" CssClass="PanelBotones" Visible="false">
                                    <asp:ImageButton ID="imgAprobacion_E14" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aprobar" ToolTip="Aprobar" />
                                    <asp:ImageButton ID="imgDesaprobacion_E14" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" AlternateText="Desaprobar" ToolTip="Desaprobar" />
                                    <act:ModalPopupExtender ID="mpe_pnlDescRechazo_E14" runat="server" TargetControlID="imgDesaprobacion_E14" PopupControlID="pnlDescRechazo_E14" CancelControlID="imgCancelar_E14" BackgroundCssClass="modalBackground" />
                                </asp:Panel>
                            </td>
                            <td style="text-align: center;">
                                <asp:Panel ID="pnlEstado_E14" runat="server" CssClass="PanelBotones">
                                    <asp:ImageButton ID="imgEstado_E14" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/EtapasEstado/application-edit-icon24.png" AlternateText="Solicitar Aprobacion" ToolTip="Solicitar Aprobacion" />
                                    <act:ConfirmButtonExtender ID="cbe_imgEstado_E14" runat="server" TargetControlID="imgEstado_E14" ConfirmText="Solicitar Aprobacion" Enabled="True" />
                                </asp:Panel>
                            </td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblEtapa_E14" runat="server" Text="Etapa 2-4" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFI_E14" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFF_E14" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFC_E14" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFV_E14" runat="server" Text="?" /></td>
                        </tr>
                        <tr class="RowStyle">
                            <td style="text-align: center;">
                                <asp:Panel ID="pnlAprobacion_E56" runat="server" CssClass="PanelBotones" Visible="false">
                                    <asp:ImageButton ID="imgAprobacion_E56" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aprobar" ToolTip="Aprobar" />
                                    <asp:ImageButton ID="imgDesaprobacion_E56" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" AlternateText="Desaprobar" ToolTip="Desaprobar" />
                                    <act:ModalPopupExtender ID="mpe_imgDesaprobacion_E56" runat="server" TargetControlID="imgDesaprobacion_E56" PopupControlID="pnlDescRechazo_E56" CancelControlID="imgCancelar_E56" BackgroundCssClass="modalBackground" />
                                </asp:Panel>
                            </td>
                            <td style="text-align: center;">
                                <asp:Panel ID="pnlEstado_E56" runat="server" CssClass="PanelBotones">
                                    <asp:ImageButton ID="imgEstado_E56" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/EtapasEstado/application-edit-icon24.png" AlternateText="Solicitar Aprobacion" ToolTip="Solicitar Aprobacion" />
                                    <act:ConfirmButtonExtender ID="cbe_imgEstado_E56" runat="server" TargetControlID="imgEstado_E56" ConfirmText="Solicitar Aprobacion" Enabled="True" />
                                </asp:Panel>
                            </td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblEtapa_E56" runat="server" Text="Etapa 5-6" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFI_E56" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFF_E56" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFC_E56" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFV_E56" runat="server" Text="?" /></td>
                        </tr>
                        <tr class="RowStyle">
                            <td style="text-align: center;">
                                <asp:Panel ID="pnlAprobacion_E78" runat="server" CssClass="PanelBotones" Visible="false">
                                    <asp:ImageButton ID="imgAprobacion_E78" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aprobar" ToolTip="Aprobar" />
                                    <asp:ImageButton ID="imgDesaprobacion_E78" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" AlternateText="Desaprobar" ToolTip="Desaprobar" />
                                    <act:ModalPopupExtender ID="mpe_imgDesaprobacion_E78" runat="server" TargetControlID="imgDesaprobacion_E78" PopupControlID="pnlDescRechazo_E78" CancelControlID="imgCancelar_E78" BackgroundCssClass="modalBackground" />
                                </asp:Panel>
                            </td>
                            <td style="text-align: center;">
                                <asp:Panel ID="pnlEstado_E78" runat="server" CssClass="PanelBotones">
                                    <asp:ImageButton ID="imgEstado_E78" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/EtapasEstado/application-edit-icon24.png" AlternateText="Solicitar Aprobacion" ToolTip="Solicitar Aprobacion" />
                                    <act:ConfirmButtonExtender ID="cbe_imgEstado_E78" runat="server" TargetControlID="imgEstado_E78" ConfirmText="Solicitar Aprobacion" Enabled="True" />
                                </asp:Panel>
                            </td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblEtapa_E78" runat="server" Text="Etapa 7-8" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFI_E78" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFF_E78" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFC_E78" runat="server" Text="?" /></td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblFV_E78" runat="server" Text="?" /></td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </act:TabPanel>--%>
    </act:TabContainer>

    <%--
		"RoundedCornersExtender" no funciona en IE8 si se combina con "ModalPopupExtender"
		<act:RoundedCornersExtender ID="rce_pnlDescRechazo_E14" runat="server" TargetControlID="pnlDescRechazo_E14" Radius="20"></act:RoundedCornersExtender>--%>
    <asp:Panel ID="pnlDescRechazo_E14" runat="server" CssClass="modalBox" Style="display: none;">
        <table style="border-collapse: collapse; margin: 5px;">
            <tr class="BarraTitulo">
                <th style="text-align: left">
                    <asp:Label ID="Label35" runat="server" Text="Razones del rechazo"></asp:Label>
                </th>
                <th style="text-align: right">
                    <asp:Panel ID="pnlBotonesCabecera" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgCancelar_E14" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                    </asp:Panel>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="imgMarvin" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
                            <td>
                                <asp:TextBox ID="txtObvRechazo_E14" runat="server" Columns="100" Rows="10" TextMode="MultiLine" ToolTip="Razones del rechazo"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="rev_txtObvRechazo_E14" runat="server" ControlToValidate="txtObvRechazo_E14" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="imgAceptarRechazo_E14" />
                                <act:ValidatorCalloutExtender ID="vce_rev_txtObvRechazo_E14" TargetControlID="rev_txtObvRechazo_E14" runat="server" PopupPosition="TopLeft" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="BarraTitulo">
                <td style="text-align: center" colspan="2">
                    <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgAceptarRechazo_E14" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptarRechazo_E14" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--
		"RoundedCornersExtender" no funciona en IE8 si se combina con "ModalPopupExtender"
		<act:RoundedCornersExtender ID="rce_pnlDescRechazo_E56" runat="server" TargetControlID="pnlDescRechazo_E56" Radius="20"></act:RoundedCornersExtender>--%>
    <asp:Panel ID="pnlDescRechazo_E56" runat="server" CssClass="modalBox" Style="display: none;">
        <table style="border-collapse: collapse; margin: 5px;">
            <tr class="BarraTitulo">
                <th style="text-align: left">
                    <asp:Label ID="Label36" runat="server" Text="Razones del rechazo"></asp:Label>
                </th>
                <th style="text-align: right">
                    <asp:Panel ID="Panel5" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgCancelar_E56" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                    </asp:Panel>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
                            <td>
                                <asp:TextBox ID="txtObvRechazo_E56" runat="server" Columns="100" Rows="10" TextMode="MultiLine" ToolTip="Razones del rechazo"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="rev_txtObvRechazo_E56" runat="server" ControlToValidate="txtObvRechazo_E56" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="imgAceptarRechazo_E56" />
                                <act:ValidatorCalloutExtender ID="vce_rev_txtObvRechazo_E56" TargetControlID="rev_txtObvRechazo_E56" runat="server" PopupPosition="TopLeft" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="BarraTitulo">
                <td style="text-align: center" colspan="2">
                    <asp:Panel ID="Panel6" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgAceptarRechazo_E56" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptarRechazo_E56" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--
		"RoundedCornersExtender" no funciona en IE8 si se combina con "ModalPopupExtender"
		<act:RoundedCornersExtender ID="rce_pnlDescRechazo_E78" runat="server" TargetControlID="pnlDescRechazo_E78" Radius="20"></act:RoundedCornersExtender>--%>
    <asp:Panel ID="pnlDescRechazo_E78" runat="server" CssClass="modalBox" Style="display: none;">
        <table style="border-collapse: collapse; margin: 5px;">
            <tr class="BarraTitulo">
                <th style="text-align: left">
                    <asp:Label ID="Label37" runat="server" Text="Razones del rechazo"></asp:Label>
                </th>
                <th style="text-align: right">
                    <asp:Panel ID="Panel7" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgCancelar_E78" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                    </asp:Panel>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
                            <td>
                                <asp:TextBox ID="txtObvRechazo_E78" runat="server" Columns="100" Rows="10" TextMode="MultiLine" ToolTip="Razones del rechazo"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="rev_txtObvRechazo_E78" runat="server" ControlToValidate="txtObvRechazo_E78" ErrorMessage="Solo 1000 Caracteres" ValidationExpression="^[\s\S]{0,1000}$" Display="None" SetFocusOnError="true" ValidationGroup="imgAceptarRechazo_E78" />
                                <act:ValidatorCalloutExtender ID="vce_rev_txtObvRechazo_E78" TargetControlID="rev_txtObvRechazo_E78" runat="server" PopupPosition="TopLeft" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="BarraTitulo">
                <td style="text-align: center" colspan="2">
                    <asp:Panel ID="Panel8" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgAceptarRechazo_E78" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptarRechazo_E78" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>

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

    <div id="modalCognos" runat="server" class="modal" style="display:none;position:fixed;z-index:1;left:0;top:0;width:100%;height:100%;overflow:auto;background-color: rgb(0,0,0);background-color: rgba(0,0,0,0.4)">
      <!-- Modal content -->
      <div class="modal-content" style="background-color:white;padding:20px;display:inline-block;position:absolute;transform:translate(-50%,-50%);top:50%;left:50%;">
        <%--<span class="close" style="color:#f00;float:right;font-weight:bold">&times;</span>--%>
        <center><p style="font-variant:small-caps;font-weight:700">Choose one:</p></center>
          <label>
              <input type="radio" value="Informe con información de costes" runat="server" id="conCostes"/>
              Informe con información de costes
          </label>
          <br />
          <label>
              <input type="radio" value="Informe sin información de costes" runat="server" id="sinCostes"/>
              Informe sin información de costes
          </label>
          <br />
          <br />
          <center>
              <input type="button" value="Aceptar" runat="server" id="btnAceptarCognos" />
            <input type="button" value="Cancelar" runat="server" id="btnCancelarCognos" />
          </center>
       </div>
    </div>
    <div id="dialog" class="dialog"></div>
    <%--<asp:Textbox ID="hfRazonRechazoCierre" runat="server" OnTextChanged="EnviarEmailRechazo" AutoPostBack="true"/>--%>
    <asp:Textbox ID="hfRazonRechazoCierre" runat="server" AutoPostBack="true" style="visibility:hidden"/>

    <script type="text/javascript">
<%--        document.addEventListener("DOMContentLoaded", function (event) {
            var modal = document.getElementById("modalCognos");
            alert(modal);
            var btn = document.getElementById("<%=btnImprimir8D.ClientID%>");
            alert(btn);
            //var btn2 = document.getElementById("<%=pnlBotonesDetalle.ClientID%>").getElementsByClassName("btnImprimir8D")
            //btn.onclick = function () {
            //    modal.style.display = "block";
            //}
        });--%>

        function rechazarCierre() {
            //var dialog = $('<textarea name="razonRechazoCierre" rows="5" style="width:270px!important;margin:5px auto;border:1px solid lightgrey" autofocus></textarea>').dialog({ // jquery ui dialog
            //    closeOnEscape: false,
            //    title: "Razón del rechazo",
            //    buttons: {
            //        "OK": function () {
            //            debugger;
            //            var razon = $('textarea[name="razonRechazoCierre"]').val();                        
            //            saveData(razon);
            //            dialog.dialog('close');
            //            //var cero = < %=EnviarEmailRechazo()%>
            //        },
            //        "Cancel": function () {
            //            $('#CREARENGTK').val(false);
            //            dialog.dialog('close');
            //            //e.preventDefault();
            //        }
            //    },
            //    modal: true,
            //    resizable: false,
            //    draggable: false
            //});
            $(".dialog").html("");
            $(".dialog").append('<textarea name="razonRechazoCierre" rows="5" style="width:270px!important;margin:5px auto;border:1px solid lightgrey" autofocus></textarea>');
            var dialog = $(".dialog").dialog({ // jquery ui dialog
                closeOnEscape: false,
                title: "Razón del rechazo",
                buttons: {
                    "OK": function () {
                        var razon = $('textarea[name="razonRechazoCierre"]').val();            
                        if (!$.trim(razon)) {
                            alert("You have to specify a reason!");
                        } else {
                            saveData(razon);
                            dialog.dialog('close');
                            //var cero = < %=EnviarEmailRechazo()%>
                        }
                    },
                    "Cancel": function () {
                        $('#CREARENGTK').val(false);
                        dialog.dialog('close');
                        //e.preventDefault();
                    }
                },
                modal: true,
                resizable: false,
                draggable: false
            });
        }

        function saveData(razon) {
            var hf = $('#ContentPlaceHolder_FORM_hfRazonRechazoCierre');
            //hf.value = razon;
            //hf.trigger('change');
            hf.val(razon).change();

        }

    </script>
    <%--<asp:HiddenField ID="hf_pnlMensaje_LA" runat="server" />
    <act:ModalPopupExtender ID="mpe_pnlMensaje_LA" runat="server" TargetControlID="hf_pnlMensaje_LA" PopupControlID="pnlMensaje_LA" CancelControlID="imgCancelar_LC" BackgroundCssClass="modalBackground" />--%>
</asp:Content>
