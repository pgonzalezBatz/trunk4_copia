<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="CuentasContables.aspx.vb" Inherits="WebRaiz.CuentasContables" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <ajax:TabContainer runat="server" ID="tabContainer">
                <ajax:TabPanel runat="server" ID="tabListado" HeaderText="Cuentas contables">
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
                                            <asp:Textbox runat="server" id="txtFilter" CssClass="form-control"></asp:Textbox>
                                            <span class="input-group-btn">
                                               <button runat="server" id="btnSearch" class="btn btn-primary" type="button"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></button>
                                            </span>
                                        </div>                              
                                        <asp:Label runat="server" ID="labelInfo" CssClass="help-block" Text="Pagina para relacionar los departamentos con cuentas contables"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:GridView runat="server" ID="gvCuentas" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">		            
                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                            <PagerSettings PageButtonCount="5" />
		                    <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                    <Columns>
                                <asp:BoundField DataField="CodigoDepartamento" HeaderText="Cod dep" SortExpression="CodigoDepartamento" />
                                <asp:BoundField DataField="Departamento" HeaderText="departamento" SortExpression="Departamento" />
                                <asp:BoundField DataField="Cuenta18" HeaderText="Cuenta iva normal" />
                                <asp:BoundField DataField="Cuenta8" HeaderText="Cuenta iva reducido" />
                                <asp:BoundField DataField="Cuenta0" HeaderText="Cuenta exento iva" />                                     
		                    </Columns>        
	                    </asp:GridView>
                        <div id="divModal" class="modal fade" data-keyboard="false">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Cuentas departamento"></asp:Label></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-sm-4"><asp:Label runat="server" ID="labelDepto" Text="Departamento"></asp:Label></div>
                                            <div class="col-sm-8"><b><asp:Label runat="server" ID="lblDpto"></asp:Label></b></div>
                                        </div>  
                                        <div class="row">
                                            <div class="col-sm-4"><asp:Label runat="server" ID="labelCtaNormal" Text="Cuenta iva normal"></asp:Label></div>
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtCuenta18" Columns="20" CssClass="form-control required"></asp:TextBox>
                                                <ajax:FilteredTextBoxExtender ID="ftbCuenta18" runat="server" TargetControlID="txtCuenta18" FilterType="Numbers" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-4"><asp:Label runat="server" ID="labelCtaReducida" Text="Cuenta iva reducido"></asp:Label></div>
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtCuenta8" Columns="20" CssClass="form-control required"></asp:TextBox>
                                                <ajax:FilteredTextBoxExtender ID="ftbCuenta8" runat="server" TargetControlID="txtCuenta8" FilterType="Numbers" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-4"><asp:Label runat="server" ID="labelCtaExenta" Text="Cuenta exento iva"></asp:Label></div>
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtCuenta0" Columns="20" CssClass="form-control required"></asp:TextBox>
                                                <ajax:FilteredTextBoxExtender ID="ftbCuenta0" runat="server" TargetControlID="txtCuenta0" FilterType="Numbers" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <div class="col-sm-6"><asp:Button runat="server" ID="btnGuardarM" Text="Guardar" CssClass="form-control btn btn-primary" /></div>
                                        <div class="col-sm-6"><asp:Button runat="server" ID="btnCerrarM" Text="Cerrar" CssClass="form-control btn btn-default" data-dismiss="modal" /></div>
                                    </div> 
                                </div>
                            </div>
                        </div>                                                                                      
                    </ContentTemplate>
                </ajax:TabPanel>        
                <ajax:TabPanel runat="server" ID="tabSincronizar" HeaderText="Sincronizar">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlSincronizar" GroupingText="sincronizar" CssClass="form-group">
                            <asp:Label runat="server" ID="labelInfo2" Text="Accion para comprobar si se ha modificado o añadido algun departamento" CssClass="help-block" />
                            <asp:Label runat="server" ID="labelInfo3" Text="Se mostraran los departamentos sin registrar y aquellos que alla que eliminar" CssClass="help-block" /><br />                    
                            <asp:Button runat="server" ID="btnSincronizar" Text="refrescar" CssClass="form-control btn btn-primary" />                    
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlSinCambios" GroupingText="resultado">
                            <b><asp:Label runat="server" ID="labelCambioDpto" Text="No ha habido ningun cambio de departamentos"></asp:Label></b>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlEliminados" GroupingText="Departamentos eliminados">                    
                            <b><asp:Label runat="server" ID="labelInfo4" Text="Los siguientes departamentos no se encuentran en el origen, asi que van a ser marcados como obsoletos" /></b><br /><br />
                            <div class="form-group">
                                <asp:Button runat="server" ID="btnEliminar2" Text="Marcar como obsoletos" CssClass="form-control btn btn-primary"/>
                            </div>
                            <asp:GridView runat="server" ID="gvEliminados" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">
		                        <Columns>
                                    <asp:BoundField DataField="CodigoDepartamento" HeaderText="Cod dep" />
                                    <asp:BoundField DataField="Departamento" HeaderText="departamento" />
                                    <asp:BoundField DataField="Cuenta18" HeaderText="Cuenta iva normal" />
                                    <asp:BoundField DataField="Cuenta8" HeaderText="Cuenta iva reducido" />
                                    <asp:BoundField DataField="Cuenta0" HeaderText="Cuenta exento iva" />                                    
		                        </Columns>        
	                        </asp:GridView>
                            <div class="form-group">
                                <asp:Button runat="server" ID="btnEliminar" Text="Marcar como obsoletos" CssClass="form-control btn btn-primary"/>
                            </div><br />
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlAñadidos" GroupingText="Departamentos añadidos">
                            <b><asp:Label runat="server" ID="labelInfo5" Text="Los siguientes departamentos todavia no se han dado de alta en Bidaiak. Introduzca las cuentas contables y sus ofs improductivas asociadas para guardarlas en el sistema. Si todavia no conoce alguna cuenta para el departamento (18%,8%,0%) deje la linea en blanco y no se insertara" /><br /><br />
                            <div class="form-group">
                                <asp:Button runat="server" ID="btnAñadir2" Text="Añadir" CssClass="form-control btn btn-primary" />
                            </div><br />                    
                            <asp:GridView runat="server" ID="gvAñadidos" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		                
		                        <Columns>
                                    <asp:BoundField DataField="CodigoDepartamento" HeaderText="Cod dep" />
                                    <asp:BoundField DataField="Departamento" HeaderText="departamento" />
                                    <asp:TemplateField>
                                        <HeaderTemplate><asp:Label runat="server" Text="Cuenta iva normal"></asp:Label></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtCuenta18" Columns="20" CssClass="form-control required"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftbCuenta18" runat="server" TargetControlID="txtCuenta18" FilterType="Numbers" />
                                        </ItemTemplate>
                                    </asp:TemplateField>   
                                    <asp:TemplateField>
                                        <HeaderTemplate><asp:Label runat="server" Text="Cuenta iva reducido"></asp:Label></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtCuenta8" Columns="20" CssClass="form-control required"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftbCuenta8" runat="server" TargetControlID="txtCuenta8" FilterType="Numbers" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField>
                                        <HeaderTemplate><asp:Label runat="server" Text="Cuenta exento iva"></asp:Label></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtCuenta0" Columns="20" CssClass="form-control required"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftbCuenta0" runat="server" TargetControlID="txtCuenta0" FilterType="Numbers" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                             
		                        </Columns>
	                        </asp:GridView>
                            <div class="form-group">
                                <asp:Button runat="server" ID="btnAñadir" Text="Añadir" CssClass="form-control btn btn-primary" />
                            </div>                    
                        </asp:Panel>
                    </ContentTemplate>        
                </ajax:TabPanel>
            </ajax:TabContainer>
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

        //Chequea que se hayan introducido las 3 cuentas para cada departamento
        function ChequearCuentas() {
            var grid = document.getElementById('<%=gvAñadidos.ClientID %>');
            var cuenta18, cuenta8, cuenta0, ofImprod;
            var check;
            for (var index = 1; index < grid.rows.length; index++) {
                cuenta18 = grid.rows[index].cells[2].children[0].value;
                cuenta8 = grid.rows[index].cells[3].children[0].value;
                cuenta0 = grid.rows[index].cells[4].children[0].value;                
                check = ((cuenta18 != "") || (cuenta8 != "") || (cuenta0 != "") || (ofImprod != ""));
                if (check)  //Si existe alguna cuenta informada, todas tendran que estar informadas                   
                {
                    if ((cuenta18 == "") || (cuenta8 == "") || (cuenta0 == "") || (ofImprod == "")) {
                        alert("Introduzca las 3 cuentas y la of improductiva o no rellene ninguna");
                        return false;
                    }
                }
            }
            return true;
        }
    </script>
</asp:Content>
