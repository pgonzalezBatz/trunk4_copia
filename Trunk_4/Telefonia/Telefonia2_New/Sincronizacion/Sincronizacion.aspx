<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="Sincronizacion.aspx.vb" Inherits="Telefonia.Sincronizacion" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">  
     <asp:GridView runat="server" ID="gvSincro" AutoGenerateColumns="false" CssClass="GridView" Width="75%">
        <HeaderStyle CssClass="GridViewHeaderStyle" />
        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
        <RowStyle CssClass="GridViewRowStyle" />
        <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
        <EmptyDataTemplate><br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" text="noExisteNadaActualizar"></asp:Label></EmptyDataTemplate>
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" text="estado"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" text="baja" CssClass="labelRojo"></asp:Label></ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField Visible="false">
                <HeaderTemplate>&nbsp;</HeaderTemplate>
                <ItemTemplate><asp:Label ID="lblId" runat="server" Text='<%#Container.DataItem(2)%>' ></asp:Label></ItemTemplate>
            </asp:TemplateField>           
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" text="nombre"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" Text='<%#Container.DataItem(3)%>'></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" text="extensiones/telefonosAsignados"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" id="lblExtenTlfno"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" text="accion"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label ID="lblAccion" runat="server"></asp:Label></ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
