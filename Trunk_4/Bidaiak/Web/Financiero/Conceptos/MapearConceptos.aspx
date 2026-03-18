<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="MapearConceptos.aspx.vb" Inherits="WebRaiz.MapearConceptos" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">                  
    <div class="form-group">
        <asp:Label runat="server" ID="labelInfo1" Text="En esta tabla se relacionan lo diversos conceptos provenientes de los ficheros de visas y facturas de agencia, con los conceptos de Batz" CssClass="help-block"></asp:Label>
        <asp:Label runat="server" ID="labelInfo2" Text="Por defecto, al importar un fichero de visas o de facturas de agencia, si el concepto no esta en esta tabla, se relacionara con Desconocido, para que sepa que ese concepto hay que relacionarlo" CssClass="help-block"></asp:Label>
        <asp:Label runat="server" ID="labelInfo3" Text="Al cambiar el concepto de Batz en el desplegable, se guardara automaticamente" CssClass="help-block"></asp:Label>
        <asp:Label runat="server" ID="labelInfo4" Text="Hay algunos conceptos que son muy genericos y contienen conceptos de distinto tipo. Para estos casos, habra que seleccionar el check y acceder a traves del link a una pagina donde se configuren" CssClass="help-block"></asp:Label>
    </div>       
    <asp:UpdatePanel runat="server">
        <ContentTemplate>                               
            <asp:Panel runat="server" ID="pnlSinRelacionar" CssClass="alert alert-warning">
                <b><asp:Label runat="server" id="lblSinAsociar"></asp:Label></b>
            </asp:Panel>            
            <asp:GridView runat="server" ID="gvConceptos" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		        
		        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		        <Columns>
                    <asp:TemplateField ItemStyle-Width="20" ItemStyle-HorizontalAlign="Center">
				        <ItemTemplate><asp:Checkbox ID="chbGenerico" runat="server" AutoPostBack="true" OnCheckedChanged="ChequearConcepto"></asp:Checkbox></ItemTemplate>
			        </asp:TemplateField>				
			        <asp:TemplateField>
				        <HeaderTemplate><asp:Label text="Concepto fichero" runat="server"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:LinkButton ID="lnkConceptoFichero" runat="server" OnClick="lnkConceptoFichero_Click"></asp:LinkButton></ItemTemplate>
			        </asp:TemplateField>							
			        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%">
				        <HeaderTemplate><asp:Label text="Conceptos Batz" runat="server"></asp:Label></HeaderTemplate>
				        <ItemTemplate>
                            <asp:Panel runat="server" ID="pnlSelectConc" style="display:inline;">
					            <asp:Dropdownlist runat="server" ID="ddlConceptoBatz" AutoPostBack="true" DataTextField="Nombre" DataValueField="Id" OnSelectedIndexChanged="SeleccionarConcepto" CssClass="form-control"></asp:Dropdownlist>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlLinkConc" style="display:inline;">
                                <asp:LinkButton runat="server" ID="lnkIr" Text="Ir al detalle" OnClick="lnkIr_Click"></asp:LinkButton>
                            </asp:Panel>                            
                            <asp:Label runat="server" ID="labelGuardado" CssClass="text-primary text-uppercase" Text="Guardado" style="margin-left:15px;" visible="False"></asp:Label>                            
				        </ItemTemplate>                
			        </asp:TemplateField>            
		        </Columns>
	        </asp:GridView>
            <asp:HiddenField runat="server" ID="hfNumSinRel" />
            <div id="divModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                 <asp:Label runat="server" ID="labelInfoM" Text="Se muestran los datos asociados al concepto"></asp:Label>
                            </div>
                           <asp:GridView runat="server" ID="gvDatosConc" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" />
                                    <asp:BoundField DataField="Localidad" HeaderText="Localidad" />
                                    <asp:BoundField DataField="NombreUsuario" HeaderText="Persona" />
		                        </Columns>
	                        </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-12"><asp:Button runat="server" ID="btnCerrarM" Text="Cerrar" CssClass="form-control btn btn-default" data-dismiss="modal" /></div>
                        </div> 
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
    <uc:CargandoDatos runat="server" />
</asp:Content>
