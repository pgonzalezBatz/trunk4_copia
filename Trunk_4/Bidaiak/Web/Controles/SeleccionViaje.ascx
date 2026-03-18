<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SeleccionViaje.ascx.vb" Inherits="WebRaiz.SeleccionViaje" %>
<asp:UpdatePanel runat="server">
	<ContentTemplate>
		<asp:Panel runat="server" ID="pnlError" CssClass="alert alert-danger">
			<asp:Label runat="server" ID="lblMensaje"></asp:Label>
		</asp:Panel>		
		<asp:GridView runat="server" ID="gvViajes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">		
            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
            <PagerSettings PageButtonCount="5" />
			<EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
			<Columns>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
					<ItemTemplate><asp:LinkButton runat="server" ID="lnkSel" ToolTip="Seleccionar" OnClick="SeleccionarViaje"><i class="glyphicon glyphicon-share-alt"></i></asp:LinkButton></ItemTemplate>                     
				</asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
					<HeaderTemplate><asp:Label runat="server" Text="Id Viaje"></asp:Label></HeaderTemplate>
					<ItemTemplate><asp:Label runat="server" ID="lblIdViaje"></asp:Label></ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate><asp:Label runat="server" Text="Destino"></asp:Label></HeaderTemplate>
					<ItemTemplate><asp:Label runat="server" ID="lblDestino"></asp:Label></ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
					<HeaderTemplate><asp:Label runat="server" Text="Fecha ida"></asp:Label></HeaderTemplate>
					<ItemTemplate><asp:Label runat="server" ID="lblFechaIda"></asp:Label></ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs">
					<HeaderTemplate><asp:Label runat="server" Text="Fecha vuelta"></asp:Label></HeaderTemplate>
					<ItemTemplate><asp:Label runat="server" ID="lblFechaVuelta"></asp:Label></ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate><asp:Label runat="server" Text="Integrantes"></asp:Label></HeaderTemplate>
					<ItemTemplate><asp:Label runat="server" ID="lblIntegrantes"></asp:Label></ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>		
	</ContentTemplate>
</asp:UpdatePanel>
