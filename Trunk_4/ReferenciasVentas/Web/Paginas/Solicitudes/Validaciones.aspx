 <%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/RefSis.Master" CodeBehind="Validaciones.aspx.vb" Inherits="ReferenciasVentas.Validaciones" %>
<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
     <style type="text/css">
        .solicitud fieldset{ border-color:rgb(173, 206, 239); }
        .solicitud legend{ background-color: rgb(173, 206, 239); color:black; font-size:14px; padding-left:10px; padding-right: 10px; text-align:center; margin-left:45%; width:10% }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">    
      
 
   <Titulo:Titulo ID="titValidacion" Texto="Selling Part Numbers requests validation" runat="server"  />
    <asp:UpdatePanel ID="upValidacion" runat="server" >
            <ContentTemplate>            
                <asp:Panel ID="pnlGridView" runat="server" Width="100%">
                    <%-- Capa de las solicitudes--%> 
                    <div style="margin-bottom:30px">
                        <asp:GridView ID="gvSolicitudes" runat="server" DataKeyNames="Id" AllowPaging="false" AllowSorting="true"
                            Width="100%" AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None" Caption="">
                            <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                            <FooterStyle CssClass="FooterStyle" />
                            <PagerStyle CssClass="PagerStyle" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" />
                            <HeaderStyle CssClass="HeaderStyle" />
                            <EditRowStyle CssClass="EditRowStyle" />
                            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                            <EmptyDataTemplate>
                                <asp:Label ID="lblSinRegistros" runat="server" Text="" />
                            </EmptyDataTemplate>
                            <Columns>                                                                                                
                                <asp:TemplateField HeaderText="Id" SortExpression="Id" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Visible="true" Text='<%# Eval("Id")%>'></asp:Label>                                            
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Requester" DataField="NombreSolicitante" SortExpression="NombreSolicitante" ItemStyle-HorizontalAlign="Left" />                                                           
                                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha alta" SortExpression="FechaCreacion" />                                                                                                                                
                                <asp:TemplateField HeaderText="Validator" SortExpression="NombreUsuarioValidador">
                                    <ItemTemplate>
                                        <asp:Label ID="lblValidador" runat="server" Text='<%# Eval("NombreUsuarioValidador").ToUpper()%>' style="text-transform:uppercase"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowSelectButton="true" ButtonType="Link" Visible="false" />                                   
                            </Columns>
                        </asp:GridView>
                    </div>                   
                </asp:Panel>               

                <asp:Panel ID="pnlDetalleSolicitud" runat="server">
                                            
                    <div style="float:left">
                        <table style="width:auto; margin-top:10px; margin-bottom: 20px">
                            <tr>
                                <td style="width:10%; background-color:rgb(173, 206, 239); text-align:center">
                                    <asp:Label ID="lblIdSolicitud" runat="server" Text="ID Request" />
                                </td>
                                <td style="width:25%; background-color:rgb(221,231,242); text-align:center">
                                    <asp:Label ID="txtIdSolicitud" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%; background-color:rgb(173, 206, 239); text-align:center">
                                    <asp:Label ID="lblApplicant" runat="server" Text="Applicant" />
                                </td>
                                <td style="width:25%; background-color:rgb(221,231,242); text-align:center">
                                    <asp:Label ID="txtApplicant" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%; background-color:rgb(173, 206, 239); text-align:center">
                                    <asp:Label ID="lblAppDate" runat="server" Text="Appl. Date" />
                                </td>
                                <td style="width:25%; background-color:rgb(221,231,242); text-align:center">
                                    <asp:Label ID="txtApplDate" runat="server" />
                                </td>
                                <td style="width:30%">&nbsp</td>
                            </tr>
                        </table>
                    </div>

                    <div id="divSolicitudAprobada" runat="server" style="width:30%; overflow:hidden"><%-- width:auto--%>
                        <table style="width:100%; margin-bottom:20px; margin-top: 15px; text-align:center">                            
                            <tr>
                                <td>
                                    <asp:Button ID="btnValidarSolicitud" runat="server" Text="Approve request" OnClick="btnValidarSolicitud_Click" Font-Size="20px" BackColor="#8FBE4E" ForeColor="White" />
                                    <act:ConfirmButtonExtender ID="cbeValidar" runat="server" DisplayModalPopupID="mpeValidar" TargetControlID="btnValidarSolicitud"></act:ConfirmButtonExtender>                    
                                    <act:ModalPopupExtender ID="mpeValidar" runat="server" PopupControlID="pnlConfirmarAprobacion" TargetControlID="btnValidarSolicitud" OkControlID="btnSi"
                                        CancelControlID="btnNo" BackgroundCssClass="modalBackground"></act:ModalPopupExtender>
                                    <asp:Panel ID="pnlConfirmarAprobacion" runat="server" CssClass="modalPopup" Style="display: none">
                                        <div class="header">
                                            <asp:Label ID="lblConfirmacion" runat="server" Text="Approve request" />
                                        </div>
                                        <div class="body" style="margin-bottom:10px">
                                            <asp:Label ID="lblConfirmarBorrado" runat="server" Text="Are you sure you want to approve the request?" />                                            
                                        </div>
                                        <div class="footer" align="center">
                                            <asp:Button ID="btnSi" runat="server" CssClass="si" Text="Yes" />
                                            <asp:Button ID="btnNo" runat="server" CssClass="no" Text="No" />
                                        </div>
                                    </asp:Panel>   
                                </td>
                                <td>
                                    <asp:Button ID="btnRechazarSolicitud" runat="server" Text="Reject request" OnClick="btnRechazarSolicitud_Click" Font-Size="20px" BackColor="Red" ForeColor="White" />
                                </td>
                            </tr>
                        </table>
                    </div>                
                    <div style="clear: both"></div>
                    
                    <asp:DataList ID="dlReferencia" runat="server" Width="100%" OnItemDataBound="dlReferencia_ItemDataBound" DataKeyField="Id" HorizontalAlign="Center">
                        <ItemTemplate>
                            <div id="divAltura" runat="server" style="height:30px"></div>
                            <asp:Panel ID="tablaReferencia" runat="server" CssClass="solicitud" HorizontalAlign="Center" GroupingText='<%# "CPN: " + Eval("CustomerPartNumber")%>' >    
                                <table style="width:100%; margin-top: 10px; margin-bottom:10px; border-collapse: collapse">                            
                                    <tr style="background-color: #5D7B9D;">
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblTitTypeReference" Text="Type" runat="server" ForeColor="white"  />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblTitDrawingNumber" Text="Drawing No." runat="server" ForeColor="white"  />
                                        </td>
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblTitTypeNumber" Text="No. type" runat="server" ForeColor="white"  />
                                        </td>
                                        <td colspan="4" style="text-align:center">
                                            <asp:Label ID="lblTitPlantToCharge" Text="Plants to charge" runat="server" ForeColor="white" />
                                        </td>
                                        <%--<td style="width:20%; text-align:center">
                                            <asp:Label ID="lblTitGeneradoEnBrain" Text="Part in Brain" runat="server" ForeColor="white" />
                                        </td>--%>
                                    </tr>                                                                    
                                    <tr style="background-color:#F7F6F3; color:#333333">
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblTypeReferencia" runat="server" Text='<%# Eval("TipoReferenciaNombre")%>' />                                        
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblDrawingNumber" runat="server" Text='<%# Eval("DrawingNumber")%>' />
                                        </td>  
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblTipoNumero" runat="server" Text='<%# Eval("TipoNumeroNombre")%>' />
                                        </td>                                                                                                           
                                        <td colspan="4" style="text-align:center">
                                            <asp:CheckBoxList ID="chkPlantsToCharge" runat="server" Enabled="false" Width="100%" DataTextField="Nombre" DataValueField="Codigo" RepeatDirection="Horizontal" />
                                        </td>
                                        <%--<td  style='<%# If(Eval("InsercionBrain"), "background:rgb(221,231,242); color:#000000; width:10%", "background:rgb(255,241,181); color:#284775; width:10%")%>'>
                                            <asp:Label ID="lblGeneradoEnBrain" runat="server" Text='<%# If(Eval("InsercionBrain"), "Generated", "Pending")%>' Font-Italic="true" />
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="7">&nbsp</td>
                                    </tr>
                                    <tr style="background-color: #5D7B9D">
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblTitPreviousBatzNumber" Text="Prev. Batz Part Number" runat="server" ForeColor="white"  />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblTitEvolutionChanges" Text="Evolution Changes" runat="server" ForeColor="white"  />
                                        </td>
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblTitPlanoWeb" Text="Drawing No." runat="server" ForeColor="white"  />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblTitNivelIngenieria" Text="Engineering Level" runat="server" ForeColor="white"  />
                                        </td>                                                                        
                                        <td style="width:20%; text-align:center">
                                            <asp:Label ID="lblTitFinalNameBrain" Text="Part Name" runat="server" ForeColor="white"  />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblTitCustProjectName" Text="Customer´s Project Name" runat="server" ForeColor="white" />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblTitComentario" runat="server" Text="Comment" ForeColor="white" />
                                        </td>
                                    </tr>
                                    <tr style="background-color:#F7F6F3; color:#333333;">
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblPreviousBatzNumber" runat="server" Text='<%# Eval("PreviousBatzPartNumber")%>' />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblEvolutionChanges" runat="server" Text='<%# Eval("EvolutionChanges")%>'  />
                                        </td> 
                                        <td style="width:10%; text-align:center">
                                            <asp:Label ID="lblPlanoWeb" runat="server" Text='<%# Eval("PlanoWeb")%>'  />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblNivelIngenieria" runat="server" Text='<%# Eval("NivelIngenieria")%>'  />
                                        </td>                                      
                                        <td style="width:20%; text-align:center">
                                            <asp:Label ID="lblFinalNameBrain" runat="server" Text='<%# Eval("FinalNameBrain")%>' />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblCustProjectName" runat="server" Text='<%# Eval("NameCustomerProjectName")%>' />
                                        </td>
                                        <td style="width:15%; text-align:center">
                                            <asp:Label ID="lblComentario" runat="server" Text='<%# Eval("Comentario")%>' />
                                        </td>                                                                                          
                                    </tr>                                                                 
                                </table>
                            </asp:Panel>          
                        </ItemTemplate>                                                                                          
                    </asp:DataList>                                        

                    <div class="BotonVolverListado">
                        <asp:Button ID="lbtnVolverListadoSolicitudes" runat="server" CausesValidation="False" Text="Back to the list" ToolTip="Back to the list of requests" CssClass="boton" UseSubmitBehavior="false" />                         
                    </div>
                </asp:Panel>

                <asp:Button ID="btnMP_Open" runat="server" style="display:none" />
                <act:ModalPopupExtender ID="mpe_RechazarSolicitud" runat="server" BackgroundCssClass="modalBackground" CancelControlID="imgCerrar" PopupControlID="pnlRechazarIncidencia" TargetControlID="btnMP_Open">
                </act:ModalPopupExtender>
                <asp:HiddenField ID="hfIdSolicitud" runat="server" />
                <asp:Panel ID="pnlRechazarIncidencia" runat="server" CssClass="modalBox" Width="50%" style="display:none">
                    <table style="width:100%">
                        <tr>                
                            <td style="text-align:right" valign="top">
                                <asp:ImageButton ID="imgCerrar" runat="server" AlternateText="Close" ImageAlign="Right" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/cerrar.gif" ToolTip="Close" />
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color:#5D7B9D; color:White; height:30px; text-align:center">                    
                                <asp:Label ID="lblMotivoRechazo" runat="server" Text="Reason for rejection" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtMotivoRechazo" runat="server" Width="99%" Rows="6" TextMode="MultiLine" />
                                <asp:RequiredFieldValidator ID="rfvMotivoRechazo" runat="server" Text="*" ErrorMessage="Required field" Display="None" ControlToValidate="txtMotivoRechazo" ValidationGroup="CamposVacios" /> 
                                <act:ValidatorCalloutExtender ID="vceMotivoRechazo" runat="server" TargetControlID="rfvMotivoRechazo" PopupPosition="BottomLeft" />
                            </td>                 
                        </tr>
                        <tr style="text-align:center">
                            <td style="padding-top:30px">
                                <asp:Button ID="btnConfirmarRechazo" runat="server" Text="Confirm rejection" ValidationGroup="CamposVacios" OnClick="btnConfirmarRechazo_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>                                          
            </ContentTemplate>
        </asp:UpdatePanel>
       
    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>


