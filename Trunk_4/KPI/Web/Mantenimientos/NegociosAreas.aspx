<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="NegociosAreas.aspx.vb" Inherits="WebRaiz.NegociosAreas" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script src="../Js/jQuery/jquery.js" type="text/javascript"></script>    
    <script language="javascript"> 

        $(document).ready(function () {
            //Al cambiar el valor del area origen, se escribe el nombre de la misma en la caja de texto a no ser que se haya seleccionado el primer item(seleccione uno)
            $('#<%= ddlAreasOrig.ClientID%>').change(function () {                
                var selectedValue = $('#<%= ddlAreasOrig.ClientID%>' + " option:selected").val();
                var selectedText = $('#<%= ddlAreasOrig.ClientID%>' + " option:selected").text();
                if (selectedValue == "0")
                    $('#<%= txtNombreArea.ClientID%>').val('');
                else
                    $('#<%= txtNombreArea.ClientID%>').val(selectedText);
             });
        });
    </script>
    <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">       
        <asp:View runat="server" ID="vListado">
            <tit:Titulo runat="server" Texto="Negocios/Areas" />
            <fieldset style="width: 850px">
                <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="labelSearch" text="Negocio"></asp:Label>&nbsp;
                            <asp:TextBox runat="server" ID="txtSearch" Width="300px"></asp:TextBox>
                        </td>                        
                        <td style="padding-left:20px;padding-right:20px;"><asp:Button runat="server" ID="btnBuscar" text="Buscar" CssClass="boton" /></td>                        
                        <td>
                            <asp:LinkButton runat="server" ID="lnkNuevo" text="Nuevo" ToolTip="Registra un nuevo negocio"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkCopiarArea" text="Copiar area" style="margin-left:20px;" ToolTip="Copia un area junto con sus valores e indicadores"></asp:LinkButton>
                        </td>
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
                    <asp:TemplateField HeaderText="Tiene areas" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" HeaderStyle-Wrap="false">
                        <ItemTemplate><asp:CheckBox runat="server" ID="chbTieneAreas" Enabled="false" /></ItemTemplate>
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
                        <asp:TextBox runat="server" ID="txtNombre" MaxLength="100" Width="100%"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Display="None" ControlToValidate="txtNombre" ValidationGroup="Save" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                        <ajax:ValidatorCalloutExtender runat="server" ID="vceNombre" TargetControlID="rfvNombre" />
                    </td>
                </tr>                
            </table><br />                        
            <div>
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Save" CssClass="boton" />                
                <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="boton" style="margin-left:15px;" />
            </div><br /><br />
            <asp:Panel runat="server" ID="pnlAreas" GroupingText="Areas del negocio">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>                         
                        <asp:LinkButton runat="server" ID="lnkNuevaArea" text="nuevo"></asp:LinkButton><br /><br />
                        <asp:GridView runat="server" ID="gvAreas" AutoGenerateColumns="false" CssClass="Gridview"
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
                                <asp:TemplateField HeaderText="Nombre">
                                    <ItemTemplate><asp:Label runat="server" ID="lblNombre" /></ItemTemplate>
                                </asp:TemplateField>                      
                                <asp:TemplateField HeaderText="Valores" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate><asp:Label runat="server" ID="lblValores" /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Indicadores" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate><asp:Label runat="server" ID="lblIndicadores" /></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>                        

                        <asp:Label runat="server" ID="lblHiddenArea" Style="display: none"></asp:Label>
                        <ajax:ModalPopupExtender ID="mpeArea" runat="server" TargetControlID="lblHiddenArea" PopupControlID="pnlAreaInfo" CancelControlID="imgCerrarDetalle" BackgroundCssClass="modalBackground" />
                        <asp:Panel runat="server" ID="pnlAreaInfo" Style="display: none;" CssClass="modalBox">           
                            <table>
                                <tr>
                                    <td>
                                        <fieldset>
                                            <table>
                                                <tr>
                                                    <td colspan="2"><asp:Label runat="server" ID="labelTitArea" Text="Area" style="font-weight:bold;"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label runat="server" ID="labelAreaInfo" Text="Area"></asp:Label></td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAreaInfo" MaxLength="100" Width="300px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvAreaInfo" runat="server" ControlToValidate="txtAreaInfo" Display="Dynamic" ErrorMessage="*" ValidationGroup="saveArea"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>                                    
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button runat="server" ID="btnSaveArea" Text="Aceptar" ValidationGroup="saveArea"></asp:Button>
                                                        <asp:Button runat="server" ID="btnDeleteArea" Text="Eliminar" style="margin-left:10px;"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>                                
                                        </fieldset>
                                    </td>
                                    <td valign="top"><asp:ImageButton runat="server" ID="imgCerrarDetalle" ImageUrl="~/App_Themes/Tema1/images/cerrar.gif" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vCopiadoArea" runat="server">
            <tit:Titulo runat="server" ID="titCopiado" Texto="Copiar datos" />
            <asp:Label runat="server" id="labelCopiarInfo" Text="Se copiara la informacion del area, junto con todos sus valores e indicadores registrados. Sin embargo, el campo calculo del indicador, tendra que informarse a posteriori"></asp:Label><br /><br />
            <table>
                <tr>
                    <td><asp:Label runat="server" ID="labelNegOrig" Text="Negocio origen"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlNegOrig" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList></td>
                    <td><asp:Label runat="server" ID="labelAreaOrig" Text="Area origen"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlAreasOrig" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="4"><hr /></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="labelNegDest" Text="Negocio destino"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlNegDest" DataTextField="Nombre" DataValueField="Id" AppendDataBoundItems="true"></asp:DropDownList></td>
                    <td><asp:Label runat="server" ID="labelNombreArea" Text="Nombre area"></asp:Label></td>
                    <td><asp:Textbox runat="server" ID="txtNombreArea" MaxLength="100"></asp:Textbox></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="3"><br /><asp:Button runat="server" ID="btnCopiar" Text="Copiar datos" CssClass="boton" /></td>
                </tr>
            </table><br />            
        </asp:View>
    </asp:MultiView>
</asp:Content>
