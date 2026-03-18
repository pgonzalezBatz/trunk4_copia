<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Viajes.aspx.vb" Inherits="WebRaiz.Viajes" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    	
     <asp:Panel runat="server" ID="pnlGastosVisaLibres" CssClass="alert alert-warning">
        <asp:Image runat="server" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/Visa.png" />&nbsp;&nbsp;
        <b><asp:Label runat="server" ID="labelInfoVisa" Text="Gastos de visa libres detectados sin justificar" CssClass="text-uppercase"></asp:Label></b>
        <asp:HyperLink runat="server" ID="hlAcceder" Text="Acceder" NavigateUrl="~/Publico/HistoricoVisas.aspx"></asp:HyperLink>
     </asp:Panel>    
    <div class="row">
        <div class="col-sm-6">
            <asp:Image runat="server" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/HojaGastos_small.png" />
            <asp:LinkButton runat="server" ID="lnkVerHGSinViaje" Text="Hojas de gastos sin viajes asociados" style="font-size:15px;"></asp:LinkButton>
        </div>
        <div class="col-sm-6">
            <asp:Image runat="server" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/Security.png" />
            <asp:LinkButton runat="server" ID="lnkVerseguridad" Text="Seguridad en viajes: acceso a servicios contratados" style="font-size:15px;" CssClass="alert-info"></asp:LinkButton>
        </div>
    </div>            
     <div class="panel-group" id="divAccordion">
        <div class="panel panel-primary">                    
            <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                <h4 class="panel-title">                            
                    <span class="glyphicon glyphicon glyphicon-filter"></span>
				    <asp:Label runat="server" id="labelTitle" Text="Filtro (Si su viaje no aparece, pinche aqui para buscarlo)" CssClass="control-label"></asp:Label>                            
			    </h4>                              
            </div>
            <div class="panel-collapse collapse" id="divCollapse">                           
                <div class="panel-body lines">
                    <div class="form-group">
                        <asp:Label runat="server" ID="labelInfo1" CssClass="help-block"></asp:Label>
                        <asp:Label runat="server" ID="labelInfo2" Text="Una vez llegue la fecha de ida de un viaje, se podrá empezar a rellenar la hoja de gastos pinchando en el icono de las monedas de cada viaje" CssClass="help-block"></asp:Label>
                        <asp:Label runat="server" ID="labelInfo3" Text="Si la actividad que desarrolla en su viaje es exenta, debera introducir los documentos como tarjetas de embarque, facturas, etc..." CssClass="help-block"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 form-inline">
                            <asp:CheckBox runat="server" ID="chbUsarFechas" />
                            <asp:Label runat="server" ID="labelFIni" Text="Fecha inicio"></asp:Label>
                        </div>
                        <div class="col-sm-4">
                            <div class="input-group date" id="dtFechaIni">
                                <asp:TextBox runat="server" ID="txtFechaInicio" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                        </div>
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelFFin" text="FechaFin"></asp:Label></div>
                        <div class="col-sm-4">
                            <div class="input-group date" id="dtFechaFin">
                                <asp:TextBox runat="server" ID="txtFechaFin" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:TextBox runat="server" ID="txtFilter" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="form-control btn btn-primary" /></div>
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnVerTodos" Text="Ver todos los viajes" CssClass="form-control btn btn-primary" /></div>
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnVerCancelados" Text="Ver viajes cancelados" CssClass="form-control btn btn-danger" /></div>
                    </div>                                        
                </div>
            </div>
        </div>
    </div>      
    <asp:GridView runat="server" ID="gvViajes" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-condensed table-hover" GridLines="None" PageSize="20">	    
        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
        <PagerSettings PageButtonCount="5" />
	    <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
	    <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" Text="Viaje"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Imagebutton runat="server" id="imgVerSolicitud" CommandName="ver" ImageUrl="~/App_Themes/Tema1/Images/ver.png"></asp:Imagebutton></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" Text="HG"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Imagebutton runat="server" id="imgLineasGastos" CommandName="linea"></asp:Imagebutton></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs">
                <HeaderTemplate><asp:Label runat="server" Text="Visas"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Imagebutton runat="server" id="imgGastosVisa" CommandName="linea"></asp:Imagebutton></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" Text="Docs"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Imagebutton runat="server" id="imgDocs" CommandName="docs"></asp:Imagebutton></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField SortExpression="IdViaje" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs">
			    <HeaderTemplate><asp:LinkButton runat="server" text="Id Viaje" CommandArgument="IdViaje" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblIdViaje" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField SortExpression="FechaIda" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			    <HeaderTemplate><asp:LinkButton runat="server" text="Fecha ida" CommandArgument="FechaIda" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblFechaIda" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>            
            <asp:TemplateField SortExpression="FechaVuelta" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center hidden-xs" ItemStyle-CssClass="hidden-xs">
			    <HeaderTemplate><asp:LinkButton runat="server" text="Fecha vuelta" CommandArgument="FechaVuelta" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblFechaVuelta" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>            
            <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
		    <asp:TemplateField>
			    <HeaderTemplate><asp:Label runat="server" Text="Integrantes"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblIntegrantes" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
			    <HeaderTemplate><asp:Label runat="server" text="Viaje"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblViaje" runat="server" style="font-size:13px;"></asp:Label></ItemTemplate>
		    </asp:TemplateField>	
            <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
			    <HeaderTemplate><asp:Label runat="server" text="Agencia"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblAgencia" runat="server" style="font-size:13px;"></asp:Label></ItemTemplate>
		    </asp:TemplateField>		    			
            <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
			    <HeaderTemplate><asp:Label runat="server" text="Anticipos"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblAnticipos" runat="server" style="font-size:13px;"></asp:Label></ItemTemplate>
		    </asp:TemplateField>		    
	    </Columns>
    </asp:GridView>
</asp:Content>
