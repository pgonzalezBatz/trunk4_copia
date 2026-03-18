<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ConveniosCategorias.aspx.vb" Inherits="WebRaiz.ConveniosCategorias" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">    
    <b><asp:Label runat="server" ID="labelInfo" Text="Marque que estructuras/niveles salariales podran solicitar una visa o un anticipo y el tipo de liquidacion de cada uno"></asp:Label></b>
    <asp:Label runat="server" ID="labelInfo1" CssClass="help-block" Text="Mostrar como empresa: Si se marca, a la hora de transferir las hojas de gastos de una liquidacion de tipo factura, se podran seleccionar como empresa a la que se factura"></asp:Label>
    <asp:Label runat="server" ID="labelInfo2" CssClass="help-block" Text="Recibe Visa/Anticipo: Si se marca, las personas podran recibir visas de la empresa y anticipos"></asp:Label>
    <asp:Label runat="server" ID="labelInfo3" CssClass="help-block" Text="Tipo liquidacion: Metalico=>Para indicar las personas cuya liquidacion es interna, con un pago directo desde administracion. Factura=>Para indicar aquellos cuyas hojas de gastos se tienen que enviar para recibir una factura"></asp:Label>
    <asp:GridView runat="server" ID="gvConvCat" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-striped table-hover" GridLines="None">	    
	    <EmptyDataTemplate><asp:Label runat="server" Text="No existe ningun registro"></asp:Label></EmptyDataTemplate>
	    <Columns>    
            <asp:TemplateField HeaderText="Estructura salarial">                
			    <ItemTemplate>
                    <asp:Label runat="server" ID="lblConvenio"></asp:Label>
                    <asp:HiddenField runat="server" ID="hfIdConvenio" />
                    <asp:HiddenField runat="server" ID="hfIdConvCat" />
			    </ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField HeaderText="Nivel salarial">                
			    <ItemTemplate>
                    <asp:Label runat="server" ID="lblCategoria"></asp:Label>
                    <asp:HiddenField runat="server" ID="hfIdCategoria"></asp:HiddenField>
			    </ItemTemplate>
		    </asp:TemplateField> 
            <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderText="Empresa" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" ItemStyle-Width="5%">
			    <ItemTemplate><asp:CheckBox runat="server" ID="chbMostrarEmpresa" CssClass="form-control" /></ItemTemplate>
		    </asp:TemplateField>           		   
            <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderText="Recibe Visa/Anticipo" ItemStyle-Width="5%">
			    <ItemTemplate><asp:CheckBox runat="server" ID="chbRecibe" CssClass="form-control" /></ItemTemplate>
		    </asp:TemplateField>
            <asp:TemplateField  HeaderText="Tipo liquidacion">          
			    <ItemTemplate><asp:DropDownList runat="server" ID="ddlTipoLiq" CssClass="form-control" /></ItemTemplate>
		    </asp:TemplateField>    
	    </Columns>
    </asp:GridView>
    <div class="form-group">
        <asp:Button runat="server" ID="btnSave" Text="Guardar" CssClass="form-control btn btn-primary" />
    </div>
    <script>
        $(document).ready(function () {
            $("select").change(function () {
                if (this.value == "0") { //Metalico
                    $(this).parent().parent().removeClass();
                    $(this).parent().parent().addClass("success");
                } else if (this.value == "1") { //Factura
                    $(this).parent().parent().removeClass();
                    $(this).parent().parent().addClass("warning");
                }
                else { //Ninguna
                    $(this).parent().parent().removeClass();
                }
            });           
        });
    </script>
</asp:Content>
