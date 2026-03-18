<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DetPresupuestoFactura.aspx.vb" Inherits="WebRaiz.DetPresupuestoFactura" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivViaje" Text="Datos del viaje"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelViaje" Text="Viaje"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblIdViaje" CssClass="label label-info" style="font-size:18px"></asp:Label></b></div>       
                <div class="col-sm-2"><asp:Label runat="server" ID="labelFechas" Text="Fechas"></asp:Label></div>
                <div class="col-sm-4 form-inline">
                    <b>
                        <asp:Label runat="server" ID="lblFIda"></asp:Label>-
                        <asp:Label runat="server" ID="lblFVuelta"></asp:Label>
                    </b>
                </div> 
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelSolicitante" Text="Solicitante"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblSolicitante"></asp:Label></b></div>       
                <div class="col-sm-2"><asp:Label runat="server" ID="labelRespVal" Text="Responsable"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label runat="server" ID="lblRespVal"></asp:Label></b></div>       
            </div>
             <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelViajeros" Text="Viajeros"></asp:Label></div>
                <div class="col-sm-10">
                    <asp:GridView runat="server" ID="gvIntegrantes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		      
		                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                <Columns>                             
                            <asp:TemplateField HeaderText="Integrante">				           
				                <ItemTemplate><asp:Label runat="server" ID="lblIntegrante"></asp:Label></ItemTemplate>
			                </asp:TemplateField>
                            <asp:Templatefield HeaderText="Fechas viaje">
				                <ItemTemplate><asp:Label runat="server" ID="lblFechasViaje"></asp:Label></ItemTemplate>
                            </asp:Templatefield>			                
		                </Columns>        
	                </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivFactura" Text="Datos de la factura"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="row well">                
                <div class="col-sm-2"><b><asp:Label runat="server" ID="labelFacturadoTotal" Text="Facturado total" style="font-size:16px;"></asp:Label></b></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblFacturadoTotal" style="font-size:18px"></asp:Label></b>
                    <asp:Label runat="server" Text="€" style="font-size:15px;"></asp:Label>
                </div>        
            </div><br />
            <asp:GridView runat="server" ID="gvFacturado" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		      
		        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		        <Columns>                             
                    <asp:TemplateField HeaderText="Factura" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">				           
				        <ItemTemplate><asp:Label runat="server" ID="lblFactura"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:Templatefield HeaderText="Fecha servicio">
				        <ItemTemplate><asp:Label runat="server" ID="lblFechaServ"></asp:Label></ItemTemplate>
                    </asp:Templatefield>
                    <asp:Templatefield HeaderText="Persona">
				        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                    </asp:Templatefield>
                    <asp:Templatefield HeaderText="Destino" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
				        <ItemTemplate><asp:Label runat="server" ID="lblDestino"></asp:Label></ItemTemplate>
                    </asp:Templatefield>
                    <asp:Templatefield HeaderText="Producto">
				        <ItemTemplate><asp:Label runat="server" ID="lblProducto"></asp:Label></ItemTemplate>
                    </asp:Templatefield>
                    <asp:Templatefield HeaderText="Proveedor" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
				        <ItemTemplate><asp:Label runat="server" ID="lblProveedor"></asp:Label></ItemTemplate>
                    </asp:Templatefield>
                    <asp:Templatefield HeaderText="Importe">
				        <ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
                    </asp:Templatefield>
		        </Columns>        
	        </asp:GridView>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivPresupuesto" Text="Datos del presupuesto"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelEstado" Text="Estado"></asp:Label></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblEstado" style="font-size:16px"></asp:Label></b>                   
                </div>               
            </div>
             <div class="row well">                
                <div class="col-sm-2"><b><asp:Label runat="server" ID="labelPresupTotal" Text="Presupuesto total" style="font-size:16px;"></asp:Label></b></div>
                <div class="col-sm-4">
                    <b><asp:Label runat="server" ID="lblTotal" style="font-size:18px"></asp:Label></b>
                    <asp:Label runat="server" Text="€" style="font-size:15px;"></asp:Label>
                </div>        
            </div><br />
            <table class="table table-striped table-condensed">
                <tr>
                    <th><asp:Label runat="server" ID="labelAvion" Text="Avion"></asp:Label></th>
                    <th><asp:Label runat="server" ID="labelHotel" Text="Hotel"></asp:Label></th>
                    <th><asp:Label runat="server" ID="LabelCocheAlq" Text="Coche alquiler"></asp:Label></th>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="lblAvion"></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblHotel"></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblCocheAlq"></asp:Label></td>
                </tr>
            </table>
            <div class="row">
                
            </div>
        </div>
    </div>
</asp:Content>
