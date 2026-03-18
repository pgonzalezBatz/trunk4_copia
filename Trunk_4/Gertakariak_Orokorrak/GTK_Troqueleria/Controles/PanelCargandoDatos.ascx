<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PanelCargandoDatos.ascx.vb" Inherits="GTK_Troqueleria.PanelCargandoDatos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<!-- El contenido de la pagina debe estar dentro de un UpdatePanel. -->
<!-- No se puede usar con Server.Transfer -->
<script type="text/javascript">
    var mpe_pnUpdateProgress = '<%= mpe_pnUpdateProgress.ClientID%>';
	Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
	function beginRequest(sender, args) { $find(mpe_pnUpdateProgress).show();}
	function endRequest(sender, args) { $find(mpe_pnUpdateProgress).hide(); }
     
    /*--------------------------------------------------------*/
	//Mostramos el panel para el "SiteMapPath" de la master.
	//Incluir en "ToolkitScriptManager" la referencia a jQuery.
    /*--------------------------------------------------------*/
	$('#SiteMapPath a').click(function () {
		$('#<%= pnUpdateProgress.ClientID%> div').css("display", "inline");
	    beginRequest();
    });
    /*--------------------------------------------------------*/
</script>
<asp:Panel ID="pnUpdateProgress" runat="server" >
	<asp:UpdateProgress ID="upCargadoDatos" DisplayAfter="0" runat="server">
		<ProgressTemplate>
			<asp:Panel ID="Panel1" runat="server" CssClass="updateProgress" >
				<table>
					<tr>
						<td>
							<asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Batz/Imagenes/Cargando.gif" AlternateText="cargandoDatos" ToolTip="cargandoDatos"  />
							<br />
							<asp:Label ID="lblCargandoDatos" runat="server" Text="cargandoDatos" ></asp:Label>
						</td>
					</tr>
				</table>
			</asp:Panel>
		</ProgressTemplate>
	</asp:UpdateProgress>
</asp:Panel>
<act:ModalPopupExtender ID="mpe_pnUpdateProgress" runat="server" TargetControlID="pnUpdateProgress" BackgroundCssClass="modalBackground" PopupControlID="pnUpdateProgress" />