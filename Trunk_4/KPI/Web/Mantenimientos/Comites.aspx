<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Comites.aspx.vb" Inherits="WebRaiz.Comites" %>
<%@ Register src="~/Controles/PanelCargandoDatos.ascx" tagname="CargandoDatos" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <tit:Titulo runat="server" Texto="Indicadores por comite" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:MultiView ID="mvComites" runat="server" ActiveViewIndex="0">
                <asp:View runat="server" ID="vwListado">
                    <fieldset style="width: 700px">
                        <asp:DropDownList runat="server" ID="ddlComite" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true" AppendDataBoundItems="true"></asp:DropDownList>
                        <asp:LinkButton runat="server" ID="lnkNuevo" text="Nuevo" style="margin-left:15px;"></asp:LinkButton>                
                        <asp:LinkButton runat="server" ID="lnkEditar" text="Editar" style="margin-left:15px;"></asp:LinkButton>                
                    </fieldset><br />
                    <asp:Panel runat="server" ID="pnlIndicadores">
                        <asp:Button runat="server" ID="btnGuardarIndUp" Text="Guardar" /><br /><br />
                        <table style="width:30%">
                            <asp:Repeater runat="server" id="rptIndicadores">
                                <ItemTemplate>                        
                                    <tr runat="server" id="trNegArea" class="CabeceraDiv">
                                        <td><asp:Label runat="server" ID="lblNegArea"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:CheckBox runat="server" ID="chbIndicador" /></td>
                                    </tr>                                                                        
                                </ItemTemplate>
                            </asp:Repeater>                
                        </table><br />
                        <asp:Button runat="server" ID="btnGuardarIndDown" Text="Guardar" />
                    </asp:Panel>
                </asp:View>
                <asp:View runat="server" ID="vwDetalle">                   
                     <table>
                         <tr>
                             <td style="width:100px"><asp:Label ID="labelComite" runat="server" Text="Nombre"></asp:Label></td>
                             <td><asp:textbox runat="server" ID="txtComite" Columns="50"></asp:textbox></td>
                         </tr>                                                 
                     </table><br />                                                   
                     <div id="botones">
                         <asp:Button runat="server" ID="btnGuardar" Text="Guardar" />                     
                         <asp:Button runat="server" ID="btnVolver" Text="Volver" style="margin-left:25px;" />                         
                         <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" style="margin-left:25px;" />
                     </div>          
                </asp:View>
            </asp:MultiView>           
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc1:CargandoDatos ID="CargandoDatos1" runat="server"></uc1:CargandoDatos>
</asp:Content>