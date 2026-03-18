<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="DocIntereses.aspx.vb" Inherits="Nominas.DocIntereses" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:Panel runat="server" ID="pnlInfo">
        <div class="row">
            <div class="col-sm-1 col-xs-2"><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label>:</div>
            <div class="col-sm-11 col-xs-10"><asp:Label runat="server" ID="lblPlanta" style="font-weight:bold"></asp:Label></div>
        </div><br />
        <asp:Label runat="server" ID="labelInfo2" Text="Este proceso, cogera el pdf con todos los intereses y realizara las siguientes funciones"></asp:Label><br />
        <ul>
            <li><asp:Label runat="server" ID="labelInfo3" Text="Partira el pdf original en tantos pdf como DNI encuentre"></asp:Label><br /></li>
            <li><asp:Label runat="server" ID="labelInfo4"  Text="Protegera los pdfs individuales con el DNI como contraseña"></asp:Label><br /></li>        
            <li><asp:Label runat="server" ID="labelInfo5" Text="Enviara por email a todas las personas con el documento de intereses adjunto"></asp:Label><br /></li>
        </ul>
        <div class="text-justify"><asp:Label runat="server" ID="labelSelFich" Text="Seleccione el fichero de los intereses y realice primero un testeo. Una vez que la simulacion sea correcta, vuelva a seleccionar el fichero y ejecute el proceso" CssClass="negrita"></asp:Label></div><br />
        <div class="row">
            <div class="col-sm-4"><asp:Label runat="server" ID="labelSelEmail" Text="Email al que se le avisara cuando finalice el proceso"></asp:Label></div>
            <div class="col-sm-8"><asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox></div>
        </div>
        <div class="row">
            <div class="col-sm-4"><asp:Label runat="server" ID="labelSelPdf" Text="Seleccione el pdf con los intereses"></asp:Label></div>
            <div class="col-sm-8"><asp:FileUpload runat="server" ID="fuDocumento" /></div>
        </div><br />
        <div class="form-inline">
            <asp:Button runat="server" ID="btnTestear" Text="Testear proceso" ToolTip="Hace una simulacion del resultado que daria al procesar para poder quitar errores" CssClass="btn btn-primary" />
            <asp:Button runat="server" ID="btnProcesar" Text="Procesar" ToolTip="Inicia el proceso" CssClass="btn btn-primary" />  
        </div><br /><br /> 
    </asp:Panel>
    <asp:Label runat="server" ID="labelProcesando" text="Se estan procesando los documentos de intereses. Cuando finalice se le avisara al email especificado. Si quiere puede actualizar para comprobar si ha terminado" style="font-weight:bold;font-size:20px;"></asp:Label>
    <asp:Panel runat="server" ID="pnlUltimaEjecucion" CssClass="panel panel-default">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="lblResul" Text="Resultado" style="text-transform:uppercase"></asp:Label></strong>
        </div>
        <div><asp:Label runat="server" ID="lblMensa"></asp:Label></div>	
	</asp:Panel>
</asp:Content>
