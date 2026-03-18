<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Accion.aspx.vb" Inherits="GertakariakMSWeb_Raiz.Accion" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/SeleccionUsuarios.ascx" TagName="SeleccionUsuarios" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHEAD" runat="server">
	<script type="text/javascript">
		function Validar_FechaFin()
		{
			var txtFechaFin = document.getElementById("<%=txtFechaFin.ClientID%>");
			var txtRealizacion = document.getElementById("<%=txtRealizacion.ClientID%>");
			if (jsTrim(txtFechaFin.value) != "") { txtRealizacion.value = "100"; }
			else if (jsTrim(txtFechaFin.value) == "" && jsTrim(txtRealizacion.value) == "100") { txtRealizacion.value = "0"; }
		}

		function Validar_Realizacion()
		{
			var txtFechaFin = document.getElementById("<%=txtFechaFin.ClientID%>");
			var txtRealizacion = document.getElementById("<%=txtRealizacion.ClientID%>");
			var FechaActual = "<%=Now.ToShortDateString%>";
			if (jsTrim(txtRealizacion.value) == "100" && jsTrim(txtFechaFin.value) == "")
			{ txtFechaFin.value = FechaActual; }
			else if (jsTrim(txtRealizacion.value) != "100") { txtFechaFin.value = ""; }
		}

		//************************************************************************
		/*
		Función que quita los espacios en blanco de delante y detrás de la cadena y si contiene espacios en blanco duplicados los junta en un solo espacio.
		Por ejemplo " mi texto de prueba " pasa a ser "mi texto de prueba".
		En la funcion vemos que el segundo parmetro del mtodo replace es "$2".
		Esto se entiende cogiendo la expresión regular /^(\s*)([\W\w]*)(\b\s*$)/ y agrupando los elementos en parénntesis de la siguiente manera:
		$1->(\s*)		--> Todas las agrupaciones(*) de espacios en blanco(\s)
		$2->([\W\w]*)	--> Todas las agrupaciones(*) alfanumericas(\w) y no alfanumericas(\W)
		$3->(\b\s*$)	--> Marca el inicio y el final de una palabra(\b) para todas las agrupaciones(*) 
							de espacios en blanco(\s) al final de la linea($).
		*/
		//************************************************************************
		function jsTrim(JSvalue) {
			var JStemp = JSvalue;
			var JSobj = /^(\s*)([\W\w]*)(\b\s*$)/;
			if (JStemp != undefined) {
				//Elimina los espacios de delante y detrás
				if (JSobj.test(JStemp)) { JStemp = JStemp.replace(JSobj, "$2"); }
				//Elimina los espacios duplicados
				var JSobj = / +/g;
				JStemp = JStemp.replace(JSobj, " ");
				if (JStemp == " ") { JStemp = ""; }
			} else {
				JStemp = "";
			}
			return JStemp;
		}

	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<asp:UpdatePanel ID="UPC" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<uc1:Titulo ID="TituloPagina" runat="server" Texto="datosAccion" />
			<table id="Table1" runat="server" class="GridViewASP">
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label13" runat="server" Text="Tipo de Acción"></asp:Label>
					</th>
					<th>
						<asp:Label ID="Label14" runat="server" Text="FechaInicio"></asp:Label>
					</th>
					<th>
						<asp:Label ID="Label15" runat="server" Text="Fecha Prevista de Cierre"></asp:Label>
					</th>
					<th>
						<asp:Label ID="Label16" runat="server" Text="FechaFin"></asp:Label>
					</th>
					<th>
						<asp:Label ID="Label9" runat="server" Text="Demora" />
						(<asp:Label ID="Label10" runat="server" Text="semanas" />)
					</th>
					<th>
						<asp:Label ID="Label12" runat="server" Text="Desarrollo"></asp:Label>(%)
					</th>
				</tr>
				<tr class="RowStyle" style="text-align: center">
					<td>
						<asp:DropDownList ID="ddlTipoAccion" runat="server" AppendDataBoundItems="true">
							<asp:ListItem></asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						<asp:TextBox ID="txtFechaApertura" runat="server" Width="85px"></asp:TextBox>
						<act:CalendarExtender ID="txtFechaApertura_CalendarExtender" runat="server" TargetControlID="txtFechaApertura" />
						<act:CalendarExtender ID="imgCalendario_CalendarExtender" runat="server" TargetControlID="txtFechaApertura" PopupButtonID="imgCalendario" />
						&nbsp;<asp:ImageButton ID="imgCalendario" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
					</td>
					<td>
						<asp:TextBox ID="txtFechaRevision" runat="server" Width="85px"></asp:TextBox>
						<act:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaRevision" />
						<act:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaRevision" PopupButtonID="imgCalendarioRevision" />
						&nbsp;<asp:ImageButton ID="imgCalendarioRevision" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
					</td>
					<td>
						<asp:TextBox ID="txtFechaFin" runat="server" Width="85px" onkeyup="Validar_FechaFin();"></asp:TextBox>
						<act:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFechaFin" />
						<act:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFechaFin" PopupButtonID="imgCalendarioFin" />
						&nbsp;<asp:ImageButton ID="imgCalendarioFin" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
					</td>
					<td>
						<asp:Label ID="lblDemora" runat="server" />
					</td>
					<td>
						<asp:TextBox ID="txtRealizacion" runat="server" Columns="3" MaxLength="3" onkeyup="Validar_Realizacion();"></asp:TextBox>
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label26" runat="server" Text="descripcion"></asp:Label>
					</th>
					<td colspan="5" class="RowStyle">
						<asp:TextBox ID="txtDescripcion" runat="server" Rows="5" Width="99%" TextMode="MultiLine" />
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th rowspan="2">
						<asp:Label ID="Label22" runat="server" Text="eficacia"></asp:Label>
					</th>
					<td class="RowStyle" colspan="4" rowspan="2">
						<asp:TextBox ID="txtEficacia" runat="server" Rows="5" Width="99%" TextMode="MultiLine" />
					</td>
					<th style="white-space: nowrap">
						<asp:Label ID="Label25" runat="server" Text="responsables"></asp:Label>
					</th>
				</tr>
				<tr class="HeaderStyle">
					<td class="RowStyle">
						<asp:BulletedList ID="blResponsables" runat="server" />
						<fieldset style="text-align: center;">
							<asp:ImageButton ID="btnBuscarResponsables" runat="server" AlternateText="Nuevo" ToolTip="Nuevo" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Nuevo24.png" />
						</fieldset>
					</td>
				</tr>
			</table>
			<fieldset style="text-align: center;">
				<asp:ImageButton ID="btnGuardar" runat="server" AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" CausesValidation="true" ValidationGroup="btnGuardar" />
				&nbsp;<asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Eliminar24.png" />
			</fieldset>
			<br />
			<fieldset style="text-align: center">
				<asp:Button ID="btnVolver" runat="server" Text="Volver" ToolTip="Volver" />
			</fieldset>
			<PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
			<asp:Panel ID="pnlSelectorUsuario" runat="server" Style="display: none;" CssClass="modalBox">
				<table border="0px">
					<tr>
						<td>
							<fieldset style="text-align: right;">
								<asp:ImageButton ID="btnCerrarPanel" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Cancelar24.png" />
							</fieldset>
						</td>
					</tr>
					<tr>
						<td>
							<uc1:SeleccionUsuarios ID="SeleccionUsuarios" runat="server" Vigentes="true" Trabajador="UsuariosBatz" />
						</td>
					</tr>
					<tr align="center">
						<td>
							<fieldset style="text-align: center;">
								<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAcciones/Aceptar24.png" />
							</fieldset>
						</td>
					</tr>
				</table>
			</asp:Panel>
			<act:ModalPopupExtender ID="mpe_SelectorUsuarios" runat="server" CancelControlID="btnCerrarPanel" PopupControlID="pnlSelectorUsuario" TargetControlID="btnBuscarResponsables" BackgroundCssClass="modalBackground" />
			<act:ConfirmButtonExtender ID="cf_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" />
			<!------------------------------------------------------------------------------------------------------>
			<!-- Validacion de Campos ------------------------------------------------------------------------------>
			<!------------------------------------------------------------------------------------------------------>
			<!-- Fecha de Apertura Obligatorio --------------------------------------------------------------------->
			<asp:RequiredFieldValidator ID="rfvFechaApertura" runat="server" ErrorMessage="requerido" ControlToValidate="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_rfvFechaApertura" runat="server" TargetControlID="rfvFechaApertura" />
			<!------------------------------------------------------------------------------------------------------>
			<!-- Fecha de Apertura solo fecha ---------------------------------------------------------------------->
			<asp:CompareValidator ID="cvFechaApertura" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaApertura" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_cvFechaApertura" runat="server" TargetControlID="cvFechaApertura" />
			<!------------------------------------------------------------------------------------------------------>
			<!-- Fecha de Cierre solo fecha ------------------------------------------------------------------------>
			<asp:CompareValidator ID="cvFechaCierre" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_cvFechaCierre" runat="server" TargetControlID="cvFechaCierre" />
			<!------------------------------------------------------------------------------------------------------>
			<!-- FechaCierre >= FechaInicio ------------------------------------------------------------------------>
			<asp:CompareValidator ID="cvFechaCierre2" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Operator="GreaterThanEqual" ControlToCompare="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_cvFechaCierre2" runat="server" TargetControlID="cvFechaCierre2" />
			<!------------------------------------------------------------------------------------------------------>
			<!-- Fecha Prevista de Cierre solo fecha ---------------------------------------------------------------------->
			<asp:CompareValidator ID="cvFechaRevision" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaRevision" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_cvFechaRevision" runat="server" TargetControlID="cvFechaRevision" />
			<!------------------------------------------------------------------------------------------------------>
			<!-- FechaRevision >= FechaInicio ---------------------------------------------------------------------->
			<asp:CompareValidator ID="cvFechaRevision2" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaRevision" Operator="GreaterThanEqual" ControlToCompare="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" CultureInvariantValues="true" />
			<act:ValidatorCalloutExtender ID="vce_cvFechaRevision2" runat="server" TargetControlID="cvFechaRevision2" />
			<!------------------------------------------------------------------------------------------------------>
			<!-- FechaCierre >= FechaRevision ---------------------------------------------------------------------->
