<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="OrdenarItemsArea.aspx.vb" Inherits="WebRaiz.OrdenarItemsArea" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <tit:Titulo ID="myTitle" runat="server" />
    <asp:Label runat="server" ID="labelInfo" Text="Seleccionar el orden que tendran los elementos"></asp:Label><br /><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">        
        <ContentTemplate>
            <table>
                <tr>
                    <td><asp:Label runat="server" id="labelNeg" Text="Negocio"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlNegocios" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" id="labelArea" Text="Area"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlAreas" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList></td>
                </tr>
            </table>
            <ajax:reorderlist id="rlItems" runat="server" draghandlealignment="Left" enableviewstate="true" ItemInsertLocation="End" AllowReorder="true" PostBackOnReorder="true" CssClass="ReOrderList" clientidmode="AutoID" >
            <ItemTemplate>
                <div class="ClsItemArea">
                    <table style="width: 100%;">
                        <tr>
                            <td runat="server" id="myTd">
                                <asp:Label ID="litItem" runat="server" CssClass="mayusculas"></asp:Label>
                                <asp:Panel runat="server" ID="pnlContent"></asp:Panel>
                                <asp:HiddenField ID="hfItem" runat="server" />
                                <asp:HiddenField ID="hfOrden" runat="server" />                                    
                            </td>                                    
                        </tr>
                    </table>
                </div>
            </ItemTemplate>
            <EmptyListTemplate>
                <asp:Label runat="server" CssClass="negrita" text="Todavia no se ha añadido ningun elemento. Vuelva atras y cuando tenga elementos, vuelva para ordenarlos"></asp:Label>
            </EmptyListTemplate>
            <ReorderTemplate>
                <asp:Panel ID="Panel2" runat="server" CssClass="ClsReorderCue"></asp:Panel>
            </ReorderTemplate>
            <DragHandleTemplate>
                <div class="ClsDragHandle"></div>
            </DragHandleTemplate>
        </ajax:reorderlist>           
        </ContentTemplate>
    </asp:UpdatePanel>  
    <br />
    <div id="botones">
        <asp:Button runat="server" id="btnVolver" CssClass="boton" Text="Volver" />        
    </div>  
</asp:Content>
