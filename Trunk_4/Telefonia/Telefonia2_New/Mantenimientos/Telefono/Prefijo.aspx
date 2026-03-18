<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="Prefijo.aspx.vb" Inherits="Telefonia.Prefijo" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script src="../../js/Utiles.js" type="text/javascript"></script>   
    <asp:Label runat="server" ID="labelInfo" Text="Introduzca el prefijo de los telefonos de la planta. Si se informa, en la busqueda de telefonos se le antepondra este prefijo"></asp:Label><br /><br />
    <asp:Label runat="server" ID="labelPrefijo" Text="Prefijo"></asp:Label>&nbsp;
    <asp:TextBox runat="server" ID="txtPrefijo" Columns="5" MaxLength="10" style="text-align:center" onkeydown="return soloNumeros(event);"></asp:TextBox>
    <asp:Label runat="server" ID="labelCom" Text="No introducir el simbolo +, solo los numeros" CssClass="font11"></asp:Label><br /><br />
    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" style="margin-left:50px" />
</asp:Content>
