<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ListadoNotificaciones.aspx.vb" Inherits="GTK_Troqueleria.ListadoNotificaciones" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">

<%--    <asp:Panel ID="Panel1" runat="server" CssClass="Comentario" Style="vertical-align: middle;">
        <asp:Label ID="Label1" runat="server" Text="Listado de usuarios a los que se notificara si la NC esta relacionada con la UG."></asp:Label>
    </asp:Panel>--%>

    <asp:Panel ID="pnlBotonesGv" runat="server" CssClass="PanelBotones">
        <asp:ImageButton ID="btnNuevaNotificacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo24.png" AlternateText="Nueva Notificacion" ToolTip="Nueva Notificacion" PostBackUrl="~/Mantenimiento/Notificaciones/Usuario.aspx" />
    </asp:Panel>

    <asp:DataList ID="dlNotificaciones" runat="server" GridLines="None" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4" Width="96%">
        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="300px" />
        <ItemTemplate>
            <asp:GridView SkinID="GridView" ID="gvNotificaciones" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" AutoGenerateColumns="false" AllowPaging="false" OnRowDeleting="gvNotificaciones_RowDeleting">
                <Columns>
                    <%--<asp:CommandField ShowDeleteButton="true" ButtonType="Link" ItemStyle-Width="1%" />
                    <asp:CommandField ShowSelectButton="True" ShowEditButton="true" />--%>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnBorrar" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Eliminar"  />
                            <act:ConfirmButtonExtender ID="btnBorrar_ConfirmButtonExtender" runat="server" TargetControlID="btnBorrar" ConfirmText="Desea eliminar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ItemTemplate>
    </asp:DataList>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
