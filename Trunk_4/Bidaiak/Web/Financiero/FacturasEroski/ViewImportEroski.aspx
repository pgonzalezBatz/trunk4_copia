<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ViewImportEroski.aspx.vb" Inherits="WebRaiz.ViewImportEroski" %>
<%@ Register src="Controles/ResumenFinalFac.ascx" tagname="ResumenFinal" tagprefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:Label runat="server" ID="labelInfo" Text="Resumen de los asientos contables de facturas Eroski realizados en el mes y año seleccionado y la totalidad de los albaranes que venian en el fichero" CssClass="help-block"></asp:Label>
    <div class="form-inline">
        <asp:Label runat="server" ID="labelFecha" Text="Fecha del fichero"></asp:Label>:
        <b><asp:Label runat="server" ID="lblFechaFichero" CssClass="text-uppercase" style="font-size:18px"></asp:Label></b>
        <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="btn btn-primary forceInline" />
    </div>    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <ajax:TabContainer runat="server" ID="tabPaneles" AutoPostBack="true">
                <ajax:TabPanel runat="server" ID="tabAsientos" HeaderText="Asientos contables">
                    <ContentTemplate>
                        <uc:ResumenFinal ID="resumen" runat="server" Modo="View" />
                    </ContentTemplate>
                </ajax:TabPanel>
                <ajax:TabPanel runat="server" ID="tabAlbaranes" HeaderText="Albaranes">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="gvAlbaranes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		                    
		                    <EmptyDataTemplate><asp:Label runat="server" Text="No se ha encontrado ningun albaran de la factura de Eroski para esta importacion"></asp:Label></EmptyDataTemplate>
		                    <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Factura"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblFactura"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Albaran"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblAlbaran"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Fecha"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Persona"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Producto"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblProducto"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Destino"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblDestino"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                                           
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Proveedor"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblProveedor"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                                           
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Base IG"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblBaseIG"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                                           
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Cuota G"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblCuotaG"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Base IR"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblBaseIR"></asp:Label></ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Cuota R"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblCuotaR"></asp:Label></ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Base Exe"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblBaseExe"></asp:Label></ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Reg Esp"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblRegEsp"></asp:Label></ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Cuota RE"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblCuotaRE"></asp:Label></ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Tasas"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblTasas"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Cta Iva normal"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblCtaIvaNormal"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                        
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Cta Iva reducido"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblCtaIvaReducido"></asp:Label></ItemTemplate>
                                </asp:TemplateField> 
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Cta Iva Exento"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblCtaIvaExento"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                       
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblDpto"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                                                                                          
		                    </Columns>
	                    </asp:GridView>
                    </ContentTemplate>
                </ajax:TabPanel>
            </ajax:TabContainer> 
            <asp:HiddenField runat="server" id="hfIdImportacion" />
      </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
    <script>
        Sys.Application.add_load(initPage);        
        function initPage() {
            $(".container").attr('class', 'container-fluid');
        }
    </script>
</asp:Content>