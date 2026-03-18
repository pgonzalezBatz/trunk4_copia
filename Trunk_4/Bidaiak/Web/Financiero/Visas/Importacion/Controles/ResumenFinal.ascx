<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ResumenFinal.ascx.vb" Inherits="WebRaiz.ResumenFinal" %>
    <asp:Panel runat="server" ID="pnlCabecera">        
        <b><asp:Label runat="server" ID="lblResul"></asp:Label></b>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlAsientos">
        <asp:Label runat="server" ID="labelInfo1" Text="Se muestra un resumen de los asientos insertados en Navision" CssClass="help-block"></asp:Label><br />          
        <div class="row">
            <div class="col-sm-2"><asp:Label runat="server" ID="labelContrapartida" Text="Contrapartida"></asp:Label></div>
            <div class="col-sm-1"><b><asp:Label runat="server" ID="lblContrapartida" CssClass="label label-success" style="font-size:16px"></asp:Label></b></div>            
        </div>
        <div class="row">
            <div class="col-sm-2"><asp:Label runat="server" ID="labelCuota" Text="Cuota"></asp:Label></div>
            <div class="col-sm-1"><b><asp:Label runat="server" ID="lblCuota" CssClass="label label-info" style="font-size:16px"></asp:Label></b></div>
            <div class="col-sm-1"><asp:Label runat="server" ID="labelCuenta" Text="Cuenta"></asp:Label></div>
            <div class="col-sm-1"><b><asp:Label runat="server" ID="lblCtaCuota"></asp:Label></b></div>
        </div>
        <div class="row">
            <div class="col-sm-2"><asp:Label runat="server" ID="labelImporteVisaExcep" Text="Cuenta gastos consumibles (visa excepcion)"></asp:Label></div>
            <div class="col-sm-1"><b><asp:Label runat="server" ID="lblImporteVisaExcep" CssClass="label label-warning" style="font-size:16px"></asp:Label></b></div>            
           <%-- <div class="col-sm-1"><asp:Label runat="server" ID="labelCuentaVisaExcep" Text="Lantegi"></asp:Label></div>
            <div class="col-sm-1"><b><asp:Label runat="server" ID="lblCuentaVisaExcep" style="font-size:16px"></asp:Label></b></div>      
            <div class="col-sm-1"><asp:Label runat="server" ID="labelLantegiVisaExcep" Text="Lantegi"></asp:Label></div>
            <div class="col-sm-1"><b><asp:Label runat="server" ID="lblLantegiVisaExcep" style="font-size:16px"></asp:Label></b></div> --%>     
        </div>
        <asp:GridView runat="server" ID="gvAsientos" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" ShowFooter="true">		    
		    <Columns>	
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblDepart"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Cuenta"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblCuenta"></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Organizacion"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblOrganizacion"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Lantegi"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblLantegi"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
                </asp:TemplateField>                  			    										                					                              		    	
		    </Columns>
	    </asp:GridView>                            
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSinCuenta">            
        <b><asp:Label ID="labelInfo2" runat="server" Text="Los siguientes departamentos, no existen en la estructura de Batz asi que no se van a integrar sus asientos ni se va a sumar su importe a la cuenta de contrapartida"></asp:Label></b><br /><br />
        <div class="form-inline">
            <asp:Label runat="server" ID="labelContrapartida2" Text="Importe que no se añade a la cuenta de contrapartida"></asp:Label>
            <b><asp:Label runat="server" ID="lblContrapartida2"></asp:Label></b>
        </div>                        
        <asp:GridView runat="server" ID="gvSinCuenta" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" ShowFooter="true">		       
		    <Columns>	
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblDepart"></asp:Label></ItemTemplate>
                </asp:TemplateField>                  
                <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="Importe"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
                </asp:TemplateField>                  			    										                					                              		    	
		    </Columns>
	    </asp:GridView>
    </asp:Panel>     
