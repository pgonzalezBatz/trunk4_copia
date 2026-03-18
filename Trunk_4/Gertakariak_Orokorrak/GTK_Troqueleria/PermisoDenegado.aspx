<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PermisoDenegado.aspx.vb" Inherits="GTK_Troqueleria.PermisoDenegado"  %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<%--<asp:Label ID="Label1" runat="server" Text="permisoDenegado" CssClass="titulo"></asp:Label>--%>
	<table>
		<tr>
			<td>
				<asp:Image ID="imgMArvin" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
			<td>
				<asp:Label ID="lblMensaje" runat="server" Text="permisoDenegado" CssClass="MensajeError"></asp:Label></td>
		</tr>
	</table>
</asp:Content>