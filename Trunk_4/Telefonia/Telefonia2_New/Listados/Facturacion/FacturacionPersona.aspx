<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="FacturacionPersona.aspx.vb" Inherits="Telefonia.FacturacionPersona" %>
<%@ Register Src="~/Controls/msGrafico.ascx" TagName="Graficos" TagPrefix="mp" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script src="../../js/Utiles.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
	  var ModalProgress ='<%= ModalProgress.ClientID %>';   	  
    </script>  
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="pnlFiltros" runat="server" Width="90%">
                    <table>
                        <tr>
                            <td>
                                 <asp:Label runat="server" ID="labelAño" text="año"></asp:Label>
                            </td>
                            <td>
                                 <asp:DropDownList runat="server" ID="ddlAño" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                 <asp:Label runat="server" ID="labelTipoLlamada" text="tipoLlamada"></asp:Label>
                            </td>
                            <td>
                                 <asp:DropDownList runat="server" ID="ddlTipoLlamada" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                 <asp:Label runat="server" ID="labelImportesEn" text="importesEn"></asp:Label>
                            </td>
                            <td>
                                 <asp:DropDownList runat="server" ID="ddlImporte" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>                   
            </div>
            <br />
            <asp:ImageButton runat="server" ID="imgAtras" Tooltip="atras" ImageUrl="~/App_Themes/Tema1/Images/atras.png" />
            <br />
            <div>				
	            <table class="estiloRepeaterSimple" cellpadding="0" cellspacing="0">
                    <asp:Repeater ID="rptFacPersonas" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th>
                                        &nbsp;
                                    </th>
                                    <th colspan="12">
                                        <asp:Label runat="server" text="meses"></asp:Label>
                                    </th>
                                    <th colspan="4">
                                        <asp:Label runat="server" text="totales"></asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Label runat="server" text="trabajadores"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="ene"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="feb"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="mar"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="abr"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="may"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="jun"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="jul"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="ago"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="sep"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="oct"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="nov"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="dic"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" text="total"></asp:Label>
                                    </th>
                                    <th runat="server" id="thVoz">
                                        <asp:Label runat="server" text="voz"></asp:Label>
                                    </th>
                                    <th runat="server" id="thDatos">
                                        <asp:Label runat="server" text="datos"></asp:Label>
                                    </th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr runat="server" id="trServer">
                                <td class="izquierda">
                                    <asp:Panel ID="pnlLinkPersona" runat="server">
                                        <asp:LinkButton runat="server" ID="lnkPersona" CssClass="linkSinSubrayado" OnClick="linkPersona_Click"></asp:LinkButton>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlLabelPersona" runat="server">
                                        <asp:Label runat="server" ID="lblPersona"></asp:Label>
                                    </asp:Panel>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkEnero" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkFebrero" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkMarzo" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkAbril" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkMayo" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkJunio" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkJulio" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkAgosto" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkSeptiembre" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkOctubre" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkNoviembre" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="centrado">
                                    <asp:LinkButton runat="server" ID="lnkDiciembre" OnClick="linkMes_Click" Enabled="false"></asp:LinkButton>
                                </td>
                                <td class="celdaTotal">
                                    <asp:Label runat="server" ID="lblTotal"></asp:Label>
                                </td>
                                <td class="celdaTotal">
                                    <asp:Label runat="server" ID="lblTotalVoz"></asp:Label>
                                </td>
                                <td class="celdaTotal">
                                    <asp:Label runat="server" ID="lblTotalDatos"></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td class="celdaTotal">
                                    <asp:Label runat="server" text="totales" CssClass="Titulo"></asp:Label>
                                </td>
                                <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalEnero"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalFebrero"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalMarzo"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalAbril"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalMayo"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalJunio"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalJulio"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalAgosto"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalSeptiembre"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalOctubre"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalNoviembre"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalDiciembre"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalTodo" CssClass="Titulo"></asp:Label>
                                </td>
                                 <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalVozHoriz"></asp:Label>
                                </td>
                                <td class="celdaTotal">
                                   <asp:Label runat="server" ID="lblTotalDatosHoriz"></asp:Label>
                                </td>                                                               
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>               
            </div>
            <br />
            <br />            
            <div align="center">              
				<asp:Panel runat="server" ID="pnlInfoExtensiones">
					<asp:Label runat="server" ID="labelExtenCostes" text="Extensiones de los costes"></asp:Label>&nbsp;:&nbsp;
					<asp:Label runat="server" ID="lblExtensionesGrafico" CssClass="negrita"></asp:Label>
					<br /><br />
				</asp:Panel>
                <mp:Graficos ID="GraficoFacturacion" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress">
        <asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server">
            <ProgressTemplate>
                <div style="position: relative; top: 30%; text-align: center;">
                    <asp:Image runat="server" ImageUrl="../../App_Themes/Tema1/Images/loadin.gif" Tooltip="procesando" />
                    <asp:Label runat="server" ID="labelCargando" text="cargandoDatos"></asp:Label>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <ajax:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress"
        BackgroundCssClass="modalBackground2" PopupControlID="panelUpdateProgress" />
</asp:Content>
