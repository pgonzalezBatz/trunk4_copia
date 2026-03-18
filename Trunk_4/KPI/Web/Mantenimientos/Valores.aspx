<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Valores.aspx.vb" Inherits="WebRaiz.Valores" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">       
        <asp:View runat="server" ID="vListado">
            <tit:Titulo runat="server" Texto="Valores" />
            <fieldset style="width: 700px">
                <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                <table width="50%">
                    <tr>
                        <td style="width:1%"><asp:Label runat="server" ID="labelSearch" text="Valor"></asp:Label></td>
                        <td><asp:TextBox runat="server" ID="txtSearch" Width="300px"></asp:TextBox></td>                        
                        <td style="padding-left:20px;padding-right:20px;"><asp:Button runat="server" ID="btnBuscar" text="Buscar" CssClass="boton" /></td>                        
                        <td><asp:LinkButton runat="server" ID="lnkNuevo" text="nuevo"></asp:LinkButton></td>
                    </tr>
                    <tr>
                        <td style="width:1%"><asp:Label runat="server" ID="labelNegSearch" Text="Negocio"></asp:Label></td>
                        <td colspan="3"><asp:DropDownList runat="server" ID="ddlNegSearch" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList></td>                        
                    </tr>
                    <tr>
                        <td style="width:1%"><asp:Label runat="server" ID="labelAreaSearch" Text="Area"></asp:Label></td>
                        <td colspan="3"><asp:DropDownList runat="server" ID="ddlAreaSearch" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true"></asp:DropDownList></td>                        
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
                    <asp:TemplateField HeaderText="Negocio">
                        <ItemTemplate><asp:Label runat="server" ID="lblNegocio" /></ItemTemplate>
                    </asp:TemplateField>                     
                    <asp:TemplateField HeaderText="Area">
                        <ItemTemplate><asp:Label runat="server" ID="lblArea" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unidad">
                        <ItemTemplate><asp:Label runat="server" ID="lblUnidad" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Metodo acum.">
                        <ItemTemplate><asp:Label runat="server" ID="lblMetodoAcum" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView><br /><br />
            <asp:Button runat="server" ID="btnOrdenar" Text="Ordenar elementos" CssClass="boton" />
        </asp:View>
        <asp:View runat="server" ID="vDetalle">
            <tit:Titulo runat="server" ID="titDetalle" Texto="Detalle" />
            <table id="detalle">
                <tr>
                    <th><asp:Label runat="server" ID="labelNombre" Text="Nombre"></asp:Label></th>
                    <td>
                        <asp:TextBox runat="server" ID="txtNombre" MaxLength="75" Width="100%"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Display="None" ControlToValidate="txtNombre" ValidationGroup="Save" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                        <ajax:ValidatorCalloutExtender runat="server" ID="vceNombre" TargetControlID="rfvNombre" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2"><asp:Label runat="server" ID="labelDescr" Text="Descripcion"></asp:Label></th>
                </tr>
                <tr>
                    <td colspan="2"><asp:TextBox runat="server" ID="txtDescripcion" Width="100%" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label runat="server" ID="labelUnit" Text="Unidad"></asp:Label></th>
                    <td><asp:DropDownList runat="server" ID="ddlUnidades" DataTextField="Nombre" DataValueField="Id" /></td>
                </tr>
                <tr>
                    <th><asp:Label runat="server" ID="labelMetAcum" Text="Metodo acumulado"></asp:Label></th>
                    <td><asp:DropDownList runat="server" ID="ddlMetAcum" /></td>
                </tr>
                <tr>
                    <th><asp:Label runat="server" ID="labelNegocio" Text="Negocio"></asp:Label></th>
                    <td><asp:DropDownList runat="server" ID="ddlNegocios" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true" /></td>
                </tr>
                <tr>
                    <th><asp:Label runat="server" ID="labelArea" Text="Area"></asp:Label></th>
                    <td><asp:DropDownList runat="server" ID="ddlAreas" DataTextField="Nombre" DataValueField="Id" /></td>
                </tr>
            </table><br />                        
            <div>
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Save" CssClass="boton" />
                <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" CssClass="boton" style="margin-left:15px;" />
                <asp:Button runat="server" ID="btnActivar" Text="Activar" CssClass="boton" style="margin-left:15px;" />
                <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="boton" style="margin-left:15px;" />
            </div><br /><br />
            <asp:Panel runat="server" ID="pnlOtrasAreas">
                <asp:Label runat="server" ID="labelOtrasAreas" Text="Areas a mostrar en modo lectura" style="font-weight:bold;"></asp:Label><br /><br />
                <table>
                    <tr>
                        <td><asp:Label runat="server" ID="labelNeg" Text="Negocio"></asp:Label></td>
                        <td><asp:DropDownList runat="server" ID="ddlNeg" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="labelAreas" Text="Areas"></asp:Label></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlOtrasAreas" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true"></asp:DropDownList>
                            <asp:ImageButton runat="server" ID="imgAddArea" ToolTip="Añadir" Imageurl="~/App_Themes/Tema1/IconosAcciones/Nuevo.png" ImageAlign="Middle" />
                        </td>
                    </tr>
                </table><br />
                <asp:GridView runat="server" ID="gvOtrasAreas" AutoGenerateColumns="false" CssClass="Gridview"
                    Width="50%" AllowSorting="false" AllowPaging="true" PageSize="15" PagerSettings-Mode="NumericFirstLast">
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
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:TemplateField HeaderText="Negocio">
                            <ItemTemplate><asp:Label runat="server" ID="lblNegocio" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><asp:ImageButton runat="server" ID="imgDelArea" ToolTip="Eliminar" Imageurl="~/App_Themes/Tema1/IconosAcciones/Cancelar.gif" OnClick="imgDelArea_Click" /></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
