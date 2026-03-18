<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubirFactura.ascx.vb" Inherits="WebRaiz.SubirFactura" %>
    <fieldset style="width:700px">        
        <br /><div>
            <asp:Label runat="server" ID="labelInfo1" Text="Seleccione el fichero de facturas que contiene los albaranes (Formato .xlsx)" CssClass="labelDetalle"></asp:Label><br /><br />
            <asp:Label runat="server" ID="labelInfo2" Text="Seleccione un fichero"></asp:Label>:<asp:FileUpload ID="fuFichero" runat="server" />&nbsp;
            (<asp:Label runat="server" ID="labelInfo3" Text="tamañoMaximo10M" CssClass="font10"></asp:Label>)<br />
        </div>
        <br />
        <div id="botones">
            <asp:Imagebutton runat="server" ID="imgSubir" Tooltip="subirFichero" ImageUrl="~/App_Themes/Tema1/IconosBotones/Upload.png" />
        </div>
    </fieldset>