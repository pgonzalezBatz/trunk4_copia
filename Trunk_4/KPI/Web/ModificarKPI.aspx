<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ModificarKPI.aspx.vb" Inherits="WebRaiz.ModificarKPI" %>
<%@ Register src="~/Controles/PanelCargandoDatos.ascx" tagname="CargandoDatos" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="js/jQuery/jquery.js"></script>
    <script language="javascript" type="text/javascript">       
        Sys.Application.add_load(init);
        function init() {   
            //Permite el punto, la coma, las fechas y el borrado
            $('.numeric').keypress(function (e) {
                var verified = (e.which == 8 || e.which == undefined || e.which == 0 || e.which == 44 || e.which == 45 || e.which == 46) ? null : String.fromCharCode(e.which).match(/[^0-9]/);
                if (verified) { e.preventDefault(); }
            });

            //$('input[type = "text"]').keyup(function () { $('#<btnCalcularKPIs.ClientID').css('visibility', 'hidden'); });
        };
    </script>
    <asp:MultiView runat="server" ID="mvKPI">
        <asp:View runat="server" ID="vSinPerfil">
            <b><asp:Label runat="server" ID="lblSinPerfil" Text="No tiene ningun perfil configurado. Cree un helpdesk si necesita modificar los KPIs de alguna area-planta"></asp:Label></b>
        </asp:View>
        <asp:View runat="server" ID="vKPI">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>    
                    <table id="filtro">
                        <tr>
                            <th><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></th>
                            <th><asp:Label runat="server" ID="labelNegocio" Text="Negocio"></asp:Label></th>
                            <th><asp:Label runat="server" ID="labelArea" Text="Area"></asp:Label></th>
                            <th><asp:Label runat="server" ID="labelAnno" Text="Año"></asp:Label></th>
                            <th style="padding-right:30px;"><asp:Label runat="server" ID="labelMes" Text="Mes"></asp:Label></th>
                        </tr>
                        <tr>
                            <td><asp:DropDownList runat="server" ID="ddlPlantas" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true"></asp:DropDownList></td>
                            <td><asp:DropDownList runat="server" ID="ddlNegocios" AppendDataBoundItems="true" DataTextField="NombreNegocio" DataValueField="IdNegocio" AutoPostBack="true"></asp:DropDownList></td>
                            <td><asp:DropDownList runat="server" ID="ddlAreas" AppendDataBoundItems="true" DataTextField="NombreArea" DataValueField="IdArea" AutoPostBack="true"></asp:DropDownList></td>
                            <td><asp:DropDownList runat="server" ID="ddlAnnos" AutoPostBack="true"></asp:DropDownList></td>
                            <td style="padding-right:30px;"><asp:DropDownList runat="server" ID="ddlMeses" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList></td>
                        </tr>
                    </table><br />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlContenido">
                        <asp:Panel runat="server" ID="pnlCierreInfo" style="background-color:#edeff1;border:1px solid #000000;height:40px;vertical-align:middle;width:65%;">
                            <div style="margin-top:10px;margin-left:40px;">
                                <asp:Label runat="server" ID="lblCerrado" style="font-weight:bold;color:#4666bf;font-size:16px;"></asp:Label>
                                <asp:Button runat="server" ID="btnCerrar" Text="Cerrar indicadores del mes de la planta" style="font-size:16px;" CssClass="boton" />
                            </div>
                        </asp:Panel>
                        <asp:Label runat="server" ID="labelTituloArea" style="font-weight:bold;font-size:16px;"></asp:Label><br />
                        <ajax:TabContainer runat="server" ID="tabDatos" CssClass="Tab" AutoPostBack="true" ScrollBars="Auto" Width="85%">
                            <ajax:TabPanel runat="server" ID="tabEntrada" HeaderText="Valores entrada">
                                <ContentTemplate>
                                    <table style="width:65%;padding-left:15px;padding-top:15px;" class="Repeater">
                                        <tr>
                                           <th><asp:Label runat="server" ID="labelValoresIn" Text="Valores"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelRealIn" Text="Real_KPI"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelPresupIn" Text="Presupuestado"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelUnidadIn" Text="Unidad"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelAreaRespIn" Text="Area responsable"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelAcumRealIn" Text="Acum Real"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelAcumPresupIn" Text="Acum Presup"></asp:Label></th>                                   
                                        </tr>                                
                                            <asp:Repeater runat="server" ID="rptEntrada">                                        
                                               <ItemTemplate>
                                                   <tr>
                                                       <td style="width:10%;">
                                                           <asp:HiddenField runat="server" ID="hfIdValor" />
                                                           <asp:Image runat="server" ID="imgInfo" ImageUrl="~/App_Themes/Tema1/Images/info16.png" style="padding-right:10px;cursor:help;"/>
                                                           <asp:Label runat="server" ID="lblValor"></asp:Label>
                                                       </td>
                                                       <td style="width:1%;" align="right">
                                                           <asp:Label runat="server" ID="lblReal"></asp:Label>
                                                           <asp:TextBox runat="server" ID="txtReal" CssClass="numeric" style="text-align:right"></asp:TextBox>
                                                           <asp:RegularExpressionValidator ID="revReal" Text="No valido" ControlToValidate="txtReal" Runat="server" Display="Dynamic" ValidationExpression="^-?[0-9]+([,\.][0-9]*)?$" ValidationGroup="Save" style="color:red;border:1px solid #FF0000;"></asp:RegularExpressionValidator>                                                   
                                                       </td>
                                                       <td style="width:1%;" align="right">
                                                           <asp:Label runat="server" ID="lblPresup"></asp:Label>
                                                           <asp:TextBox runat="server" ID="txtPresup" CssClass="numeric" style="text-align:right"></asp:TextBox>
                                                           <asp:RegularExpressionValidator ID="revPresup" Text="No valido" ControlToValidate="txtPresup" Runat="server" Display="Dynamic" ValidationExpression="^-?[0-9]+([,\.][0-9]*)?$" ValidationGroup="Save" style="color:red;border:1px solid #FF0000;"></asp:RegularExpressionValidator>
                                                       </td>
                                                       <td style="width:1%;" align="left"><asp:Label runat="server" ID="lblUnidad"></asp:Label></td>
                                                       <td style="width:1%;"><asp:Label runat="server" ID="lblAreaResp"></asp:Label></td>
                                                       <td style="width:1%;" align="right">
                                                           <asp:Label runat="server" ID="lblAcumReal"></asp:Label>
                                                           <asp:TextBox runat="server" ID="txtAcumReal" CssClass="numeric" style="text-align:right"></asp:TextBox>
                                                           <asp:RegularExpressionValidator ID="revAcumReal" Text="No valido" ControlToValidate="txtAcumReal" Runat="server" Display="Dynamic" ValidationExpression="^-?[0-9]+([,\.][0-9]*)?$" ValidationGroup="Save" style="color:red;border:1px solid #FF0000;"></asp:RegularExpressionValidator>
                                                       </td>
                                                       <td style="width:1%;" align="right">
                                                           <asp:Label runat="server" ID="lblAcumPresup"></asp:Label>
                                                           <asp:TextBox runat="server" ID="txtAcumPresup" CssClass="numeric" style="text-align:right"></asp:TextBox>
                                                           <asp:RegularExpressionValidator ID="revAcumPresup" Text="No valido" ControlToValidate="txtAcumPresup" Runat="server" Display="Dynamic" ValidationExpression="^-?[0-9]+([,\.][0-9]*)?$" ValidationGroup="Save" style="color:red;border:1px solid #FF0000;"></asp:RegularExpressionValidator>
                                                       </td>
                                                   </tr>
                                               </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr><td colspan="7">&nbsp;</td></tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td colspan="4"><asp:Button runat="server" ID="btnCalcularKPIs" Text="Calcular KPIs" ValidationGroup="Save" style="width:100%" CssClass="boton" /></td>                                            
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajax:TabPanel>
                            <ajax:TabPanel runat="server" ID="tabSalida" HeaderText="Indicadores">
                                <ContentTemplate>
                                    <table style="width:65%;padding-left:15px;padding-top:15px;" class="Repeater">
                                        <tr>
                                           <th><asp:Label runat="server" ID="labelIndicadoresOut" Text="Indicadores"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelRealOut" Text="Real_KPI"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelPresupOut" Text="Presupuestado"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelUnidadOut" Text="Unidad"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelAcumRealOut" Text="Acum Real"></asp:Label></th>
                                           <th><asp:Label runat="server" ID="labelAcumPresupOut" Text="Acum Presup"></asp:Label></th>                                   
                                        </tr>                                
                                        <asp:Repeater runat="server" ID="rptSalida">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width:10%;">
                                                        <asp:Image runat="server" ID="imgInfo" ImageUrl="~/App_Themes/Tema1/Images/info16.png" style="padding-right:10px;cursor:help;"/>
                                                        <asp:Label runat="server" ID="lblIndicador"></asp:Label>
                                                    </td>
                                                    <td style="width:1%;" align="right"><asp:Label runat="server" ID="lblReal"></asp:Label></td>
                                                    <td style="width:1%;" align="right"><asp:Label runat="server" ID="lblPresup"></asp:Label></td>
                                                    <td style="width:1%;" align="left"><asp:Label runat="server" ID="lblUnidad"></asp:Label></td>
                                                    <td style="width:1%;" align="right"><asp:Label runat="server" ID="lblAcumReal"></asp:Label></td>
                                                    <td style="width:1%;" align="right"><asp:Label runat="server" ID="lblAcumPresup"></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>                                
                                    </table>
                                </ContentTemplate>
                            </ajax:TabPanel>
                        </ajax:TabContainer>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>   
    <uc1:CargandoDatos runat="server"></uc1:CargandoDatos>
</asp:Content>
