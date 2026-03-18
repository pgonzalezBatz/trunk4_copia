<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPKEM.master" CodeBehind="SeleccionPlanta.aspx.vb" Inherits="KEM.SeleccionPlanta" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Master/MPKEM.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
<div>
   <tit:Titulo ID="tit" runat="server" Texto="plantaAdministracion" />&nbsp;&nbsp;
   <fieldset style="width:50%">
        <asp:Label runat="server" ID="labelPlanta" Text="seleccionePlanta"></asp:Label>&nbsp;&nbsp;
        <asp:DropDownList runat="server" ID="ddlPlantas" AppendDataBoundItems="true"></asp:DropDownList>&nbsp;
        <asp:Button runat="server" ID="btnIr" Text="entrar" />
   </fieldset>
</div>
</asp:Content>
