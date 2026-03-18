<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Plantas.aspx.vb" Inherits="IstrikuWebRaiz.Plantas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:Panel runat="server" ID="pnlFiltroCabecera">
        <center>
            <asp:DropDownList ID="ddlPlantas" runat="server" AutoPostBack="true"></asp:DropDownList> 
            <td colspan="2" style="text-align:center;">
				<asp:Panel ID="Panel2" runat="server" CssClass="PanelBotones" >
					<asp:ImageButton ID="imgAceptar" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" ValidationGroup="imgAceptar" />
				</asp:Panel>
			</td>            
        </center>
    </asp:Panel>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
