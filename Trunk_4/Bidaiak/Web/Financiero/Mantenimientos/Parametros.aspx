<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Parametros.aspx.vb" Inherits="WebRaiz.Parametros" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">         
     <asp:Label runat="server" ID="labelInfo" Text="Configuracion de los distintos parametros que requiere la aplicacion"></asp:Label><br /><br />
     <div class="form-group">
         <asp:Label runat="server" ID="labelTitPrecioKm" Text="Indica el precio al que se paga el kilometraje que luego sera utilizado en las hojas de gastos al meter lineas relacionadas con el kilometraje"></asp:Label>
     </div>
    <div class="row">
        <div class="col-sm-2"><b><asp:Label runat="server" ID="labelPrecioKm" Text="Precio km"></asp:Label></b></div>
        <div class="col-sm-4">
            <asp:TextBox runat="server" ID="txtPrecioKm" CssClass="form-control"></asp:TextBox>
            <ajax:FilteredTextBoxExtender ID="ftbPrecioKm" runat="server" TargetControlID="txtPrecioKm" FilterType="Numbers,Custom" ValidChars=".," />
        </div>
        <div class="col-sm-2"><asp:Label runat="server" ID="labelIdConcep" Text="Concepto asociado"></asp:Label></div>
        <div class="col-sm-4"><asp:DropDownList runat="server" ID="ddlConceptoKm" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id" CssClass="form-control"></asp:DropDownList></div>
    </div><hr />
    <div class="form-group">
        <asp:Label runat="server" ID="labelTitCodAgencia" Text="Este codigo sera utilizado para realizar el asiento contable, al importar la factura de la agencia de viajes"></asp:Label>
    </div>  
     <div class="row">
        <div class="col-sm-2"><b><asp:Label runat="server" ID="labelCodAgencia" Text="Codigo proveedor de agencia"></asp:Label></b></div>
        <div class="col-sm-4"><asp:TextBox runat="server" ID="txtCodProvAgencia" MaxLength="20" CssClass="form-control"></asp:TextBox></div>
    </div><hr />
    <div class="form-group">
        <asp:Label runat="server" ID="labelTitDiasCaduc" Text="Nº de dias a partir de los cuales, se considerara un viaje caducado. Este dato podra tener distintas utilizaciones. Un dato por defecto, se podria considerar 30 dias"></asp:Label>
    </div>  
     <div class="row">
        <div class="col-sm-2"><b><asp:Label runat="server" ID="labelDiasCaduc" Text="Dias de caducidad de un viaje"></asp:Label></b></div>
        <div class="col-sm-4">
            <asp:TextBox runat="server" ID="txtCaducidadViaje" MaxLength="3" CssClass="form-control"></asp:TextBox>
            <ajax:FilteredTextBoxExtender ID="ftbCaducidadViaje" runat="server" TargetControlID="txtCaducidadViaje" FilterType="Numbers"/>
        </div>
    </div><hr />
    <div class="form-group">
        <asp:Label runat="server" ID="labelTitDiasSolicAntic" Text="Nº de dias minimos necesarios para poder solicitar un anticipo"></asp:Label>
    </div>  
     <div class="row">
        <div class="col-sm-2"><b><asp:Label runat="server" ID="labelDiasSolicAntic" Text="Dias de solicitud de anticipo"></asp:Label></b></div>
        <div class="col-sm-4">
            <asp:TextBox runat="server" ID="txtDiasAnticipo" MaxLength="3" CssClass="form-control"></asp:TextBox>
            <ajax:FilteredTextBoxExtender ID="ftbDiasAntic" runat="server" TargetControlID="txtDiasAnticipo" FilterType="Numbers"/>
        </div>
    </div><br />   
    <div class="form-group">
        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Save" CssClass="form-control btn btn-primary" />
    </div>        
</asp:Content>
