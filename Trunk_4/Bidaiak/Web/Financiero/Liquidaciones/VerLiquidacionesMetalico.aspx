<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="VerLiquidacionesMetalico.aspx.vb" Inherits="WebRaiz.VerLiquidacionesMetalico" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">  
    <div class="form-group">
        <asp:Label runat="server" id="labelMensaje" Text="Se muestran las liquidaciones en metalico emitidas de las hojas de gastos de los trabajadores de Batz"></asp:Label>
    </div>
    <div class="form-inline">
        <asp:Label runat="server" ID="labelSelLiq" Text="Seleccione una emision"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlLiq" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
    </div>   
     <asp:Panel runat="server" ID="pnlInfoLiq">
         <asp:GridView runat="server" ID="gvHojasLiq" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" ShowFooter="true">	        
	        <EmptyDataTemplate><asp:Label runat="server" Text="No existe ninguna hoja de gastos a liquidar"></asp:Label></EmptyDataTemplate>
	        <Columns>                                        
               <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Persona"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                     <FooterTemplate><asp:Label runat="server" ID="labelTotal" text="Total"></asp:Label></FooterTemplate>    
		        </asp:TemplateField>                       
                <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="right">
                    <HeaderTemplate><asp:Label runat="server" Text="Liquidacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblLiquidacion" style="font-size:13px;font-weight:bold;"></asp:Label></ItemTemplate> 
                    <FooterTemplate><asp:Label runat="server" ID="lblTotal" style="font-size:15px;font-weight:bold;"></asp:Label></FooterTemplate>               
		        </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" text="Viajes/Hojas"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Hyperlink runat="server" ID="hlViajeHoja" Target="_blank" style="padding-left:15px;"></asp:Hyperlink></ItemTemplate>
                </asp:TemplateField>            
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" text="Fecha validacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblFVal"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" text="Cuenta contable"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblCuenta"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" Text="Organizacion"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblOrganizacion"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <HeaderTemplate><asp:Label runat="server" Text="Lantegi"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblLantegi"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblImportes"></asp:Label></ItemTemplate>
                </asp:TemplateField>
	        </Columns>
        </asp:GridView><br />  
        <div class="row">
            <div class="col-sm-4"><b><asp:Button runat="server" ID="btnDescargar" Text="Descargar fichero del banco" CssClass="form-control btn btn-primary" /></b></div>            
        </div>   
      </asp:Panel>
</asp:Content>
