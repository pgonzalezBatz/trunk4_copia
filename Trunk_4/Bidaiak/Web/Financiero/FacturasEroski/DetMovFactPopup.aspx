<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetMovFactPopup.aspx.vb" Inherits="WebRaiz.DetMovFacPopup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div style="height: 150px; overflow: auto;">   
            <asp:Panel runat="server" ID="pnlError">
                <asp:Label runat="server" ID="lblMensa" CssClass="MensajeError"></asp:Label>
            </asp:Panel>
            <asp:Label runat="server" ID="lblDeptCuenta" CssClass="labelDetalle"></asp:Label><br />
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
                        <HeaderTemplate><asp:Label runat="server" Text="producto"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblProducto"></asp:Label></ItemTemplate>
                    </asp:TemplateField>                  			    										                					
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Base IG"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblBase18"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalBase18"></asp:Label></FooterTemplate>
                    </asp:TemplateField> 	
                     <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Cuota G"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblCuota18"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalCuota18"></asp:Label></FooterTemplate>
                    </asp:TemplateField> 		    						
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Base IR"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblBase8"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalBase8"></asp:Label></FooterTemplate>
                    </asp:TemplateField> 
                     <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Cuota R"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblCuota8"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalCuota8"></asp:Label></FooterTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Base Exe"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblBase0"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalBase0"></asp:Label></FooterTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Reg Esp"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblRegEsp"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalRegEsp"></asp:Label></FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Cuota RE"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblCuota0"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalCuota0"></asp:Label></FooterTemplate>
                    </asp:TemplateField> 	               		    	
		        </Columns>
	        </asp:GridView>
        </div>
    </form>
</body>
</html>
