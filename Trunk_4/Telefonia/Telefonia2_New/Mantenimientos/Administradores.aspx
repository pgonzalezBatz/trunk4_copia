<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="Administradores.aspx.vb" Inherits="Telefonia.Administradores" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">      
     <script type="text/javascript" src="../js/Utiles.js"></script>    
     <asp:UpdatePanel runat="server">
        <contenttemplate> 
           <fieldset style="width:500px;">
             <table cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="3">
                      <asp:Panel runat="server" id="pnlUsuarios">
                             <asp:Label runat="server" ID="labelUsers" text="usuarios"  />&nbsp;
                            <asp:DropDownList ID="ddlUsuarios" runat="server" CssClass="mayusculas" />&nbsp;
                            <asp:ImageButton id="imgAgregar" runat="server" imageUrl="~/App_Themes/Tema1/Images/agregar.gif" ToolTip="agregar" />
                       </asp:Panel>
                       <asp:Panel runat="server" id="pnlSinUsuarios">
                            <asp:Label runat="server" ID="lblMensaje" text="noExistenMasUsuarios" />
                       </asp:Panel>
                    </td>
                </tr>
                <tr><td colspan="3">&nbsp;</td></tr>
                <tr>
                    <td>
                         <asp:Repeater ID="rptAdministradores" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><asp:Image ID="imgBullet" runat="server" ImageUrl="~/App_Themes/Tema1/Images/bullet_hl.gif" /></td>
                                    <td><asp:Literal runat="server" Text='<%#Eval("NombreCompleto")%>'></asp:Literal></td>
                                    <td><asp:ImageButton ID="imgBorrar" CommandArgument='<%#Eval("Id")%>' CommandName='<%#Eval("NombreCompleto")%>' OnClientClick="return confirm('¿Desea quitar al administrador de la lista?');" OnClick="DeleteAdministradorPlanta" ImageUrl="~/App_Themes/Tema1/Images/cancelar.gif" runat="server" ToolTip="Eliminar" /></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
             </table>
             </fieldset>
        </contenttemplate>
     </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
</asp:Content>