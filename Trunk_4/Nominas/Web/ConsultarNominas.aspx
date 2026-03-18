<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ConsultarNominas.aspx.vb" Inherits="Nominas.ConsultarNominas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:Label runat="server" ID="labelMesAno" Text="Seleccione el mes y el año de la nomina a consultar"></asp:Label><br /><br />
    <div class="form-inline">
        <div class="form-group" >
            <asp:Label runat="server" ID="labelMes" text="Mes"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlMes" CssClass="form-control" style="margin-right:25px;"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="labelAno" text="Año"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlAño" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-primary col-xs-12" />
        </div>
    </div><br /><br class="visible-xs" /><br class="visible-xs" />     
    <asp:Panel runat="server" ID="pnlResultados" CssClass="panel panel-default">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelTitleNominas" Text="Nominas" style="text-transform:uppercase"></asp:Label></strong>
        </div>
		 <div class="panel-body">
			<asp:Repeater runat="server" ID="rptNominas">
				<ItemTemplate>
					<div class="row">  
                        <div class="col-sm-3 col-xs-8"><asp:Label runat="server" ID="lblGrupo"></asp:Label></div>
                        <div class="col-sm-1 hidden-xs"><asp:Imagebutton runat="server" ID="imgVer" ImageUrl="~/App_Themes/Tema1/Images/pdf.gif" ToolTip="Ver nomina" OnClick="VerNomina" /></div>
                        <%--<div class="col-sm-1"><asp:Imagebutton runat="server" ID="imgPrint" ImageUrl="~/App_Themes/Tema1/Images/print.png" ToolTip="Imprimir" OnClick="ImprimirNomina" style="visibility:hidden" /></div>--%>
                        <div class="col-xs-4 visible-xs"><asp:LinkButton runat="server" ID="lnkVer" CssClass="btn btn-primary" OnClick="VerNominaMobile"><span class="glyphicon glyphicon-download"></span></asp:LinkButton></div>                        
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>									
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSinResultados" CssClass="alert alert-warning">
		<asp:Label runat="server" Text="No se ha encontrado ninguna nomina" style="font-weight:bold;"></asp:Label><br />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlOharrakIgorre" CssClass="panel panel-default">
         <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelTitleOharrak" Text="Oharrak" style="text-transform:uppercase"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <a href="https://intranet2.batz.es/batzscoop/documentos/JAKINARAZPENAK%20-%20Notificaciones/2017%2012%2021%20NOMINAK%20INTRANETEN%20BIDEZ.pdf">OHARRA: Nomina Intraneten bidez bakarrik</a>
        </div>		
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlOtrosDocs" CssClass="panel panel-default">
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelTitleDoc" Text="Documentos" style="text-transform:uppercase"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <asp:Repeater runat="server" ID="rptDocumentos">
				<ItemTemplate>
                    <div class="row">
                        <div class="col-sm-6"><asp:LinkButton runat="server" ID="lnkDoc"></asp:LinkButton></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>        
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfHost" />
    <asp:Label runat="server" ID="lblHidden" Style="display: none"></asp:Label>
    <ajax:ModalPopupExtender ID="mpePopUp" runat="server" PopupControlID="pnlPassword" TargetControlID="lblHidden" CancelControlID="imgCerrar" BackgroundCssClass="modalBackground" />                   
    <asp:Panel runat="server" ID="pnlPassword" Style="display: none;" CssClass="modalBox" DefaultButton="imgImprimir">
        <table>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlDNI">
                        <asp:Label runat="server" ID="lblError" CssClass="MensajeError"></asp:Label><br />
                        <asp:Label runat="server" ID="labelDNI" Text="Introduzca el DNI con letra"></asp:Label>&nbsp;
                        <asp:TextBox runat="server" ID="txtDNI" Columns="10" TextMode="Password" style="text-align:center;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDNI" runat="server" Display="None" ControlToValidate="txtDNI" ValidationGroup="print" ErrorMessage="Introduzca el dato" EnableClientScript="true"></asp:RequiredFieldValidator>
                        <ajax:ValidatorCalloutExtender runat="server" ID="vceDNI" TargetControlID="rfvDNI" /><br />                   
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlImpreso">
                        <asp:Label runat="server" ID="lblImpreso" CssClass="MensajeInfoAdvertencia"></asp:Label>
                    </asp:Panel>
                </td>
                <td valign="top"><asp:ImageButton runat="server" ID="imgCerrar" ImageUrl="~/App_Themes/Tema1/images/cerrar.gif" /></td>
            </tr>   
            <tr>
                <td colspan="2"> 
                    <div style="margin-left:80px;"><br /><asp:Imagebutton runat="server" ID="imgImprimir" ToolTip="Imprimir" ValidationGroup="print" ImageUrl="~/App_Themes/Tema1/Images/Guardar.png" /></div>
                </td>
            </tr>                    
        </table>
    </asp:Panel>
</asp:Content>