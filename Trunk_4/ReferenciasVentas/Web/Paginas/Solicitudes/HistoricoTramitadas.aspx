<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HistoricoTramitadas.aspx.vb" Inherits="ReferenciasVentas.HistoricoTramitadas" MasterPageFile="~/RefSis.Master" %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>
<%@ Register Src="~/Controles/SelectorUsuario.ascx" TagName="Usuario" TagPrefix="Usuario" %> 

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function validatenumerics(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if (keycode > 31 && (keycode < 48 || keycode > 57)) {
                return false;
            }
            else return true;
        }
    </script>
     <style type="text/css">
        .solicitud fieldset{ border-color:#91A0A7; border-style:double }
        .solicitud legend{ background-color:#91A0A7; color:white; font-size:14px; padding-left:10px; padding-right: 10px }/*#91A0A7*/
    </style>
</asp:Content>

<asp:Content ID="Contenido_PRINCIPAL" ContentPlaceHolderID="cuerpoPrincipal" runat="server">
    <asp:Label ID="Label1" runat="server" />
    <asp:Panel ID="pnlCabeceraFiltrado" runat="server" CssClass="cpHeader" >
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

    <act:CollapsiblePanelExtender id="cpeDatosSolicitudes" runat="server"  
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
                    <asp:Label ID="lblIdentificador" runat="server" Text="ID" />
                </td>
                <td class="campoTextoNormal width10">
                    <asp:TextBox ID="txtIdFiltrado" runat="server" Width="90%" MaxLength="6" onkeypress="return validatenumerics(event);" />
                </td>
                <td class="definicion15">
                    <asp:Label ID="lblAprobado" runat="server" Text="Approved" />
                </td>
                <td class="campoTextoNormal width10">
                    <asp:RadioButtonList ID="rblAprobadoFiltrado" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1" Selected="true" />
                        <asp:ListItem Text="No" Value="0" />
                        <%--<asp:ListItem Text="Indiferente" Value="2" Selected="true" />--%>
                    </asp:RadioButtonList>
                </td>
                <td class="definicion15">
                    <asp:Label ID="lblUsuarioFiltrado" runat="server" Text="Applicant" />
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
                    <asp:ImageButton ID="imgCalendarioCreacionDesde" runat="server" AlternateText="Calendar" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />                                    
                    <act:CalendarExtender ID="imgCalendarioCreacionDesde_CalendarExtender" runat="server" TargetControlID="txtFechaCreacionDesde" PopupButtonID="imgCalendarioCreacionDesde" />
                </td>
                <td class="definicion15" rowspan="2">
                    <asp:Label ID="lblFechaResolucion" runat="server" Text="Processing date" />
                </td>
                <td class="definicion">
                    <asp:Label ID="lblFechaResolucionDesde" runat="server" Text="From" AssociatedControlID="imgCalendarioResolucionDesde" />
                </td>
                <td  class="campoTextoNormal">
                    <asp:TextBox ID="txtFechaResolucionDesde" runat="server" Width="100%" />
                </td>
                <td class="campoTextoNormal" align="left">
                    <asp:ImageButton ID="imgCalendarioResolucionDesde" runat="server" AlternateText="Calendar" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />                                    
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
                    <asp:ImageButton ID="imgCalendarioCreacionHasta" runat="server" AlternateText="Calendar" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />                                    
                    <act:CalendarExtender ID="imgCalendarioCreacionHasta_CalendarExtender" runat="server" TargetControlID="txtFechaCreacionHasta" PopupButtonID="imgCalendarioCreacionHasta" />
                </td>
                <td class="definicion">
                    <asp:Label ID="lblFechaResolucionHasta" runat="server" Text="To" />
                </td>
                <td class="campoTextoNormal">
                    <asp:TextBox ID="txtFechaResolucionHasta" runat="server" Width="100%" />
                </td>
                <td class="campoTextoNormal" align="left">
                    <asp:ImageButton ID="imgCalendarioResolucionHasta" runat="server" AlternateText="Calendar" ImageUrl="~/App_Themes/Batz/Imagenes/Calendario.gif" />                                    
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
    <asp:Panel ID="pnlSolicitudasTramitadas" runat="server">
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
                <asp:BoundField DataField="Id" HeaderText="Request ID" SortExpression="Id" ItemStyle-Width="15%"/>
                <asp:BoundField DataField="FechaAlta" HeaderText="Creation date" SortExpression="FechaAlta" ItemStyle-Width="15%"/>
                <asp:TemplateField HeaderText="APPROVED" ItemStyle-Width="15%" >
                    <ItemTemplate>
                        <asp:Image ID="imgAprobado" runat="server" ImageUrl='<%# If(Eval("Aprobado"), "~/App_Themes/Batz/Imagenes/seleccionado.png", "~/App_Themes/Batz/Imagenes/error.png")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NombreUsuarioTramitador" HeaderText="Dealer" SortExpression="UsuarioTramitador" ItemStyle-Width="15%"/>
                <asp:BoundField DataField="FechaGestion" HeaderText="Processing date" SortExpression="FechaGestion" ItemStyle-Width="15%"/> 
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
                                            <asp:BoundField DataField="TipoNumeroNombre" HeaderText="Type" SortExpression="TipoNumeroNombre" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black"/>           
                                            <asp:BoundField DataField="CustomerPartNumber" HeaderText="Customer Part Number" SortExpression="CustomerPartNumber" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black"/>                                            
                                            <asp:BoundField DataField="FinalNameBrain" HeaderText="Part Name" SortExpression="FinalNameBrain" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black"/>                                                    
                                            <asp:BoundField DataField="PreviousBatzPartNumber" HeaderText="Previous Batz Part Number" SortExpression="PreviousBatzPartNumber" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black"/>
                                            <asp:BoundField DataField="BatzPartNumber" HeaderText="Batz Part Number" SortExpression="BatzPartNumber" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black"/>
                                            <asp:TemplateField HeaderText="Plants to charge" HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPlantsToCharge" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                               
                                            <asp:TemplateField HeaderStyle-BackColor="#cbd5ce" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>                                                                                                 
                                                    <asp:LinkButton ID="lnbtBrain" runat="server" Text="See in Brain" CommandArgument='<%# Eval("Id")%>' Visible='<%#If(Eval("TipoNumero") = "4", "False", "True")%>' OnClick="lnbtBrain_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
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

</asp:Content>