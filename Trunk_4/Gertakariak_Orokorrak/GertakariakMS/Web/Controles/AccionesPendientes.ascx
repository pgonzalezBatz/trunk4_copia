<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AccionesPendientes.ascx.vb" Inherits="GertakariakMSWeb_Raiz.AccionesPendientes" %>
<style type="text/css">
	.gvAccionesDia
	{
		font-size: xx-small;
		width: 1px;
		border-collapse: collapse;
		border-width: thin;
		border-style: solid;
		border-color: Black;
		text-align: center;
	}
	.Izquierda
	{
		text-align:left;
	}
</style>
<asp:Table ID="gvAccionesDia" runat="server" CssClass="gvAccionesDia" >
	<asp:TableHeaderRow CssClass="HeaderStyle">
		<%--<asp:TableCell RowSpan="2" HorizontalAlign="Right">
			<asp:ImageButton ID="imgAnterior" runat="server" AlternateText="Atras" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Atras.png" ToolTip="Atras" Width="60%" />
		</asp:TableCell>--%>
		<asp:TableCell CssClass="CommandRowStyleDetailsView" Wrap="false" ToolTip="Semana">
			<%--<asp:Label ID="abrSemana" runat="server" Text="s" />--%><%--<br />--%>
		</asp:TableCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblLunes" runat="server" Text="L" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblMartes" runat="server" Text="M" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblMiercoles" runat="server" Text="M" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblJueves" runat="server" Text="J" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblViernes" runat="server" Text="V" />
		</asp:TableHeaderCell>
		<asp:TableCell CssClass="CommandRowStyleDetailsView" Wrap="false" ToolTip="Semana">
			<%--<asp:Label ID="Label1" runat="server" Text="s" />--%><%--<br />--%>
		</asp:TableCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblLunes2" runat="server" Text="L" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblMartes2" runat="server" Text="M" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblMiercoles2" runat="server" Text="M" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblJueves2" runat="server" Text="J" />
		</asp:TableHeaderCell>
		<asp:TableHeaderCell>
			<asp:Label ID="lblViernes2" runat="server" Text="V" />
		</asp:TableHeaderCell>
		<%--<asp:TableCell RowSpan="2" Width="0px" HorizontalAlign="Left" >
			<asp:ImageButton ID="imgSiguiente" runat="server" AlternateText="Siguiente" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Siguiente.png" ToolTip="Siguiente" Width="60%" />
		</asp:TableCell>--%>
		<asp:TableCell CssClass="CommandRowStyleDetailsView" Wrap="false">
		</asp:TableCell>
	</asp:TableHeaderRow>
	<asp:TableRow CssClass="AlternatingRowStyle">
		<asp:TableCell CssClass="CommandRowStyleDetailsView" Wrap="false" ToolTip="Semana">
			<asp:Label ID="lblNumSemana" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesLunes" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMartes" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMiercoles" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesJueves" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesViernes" runat="server" />
		</asp:TableCell>
		<asp:TableCell CssClass="CommandRowStyleDetailsView" Wrap="false" ToolTip="Semana">
			<asp:Label ID="lblNumSemana2" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesLunes2" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMartes2" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMiercoles2" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesJueves2" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesViernes2" runat="server" />
		</asp:TableCell>
		<asp:TableCell CssClass="CommandRowStyleDetailsView Izquierda" Wrap="false" HorizontalAlign="Left">
			<asp:Label ID="lblTotales" runat="server" Text=":Totales" ToolTip="Acciones Totales por día." />
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow CssClass="AlternatingRowStyle">
		<asp:TableCell CssClass="CommandRowStyleDetailsView" Wrap="false" ToolTip="Semana"/>
		<asp:TableCell>
			<asp:Label ID="lblAccionesLunesP" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMartesP" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMiercolesP" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesJuevesP" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesViernesP" runat="server" />
		</asp:TableCell>
		<asp:TableCell CssClass="CommandRowStyleDetailsView" Wrap="false" ToolTip="Semana"/>
		<asp:TableCell>
			<asp:Label ID="lblAccionesLunes2P" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMartes2P" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesMiercoles2P" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesJueves2P" runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblAccionesViernes2P" runat="server" />
		</asp:TableCell>
		<asp:TableCell CssClass="CommandRowStyleDetailsView Izquierda" Wrap="false">
			<asp:Label ID="Label13" runat="server" Text=":Parciales" ToolTip="Acciones Parciales por día." />
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>