<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Login.aspx.vb" Inherits="IstrikuWebRaiz.Login" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">

	<table class="GridViewASP" style="width:1%; margin:auto;">
		<caption>
			<asp:Label ID="Label3" runat="server" Text="Usuario"></asp:Label></caption>
		<tr class="HeaderStyle">
			<th style="white-space:nowrap;">
				<asp:Label ID="Label1" runat="server" Text="Nº Trabajador"></asp:Label></th>
			<td class="RowStyle">
				<asp:TextBox ID="txtNumTrabajador" runat="server" Width="150px"></asp:TextBox></td>
		</tr>
		<tr class="HeaderStyle">
			<th style="white-space:nowrap;">
				<asp:Label ID="Label2" runat="server" Text="Clave" ToolTip="Clave del 'Portal del Empleado'"></asp:Label></th>
			<td class="RowStyle">
				<asp:TextBox ID="txtClave" runat="server" TextMode="Password" Width="150px"></asp:TextBox></td>
		</tr>
		<tr class="RowStyle">
			<td colspan="2" style="text-align:center;">
				<asp:Panel ID="Panel2" runat="server" CssClass="PanelBotones" >
					<asp:ImageButton ID="imgAceptar" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptar" />
				</asp:Panel>
			</td>
		</tr>
	</table>

	<!------------------------------------------------------------------------------------------------------>
	<!-- Validacion de Campos ------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
	<asp:RequiredFieldValidator ID="rfv_txtNumTrabajador" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtNumTrabajador" Display="None" ValidationGroup="imgAceptar" />
	<act:ValidatorCalloutExtender ID="vce_rfv_txtNumTrabajador" runat="server" TargetControlID="rfv_txtNumTrabajador" />
	<!-- Campo Numerico -->
    <asp:RegularExpressionValidator ID="rev_txtNumTrabajador" ControlToValidate="txtNumTrabajador" ValidationExpression="[0-9]*" Display="None" runat="server" ErrorMessage="Solo enteros" ValidationGroup="imgAceptar" />
    <act:ValidatorCalloutExtender ID="vce_rev_txtNumTrabajador" TargetControlID="rev_txtNumTrabajador" runat="server" />
	<!------------------------------------------------------------------------------------------------------>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>