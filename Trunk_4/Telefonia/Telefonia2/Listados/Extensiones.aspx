<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master"
    CodeBehind="Extensiones.aspx.vb" Inherits="Telefonia.Extensiones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">

    <script src="../js/Utiles.js" type="text/javascript"></script>
     <script src="../js/ajax.js" type="text/javascript"></script>
     
      <script language="javascript">
       var ModalProgress ='<%= ModalProgress.ClientID %>';    
       </script>

    <tit:Titulo runat="server" Texto="revisionExtensiones" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset style="width: 75%">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:Label runat="server" ID="labelOrdenadoPor" text="ordenadaPor" CssClass="negrita"></asp:Label><b>:</b>
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rblOrden" RepeatColumns="2" RepeatDirection="Horizontal"
                                AutoPostBack="true">
                            </asp:RadioButtonList>                            
                        </td>                       
                    </tr>
                </table>
            </fieldset>
            <br />
            <asp:GridView runat="server" ID="gvExtensiones" AutoGenerateColumns="false" AllowSorting="True"
                CssClass="GridView" Width="75%">
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField SortExpression="extension">
                        <HeaderTemplate>
                           <asp:Panel runat="server" Visible='<%#IsTipoExtension() %>'>
                                <asp:LinkButton runat="server" text="extension" CommandName="Sort" CommandArgument="extension"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#IsExtension() %>'>
                                <asp:Label runat="server" text="extension"></asp:Label>
                            </asp:Panel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("Extension")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="nombre">
                        <HeaderTemplate>
                            <asp:Panel runat="server" Visible='<%#IsTipoExtension() %>'>
                                <asp:LinkButton runat="server" text="nombre" CommandName="Sort" CommandArgument="nombre"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#IsExtension() %>'>
                                <asp:Label runat="server" text="nombre"></asp:Label>
                            </asp:Panel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="tipolinea">
                        <HeaderTemplate>
                            <asp:Panel runat="server" Visible='<%#IsTipoExtension() %>'>
                                <asp:LinkButton runat="server" text="tipo" CommandName="Sort" CommandArgument="tipolinea"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#IsExtension() %>'>
                                <asp:Label runat="server" text="tipo"></asp:Label>
                            </asp:Panel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("TipoLinea")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label runat="server" text="alveolo"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("Ruta")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress">
       <asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server" >
        <ProgressTemplate>
          <div style="position: relative; top: 30%; text-align:center;">    
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/Images/loadin.gif" />                            
            <asp:Label ID="lblFiltrando" runat="server" text="cargandoDatos"></asp:Label>   
          </div>
        </ProgressTemplate>
      </asp:UpdateProgress>
     </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground2" PopupControlID="panelUpdateProgress" />     

</asp:Content>
