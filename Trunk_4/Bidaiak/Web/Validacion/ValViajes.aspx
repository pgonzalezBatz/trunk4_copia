<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ValViajes.aspx.vb" Inherits="WebRaiz.ValViajes" EnableEventValidation="false" %>
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
                    <div class="panel-collapse collapse in" id="divCollapse">                           
                         <div class="panel-body lines">                        
                            <asp:RadioButton runat="server" ID="rbtPorFechas" GroupName="Opciones" Text="Seleccione el mes y año del que quiere ver los viajes a validar" />    
                            <div class="form-inline">                                     
                                <asp:DropDownList runat="server" ID="ddlMes" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlAño" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <asp:RadioButton runat="server" ID="rbtSinValidar" RepeatDirection="Horizontal" GroupName="Opciones" Text="Ver viajes sin validar" CellPadding="30" CellSpacing="30"></asp:RadioButton>
                             <div class="form-group">
                                <asp:Button runat="server" ID="btnSearchF" Text="Buscar" CssClass="form-control btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:GridView runat="server" ID="gvViajes" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">
	            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />
	            <EmptyDataTemplate><asp:Label runat="server" Text="No existe ningun viaje a validar entre las fechas seleccionadas"></asp:Label></EmptyDataTemplate>
	            <Columns>            
                    <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" text="persona"></asp:Label></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
		            </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                        <HeaderTemplate><asp:Label runat="server" text="Fecha creacion"></asp:Label></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblFechaCreacion"></asp:Label></ItemTemplate>
		            </asp:TemplateField>            
		            <asp:TemplateField>
                        <HeaderTemplate><asp:Label runat="server" text="Nº viaje"></asp:Label></HeaderTemplate>
			            <ItemTemplate><asp:Label runat="server" ID="lblIdViaje"></asp:Label></ItemTemplate>
		            </asp:TemplateField> 
		           <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                        <HeaderTemplate><asp:Label runat="server" text="estado"></asp:Label></HeaderTemplate>
			            <ItemTemplate><asp:Image runat="server" id="imgEstado"></asp:Image></ItemTemplate>
		            </asp:TemplateField>           
	            </Columns>
            </asp:GridView>
            <asp:HiddenField runat="server" ID="hfOpcionSel" />   
        </ContentTemplate>
    </asp:UpdatePanel>   
    <uc:CargandoDatos runat="server" />
    <script type="text/javascript">
        //Se habilita o deshabilita los controles de los radiobutton
        //Si es 0, se habilitara la busqueda por mes y año
        //Si es 1, se habilitara la busqueda por movimientos sin validar
        function ChangeRadio(check) {
            document.getElementById('<%=rbtPorFechas.ClientID %>').checked = (check == 0);
            document.getElementById('<%=rbtSinValidar.ClientID %>').checked = (check == 1);
            document.getElementById('<%=ddlMes.ClientID %>').disabled = (check == 1);
            document.getElementById('<%=ddlAño.ClientID %>').disabled = (check == 1);
            document.getElementById('<%=hfOpcionSel.ClientID %>').value = check;
        }

        //Se repinta en cada postback
        ChangeRadio(document.getElementById('<%=hfOpcionSel.ClientID %>').value);

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            ChangeRadio(document.getElementById('<%=hfOpcionSel.ClientID %>').value);
        });
    </script>
</asp:Content>
