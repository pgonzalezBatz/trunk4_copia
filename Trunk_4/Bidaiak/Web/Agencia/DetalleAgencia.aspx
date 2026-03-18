<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DetalleAgencia.aspx.vb" Inherits="WebRaiz.DetalleAgencia" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivCab" Text="Datos de cabecera"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelIdViaje" text="Num Viaje"></asp:Label></div>
                <div class="col-sm-4"><asp:Label ID="lblIdViaje" runat="server" CssClass="label label-info" style="font-size:18px"></asp:Label></div>
                <div class="col-sm-2"><asp:LinkButton runat="server" ID="lnkModificarViaje" Text="Ver viaje"></asp:LinkButton></div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelDestino" text="Destino"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblDestino" runat="server"></asp:Label></b></div>
                <div class="col-sm-2"><asp:Label runat="server" id="labelTipoViaje" Text="Tipo viaje"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblTipoViaje" runat="server"></asp:Label></b></div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelSol" text="Solicitante"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblSol" runat="server" CssClass="text-uppercase"></asp:Label></b></div>
                <div class="col-sm-2"><asp:Label runat="server" ID="labelEstado" text="Estado"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblEstado" runat="server" CssClass="text-uppercase" style="font-size:14px"></asp:Label></b></div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelFechaida" text="Fecha ida"></asp:Label></div>
                <div class="col-sm-4"><b><asp:Label ID="lblFechaIda" runat="server"></asp:Label></b></div>
                <div class="col-sm-2"><asp:Label runat="server" ID="labelFechaVuelta" text="Fecha vuelta"></asp:Label></div>
                <div class="col-sm-3">
                    <b><asp:Label ID="lblFechaVuelta" runat="server"></asp:Label></b>
                    <asp:Label runat="server" ID="lblInfo" CssClass="glyphicon glyphicon-info-sign text-primary" style="margin-left:10px;cursor:help"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelProyectos" Text="Proyectos"></asp:Label></div>
                <div class="col-sm-10"><b><asp:Label runat="server" ID="lblProyectos"></asp:Label></b></div>                
            </div>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivInt" Text="Integrantes del viaje"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:GridView runat="server" ID="gvIntegrantes" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		        
		        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		        <Columns>				
                    <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
				        <HeaderTemplate><asp:Label runat="server" Text="IdTrabajador"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label runat="server" ID="lblNumTrabajador"></asp:Label></ItemTemplate>
			        </asp:TemplateField>							
			        <asp:TemplateField>                        
				        <HeaderTemplate><asp:Label runat="server" Text="Nombre"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label runat="server" ID="lblNombrePersona"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="gridview-header-center" ItemStyle-HorizontalAlign="Center">
				        <HeaderTemplate><asp:Label runat="server" Text="Desde"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label runat="server" ID="lblFDesde"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="gridview-header-center" ItemStyle-HorizontalAlign="Center">
				        <HeaderTemplate><asp:Label runat="server" Text="Hasta"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:Label runat="server" ID="lblFHasta"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
			        <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
				        <HeaderTemplate><asp:Label runat="server" Text="departamento"></asp:Label></HeaderTemplate>					
				        <ItemTemplate><asp:Label runat="server" ID="lblDepartamento"></asp:Label></ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField>
				        <HeaderTemplate><asp:Label runat="server" Text="DNI"></asp:Label></HeaderTemplate>					
				        <ItemTemplate><asp:Label runat="server" ID="lblDNI"></asp:Label></ItemTemplate>
			        </asp:TemplateField>	                   
		        </Columns>
	        </asp:GridView>
        </div>
    </div>        
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivRec" Text="Recursos requeridos"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="form-group"><b><asp:Label runat="server" ID="lblServReq" style="font-size:18px" CssClass="text-primary"></asp:Label></b></div>
            <div class="form-group">
                <asp:Label runat="server" ID="labelDescripSolic" text="Descripcion del viaje"></asp:Label>
                <b><asp:Label runat="server" ID="lblDescripcionSolic"></asp:Label></b>
            </div>
            <div class="form-group">
                <asp:Label runat="server" ID="labelComenSolic" text="Comentarios del solicitante"></asp:Label>
                <b><asp:Label runat="server" ID="lblComentariosSolic"></asp:Label></b>
            </div>
            <div class="form-group" runat="server" id="divCocheAlq">
                <asp:Label runat="server" ID="labelConductor" text="Conductor del coche"></asp:Label>
                <b><asp:Label runat="server" ID="lblConductor"></asp:Label></b>
            </div>
        </div>        
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelDivPresup" Text="Presupuesto"></asp:Label></strong>
        </div>
        <div class="panel-body">            
            <div runat="server" id="divPresup">
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelUO" Text="Unidad organizativa"></asp:Label></div>
                    <div class="col-sm-4"><b><asp:Label runat="server" ID="lblUnidadOrg"></asp:Label></b></div>
                    <div class="col-sm-6"><b><asp:Label ID="lblEstadoValidacion" runat="server" cssClass="text-uppercase" style="font-size:18px;"></asp:Label></b></div>
                </div>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelValid" Text="validador"></asp:Label></div>
                    <div class="col-sm-4">
                        <asp:Label runat="server" ID="lblValidador"></asp:Label>
                        <asp:HiddenField runat="server" ID="hfIdValidador" />
                    </div>
                    <div class="col-sm-6"><b><asp:LinkButton runat="server" ID="lnkVerPresupuesto" Text="Ver presupuesto" style="font-size:18px;"></asp:LinkButton></b></div>                    
                </div>                
            </div>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelCabDivAlb" Text="Albaranes"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <div class="form-inline">
                <asp:TextBox runat="server" ID="txtAlbaran" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAlbaran" runat="server" Display="None" ControlToValidate="txtAlbaran" ValidationGroup="GuardarAlb" ErrorMessage="Introduzca el albaran"></asp:RequiredFieldValidator>
                <asp:Button runat="server" ID="btnAddAlbaran" Text="Añadir" ValidationGroup="GuardarAlb" CssClass="form-control bn btn-primary" />
                <asp:Label runat="server" ID="labelRecordatorio" Text="No olvide guardar los datos al final" CssClass=" text-warning"></asp:Label>
            </div>
            <ul id="mylist" class="list-group">
                <asp:Repeater runat="server" ID="rptAlbaranes">
                    <ItemTemplate><li class="list-group-item"><asp:Label runat="server" ID="lblAlbaran"></asp:Label><asp:LinkButton runat="server" ID="lnkQuitarAlb" CssClass="pull-right" OnClick="lnkQuitarAlb_Click"><i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton></li></ItemTemplate>                
                </asp:Repeater>
            </ul>
        </div>
    </div>
    <div class="form-group">
        <asp:checkbox runat="server" ID="chbGestionado" Text="Gestionado" TextAlign="Right" />
    </div>
    <div class="row">
        <div class="col-sm-3"><asp:Button runat="server" ID="btnGuardar" Text="Guardar datos" CssClass="form-control btn btn-primary" /></div>
        <div class="col-sm-3"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
    </div>	
</asp:Content>
