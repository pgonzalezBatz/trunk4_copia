<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ConfigCalculo.aspx.vb" Inherits="WebRaiz.ConfigCalculo" %>
<%@ Register src="~/Controles/PanelCargandoDatos.ascx" tagname="CargandoDatos" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            //Permite el punto, la coma, las fechas, el borrado, numeros, operaciones + - * / y parentesis
            $('.formula').keypress(function (e) {
                var verified = (e.which == 8 || e.which == undefined || e.which == 0 || e.which == 44 || e.which == 46 || e.which == 40 || e.which == 41 || e.which == 42 || e.which == 43 || e.which == 45 || e.which == 47) ? null : String.fromCharCode(e.which).match(/[^0-9]/);
                if (verified) { e.preventDefault(); }
            });
        });
    </script>
    <tit:Titulo runat="server" Texto="Configuracion del calculo" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td><asp:Label runat="server" ID="labelInd" Text="Indicador"></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblIndicador" style="font-weight:bold;"></asp:Label></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="labelDescr" Text="Descripcion"></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblDescripcion" style="font-weight:bold;"></asp:Label></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="labelNegocio" Text="Negocio"></asp:Label></td>
                    <td>
                        <asp:Label runat="server" ID="lblNegocio" style="font-weight:bold;"></asp:Label>
                        <asp:HiddenField runat="server" ID="hfIdNegocio" />
                    </td>
                </tr>
            </table><br />
            <asp:Panel runat="server" ID="pnlFormula" GroupingText="Formula">                   
                <br /><table id="detalle">
                    <tr>
                        <th><asp:Label runat="server" ID="labelTexto" Text="Texto"></asp:Label></th>
                        <th><asp:Label runat="server" ID="labelValInd" Text="Valores/Indicadores"></asp:Label></th>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox runat="server" ID="txtTexto" Width="80%" CssClass="formula"></asp:TextBox><br />
                            <asp:Label runat="server" ID="labelCharAdmit" style="font-size:9px;font-style:italic;" Text="Caracteres admitidos"></asp:Label>
                            <asp:Label runat="server" ID="labelCharAdmit2" style="font-size:9px;font-style:italic;" Text="0-9 + - * / , . ( )"></asp:Label>
                        </td>
                        <td>
                            <table> 
                                <tr>
                                    <td><asp:Label runat="server" ID="labelNegocioForm" Text="Negocio"></asp:Label></td>
                                    <td><asp:DropDownList runat="server" id="ddlNegocio" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" ID="labelArea" Text="Area"></asp:Label></td>
                                    <td><asp:DropDownList runat="server" id="ddlAreas" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" ID="labelValor" Text="Valor"></asp:Label></td>
                                    <td><asp:DropDownList runat="server" id="ddlValores" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" ID="labelIndicador" Text="Indicador"></asp:Label></td>
                                    <td><asp:DropDownList runat="server" id="ddlIndicador" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id"></asp:DropDownList></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:25px;"><asp:Button runat="server" ID="btnAddTexto" Text="Añadir" CssClass="boton" /></td>
                        <td style="padding-left:25px;"><asp:Button runat="server" ID="btnAddValInd" Text="Añadir" CssClass="boton" /></td>
                    </tr>
                </table><br /><br />
                <asp:Panel ID="pnlCalculo" runat="server" style="background: #F3F3F3;font-size:15px;font-weight:bold;height:50px;border:1px solid #000000;width:70%" /><br />
                <asp:LinkButton runat="server" ID="lnkLimpiar" Text="Limpiar"></asp:LinkButton>
            </asp:Panel><br />           
            <div id="botones">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" />
                <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="boton" style="margin-left:20px" />
            </div>
         </ContentTemplate>
        </asp:UpdatePanel>
    <uc1:CargandoDatos ID="CargandoDatos1" runat="server"></uc1:CargandoDatos>
</asp:Content>