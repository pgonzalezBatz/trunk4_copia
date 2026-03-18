<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ListadoAMFE.aspx.vb" Inherits="DesignFMEA.ListadoAMFE" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="CargandoDatos" TagPrefix="uc" %>
<%@ Register src="~/Controles/AutocompleteGV.ascx" tagname="AutoCompleteGV" tagprefix="uc" %>
<%@ MasterType VirtualPath="~/MPWeb.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../js/utiles.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../js/jQuery/autocompleteGV.js"></script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
         <asp:MultiView ID="mvAmfes" runat="server" ActiveViewIndex="0">
             <asp:View runat="server" ID="vwListado">
                 <tit:Titulo runat="server" Texto="Causas" />
                 <fieldset style="width: 700px">
                    <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                    <table width="50%">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="labelSearch" text="Product"></asp:Label>&nbsp;
                                <asp:DropDownList runat="server" ID="ddlSearchProductos" AppendDataBoundItems="true" />
                            </td>                        
                            <td style="padding-left:20px;padding-right:20px;"><asp:Button runat="server" ID="btnBuscar" text="Buscar" CssClass="boton" /></td>                        
                            <td><asp:LinkButton runat="server" ID="lnkAdd" Text="Añadir"></asp:LinkButton></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="labelSearch2" text="Type"></asp:Label>&nbsp;
                                <asp:DropDownList runat="server" ID="ddlTipo" >
                                    <asp:ListItem Text="Select one" Value="0" />
                                    <asp:ListItem Text="Product" Value="1" />
                                    <asp:ListItem Text="Process" Value="2" />
                                    <asp:ListItem Text="Control" Value="3" />
                                    </asp:DropDownList>
                            </td>  
                        </tr>
                    </table>
                    </asp:Panel>
                </fieldset><br /><br />
                 <asp:GridView runat="server" ID="gvAmfes" AutoGenerateColumns="false" AllowSorting="False" AllowPaging="true" CssClass="GridView" Width="50%" PageSize="15" PagerSettings-Mode="NumericFirstLast">
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <RowStyle CssClass="GridViewRowStyle" />
                    <PagerStyle HorizontalAlign="Center" />
                    <PagerSettings PageButtonCount="5" />
                    <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                    <EmptyDataTemplate><asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>        
                    <Columns> 
                        <asp:BoundField DataField="Id" Visible="false" /> 
                        <asp:TemplateField HeaderText="Proyectos">
                            <ItemTemplate><asp:Label runat="server" ID="lblProyecto"></asp:Label></ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Tipo">
                            <ItemTemplate><asp:Label runat="server" ID="lblTipo"></asp:Label></ItemTemplate>
                        </asp:TemplateField>                                               
                    </Columns>
                </asp:GridView>
             </asp:View>
             <asp:View runat="server" ID="vDetalle">
                 <tit:Titulo runat="server" Texto="Detalle" />
                 <asp:Panel runat="server" ID="pnlProyNuevo">                 
                     <table>
                        <tr>
                            <td><asp:Label runat="server" ID="labelProducto" Text="Product"></asp:Label></td>
                            <td><asp:DropDownList runat="server" ID="ddlProductos" AppendDataBoundItems="true" AutoPostBack="true"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td><asp:Label runat="server" ID="labelProyecto" Text="Project"></asp:Label></td>
                            <td><asp:DropDownList runat="server" ID="ddlProyectos" AppendDataBoundItems="true"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td><asp:Label runat="server" ID="labelTipo" Text="Type"></asp:Label></td>
                            <td><asp:DropDownList runat="server" ID="ddlTipos" AppendDataBoundItems="true"></asp:DropDownList></td>
                        </tr>
                    </table><br /><br />
                     <div id="botones">
                        <asp:Button runat="server" ID="btnCrear" Text="Crear" />                     
                        <asp:Button runat="server" ID="btnVolver" Text="Volver" style="margin-left:25px;" />                        
                    </div>
                </asp:Panel>
                 <asp:Panel runat="server" ID="pnlProyExist">
                     <asp:Label runat="server" ID="labelProyecto2" Text="Project"></asp:Label>
                     <asp:Label runat="server" ID="lblProyecto" style="font-weight:bold;"></asp:Label><br /><br />
                     <div id="botones">                   
                        <asp:Button runat="server" ID="btnVolver2" Text="Back" style="margin-left:25px;" />
                        <asp:Button runat="server" ID="btnEliminar" Text="Delete" style="margin-left:25px;" />
                    </div>
                 </asp:Panel><br /><br />
                 <asp:Panel runat="server" ID="pnlRef" GroupingText="Referencias" Width="60%">                           
                    <table>
                        <tr>
                            <td><asp:Label runat="server" ID="labelReferencia" Text="Referencia"></asp:Label></td>
                            <td><uc:AutoCompleteGV ID="searchReferencia" runat="server" PostBack="false" ShowButton="false" RutaPaginaBusqueda="../Publico/Busqueda.aspx" IdName="a" ValueName="b" FactoryName="c" GridviewClass="popupTable" MinSearchLength="3" Opcion="refbrain" MaxDivHeight="250" MarginTop="1" /></td>
                            <td><asp:Label runat="server" ID="labelCopiar" Text="Copiar lecciones de la referencia"></asp:Label></td>
                            <td><asp:DropDownList runat="server" ID="ddlRef" AppendDataBoundItems="true"></asp:DropDownList></td>
                            <td><asp:ImageButton runat="server" ID="imgAddRef" ToolTip="Añadir" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo.png"/></td>
                        </tr>
                    </table><br /><br />                                 
                    <asp:GridView runat="server" ID="gvReferencias" AutoGenerateColumns="false" AllowSorting="False" AllowPaging="false" CssClass="GridView" Width="50%" PageSize="15" PagerSettings-Mode="NumericFirstLast">
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <PagerStyle HorizontalAlign="Center" />
                        <PagerSettings PageButtonCount="5" />
                        <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                        <EmptyDataTemplate><asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>        
                        <Columns>                        
                            <asp:BoundField DataField="Ref" HeaderText="Referencia" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Nº lecciones" ItemStyle-Width="1%" HeaderStyle-Wrap="false">
                                <ItemTemplate><asp:Label runat="server" ID="lblNumLecc"></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><asp:ImageButton runat="server" ID="imgDel" CommandArgument='<%#Eval("Ref")%>' CommandName="Del" ToolTip="Eliminar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Cancelar.gif" OnClick="imgDelRef_Click" /></ItemTemplate>
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView><br /><br />
                    <asp:HyperLink runat="server" ID="hlLecciones" Text="Ver lecciones de cada referencia" Target="_blank"></asp:HyperLink>
                    <asp:HyperLink runat="server" ID="hlVerExcelOK" Text="Ver excel" Target="_blank" style="margin-left:50px;"></asp:HyperLink>
                    <asp:HyperLink runat="server" ID="hlVerExcelDesc" Text="Ver excel descartados" Target="_blank" style="margin-left:50px;"></asp:HyperLink>
                 </asp:Panel>
                 <asp:HiddenField runat="server" ID="hfIdAmfe" />
             </asp:View>
         </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server"></uc:CargandoDatos>
</asp:Content>
