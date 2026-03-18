<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubirVisas.ascx.vb" Inherits="WebRaiz.SubirVisas" %>
 <fieldset style="width:650px">
    <br /><div>
        <asp:Label runat="server" ID="labelInfo1" Text="Seleccione el fichero descargado del banco que contiene los movimientos de visa" CssClass="labelDetalle"></asp:Label><br /><br />
        <asp:Label runat="server" ID="labelInfo2" Text="Extensiones admitidas" CssClass="labelDetalle" style="text-transform:uppercase"></asp:Label>&nbsp;<asp:Label runat="server" Text="TXT" CssClass="labelDetalle"></asp:Label><br /><br />
        <asp:Label runat="server" ID="labelInfo3" Text="Seleccione un fichero"></asp:Label>:
        <asp:FileUpload ID="fuFichero" runat="server" />
        &nbsp; (<asp:Label runat="server" ID="labelTamMax" Text="tamañoMaximo10M" CssClass="font10"></asp:Label>)<br />
    </div><br />
    <div id="botones"><asp:Imagebutton runat="server" ID="imgSubir" Tooltip="subirFichero" ImageUrl="~/App_Themes/Tema1/IconosBotones/Upload.png" /></div>
</fieldset>