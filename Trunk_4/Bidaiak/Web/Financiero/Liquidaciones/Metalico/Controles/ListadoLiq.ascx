<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ListadoLiq.ascx.vb" Inherits="WebRaiz.ListadoLiq" %>
<script src="../../../js/jquery/jquery-1.6.4.min.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.signalR-2.3.0.js" type="text/javascript"></script>    
<script src="../../../signalr/hubs" type="text/javascript"></script>  
<script type="text/javascript">
        $(function () {            
            var myhub = $.connection.signalRHub;                
            myhub.client.showMessage = function (message) {                    
                var message = $('<div />').text(message).html();                        
                $('#spanMessage').text(message);
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
            });       
        });
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
            document.getElementById('<%=divProcesando.ClientID%>').style.display = 'inline';
            document.body.style.cursor="progress";
            var btn = document.createElement("progress");
            btn.setAttribute("id", "myProgress");
            btn.setAttribute("value","0");
            btn.setAttribute("max", "100");
            $('#divProgress').append(btn);
        }
    </script>  
    <div>
        <h3><asp:Label runat="server" ID="labelInfo" Text="Seleccione las hojas de gastos a pagar"></asp:Label></h3>
    </div><br />
    <div class="row">
        <div class="col-sm-5">                            
            <asp:Label runat="server" ID="labelFiltrar" Text="Hojas de gastos validadas hasta el dia"></asp:Label>
        </div>
         <div class="col-sm-3">
            <div class="input-group date" id="dtFechaVal">
                <asp:TextBox runat="server" ID="txtFechaVal" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>  
        <div class="col-sm-4">
            <asp:Checkbox runat="server" ID="chbHGEntregada" Text="Hoja de gastos entregada" />
        </div>
    </div>
    <div class="form-group">
        <asp:Button runat="server" ID="btnFiltrar" Text="Filtrar" CssClass="form-control btn btn-primary" OnClientClick="javascript:filtrar()" />
    </div>
    <div class="form-group" runat="server" id="divProcesando" style="display:none">
        <asp:Label runat="server" ID="labelProcesando" Text="Procesando. Espere..." CssClass="labelDetalle"></asp:Label><br />
        <span id="spanMessage" class="labelDetalle"></span>
        <div id="divProgress"></div>
        <asp:Button runat="server" ID="btnFiltrarHidden" style="visibility:hidden" />  
    </div>    
    <asp:Panel runat="server" ID="pnlInfo"> 
        <asp:GridView runat="server" ID="gvLiquidaciones" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="false" CssClass="table tableWithBorder table-striped table-hover" GridLines="None" ShowFooter="true">
	        <EmptyDataTemplate><asp:Label runat="server" ID="labelSinReg" Text="No existe ningun registro o no cumple ninguna de las condiciones del filtro"></asp:Label></EmptyDataTemplate>		        	        
	        <Columns>   
                <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblImportes"></asp:Label></ItemTemplate>
                </asp:TemplateField>
               <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">              
                    <HeaderTemplate><asp:CheckBox runat="server" ID="chbSelectAll" ToolTip="Seleccionar todos" /></HeaderTemplate>
			        <ItemTemplate><asp:CheckBox runat="server" ID="chbMarcar" /></ItemTemplate>
                    <FooterTemplate></FooterTemplate>
		       </asp:TemplateField>                    
               <asp:TemplateField>
                    <HeaderTemplate><asp:Label runat="server" Text="persona"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
                     <FooterTemplate><asp:Label runat="server" ID="labelTotal" text="Total"></asp:Label></FooterTemplate>    
		        </asp:TemplateField>                       
                <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="right">
                    <HeaderTemplate><asp:Label runat="server" Text="Liquidacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblLiquidacion" style="font-size:13px;font-weight:bold;"></asp:Label></ItemTemplate> 
                    <FooterTemplate><asp:Label runat="server" ID="lblTotal" style="font-size:15px;font-weight:bold;"></asp:Label></FooterTemplate>               
		        </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" >
                    <HeaderTemplate><asp:Label runat="server" text="Viajes/Hojas"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Hyperlink runat="server" ID="hlViajeHoja" Target="_blank"></asp:Hyperlink></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField Visible="false">        
                    <ItemTemplate><asp:Label runat="server" ID="lblIdUser"></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                    <HeaderTemplate><asp:Label runat="server" text="Fecha validacion"></asp:Label></HeaderTemplate>
			        <ItemTemplate><asp:Label runat="server" ID="lblFVal"></asp:Label></ItemTemplate>
                </asp:TemplateField>               
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                    <HeaderTemplate><asp:Label runat="server" Text="HG Entregada"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:Image runat="server" ID="imgHGEntreg" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/Aceptada.png"/></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridview-header-center">
                    <HeaderTemplate><asp:Label runat="server" Text="Excluir"></asp:Label></HeaderTemplate>
                    <ItemTemplate><asp:ImageButton runat="server" ID="imgExcluir" ImageAlign="Middle" ImageUrl="~/App_Themes/Tema1/Images/Denegada.png" OnClick="imgExcluir_Click" /></ItemTemplate>
                </asp:TemplateField>
	        </Columns>
        </asp:GridView><br />  
        <asp:Literal runat="server" ID="CheckBoxIDsArray"></asp:Literal>
        <div class="form-group">
            <asp:Button runat="server" ID="btnContinuar" Text="Continuar" CssClass="form-control btn btn-primary" />
        </div> 
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfConnectionId" />