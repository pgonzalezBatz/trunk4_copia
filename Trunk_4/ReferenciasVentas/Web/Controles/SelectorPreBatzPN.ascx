<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorPreBatzPN.ascx.vb" Inherits="ReferenciasVentas.SelectorPreBatzPN" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>
<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2" style="width:100%; border-width:0px">
            <tr>
                <td style="width:90%">
                    <asp:TextBox ID="txtPrevBatzPN" runat="server" Width="99%" Columns="20" MaxLength="20" />
                    <act:AutoCompleteExtender ID="acePrevBatzPN" ServiceMethod="CargarPreviousBatzPN"
                        runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                        CompletionInterval="100" EnableCaching="false"  
                        TargetControlID="txtPrevBatzPN" UseContextKey="true"
                        ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                        CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                        CompletionListItemCssClass="CompletionListItemCssClass"  />
                        <asp:HiddenField ID="hfPrevBatzPN" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvPrevBatzPN" runat="server" ErrorMessage="Required field" ControlToValidate="txtPrevBatzPN" ValidationGroup="CamposVacios" Display="None" />                       
                        
                </td>                
            </tr>
        </table>
        <act:ValidatorCalloutExtender ID="vcePrevBatzPN" runat="server" TargetControlID="rfvPrevBatzPN" PopupPosition="BottomRight" />
    </ContentTemplate>        
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

