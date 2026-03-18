<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Asignacion.aspx.vb" Inherits="WebRaiz.Asignacion" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
     <div class="panel-group" id="divAccordion">
        <div class="panel panel-primary">                    
            <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                <h4 class="panel-title">                            
                    <span class="glyphicon glyphicon glyphicon-filter"></span>
				    <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			    </h4>                              
            </div>
            <div class="panel-collapse collapse in" id="divCollapse">                           
                <div class="panel-body lines" style="padding-bottom:0px;">                    
                    <div class="form-group">
                        <asp:TextBox runat="server" ID="txtIdViaje" MaxLength="7" CssClass="form-control"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="ftbIdViaje" runat="server" TargetControlID="txtIdViaje" FilterType="Numbers" />
                    </div>
                    <div class="form-group">                        
                        <uc:Busqueda ID="searchUserF" runat="server" PostBack="false" SoloActivos="true" RutaPaginaBusqueda="../../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" />
                    </div>
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelFIda" text="Fecha ida" CssClass="custom-label-control"></asp:Label></div>
                        <div class="col-sm-4">
                            <div class="input-group date" id="dtFechaIda">
                                <asp:TextBox runat="server" ID="txtFechaIda" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                        </div>
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelFV" text="Fecha vuelta" CssClass="custom-label-control"></asp:Label></div>
                        <div class="col-sm-4">
                            <div class="input-group date" id="dtFechaVuelta">
                                <asp:TextBox runat="server" ID="txtFechaVuelta" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3"><asp:Button runat="server" ID="btnSearch" Text="Buscar" CssClass="form-control btn btn-primary" /></div>
                        <div class="col-sm-3"><asp:Button runat="server" ID="btnReset" Text="Resetear filtro" CssClass="form-control btn btn-default" /></div>
                    </div>
                </div>                
            </div>            
        </div>
    </div>
     <asp:GridView runat="server" ID="gvViajes" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">
	    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
        <PagerSettings PageButtonCount="5" />
	    <EmptyDataTemplate><asp:Label ID="lblSinRegistros" runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
	    <Columns>            
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			    <HeaderTemplate><asp:LinkButton runat="server" text="Id Viaje" CommandArgument="IdViaje" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblIdViaje" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField SortExpression="FechaIda" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			    <HeaderTemplate><asp:LinkButton runat="server" text="Fecha ida" CommandArgument="FechaIda" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblFechaIda" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>            
            <asp:TemplateField SortExpression="FechaVuelta" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			    <HeaderTemplate><asp:LinkButton runat="server" text="Fecha vuelta" CommandArgument="FechaVuelta" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblFechaVuelta" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>            
            <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
		    <asp:TemplateField>
			    <HeaderTemplate><asp:Label runat="server" text="Integrantes"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblIntegrantes" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>            	    			
            <asp:TemplateField>
			    <HeaderTemplate><asp:Label runat="server" text="Anticipos"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblAnticipos" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>		    
	    </Columns>
    </asp:GridView>
</asp:Content>
