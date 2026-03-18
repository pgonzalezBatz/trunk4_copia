<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ListadoDocs.aspx.vb" Inherits="WebRaiz.ListadoDocs" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">        
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelCliente" Text="cliente"></asp:Label></div>
                <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlCliente" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></div>
            </div>
            <div class="row">
                <div class="col-sm-2"><asp:Label runat="server" ID="labelProyecto" Text="proyecto"></asp:Label></div>
                <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlProyecto" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></div>
            </div><br />
            <asp:Panel runat="server" ID="pnlResultado">
                <asp:LinkButton runat="server" ID="lnkAnadir" Text="Añadir"></asp:LinkButton><br /><br />
                <asp:GridView runat="server" ID="gvDocumentos" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                    <PagerSettings PageButtonCount="5" />
		            <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		            <Columns>
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                        <asp:TemplateField>
				            <HeaderTemplate><asp:Label runat="server" Text="Subido por"></asp:Label></HeaderTemplate>
				            <ItemTemplate><asp:Label runat="server" ID="lblSubidoPor"></asp:Label></ItemTemplate>
			            </asp:TemplateField>					
			            <asp:TemplateField>
				            <HeaderTemplate><asp:LinkButton runat="server" text="Fecha" CommandArgument="Fecha" CommandName="Sort"></asp:LinkButton></HeaderTemplate>
				            <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
			            </asp:TemplateField>			   
		            </Columns>
	            </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" /> 
</asp:Content>
