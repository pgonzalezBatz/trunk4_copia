<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="DetValVisa.aspx.vb" Inherits="WebRaiz.DetValVisa" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <div class="form-inline">
        <asp:Label runat="server" ID="labelGastosDe" Text="Gastos de visa de"></asp:Label>&nbsp;
        <b><asp:Label runat="server" id="lblPersona"></asp:Label></b>    
    </div>
    <div class="alert alert-warning" runat="server" id="divGastosSinJustif">
        <b><asp:Label runat="server" ID="labelInfoSinJustif" Text="Existen ciertos gastos de visa que todavia no se han justificado. Hasta que no se justifiquen, no se podran validar"></asp:Label></b>
    </div>
    <div class="visible-xs">
        <asp:Repeater runat="server" ID="rptVisa">
            <ItemTemplate>
                <div runat="server" id="divRptRow">
                    <div class="row">                    
                        <div class="col-xs-4">
                            <asp:CheckBox runat="server" ID="chbMarcarRpt" />
                            <asp:Label runat="server" ID="labelFechaRpt" Text="Fecha"></asp:Label>
                        </div>
                        <div class="col-xs-8"><b><asp:Label runat="server" ID="lblFechaRpt"></asp:Label></b></div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4"><asp:Label runat="server" ID="labelHojaRpt" Text="Hoja"></asp:Label></div>
                        <div class="col-xs-8"><b><asp:Label runat="server" ID="lblHojaRpt"></asp:Label></b></div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4"><asp:Label runat="server" ID="labelSectorRpt" Text="Sector"></asp:Label></div>
                        <div class="col-xs-8"><b><asp:Label runat="server" ID="lblSectorRpt"></asp:Label></b></div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4"><asp:Label runat="server" ID="labelLocalidadRpt" Text="Localidad"></asp:Label></div>
                        <div class="col-xs-8"><b><asp:Label runat="server" ID="lblLocalidadRpt"></asp:Label></b></div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4"><asp:Label runat="server" ID="labelEstablecimientoRpt" Text="Establecimiento"></asp:Label></div>
                        <div class="col-xs-8"><b><asp:Label runat="server" ID="lblEstablecimientoRpt"></asp:Label></b></div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="labelComentarioRpt" Text="Comentario"></asp:Label><br />
                        <b><asp:Label runat="server" ID="lblComentarioRpt"></asp:Label></b>
                    </div>
                    <div class="row">
                        <div class="col-xs-4"><asp:Label runat="server" ID="labelImporteRpt" Text="Importe"></asp:Label></div>
                        <div class="col-xs-8">
                            <b>
                               <asp:Label runat="server" ID="lblImporteRpt"></asp:Label>
                               <asp:Label runat="server" ID="lblMonedaRpt"></asp:Label>
                            </b>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4"><asp:Label runat="server" ID="labelImporteEurRpt" Text="Importe (EUR)"></asp:Label></div>
                        <div class="col-xs-8">
                            <b>
                               <asp:Label runat="server" ID="lblImporteEurRpt"></asp:Label>
                               <asp:Label runat="server" ID="lblMonedaEurRpt"></asp:Label>
                            </b>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4"><asp:Label runat="server" ID="labelEstadoRpt" Text="Estado"></asp:Label></div>
                        <div class="col-xs-8"><b><asp:Label runat="server" ID="lblEstadoRpt" style="font-size:15px"></asp:Label></b></div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
     <asp:GridView runat="server" ID="gvVisa" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-striped hidden-xs" GridLines="None" PageSize="20" DataKeyNames="Id,IdUsuario,IdViaje,Fecha">	 
        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
        <PagerSettings PageButtonCount="5" />
	    <EmptyDataTemplate><asp:Label runat="server" Text="No existe ningun gasto de visa la fecha seleccionada"></asp:Label></EmptyDataTemplate>
	    <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:CheckBox runat="server" ID="chbSelectAll" ToolTip="Seleccionar todos" /></HeaderTemplate>
			    <ItemTemplate><asp:CheckBox runat="server" ID="chbMarcar" /></ItemTemplate>                
		    </asp:TemplateField> 
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" text="fecha"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>                
		    </asp:TemplateField>             
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" text="Hoja"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblHoja"></asp:Label></ItemTemplate>
		    </asp:TemplateField> 
            <asp:BoundField DataField="Sector" HeaderText="Sector" />
            <asp:BoundField DataField="Localidad" HeaderText="Localidad" HeaderStyle-CssClass="hidden-sm" ItemStyle-CssClass="hidden-sm" />
            <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" HeaderStyle-CssClass="hidden-sm" ItemStyle-CssClass="hidden-sm" />
             <asp:TemplateField>
				<HeaderTemplate><asp:Label runat="server" Text="Comentario"></asp:Label></HeaderTemplate>
				<ItemTemplate>
					<asp:Label runat="server" ID="lblComentario"></asp:Label>
                    <asp:Image runat="server" ID="imgMasComentarios" style="cursor:pointer;margin-left:10px;" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/info16.png" />
				</ItemTemplate>
			</asp:TemplateField>            
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" text="Importe"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblImporte"></asp:Label></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" text="Importe (EUR)"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblImporteEur"></asp:Label></ItemTemplate>
		    </asp:TemplateField>
		   <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" text="estado"></asp:Label></HeaderTemplate>
			    <ItemTemplate>
				    <asp:Image runat="server" id="imgEstado"></asp:Image>
                    <b><asp:Label runat="server" ID="labelSinJustificar" Text="Sin justificar" CssClass="label label-danger" style="font-size:14px;margin-left:10px;"></asp:Label></b>
			    </ItemTemplate>
		    </asp:TemplateField>
	    </Columns>
    </asp:GridView><br />
    <asp:Literal runat="server" ID="CheckBoxIDsArray"></asp:Literal>
    <div class="row">
        <div class="col-sm-3"><asp:Button runat="server" ID="btnConforme" Text="Marcar como conforme" CssClass="form-control btn btn-primary" /></div>
        <div class="col-sm-3"><asp:Button runat="server" ID="btnNoConforme" Text="Marcar como no conforme" CssClass="form-control btn btn-danger" /></div>
        <div class="col-sm-3"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
    </div>
    <script type="text/javascript">
        function ChangeCheckBoxState(id, checkState)
        {
            var cb = document.getElementById(id);
            if (cb != null && cb.disabled==false)
               cb.checked = checkState;
        }

        function ChangeAllCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter
            if (CheckBoxIDs != null)
            {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                   ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }

        function ChangeHeaderAsNeeded() {
            // Whenever a checkbox in the GridView is toggled, we need to
            // check the Header checkbox if ALL of the GridView checkboxes are
            // checked, and uncheck it otherwise
            if (CheckBoxIDs != null) {
                // check to see if all other checkboxes are checked
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (!cb.checked) {
                        // Whoops, there is an unchecked checkbox, make sure
                        // that the header checkbox is unchecked
                        ChangeCheckBoxState(CheckBoxIDs[0], false);
                        return;
                    }
                }
                // If we reach here, ALL GridView checkboxes are checked
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }
    </script>
</asp:Content>