<%--			<asp:CompareValidator ID="cvFechaRevision3" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaFin" Operator="GreaterThanEqual" ControlToCompare="txtFechaRevision" Display="None" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_cvFechaRevision3" runat="server" TargetControlID="cvFechaRevision3" />--%>
			<!------------------------------------------------------------------------------------------------------>
			<!-- Eficacia Obligatoria ------------------------------------------------------------------------------>
			<asp:RequiredFieldValidator ID="rfvEficacia" runat="server" ErrorMessage="requerido" ControlToValidate="txtEficacia" Display="None" EnableClientScript="false" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_rfvEficacia" runat="server" TargetControlID="rfvEficacia" Enabled="false" />
			<!------------------------------------------------------------------------------------------------------>

			<!-- Realizacion entre 0 y 100 ------------------------------------------------------------------------->
			<asp:RangeValidator ID="rvRealizacion" runat="server" ControlToValidate="txtRealizacion" ErrorMessage="Fuera de rango" MinimumValue="0" MaximumValue="100" Type="Integer" Display="None" ValidationGroup="btnGuardar" />
			<act:ValidatorCalloutExtender ID="vce_rvRealizacion" runat="server" TargetControlID="rvRealizacion" />
			<!------------------------------------------------------------------------------------------------------>

			<asp:CustomValidator ID="cv_FechaFin" runat="server" ControlToValidate="txtFechaFin"  ClientValidationFunction="Validar_FechaFin" Display="None" EnableClientScript="true" ></asp:CustomValidator>
			<asp:CustomValidator ID="cv_Realizacion" runat="server" ControlToValidate="txtRealizacion"  ClientValidationFunction="Validar_Realizacion" Display="None" EnableClientScript="true" ></asp:CustomValidator>

			<!------------------------------------------------------------------------------------------------------>
		</ContentTemplate>
	</asp:UpdatePanel>
	
	<!------------------------------------------------------------------------------------------------------>
	<!-- Validacion de Campos ------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
	<!------------------------------------------------------------------------------------------------------>
</asp:Content>
