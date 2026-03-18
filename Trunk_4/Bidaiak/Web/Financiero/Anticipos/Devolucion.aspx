<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Devolucion.aspx.vb" Inherits="WebRaiz.Devolucion" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">	
    <script type="text/javascript">
        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null && cb.disabled == false)
                cb.checked = checkState;
        }

        function ChangeAllCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array and updates their value to the checkState input parameter
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }

        function ChangeHeaderAsNeeded() {
            // Whenever a checkbox in the GridView is toggled, we need to check the Header checkbox if ALL of the GridView checkboxes are checked, and uncheck it otherwise
            if (CheckBoxIDs != null) {
                // check to see if all other checkboxes are checked
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (!cb.checked) {
                        // Whoops, there is an unchecked checkbox, make sure that the header checkbox is unchecked
                        ChangeCheckBoxState(CheckBoxIDs[0], false);
                        return;
                    }
                }
                // If we reach here, ALL GridView checkboxes are checked
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }
    </script>	
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong><asp:Label runat="server" ID="labelDivDev" Text="Devolucion"></asp:Label></strong>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelUsuario" Text="Usuario"></asp:Label></div>
                        <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlUsuarios" AppendDataBoundItems="true" CssClass="form-control required"></asp:DropDownList></div>
                        <div class="col-sm-2"><asp:Label runat="server" ID="labelFecha" Text="fecha"></asp:Label></div>
                        <div class="col-sm-4">
                            <div class="input-group date" id="dtFechaDev">
                                <asp:TextBox runat="server" ID="txtFechaDev" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:TextBox runat="server" ID="txtCantidad" MaxLength="8" style="text-align:center" CssClass="form-control required"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbCantidad" runat="server" TargetControlID="txtCantidad" FilterType="Numbers,Custom" ValidChars=".," />
                        </div>
                        <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlMonedas" CssClass="form-control required"></asp:DropDownList></div>
                        <div class="col-sm-2"><asp:Label runat="server" ID="lblEtiEurosPend" text="Euros pendientes"></asp:Label></div>
                        <div class="col-sm-4"><asp:Label runat="server" ID="lblEurosPend"></asp:Label></div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-sm-3"><asp:Button runat="server" ID="btnDevolver" Text="Devolver" CssClass="form-control btn btn-primary" /></div>
                        <div class="col-sm-3"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
                    </div>
                </div>
            </div><br />
             <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong><asp:Label runat="server" ID="labelDivHistorico" Text="Historico de devoluciones"></asp:Label></strong>
                </div>
                <div class="panel-body">
                    <asp:GridView runat="server" ID="gvDevoluciones" AutoGenerateColumns="false" CssClass="table table-striped table-hover" GridLines="None">	    
	                    <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
	                    <Columns>
                            <asp:TemplateField Visible="false">        
                                <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">               
                                <HeaderTemplate><asp:CheckBox runat="server" ID="chbSelectAll" ToolTip="Seleccionar todos" /></HeaderTemplate>
			                    <ItemTemplate><asp:CheckBox runat="server" ID="chbMarcar" /></ItemTemplate>
                                <FooterTemplate></FooterTemplate>
		                   </asp:TemplateField>  
		                    <asp:TemplateField>
			                    <HeaderTemplate><asp:Label runat="server" text="Fecha"></asp:Label></HeaderTemplate>
			                    <ItemTemplate><asp:Label ID="lblFechaDev" runat="server"></asp:Label></ItemTemplate>
		                    </asp:TemplateField>  
                            <asp:TemplateField>
			                    <HeaderTemplate><asp:Label runat="server" text="Cantidad"></asp:Label></HeaderTemplate>
			                    <ItemTemplate><asp:Label ID="lblCantidad" runat="server"></asp:Label></ItemTemplate>
		                    </asp:TemplateField>
		                    <asp:TemplateField>
			                    <HeaderTemplate><asp:Label runat="server" text="Moneda"></asp:Label></HeaderTemplate>
			                    <ItemTemplate><asp:Label ID="lblMoneda" runat="server"></asp:Label></ItemTemplate>
		                    </asp:TemplateField>            				
	                    </Columns>
                    </asp:GridView>
                </div>
                <div class="panel-footer">
                    <asp:Button runat="server" ID="btnPrint" Text="Imprimir" CssClass="form-control btn btn-primary" />
                </div>
             </div>    
            <asp:Literal runat="server" ID="CheckBoxIDsArray"></asp:Literal><br />    
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
