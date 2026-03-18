<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="SaldosCaja.aspx.vb" Inherits="WebRaiz.SaldosCaja" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">        
    <asp:UpdatePanel runat="server">
        <ContentTemplate>   
            <ajax:TabContainer runat="server" ID="tabSaldos" AutoPostBack="true">
                <ajax:TabPanel runat="server" ID="tabOperaciones" HeaderText="Operaciones">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="labelInfo" Text="Se muestran los saldos de cada moneda que estan en caja. Estos saldos se actualizaran automaticamente despues de entregar o recibir un anticipo"></asp:Label><br /><br />
                         <div class="panel-group" id="divAccordionOp">
                            <div class="panel panel-primary">                    
                                <div class="panel-heading" data-toggle="collapse" id="aAccordionOpTitle" style="cursor:pointer" href="#divOpCollapse" data-parent="#divAccordionOp">                                                
                                    <h4 class="panel-title">                            
                                        <span class="glyphicon glyphicon glyphicon-filter"></span>
				                        <asp:Label runat="server" id="labelOpTitle" Text="Operaciones a realizar" CssClass="control-label"></asp:Label>                            
			                        </h4>                              
                                 </div>
                                <div class="panel-collapse collapse" id="divOpCollapse">                           
                                     <div class="panel-body lines">
                                         <div class="row">
                                             <div class="col-sm-2"><asp:Label runat="server" ID="labelOpe" text="Operacion"></asp:Label></div>
                                             <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlOperacion" CssClass="form-control required"></asp:DropDownList></div>
                                         </div>
                                         <div class="row">
                                             <div class="col-sm-2"><asp:Label runat="server" ID="labelCant" text="Cantidad"></asp:Label></div>
                                             <div class="col-sm-2">
                                                 <asp:Textbox runat="server" ID="txtCantidad" MaxLength="8" style="text-align:center" CssClass="form-control required" />
                                                 <ajax:FilteredTextBoxExtender ID="ftbCant" runat="server" TargetControlID="txtCantidad" FilterType="Numbers,Custom" ValidChars=".," />
                                             </div>
                                             <div class="col-sm-2"><asp:DropDownList runat="server" ID="ddlMonedas" CssClass="form-control required"></asp:DropDownList></div>                                             
                                         </div>
                                         <div class="form-group">
                                              <asp:Label runat="server" ID="labelComen" text="Comentarios"></asp:Label><br />
                                              <asp:TextBox runat="server" ID="txtComen" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                                         </div>
                                         <div class="form-group">
                                             <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="form-control btn btn-primary" />
                                         </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:GridView runat="server" ID="gvSaldos" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None">	                        
	                        <EmptyDataTemplate><asp:Label runat="server" Text="No existe ningun registro"></asp:Label></EmptyDataTemplate>
	                        <Columns>                 
                                <asp:TemplateField HeaderText="Moneda">                
			                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkMoneda" OnClick="lnkMoneda_Click"></asp:LinkButton></ItemTemplate>                
		                       </asp:TemplateField>                       
                                <asp:TemplateField HeaderText="Saldo" ItemStyle-HorizontalAlign="Right">                
			                        <ItemTemplate><asp:Label runat="server" ID="lblSaldo" style="margin-right:10px"></asp:Label></ItemTemplate>
                                </asp:TemplateField> 
                            </Columns>
                         </asp:GridView>
                    </ContentTemplate>
                </ajax:TabPanel>
                <ajax:TabPanel runat="server" ID="tabConsulta" HeaderText="Ver movimientos">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="labelInfo2" Text="Se muestra el historico de los movimientos de cada moneda de la caja. La moneda y las fechas son obligatorias informarlas y la operacion optativa"></asp:Label><br /><br />
                        <div class="panel-group" id="divAccordionMov">
                            <div class="panel panel-primary">                    
                                <div class="panel-heading" data-toggle="collapse" id="aAccordionMovTitle" style="cursor:pointer" href="#divMovCollapse" data-parent="#divAccordionMov">                                                
                                    <h4 class="panel-title">                            
                                        <span class="glyphicon glyphicon glyphicon-filter"></span>
				                        <asp:Label runat="server" id="labelMovTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			                        </h4>                              
                                 </div>
                                <div class="panel-collapse collapse" id="divMovCollapse">                           
                                     <div class="panel-body lines">
                                         <div class="row">
                                             <div class="col-sm-2"><asp:Label runat="server" ID="labelCurrencyCons" text="Moneda"></asp:Label></div>
                                             <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlMonedaCons" CssClass="form-control required"></asp:DropDownList></div>
                                         </div>
                                         <div class="row">
                                             <div class="col-sm-2"><asp:Label runat="server" ID="labelFIni" text="FechaInicio"></asp:Label></div>
                                             <div class="col-sm-4">            
                                                <div class="input-group date" id="dtDateIni">
                                                    <asp:TextBox runat="server" ID="txtFechaInicioCons" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                                </div>  
                                             </div>
                                             <div class="col-sm-2"><asp:Label runat="server" ID="labelFFin" text="FechaFin"></asp:Label></div>                                             
                                             <div class="col-sm-4">
                                                 <div class="input-group date" id="dtDateFin">
                                                    <asp:TextBox runat="server" ID="txtFechaFinCons" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                                </div> 
                                             </div>
                                         </div>                                         
                                         <div class="row">
                                             <div class="col-sm-2"><asp:Label runat="server" ID="labelOperationCons" text="Operacion"></asp:Label></div>
                                             <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlOperationCons" CssClass="form-control"></asp:DropDownList></div>
                                         </div>
                                         <div class="form-group">
                                             <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="form-control btn btn-primary" />
                                         </div>
                                    </div>
                                </div>
                            </div>
                        </div>                                  
                        <asp:GridView runat="server" ID="gvMovimientos" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None">	                        
	                        <EmptyDataTemplate><asp:Label runat="server" Text="No existe ningun registro"></asp:Label></EmptyDataTemplate>
	                        <Columns>                 
                                <asp:TemplateField HeaderText="Fecha" ItemStyle-HorizontalAlign="Center">
			                        <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
		                        </asp:TemplateField>
                                <asp:TemplateField HeaderText="Operacion">
			                        <ItemTemplate><asp:Label runat="server" ID="lblOperacion"></asp:Label></ItemTemplate>
		                        </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad" ItemStyle-HorizontalAlign="Right">
			                        <ItemTemplate><asp:Label runat="server" ID="lblCant" style="margin-right:10px"></asp:Label></ItemTemplate>
                                </asp:TemplateField>           
                                <asp:TemplateField HeaderText="Saldo al final" ItemStyle-HorizontalAlign="Right">    
			                        <ItemTemplate><asp:Label runat="server" ID="lblSaldo" style="margin-right:10px"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comentario">
			                        <ItemTemplate><asp:Label runat="server" ID="lblComentario"></asp:Label></ItemTemplate>
		                        </asp:TemplateField>   
                            </Columns>
                         </asp:GridView>
                    </ContentTemplate>
                </ajax:TabPanel>
            </ajax:TabContainer>                                         
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>
