<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="LiquidacionFacturas.aspx.vb" Inherits="WebRaiz.LiquidacionFacturas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
     <script type="text/javascript">
        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null && cb.disabled == false)
                cb.checked = checkState;
        }
        function ChangeAllCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array and updates their value to the checkState input parameter
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }

        function ChangeHeaderAsNeeded() {
            // Whenever a checkbox in the GridView is toggled, we need to check the Header checkbox if ALL of the GridView checkboxes are checked, and uncheck it otherwise
            if (CheckBoxIDs != null) {
                // check to see if all other checkboxes are checked
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (!cb.checked) {
                        // Whoops, there is an unchecked checkbox, make sure that the header checkbox is unchecked
                        ChangeCheckBoxState(CheckBoxIDs[0], false);
                        return;
                    }
                }
                // If we reach here, ALL GridView checkboxes are checked
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }
    </script>
     <asp:UpdatePanel runat="server">
        <ContentTemplate>
        <asp:Timer runat="server" ID="temporizador"></asp:Timer>		
        </ContentTemplate>
    </asp:UpdatePanel>        
    <asp:Panel runat="server" ID="pnlLiquidaciones"> 
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
                            <asp:Textbox runat="server" id="txtSearchHG" CssClass="form-control"></asp:Textbox>
                            <span class="input-group-btn">
                                <button runat="server" id="btnSearch" class="btn btn-primary" type="button"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></button>
                            </span>
                        </div><br />
                        <div class="row">
                            <div class="col-sm-4"><asp:Button runat="server" ID="btnViewLiqPendientes" Text="Liquidaciones pendientes" CssClass="form-control btn btn-primary" /></div>
                            <div class="col-sm-4 col-sm-offset-2"><asp:Button runat="server" ID="btnViewLiqEmitidas" Text="Liquidaciones emitidas" CssClass="form-control btn btn-primary" /></div>
                        </div>                        
                    </div>
                </div>
            </div>
        </div>        
        <asp:Panel runat="server" ID="pnlInfo">
            <b><asp:Label runat="server" ID="lblTextoLiquidacion"></asp:Label></b><br /><br />
            <asp:Panel runat="server" ID="pnlLiqEmitidas">
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelSelPlanta" Text="Planta o empresa"></asp:Label></div>
                    <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlPlantaEmpresa" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></div>
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelSelFactura" text="Seleccione una factura"></asp:Label></div>
                    <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlFactura" AutoPostBack="true" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList></div>
                </div><br /><br />                
            </asp:Panel>
            <asp:GridView runat="server" ID="gvLiquidaciones" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" CssClass="table table-striped table-hover" GridLines="None" ShowFooter="true">	        
	            <EmptyDataTemplate><asp:Label runat="server" Text="No existe ninguna hoja de gastos a liquidar"></asp:Label></EmptyDataTemplate>
	            <Columns>   
                    <asp:TemplateField Visible="false">        
                        <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">        
                        <ItemTemplate><asp:Label runat="server" ID="lblImportes"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField  ItemStyle-HorizontalAlign="Center">               
                        <HeaderTemplate><asp:CheckBox runat="server" ID="chbSelectAll" ToolTip="Seleccionar todos" /></HeaderTemplate>
			            <ItemTemplate><asp:CheckBox runat="server" ID="chbMarcar" /></ItemTemplate>
                        <FooterTemplate></FooterTemplate>
		           </asp:TemplateField>                    
                   <asp:TemplateField SortExpression="Persona">
                        <HeaderTemplate><asp:LinkButton runat="server" Text="persona" CommandName="Sort" CommandArgument="Persona"></asp:LinkButton></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                         <FooterTemplate><asp:Label runat="server" ID="labelTotal" text="Total"></asp:Label></FooterTemplate>    
		            </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                        <HeaderTemplate><asp:Label runat="server" Text="Convenio"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblConvenio"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Categoria"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblCategoria"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="right" SortExpression="Liquidacion">
                        <HeaderTemplate><asp:LinkButton runat="server" Text="Liquidacion" CommandName="Sort" CommandArgument="Liquidacion"></asp:LinkButton></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblLiquidacion" style="font-size:13px;font-weight:bold;"></asp:Label></ItemTemplate> 
                        <FooterTemplate><asp:Label runat="server" ID="lblTotal" style="font-size:15px;font-weight:bold;"></asp:Label></FooterTemplate>               
		            </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" text="Viajes/Hojas"></asp:Label></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblViajeHoja" style="padding-left:15px;"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField Visible="false">        
                        <ItemTemplate><asp:Label runat="server" ID="lblIdUser"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                        <HeaderTemplate><asp:Label runat="server" text="Fecha validacion"></asp:Label></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblFVal"></asp:Label></ItemTemplate>
                    </asp:TemplateField>                
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate><asp:Label runat="server" Text="Excluir"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:ImageButton runat="server" ID="imgExcluir" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/Denegada.png" OnClick="imgExcluir_Click" /></ItemTemplate>
                    </asp:TemplateField>                
	            </Columns>
            </asp:GridView><br />
            <asp:Literal runat="server" ID="CheckBoxIDsArray"></asp:Literal>
            <asp:Panel runat="server" ID="pnlPlantaFact">
                <div class="form-inline">
                    <asp:Label runat="server" ID="labelPlantFact" Text="Planta a la que se le factura" CssClass="form-control"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlPlantFact" CssClass="form-control"></asp:DropDownList>
                    <asp:Label runat="server" ID="labelO" Text="O" CssClass="form-control" style="margin-left:10px;margin-right:10px;"></asp:Label>
                    <asp:Label runat="server" ID="labelOtrasEmpresas" Text="Otras empresas" CssClass="form-control"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlOtrasEmpresas" CssClass="form-control"></asp:DropDownList>
                </div><br />
            </asp:Panel>
            <asp:Panel runat="server" cssClass="row" ID="pnlBotones">              
                <div class="col-sm-3"><asp:Button runat="server" ID="btnContinuar" Text="Continuar" CssClass="form-control btn btn-primary" /></div>
                <div class="col-sm-3"><asp:Button runat="server" ID="btnTransferir" Text="Transferir las hojas de gastos" CssClass="form-control btn btn-success" /></div>
                <div class="col-sm-3"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>            
            </asp:Panel>
        </asp:Panel><br />                 
        <asp:Panel runat="server" ID="pnlSearch">
             <asp:GridView runat="server" ID="gvSearch" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" CssClass="table table-striped table-hover" GridLines="None">	        	            
	            <Columns>
                    <asp:TemplateField HeaderText="Persona">
                        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comentarios">
                        <ItemTemplate><asp:Label runat="server" ID="lblComentarios"></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Accion">
                        <ItemTemplate><asp:Linkbutton runat="server" ID="lnkAccion" Text="Accion" OnClick="lnkAccion_Click"></asp:Linkbutton></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>            
        </asp:Panel>
    </asp:Panel>
     <asp:Panel runat="server" ID="pnlMensaje">
        <div class="alert alert-success"><asp:Label runat="server" ID="labelMensaje"></asp:Label></div><br />       
        <asp:Button runat="server" id="btnVolverLiq" Text="Volver a las liquidaciones" CssClass="form-control btn btn-primary" />
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfHojas" />
    <asp:HiddenField runat="server" ID="hfStatePag" /> 
</asp:Content>