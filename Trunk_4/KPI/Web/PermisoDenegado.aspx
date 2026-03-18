<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="PermisoDenegado.aspx.vb" Inherits="WebRaiz.PermisoDenegado" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <table style="border:solid 1px #000000;margin-top:20px;margin-left:20px;" width="1000px;">
		<tr>
			<td colspan="2" style="background-color:#AFBFCD;font-weight:bold;text-align:center;"><asp:Label runat="server" ID="labelTitulo" Text="Permiso denegado" style="font-size:15px;text-transform:uppercase;"></asp:Label></td>
		</tr>
		<tr>
			<td><asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/error_big.gif" /></td>
			<td><asp:Label runat="server" id="lblMensaje" CssClass="MensajeInfoAdvertencia"></asp:Label></td>
		</tr>
	</table>
</asp:Content>