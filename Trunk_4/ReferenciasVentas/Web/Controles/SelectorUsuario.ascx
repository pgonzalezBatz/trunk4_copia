<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorUsuario.ascx.vb" Inherits="ReferenciasVentas.SelectorUsuario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<script type="text/javascript">
    function UsuarioElegido(source, eventArgs) {
        var hfUsuarioId = document.getElementById('<%=hfUsuario.ClientID%>');
        hfUsuarioId.value = eventArgs.get_value();
    }
</script>

<asp:UpdatePanel ID="upSelector" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table2" style="width:100%; border-width:0px">
            <tr>
                <td style="width:90%">
                    <asp:TextBox ID="txtUsuario" runat="server" Width="99%" Columns="20" />
                    <act:AutoCompleteExtender ID="aceUsuario" ServiceMethod="CargarUsuarios"
                        runat="server" MinimumPrefixLength="4" Enabled="True" FirstRowSelected="true"
                        CompletionInterval="100" EnableCaching="false" OnClientItemSelected="UsuarioElegido"
                        TargetControlID="txtUsuario" UseContextKey="true"
                        ServicePath="~/WS/ConsultasWS.asmx" CompletionSetCount="0"
                        CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                        CompletionListItemCssClass="CompletionListItemCssClass" />
                        <asp:HiddenField ID="hfUsuario" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvCustPN" runat="server" ErrorMessage="Required field" ControlToValidate="txtUsuario" ValidationGroup="CamposVacios" Display="None" />                       
                        <act:ValidatorCalloutExtender ID="vceCustPN" runat="server" TargetControlID="rfvCustPN" PopupPosition="BottomRight"  />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnComprobar" runat="server" Style="visibility: hidden; position: absolute;" />

