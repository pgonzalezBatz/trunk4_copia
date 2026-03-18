<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="HojasGastosSinViaje.aspx.vb" Inherits="WebRaiz.HojasGastosSinViaje" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">                
    <asp:Panel runat="server" ID="pnlInfo">
        <asp:Panel runat="server" ID="pnlInfoSinVisa" CssClass="alert alert-warning">
            <b><asp:Label runat="server" ID="labelInfo1" style="text-transform:uppercase;" Text="Las hojas de gastos son de caracter mensual. Hasta el ultimo dia del mes no podra enviarla"></asp:Label></b>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInfoConVisa" CssClass="alert alert-warning">
            <b>
                <asp:Label runat="server" ID="labelInfo2" style="text-transform:uppercase;font-weight:bold" Text="Las hojas de gastos son de caracter mensual. Como usted tiene una visa asignada, tendra que esperar hasta principios del mes siguiente que es cuando se carga el fichero de visas para poder enviar la hoja"></asp:Label><br />
                <asp:Label runat="server" ID="labelInfo3" Text="Si no tiene ningun gasto en metalico ni kilometraje y tan solo quiere adjunta los tickets de visa, una vez cargado el fichero, podra enviar a su validador la hoja de gastos"></asp:Label>
            </b>
        </asp:Panel>
    </asp:Panel>
    <div class="row">
        <div class="col-sm-2"><asp:Label runat="server" ID="labelAno" Text="Año"></asp:Label></div>
        <div class="col-sm-4"><asp:DropDownList runat="server" id="ddlAnno" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></div>
        <div class="col-sm-4"><asp:LinkButton runat="server" ID="lnkVerViajes" Text="Ver listado de viajes" style="font-size:15px;"></asp:LinkButton></div>
    </div>
    <asp:GridView runat="server" ID="gvHG" AutoGenerateColumns="false" CssClass="table table-striped table-condensed" GridLines="None">	        
	    <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
	    <Columns>
            <asp:TemplateField Visible="false">
                <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <ItemTemplate><asp:Label runat="server" ID="lblMesAnno"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Mes"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblMes" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" Text="HG"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Imagebutton runat="server" id="imgHG" CommandName="ver"></asp:Imagebutton></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                <HeaderTemplate><asp:Label runat="server" Text="Visas"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Imagebutton runat="server" id="imgGastosVisa" CommandName="ver"></asp:Imagebutton></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
			    <HeaderTemplate><asp:Label runat="server" Text="Num Hoja"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label ID="lblSinIdViaje" runat="server"></asp:Label></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Estado"></asp:Label></HeaderTemplate>
			    <ItemTemplate><b><asp:Label ID="lblEstado" runat="server" style="font-size:14px;"></asp:Label></b></ItemTemplate>
		    </asp:TemplateField>            
	    </Columns>
    </asp:GridView>         
</asp:Content>
