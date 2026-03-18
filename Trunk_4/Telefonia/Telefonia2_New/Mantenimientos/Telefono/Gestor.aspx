<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="Gestor.aspx.vb" Inherits="Telefonia.Gestor" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
     <script src="../../js/Utiles.js" type="text/javascript"></script>
    
    
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
                <tit:Titulo ID="Titulo1" runat="server" Texto="listadoGestores" />                                            
                  <asp:GridView runat="server" ID="gvGestor" AutoGenerateColumns="false" AllowSorting="True" allowPaging="true" CssClass="GridView" width="50%" PageSize="15" PagerSettings-Mode="NumericFirstLast"> 
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />   
                        <PagerStyle HorizontalAlign="Center"/>
                        <PagerSettings PageButtonCount="5" />
                        <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                             <br />&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label>                 
                        </EmptyDataTemplate>
                    <Columns> 
                        <asp:BoundField DataField="Id" Visible="false" />
                         <asp:TemplateField SortExpression="UsuarioGestor" ItemStyle-HorizontalAlign="Center">
                               <HeaderTemplate>                                                                                                       
                                    <asp:LinkButton  runat="server" text="gestor" CommandName="Sort" CommandArgument="UsuarioGestor"></asp:LinkButton>
                               </HeaderTemplate>
                              <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("UsuarioGestor")%>'></asp:Label>                                                                                                            
                              </ItemTemplate>
                         </asp:TemplateField>                                                             
                    </Columns>
               </asp:GridView> 
               <br />
               <asp:Button runat="server" id="btnNuevo" text="nuevo" CommandName="nuevo"></asp:Button>    
            
             <asp:Label runat="server" ID="lblHiddenPopUp" Style="display: none"></asp:Label>         
            <cc1:ModalPopupExtender ID="mpeGestor" runat="server"
                   TargetControlID="lblHiddenPopUp"
                   PopupControlID="pnlPopUp" 
                   CancelControlID="imgCerrar"
                   BackgroundCssClass="modalBackground"/> 
            <asp:Panel runat="server" ID="pnlPopUp" Style="display: none;" CssClass="modalBox">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" ValidationGroup="Gestor" ShowSummary="true" />
              <table>
                 <tr>
                    <td>
                        <fieldset>
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <tit:Titulo runat="server" ID="titPopPup"></tit:Titulo>  
                                    </td>
                                </tr>
                                <asp:Panel runat="server" ID="pnlError">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="lblError" CssClass="MensajeError"></asp:Label>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelGestor" text="gestor"></asp:Label>    
                                    </td>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlExistenteG">
                                            <asp:label runat="server" ID="lblGestor" CssClass="negrita"></asp:Label>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlNuevoG">
                                            <asp:DropDownList runat="server" ID="ddlGestor" AppendDataBoundItems="true" CssClass="mayusculas"></asp:DropDownList>                                             
                                        </asp:Panel>
                                    </td>
                                </tr>                                                                  
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr> 
                                    <td colspan="2" align="center">
                                        <asp:Button runat="server" ID="btnGuardar" text="Añadir" />
                                        <asp:Button runat="server" ID="btnEliminar" text="Eliminar" />
                                        <cc1:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="btnEliminar">
                                        </cc1:ConfirmButtonExtender>                                        
                                    </td>
                                </tr>                                                                                   
                            </table> 
                          </fieldset>                   
                    </td>
                     <td valign="top">
                        <asp:ImageButton runat="server" ID="imgCerrar" ImageUrl="~/App_Themes/Tema1/images/cerrar.gif" />                     
                     </td>
                </tr>
               </table>     
            </asp:Panel>    
           
        </ContentTemplate>       
    </asp:UpdatePanel> 
</asp:Content>
