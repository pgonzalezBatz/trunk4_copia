<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="ImportarExportar.aspx.vb" Inherits="Telefonia.ImportarExportar" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:Panel runat="server" ID="pnlAccion" GroupingText="Accion a realizar" Width="300px">
        <asp:RadioButtonList runat="server" ID="rdbAccion" AutoPostBack="true" RepeatDirection="Horizontal" CellPadding="5" CellSpacing="5">
            <asp:ListItem Text="Importar" Value="1" Selected="False"></asp:ListItem>
            <asp:ListItem Text="Exportar" Value="2" Selected="False"></asp:ListItem>
        </asp:RadioButtonList>
    </asp:Panel><br />
    <asp:MultiView runat="server" ID="mvAcciones">
        <asp:View runat="server" ID="vImportar">
            <asp:Label runat="server" ID="labelImport" Text="Seleccione el fichero CSV que contiene la guia de telefonos de Matrici a importar" /><br /><br />
            <asp:Label runat="server" ID="labelFichero" Text="Fichero"></asp:Label>&nbsp;
            <asp:FileUpload runat="server" ID="fuMatrici" /><br /><br />
            <asp:Button runat="server" ID="btnImportar" Text="Importar" style="margin-left:30px;" />
             <asp:Panel runat="server" ID="pnlResulImportar">
                <br />
                <asp:Label runat="server" ID="lblMensaImportar" CssClass="negrita"></asp:Label>
            </asp:Panel>
        </asp:View>
        <asp:View runat="server" ID="vExportar">
            <asp:Label runat="server" ID="labelExport" Text="Exporta la guia de telefonos activos y visibles de Batz a un ficheros CSV" /><br /><br />
            <asp:Button runat="server" ID="btnExportar" Text="Exportar" style="margin-left:30px;" />
            <asp:Panel runat="server" ID="pnlResulExportar">
                <br />
                <asp:Label runat="server" ID="lblMensaExportar" CssClass="negrita"></asp:Label>
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
