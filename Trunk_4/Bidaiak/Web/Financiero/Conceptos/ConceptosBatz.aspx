<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ConceptosBatz.aspx.vb" Inherits="WebRaiz.ConceptosBatz" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">         
    <asp:UpdatePanel runat="server">
        <ContentTemplate> 
            <asp:MultiView runat="server" ID="mvConceptos" ActiveViewIndex="0">
		        <asp:View runat="server" ID="vListado">
                    <asp:Label runat="server" ID="labelInfo" Text="Los conceptos sirven para agrupar los diferentes conceptos provenientes de visas y de Travel Air"></asp:Label><br />
                    <asp:Label runat="server" ID="labelInfo2" Text="* Requiere detalle: Especifica si en algunas pantallas, al seleccionar un concepto otros(p.e), se tendra que introducir un texto explicativo"></asp:Label><br />
                    <asp:Label runat="server" ID="labelInfo3" Text="* Mostrar en Hojas de gastos con recibo: Indica si este concepto se mostrara en las lineas de las hojas de gastos en el desplegable de con recibo"></asp:Label><br />
                    <asp:Label runat="server" ID="labelInfo4" Text="* Mostrar en Hojas de gastos sin recibo: Indica si este concepto se mostrara en las lineas de las hojas de gastos en el desplegable de sin recibo"></asp:Label><br /><br />
                    <asp:LinkButton runat="server" ID="lnbNuevo" Text="Nuevo"></asp:LinkButton><br /><br />            
			        <asp:GridView runat="server" ID="gvConceptos" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">
				        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                        <PagerSettings PageButtonCount="5" />
				        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				        <Columns>
					        <asp:BoundField runat="server" DataField="Id" Visible="false" />					
                            <asp:BoundField runat="server" DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />					
					        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
						        <HeaderTemplate><asp:Label text="Requiere detalle" runat="server"></asp:Label></HeaderTemplate>
						        <ItemTemplate><asp:Checkbox ID="chbRequiereDet" runat="server" Enabled="false" /></ItemTemplate>
					        </asp:TemplateField>							
					        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
						        <HeaderTemplate><asp:Label text="Mostrar en HG con recibo " runat="server"></asp:Label></HeaderTemplate>
						        <ItemTemplate><asp:Checkbox ID="chbMostrarHGRecibo" runat="server" Enabled="false" /></ItemTemplate>
					        </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
						        <HeaderTemplate><asp:Label text="Mostrar en HG sin recibo " runat="server"></asp:Label></HeaderTemplate>
						        <ItemTemplate><asp:Checkbox ID="chbMostrarHGSinRecibo" runat="server" Enabled="false" /></ItemTemplate>
					        </asp:TemplateField>
				        </Columns>
			        </asp:GridView>
			        <br />
		        </asp:View>
		        <asp:View runat="server" ID="vDetalle">			        
                    <asp:Panel runat="server" ID="pnlMensa">
                        <asp:Label runat="server" ID="lblMensaje" CssClass="MensajeError"></asp:Label><br /><br />
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelNombre" text="Nombre"></asp:Label></div>
                        <div class="col-sm-10">
                            <asp:Textbox runat="server" id="txtNombre" MaxLength="75" CssClass="form-control"></asp:Textbox>
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Display="None" ControlToValidate="txtNombre" ValidationGroup="Guardar" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator> 
				            <ajax:ValidatorCalloutExtender runat="Server" ID="vceNombre" TargetControlID="rfvNombre" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbRequiereDet" Text="Requiere detalle" ToolTip="Especifica si en algunas pantallas, al seleccionar un otros(p.e), se tendra que introducir un texto explicativo" /></div>
                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbMostrarHGRecibo" Text="Mostrar en HG con recibo" ToolTip="Indica si este concepto se mostrara en las lineas de las hojas de gastos con recibo" /></div>
                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbMostrarHGSinRecibo" Text="Mostrar en HG sin recibo" ToolTip="Indica si este concepto se mostrara en las lineas de las hojas de gastos sin recibo" /></div>
                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbObsoleto" Text="obsoleto" ToolTip="Marcar como obsoletos" /></div>                                                 
                    </div>
                        </div><br />
                    <div class="row">
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="form-control btn btn-primary" /></div>
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>                        
                    </div>                    
		        </asp:View>
	        </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>