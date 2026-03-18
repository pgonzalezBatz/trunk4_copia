<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Documento.aspx.vb" Inherits="GTK_Troqueleria.Documento" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		//'---------------------------------------------------------------------------------------------------
		//'Comprobamos el tamaña del Archivo
		//'---------------------------------------------------------------------------------------------------
		<%Dim section As Web.Configuration.HttpRuntimeSection = ConfigurationManager.GetSection("system.web/httpRuntime")%>
		function ValidarTamaño(sender, args) {
			var maxFileSize = "<%=section.MaxRequestLength * 1000%>";
			var fileSize = -1;
			try {
				fileSize = $get("<%=fuDocumento.ClientID%>").files[0].size; // Para navegadores que soporten HTML5
			} catch (ex) {
				var strFileName = $("#<%=fuDocumento.ClientID%>").val();
				var objFSO = new ActiveXObject("Scripting.FileSystemObject");
				var e = objFSO.getFile(strFileName);
				fileSize = e.size;
			}
			args.IsValid = (fileSize <= maxFileSize);
		}
		//'---------------------------------------------------------------------------------------------------
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<asp:UpdatePanel ID="upDocumento" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<ascx:Titulo ID="TituloNumNC" runat="server" Texto="Nº" />
			<table class="GridViewASP">
				<tr class="HeaderStyle">
					<th style="width: 1%;">
						<asp:Label ID="Label2" runat="server" Text="Documento"></asp:Label>
					</th>
					<td>
						<table style="width: 100%;">
							<tr>
								<td style="width: 1%;">
									<asp:HyperLink ID="hlDoc" runat="server" NavigateUrl="~/Controles/DocumentoBBDD.aspx" Target="_blank">
										<asp:Image ID="imgDoc" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/TipoArchivos/Document-Blank-icon24.png" BorderStyle="None" />
									</asp:HyperLink>
								</td>
								<td style="width: 1%;">
									<asp:FileUpload ID="fuDocumento" runat="server" Width="400px" />
								</td>
								<td style="text-align: left" class="ListSearchExtenderPrompt">(<asp:Label ID="Label6" runat="server" Text="Tamaño Maximo"/>: <asp:Label ID="lblMaxRequestLength" runat="server" Text="?" />)
								</td>

							</tr>
						</table>
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label1" runat="server" Text="Nombre"></asp:Label>
					</th>
					<td>
						<asp:TextBox ID="txtNombre" runat="server" Width="300px" MaxLength="250"></asp:TextBox>
						.<asp:Label ID="lblExtension" runat="server" ToolTip="Extension del Archivo / Tipo de Archivo"></asp:Label>
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label3" runat="server" Text="Titulo"></asp:Label>
					</th>
					<td>
						<asp:TextBox ID="txtTitulo" runat="server" Width="300px" MaxLength="250"></asp:TextBox>
					</td>
				</tr>
				<tr class="HeaderStyle">
					<th>
						<asp:Label ID="Label4" runat="server" Text="Descripcion"></asp:Label>
					</th>
					<td>
						<asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="4" Width="100%" MaxLength="2000"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td colspan="2" style="text-align: center">
						<asp:Panel ID="pnBotones_Documentos" runat="server" CssClass="PanelBotones">
							<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ToolTip="Aceptar" AlternateText="Aceptar" CausesValidation="true" ValidationGroup="btnAceptar" />
							<asp:ImageButton ID="btnEliminar" runat="server" AlternateText="Eliminar" ToolTip="Eliminar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" />
							<act:ConfirmButtonExtender ID="cfe_btnEliminar" runat="server" TargetControlID="btnEliminar" ConfirmText="Desea eliminar" Enabled="True" />
						</asp:Panel>
					</td>
				</tr>
			</table>
			<!------------------------------------------------------------------------------------------------------>
			<!------------------------------------------------------------------------------------------------------>
			<!-- Validacion de Campos ------------------------------------------------------------------------------>
			<!------------------------------------------------------------------------------------------------------>
			<!-- Fecha de Apertura Obligatorio --------------------------------------------------------------------->
			<%--<asp:RequiredFieldValidator ID="rfvFechaApertura" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />--%>
			<%--<act:ValidatorCalloutExtender ID="vce_rfvFechaApertura" runat="server" TargetControlID="rfvFechaApertura" />--%>
			<!------------------------------------------------------------------------------------------------------>
			<!-- Validacion personalizada -------------------------------------------------------------------------->
			<asp:CustomValidator runat="server" ID="cv_fuDocumento" ControlToValidate="fuDocumento" Display="None"
				ErrorMessage="tamañoMaximoFicheroSuperado"
				EnableClientScript="true"
				Enabled="true"
				ClientValidationFunction="ValidarTamaño"
				ValidationGroup="btnAceptar" />
			<act:ValidatorCalloutExtender ID="vce_cv_fuDocumento" runat="server" TargetControlID="cv_fuDocumento" />
			<!------------------------------------------------------------------------------------------------------>
			<!------------------------------------------------------------------------------------------------------>
		</ContentTemplate>
		<Triggers>
			<asp:PostBackTrigger ControlID="btnAceptar" />
		</Triggers>
	</asp:UpdatePanel>
</asp:Content>

