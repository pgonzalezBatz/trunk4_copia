<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Unidades.aspx.vb" Inherits="WebRaiz.Unidades" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">       
        <asp:View runat="server" ID="vListado">
            <tit:Titulo runat="server" Texto="Unidades" />
            <fieldset style="width: 700px">
                <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                <table width="50%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="labelSearch" text="Unidad"></asp:Label>&nbsp;
                            <asp:TextBox runat="server" ID="txtSearch" Width="300px"></asp:TextBox>
                        </td>                        
                        <td style="padding-left:20px;padding-right:20px;"><asp:Button runat="server" ID="btnBuscar" text="Buscar" CssClass="boton" /></td>                        
                        <td><asp:LinkButton runat="server" ID="lnkNuevo" text="nuevo"></asp:LinkButton></td>
                    </tr>
                </table>
                </asp:Panel>
            </fieldset><br />
            <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="Gridview"
                Width="50%" AllowSorting="true" AllowPaging="true" PageSize="15" PagerSettings-Mode="NumericFirstLast">
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <PagerStyle HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />
                <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField Visible="false" DataField="Id" />                    
                    <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                        <ItemTemplate><asp:Label runat="server" ID="lblNombre" /></ItemTemplate>
                    </asp:TemplateField>                      
                    <asp:BoundField DataField="TextoMostrar" HeaderText="Texto a mostrar" SortExpression="TextoMostrar" />
                    <asp:TemplateField HeaderText="Es moneda" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="10%">
                        <ItemTemplate><asp:CheckBox runat="server" ID="chbEsMoneda" Enabled="false" /></ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View runat="server" ID="vDetalle">
            <tit:Titulo runat="server" ID="titDetalle" Texto="Detalle" />
            <table id="detalle">
                <tr>
                    <th><asp:Label runat="server" ID="labelNombre" Text="Nombre"></asp:Label></th>
                    <td>
                        <asp:TextBox runat="server" ID="txtNombre" MaxLength="75"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Display="None" ControlToValidate="txtNombre" ValidationGroup="Save" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                        <ajax:ValidatorCalloutExtender runat="server" ID="vceNombre" TargetControlID="rfvNombre" />
                    </td>
                </tr>
                <tr>
                    <th><asp:Label runat="server" ID="labelTextoM" Text="Texto a mostrar"></asp:Label></th>
                    <td><asp:TextBox runat="server" ID="txtTextoMostrar" MaxLength="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label runat="server" ID="labelEsMoneda" Text="Es moneda"></asp:Label></th>
                    <td><asp:CheckBox runat="server" ID="chbEsMoneda"></asp:CheckBox></td>
                </tr>
            </table><br />                        
            <div>
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Save" CssClass="boton" />
                <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" CssClass="boton" style="margin-left:15px;" />
                <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="boton" style="margin-left:15px;" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
