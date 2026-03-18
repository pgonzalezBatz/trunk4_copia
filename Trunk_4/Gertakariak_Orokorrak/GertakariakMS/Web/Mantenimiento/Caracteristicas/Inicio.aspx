<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Inicio.aspx.vb" Inherits="GertakariakMSWeb_Raiz.Inicio1" %>

<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register Src="../../Controles/Titulo.ascx" TagName="Titulo" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
	<uc1:Titulo ID="Titulo1" runat="server" Texto="Caracteristicas" />
	<asp:TreeView ID="tvEstructura" runat="server" ShowLines="True" BorderColor="Black" BorderWidth="1" BorderStyle="Solid" Width="100%" ExpandDepth="1">
		<NodeStyle ForeColor="Black" />
		<%--<LevelStyles>
			<asp:TreeNodeStyle Font-Underline="false" />
		</LevelStyles>--%>
		<RootNodeStyle ForeColor="WindowText" BackColor="ActiveCaption" Width="100%" />
		<ParentNodeStyle ForeColor="WindowText" BackColor="#C9D7E7" Width="100%" />
		<%--<LeafNodeStyle BackColor="red" />--%>
		<SelectedNodeStyle Font-Bold="true" BorderColor="#8585E2" BorderStyle="Double" BorderWidth="2" />
		<HoverNodeStyle BorderColor="ActiveBorder" ForeColor="#F4F4F4" Font-Bold="true" BorderStyle="Outset" BorderWidth="3" BackColor="#345374" />
	</asp:TreeView>
	</asp:Content>