<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ResumenLiq.ascx.vb" Inherits="WebRaiz.ResumenLiq" %>
<asp:Panel runat="server" ID="pnlInfoIntegracion">
    <div>
        <h3><asp:Label runat="server" ID="labelInfo" Text="Comprueba las hojas a integrar antes de finalizar"></asp:Label></h3>
    </div><br />
    <div class="row">
        <div class="col-sm-3">                            
            <asp:Label runat="server" ID="labelFEmision" Text="Fecha emision"></asp:Label>
        </div>
        <div class="col-sm-4">
            <div class="input-group date" id="dtFechaEmi">
                <asp:TextBox runat="server" ID="txtFechaEmision" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>         
    </div>      	
    <asp:GridView runat="server" ID="gvLiquidaciones" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="false" CssClass="table tableWithBorder table-striped table-hover" GridLines="None" ShowFooter="true">	        
	        <Columns>   
                <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblImportes"></asp:Label></ItemTemplate>
                </asp:TemplateField>                               
               <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="persona"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                     <FooterTemplate><asp:Label runat="server" ID="labelTotal" text="Total"></asp:Label></FooterTemplate>    
		        </asp:TemplateField>                       
                <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="right">
                    <HeaderTemplate><asp:Label runat="server" Text="Liquidacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblLiquidacion" style="font-size:13px;font-weight:bold;"></asp:Label></ItemTemplate> 
                    <FooterTemplate><asp:Label runat="server" ID="lblTotal" style="font-size:15px;font-weight:bold;"></asp:Label></FooterTemplate>               
		        </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" >
                    <HeaderTemplate><asp:Label runat="server" text="Viajes/Hojas"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Hyperlink runat="server" ID="hlViajeHoja" Target="_blank"></asp:Hyperlink></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblIdUser"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                    <HeaderTemplate><asp:Label runat="server" text="Fecha validacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblFVal"></asp:Label></ItemTemplate>
                </asp:TemplateField>                             
	        </Columns>
        </asp:GridView><br />  
        <div class="form-group">
            <asp:Button runat="server" ID="btnIntegrar" Text="Continuar" CssClass="form-control btn btn-primary" />
        </div> 
</asp:Panel>