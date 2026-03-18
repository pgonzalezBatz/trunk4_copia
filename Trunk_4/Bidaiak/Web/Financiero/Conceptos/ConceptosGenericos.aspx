<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ConceptosGenericos.aspx.vb" Inherits="WebRaiz.ConceptosGenericos" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:Label runat="server" ID="labelInfo" Text="Asociar los conceptos genericos a un concepto de Batz actualizandole el sector por el valor seleccionado"></asp:Label><br /><br />
    <asp:GridView runat="server" ID="gvConceptos" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None" DataKeyNames="Id">		
		<EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
		<Columns>
            <asp:BoundField DataField="Id" Visible="false" />
            <asp:BoundField DataField="Sector" HeaderText="Sector" />            
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Nuevo sector"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:DropDownList runat="server" ID="ddlSectores" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" />
            <asp:BoundField DataField="Localidad" HeaderText="Localidad" />           		    
		</Columns>
	</asp:GridView><br />
	<div class="row">
        <div class="col-sm-2"><asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="form-control btn btn-primary" /></div>
        <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" PostBackUrl="~/Financiero/Conceptos/MapearConceptos.aspx" /></div>        
	</div>
</asp:Content>
