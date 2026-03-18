<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="HistoricoVisas.aspx.vb" Inherits="WebRaiz.HistoricoVisas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
     <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Timer runat="server" ID="temporizador"></asp:Timer>		
        </ContentTemplate>
    </asp:UpdatePanel>     
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
                         <div class="panel-body">                            
                            <div class="form-group">
                                <asp:RadioButton runat="server" ID="rbtPorFechas" GroupName="Opciones" Text="Seleccione el mes y año del que quiere ver los gastos de visa a validar" />    
                            </div>
                            <div class="form-inline">                                                                     
                                <asp:DropDownList runat="server" ID="ddlMes" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlAño" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                            </div>
                             <div class="form-group">
                                 <asp:RadioButton runat="server" ID="rbtVerTodos" RepeatDirection="Horizontal" GroupName="Opciones" Text="Ver movimientos ultimos 3 meses" CellPadding="30" CellSpacing="30"></asp:RadioButton>
                             </div>                            
                             <div class="form-group">
                                 <asp:RadioButton runat="server" ID="rbtSinJustificar" RepeatDirection="Horizontal" GroupName="Opciones" Text="Ver movimientos sin justificar" CellPadding="30" CellSpacing="30"></asp:RadioButton>
                             </div>                            
                            <div class="form-group">
                                <asp:Button runat="server" ID="btnSearchF" Text="Buscar" CssClass="form-control btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>           
             <asp:Panel runat="server" ID="pnlJustificar" CssClass="row hidden-xs">
                <div class="col-sm-3"><asp:Button runat="server" ID="btnJustificar" Text="Justificar gastos" CssClass="form-control btn btn-primary" /></div>
                <div class="col-sm-9"><asp:Label runat="server" ID="labelInfo" Text="Al ponerse encima del gasto, se muestran los comentarios" CssClass="help-block"></asp:Label></div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlJust1" CssClass="row hidden-xs">
                <div class="col-sm-3"><asp:Button runat="server" ID="btnGuardar1" Text="Guardar observaciones" style="font-size:16px;" CssClass="form-control btn btn-primary" ValidationGroup="GuardarJustif" /></div>
                <div class="col-sm-3"><asp:Button runat="server" ID="btnCancelar1" Text="Cancelar" style="font-size:16px;" CssClass="form-control btn btn-default" /></div>
            </asp:Panel>             
           <asp:GridView runat="server" ID="gvVisa" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" CssClass="table table-striped table-condensed table-hover" GridLines="None" PageSize="20" DataKeyNames="Id,Estado">	            
                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />
	            <EmptyDataTemplate><asp:Label runat="server" Text="No existe ningun gasto de visa la fecha seleccionada"></asp:Label></EmptyDataTemplate>
	            <Columns>   
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="visible-xs" HeaderStyle-CssClass="gridview-header-center visible-xs">
                        <ItemTemplate><asp:LinkButton runat="server" ID="lnkJustif" OnClick="lnkJustif_Click" CommandName="Sel" ToolTip="Justificar"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton></ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="Fecha">               
                        <HeaderTemplate><asp:LinkButton runat="server" text="Fecha" CommandName="Sort" CommandArgument="Fecha"></asp:LinkButton></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
		            </asp:TemplateField>             
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" text="Viaje/Hoja"></asp:Label></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblTipo" style="font-size:14px;"></asp:Label></ItemTemplate>
		            </asp:TemplateField> 
                    <asp:BoundField DataField="NumTarjeta" HeaderText="Tarjeta" SortExpression="NumTarjeta" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs" />
                    <asp:BoundField DataField="Sector" HeaderText="Sector" SortExpression="Sector" />
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" text="Observaciones"></asp:Label></HeaderTemplate>
			            <ItemTemplate>				    
                            <asp:TextBox runat="server" ID="txtJustificacion" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revJustifi" runat="server" ControlToValidate="txtJustificacion" ValidationGroup="GuardarJustif" Display="None" ErrorMessage="300 caracteres maximo" ValidationExpression="[\s\S]{0,300}"></asp:RegularExpressionValidator>                            
			            </ItemTemplate>
		            </asp:TemplateField> 
                    <asp:BoundField DataField="Localidad" HeaderText="Localidad" SortExpression="Localidad" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
                    <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" SortExpression="Establecimiento" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
                     <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" Text="Importe (€)"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                        <HeaderTemplate><asp:Label runat="server" Text="Importe moneda gasto"></asp:Label></HeaderTemplate>
                        <ItemTemplate><asp:Label runat="server" ID="lblImporteMonGasto"></asp:Label></ItemTemplate>                        
                    </asp:TemplateField>                     
		           <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate><asp:Label runat="server" text="estado"></asp:Label></HeaderTemplate>
			            <ItemTemplate>
				            <asp:Image runat="server" id="imgEstado"></asp:Image>
                            <b><asp:Label runat="server" ID="labelSinJustificar" Text="Sin justificar" CssClass="label label-warning" style="font-size:14px" Visible="false"></asp:Label></b>
			            </ItemTemplate>
		            </asp:TemplateField>
	            </Columns>
            </asp:GridView>
             <asp:Panel runat="server" ID="pnlAvisarJustif" CssClass="form-group visible-xs">
                <asp:Button runat="server" ID="btnAvisarJustif" Text="Avisar al responsable de las justificaciones" CssClass="form-control btn btn-primary" /></div>                
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hfOpcionSel" />
            <asp:Panel runat="server" ID="pnlJust2" CssClass="row hidden-xs">
                <div class="col-sm-3"><asp:Button runat="server" ID="btnGuardar2" Text="Guardar observaciones" style="font-size:16px;" CssClass="form-control btn btn-primary" ValidationGroup="GuardarJustif" /></div>
                <div class="col-sm-3"><asp:Button runat="server" ID="btnCancelar2" Text="Cancelar" style="font-size:16px;" CssClass="form-control btn btn-default" /></div>
            </asp:Panel>            
            <div id="divModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Justificacion de gasto"></asp:Label></h4>                        
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelTarjetaM" Text="Tarjeta"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblTarjetaM"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelFechaM" Text="Fecha"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblFechaM"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelViajeHojaM" Text="Viaje/Hoja"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblViajeHojaM" style="font-size:14px"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelLocalidad" Text="Localidad"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblLocalidadM"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelSectorM" Text="Sector"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblSectorM"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelEstablecimientoM" Text="Establecimiento"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblEstablecimientoM"></asp:Label></b></div>
                            </div>                            
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelImporteMonGastoM" Text="Importe"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblImporteMonGastoM"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelImporteEurM" Text="Importe euros"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblImporteEurM"></asp:Label></b></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Label runat="server" ID="labelEstadoM" Text="Estado"></asp:Label></div>
                                <div class="col-sm-9"><b><asp:Label runat="server" ID="lblEstadoM" style="font-size:14px"></asp:Label></b></div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelObservacionesM" Text="Observaciones"></asp:Label>
                                <asp:TextBox runat="server" ID="txtObservacionesM" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">                            
                            <asp:Button runat="server" ID="btnJustificarModal" Text="Justificar" cssclass="btn btn-success" /> 
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModal" Text="Cancelar"></asp:Label></button>                            
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>        
    </asp:UpdatePanel>      
    <uc:CargandoDatos runat="server" />
    <script language="javascript" type="text/javascript">

        //Se habilita o deshabilita los controles de los radiobutton
        //Si es 0, se habilitara la busqueda por mes y año
        //Si es 1, se habilitara la busqueda por movimientos sin validar
        function ChangeRadio(check) {
            document.getElementById('<%=rbtPorFechas.ClientID %>').checked = (check == 0);
            document.getElementById('<%=rbtSinJustificar.ClientID %>').checked = (check == 1);
            document.getElementById('<%=rbtVerTodos.ClientID %>').checked = (check == 2);
            document.getElementById('<%=ddlMes.ClientID %>').disabled = (check == 1 || check==2);
            document.getElementById('<%=ddlAño.ClientID %>').disabled = (check == 1 || check==2);
            document.getElementById('<%=hfOpcionSel.ClientID %>').value = check;
        }

        //Se repinta en cada postback
        ChangeRadio(document.getElementById('<%=hfOpcionSel.ClientID %>').value);        
    </script>
</asp:Content>
