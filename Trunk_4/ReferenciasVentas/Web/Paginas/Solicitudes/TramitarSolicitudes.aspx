<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TramitarSolicitudes.aspx.vb" Inherits="ReferenciasVentas.TramitarSolicitudes" MasterPageFile="~/RefSis.Master" %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .solicitud fieldset{ border-color:rgb(173, 206, 239); }
        .solicitud legend{ background-color: rgb(173, 206, 239); color:black; font-size:14px; padding-left:10px; padding-right: 10px; text-align:center; margin-left:35%; width:30% }
    </style>
</asp:Content>

<asp:Content ID="Contenido_PRINCIPAL" ContentPlaceHolderID="cuerpoPrincipal" runat="server">

    <asp:UpdatePanel ID="upSolicitudes" runat="server">
        <ContentTemplate>                           
            <asp:Panel ID="pnlTramitarSolicitudes" runat="server">
                <div style="float:left">
                    <table style="width:auto; margin-bottom:20px; margin-top: 15px">
                        <tr>
                            <th colspan="2" style="background-color:#5D7B9D; color:White; height:30px; text-align: center">
                                <asp:Label ID="titTramitarSolicitudes" runat="server" style="text-transform:uppercase" Text="Process requests" />
                            </th>
                            <tr>
                                <td style="width: auto; background-color: #F9E1D2; text-align:center">
                                    <asp:Label ID="lblTipoSolicitud" runat="server" Text="Request Type" />
                                </td>
                                <td style="padding-left:5px">
                                    <asp:DropDownList ID="ddlTipoSolicitud" runat="server" DataTextField="Nombre" DataValueField="Id" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: auto; background-color: #F9E1D2; text-align:center">
                                    <asp:Label ID="lblSolicitud" runat="server" Text="Request" />
                                </td>
                                <td style="padding-left:5px">
                                    <asp:DropDownList ID="ddlSolicitud" runat="server" OnSelectedIndexChanged="btnCargarSolicitud_Click" AutoPostBack="true" />
                                </td>
                            </tr>
                        </tr>
                    </table>
                </div>

                <div id="divSolicitudAprobada" runat="server" style="width:auto; overflow:hidden">
                    <table style="width:100%; margin-bottom:20px; margin-top: 15px; text-align:center">
                        <th style="padding-bottom:30px">
                            <asp:Label ID="lblIntegraciónRealizada" runat="server" style="text-transform:uppercase" Text="The request is completed" Font-Size="18px" Font-Underline="true" />
                        </th>
                        <tr>
                            <td>
                                <asp:Button ID="btnAprobarSolicitud" runat="server" Text="Close request" OnClick="btnAprobarSolicitud_Click" Font-Size="20px" BackColor="#8FBE4E" ForeColor="White" />
                                <act:ConfirmButtonExtender ID="cbeValidar" runat="server" DisplayModalPopupID="mpeValidar" TargetControlID="btnAprobarSolicitud"></act:ConfirmButtonExtender>                    
                                <act:ModalPopupExtender ID="mpeValidar" runat="server" PopupControlID="pnlConfirmarAprobacion" TargetControlID="btnAprobarSolicitud" OkControlID="btnSi"
                                    CancelControlID="btnNo" BackgroundCssClass="modalBackground"></act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmarAprobacion" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Close request" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmar" runat="server" Text="Are you sure you want to close the request?" />
                                    </div>
                                    <div class="footer" align="center">
                                        <asp:Button ID="btnSi" runat="server" CssClass="si" Text="Yes" />
                                        <asp:Button ID="btnNo" runat="server" CssClass="no" Text="No" />
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>                
                <div style="clear: both"></div>

                <table style="width:auto; margin-top:10px; margin-bottom: 20px">
                    <tr>
                        <td style="width:10%; background-color:rgb(173, 206, 239); text-align:center">
                            <asp:Label ID="lblApplicant" runat="server" Text="Applicant" />
                        </td>
                        <td style="width:25%; background-color:rgb(221,231,242); text-align:center">
                            <asp:Label ID="txtApplicant" runat="server" />
                        </td>
                        <td style="width:30%">&nbsp</td>
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
                    <tr>
                        <td style="width:10%; background-color:rgb(173, 206, 239); text-align:center">
                            <asp:Label ID="lblValidator" runat="server" Text="Validator" />
                        </td>
                        <td style="width:25%; background-color:rgb(221,231,242); text-align:center">
                            <asp:Label ID="txtValidator" runat="server" />
                        </td>
                        <td style="width:30%">&nbsp</td>
                    </tr>
                </table>
                <asp:DataList ID="dlReferencia" runat="server" Width="100%" OnItemDataBound="dlReferencia_ItemDataBound" DataKeyField="Id" HorizontalAlign="Center">
                    <ItemTemplate>
                        <div id="divAltura" runat="server" style="height:30px"></div>
                        <asp:Panel ID="tablaReferencia" runat="server" CssClass="solicitud" HorizontalAlign="Center" GroupingText='<%# "Customer Part Number: " + Eval("CustomerPartNumber") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Batz Part Number: " + If(Eval("BatzPartNumber") = String.Empty, "-", Eval("BatzPartNumber")) %>' >    
                            <table style="width:100%; margin-top: 10px; margin-bottom:10px; border-collapse: collapse">                            
                                <tr style="background-color: #5D7B9D;">
                                    <td style="width:10%; text-align:center">
                                        <asp:Label ID="lblTitTypeReference" Text="Type" runat="server" ForeColor="white"  />
                                    </td>
                                    <td style="width:15%; text-align:center">
                                        <asp:Label ID="lblTitDrawingNumber" Text="Previous Draw-Part No." runat="server" ForeColor="white"  />
                                    </td>
                                    <td style="width:10%; text-align:center">
                                        <asp:Label ID="lblTitTypeNumber" Text="No. type" runat="server" ForeColor="white"  />
                                    </td>
                                    <td colspan="3" style="text-align:center">
                                        <asp:Label ID="lblTitPlantToCharge" Text="Plants to charge" runat="server" ForeColor="white" />
                                    </td>
                                    <td style="width:20%; text-align:center">
                                        <asp:Label ID="lblTitGeneradoEnBrain" Text="Treated" runat="server" ForeColor="white" />
                                    </td>
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
                                    <td colspan="3" style="text-align:center">
                                        <asp:CheckBoxList ID="chkPlantsToCharge" runat="server" Enabled="false" Width="100%" DataTextField="Nombre" DataValueField="Codigo" RepeatDirection="Horizontal" />
                                    </td>
                                    <td id="tdGeneradoEnBrain" runat="server" style="width:10%"><%-- style='<%# (If Eval("InsercionBrain"), "background:rgb(221,231,242); color:#000000; width:10%", "background:rgb(255,241,181); color:#284775; width:10%")%>'--%>
                                        <asp:Label ID="lblGeneradoEnBrain" runat="server" Font-Italic="true" /> <%-- Text='<%# If(Eval("InsercionBrain"), "Generated", "Pending")%>'--%>
                                    </td>
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
                                <tr>
                                    <td colspan="7" style="text-align: center; padding-top: 20px;">
                                        <asp:Label ID="lblErrorReferenciaBrain" runat="server" ForeColor="Red" Font-Bold="true" Font-Italic="true" style="margin-top:30px;" />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="text-align:center;">
                                        <asp:Button ID="btnGuardarBrain" runat="server" Text="Create Part Number in Brain" CommandArgument='<%# Eval("Id")%>' OnClick="btnBrain_Click" style="margin-top:20px;" />
                                        <asp:Button ID="btnEliminarBrain" runat="server" Text="Delete Part Number in brain" CommandArgument='<%# Eval("Id") %>' OnClick="btnEliminarBrain_Click" style="margin-left:50px; margin-top:20px" />
                                    </td>
                                </tr>                                
                            </table>
                        </asp:Panel>          
                    </ItemTemplate>                                                                                          
                </asp:DataList>                     
                <table style="width:100%; margin-top:20px">
                    <tr>
                        <td style="text-align:center;">                            
                            <asp:Button ID="btnRechazarSolicitud" runat="server" Text="Reject request" OnClick="btnRechazarSolicitud_Click" Font-Size="16px" BackColor="Red" ForeColor="White" />                             
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlSinSolicitudes" runat="server">
                <Titulo:Titulo Texto="Process requests" runat="server" />
                <table style="width:100%">
                    <tr style="background-color:#5D7B9D;font-weight:bold; color:#FFFFFF; text-align:center">
                        <td>
                            <asp:Label ID="lblNoRecord" runat="server" Text="There are no requests in pending process"></asp:Label>  
                        </td>
                    </tr>
                </table> 
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
