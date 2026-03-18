<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Index.aspx.vb" Inherits="GTK_Troqueleria.Index" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<table style="margin-right:auto; margin-left:auto;">
		<tr>
			<td style="text-align:center;">
				<asp:Panel ID="pnlPlantas" runat="server" CssClass="recuadro" GroupingText="Plantas de Sistemas de Automocion" Width="97%" >
					<asp:DropDownList runat="server" ID="ddlPlantas" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value">
						<asp:ListItem Value="" Text="(Seleccione uno)"></asp:ListItem>
					</asp:DropDownList>
				</asp:Panel>
			</td>
		</tr>
	</table>

	<%--<asp:HiddenField ID="hf_IdRecepcion" runat="server" />
	<asp:HiddenField ID="hf_Planta" runat="server" />--%>
</asp:Content>