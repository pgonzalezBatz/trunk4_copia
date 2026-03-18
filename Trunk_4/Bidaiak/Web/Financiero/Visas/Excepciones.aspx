<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Excepciones.aspx.vb" Inherits="WebRaiz.Excepciones" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">	
    <asp:UpdatePanel runat="server">
        <ContentTemplate>                    
            <div class="panel-group" id="divAccordion">
                <div class="panel panel-primary">                    
                    <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                        <h4 class="panel-title">                            
                            <span class="glyphicon glyphicon glyphicon-filter"></span>
				            <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			            </h4>                              
                     </div>
                    <div class="panel-collapse collapse" id="divCollapse">                           
                         <div class="panel-body lines">
                             <div class="input-group">
                                <asp:Textbox runat="server" id="txtFilter" CssClass="form-control"></asp:Textbox>
                                 <ajax:FilteredTextBoxExtender ID="ftbFilter" runat="server" TargetControlID="txtFilter" FilterType="Numbers" />
                                <span class="input-group-btn">
                                   <button runat="server" id="btnSearch" class="btn btn-primary" type="button"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></button>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div> 
            <div class="form-group">
                <asp:Label runat="server" id="labelInfo" Text="Conjunto de visas que no computaran en las hojas de gastos pero si en la contabilidad en la importacion de visas" CssClass="help-block"></asp:Label>
                <asp:Label runat="server" id="labelInfo2" Text="Son excepciones que se añaden desde el proceso de importacion de visas" CssClass="help-block"></asp:Label>
            </div><br />
			<asp:GridView runat="server" ID="gvVisas" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">				
                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />
				<EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				<Columns>								
					<asp:BoundField HeaderText="Num Tarjeta" DataField="NumTarjeta" SortExpression="NumTarjeta" />																		
                    <asp:TemplateField HeaderText="Persona">
                        <ItemTemplate><asp:Label runat="server" ID="lblPersona" SortExpression="Persona"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
				</Columns>
			</asp:GridView>
		    <div id="divModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Tarjeta"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-inline">
                                <asp:Label runat="server" ID="labelNumTarj" Text="Num Tarjeta"></asp:Label>
                                <b><asp:Label runat="server" ID="lblNumTarjeta" CssClass="labelDetalle"></asp:Label></b>
                            </div>                              
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-6"><asp:LinkButton runat="server" ID="lnkQuitarM" CssClass="form-control btn btn-danger" Text="Quitar" data-toggle="modal"></asp:LinkButton></div>
                            <div class="col-sm-6"><asp:Button runat="server" ID="btnCerrarM" Text="Cerrar" CssClass="form-control btn btn-default" data-dismiss="modal" /></div>
                        </div> 
                    </div>
                </div>
            </div>
            <div class="modal fade" id="confirmDelete">
                <div class="modal-dialog modal-confirm">
                    <div class="modal-content">  
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title" id="myModalLabelRep"><asp:Label runat="server" ID="labelConfirmTitle" Text="Confirmar borrado"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:Label runat="server" ID="labelConfirmMessage" Text="confirmarEliminar"></asp:Label>
                        </div>
                        <div class="modal-footer form-inline">
                            <asp:Button runat="server" ID="btnConfirmDelete" Text="Continuar" cssclass="form-control btn btn-primary" /> 
                            <button type="button" class="form-control btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelConfirmCerrar" Text="Cerrar"></asp:Label></button>                                                
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
    <script>
        $(document).ready(function () {            
            $('#<%=txtFilter.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearch.ClientID%>').click();
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            $('#<%=txtFilter.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearch.ClientID%>').click();
            });
        });
    </script>    
</asp:Content>
