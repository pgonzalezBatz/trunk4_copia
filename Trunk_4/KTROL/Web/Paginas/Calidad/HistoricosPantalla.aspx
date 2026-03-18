<%@ Page Language="vb" AutoEventWireup="true" MasterPageFile="~/MPWeb.Master" CodeBehind="HistoricosPantalla.aspx.vb" Inherits="WebRaiz.HistoricosPantalla" EnableEventValidation="false" %>
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
   
   <Titulo:Titulo ID="titHistorico" Texto="Controles históricos por pantalla" runat="server"  />
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
                            <td class="definicion15">
                                <asp:Label ID="lblIdentificador" runat="server" Text="Identificador" />
                            </td>
                            <td class="campoTextoNormal">
                                <asp:TextBox ID="txtIdentificador" runat="server" Font-Size="18px" class="datos-numericos"/>
                            </td>
                            <td class="definicion15">
                                 <asp:Label ID="lblCaracteristica" runat="server" Text="Característica" />
                            </td>
                            <td class="campoTextoNormal width10">
                                <asp:TextBox ID="txtCaracteristica" runat="server"  MaxLength="2" Font-Size="18px" />
                            </td>                                                        
                            <td class="definicion15">
                                <asp:Label ID="lblVerificador1" runat="server" Text="Verificador 1" />
                            </td>
                            <td class="campoTextoNormal" colspan="3">
                                <div style="float: left; width: 80%">
                                    <asp:TextBox ID="txtVerificador1" runat="server" width="95%" MaxLength="50" Font-Size="18px"></asp:TextBox>                        
                                </div>  
                                <div id="imgSeleccion1" class="imagen-no-seleccionado" style="float:left; margin-left: 2px" runat="server"></div>                   
                                <div class="clear-float"></div> 
                                <div id="helper1" style="margin-top: -1px;" runat="server" /> 
                                <asp:HiddenField runat="server" ID="hfIdUsuario1" />                                                                               
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
                                <act:CalendarExtender ID="imgCalendarioDesde_CalendarExtender" runat="server" TargetControlID="txtFechaDesde" PopupButtonID="imgCalendarioDesde" /><!-- Format="yyyy/MM/dd" -->
                                <asp:CompareValidator ID="cvFechaDesde" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaDesde" Type="Date" Operator="DataTypeCheck" Display="None" />
		                        <act:ValidatorCalloutExtender ID="vce_cvFechaDesde" runat="server" TargetControlID="cvFechaDesde" />
                            </td>
                            <td class="definicion15">
                                <asp:Label ID="lblVerificador2" runat="server" Text="Verificador 2" />
                            </td>
                            <td class="campoTextoNormal" colspan="3">
                                <div style="float: left; width: 80%">
                                    <asp:TextBox ID="txtVerificador2" runat="server" width="95%" MaxLength="50" Font-Size="18px"></asp:TextBox>                        
                                </div>  
                                <div id="imgSeleccion2" class="imagen-no-seleccionado" style="float:left; margin-left: 2px" runat="server"></div>                   
                                <div class="clear-float"></div> 
                                <div id="helper2" style="margin-top: -1px;" runat="server" /> 
                                <asp:HiddenField runat="server" ID="hfIdUsuario2" />
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
                                <act:CalendarExtender ID="imgCalendarioHasta_CalendarExtender" runat="server" TargetControlID="txtFechaHasta" PopupButtonID="imgCalendarioHasta" /><!-- Format="yyyy/MM/dd" -->
                                
                                <asp:CompareValidator ID="cv_txtFechaHasta" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaHasta" Type="Date" Operator="DataTypeCheck" Display="None" />
		                        <act:ValidatorCalloutExtender ID="vce_cv_txtFechaHasta" runat="server" TargetControlID="cv_txtFechaHasta" />

		                        <asp:CompareValidator ID="cv_txtFechaFin2" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaHasta" Operator="GreaterThanEqual" ControlToCompare="txtFechaDesde" Type="Date" />
		                        <act:ValidatorCalloutExtender ID="vceComparadorFehas" runat="server" TargetControlID="cv_txtFechaFin2" />
                            </td>
                            <td class="definicion15">
                                <asp:Label ID="lblVerificador3" runat="server" Text="Verificador 3" />
                            </td>
                            <td class="campoTextoNormal" colspan="3">
                               <div style="float: left; width: 80%">
                                    <asp:TextBox ID="txtVerificador3" runat="server" width="95%" MaxLength="50" Font-Size="18px"></asp:TextBox>                        
                                </div>  
                                <div id="imgSeleccion3" class="imagen-no-seleccionado" style="float:left; margin-left: 2px" runat="server"></div>                   
                                <div class="clear-float"></div> 
                                <div id="helper3" style="margin-top: -1px;" runat="server" /> 
                                <asp:HiddenField runat="server" ID="hfIdUsuario3" />
                            </td>                                                      
                        </tr>
                        <tr>        
                            <td colspan="4" align="left">
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Font-Size="18px" CssClass="botonHistoricos" OnClick="btnLimpiar_Click" CausesValidation="false" />
                            </td>                         
                            <td colspan="4" align="right">
                                <asp:Button ID="btnFiltrar" runat="server" Text="Buscar" Font-Size="18px" CssClass="botonHistoricos" OnClick="btnFiltrar_Click" />               
                            </td>                                                                  
                        </tr>
                    </table>                                   
                </asp:Panel>                                              
                <br /><br />                                                       
                <asp:GridView ID="gvControles" runat="server" DataKeyNames="Id"  Width="100%" 
                    AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None"  OnRowDataBound="gvControles_RowDataBound">
                    <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                    <FooterStyle CssClass="FooterStyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                    <HeaderStyle CssClass="HeaderStyle" />
                    <EditRowStyle CssClass="EditRowStyle" />
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <EmptyDataTemplate>
                        <div style="text-align:center">
                            <asp:Label ID="lblSinRegistros" runat="server" Text="No hay registros que cumplan con los requisitos marcados" cssclass="negrita" />
                        </div>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIdControl" runat="server" Text='<%# Eval("Id")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIdRegistro" runat="server" Text='<%# Eval("IdRegistro")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOkNok" runat="server" Text='<%# Eval("OkNok")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>                           
                        <asp:BoundField DataField="FechaCorta" HeaderText="Fecha" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="14px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-HorizontalAlign="Center" />  
                        <asp:BoundField DataField="CodOperacion" HeaderText="Cod.Op" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Cod. Usuario">
                            <ItemTemplate>
                                <asp:Label id="lblCodUsuario" runat="server" Text='<%# If(Eval("CodUsuario") = Integer.MinValue, "", Eval("CodUsuario"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>                              
                        <asp:BoundField DataField="InfoPieza" HeaderText="Info" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Valor">
                            <ItemTemplate>
                                <asp:Image ID="imgCaracteristicaValor" runat="server" Width="24" Height="24" />
                                <asp:Label ID="lblCaracteristicaValor" runat="server" Text='<%# Eval("Valor")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fichero">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlDescargar" runat="server" NavigateUrl="../DescargarArchivo.aspx?idControl={0}&idRegistro={1}" >
                                    <asp:Image ID="imgDescargar" runat="server" AlternateText="Descargar fichero" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Descargar.png" />
                                </asp:HyperLink>                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnVerDatos" runat="server" Text="Ver" CommandArgument='<%# Eval("Id")%>' OnClick="btnVerDatos_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>                           
                    </Columns>
                </asp:GridView>                                         
           </ContentTemplate>           
       </asp:UpdatePanel>
       
       <asp:Panel ID="pnlPopUp" runat="Server" BackgroundCssClass="modalBox" Width="90%">           
           <asp:UpdatePanel ID="upPopUp" runat="server" UpdateMode="Conditional">
               <ContentTemplate>                   
                   <asp:Panel ID="pnlErrores" runat="server" BackColor="#ffffff" HorizontalAlign="Center">    
                       <div style="height:20px"></div>                   
                       <asp:DetailsView ID="dvErrores" runat="server" AllowPaging="false" DefaultMode="ReadOnly"
                            AutoGenerateRows="false" GridLines="None" Width="60%" CssClass="DetailsView">
                            <HeaderStyle CssClass="HeaderStyleDetailsView" />
                            <FieldHeaderStyle CssClass="FieldHeaderStyle" Width="20%" />
                            <RowStyle CssClass="RowStyleDetailsView" HorizontalAlign="Left" />
                            <AlternatingRowStyle CssClass="AlternatingRowStyleDetailsView" />
                            <CommandRowStyle CssClass="CommandRowStyleDetailsView" />
                            <HeaderTemplate>
                                <asp:Label ID="lblSinDatos" runat="server" Text="Error en el control" CssClass="negrita" />
                            </HeaderTemplate>
                            <Fields>         
                                <asp:TemplateField HeaderText="Validación">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkValidacion" runat="server" Checked='<%# Eval("Validado")%>' Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>                      
                                <asp:TemplateField HeaderText="Reparación">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkReparacion" runat="server" Checked='<%# Eval("Reparado")%>' Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>                               
                                <asp:TemplateField HeaderText="Cambio Ref">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCambioReferencia" runat="server" Checked='<%# Eval("CambioReferencia")%>' Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Validado Por">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNombreValidadoPor" runat="server" Text='<%# Eval("NombreValidacionUsuario")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comentario">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComentario" runat="server" TextMode="MultiLine" Text='<%# Eval("Comentario")%>' Enabled="false" Width="98%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Control validador">
                                    <ItemTemplate>
                                        <asp:Label ID="lblControlValidador" runat="server" Text='<%# Eval("IdControlValidacion")%>' Visible='<%# Eval("IdControlValidacion") <> Integer.minvalue %>' Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>           
                            </Fields>
                        </asp:DetailsView>                                              
                   </asp:Panel>
                   <asp:Panel ID="pnlDatos" runat="server" BackColor="#ffffff" HorizontalAlign="Center">
                       <div style="height:20px"></div>
                       <div style="max-height:500px; overflow-y:auto">
                           <asp:Gridview ID="gvControlesValores" runat="server" AllowPaging="False" 
                                AutoGenerateColumns="false" GridLines="None" Width="90%" CssClass="GridViewASP"
                                DataKeyNames="IdRegistro" OnRowDataBound="gvControlesValores_RowDataBound">
                                <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                                <FooterStyle CssClass="FooterStyle" />
                                <PagerStyle CssClass="PagerStyle" />
                                <SelectedRowStyle CssClass="SelectedRowStyle" />
                                <HeaderStyle CssClass="HeaderStyle" Font-Size="10px" />
                                <EditRowStyle CssClass="EditRowStyle" />
                                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblSinRegistros" runat="server" Text="No tiene características"  Font-Italic="true" Font-Size="10px" />
                                </EmptyDataTemplate>
                                <Columns>     
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIdControl" runat="server" Text='<%# Eval("IdControl")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                    
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIdRegistro" runat="server" Text='<%# Eval("IdRegistro")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOkNok" runat="server" Text='<%# Eval("OkNok")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Posición">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPosicion" runat="server" Text='<%# Eval("Posicion")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Característica">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParametro" runat="server" Text='<%# Eval("CaracParam")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                
                                    <asp:TemplateField HeaderText="Valor">
                                        <ItemTemplate>
                                            <asp:Image ID="imgCaracteristicaValor" runat="server" Width="24" Height="24" />
                                            <asp:Label ID="lblCaracteristicaValor" runat="server" Text='<%# Eval("Valor")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlDescargar" runat="server" NavigateUrl="../DescargarArchivo.aspx?idControl={0}&idRegistro={1}" >
                                                <asp:Image ID="imgDescargarDetalle" runat="server" ToolTip="Descargar fichero" AlternateText="Descargar fich." ImageUrl="~/App_Themes/Tema1/IconosAcciones/Descargar.png" /><%-- OnClick="imgDescargarDetalle_Click" --%>
                                            </asp:HyperLink>                
                                           </ItemTemplate>
                                    </asp:TemplateField>                                                                                                                                                                                                                   
                                </Columns>
                            </asp:Gridview> 
                        </div> 
                        <div style="width:100%; text-align:center; vertical-align:middle; padding-bottom:20px; padding-top:20px">                            
                            <asp:Button ID="btnOk" runat="server" Text="Cerrar ventana" font-size="16px" OnClientClick=" return cerrarModal();"  />   
                        </div>                               
                    </asp:Panel>       
               </ContentTemplate>
           </asp:UpdatePanel>            
        </asp:Panel>
        
       <asp:Button runat="server" ID="btnModalPopUp" style="display:none"/>         
        <act:ModalPopupExtender ID="mpeValores" runat="server"
                TargetControlID="btnModalPopUp" BehaviorID="modalPopup"
                PopupControlID="pnlPopUp"
                BackgroundCssClass="modalBackground">
        </act:ModalPopupExtender>
   </asp:Panel>
                
    <div id="modal_Campos" title="Error" style="display: none; text-align:center">
            <asp:Label ID="lblCamposVacios" runat="server" Text="Todos los campos no pueden estar vacíos" />
        </div>
     
    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>

