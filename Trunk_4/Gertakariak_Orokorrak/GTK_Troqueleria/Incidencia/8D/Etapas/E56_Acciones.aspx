<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="E56_Acciones.aspx.vb" Inherits="GTK_Troqueleria.E56_Acciones" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº" />
	<table class="GridViewASP" style="width: 1%">
		<caption>
			<asp:Label ID="Label11" runat="server" Text="fechas" />
		</caption>
		<tr class="HeaderStyle">
			<th style="white-space: nowrap; width: 50%">
				<asp:Label ID="Label1" runat="server" Text="FechaApertura" />
			</th>
			<th style="white-space: nowrap" class="GridViewASP">
				<asp:Label ID="Label4" runat="server" Text="Fecha Prevista" />
			</th>
			<%--<th style="white-space: nowrap" class="GridViewASP">
				<asp:Label ID="Label2" runat="server" Text="Fecha de Cierre" />
			</th>--%>
		</tr>
		<tr class="RowStyle">
			<td style="white-space: nowrap">
				<asp:TextBox ID="txtFechaApertura" runat="server" Width="85px" AutoPostBack="false"></asp:TextBox>
				<act:CalendarExtender ID="txtFechaApertura_CalendarExtender" runat="server" TargetControlID="txtFechaApertura" />
				<act:CalendarExtender ID="imgCalendario_CalendarExtender" runat="server" TargetControlID="txtFechaApertura" PopupButtonID="imgCalendario" />
				&nbsp;<asp:ImageButton ID="imgCalendario" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" />
			</td>
			<td style="white-space: nowrap">
				<asp:TextBox ID="txtFechaPrevista" runat="server" Width="85px" AutoPostBack="false"></asp:TextBox>
				<act:CalendarExtender ID="ce_txtFechaPrevista" runat="server" TargetControlID="txtFechaPrevista" />
				<act:CalendarExtender ID="ce_imgCalendario_FP" runat="server" TargetControlID="txtFechaPrevista" PopupButtonID="imgCalendario_FP" />
				&nbsp;<asp:ImageButton ID="imgCalendario_FP" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" />
			</td>
			<%--<td style="white-space: nowrap">
				<asp:TextBox ID="txtFechaCierre" runat="server" Width="70px" AutoPostBack="false"></asp:TextBox>
				<act:CalendarExtender ID="txtFechaCierre_CalendarExtender" runat="server" TargetControlID="txtFechaCierre" />
				<act:CalendarExtender ID="imgCalendario2_CalendarExtender" runat="server" TargetControlID="txtFechaCierre" PopupButtonID="imgCalendario2" />
				&nbsp;<asp:ImageButton ID="imgCalendario2" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" />
			</td>--%>
		</tr>
	</table>
	<table style="width: 100%" class="GridViewASP">
		<tr class="HeaderStyle">
			<th>
				<asp:Label ID="Label10" runat="server" Text="Descripcion"></asp:Label>
			</th>
		</tr>
		<tr class="RowStyle">
			<td style="width: 48%">
				<asp:TextBox ID="txtDesc" runat="server" Rows="6" TextMode="MultiLine" Width="98%"></asp:TextBox>
				<act:AutoCompleteExtender ID="ace_txtDesc" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_DescAcc" TargetControlID="txtDesc" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
					OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
				<asp:RegularExpressionValidator ID="rev_txtDesc" runat="server" ControlToValidate="txtDesc" ErrorMessage="Solo 4000 Caracteres" ValidationExpression="^[\s\S]{0,4000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnGuardar" />
				<act:ValidatorCalloutExtender ID="vce_rev_txtDesc" TargetControlID="rev_txtDesc" runat="server" PopupPosition="TopLeft" />
			</td>
		</tr>
		<tr class="HeaderStyle">
			<th>
				<asp:Label ID="Label3" runat="server" Text="Responsable (Area/Departamento)"></asp:Label>
			</th>
		</tr>
		<tr class="RowStyle">
			<td style="width: 48%">
				<asp:TextBox ID="txtEficacia" runat="server" Width="98%" MaxLength="4000"></asp:TextBox>
				<act:AutoCompleteExtender ID="ace_txtEficacia" runat="server" Enabled="True" ServicePath="~/Controles/ServiciosWeb.asmx" ServiceMethod="get_EficAcc" TargetControlID="txtEficacia" UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="200" EnableCaching="true" CompletionSetCount="0" CompletionListCssClass="CompletionListCssClass" CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" FirstRowSelected="true"
					OnClientPopulating="Rellenando" OnClientPopulated="Rellenado" OnClientHiding="Rellenado" />
				<%--<asp:RegularExpressionValidator ID="rev_txtEficacia" runat="server" ControlToValidate="txtEficacia" ErrorMessage="Solo 4000 Caracteres" ValidationExpression="^[\s\S]{0,4000}$" Display="None" SetFocusOnError="true" ValidationGroup="btnGuardar" />
				<act:ValidatorCalloutExtender ID="vce_rev_txtEficacia" TargetControlID="rev_txtEficacia" runat="server" PopupPosition="TopLeft" />--%>
			</td>
		</tr>
	</table>
	<fieldset style="text-align: center;">
		<asp:Panel ID="Panel4" runat="server" CssClass="PanelBotones">
			<asp:ImageButton ID="btnGuardar" runat="server" AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" CausesValidation="true" ValidationGroup="btnGuardar" />
			<asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" />
			<act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" Enabled="True" />
		</asp:Panel>
	</fieldset>
	<!------------------------------------------------------------------------------------------------------>
	<!-- Validacion de Campos ------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha de Apertura Obligatorio --------------------------------------------------------------------->
	<asp:RequiredFieldValidator ID="rfvFechaApertura" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rfvFechaApertura" runat="server" TargetControlID="rfvFechaApertura" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha de Apertura solo fecha ---------------------------------------------------------------------->
	<asp:CompareValidator ID="cvFechaApertura" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaApertura" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaApertura" runat="server" TargetControlID="cvFechaApertura" ViewStateMode="Enabled" EnableViewState="true" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha de Cierre solo fecha ------------------------------------------------------------------------>
	<%--<asp:CompareValidator ID="cvFechaCierre" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaCierre" runat="server" TargetControlID="cvFechaCierre" />--%>
	<!------------------------------------------------------------------------------------------------------>
	<!-- FechaInicio <= FechaCierre ------------------------------------------------------------------------>
