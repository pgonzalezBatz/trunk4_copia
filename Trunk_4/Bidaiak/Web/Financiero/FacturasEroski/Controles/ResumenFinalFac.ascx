<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ResumenFinalFac.ascx.vb" Inherits="WebRaiz.ResumenFinalFac" %>
     <asp:Panel runat="server" ID="pnlCabecera">        
        <b><asp:Label runat="server" ID="lblResul"></asp:Label></b>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlAsientos">
        <asp:Label runat="server" ID="labelInfo1" Text="Se muestra un resumen de los asientos contables que se han realizado en Navision" CssClass="help-block"></asp:Label><br />         
        <asp:Panel runat="server" ID="pnlDatosCab" CssClass="panel panel-primary">            
            <div class="panel-heading">
                <strong><asp:Label runat="server" ID="labelDivCab" Text="Datos de cabecera"></asp:Label></strong>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelNFactura" Text="N. factura"></asp:Label></div>
                    <div class="col-sm-10"><asp:Dropdownlist runat="server" ID="ddlNFactura" AutoPostBack="true" CssClass="form-control"></asp:Dropdownlist></div>
                </div>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelDocBatz" Text="Doc Batz"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblDocBatz"></asp:Label></b></div>
                </div>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelFFact" Text="Fecha factura"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblFFactura"></asp:Label></b></div>
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelFContab" Text="Fecha contabilizacion"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblFContab"></asp:Label></b></div>
                </div>
                <div class="row">                                            
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelFEmision" Text="Fecha emision"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblFEmision"></asp:Label></b></div>                    
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelFVenc" Text="Fecha vencimiento"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblFVenc"></asp:Label></b></div>
                </div>
                <div class="row">                    
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelImporte" Text="Importe Sin IVA"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblImporte"></asp:Label></b></div>
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelIVA" Text="IVA"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblIVA"></asp:Label></b></div>
                </div>
                <div class="row">
                    
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelImporteTotal" Text="Importe total"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblImporteTotal"></asp:Label></b></div>
                </div>
            </div>
        </asp:Panel><br />      
        <asp:GridView runat="server" ID="gvAsientos" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		    
		    <Columns>	
                <asp:TemplateField>                    
                    <HeaderTemplate><asp:Label runat="server" Text="Linea"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblLinea"></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <HeaderTemplate><asp:Label runat="server" Text="Cuenta"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblCuenta"></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblDepartamento"></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Organizacion"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblOrganizacion"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Lantegi"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblLantegi"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Tipo Iva"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblTipoIva"></asp:Label></ItemTemplate>
                </asp:TemplateField>    
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <HeaderTemplate><asp:Label runat="server" Text="Importe Sin IVA"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
                    <FooterTemplate><asp:Label runat="server" ID="lblTotalImporte"></asp:Label></FooterTemplate>
                </asp:TemplateField> 
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <HeaderTemplate><asp:Label ID="lblIva" runat="server" Text="Iva"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblIva"></asp:Label></ItemTemplate>       
                    <FooterTemplate><asp:Label runat="server" ID="lblTotalIva"></asp:Label></FooterTemplate>
                </asp:TemplateField>                    			    										                					                              		    	
		    </Columns>
	    </asp:GridView>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlSinCuenta">            
            <b><asp:Label ID="labelInfo2" runat="server" Text="Los siguientes departamentos, no existen en la estructura de Batz asi que no se han integrado sus asientos"></asp:Label></b><br /><br />
            <asp:GridView runat="server" ID="gvSinCuenta" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" ShowFooter="true">		        
		        <Columns>	
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblDepart"></asp:Label></ItemTemplate>
                    </asp:TemplateField>                  
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
                        <FooterTemplate><asp:Label runat="server" ID="lblTotalImporte"></asp:Label></FooterTemplate>
                    </asp:TemplateField>                  			    										                					                              		    	
		        </Columns>
	        </asp:GridView>
        </asp:Panel>    
    <asp:hiddenField runat="server" ID="hfIdImportacion" />
