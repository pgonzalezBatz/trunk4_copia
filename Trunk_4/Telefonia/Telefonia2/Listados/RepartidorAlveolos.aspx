<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master" CodeBehind="RepartidorAlveolos.aspx.vb" Inherits="Telefonia.RepartidorAlveolos"  %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
     <tit:Titulo runat="server" Texto="repartidor" />
     <table runat="server" id="tAlveolos" class="tabla1" width="800px">                                                                                               
        <thead>
            <tr>
               <th>&nbsp;</th> 
               <th>&nbsp;</th>
               <th><asp:Label runat="server" Text="CNX-1"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-2"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-3"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-4"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-5"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-6"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-7"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-8"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-9"></asp:Label></th>
               <th><asp:Label runat="server" Text="CNX-10"></asp:Label></th>
            </tr>
        </thead>         
     </table>
</asp:Content>
