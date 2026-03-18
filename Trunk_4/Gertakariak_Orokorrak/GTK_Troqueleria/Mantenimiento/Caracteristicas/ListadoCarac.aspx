<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ListadoCarac.aspx.vb" Inherits="GTK_Troqueleria.ListadoCarac" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		/*********************************************************************************************/
		/* Menu del TreeView                                                                         */
		/*********************************************************************************************/
		var PosX, PosY;
		function pageLoad() {
			$(document).ready(function () {
				$("#<%= dlEstructuras.ClientID%>").click(function (e) {PosX = e.pageX; PosY = e.pageY;});
				$(document).click(function (event) {Cerrar_pnlBotones();});
			})
		}
		function NodoSeleccionado(ID)
		{
			var pnlBotones = $get("<%= pnlBotones.ClientID%>");
			var hfIdNodo = $get("<%= hfIdNodo.ClientID%>");
			hfIdNodo.value = ID;
			pnlBotones.style.visibility = 'visible';
			pnlBotones.style.top = (PosY - 5) + "px";
			pnlBotones.style.left = (PosX + 20) + "px";
		}
		function Cerrar_pnlBotones()
		{
			var pnlBotones = $get("<%= pnlBotones.ClientID%>");
			pnlBotones.style.visibility = 'hidden';
		}
		/*********************************************************************************************/
	</script>
</asp:Content>
	    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<asp:DataList ID="dlEstructuras" runat="server" GridLines="None" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4" Width="96%" >
		<ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
		<ItemTemplate>
			<asp:TreeView ID="tvEstructura" runat="server" SkinID="TreeView" ViewStateMode="Disabled">
			</asp:TreeView>
		</ItemTemplate>
	</asp:DataList>
		
	<asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones" Style="visibility:hidden; position:absolute; left:0px; top:0px; ">
		<asp:HiddenField ID="hfIdNodo" runat="server" />
		<asp:ImageButton ID="btnNuevoSub" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo16.png" AlternateText="Nuevo Subelemento" ToolTip="Nuevo Subelemento" />	
		<asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar16.png" AlternateText="Editar" ToolTip="Editar" />
		<asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
		<act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" Enabled="True" OnClientCancel="Cerrar_pnlBotones"/>
	</asp:Panel>
</asp:Content>