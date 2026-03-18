<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="SelectPlant.aspx.vb" Inherits="WebRaiz.SelectPlant" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <div class="form-inline">
        <div class="form-group">
            <asp:Label runat="server" ID="labelInfo" Text="Seleccione la planta que va a gestionar"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlPlantas" DataTextField="Text" DataValueField="Value" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnEntrar" Text="Entrar" CssClass="form-control btn btn-primary" />
        </div>
    </div>
</asp:Content>
