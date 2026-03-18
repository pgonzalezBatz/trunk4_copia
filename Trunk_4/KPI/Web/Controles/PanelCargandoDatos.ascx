<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PanelCargandoDatos.ascx.vb" Inherits="WebRaiz.PanelCargandoDatos" %>

<!-- El contenido de la pagina debe estar dentro de un UpdatePanel. -->
<script type="text/javascript" language="javascript">
	var ModalProgress = '<%= ModalProgress.ClientID %>';
	Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
	function beginRequest(sender, args) { $find(ModalProgress).show(); }
	function endRequest(sender, args) { $find(ModalProgress).hide(); }
</script>

<asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress">
	<asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server">
		<ProgressTemplate>
			<div style="position: relative; top: 30%; text-align: center;">
				<asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/Images/Loadin.gif" AlternateText="cargandoDatos" ToolTip="cargandoDatos" meta:resourcekey="Image1Resource1" />
				<br />
				<asp:Label ID="lblCargandoDatos" runat="server" Text="cargandoDatos" meta:resourcekey="lblCargandoDatosResource1"></asp:Label>
			</div>
		</ProgressTemplate>
	</asp:UpdateProgress>
</asp:Panel>
<ajax:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground" PopupControlID="panelUpdateProgress" />