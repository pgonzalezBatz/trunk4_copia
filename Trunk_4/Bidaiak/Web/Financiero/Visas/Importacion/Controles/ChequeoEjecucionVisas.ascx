<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChequeoEjecucionVisas.ascx.vb" Inherits="WebRaiz.ChequeoEjecucionVisas" %>
<asp:Panel runat="server" ID="pnlDistintoUsuario">
        <table style="border:solid 1px #000000;margin-top:20px;margin-left:20px;" width="700px;">
		<tr>
			<td colspan="2" align="center" style="background-color:#C3C3C3;"><asp:Label runat="server" ID="labelTit1" CssClass="titulo" Text="Ejecucion interrumpida"></asp:Label></td>
		</tr>
		<tr>
			<td><asp:Image runat="server" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/error_big.gif" /></td>
            <td style="padding-left:25px;"><asp:Label runat="server" id="lblMensa1" CssClass="labelDetalle"></asp:Label></td>
        </tr>                  
	</table>    
</asp:Panel>
<asp:Panel runat="server" ID="pnlMismoUsuario">
        <table style="border:solid 1px #000000;margin-top:20px;margin-left:20px;" width="700px;">
		<tr>
			<td colspan="2" align="center" style="background-color:#C3C3C3;"><asp:Label runat="server" ID="labelTit2" CssClass="titulo" Text="Ejecucion ya existente"></asp:Label></td>
		</tr>
		<tr>
			<td><asp:Image runat="server" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/Question_big.png" /></td>
            <td style="padding-left:25px;">
                <asp:Label runat="server" id="lblMensa2" CssClass="labelDetalle"></asp:Label><br /><br />
                <div id="botones">
                    <asp:Button runat="server" ID="btnSi" text="si" Width="50" />
                    <asp:Button runat="server" ID="btnNo" text="no" Width="50" style="margin-left:25px;" />
                    <asp:Button runat="server" ID="btnCancelar" text="Cancelar" style="margin-left:25px;" />
                </div>
            </td>
        </tr> 
        </table>     
</asp:Panel>