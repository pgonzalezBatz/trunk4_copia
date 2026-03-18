<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="SeleccionPlanta.aspx.vb" Inherits="Telefonia.SeleccionPlanta" %>
<%@ MasterType VirtualPath="~/MPTelefonia.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server"> 
  <div class="form-inline">
        <div class="form-group">
            <asp:Label runat="server" ID="labelInfo" Text="Seleccione la planta que va a gestionar"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlPlantas" DataTextField="Nombre" DataValueField="Id" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnEntrar" Text="Entrar" CssClass="form-control btn btn-primary" />
        </div>
    </div>
</asp:Content>
