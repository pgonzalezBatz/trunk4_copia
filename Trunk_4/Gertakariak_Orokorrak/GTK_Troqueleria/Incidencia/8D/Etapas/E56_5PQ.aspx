<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="E56_5PQ.aspx.vb" Inherits="GTK_Troqueleria.E56_5PQ" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº"/>
	<table class="GridViewASP" style="width: 1%; margin-left: auto; margin-right: auto;">
		<tr class="HeaderStyle">
			<th>
				<asp:Label ID="Label2" runat="server" Text="Orden"></asp:Label>
			</th>
			<th>
				<asp:Label ID="Label1" runat="server" Text="¿Por qué?"></asp:Label>
			</th>
		</tr>
		<tr class="RowStyle">
			<td style="vertical-align:top;">
				<asp:TextBox ID="txtOrden" runat="server"></asp:TextBox>
				<act:NumericUpDownExtender ID="nude_txtOrden" runat="server" Minimum="1" Step="1" TargetControlID="txtOrden" Width="90" />
			</td>
			<td>
				<asp:TextBox ID="txtPregunta" runat="server" Columns="100" Rows="10" TextMode="MultiLine"></asp:TextBox>
				<act:AutoCompleteExtender ID="ace_txtPregunta" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_DescAcc" TargetControlID="txtPregunta" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
					OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
				<asp:RegularExpressionValidator ID="rev_txtPregunta" runat="server" ControlToValidate="txtPregunta" ErrorMessage="Solo 4000 Caracteres" ValidationExpression="^[\s\S]{0,4000}$" Display="None" SetFocusOnError="true" ValidationGroup="imgAceptar" />
				<act:ValidatorCalloutExtender ID="vce_rev_txtPregunta" TargetControlID="rev_txtPregunta" runat="server" PopupPosition="TopLeft" />
			</td>
		</tr>
		<%--<tr class="HeaderStyle">
			<th>
				<asp:Label ID="Label2" runat="server" Text="Respuesta"></asp:Label>
			</th>
		</tr>
		<tr class="RowStyle">
			<td>
				<asp:TextBox ID="txtRespuesta" runat="server" Columns="100" Rows="10" TextMode="MultiLine" ToolTip="Razones del rechazo"></asp:TextBox>
				<act:AutoCompleteExtender ID="ace_txtRespuesta" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_EficAcc" TargetControlID="txtRespuesta" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
					OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
				<asp:RegularExpressionValidator ID="rev_txtRespuesta" runat="server" ControlToValidate="txtRespuesta" ErrorMessage="Solo 4000 Caracteres" ValidationExpression="^[\s\S]{0,4000}$" Display="None" SetFocusOnError="true" ValidationGroup="imgAceptar" />
				<act:ValidatorCalloutExtender ID="vce_rev_txtRespuesta" TargetControlID="rev_txtRespuesta" runat="server" PopupPosition="TopLeft" />
			</td>
		</tr>--%>
		<tr>
			<td style="text-align: center" colspan="2">
				<asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
					<asp:ImageButton ID="imgAceptar" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptar" />
					<asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" />
					<act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" Enabled="True" />
				</asp:Panel>
			</td>
		</tr>
	</table>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
