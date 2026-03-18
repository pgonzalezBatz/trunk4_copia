<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Informes.aspx.vb" Inherits="WebRaiz.Informes" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
     <tit:Titulo runat="server" Texto="Informes" />
    <asp:Label runat="server" ID="labelInfo" Text="El informe se descargara automaticamente en Excel en la carpeta descargas"></asp:Label><br />
    <ul>        
        <li><asp:LinkButton runat="server" ID="lnkVerKPIBMS" Text="KPI BMS" /></li>
        <li><asp:LinkButton runat="server" ID="lnkVerKPICommittee" Text="KPI por comite" /></li>
    </ul>
    <%--<table>
        <tr>
            <td><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ddlPlantas" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="labelNegocio" Text="Negocio"></asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ddlNegocios" DataTextField="NombreNegocio" DataValueField="IdNegocio" AppendDataBoundItems="true"></asp:DropDownList></td>
        </tr>
    </table><br />
    <asp:Button runat="server" ID="btnVer" Text="Ver informe" />--%>
</asp:Content>
