<%@ Page Language="vb" CodeBehind="RolesUsuarios.aspx.vb"  Inherits="ReferenciasVentas.RolesUsuarios" MasterPageFile="~/RefSis.Master"  %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">         
    <Titulo:Titulo ID="titProducto" Texto="Users and roles" runat="server" />
    
    <asp:UpdatePanel ID="upUsersRoles" runat="server">
        <ContentTemplate>
            <table style="width:auto">
                <tr>
                    <td>
                        <img id="imgInfo" alt="Advertencia" src="../../App_Themes/Batz/Imagenes/info.png" />
                    </td>
                    <td>
                        <asp:Label ID="lblAdvertencia" runat="server" Text="The roles of the users in Selling Part Numbers application may differ from Systems Timecards application roles" Font-Italic="true" Font-Bold="true" Font-Size="14px" />
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="background-color:#5D7B9D; color:White;width:40px; text-align:center">
                        <asp:Label ID="lblRol" runat="server" Text="Role" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRol" runat="server" OnSelectedIndexChanged="ddlRol_SelectedIndexChanged" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gvRolUsuario" runat="Server" AutoGenerateColumns="False" CssClass="GridViewASP"       
                GridLines="None"  DataKeyNames="Id" Width="20%">
                <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" Height="40" VerticalAlign="Middle" />
                <PagerStyle CssClass="PagerStyle" />
                <SelectedRowStyle CssClass="SelectedRowStyle" />
                <HeaderStyle CssClass="HeaderStyle" />
                <EditRowStyle CssClass="EditRowStyle" />
                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                <EmptyDataRowStyle CssClass="EmptyRowStyle" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                    <asp:TemplateField HeaderText="Users" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" Text='<%# Eval("Nombre")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>                  
                </Columns>        
            </asp:GridView>
            <br />
            <asp:GridView ID="gvRolUsuarioExcepcion" runat="Server" AutoGenerateColumns="False" CssClass="GridViewASP"       
                GridLines="None"  Width="20%">
                <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" Height="40" VerticalAlign="Middle" />
                <PagerStyle CssClass="PagerStyle" />
                <SelectedRowStyle CssClass="SelectedRowStyle" />
                <HeaderStyle CssClass="HeaderStyle" />
                <EditRowStyle CssClass="EditRowStyle" />
                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                <EmptyDataRowStyle CssClass="EmptyRowStyle" />
                <Columns>
                    <asp:BoundField DataField="Key" HeaderText="Other users" ItemStyle-HorizontalAlign="Left" />                
                </Columns>        
            </asp:GridView>

            <%--<asp:ListView ID="lvRolesUsuarios" runat="server" ItemPlaceholderID="myItemPlaceHolder">
                <ItemTemplate>
                    <li>
                        <asp:Label runat="server" Text='<%# Eval("Nombre") %>' />                        
                    </li>
                </ItemTemplate>
                <LayoutTemplate>
                    <ul class="productlist">
                        <asp:PlaceHolder ID="myItemPlaceHolder" runat="server" />                        
                    </ul>
                </LayoutTemplate>
            </asp:ListView>--%>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>
