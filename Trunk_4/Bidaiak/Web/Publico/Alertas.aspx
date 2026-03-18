<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="Alertas.aspx.vb" Inherits="WebRaiz.Alertas" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <div class="form-group">
        <asp:Label runat="server" ID="labelExistAcc" text="Existen acciones por realizar"></asp:Label>
    </div>        
        <asp:Repeater runat="server" ID="rptAlertas">
			<ItemTemplate>
                <div class="alert alert-warning">									
					<asp:LinkButton ID="lnkAccion" runat="server" OnClick="LinkAlerta" />
                    <asp:Label ID="lblInfo" runat="server" CssClass="badge" style="margin-left:10px;"></asp:Label>
				</div>
			</ItemTemplate>            
        </asp:Repeater>            
</asp:Content>
