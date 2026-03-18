<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="DevolucionCancelacion.aspx.vb" Inherits="WebRaiz.DevolucionCancelacion" %>
<%@ Register Src="~/Controles/Importes.ascx" TagName="Importes" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../../js/jQuery/jquery.toastmessage.js"></script>
    <tit:Titulo runat="server" Texto="Devolucion del anticipo entregado de un viaje que ha sido cancelado" /><br />  
    <asp:Label runat="server" ID="labelInformacion" Text="Cuando reciba el dinero del anticipo, seleccione el usuario y pulse el boton de Devolver para que el anticipo quede liquidado"></asp:Label><br /><br />
    <asp:Label runat="server" ID="labelViaje" Text="Viaje"></asp:Label>&nbsp;
    <asp:Label runat="server" ID="lblViaje" CssClass="labelDetalle"></asp:Label><br />
    <asp:Label runat="server" ID="labelUsuario" Text="Usuario de entrega"></asp:Label>&nbsp;
    <asp:DropDownList runat="server" ID="ddlUsuarios" AppendDataBoundItems="true"></asp:DropDownList><br /><br />
    <asp:Label runat="server" ID="labelInfo" Text="Anticipo entregado"></asp:Label><br /><br />
    <uc:Importes runat="server" id="selImportes" Modificable="false" style="margin-top:10px;"></uc:Importes>								
     <table>
        <tr>
            <td><asp:Label runat="server" ID="labelEurSol" Text="Euros solicitados" CssClass="negrita"></asp:Label>:</td>
            <td><asp:Label runat="server" ID="lblEurosSolicitados" CssClass="labelDetalle" style="font-size:15px;"></asp:Label></td>
        </tr>
        <asp:Panel runat="server" ID="pnlEurosEntregados">
            <tr>
                <td><asp:Label runat="server" ID="labelEurEntreg" Text="Euros entregados" CssClass="negrita"></asp:Label>:</td>
                <td><asp:Label runat="server" ID="lblEurosEntregados" CssClass="labelDetalle" style="font-size:15px;"></asp:Label></td>			    
                </tr>
        </asp:Panel>    
    </table><br /><br />    
    <div id="botones">
		<asp:Imagebutton runat="server" ID="imgDevolver" ToolTip="Devolver" ImageUrl="~/App_Themes/Tema1/IconosBotones/Guardar.png" />        		
        <asp:Imagebutton runat="server" ID="imgVolver" ToolTip="Volver" style="margin-left:30px;" ImageUrl="~/App_Themes/Tema1/IconosBotones/Volver.png" />        		
	</div>
</asp:Content>
