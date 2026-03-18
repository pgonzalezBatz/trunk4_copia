<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ViewImportVisa.aspx.vb" Inherits="WebRaiz.ViewImportVisa" %>
<%@ Register src="Controles/ResumenFinal.ascx" tagname="ResumenFinal" tagprefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:Label runat="server" ID="labelInfo" Text="Resumen de los asientos contables de visas realizados en el mes y año seleccionado y la totalidad de los movimientos que venian en el fichero" CssClass="help-block"></asp:Label>
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
                <ajax:TabPanel runat="server" ID="tabMovimientos" HeaderText="Movimientos">
                    <ContentTemplate>
                        <b><asp:Label runat="server" ID="labelMovVisa" text="Movimientos visa"></asp:Label></b><br />
                        <asp:GridView runat="server" ID="gvMovimientos" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		                    
		                    <EmptyDataTemplate><asp:Label runat="server" Text="No se ha encontrado ningun movimiento de visa para esta importacion"></asp:Label></EmptyDataTemplate>
		                    <Columns>	                                                
                                <asp:BoundField DataField="NombreUsuario" HeaderText="Persona" />
                                <asp:BoundField DataField="NumTarjeta" HeaderText="Tarjeta" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" HtmlEncode="false" />
                                <asp:BoundField DataField="Sector" HeaderText="Sector" />
                                <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" />
                                <asp:BoundField DataField="Localidad" HeaderText="Localidad" />                                                                                                
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                    <HeaderTemplate><asp:Label runat="server" Text="Viaje"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblViaje"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                    <HeaderTemplate><asp:Label runat="server" Text="Cuenta"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblCtaContable"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                        
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblDpto"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                                                                                         
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblImporteMonGasto"></asp:Label></ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Importe (€)"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>
                                </asp:TemplateField> 
		                    </Columns>
	                    </asp:GridView><br /><br />
                        <b><asp:Label runat="server" ID="labelMovVisaExcep" Text="Gastos consumibles (visas excepcion)"></asp:Label></b><br />
                        <asp:GridView runat="server" ID="gvMovimientosExcep" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		                    
		                    <EmptyDataTemplate><asp:Label runat="server" Text="No se ha encontrado ningun movimiento de visa de excepcion para esta importacion"></asp:Label></EmptyDataTemplate>
		                    <Columns>	                                                                                
                                <asp:BoundField DataField="NumTarjeta" HeaderText="Tarjeta" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" HtmlEncode="false" />
                                <asp:BoundField DataField="Sector" HeaderText="Sector" />
                                <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" />
                                <asp:BoundField DataField="Localidad" HeaderText="Localidad" />                                                                                                                                
                                <%--<asp:BoundField DataField="Cuenta" HeaderText="Cuenta" />  
                                <asp:BoundField DataField="Lantegi" HeaderText="Lantegi" />                                  --%>
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblImporteMonGasto"></asp:Label></ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Importe (€)"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>
                                </asp:TemplateField> 
		                    </Columns>
	                    </asp:GridView>
                    </ContentTemplate>
                </ajax:TabPanel>
                <ajax:TabPanel runat="server" ID="tabImportePersona" HeaderText="Importes por persona">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="gvImportesPersona" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped" GridLines="None" Width="65%">		                    
		                    <EmptyDataTemplate><asp:Label runat="server" Text="No se ha encontrado ningun movimiento para agrupar para esta importacion"></asp:Label></EmptyDataTemplate>
		                    <Columns>	
                                <asp:TemplateField>
                                    <HeaderTemplate><asp:Label runat="server" Text="Persona"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                                </asp:TemplateField>                                                             
                                <asp:TemplateField HeaderStyle-CssClass="gridview-header-right" ItemStyle-HorizontalAlign="Right">
                                    <HeaderTemplate><asp:Label runat="server" Text="Importe (€)"></asp:Label></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>
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