<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master"
    CodeBehind="NumerosDirectos.aspx.vb" Inherits="Telefonia.NumerosDirectos" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script src="../js/Utiles.js" type="text/javascript"></script>
    <tit:Titulo runat="server" Texto="listadoNumerosDirectos" />
    <asp:GridView runat="server" ID="gvDirectos" AutoGenerateColumns="false" AllowSorting="True"
         CssClass="GridView" Width="75%" EnableViewState="false">
        <HeaderStyle CssClass="GridViewHeaderStyle" />
        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
        <RowStyle CssClass="GridViewRowStyle" />
        <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
        <EmptyDataTemplate>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label>
        </EmptyDataTemplate>
        <Columns>            
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" text="numero"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#Eval("TlfnoDirecto")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label  runat="server" text="extension"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#FormatInt(Eval("ExtensionInterna"))%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" text="descripcion"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#Eval("Nombre")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" text="comentarios"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#Eval("Comentarios")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
