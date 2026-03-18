<%@ Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ListadoFormacionISO.aspx.vb" Inherits="WebRaiz.ListadoFormacionISO" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
     <div class="form-group">
        <asp:Label runat="server" ID="labelSOS" text="Listado de personas que han viajado entre fechas y que nunca han recibido la formacion de ISOS"></asp:Label>        
    </div>
    <div class="row">
        <div class="col-sm-2 form-inline">
            <asp:Label runat="server" ID="labelFIni" Text="Fecha inicio"></asp:Label>
        </div>
        <div class="col-sm-4">
            <div class="input-group date" id="dtFechaIni">
                <asp:TextBox runat="server" ID="txtFechaInicio" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
        <div class="col-sm-2"><asp:Label runat="server" ID="labelFFin" text="FechaFin"></asp:Label></div>
        <div class="col-sm-4">
            <div class="input-group date" id="dtFechaFin">
                <asp:TextBox runat="server" ID="txtFechaFin" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-2"><asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="form-control btn btn-primary" /></div>        
    </div>   
     <asp:GridView runat="server" ID="gvPersonas" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" CssClass="table table-striped table-condensed table-hover" GridLines="None" PageSize="20">	    
        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
        <PagerSettings PageButtonCount="5" />
	    <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
	    <Columns>
            <asp:BoundField HeaderText="Nº trabajador" DataField="CodPersona" SortExpression="CodPersona" />
            <asp:BoundField HeaderText="Nombre persona" DataField="NombreCompleto" SortExpression="NombreCompleto" />
            <asp:BoundField HeaderText="Nº viajes" DataField="NumViajes" SortExpression="NumViajes" />		             
	    </Columns>
    </asp:GridView>
</asp:Content>