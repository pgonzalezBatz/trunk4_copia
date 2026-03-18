<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Actividades.aspx.vb" Inherits="WebRaiz.Actividades" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">             
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="panel-group" id="divAccordion">
                <div class="panel panel-primary">                    
                    <div class="panel-heading" data-toggle="collapse" id="aAccordionTitle" style="cursor:pointer" href="#divCollapse" data-parent="#divAccordion">                                                
                        <h4 class="panel-title">                            
                            <span class="glyphicon glyphicon glyphicon-filter"></span>
				            <asp:Label runat="server" id="labelTitle" Text="Filtro" CssClass="control-label"></asp:Label>                            
			            </h4>                              
                     </div>
                    <div class="panel-collapse collapse" id="divCollapse">                           
                         <div class="panel-body lines">
                             <div class="input-group">
                                <asp:Textbox runat="server" id="txtFilter" CssClass="form-control"></asp:Textbox>
                                <span class="input-group-btn">
                                   <button runat="server" id="btnSearch" class="btn btn-primary" type="button"><span aria-hidden="true" class="glyphicon glyphicon-search"></span></button>
                                </span>
                            </div><br />
                            <asp:RadioButtonList runat="server" ID="rblMostrar" RepeatDirection="Horizontal" CellPadding="30" CellSpacing="30"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>            
             <asp:LinkButton runat="server" ID="lnkNueva" Text="Nueva"></asp:LinkButton><br /><br />
             <asp:GridView runat="server" ID="gvActividades" AutoGenerateColumns="false" CssClass="table table-striped table-condensed table-hover" GridLines="None" AllowSorting="true" AllowPaging="true" PageSize="20">		        
		        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />
		        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		        <Columns>		
                    <asp:BoundField DataField="Nombre" HeaderText="actividad" SortExpression="Nombre" />
			        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
				        <HeaderTemplate><asp:Label runat="server" Text="Exenta IRPF"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:CheckBox ID="chbExenta" runat="server" Enabled="false" /></ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
				        <HeaderTemplate><asp:Label runat="server" Text="Requiere texto"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:CheckBox ID="chbReqTexto" runat="server" Enabled="false"></asp:CheckBox></ItemTemplate>
			        </asp:TemplateField>
                     <asp:TemplateField ItemStyle-HorizontalAlign="Center">
				        <HeaderTemplate><asp:Label runat="server" Text="PaP"></asp:Label></HeaderTemplate>
				        <ItemTemplate><asp:CheckBox ID="chbPaP" runat="server" Enabled="false"></asp:CheckBox></ItemTemplate>
			        </asp:TemplateField>            
		        </Columns>
	        </asp:GridView>	
            <div id="divModal" class="modal fade" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelTitleModal" Text="Cuenta planta"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:Panel runat="server" ID="pnlMensa">
                                <asp:Label runat="server" ID="lblMensaje" CssClass="text-danger" Text="No se puede modificar su nombre ni cambiar si exenta o no porque esta relacionado con alguna persona"></asp:Label><br /><br />
                            </asp:Panel>
                            <div class="form-group">
                                <asp:Label runat="server" ID="labelActividad" Text="actividad"></asp:Label>
                                <asp:TextBox runat="server" ID="txtActividad" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvActiv" runat="server" Display="None" ControlToValidate="txtActividad" ValidationGroup="SaveDet" ErrorMessage="Introduzca el dato" EnableClientScript="true"></asp:RequiredFieldValidator>
                                <ajax:ValidatorCalloutExtender runat="server" ID="vceActiv" TargetControlID="rfvActiv" />
                            </div>
                            <div class="form-group">
                                <asp:RadioButtonlist runat="server" ID="rblExento" RepeatDirection="Horizontal" Width="50%"></asp:RadioButtonlist>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"><asp:Checkbox runat="server" ID="chbReqTexto" Text="Requiere texto"></asp:Checkbox></div>
                                <div class="col-sm-3"><asp:Checkbox runat="server" ID="chbPaP" Text="PaP" ToolTip="Indica si es una puesta a punto"></asp:Checkbox></div>
                                <div class="col-sm-3"><asp:Checkbox runat="server" ID="chbObsoleta" Text="obsoleto"></asp:Checkbox></div>                                
                            </div>
                            <div class="form-inline">                                
                                <asp:Label runat="server" ID="labelDptoAfect" Text="Departamentos afectados"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlDepartamentos" AppendDataBoundItems="true" DataTextField="text" DataValueField="value" CssClass="form-control"></asp:DropDownList>
                                <asp:Linkbutton runat="server" id="lnkAddDpto" ToolTip="Añadir" ValidationGroup="AñadirDpto" CssClass="form-control btn btn-success"><span aria-hidden="true" class="glyphicon glyphicon-plus"></span></asp:Linkbutton>                                                                
                            </div>
                            <asp:GridView runat="server" ID="gvDepartamentos" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">		                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
				                        <HeaderTemplate><asp:Label runat="server" Text="quitar"></asp:Label></HeaderTemplate>
				                        <ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CssClass="form-control btn btn-danger" CommandArgument='<%#Eval("Id")%>' CommandName='<%# Eval("Nombre")%>' OnClick="lnkElim_Click"><span aria-hidden="true" class="glyphicon glyphicon-trash"></span></asp:Linkbutton></ItemTemplate>
			                        </asp:TemplateField>	
		                        </Columns>
	                        </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-12"><asp:Button runat="server" ID="btnSaveM" Text="Guardar" ValidationGroup="SaveDet" CssClass="form-control btn btn-primary" /></div>                            
                        </div> 
                    </div>
                </div>
            </div>             
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
     <script>
        $(document).ready(function () {            
            $('#<%=txtFilter.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearch.ClientID%>').click();
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            $('#<%=txtFilter.ClientID%>').on('keypress', function (e) {                
                if(e.which === 13)
                    $('#<%=btnSearch.ClientID%>').click();
            });
        });
    </script>
</asp:Content>
