<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="cambiarUsuario.aspx.vb" Inherits="GTK_Troqueleria.cambiarUsuario" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
	<table style="margin:0 auto;width:400px">
		<tr>
			<td style="text-align:center;">
				<asp:Panel ID="pnlCambioUsuario" runat="server" CssClass="recuadro" GroupingText="Plantas de Sistemas de Automocion" Width="97%" >
                    <table>
                        <tr>
                            <td><asp:Label Text="Código trabajador" runat="server"></asp:Label></td>
                            <td><asp:TextBox runat="server" ID="tbLogin" onkeypress="return isNumberKey(event)" MaxLength="8" style="width:150px;"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:Label Text="Password" runat="server"></asp:Label></td>
                            <td><asp:TextBox runat="server" ID="tbPass" TextMode="Password" style="width:150px;"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:Label Text="Planta" runat="server"></asp:Label></td>
                            <td><asp:DropDownList runat="server" ID="ddlPlantas" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value"  style="width:150px;">
                                <asp:ListItem Value="" Text="(Seleccione uno)"></asp:ListItem>
                            </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center"><asp:Button runat="server" ID="cambioUsuario" Text="Cambiar de usuario"/></td>
                        </tr>
                    </table>
				</asp:Panel>
			</td>
		</tr>
	</table>

        <script type="text/javascript">
            function isNumberKey(evt)
              {
                 var charCode = (evt.which) ? evt.which : evt.keyCode;
                 if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;    
                 return true;
              }
        </script>

    </asp:Content>