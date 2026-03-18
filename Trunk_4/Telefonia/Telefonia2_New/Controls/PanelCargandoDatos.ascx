<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PanelCargandoDatos.ascx.vb" Inherits="Telefonia.PanelCargandoDatos" %>
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
			<div style="text-align: center;" class="form-inline">
				<br /><div class="form-group" id="divImg"><asp:Image runat="server" ImageUrl="~/App_Themes/Tema1/Images/loading.gif" AlternateText="cargandoDatos" ToolTip="cargandoDatos" meta:resourcekey="Image1Resource1" /></div>
                <div class="form-group"><asp:Label ID="lblCargandoDatos" runat="server" Text="cargandoDatos" meta:resourcekey="lblCargandoDatosResource1" CssClass="control-label"></asp:Label></div>								
			</div>
		</ProgressTemplate>
	</asp:UpdateProgress>
</asp:Panel>
<ajax:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground" PopupControlID="panelUpdateProgress" />