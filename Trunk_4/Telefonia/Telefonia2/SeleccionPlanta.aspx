<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPSinMenu.master" CodeBehind="SeleccionPlanta.aspx.vb" Inherits="Telefonia.SeleccionPlanta" %>
<%@ MasterType VirtualPath="~/MPSinMenu.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
<div>
   &nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="labelTitulo" CssClass="Titulo" Text="plantaAdministracion"></asp:Label><br /><br />&nbsp;&nbsp;&nbsp;
   <fieldset style="width:50%">
        <asp:Label runat="server" ID="labelSelPlanta" Text="seleccionePlanta"></asp:Label>&nbsp;&nbsp;
        <asp:DropDownList runat="server" ID="ddlPlantas" AppendDataBoundItems="true"></asp:DropDownList>&nbsp;
        <asp:Button runat="server" ID="btnIr" Text="entrar" />
   </fieldset>
</div>
</asp:Content>
