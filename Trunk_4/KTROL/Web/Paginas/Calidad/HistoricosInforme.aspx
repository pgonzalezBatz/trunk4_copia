<%@ Page Language="vb" AutoEventWireup="true" MasterPageFile="~/MPWeb.Master" CodeBehind="HistoricosInforme.aspx.vb" Inherits="WebRaiz.HistoricosInforme" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>
<%@ Register Src="~/Controles/SelectorOperacion_Historial.ascx" TagPrefix="uc1" TagName="SelectorOperacion" %>
<%@ Register Src="~/Controles/SelectorReferencia_Historial.ascx" TagPrefix="uc1" TagName="SelectorReferencia" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
   
    .FondoAplicacion
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <!--js de Jquery-->    
    <script type="text/javascript" src="../../js/jQuery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery-ui.js"></script>
    <link type="text/css" href="../../App_Themes/Tema1/jquery-ui.css" rel="stylesheet" />
    <link type="text/css" href="../../App_Themes/Tema1/style.css" rel="stylesheet" />

    <!-- Js de usuarios-->
    <script src="../../js/jQuery/usuarios.js" type="text/javascript"></script>

    <!-- Js de numeric -->
    <script type="text/javascript" src="../../js/jQuery/jquery.numeric.js"></script>

    <script type="text/javascript">
        $(function () {     
            $('.datos-numericos').numeric({ negative: false });
        });

        function cerrarModal() {
            var modal = $find('modalPopup');
            modal.hide();
            return false;
        }
   </script>
   
   <Titulo:Titulo ID="titHistorico" Texto="Controles históricos por informe" runat="server"  />
   <br />   
   <asp:Panel ID="pnlControles" runat="server">
       <asp:UpdatePanel ID="upControles" runat="server" UpdateMode="Conditional">
           <ContentTemplate>
                <asp:Label ID="Label1" runat="server" />
                <asp:Panel ID="pnlCabeceraFiltradoCaracteristicas" runat="server" CssClass="cpHeader" >
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="imgCollapseFiltradoCaracteristicas" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg" />
                            </td>
                            <td>
                                <asp:Label ID="lblFiltradoCaracteristicas" runat="server" Text="Búsqueda" Font-Bold="true" ForeColor="Black" />
                            </td>
                        </tr>
                    </table>                    
                </asp:Panel>                

                <act:CollapsiblePanelExtender id="cpeDatosIncidencia" runat="server"  
                    TargetControlID="pnlFiltradoCaracteristicas" 
                    CollapseControlID="pnlCabeceraFiltradoCaracteristicas" 
                    ExpandControlID="pnlCabeceraFiltradoCaracteristicas" 
                    Collapsed="false" 
                    CollapsedSize="0" 
                    AutoCollapse="False"
                    AutoExpand="False"
                    ExpandDirection="Vertical"
                    ImageControlID="imgCollapseFiltradoCaracteristicas"
                    TextLabelID="Label1"
                    ScrollContents="false"
                    CollapsedImage="~/App_Themes/Tema1/IconosAjaxControl/CollapsiblePanelExtender/expand_blue.jpg"
                    ExpandedImage="~/App_Themes/Tema1/IconosAjaxControl/CollapsiblePanelExtender/collapse_blue.jpg">
                </act:CollapsiblePanelExtender>

                <!--Nombre de usuario -->
                <asp:Panel ID="pnlFiltradoCaracteristicas" runat="server" CssClass="cpBody">
                    <table width="100%" cellpadding="3">
                        <tr>
                            <td class="definicion15">
                                <asp:Label ID="lblReferencia" runat="server" Text="Referencia" />
                            </td>
                            <td colspan="3" class="campoTextoNormal" valign="middle">
                                <uc1:SelectorReferencia ID="selReferencia" runat="server" />
                            </td>                            
                            <td class="definicion15">
                                <asp:Label ID="lblOperacion" runat="server" Text="Operación" />
                            </td>
                            <td colspan="3" class="campoTextoNormal" valign="middle">
                                <uc1:SelectorOperacion ID="selOperacion" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="definicion15" rowspan="2">
                                <asp:Label ID="lblFecha" runat="server" Text="Fecha" />
                            </td>
                            <td class="definicion">
                                <asp:Label ID="lblFechaDesde" runat="server" Text="Desde" AssociatedControlID="imgCalendarioDesde" />
                            </td>
                            <td class="campoTextoNormal width10">
                                <asp:TextBox ID="txtFechaDesde" runat="server" Width="90%" Font-Size="18px" />
                            </td>
                            <td class="campoTextoNormal width10" align="left">
                                <asp:ImageButton ID="imgCalendarioDesde" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />                                    
                                <act:CalendarExtender ID="imgCalendarioDesde_CalendarExtender" runat="server" TargetControlID="txtFechaDesde" PopupButtonID="imgCalendarioDesde" Format="yyyy/MM/dd" />
                                <asp:CompareValidator ID="cvFechaDesde" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaDesde" Type="Date" Operator="DataTypeCheck" Display="None" />
		                        <act:ValidatorCalloutExtender ID="vce_cvFechaDesde" runat="server" TargetControlID="cvFechaDesde" />
                            </td>
                            <td class="definicion15">
                                <asp:Label ID="lblIdentificador" runat="server" Text="Identificador" />
                            </td>
                            <td class="campoTextoNormal" colspan="3">
                                <asp:TextBox ID="txtIdentificador" runat="server" Width="30%" MaxLength="6" Font-Size="18px" class="datos-numericos"/>
                            </td>                          
                        </tr>                            
                        <tr>                               
                            <td class="definicion">
                                <asp:Label ID="lblFechaHasta" runat="server" Text="Hasta" />
                            </td>
                            <td class="campoTextoNormal">
                                <asp:TextBox ID="txtFechaHasta" runat="server" Width="90%" Font-Size="18px" />
                            </td>
                            <td class="campoTextoNormal" align="left">
                                <asp:ImageButton ID="imgCalendarioHasta" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />                                    
                                <act:CalendarExtender ID="imgCalendarioHasta_CalendarExtender" runat="server" TargetControlID="txtFechaHasta" PopupButtonID="imgCalendarioHasta" Format="yyyy/MM/dd" />
                                
                                <asp:CompareValidator ID="cv_txtFechaHasta" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaHasta" Type="Date" Operator="DataTypeCheck" Display="None" />
		                        <act:ValidatorCalloutExtender ID="vce_cv_txtFechaHasta" runat="server" TargetControlID="cv_txtFechaHasta" />
                                
                                <!-- Validacion de txtFechaFin, FechaDesde <= FechaFin ------------------------------------------------------------------------------------>
		                        <asp:CompareValidator ID="cv_txtFechaFin2" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaHasta" Operator="GreaterThanEqual" ControlToCompare="txtFechaDesde" Type="Date" />
		                        <act:ValidatorCalloutExtender ID="vceComparadorFehas" runat="server" TargetControlID="cv_txtFechaFin2" />
                            </td>
                            <td colspan="4" class="campoTextoNormal">
                                &nbsp
                            </td>                                                      
                        </tr>
                        <tr>        
                            <td colspan="8" align="left">
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="botonHistoricos" UseSubmitBehavior="false" OnClick="btnLimpiar_Click" />
                            </td>                                                                                               
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnGenerarHojaRegistros" runat="server" Text="Hoja de registros" CssClass="botonHistoricos" Font-Size="18px" OnClick="btnGenerarInforme_Click" CausesValidation="false" />
                            </td>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnGenerarEstadisticas" runat="server" Text="Estadísticas" CssClass="botonHistoricos" Font-Size="18px" OnClick="btnGenerarEstadisticas_Click" />                                   
                            </td>
                        </tr>
                    </table>                                   
                </asp:Panel>                
               <div style="height:20px"></div>
           </ContentTemplate>           
       </asp:UpdatePanel>      
   </asp:Panel>
                
    <div id="modal_Campos" title="Error" style="display: none; text-align:center">
        <asp:Label ID="lblCamposVacios" runat="server" Text="Todos los campos no pueden estar vacíos" />
    </div>
     
    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>