<%--	<asp:CompareValidator ID="cvFechaCierre2" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Operator="GreaterThanEqual" ControlToCompare="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaCierre2" runat="server" TargetControlID="cvFechaCierre2" />--%>
	<!------------------------------------------------------------------------------------------------------>

	<!-- Fecha Prevista Obligatorio ----------------------------------------------------------------------->
	<asp:RequiredFieldValidator ID="rfvtxtFechaPrevista" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtFechaPrevista" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rfvtxtFechaPrevista" runat="server" TargetControlID="rfvtxtFechaPrevista" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- Fecha Prevista solo fecha ------------------------------------------------------------------------->
	<asp:CompareValidator ID="cv_txtFechaPrevista" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaPrevista" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cv_txtFechaPrevista" runat="server" TargetControlID="cv_txtFechaPrevista" ViewStateMode="Enabled" EnableViewState="true" />
	<!------------------------------------------------------------------------------------------------------>
	<!-- FechaInicio <= Fecha Prevista --------------------------------------------------------------------->
	<asp:CompareValidator ID="cvFechaPrevista" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaPrevista" Operator="GreaterThanEqual" ControlToCompare="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_cvFechaPrevista" runat="server" TargetControlID="cvFechaPrevista" />
	<!------------------------------------------------------------------------------------------------------>

	<!-- Campos Obligatorios ------------------------------------------------------------------------------->
	<asp:RequiredFieldValidator ID="rfv_txtDesc" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtDesc" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rfv_txtDesc" runat="server" TargetControlID="rfv_txtDesc" PopupPosition="TopLeft"/>
	<asp:RequiredFieldValidator ID="rfv_txtEficacia" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtEficacia" Display="None" ValidationGroup="btnGuardar" />
	<act:ValidatorCalloutExtender ID="vce_rfv_txtEficacia" runat="server" TargetControlID="rfv_txtEficacia" PopupPosition="TopLeft"/>
	<!------------------------------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>