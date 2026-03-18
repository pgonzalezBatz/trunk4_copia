<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleConceptos.aspx.vb" Inherits="WebRaiz.DetalleConceptos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="min-height:75px;max-height:250px; overflow: auto;">
        <asp:Panel runat="server" ID="pnlError">
            <asp:Label runat="server" ID="lblError" CssClass="MensajeError"></asp:Label>
        </asp:Panel>
        <asp:Label runat="server" ID="labelInfo" Text="Se muestran los datos asociados al concepto"></asp:Label><br /><br />
        <asp:GridView runat="server" ID="gvDatosConc" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="GridViewB" Width="90%">
		    <HeaderStyle CssClass="GridViewBHeaderStyle" />
		    <AlternatingRowStyle CssClass="GridViewBAlternatingRowStyle" />
		    <RowStyle CssClass="GridViewBRowStyle" />			
		    <FooterStyle CssClass="GridViewBFooterStyle" />
		    <EmptyDataRowStyle CssClass="GridViewBEmptyRowStyle" HorizontalAlign="Center" />
		    <EmptyDataTemplate>
			    <asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label>
		    </EmptyDataTemplate>
		    <Columns>
                <asp:BoundField DataField="Establecimiento" HeaderText="Establecimiento" />
                <asp:BoundField DataField="Localidad" HeaderText="Localidad" />
                <asp:BoundField DataField="NombreUsuario" HeaderText="Persona" />
		    </Columns>
	    </asp:GridView>
    </div>
    </form>
</body>
</html>
