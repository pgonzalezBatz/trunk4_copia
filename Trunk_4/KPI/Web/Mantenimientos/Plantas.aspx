<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Plantas.aspx.vb" Inherits="WebRaiz.Plantas" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">       
        <asp:View runat="server" ID="vListado">
            <tit:Titulo runat="server" Texto="Plantas" />
            <fieldset style="width: 700px">
                <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                <table width="50%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="labelSearch" text="Planta"></asp:Label>&nbsp;
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
                    <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="NombreMoneda" SortExpression="NombreMoneda" HeaderText="Moneda" />
                    <asp:TemplateField headertext="Avisar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><asp:CheckBox runat="server" ID="chbAvisar" Enabled="false" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View runat="server" ID="vDetalle">
            <tit:Titulo runat="server" ID="titDetalle" Texto="Detalle" />
            <table id="detalle">                
                <asp:Panel runat="server" ID="pnlUpdate">
                    <tr>
                        <th><asp:Label runat="server" ID="labelNombre" Text="Nombre"></asp:Label></th>
                        <td>
                            <asp:TextBox runat="server" ID="txtNombre" MaxLength="75"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Display="None" ControlToValidate="txtNombre" ValidationGroup="Save" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                            <ajax:ValidatorCalloutExtender runat="server" ID="vceNombre" TargetControlID="rfvNombre" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel runat="server" id="pnlNew">
                    <tr>
                        <th><asp:Label runat="server" ID="labelNombre2" Text="Plantas"></asp:Label></th>
                        <td><asp:DropDownList runat="server" ID="ddlPlantas" AutoPostBack="true"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th><asp:Label runat="server" ID="labelNombrePlant" Text="Nombre nuevo"></asp:Label></th>
                        <td><asp:TextBox runat="server" ID="txtNombreNuevo" MaxLength="75"></asp:TextBox></td>
                    </tr>
                </asp:Panel>                    
                <tr>
                    <th><asp:Label runat="server" ID="labelMoneda" Text="Moneda"></asp:Label></th>
                    <td><asp:DropDownList runat="server" ID="ddlMonedas"></asp:DropDownList></td>
                </tr> 
                <tr>
                    <th><asp:Label runat="server" ID="labelAvisar" Text="Avisar"></asp:Label></th>
                    <td><asp:Checkbox runat="server" ID="chbAvisar" ToolTip="Si se chequea, se les avisara de que tienen indicadores pendientes por cerrar"></asp:Checkbox></td>
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
