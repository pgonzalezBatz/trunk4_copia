<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Perfiles.aspx.vb" Inherits="WebRaiz.Perfiles" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">			
	<asp:MultiView ID="mvUsuarios" runat="server" ActiveViewIndex="0">
		<asp:View ID="vListado" runat="server">
            <uc:Busqueda ID="searchUser" runat="server" PostBack="true" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" /><br />    	                                        					
		</asp:View>
		<asp:View runat="server" ID="vDetalle">
            <div class="row">
                <div class="col-sm-3">
                    <b><asp:Label ID="lblPersona" runat="server"></asp:Label></b>
                    <asp:HiddenField runat="server" ID="hfIdUsuario" />
                </div>
                <div class="col-sm-2"><asp:CheckBox runat="server" ID="chbAccesoDir" Text="Acceso directo" ToolTip="Se mostrara en la Home Intranet, sino habra que acceder a traves del portal del empleado" /></div>
            </div>                                          
				<asp:Panel runat="server" ID="pnlPerfiles" CssClass="row">
                    <div class="col-sm-2"><asp:CheckBox runat="server" ID="chbAdmin" Text="Administrador" AutoPostBack="true" /></div>
                    <div class="col-sm-12"><asp:CheckBoxList runat="server" ID="chblPerfiles" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList></div>                         
				</asp:Panel><br /><br />
			<div class="row">
                <div class="col-sm-2"><asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="form-control btn btn-primary" /></div>
                <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
                <div class="col-sm-2"><asp:LinkButton runat="server" ID="lnkEliminar" CssClass="form-control btn btn-danger" Text="Eliminar" data-toggle="modal"></asp:LinkButton></div>                                         
			</div>
            <div class="modal fade" id="confirmDelete">
                <div class="modal-dialog modal-confirm">
                    <div class="modal-content">  
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title" id="myModalLabelRep"><asp:Label runat="server" ID="labelConfirmTitle" Text="Confirmar borrado"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:Label runat="server" ID="labelConfirmMessage" Text="confirmarEliminar"></asp:Label>
                        </div>
                        <div class="modal-footer form-inline">
                            <asp:Button runat="server" ID="btnConfirmDelete" Text="Continuar" cssclass="form-control btn btn-primary" /> 
                            <button type="button" class="form-control btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelConfirmCerrar" Text="Cerrar"></asp:Label></button>                                                
                        </div>
                    </div>
                </div>
            </div>
		</asp:View>
	</asp:MultiView>
</asp:Content>
