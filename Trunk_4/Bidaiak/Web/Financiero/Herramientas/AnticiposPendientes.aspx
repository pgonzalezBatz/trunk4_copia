<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="AnticiposPendientes.aspx.vb" Inherits="WebRaiz.AnticiposPendientes" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
     <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Timer runat="server" ID="temporizador"></asp:Timer>		
        </ContentTemplate>
    </asp:UpdatePanel>     
    <asp:Label runat="server" ID="labelInfo" Text="Se muestran todos aquellos anticipos que a dia de hoy no han sido justificados"></asp:Label><br /><br />
    <asp:panel runat="server" ID="pnlResultado">
        <div class="form-group">
            <asp:Label runat="server" ID="labelResul" Text="Registros encontrados"></asp:Label>
            <b><asp:Label runat="server" ID="lblReg"></asp:Label></b>
        </div>
        <asp:GridView runat="server" ID="gvAnticPend" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" ShowFooter="true">	        
	        <EmptyDataTemplate><asp:Label runat="server" Text="No existe ningun registro"></asp:Label></EmptyDataTemplate>
	        <Columns>                 
               <asp:TemplateField HeaderText="Persona">                
			        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>                
		        </asp:TemplateField>                       
                <asp:TemplateField HeaderText="Viaje">                
			        <ItemTemplate><asp:Hyperlink runat="server" ID="hlViaje" Target="_blank"></asp:Hyperlink></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Fechas viaje">                
			        <ItemTemplate><asp:Label runat="server" ID="lblFViaje"></asp:Label></ItemTemplate>
                </asp:TemplateField>                 
                <asp:TemplateField HeaderText="Estado hoja" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" FooterStyle-CssClass="hidden-xs">                
			        <ItemTemplate><asp:Label runat="server" ID="lblEstadoHoja"></asp:Label></ItemTemplate>
                    <FooterTemplate><asp:Label runat="server" ID="labelTotal" text="Total"></asp:Label></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" HeaderText="Pendiente justificar">                
			        <ItemTemplate><b><asp:Label runat="server" ID="lblPendiente" style="font-size:13px;"></asp:Label>&nbsp;€</b></ItemTemplate> 
                    <FooterTemplate><b><asp:Label runat="server" ID="lblTotal" style="font-size:15px;"></asp:Label></b></FooterTemplate>
		        </asp:TemplateField>                                    
	        </Columns>
        </asp:GridView>
    </asp:panel>
</asp:Content>
