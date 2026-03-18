<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ConversionMonedas.aspx.vb" Inherits="WebRaiz.ConversionMonedas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">	
	<script type="text/javascript">
		//Convierte el valor del importe introducido al resto de monedas
		//1ºConvierte a euros el valor del importe por una regla de 3
		//txtImporte/Rate Moneda seleccionada=Valor en euros
		//Para cada moneda de la tabla, se obtiene su valor que sera Valor en euros x Rate de la moneda
		function ConversionCantidad() {
			try {                                
				var cantidad = document.getElementById('<%=txtCantidad.ClientID %>').value.replace(",",".");
				var monedaSel = document.getElementById('<%=ddlMonedas.ClientID %>').value;
				var gvMonedas = document.getElementById('<%=gvConversiones.ClientID %>');				
				var valorEuros = 0;
				//Se obtiene la info de la moneda seleccionada para calcular su correspondiente valor en euros
				var monedaDesplegable = getInfoMoneda(monedaSel);                
				if (monedaDesplegable != null) {                    
					valorEuros = parseFloat(cantidad / parseFloat(monedaDesplegable[1].replace(",", "."))).toFixed(0);                    
				}
				else 
					throw new Error();														
				//Nos recorremos el gridview y para cada moneda, se asigna el valor en su moneda correspondiente
				var idMonedaTabla, rateMonedaTabla, conversion;                				
				for (var fila = 0; fila < gvMonedas.rows.length; fila++) {
					if (fila > 0) {                        
                        idMonedaTabla = gvMonedas.rows[fila].cells[0].textContent;
                        rateMonedaTabla = gvMonedas.rows[fila].cells[3].textContent;
						if (idMonedaTabla != null && rateMonedaTabla != null) {
							conversion = parseFloat(valorEuros * parseFloat(getInfoMoneda(idMonedaTabla)[1].replace(",", ".")));
                            gvMonedas.rows[fila].cells[1].textContent = conversion.toFixed(0);
						}
						else
							throw new Error();											
					}
				}
			} catch (err) {
                alert("Error al realizar el calculo");
			}
		}		

		//Obtiene la informacion de la moneda del campo hidden
		function getInfoMoneda(idMoneda) {			
			var monedas = document.getElementById('<%=hfRate.ClientID %>').value.split(';');
			for (index = 0; index < monedas.length; index = index + 1) {				
				var infoMoneda = monedas[index].split('_');
				if (parseInt(infoMoneda[0]) == parseInt(idMoneda)) {
					return infoMoneda;					
				}
			}
			return null;
		}
    </script>	
	<div class="form-inline">
		<asp:Label runat="server" ID="labelCant" Text="Cantidad" CssClass="form-control"></asp:Label>
		<asp:TextBox runat="server" ID="txtCantidad" MaxLength="8" style="text-align:center;" CssClass="form-control"></asp:TextBox>
        <ajax:FilteredTextBoxExtender ID="ftbCantidad" runat="server" TargetControlID="txtCantidad" FilterType="Numbers,Custom" ValidChars=".," />
		<asp:Label runat="server" ID="labelMon" Text="moneda" CssClass="form-control"></asp:Label>
		<asp:DropDownList runat="server" ID="ddlMonedas" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
	</div><br />
	<b><asp:Label runat="server" ID="labelCambios" Text="Cambios a dia de hoy"></asp:Label></b><br /><br />
	<asp:UpdatePanel runat="server" ID="upNiveles">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-8">
                    <asp:GridView runat="server" ID="gvConversiones" AutoGenerateColumns="false" CssClass="table table-striped" GridLines="None">		       
		                <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                <Columns>                                        
                            <asp:TemplateField ItemStyle-Width="1px">
                                <ItemTemplate><asp:Label runat="server" Text='<%#Eval("Id")%>' style="visibility:hidden"></asp:Label></ItemTemplate>                        
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Conversion" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="Right">					
				                <ItemTemplate><asp:Label runat="server" Text="0"></asp:Label></ItemTemplate>
			                </asp:TemplateField>																
			                <asp:BoundField HeaderText="moneda" DataField="Nombre" ItemStyle-CssClass="text-uppercase" />
			                <asp:BoundField HeaderText="1 euro son" DataField="ConversionEuros" ItemStyle-HorizontalAlign="Right" />
		                </Columns>
	                </asp:GridView>
                </div>
            </div>	        
	    </ContentTemplate>
	</asp:UpdatePanel>
	<asp:HiddenField runat="server" ID="hfRate" />
</asp:Content>
