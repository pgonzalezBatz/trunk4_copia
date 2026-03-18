<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Perfiles.aspx.vb" Inherits="WebRaiz.Perfiles" %>
<%@ Register src="~/Controles/SearchItems.ascx" tagname="SearchItem" tagprefix="uc1" %>
<%@ Register src="~/Controles/PanelCargandoDatos.ascx" tagname="CargandoDatos" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
     <script src="../Js/jQuery/jquery.js" type="text/javascript"></script>         
    <tit:Titulo runat="server" Texto="Perfiles" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
            <table>
                <tr>
                    <td valign="top"><asp:Label runat="server" ID="labelUsuario" Text="Usuario"></asp:Label></td>
                    <td colspan="2"><uc1:SearchItem ID="searchUser" runat="server" PostBack="false" Opcion="user" /></td>            
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></td>
                    <td colspan="2"><asp:DropDownList runat="server" ID="ddlPlantas" DataTextField="Nombre" DataValueField="Id"></asp:DropDownList></td>            
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="labelNegocio" Text="Negocio"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlNegocios" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true"></asp:DropDownList></td>
                    <td><asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="boton" ToolTip="Buscar perfiles segun los filtros rellenados" /></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="labelArea" Text="Area"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlAreas" DataTextField="Nombre" DataValueField="Id"></asp:DropDownList></td>
                    <td><asp:Button runat="server" ID="btnAnadir" Text="Añadir perfil" CssClass="boton" /></td>
                </tr>
            </table><br />
            <asp:GridView runat="server" ID="gvPerfiles" AutoGenerateColumns="false" CssClass="Gridview"
                Width="75%" AllowSorting="true" AllowPaging="true" PageSize="15" PagerSettings-Mode="NumericFirstLast">
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <PagerStyle HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />
                <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" text="noExisteNingunRegistro"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" SortExpression="NombreUsuario" />
                    <asp:BoundField DataField="NombrePlanta" HeaderText="Planta" SortExpression="NombrePlanta" />
                    <asp:BoundField DataField="NombreNegocio" HeaderText="Negocio" SortExpression="NombreNegocio" />
                    <asp:BoundField DataField="NombreArea" HeaderText="Area" SortExpression="NombreArea" />
                    <asp:TemplateField ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><asp:ImageButton runat="server" ID="imgDelete" ToolTip="Eliminar" Imageurl="~/App_Themes/Tema1/IconosAcciones/Cancelar.gif" CommandName="Del" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc1:CargandoDatos runat="server"></uc1:CargandoDatos>
</asp:Content>