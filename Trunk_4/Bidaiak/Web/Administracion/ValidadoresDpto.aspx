<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ValidadoresDpto.aspx.vb" Inherits="WebRaiz.ValidadoresDpto" %>
<%@ Register src="../Controles/BusquedaUsuarios.ascx" tagname="Busqueda" tagprefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">                
    <asp:Label runat="server" ID="labelInfo" Text="Se muestra la informacion de los validadores que habra que considerar en vez de los que marque la estructura de Batz para cada persona perteneciente al departamento seleccionado"></asp:Label><br /><br />
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelDpto" Text="Departamento"></asp:Label></div>
        <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlDpto" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList></div>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelVal" Text="Validador"></asp:Label></div>
        <div class="col-sm-10"><uc:Busqueda ID="searchUser" runat="server" PostBack="false" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" /><br />    	                                        					</div>
    </div>
    <div class="form-group">
        <asp:Button runat="server" ID="btnAnadir" Text="Añadir" CssClass="form-control btn btn-primary" />
    </div><br />
    <asp:GridView runat="server" ID="gvDptos" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None" AllowPaging="true" PageSize="20">
		<PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
        <PagerSettings PageButtonCount="5" />
		<EmptyDataTemplate>
			<br /><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		<Columns>	
            <asp:TemplateField>
				<HeaderTemplate><asp:Label runat="server" Text="Departamento"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:Label ID="lblDepartamento" runat="server"></asp:Label></ItemTemplate>
			</asp:TemplateField>			
			<asp:TemplateField>
				<HeaderTemplate><asp:Label runat="server" Text="nombrePersona"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:Label runat="server" ID="lblNombreCompleto"></asp:Label></ItemTemplate>
			</asp:TemplateField>				
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
				<HeaderTemplate><asp:Label runat="server" Text="Elim"></asp:Label></HeaderTemplate>
				<ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CssClass="form-control btn btn-danger" data-toggle="modal"><span aria-hidden="true" class="glyphicon glyphicon-trash"></span></asp:Linkbutton></ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
    <asp:HiddenField runat="server" ID="hfIdDptoDelete" />
    <div class="modal fade" id="confirmDelete">
        <div class="modal-dialog modal-confirm">
            <div class="modal-content">            
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel"><asp:Label runat="server" ID="labelConfirmDeleteTitle" Text="Confirmar borrado"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="labelConfirmMessage" Text="confirmarEliminar"></asp:Label>
                </div>
                <div class="modal-footer">                          
                    <asp:Button runat="server" ID="btnEliminarModal" Text="Eliminar" cssclass="btn btn-primary" /> 
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelar" Text="Cancelar"></asp:Label></button>                          
                </div>
            </div>
        </div>
    </div>
</asp:Content>
