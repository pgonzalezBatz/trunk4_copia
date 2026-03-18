<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchItems.ascx.vb" Inherits="WebRaiz.SearchItems" %>

<asp:TextBox ID="txtItem" runat="server" CssClass="form-control" />
<ajax:AutoCompleteExtender ID="aceItems" ServiceMethod="CargarItems"
    runat="server" MinimumPrefixLength="3" Enabled="True" FirstRowSelected="true"
    CompletionInterval="100" EnableCaching="false" OnClientItemSelected="ElegidoItem"
    UseContextKey="true"
    TargetControlID="txtItem" ServicePath="~/WS/WSSearch.asmx" CompletionSetCount="0"
    CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
    CompletionListItemCssClass="CompletionListItemCssClass" />                    
<%--    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtUsuario" Display="None" ValidationGroup="CamposVacios" /> --%>                      
<%--    <ajax:ValidatorCalloutExtender ID="vceUsuario" runat="server" TargetControlID="rfvUsuario" PopupPosition="BottomRight" />--%>             
<asp:HiddenField ID="hfItem" runat="server" />
<asp:Button runat="server" ID="btnFire" style="display:none" />  
