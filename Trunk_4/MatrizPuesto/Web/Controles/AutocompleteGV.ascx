<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AutocompleteGV.ascx.vb" Inherits="AdokWeb.AutocompleteGV" %>    
        
    <div class="imagen-no-seleccionado" id="divImgGV" runat="server" style="margin-right:10px;"></div>
	<asp:TextBox runat="server" ID="txtInput" Columns="35" ToolTip="Seleccione uno" CssClass="campoObligatorio" AutoComplete="Off" style="margin-left:5px;"></asp:TextBox>           
    <asp:HiddenField runat="server" ID="hfValue" />
    <asp:ImageButton runat="server" ID="imgBuscar" ToolTip="Buscar"  ImageAlign="Bottom" ImageUrl="~/App_Themes/Tema1/IconosBotones/Buscar.png" style="margin-left:15px;" /><br />
    <div id="helper" runat="server" style="margin-top:-1px;"></div>    
    <asp:Button runat="server" ID="btnFire" style="visibility:hidden;" />  