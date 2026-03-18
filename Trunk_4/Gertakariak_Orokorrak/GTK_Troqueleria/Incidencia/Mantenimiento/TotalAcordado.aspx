<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TotalAcordado.aspx.vb" Inherits="GTK_Troqueleria.TotalAcordado" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <table class="GridViewASP" style="width: 600px;">
        <caption>
            <asp:Label ID="Label4" runat="server" Text="Total Acordado"></asp:Label>
        </caption>
        <tr class="HeaderStyle">
            <th style="white-space: nowrap; text-align: right; width: 1%;">
                <asp:Label ID="Label1" runat="server" Text="Coste Real"></asp:Label>
            </th>
            <td>
                <asp:Label ID="lblCosteReal" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="white-space: nowrap; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Total Acordado"></asp:Label>
            </th>
            <td>
                <asp:TextBox ID="txtTotalAcordado" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="white-space: nowrap; text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="Compensado"></asp:Label>
            </th>
            <td>
                <asp:CheckBox ID="chkCompensado" runat="server" />
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="white-space: nowrap; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="observaciones"></asp:Label>
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtObservacionesCoste" TextMode="MultiLine" Rows="5" Width="99%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FooterStyle">
                <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                    <asp:ImageButton runat="server" ID="btnGuarbar" ToolTip="Guardar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar16.png" ImageAlign="AbsMiddle" ValidationGroup="btnGuardar" />
                </asp:Panel>
            </td>
        </tr>
    </table>

    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <!------------------------------------------------------------------------------------------------------>
    <!-- Campo Decimal ------------------------------------------------------------------------------------->
    <asp:RegularExpressionValidator ID="rev_txtTotalAcordado" runat="server" ErrorMessage="advDebeSerNumerico" ControlToValidate="txtTotalAcordado" ValidationExpression="^[+-]?(?:\d+\.?\d*|\d+\,?\d*|\d*\,?\d+|\d*\.?\d+)[\r\n]*$" Display="None" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_rev_txtTotalAcordado" TargetControlID="rev_txtTotalAcordado" runat="server" />
    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion MaxLength ------------------------------------------------------------------------------>
    <asp:RegularExpressionValidator ID="rev_txtObservacionesCoste" runat="server" ControlToValidate="txtObservacionesCoste" ErrorMessage="Solo 2000 Caracteres" ValidationExpression="^[\s\S]{0,2000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnGuardar" />
    <act:ValidatorCalloutExtender ID="vce_rev_txtObservacionesCoste" TargetControlID="rev_txtObservacionesCoste" runat="server" />
    <!-- ------------------------------------------------------------------------------------------------ -->
    <!------------------------------------------------------------------------------------------------------>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
