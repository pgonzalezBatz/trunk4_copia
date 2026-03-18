<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPKEM.Master" CodeBehind="DarBajaResponsable.aspx.vb" Inherits="KEM.DarBajaResponsable" %>
<%@ Register src="~/Controls/AutocompleteGV.ascx" tagname="AutoCompleteGV" tagprefix="uc1" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Master/MPKEM.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>            
    <script src="../js/jQuery/autocompleteGV.js" type="text/javascript"></script> 
    <tit:Titulo runat="server" Texto="Baja de usuario" />
    <asp:Label runat="server" ID="labelInfo" Text="El usuario a dar de baja, tiene gente a su cargo. Por defecto, se les va a asignar el gerente de la planta como responsable aunque se puede cambiar a continuacion"></asp:Label><br /><br /><br />
    <asp:Label runat="server" ID="labelUserBaja" Text="Usuario a dar de baja"></asp:Label>
    <asp:Label runat="server" ID="lblUserBaja" style="font-weight:bold;margin-left:10px;"></asp:Label><hr />
    <table class="tablaMediana2">
        <asp:Repeater runat="server" id="rptUsers">
            <HeaderTemplate>
                <tr>
                    <th><asp:Label runat="server" text="Usuario"></asp:Label></th>
                    <th><asp:Label runat="server" Text="Nuevo responsable"></asp:Label></th>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td valign="top">
                        <asp:Label runat="server" ID="lblUsuario"></asp:Label>
                        <asp:HiddenField runat="server" ID="hfIdUser" />
                    </td>
                    <td valign="top"><uc1:AutoCompleteGV ID="acGV" runat="server" PostBack="false" ShowButton="False" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="a" ValueName="b" GridviewClass="popupTable" MinSearchLength="3" Opcion="user" SoloActivos="true" MaxDivHeight="250" /></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table><br />
    <div id="botones">
        <asp:Button runat="server" ID="btnGuardar" Text="Dar de baja y guardar responsables" />
        <asp:Button runat="server" ID="btnVolver" Text="Volver" style="margin-left:15px;" />
    </div>
</asp:Content>
