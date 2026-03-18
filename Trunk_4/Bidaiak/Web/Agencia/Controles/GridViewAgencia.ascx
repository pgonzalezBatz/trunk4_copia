<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GridViewAgencia.ascx.vb" Inherits="WebRaiz.GridViewAgencia" %>
<asp:GridView runat="server" ID="gvSolicitud" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">	
    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
    <PagerSettings PageButtonCount="5" />
	<EmptyDataTemplate><asp:Label runat="server" ID="labelSinReg" Text="No existe ningun registro o no cumple ninguna de las condiciones del filtro"></asp:Label></EmptyDataTemplate>
	<Columns>
		<asp:TemplateField SortExpression="IdViaje" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			<HeaderTemplate><asp:LinkButton runat="server" text="Num Viaje" CommandArgument="IdViaje" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblIdViaje" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="FechaIda" HeaderStyle-CssClass="gridview-header-center">
			<HeaderTemplate><asp:LinkButton runat="server" text="Fecha ida" CommandArgument="FechaIda" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblFechaIda" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField SortExpression="FechaVuelta" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			<HeaderTemplate><asp:LinkButton runat="server" text="Fecha vuelta" CommandArgument="FechaVuelta" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblFechaVuelta" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField SortExpression="Destino">
			<HeaderTemplate><asp:LinkButton runat="server" text="Destino"  CommandArgument="Destino" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblDestino" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			<HeaderTemplate><asp:Label text="albaran" runat="server"></asp:Label></HeaderTemplate>
			<ItemTemplate><asp:CheckBox runat="server" ID="chbAlbaran" Enabled="false" /></ItemTemplate>
		</asp:TemplateField>        		
        <asp:TemplateField SortExpression="EstadoPresup" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
			<HeaderTemplate><asp:Label text="Estado presupuesto" runat="server"></asp:Label></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblEstadoPresup" runat="server" style="font-size:14px"></asp:Label></ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
