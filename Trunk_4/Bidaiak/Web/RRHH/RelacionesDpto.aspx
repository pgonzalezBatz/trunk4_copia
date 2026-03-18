<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="RelacionesDpto.aspx.vb" Inherits="WebRaiz.RelacionesDpto" %>
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
                    <div class="panel-collapse collapse in" id="divCollapse">                           
                        <div class="panel-body lines">
                            <div class="input-group">
                                <asp:Textbox runat="server" id="txtDpto" CssClass="form-control"></asp:Textbox>
                                <span class="input-group-btn">
                                   <button runat="server" id="btnSearch" class="btn btn-primary" type="button"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></button>
                                </span>
                            </div><br />
                            <asp:Checkbox runat="server" ID="cbSinActiv" Text="Sin actividades"></asp:Checkbox> 
                            <asp:Label runat="server" ID="labelSinActiv" CssClass="help-block" Text="Se consideran departamentos sin actividades aquellos que solo tengan asignadas las comunes de otras actividades exentas y no exentas"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlResul">
                <asp:GridView runat="server" ID="gvListado" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None" AllowPaging="true" PageSize="20">
		            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                    <PagerSettings PageButtonCount="5" />
		            <EmptyDataTemplate><br /><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		            <Columns>	                            		
                        <asp:TemplateField>
				            <HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
				            <ItemTemplate>
                                <asp:Label runat="server" ID="lblNumActiv" CssClass="badge" />
                                <asp:LinkButton runat="server" ID="lnkDpto" OnClick="lnkDpto_Click" />                                
				            </ItemTemplate>
			            </asp:TemplateField>		            		                    
                        <asp:TemplateField>
				            <HeaderTemplate><asp:Label runat="server" Text="Con actividad"></asp:Label></HeaderTemplate>
				            <ItemTemplate><asp:Checkbox runat="server" ID="cbConActiv" Enabled="false" /></ItemTemplate>
			            </asp:TemplateField>
		            </Columns>
	            </asp:GridView>	
            </asp:Panel>
            <div id="divModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Actividades departamento"></asp:Label></h4>
                        </div>
                        <div class="modal-body">                            
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelDptoM" Text="Departamento"></asp:Label>
                                <b><asp:Label runat="server" ID="lblDptoM"></asp:Label></b>                                
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelActivRelM"></asp:Label>
                            </div>
                            <ul class="list-group">
                                <asp:Repeater runat="server" ID="rptActivM">
                                    <ItemTemplate>
                                        <li class="list-group-item"><asp:Label runat="server" ID="lblActivM" Text='<%#Eval("Nombre") %>'></asp:Label></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>                            
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-12"><asp:Button runat="server" ID="btnCerrar" data-dismiss="modal" Text="Cerrar" CssClass="form-control btn btn-primary" /></div>                            
                        </div> 
                    </div>
                </div>
            </div> 
		</ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
      <script>
        $(document).ready(function () {            
            $('#<%=txtDpto.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearch.ClientID%>').click();
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            $('#<%=txtDpto.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearch.ClientID%>').click();
            });
        });
    </script>
</asp:Content>