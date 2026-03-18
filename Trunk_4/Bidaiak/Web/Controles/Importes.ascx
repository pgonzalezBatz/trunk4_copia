<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Importes.ascx.vb" Inherits="WebRaiz.Importes" %>
	<asp:Panel runat="server" ID="pnlModificar" CssClass="form-inline">
		<asp:Label runat="server" ID="labelImp" Text="importe"></asp:Label>
		<asp:TextBox runat="server" ID="txtImporte" Columns="5" CssClass="form-control"></asp:TextBox>	
        <ajax:FilteredTextBoxExtender ID="ftbImporte" runat="server" TargetControlID="txtImporte" FilterType="Numbers,Custom" ValidChars=".," />
		<asp:RequiredFieldValidator ID="rfvImporte" runat="server" Display="None" ControlToValidate="txtImporte" ValidationGroup="AñadirImporte" ErrorMessage="Introduzca el importe a solicitar"></asp:RequiredFieldValidator>		
		<asp:Dropdownlist runat="server" ID="ddlMoneda" CssClass="form-control"></asp:Dropdownlist>		
        <asp:Button runat="server" ID="btnAddImporte" Text="Añadir" CssClass="form-control btn btn-primary" />        
	</asp:Panel>
	<asp:GridView runat="server" ID="gvImportes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		
		<EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		<Columns>				
			<asp:TemplateField  ItemStyle-HorizontalAlign="right">
				<HeaderTemplate><asp:Label runat="server" Text="importe"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
			</asp:TemplateField>	
			<asp:TemplateField>
				<HeaderTemplate><asp:Label runat="server" Text="moneda"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:Label runat="server" ID="lblMoneda" CssClass="text-uppercase"></asp:Label></ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField  ItemStyle-HorizontalAlign="right">
				<HeaderTemplate><asp:Label runat="server" Text="Conversion en euros"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:Label runat="server" ID="lblConversion"></asp:Label></ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">				
                <ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CommandName="Elim" CssClass="form-control btn btn-danger" ToolTip="Eliminar" OnClick="lnkElim_Click"><span aria-hidden="true" class="glyphicon glyphicon-remove-sign"></span></asp:Linkbutton></ItemTemplate>				
			</asp:TemplateField>	
		</Columns>
	</asp:GridView>
	<asp:Panel runat="server" ID="pnlModificar2" CssClass="form-inline">
		<asp:Label runat="server" CssClass="negrita" ID="labelImpEur" Text="El importe en euros asciende a"></asp:Label>
		<b><asp:Label runat="server" ID="lblImporteEuros"></asp:Label></b>
	</asp:Panel>    