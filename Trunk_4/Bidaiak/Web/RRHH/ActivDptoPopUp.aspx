<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActivDptoPopUp.aspx.vb" Inherits="WebRaiz.ActivDptoPopUp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel runat="server" ID="pnlError">
            <asp:Label runat="server" ID="lblMensa" CssClass="MensajeError"></asp:Label>
        </asp:Panel>
        <asp:Label runat="server" ID="labelInfo" Text="Actividades que se han relacionado con el departamento"></asp:Label><br /><br />
        <asp:GridView runat="server" ID="gvActividades" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="GridViewB" ShowFooter="true" Width="600px">
		    <HeaderStyle CssClass="GridViewBHeaderStyle" />
		    <AlternatingRowStyle CssClass="GridViewBAlternatingRowStyle" />
		    <RowStyle CssClass="GridViewBRowStyle" />			       		    
            <EmptyDataTemplate><br /><asp:Label runat="server" Text="noExisteNingunRegistro" style="margin-left:20px;"></asp:Label></EmptyDataTemplate>
		    <Columns>	
                <asp:BoundField DataField="Nombre" HeaderText="Actividad" />                
		    </Columns>
	    </asp:GridView>
    </div>
    </form>
</body>
</html>
