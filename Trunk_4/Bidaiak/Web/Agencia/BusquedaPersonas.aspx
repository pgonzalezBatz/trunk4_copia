<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="BusquedaPersonas.aspx.vb" Inherits="WebRaiz.BusquedaPersonas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <uc:Busqueda ID="searchUser" runat="server" PostBack="true" SoloActivos="true" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="id" ValueName="no" GridviewClass="table table-striped table-bordered table-hover" MinSearchLength="3" Opcion="user" /><br /><br />
            <asp:Panel runat="server" ID="pnlInfo">
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelCodTrab" Text="IdTrabajador"></asp:Label></div>
                    <div class="col-sm-10"><b><asp:Label ID="lblCodPerso" runat="server"></asp:Label></b></div>
                </div>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelNombre" Text="nombrePersona"></asp:Label></div>
                    <div class="col-sm-10"><b><asp:Label ID="lblNombre" runat="server"></asp:Label></b></div>
                </div>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelDpto" Text="departamento"></asp:Label></div>
                    <div class="col-sm-10"><b><asp:Label ID="lblDepartamento" runat="server"></asp:Label></b></div>
                </div>
                <div class="row">
                    <div class="col-sm-2"><asp:Label runat="server" ID="labelEmail" Text="Email"></asp:Label></div>
                    <div class="col-sm-10"><b><asp:Label ID="lblEmail" runat="server"></asp:Label></b></div>
                </div>
            </asp:Panel>    
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>