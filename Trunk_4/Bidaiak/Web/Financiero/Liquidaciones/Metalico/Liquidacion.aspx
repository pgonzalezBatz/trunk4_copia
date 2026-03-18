<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Liquidacion.aspx.vb" Inherits="WebRaiz.Liquidacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">    
    <script src="../../../js/jquery/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script  src="../../../js/jQuery/jquery.toastmessage.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery.signalR-2.3.0.js" type="text/javascript"></script>    
    <script src="../../../signalr/hubs" type="text/javascript"></script>   
    <script type="text/javascript">
        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null && cb.disabled == false)
                cb.checked = checkState;
        }
        function ChangeAllCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array and updates their value to the checkState input parameter
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }

        function ChangeHeaderAsNeeded() {
            // Whenever a checkbox in the GridView is toggled, we need to check the Header checkbox if ALL of the GridView checkboxes are checked, and uncheck it otherwise
            if (CheckBoxIDs != null) {
                // check to see if all other checkboxes are checked
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (!cb.checked) {
                        // Whoops, there is an unchecked checkbox, make sure that the header checkbox is unchecked
                        ChangeCheckBoxState(CheckBoxIDs[0], false);
                        return;
                    }
                }
                // If we reach here, ALL GridView checkboxes are checked
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }

        function filtrar() {
            document.getElementById('<%=btnFiltrar.ClientID%>').style.display = 'none';                
            document.getElementById('<%=labelProcesando.ClientID%>').style.display = 'inline';
            document.body.style.cursor="progress";
            var btn = document.createElement("progress");
            btn.setAttribute("id", "myProgress");
            btn.setAttribute("value","0");
            btn.setAttribute("max", "100");
            $('#divProgress').append(btn);        
        }

        $(function () {            
            var myhub = $.connection.signalRHub;                
            myhub.client.showMessage = function (message) {
                // Html encode display step and message.                
                var message = $('<div />').text(message).html();                        
                $('#spanMessage').text(message);
            };
             myhub.client.showMessage_Integracion = function (message) {
                // Html encode display step and message.                
                var message = $('<div />').text(message).html();                        
                $('#spanMessageInt').text(message);
                };  
            myhub.client.showProgress = function (numGest,numTotal) {
                    var prog = document.getElementById('myProgress');
                prog.setAttribute("value", numGest);                    
                prog.setAttribute("max", numTotal);   
            }; 
            $.connection.hub.start().done(function () {
                $('#' + '<%=btnFiltrar.ClientID%>').click(function () {                
                    $('#' + '<%=hfConnectionId.ClientID%>').val(myhub.connection.id);    
                    $('#' + '<%=btnFiltrarHidden.ClientID%>').click();                
                });                    
                $('#' + '<%=btnIntegrar.ClientID%>').click(function () {                
                    $('#' + '<%=hfConnectionId.ClientID%>').val(myhub.connection.id);    
                    $('#' + '<%=btnIntegrarHidden.ClientID%>').click();                
                });
            });       
        });
    </script>
     <asp:UpdatePanel runat="server">
        <ContentTemplate>
        <asp:Timer runat="server" ID="temporizador"></asp:Timer>		
        </ContentTemplate>
    </asp:UpdatePanel> 
    <tit:Titulo runat="server" Texto="Liquidacion" /><br />        
    <asp:Panel runat="server" ID="pnlInfo">
        <asp:Label runat="server" ID="lblTextoLiquidacion" CssClass="labelDetalle"></asp:Label><br /><br />
        <asp:LinkButton runat="server" ID="lnkTipoLiq"></asp:LinkButton><br /><br />        
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlFiltro">
        <asp:Label runat="server" ID="labelFiltrar" Text="Filtrar hojas de gastos validadas hasta el dia"></asp:Label>&nbsp;
        <asp:TextBox ID="txtFechaFiltro" runat="server" Columns="10"></asp:TextBox>
		<asp:ImageButton ID="imgCalFFiltro" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
		<ajax:CalendarExtender ID="ceFF" runat="server" TargetControlID="txtFechaFiltro" PopupButtonID="imgCalFFiltro" />
        <asp:Checkbox runat="server" ID="chbHGEntregada" Text="Hoja de gastos entregada" style="margin-left:15px;" />
        <asp:Button runat="server" ID="btnFiltrar" Text="Filtrar" style="margin-left:15px;" OnClientClick="filtrar()" /><br /><br />
        <asp:Label runat="server" ID="labelProcesando" Text="Procesando. Espere..." CssClass="labelDetalle" style="display:none;"></asp:Label><br />
        <span id="spanMessage" class="labelDetalle"></span>
        <div id="divProgress"></div>
        <asp:Button runat="server" ID="btnFiltrarHidden" style="visibility:hidden" />        
    </asp:Panel>   
    <asp:Panel runat="server" ID="pnlInfoIntegracion">
        <asp:Label runat="server" ID="labelCompruebe" text="Comprueba las hojas a integrar antes de finalizar" CssClass="labelDetalle"></asp:Label><br /><br />
        <asp:Label runat="server" ID="labelFEmision" text="Fecha emision"></asp:Label>&nbsp;
		<asp:TextBox ID="txtFechaEmision" runat="server" contentEditable="false" Columns="10"></asp:TextBox>
		<asp:ImageButton ID="imgCalendarioFE" runat="server" ImageUrl="~/App_Themes/Tema1/IconosAjaxControl/CalendarExtender/Calendario.gif" />
		<ajax:CalendarExtender ID="ceFE" runat="server" TargetControlID="txtFechaEmision" PopupButtonID="imgCalendarioFE" /><br /><br />
        <span id="spanMessageInt" class="labelDetalle"></span><br /><br />        
    </asp:Panel>
    <asp:GridView runat="server" ID="gvLiquidaciones" AutoGenerateColumns="false" AllowPaging="false" CssClass="GridViewB" Width="60%" PageSize="20" PagerSettings-Mode="NumericFirstLast" ShowFooter="true">
	    <HeaderStyle CssClass="GridViewBHeaderStyle" />
	    <AlternatingRowStyle CssClass="GridViewBAlternatingRowStyle" />
	    <RowStyle CssClass="GridViewBRowStyle" />
	    <PagerStyle HorizontalAlign="Center" CssClass="GridViewBPagerStyle" />
	    <PagerSettings PageButtonCount="5" />
	    <FooterStyle CssClass="GridViewBFooterStyle" />
	    <EmptyDataRowStyle CssClass="GridViewBEmptyRowStyle" HorizontalAlign="Center" />
	    <EmptyDataTemplate>
		    <asp:Label runat="server" Text="No existe ninguna hoja de gastos a liquidar"></asp:Label>
	    </EmptyDataTemplate>
	    <Columns>   
            <asp:TemplateField Visible="false">        
                <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">        
                <ItemTemplate><asp:Label runat="server" ID="lblImportes"></asp:Label></ItemTemplate>
            </asp:TemplateField>
           <asp:TemplateField  ItemStyle-HorizontalAlign="Center">               
                <HeaderTemplate><asp:CheckBox runat="server" ID="chbSelectAll" ToolTip="Seleccionar todos" /></HeaderTemplate>
			    <ItemTemplate><asp:CheckBox runat="server" ID="chbMarcar" /></ItemTemplate>
                <FooterTemplate></FooterTemplate>
		   </asp:TemplateField>                    
           <asp:TemplateField SortExpression="Persona">
                <HeaderTemplate><asp:LinkButton runat="server" Text="persona" CommandName="Sort" CommandArgument="Persona"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                 <FooterTemplate><asp:Label runat="server" ID="labelTotal" text="Total"></asp:Label></FooterTemplate>    
		    </asp:TemplateField>                       
            <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="right" SortExpression="Liquidacion">
                <HeaderTemplate><asp:LinkButton runat="server" Text="Liquidacion" CommandName="Sort" CommandArgument="Liquidacion"></asp:LinkButton></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblLiquidacion" style="font-size:13px;font-weight:bold;"></asp:Label></ItemTemplate> 
                <FooterTemplate><asp:Label runat="server" ID="lblTotal" style="font-size:15px;font-weight:bold;"></asp:Label></FooterTemplate>               
		    </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" text="Viajes/Hojas"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Hyperlink runat="server" ID="hlViajeHoja" Target="_blank" style="padding-left:15px;"></asp:Hyperlink></ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField Visible="false">        
                <ItemTemplate><asp:Label runat="server" ID="lblIdUser"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate><asp:Label runat="server" text="Fecha validacion"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblFVal"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate><asp:Label runat="server" text="Cuenta contable"></asp:Label></HeaderTemplate>
			    <ItemTemplate><asp:Label runat="server" ID="lblCuenta"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Organizacion"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblOrganizacion"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="10%">
                <HeaderTemplate><asp:Label runat="server" Text="HG Entregada"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Image runat="server" ID="imgHGEntreg" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/Aceptada.png"/></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                <HeaderTemplate><asp:Label runat="server" Text="Excluir"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:ImageButton runat="server" ID="imgExcluir" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/Denegada.png" OnClick="imgExcluir_Click" /></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                <HeaderTemplate><asp:Label runat="server" Text="Lantegi"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblLantegi"></asp:Label></ItemTemplate>
            </asp:TemplateField>
	    </Columns>
    </asp:GridView><br />
    <asp:Panel runat="server" ID="pnlMarca">
        <asp:Label runat="server" Text="(*)" CssClass="font10"></asp:Label>&nbsp;
        <asp:Label runat="server" ID="labelInfo" Text="La informacion de la liquidacion de los subcontratados no se generara en el fichero que se envia al banco. Se realiza el pago por otros medios" CssClass="font10"></asp:Label>            
    </asp:Panel><br />   
    <asp:Literal runat="server" ID="CheckBoxIDsArray"></asp:Literal>
    <asp:Panel runat="server" cssClass="botones" ID="pnlBotones">        
        <asp:Button runat="server" ID="btnContinuar" Text="Continuar" style="margin-left:20px;" />                    
        <asp:Button runat="server" ID="btnIntegrar" Text="Integrar las hojas de gastos" style="margin-left:20px;" />        
        <asp:Button runat="server" ID="btnVolver" Text="Volver" style="margin-left:20px;" />
        <asp:Button runat="server" ID="btnIntegrarHidden" style="visibility:hidden" />
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfHojas" />
    <asp:HiddenField runat="server" ID="hfStatePag" />
    <asp:HiddenField runat="server" ID="hfConnectionId" />
</asp:Content>
