<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VerSolicitudes.aspx.vb" Inherits="ReferenciasVentas.VerSolicitudes" MasterPageFile="~/RefSis.Master" %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>
<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/SelectorUsuario.ascx" TagName="Usuario" TagPrefix="Usuario" %> 

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
     <style type="text/css">
        .solicitud fieldset{ border-color:#91A0A7; border-style:double }/*#91A0A7*/
        .solicitud legend{ background-color:#91A0A7; color:white; font-size:14px; padding-left:10px; padding-right: 10px }/*#91A0A7*/
    </style>
</asp:Content>

<asp:Content ID="Contenido_PRINCIPAL" ContentPlaceHolderID="cuerpoPrincipal" runat="server">
<%--    <asp:UpdatePanel ID="upGlobal" runat="server">
        <ContentTemplate>--%>

            <table style="width: 20%; margin-bottom: 20px; margin-top: 15px">
                <tr>
                    <td style="width: auto; background-color: #F9E1D2; text-align: center">
                        <asp:Label ID="lblEstadoSolicitud" runat="server" Text="Request State" ForeColor="Black" />
                    </td>
                    <td style="padding-left: 5px">
                        <asp:DropDownList ID="ddlEstadoSolicitud" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoSolicitud_SelectedIndexChanged">
                            <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Processed" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlSolicitudesPendientes" runat="server" CssClass="solicitud">
                <asp:DataList ID="dlSolicitudes" runat="server" Width="100%" OnItemDataBound="dlSolicitudes_ItemDataBound" DataKeyField="Id">
                    <ItemTemplate>
                        <asp:Panel ID="pnlSolicitud" runat="server" CssClass="solicitud" GroupingText='<%# String.Concat("Request no. ", (Eval("Id")))%>'  Style="margin-bottom: 20px" BorderColor="black"><%-- GroupingText='<%# String.Concat("Solicitud nº ", (Convert.ToInt32(Container.ItemIndex) + 1))%>' --%>
                            <table align="center" style="width: 20%; margin-top: 10px; margin-bottom:10px">
                                <tr>
                                    <td style="width: 50%; background-color: rgb(173, 206, 239); text-align: center">
                                        <asp:Label ID="lblAppDate" runat="server" Text="Appl. Date" ForeColor="Black" />
                                    </td>
                                    <td style="width: 50%; background-color: rgb(221,231,242); text-align: center"><%-- #EBEFF0 --%>
                                        <asp:Label ID="txtApplDate" runat="server" Text='<%# Eval("FechaAlta")%>' />
                                    </td>
                                    <%--<td style="width: 75%">&nbsp</td>--%>
                                </tr>
                            </table>
                            <asp:DataList ID="dlReferencia" runat="server" Width="100%" OnItemDataBound="dlReferencia_ItemDataBound"  DataKeyField="Id" HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div id="divAltura" runat="server" style="height:30px"></div>
                                    <%--<asp:Panel ID="tablaReferencia" runat="server" CssClass="solicitud" HorizontalAlign="Center" GroupingText='<%# "Ref: " + Eval("CustomerPartNumber")%>' >--%>    
                                        <table style="width:100%; margin-top: 10px; margin-bottom:10px; border-collapse: collapse">                            
                                            <tr style="background-color: #5D7B9D;">
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblTitTypeReference" Text="Type" runat="server" ForeColor="white"  />
                                                </td>
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblTitReferenceNumber" Text="Ref. No." runat="server" ForeColor="white"  />
                                                </td>
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblTitDrawingNumber" Text="Drawing No." runat="server" ForeColor="white"  />
                                                </td>
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblTitTypeNumber" Text="No. type" runat="server" ForeColor="white"  />
                                                </td>
                                                <td colspan="3" style="text-align:center">
                                                    <asp:Label ID="lblTitPlantToCharge" Text="Plants to charge" runat="server" ForeColor="white" />
                                                </td>
                                            </tr>                                                                    
                                            <tr style="background-color:#F7F6F3; color:#333333">
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblTypeReferencia" runat="server" Text='<%# Eval("TipoReferenciaNombre")%>' />
                                                </td>
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblReferenceNumber" runat="server" Text='<%# Eval("CustomerPartNumber")%>' />
                                                </td>  
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblDrawingNumber" runat="server" Text='<%# Eval("DrawingNumber")%>' />
                                                </td>  
                                                <td style="width:10%; text-align:center">
                                                    <asp:Label ID="lblTipoNumero" runat="server" Text='<%# Eval("TipoNumeroNombre")%>' />
                                                </td>                                                                                                           
                                                <td colspan="3" style="text-align:center">
                                                    <asp:CheckBoxList ID="chkPlantsToCharge" runat="server" Enabled="false" Width="100%" DataTextField="Nombre" DataValueField="Codigo" RepeatDirection="Horizontal" />
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
                                                    <asp:Label ID="lblTitPlanoWeb" Text="Plano Web" runat="server" ForeColor="white"  />
                                                </td>
                                                <td style="width:15%; text-align:center">
                                                    <asp:Label ID="lblTitNivelIngenieria" Text="Nivel Ingenieria" runat="server" ForeColor="white"  />
                                                </td>                                                                        
                                                <td style="width:20%; text-align:center">
                                                    <asp:Label ID="lblTitFinalNameBrain" Text="Ref. Name" runat="server" ForeColor="white"  />
                                                </td>
                                                <td style="width:15%; text-align:center">
                                                    <asp:Label ID="lblTitCustProjectName" Text="Customer´s Project Name" runat="server" ForeColor="white" />
                                                </td>
                                                <td style="width:15%; text-align:center">
                                                    <asp:Label ID="lblTitComentario" runat="server" Text="Comentario" ForeColor="white" />
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
                                    <%--</asp:Panel>--%>          
                                </ItemTemplate>                                                                                          
                            </asp:DataList>                     
                            <%--<asp:GridView ID="gvSolicitudesPendientes" runat="server" DataKeyNames="Id" AllowPaging="true" PageSize="10"
                                Width="100%" AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None" Caption="" AllowSorting="true"
                                OnRowDataBound="gvSolicitudesPendientes_RowDataBound">
                                <RowStyle CssClass="RowStyle" HorizontalAlign="Center" Height="32px" />
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
                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustomerPartNumber" HeaderText="Customer Part Number" />
                                    <asp:BoundField DataField="NameProduct" HeaderText="Product" />
                                    <asp:BoundField DataField="NameType" HeaderText="Type" />
                                    <asp:BoundField DataField="NameTransmissionMode" HeaderText="Transmission Mode" />
                                    <asp:BoundField DataField="NameCustomerProjectName" HeaderText="Customer´s Project Name" />
                                    <asp:BoundField DataField="Specification" HeaderText="Specification" />
                                    <asp:TemplateField HeaderText="Plants to charge">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPlantsToCharge" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BatzPartNumber" HeaderText="Batz Part Number" AccessibleHeaderText="BatzPartNumber" />
                                </Columns>
                            </asp:GridView>--%>
                        </asp:Panel>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr style="width: 100%; background-color: #5D7B9D; font-weight: bold; color: #FFFFFF; text-align: center">
                            <td>
                                <asp:Label ID="lblNoRecord" runat="server" Visible='<%#Boolean.Parse((dlSolicitudes.Items.Count = 0).ToString())%>' Text="You have no pending requests to be processed"></asp:Label>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:DataList>
            </asp:Panel>

            <asp:Panel ID="pnlSolicitudasTramitadas" runat="server">

                <asp:Label ID="Label1" runat="server" />
                <asp:Panel ID="pnlCabeceraFiltrado" runat="server" CssClass="cpHeader">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="imgCollapseFiltrado" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/expand_blue.jpg" />
                            </td>
                            <td>
                                <asp:Label ID="lblFiltradoSolicitudes" runat="server" Text="Filter requests" Font-Bold="true" ForeColor="Black" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <act:CollapsiblePanelExtender ID="cpeDatosSolicitudes" runat="server"
                    TargetControlID="pnlFiltrado"
                    CollapseControlID="pnlCabeceraFiltrado"
                    ExpandControlID="pnlCabeceraFiltrado"
                    Collapsed="false"
                    CollapsedSize="0"
                    AutoCollapse="False"
                    AutoExpand="False"
                    ExpandDirection="Vertical"
                    ImageControlID="imgCollapseFiltrado"
                    TextLabelID="Label1"
                    ScrollContents="false"
                    CollapsedImage="~/App_Themes/Batz/IconosAcciones/expand_blue.jpg"
                    ExpandedImage="~/App_Themes/Batz/IconosAcciones/collapse_blue.jpg">
                </act:CollapsiblePanelExtender>

                <asp:Panel ID="pnlFiltrado" runat="server" CssClass="cpBody">
                    <table width="100%" cellpadding="3">
                        <tr>
                            <td class="definicion15">
                                <asp:Label ID="lblIdentificador" runat="server" Text="Identifier" />
                            </td>
                            <td class="campoTextoNormal width10">
                                <asp:TextBox ID="txtIdFiltrado" runat="server" Width="90%" MaxLength="6" onkeypress="return validatenumerics(event);" />
                            </td>
                            <td class="definicion15">
                                <asp:Label ID="lblAprobado" runat="server" Text="Approved" />
                            </td>
                            <td class="campoTextoNormal width10">
                                <asp:RadioButtonList ID="rblAprobadoFiltrado" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" />
                                    <asp:ListItem Text="Indifferent" Value="2" Selected="true" />
                                </asp:RadioButtonList>
                            </td>
                            <td class="definicion15">
                                <asp:Label ID="lblUsuarioFiltrado" runat="server" Text="Dealer" />
                            </td>
                            <td class="campoTextoNormal" colspan="3">
                                <Usuario:Usuario ID="txtUsuarioFiltrado" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="definicion15" rowspan="2">
                                <asp:Label ID="lblFechaCreacion" runat="server" Text="Creation date" />
                            </td>
                            <td class="definicion">
                                <asp:Label ID="lblFechaCreacionDesde" runat="server" Text="From" AssociatedControlID="imgCalendarioCreacionDesde" />
                            </td>
                            <td class="campoTextoNormal">
                                <asp:TextBox ID="txtFechaCreacionDesde" runat="server" Width="100%" />
                            </td>
                            <td class="campoTextoNormal" align="left">
                                <asp:ImageButton ID="imgCalendarioCreacionDesde" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />
                                <act:CalendarExtender ID="imgCalendarioCreacionDesde_CalendarExtender" runat="server" TargetControlID="txtFechaCreacionDesde" PopupButtonID="imgCalendarioCreacionDesde" />
                            </td>
                            <td class="definicion15" rowspan="2">
                                <asp:Label ID="lblFechaResolucion" runat="server" Text="Dealing date" />
                            </td>
                            <td class="definicion">
                                <asp:Label ID="lblFechaResolucionDesde" runat="server" Text="From" AssociatedControlID="imgCalendarioResolucionDesde" />
                            </td>
                            <td class="campoTextoNormal">
                                <asp:TextBox ID="txtFechaResolucionDesde" runat="server" Width="100%" />
                            </td>
                            <td class="campoTextoNormal" align="left">
                                <asp:ImageButton ID="imgCalendarioResolucionDesde" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />
                                <act:CalendarExtender ID="imgCalendarioResolucionDesde_CalendarExtender" runat="server" TargetControlID="txtFechaResolucionDesde" PopupButtonID="imgCalendarioResolucionDesde" />
                            </td>
                        </tr>
                        <tr>
                            <td class="definicion">
                                <asp:Label ID="lblFechaCreacionHasta" runat="server" Text="To" />
                            </td>
                            <td class="campoTextoNormal">
                                <asp:TextBox ID="txtFechaCreacionHasta" runat="server" Width="100%" />
                            </td>
                            <td class="campoTextoNormal" align="left">
                                <asp:ImageButton ID="imgCalendarioCreacionHasta" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />
                                <act:CalendarExtender ID="imgCalendarioCreacionHasta_CalendarExtender" runat="server" TargetControlID="txtFechaCreacionHasta" PopupButtonID="imgCalendarioCreacionHasta" />
                            </td>
                            <td class="definicion">
                                <asp:Label ID="lblFechaResolucionHasta" runat="server" Text="To" />
                            </td>
                            <td class="campoTextoNormal">
                                <asp:TextBox ID="txtFechaResolucionHasta" runat="server" Width="100%" />
                            </td>
                            <td class="campoTextoNormal" align="left">
                                <asp:ImageButton ID="imgCalendarioResolucionHasta" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />
                                <act:CalendarExtender ID="imgCalendarioResolucionHasta_CalendarExtender" runat="server" TargetControlID="txtFechaResolucionHasta" PopupButtonID="imgCalendarioResolucionHasta" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Button ID="lnkbLimpiarCampos" runat="server" Text="Clean" CssClass="boton" UseSubmitBehavior="false" OnClick="lnkbLimpiarCampos_Click" />
                            </td>
                            <td colspan="4" align="right">
                                <asp:Button ID="lnkbFiltrado" runat="server" Text="Filter" OnClick="lnkbFiltrado_Click" CssClass="boton" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <hr />
                <asp:GridView ID="gvSolicitudesTramitadas" runat="server" DataKeyNames="Id" AllowPaging="true" PageSize="5"
                    Width="100%" AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None" Caption="" AllowSorting="true"
                    OnRowDataBound="gvSolicitudesTramitadas_RowDataBound" OnPageIndexChanging="gvSolicitudesTramitadas_PageIndexChanging">
                    <RowStyle CssClass="RowStyle" HorizontalAlign="Center" Height="32px" BackColor="#F9E1D2" />
                    <FooterStyle CssClass="FooterStyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                    <HeaderStyle CssClass="HeaderStyle" />
                    <EditRowStyle CssClass="EditRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Request ID" SortExpression="Id" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="FechaAlta" HeaderText="Creation date" SortExpression="FechaAlta" ItemStyle-Width="15%" />
                        <asp:TemplateField HeaderText="APPROVED" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Image ID="imgAprobado" runat="server" ImageUrl='<%# If(Eval("Aprobado"), "~/App_Themes/Batz/Imagenes/seleccionado.png", "~/App_Themes/Batz/Imagenes/error.png")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NombreUsuarioTramitador" HeaderText="Dealer" SortExpression="UsuarioTramitador" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="FechaGestion" HeaderText="Dealing date" SortExpression="FechaGestion" ItemStyle-Width="15%" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td colspan="100%">
                                        <asp:Panel ID="pnlDetails" runat="server">
                                            <asp:GridView ID="gvReferencias" runat="server" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="GridViewASP" GridLines="None" BorderColor="Black"
                                                BorderWidth="1px" BorderStyle="Solid" OnRowDataBound="gvReferencias_RowDataBound">
                                                <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                                                <FooterStyle CssClass="FooterStyle" />
                                                <PagerStyle CssClass="PagerStyle" />
                                                <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                <HeaderStyle CssClass="HeaderStyle" Font-Size="10px" />
                                                <EditRowStyle CssClass="EditRowStyle" />
                                                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblSinRegistros" runat="server" Text="" Font-Italic="true" Font-Size="10px" />
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField DataField="TipoReferenciaNombre" HeaderText="Type" SortExpression="TipoReferenciaNombre" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="CustomerPartNumber" HeaderText="Part No." SortExpression="CustomerPartNumber" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="DrawingNumber" HeaderText="Drawing No." SortExpression="DrawingNumber" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="TipoNumeroNombre" HeaderText="Type No." SortExpression="TipoNumeroNombre" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />                                                    
                                                    <asp:BoundField DataField="PreviousBatzPartNumber" HeaderText="Previous Batz Part Number" SortExpression="PreviousBatzPartNumber" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="EvolutionChanges" HeaderText="Evolution Changes" SortExpression="EvolutionChanges" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="PlanoWeb" HeaderText="Drawing Number" SortExpression="PlanoWeb" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="NivelIngenieria" HeaderText="Engineering Level" SortExpression="NivelIngenieria" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="FinalNameBrain" HeaderText="Part Name" SortExpression="FinalNameBrain" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <%--<asp:BoundField DataField="NameProduct" HeaderText="Product" SortExpression="NameProduct" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="NameType" HeaderText="Type" SortExpression="NameType" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="NameTransmissionMode" HeaderText="Transmission Mode" SortExpression="NameTransmissionMode" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:BoundField DataField="Specification" HeaderText="Specification" SortExpression="Specification" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />--%>
                                                    <asp:BoundField DataField="NameCustomerProjectName" HeaderText="Customer´s Project Name" SortExpression="NameCustomerProjectName" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />                                                    
                                                    <asp:BoundField DataField="Comentario" HeaderText="Comment" SortExpression="Comment" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                    <asp:TemplateField HeaderText="Plants to charge" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPlantsToCharge" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="BatzPartNumber" HeaderText="Batz Part Number" SortExpression="BatzPartNumber" AccessibleHeaderText="BatzPartNumber" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
<%--        </ContentTemplate>
    </asp:UpdatePanel>
    <PanelCargandoDatos:PanelCargandoDatos ID="panelCargandoDatos1" runat="server" />--%>
</asp:Content>