<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GridViewAnticipo.ascx.vb" Inherits="WebRaiz.GridViewAnticipo" %>
<asp:GridView runat="server" ID="gvAnticipos" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table tableWithBorder table-striped table-hover" GridLines="None" PageSize="20">
	<PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
    <PagerSettings PageButtonCount="5" />
	<EmptyDataTemplate><asp:Label runat="server" ID="labelSinReg" Text="No existe ningun registro o no cumple ninguna de las condiciones del filtro"></asp:Label></EmptyDataTemplate>
	<Columns>
		<asp:TemplateField SortExpression="IdViaje" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			<HeaderTemplate><asp:LinkButton runat="server" text="Num Viaje" CommandArgument="IdViaje" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblIdViaje" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField SortExpression="Destino">
			<HeaderTemplate><asp:LinkButton runat="server" text="Destino" CommandArgument="Destino" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblDestino" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>  
        <asp:TemplateField SortExpression="Solicitante">
			<HeaderTemplate><asp:LinkButton runat="server" text="Solicitante" CommandArgument="Solicitante" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblSolicitante" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField SortExpression="FechaNecesidad" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			<HeaderTemplate><asp:LinkButton runat="server" text="Fecha requiere" CommandArgument="FechaNecesidad" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblFechaRequiere" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
        <asp:TemplateField SortExpression="Liquidador">
			<HeaderTemplate><asp:LinkButton runat="server" text="Liquidador" CommandArgument="Liquidador" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblLiquidador" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>					
        <asp:TemplateField>
			<HeaderTemplate><asp:Label runat="server" text="Solicitado"></asp:Label></HeaderTemplate>
			<ItemTemplate><asp:Label ID="lblSolicitado" runat="server"></asp:Label></ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>