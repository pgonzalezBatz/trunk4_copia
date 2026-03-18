<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="GrupoExtension.aspx.vb" Inherits="Telefonia.GrupoExtension" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script src="../js/Utiles.js" type="text/javascript"></script>     
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>                
                  <asp:GridView runat="server" ID="gvGrupos" AutoGenerateColumns="false" AllowSorting="True" allowPaging="true" CssClass="GridView" width="50%" PageSize="15" PagerSettings-Mode="NumericFirstLast"> 
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />   
                        <PagerStyle HorizontalAlign="Center"/>
                        <PagerSettings PageButtonCount="5" />
                        <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                             <br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label  runat="server" text="noExisteNingunRegistro"></asp:Label>
                        </EmptyDataTemplate>
                    <Columns> 
                        <asp:BoundField DataField="Id" Visible="false" />
                         <asp:TemplateField SortExpression="Nombre" ItemStyle-HorizontalAlign="Center">
                               <HeaderTemplate><asp:LinkButton runat="server" text="grupo" CommandName="Sort" CommandArgument="Nombre"></asp:LinkButton></HeaderTemplate>
                              <ItemTemplate><asp:Label ID="Label1" runat="server" Text='<%#Eval("Nombre")%>'></asp:Label></ItemTemplate>
                         </asp:TemplateField>  
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                               <HeaderTemplate><asp:Label runat="server" text="libre"></asp:Label></HeaderTemplate>
                              <ItemTemplate><asp:Checkbox runat="server" Checked='<%#Eval("Libre")%>' Enabled="false"></asp:Checkbox></ItemTemplate>
                         </asp:TemplateField>                                                                 
                    </Columns>
               </asp:GridView><br />
               <asp:Button runat="server" id="btnNuevo" text="nuevo" CommandName="nuevo"></asp:Button>                
             <asp:Label runat="server" ID="lblHiddenPopUp" Style="display: none"></asp:Label>         
            <cc1:ModalPopupExtender ID="mpeGrupoExt" runat="server" TargetControlID="lblHiddenPopUp" PopupControlID="pnlPopUp" CancelControlID="imgCerrar" BackgroundCssClass="modalBackground"/> 
            <asp:Panel runat="server" ID="pnlPopUp" Style="display: none;" CssClass="modalBox">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" ValidationGroup="Grupo" ShowSummary="true" />
              <table>
                 <tr>
                    <td>
                        <fieldset>
                            <table>
                                <tr>
                                    <td colspan="2"><asp:Label runat="server" ID="lblTitPopPup"></asp:Label></td>
                                </tr>
                                <asp:Panel runat="server" ID="pnlError">
                                    <tr>
                                        <td colspan="2"><asp:Label runat="server" ID="lblError" CssClass="MensajeError"></asp:Label></td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td><asp:Label runat="server" ID="labelGrupo" text="grupo"></asp:Label></td>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlExistente"><asp:label runat="server" ID="lblGrupo"></asp:label></asp:Panel>
                                        <asp:Panel runat="server" ID="pnlNuevo">
                                            <asp:TextBox runat="server" ID="txtGrupo" MaxLength="2" Width="30px"></asp:TextBox> 
                                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtGrupo" ID="rfvGrupo" ValidationGroup="Grupo" Display="Dynamic" text="introduzcaGrupoExtension" />
                                             <asp:RegularExpressionValidator runat="server" ControlToValidate="txtGrupo" ID="revGrupo" ValidationGroup="Grupo" Display="Dynamic" text="introduzcaGrupoExtension" ValidationExpression="[0-9]{2}" />
                                        </asp:Panel>
                                    </td>
                                </tr>                               
                                <tr>
                                    <td><asp:Label runat="server" ID="labelLibre" text="libre"></asp:Label>    </td>
                                    <td><asp:CheckBox runat="server" ID="chbLibre" /></td>
                                </tr>                                                               
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr> 
                                    <td colspan="2" align="center">
                                        <asp:Button runat="server" ID="btnGuardar" text="Guardar" />&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btnEliminar" text="Eliminar" />
                                        <cc1:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="btnEliminar" />                                        
                                    </td>
                                </tr>                                                                                   
                            </table> 
                          </fieldset>                   
                    </td>
                     <td valign="top"><asp:ImageButton runat="server" ID="imgCerrar" ImageUrl="~/App_Themes/Tema1/images/cerrar.gif" /></td>
                </tr>
               </table>     
            </asp:Panel>               
        </ContentTemplate>       
    </asp:UpdatePanel> 
</asp:Content>