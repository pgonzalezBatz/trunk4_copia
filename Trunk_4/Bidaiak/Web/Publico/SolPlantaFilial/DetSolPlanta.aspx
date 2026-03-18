<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DetSolPlanta.aspx.vb" Inherits="WebRaiz.DetSolPlanta" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">        
    <asp:Panel runat="server" ID="pnlExiste">
        <div class="row">
            <div class="col-sm-2"><asp:Label runat="server" ID="labelEstado" Text="Estado"></asp:Label></div>
            <div class="col-sm-4"><b><asp:Label runat="server" ID="lblEstado" style="font-size:15px"></asp:Label></b></div>
            <div class="col-sm-2"><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></div>
            <div class="col-sm-4"><b><asp:Label runat="server" ID="lblPlanta"></asp:Label></b></div>
        </div>
        <div class="row">
            <div class="col-sm-2"><asp:Label runat="server" ID="labelViaje" Text="Viaje"></asp:Label></div>
            <div class="col-sm-4"><b><asp:Label runat="server" ID="lblViaje"></asp:Label></b></div>
            <div class="col-sm-2"><asp:Label runat="server" ID="labelFechas" Text="Fechas"></asp:Label></div>
            <div class="col-sm-4"><b><asp:Label runat="server" ID="lblFechas"></asp:Label></b></div>
        </div>
        <div class="row">
            <div class="col-sm-2"><asp:Label runat="server" ID="labelDescrip" Text="Descripcion"></asp:Label></div>
            <div class="col-sm-10"><b><asp:Label runat="server" ID="lblDescrip"></asp:Label></b></div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" Text="Asistentes"></asp:Label>
        </div>
        <asp:GridView runat="server" ID="gvPersonas" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		    
		    <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		    <Columns>							                
			    <asp:TemplateField>
				    <HeaderTemplate><asp:Label runat="server" Text="Nombre"></asp:Label></HeaderTemplate>
				    <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
			    </asp:TemplateField> 
                <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
				    <HeaderTemplate><asp:Label runat="server" Text="Actividad"></asp:Label></HeaderTemplate>
				    <ItemTemplate><asp:Label runat="server" ID="lblActividad"></asp:Label></ItemTemplate>
			    </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				    <HeaderTemplate><asp:Label runat="server" Text="Fecha Ida"></asp:Label></HeaderTemplate>
				    <ItemTemplate><asp:Label runat="server" ID="lblFechaIda"></asp:Label></ItemTemplate>
			    </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
				    <HeaderTemplate><asp:Label runat="server" Text="Fecha Vuelta"></asp:Label></HeaderTemplate>
				    <ItemTemplate><asp:Label runat="server" ID="lblFechaVuelta"></asp:Label></ItemTemplate>
			    </asp:TemplateField>
                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
		    </Columns>
	    </asp:GridView>         
        <div class="form-group">
            <asp:Label ID="labelObserv" runat="server" Text="Rellena la solicitud"></asp:Label><br />
            <asp:TextBox runat="server" ID="txtObserv" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
        </div>        
    </asp:Panel>   
    <asp:Panel runat="server" ID="pnlNoExiste" CssClass="alert alert-danger">
        <asp:Label ID="lblNoExiste" runat="server" Text="La solicitud no existe. Puede que el solicitante la haya cancelado"></asp:Label>
    </asp:Panel>
    <div class="row">
        <div class="col-sm-2"><asp:Button runat="server" ID="btnAprobar" Text="Aprobar" CssClass="form-control btn btn-success" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnRechazar" Text="Rechazar" CssClass="form-control btn btn-danger" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
    </div>
    <div id="divModalRej" class="modal fade" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Rechazo de solicitud"></asp:Label></h4>                    
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="labelConfirmMessageModal" Text="Introduzca un comentario de rechazo"></asp:Label><br />
                    <asp:TextBox runat="server" ID="txtRechazo" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnRechazarModalM" Text="Rechazar" cssclass="btn btn-danger" /> 
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true"><asp:Label runat="server" ID="labelCancelarModal" Text="Cancelar"></asp:Label></button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
