<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetMovVisasPopup.aspx.vb" Inherits="WebRaiz.DetMovVisasPopup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div style="height: 300px; overflow: auto;">
            <asp:Panel runat="server" ID="pnlError">
                <asp:Label runat="server" ID="lblMensa" CssClass="MensajeError"></asp:Label>
            </asp:Panel>
            <asp:GridView runat="server" ID="gvInfo" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="GridViewB" ShowFooter="true" Width="900px">
		        <HeaderStyle CssClass="GridViewBHeaderStyle" />
		        <AlternatingRowStyle CssClass="GridViewBAlternatingRowStyle" />
		        <RowStyle CssClass="GridViewBRowStyle" />			       
		        <FooterStyle CssClass="GridViewBFooterStyle" />
		        <Columns>	
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="IdTrabajador"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblNumTrab"></asp:Label></ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Persona"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Fecha"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Concepto"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblConcepto"></asp:Label></ItemTemplate>
                    </asp:TemplateField>                  			    										                					 	
                     <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Localidad"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblLocalidad"></asp:Label></ItemTemplate>                    
                    </asp:TemplateField> 		    						
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Establecimiento"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblEstablecimiento"></asp:Label></ItemTemplate>                    
                    </asp:TemplateField>                 		    	
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <HeaderTemplate><asp:Label runat="server" Text="Importe (€)"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalImporteEur"></asp:Label></FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <HeaderTemplate><asp:Label runat="server" Text="Importe moneda gasto"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblImporteMonGasto"></asp:Label></ItemTemplate>                        
                    </asp:TemplateField>
		        </Columns>
	        </asp:GridView>
        </div>
    </form>
</body>
</html>
