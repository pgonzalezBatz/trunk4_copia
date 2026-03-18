<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="MonedasAnticipo.aspx.vb" Inherits="WebRaiz.MonedasAnticipo" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:Label runat="server" ID="labelInfo" Text="Seleccionar las monedas que se podran solicitar en un anticipo"></asp:Label><br /><br />
    <div class="row">
        <div class="col-sm-6">        
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong><asp:Label runat="server" ID="labelDivCabMonSel" Text="Monedas seleccionables en anticipos"></asp:Label></strong>
                </div>
                <div class="panel-body">
                     <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gvMonSel" AutoGenerateColumns="false" CssClass="table table-striped" GridLines="None">                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Moneda" />                                    
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">				                        
				                        <ItemTemplate><asp:Linkbutton runat="server" id="lnkElim" CommandName="Elim" CssClass="form-control btn btn-danger" OnClick="AddMoneda"><span aria-hidden="true" class="glyphicon glyphicon-remove-sign"></span></asp:Linkbutton></ItemTemplate>
			                        </asp:TemplateField>
		                        </Columns>
	                        </asp:GridView>
                        </ContentTemplate>                               
                    </asp:UpdatePanel>                        
                </div>
            </div>
        </div>
        <div class="col-sm-6">
             <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong><asp:Label runat="server" ID="labelDivCabMonNoSel" Text="Monedas no seleccionables"></asp:Label></strong>
                </div>
                <div class="panel-body">
                     <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gvMonNoSel" AutoGenerateColumns="false" CssClass="table table-striped" GridLines="None">                        
		                        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		                        <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Moneda" />
                                   <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">				                        
				                        <ItemTemplate><asp:Linkbutton runat="server" id="lnkAdd" CommandName="Add" CssClass="form-control btn btn-success" OnClick="DeleteMoneda"><span aria-hidden="true" class="glyphicon glyphicon-plus-sign"></span></asp:Linkbutton></ItemTemplate>
			                        </asp:TemplateField>
		                        </Columns>
	                        </asp:GridView>
                        </ContentTemplate>                               
                    </asp:UpdatePanel>                        
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
