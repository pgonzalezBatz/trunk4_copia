<%@ Page Language="vb" AutoEventWireup="true" MasterPageFile="~/MPWeb.Master" CodeBehind="Historicos.aspx.vb" Inherits="WebRaiz.Historicos" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>
<%@ Register Src="~/Controles/SelectorOperacion_Historial.ascx" TagPrefix="uc1" TagName="SelectorOperacion" %>

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

            $("[src*=plus]").live("click", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../../App_Themes/Tema1/Imagenes/minus.png");
            });
            $("[src*=minus]").live("click", function () {
                $(this).attr("src", "../../App_Themes/Tema1/Imagenes/plus.png");
                $(this).closest("tr").next().remove();
            });

            $('.datos-numericos').numeric({ negative: false });
        });

        function cerrarModal() {
            var modal = $find('modalPopup');
            modal.hide();
            return false;
        }

        function ComprobarCampos() {
            var txtOperacion = $("[id$='txtSelectorOperacion']").attr('id');
            var operacion = $('#' + txtOperacion).val();

            var txtCaracteristica = $("[id$='txtCaracteristica']").attr('id');
            var caracteristica = $('#' + txtCaracteristica).val();
            
            var txtFecha = $("[id$='txtFecha']").attr('id');
            var fecha = $('#' + txtFecha).val();

            var txtFechaFin = $("[id$='txtFechaFin']").attr('id');
            var fechaFin = $('#' + txtFechaFin).val();
            
            var txtVerificador1 = $("[id$='txtVerificador1']").attr('id');
            var verificador1 = $('#' + txtVerificador1).val();
            var txtVerificador2 = $("[id$='txtVerificador2']").attr('id');
            var verificador2 = $('#' + txtVerificador2).val();            
            var txtVerificador3 = $("[id$='txtVerificador3']").attr('id');
            var verificador3 = $('#' + txtVerificador3).val();

            if (operacion.length == 0 & caracteristica.length == 0 & fecha.length == 0 & fechaFin.length == 0 & verificador1.length == 0 & verificador2.length == 0 & verificador3.length == 0) {
                $("#modal_Campos").dialog({
                    resizable: false,
                    draggable: false,
                    height: 150,
                    width: 600,
                    buttons: {
                        OK: function () {
                            $(this).dialog("close");
                        }
                    },
                    modal: true
                });
                return false;
            }
            else {
                return true;
            }
        }
   </script>
   
   <Titulo:Titulo ID="titHistorico" Texto="Controles" runat="server"  />
   <br />   
   <asp:Panel ID="pnlControles" runat="server">
       <asp:UpdatePanel ID="upControles" runat="server" UpdateMode="Conditional">
           <ContentTemplate>
               <div style="float:left; width:10%">
                   <asp:Label ID="lblCodOperacion" runat="server" Text="Operacion" CssClass="negrita"></asp:Label>:
               </div>
               <div style="float:left; width:35%">
                   <asp:TextBox ID="txtSelectorOperacion" runat="server" Font-Size="18px" Text="8101977" MaxLength="8" CssClass="datos-numericos" />
               </div>
               <div style="float:left; width:1%"></div>
               <div style="float:left; width:10%">
                    <asp:Label ID="lblVerificador1" runat="server" Text="Verif. 1" CssClass="negrita"></asp:Label>:
               </div>
               <div style="float:left; width:25%">                    
                    <asp:TextBox ID="txtVerificador1" runat="server" Width="95%" MaxLength="50"></asp:TextBox>                        
               </div>  
               <div id="imgSeleccion1" class="imagen-no-seleccionado" style="float:left; width:5%" runat="server">                   
                    <asp:HiddenField runat="server" ID="hfIdUsuario1" />
                    <div id="helper1" style="margin-top: -1px;" runat="server">
                    </div>  
               </div>
                                          

               <div class="clear-float"></div>
               <div style="height:20px"></div>

               <div style="float:left; width:10%">
                    <asp:Label ID="lblFechaDesde" runat="server" Text="Fecha" CssClass="negrita"></asp:Label>:
                </div>
                <div style="float:left; width:35%">
                    <!-- Fecha desde -->
                    <asp:TextBox ID="txtFecha" runat="server" width="30%" Font-Size="18px"></asp:TextBox>&nbsp;
                    <asp:ImageButton ID="imgCalendario" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
                    
		            <act:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" TargetControlID="txtFecha" />
		            <act:CalendarExtender ID="imgCalendario_CalendarExtender" runat="server" TargetControlID="txtFecha" PopupButtonID="imgCalendario" />
		            
		            <asp:CompareValidator ID="cvFecha" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFecha" Type="Date" Operator="DataTypeCheck" Display="None" />
		            <act:ValidatorCalloutExtender ID="vce_cvFecha" runat="server" TargetControlID="cvFecha" />
                    --
                    <!-- Fecha hasta -->
                    <asp:TextBox ID="txtFechaFin" runat="server" width="30%" Font-Size="18px"></asp:TextBox>
                    <asp:ImageButton ID="imgCalendarioFechaFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
                    <act:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" TargetControlID="txtFechaFin" />
                    
		            <act:CalendarExtender ID="imgCalendario_txtFechaFin_CalendarExtender" runat="server" TargetControlID="txtFechaFin" PopupButtonID="imgCalendarioFechaFin" />
		            
		            <asp:CompareValidator ID="cv_txtFechaFin" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Type="Date" Operator="DataTypeCheck" Display="None" />
		            <act:ValidatorCalloutExtender ID="vce_cv_txtFechaFin" runat="server" TargetControlID="cv_txtFechaFin" />
		            
		            <asp:CompareValidator ID="cv_txtFechaFin2" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Operator="GreaterThanEqual" ControlToCompare="txtFecha" Type="Date" />
		            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="cv_txtFechaFin2" />
                </div>
                <div style="float:left; width:1%"></div>
                <div style="float:left; width:10%">
                    <asp:Label ID="lblVerificador2" runat="server" Text="Verif. 2" CssClass="negrita"></asp:Label>:
                </div>
                <div style="float:left; width:25%">
                    <asp:TextBox ID="txtVerificador2" runat="server" width="95%" />                                   
                </div>
               <div id="imgSeleccion2" runat="server" class="imagen-no-seleccionado" style="float:left; width:5%">                    
                    <asp:HiddenField runat="server" ID="hfIdUsuario2" />
                    <div id="helper2" style="margin-top: -1px;" runat="server">
                    </div>
               </div>

               <div class="clear-float"></div>
               <div style="height:20px"></div>

               <div style="float:left; width:10%">
                   <asp:Label ID="lblCaracteristica" runat="server" Text="Carac" CssClass="negrita"></asp:Label>:                   
               </div>
               <div style="float:left; width:35%;">
                   <asp:TextBox ID="txtCaracteristica" runat="server" Font-Size="18px" MaxLength="2" />                                  
               </div>
               <div style="float:left; width:1%"></div>
               <div style="float:left; width:10%">
                   <asp:Label ID="lblVerificador3" runat="server" Text="Verif. 3" CssClass="negrita"></asp:Label>:
               </div>
               <div style="float:left; width:25%;">
                   <asp:TextBox ID="txtVerificador3" runat="server" width="95%" />
               </div>
               <div id="imgSeleccion3" runat="server" class="imagen-no-seleccionado" style="float:left; width:5%">
                    <asp:HiddenField runat="server" ID="hfIdUsuario3" />
                    <div id="helper3" style="margin-top: -1px;" runat="server">
                    </div>
               </div>
              
               <div class="clear-float"></div>
               <div style="height:20px"></div> 
                                           
               <div style="float:left; width:100%; text-align:center">
                   <asp:Button ID="btnFiltrar" runat="server" Text="Buscar" CssClass="Botonera" Font-Size="18px" OnClientClick="return ComprobarCampos()" OnClick="btnFiltrar_Click" />
               </div>

               <div class="clear-float"></div>
               <br /><br />
               <asp:Label ID="lblProblemaFechas" runat="server" Text="La diferencia entre fechas no puede ser superior a los 7 días." cssclass="negrita" ForeColor="Red" Visible="false" />
               <div> 
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
                   <br />
                </div>                                           
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
                                        <asp:Textbox ID="lblComentario" runat="server" TextMode="MultiLine" Text='<%# Eval("Comentario")%>' Rows="4" ReadOnly="true" Width="100%" />
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
        <act:ModalPopupExtender ID="modalPopUpExtender1" runat="server"
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

