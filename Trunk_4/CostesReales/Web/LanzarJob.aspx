<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="LanzarJob.aspx.vb" Inherits="CostesReales.LanzarJob" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="cnt1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:Panel runat="server" ID="pnlSinEjecucion">
        <div style="margin-left:20px">
            <asp:DropDownList ID="jobNames" runat="server"></asp:DropDownList>
            <asp:Button ID="btnEjecutar" runat="server" Text="Ejecutar" />
            <br />
            <asp:Label ID="lbResult" runat="server"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlConEjecucion">
        <h2><asp:Label runat="server" ID="labelInfo" style="color:#2a64cc"></asp:Label></h2>
    </asp:Panel>
</asp:Content>