<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Facturacion.aspx.vb" Inherits="GTK_Troqueleria.Facturacion" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
/***********************************/
//Cambio de color de las Filas
/***********************************/
var TrClase;
function Sartu(TR){
 TrClase = TR.className;
 TR.className = 'trOver';
}
function Irten(TR){
 TR.className = TrClase;
}

/***********************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    	<asp:UpdatePanel runat="server">
		<ContentTemplate>
			<fieldset>
				<legend>
					<asp:Label ID="Label4" runat="server" Text="Seleccione uno" />:</legend>
				<asp:RadioButtonList ID="tipoFacturacion" runat="server" AutoPostBack="true" EnableViewState="true">
				</asp:RadioButtonList>
			</fieldset>
			<fieldset runat="server" id="FecEntraga" visible="false">
				<legend>
					<asp:Label ID="Label5" runat="server" Text="FechaEntrega" /></legend>
				<asp:Label ID="Label6" runat="server" Text="FechaEntrega" />:
				<asp:TextBox ID="txtFechaEntrega" Width="85" runat="server"></asp:TextBox>
				<asp:ImageButton ID="imgbtn1" ImageUrl="~/App_Themes/Tema1/imagen/Calendar2.gif" runat="server" />
				<act:CalendarExtender ID="calExt1" TargetControlID="txtFechaEntrega" runat="server" FirstDayOfWeek="Monday" PopupButtonID="imgbtn1" />
				<act:CalendarExtender ID="calExt2" TargetControlID="txtFechaEntrega" runat="server" FirstDayOfWeek="Monday" PopupButtonID="txtFechaEntrega" />
			</fieldset>
			<fieldset runat="server" id="Albaranes" visible="false">
				<legend>
					<asp:Label ID="Label7" runat="server" Text="albaranes" />:</legend>
				<asp:GridView runat="server" ID="gvAlbaranes" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" AllowSorting="True" CssClass="GridView" PagerSettings-Mode="NumericFirstLast" DataKeyNames="NUMALBAR,TIPO,CODPROV,ANNO">
					<HeaderStyle CssClass="GridViewHeaderStyle" />
					<AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
					<RowStyle CssClass="GridViewRowStyle" />
					<%--<PagerStyle CssClass="GridViewPagerStyle" HorizontalAlign="Center" />--%>
					<PagerStyle CssClass="PagerStyle" />
					<PagerSettings PageButtonCount="5"></PagerSettings>
					<SelectedRowStyle CssClass="trConsultada" />
					<Columns>
						<asp:TemplateField SortExpression="NUMALBAR" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false">
							<HeaderTemplate>
								<asp:LinkButton ID="LinkButton1" runat="server" Text="albaran" CommandName="Sort" CommandArgument="NUMALBAR" />
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Label ID="Label1" runat="server" Text='<%#Eval("NUMALBAR")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField SortExpression="TIPO" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false">
							<HeaderTemplate>
								<asp:LinkButton ID="LinkButton2" runat="server" Text="tipo" CommandName="Sort" CommandArgument="TIPO" />
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Label ID="Label11" runat="server" Text='<%#Eval("TIPO")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField SortExpression="CODPROV" HeaderStyle-Wrap="false">
							<HeaderTemplate>
								<asp:LinkButton ID="LinkButton3" runat="server" Text="Proveedor" CommandName="Sort" CommandArgument="CODPROV" />
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Label ID="Label2" runat="server" Text='<%#Eval("CODPROV")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField SortExpression="ANNO" HeaderStyle-Wrap="false">
							<HeaderTemplate>
								<asp:LinkButton ID="LinkButton4" runat="server" Text="año" CommandName="Sort" CommandArgument="ANNO" />
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Label ID="Label3" runat="server" Text='<%#Eval("ANNO")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>
				<br />
				<table style="border: solid 1px #000000;">
					<tr>
						<td class="recuadroAzul" style="width: 10px;">
							&nbsp;
						</td>
						<td>
							<asp:Label ID="Label8" runat="server" Text="seleccionada" />
						</td>
					</tr>
				</table>
			</fieldset>
			<asp:Panel runat="server" ID="pnlCilindros" GroupingText="Cilindros a devolver" Visible="false" Width="50%">
				<asp:GridView ID="gvCilindros" runat="server" AutoGenerateColumns="False" RowHeaderColumn="ID" DataKeyNames="ID, NUMPED, NUMLIN" CssClass="GridViewASP" EmptyDataText="Sin Datos" Caption="Listado de Cilindros" GridLines="None" PagerSettings-Position="Bottom" EnableModelValidation="True">
					<RowStyle CssClass="RowStyle" />
					<FooterStyle CssClass="FooterStyle" />
					<PagerStyle CssClass="PagerStyle" />
					<SelectedRowStyle CssClass="SelectedRowStyle" />
					<HeaderStyle CssClass="HeaderStyle" />
					<EditRowStyle CssClass="EditRowStyle" />
					<AlternatingRowStyle CssClass="AlternatingRowStyle" />
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:CheckBox ID="chkCilindro" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" />
						<asp:BoundField DataField="NUMALBAR" HeaderText="albaran" />
						<asp:BoundField DataField="NUMPED" HeaderText="Nº Pedido" />
						<asp:BoundField DataField="NUMLIN" HeaderText="nºLinea" />
						<asp:BoundField DataField="SERIE" HeaderText="Serie" />
						<asp:BoundField DataField="CODART" HeaderText="articulo" />
						<asp:BoundField DataField="CODPRO" HeaderText="Código Proveedor" />
					</Columns>
				</asp:GridView>
			</asp:Panel>
			<asp:Panel runat="server" ID="pnlBulones" GroupingText="Bulones a devolver" Visible="false" Width="50%">
				<asp:GridView ID="gvBulones" runat="server" AutoGenerateColumns="False" RowHeaderColumn="ID" DataKeyNames="ID, NUMPED, NUMLIN" CssClass="GridViewASP" EmptyDataText="Sin Datos" Caption="Listado de Bulones" GridLines="None" PagerSettings-Position="Bottom" EnableModelValidation="True">
					<RowStyle CssClass="RowStyle" />
					<FooterStyle CssClass="FooterStyle" />
					<PagerStyle CssClass="PagerStyle" />
					<SelectedRowStyle CssClass="SelectedRowStyle" />
					<HeaderStyle CssClass="HeaderStyle" />
					<EditRowStyle CssClass="EditRowStyle" />
					<AlternatingRowStyle CssClass="AlternatingRowStyle" />
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:CheckBox ID="chkBulon" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" />
						<asp:BoundField DataField="NUMALBAR" HeaderText="albaran" />
						<asp:BoundField DataField="NUMPED" HeaderText="Nº Pedido" />
						<asp:BoundField DataField="NUMLIN" HeaderText="nºLinea" />
						<asp:BoundField DataField="SERIE" HeaderText="Serie" />
						<asp:BoundField DataField="CODART" HeaderText="articulo" />
						<asp:BoundField DataField="CODPRO" HeaderText="Código Proveedor" />
					</Columns>
				</asp:GridView>
			</asp:Panel>
			<br />
			<center>
				<%--<asp:Button ID="btnTratamiento" runat="server" Text="tramitar" ValidationGroup="otros" Enabled="false" />--%>
				<asp:Button ID="btnTramitacion" runat="server" Text="tramitar" ValidationGroup="otros" Enabled="false" />
			</center>
			<%--<br />
			<center>
				<asp:Button ID="btnVolver" runat="server" Text="volver" ValidationGroup="otros" /></center>
			<br />--%>
		</ContentTemplate>
	</asp:UpdatePanel>

</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
