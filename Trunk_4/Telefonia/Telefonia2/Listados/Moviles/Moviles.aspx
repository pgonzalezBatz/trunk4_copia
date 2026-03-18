<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master" CodeBehind="Moviles.aspx.vb" Inherits="Telefonia.Moviles" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
     <script src="../../js/Utiles.js" type="text/javascript"></script>
    <tit:Titulo runat="server" Texto="moviles" />
    <asp:GridView runat="server" ID="gvMoviles" AutoGenerateColumns="false" AllowSorting="True"
         CssClass="GridView" Width="800px">
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
            <asp:TemplateField SortExpression="TlfnoMovil">
                <HeaderTemplate>
                    <asp:LinkButton runat="server" text="numeroMovil" CommandName="Sort" CommandArgument="TlfnoMovil"></asp:LinkButton>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#Eval("TlfnoMovil")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="ExtensionMovil">
                <HeaderTemplate>
                    <asp:LinkButton runat="server" text="extensionMovil" CommandName="Sort" CommandArgument="ExtensionMovil"></asp:LinkButton>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblExtMovil"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" text="relacion"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" id="lblRelacion"></asp:Label>
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
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                     <asp:Label runat="server" text="visible"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox runat="server" id="chbVisible" Enabled="false" />                    
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
