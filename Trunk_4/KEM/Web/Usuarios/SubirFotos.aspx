<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPKEM.Master" CodeBehind="SubirFotos.aspx.vb" Inherits="KEM.SubirFotos" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Master/MPKEM.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <tit:Titulo runat="server" Texto="Foto" /> 
    <fieldset style="width:500px">
        <asp:Label runat="server" ID="labelSubirFoto" Text="Subir foto" CssClass="negrita"></asp:Label><br />
        <asp:FileUpload ID="fuFoto" runat="server" />&nbsp;&nbsp;
        <asp:ImageButton runat="server" id="imgSubir" Text="subir" ImageUrl="~/App_Themes/Tema1/Images/agregar.gif" ImageAlign="AbsMiddle" /><br />
        <asp:Panel ID="pnlFotoExistente" runat="server"><asp:Label runat="server" ID="labelInfo" Text="Ya existe una foto, si sube otra, se reemplazara y perdera la antigua información"></asp:Label><br /></asp:Panel>
    </fieldset><br /><br />
    <asp:Panel runat="server" ID="pnlMostrarEliminar">
        <fieldset style="width:500px">
            <asp:Label runat="server" ID="labelInfo2" Text="Eliminar la foto del usuario" CssClass="negrita"></asp:Label>&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnEliminar" Text="eliminar" />    
         </fieldset>
     </asp:Panel><br />
</asp:Content>
