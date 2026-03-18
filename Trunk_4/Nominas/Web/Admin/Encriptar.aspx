<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Encriptar.aspx.vb" Inherits="Nominas.Encriptar" EnableEventValidation="false" ValidateRequest="false"%>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript">
        $(window).on("load", function () {
            $('.summernote').summernote({
                lang: 'eu-ES',
                placeholder: 'Oharra',
                toolbar: [
                 ['style', ['style']],
                 ['fontname', ['fontname']],
                 ['font', ['bold', 'italic', 'underline', 'clear']],
                 ['color', ['color']],
                 ['para', ['ul', 'ol', 'paragraph']],
                 ['table', ['table']],
                 ['insert', ['link']],
                 ['view', ['fullscreen', 'help']]
                ]
            });
        });

    </script>
    <div class="row">
        <div class="col-sm-1 col-xs-2"><asp:Label runat="server" ID="labelPlanta" Text="Planta"></asp:Label></div>
        <div class="col-sm-11 col-xs-10"><asp:Label runat="server" ID="lblPlanta" style="font-weight:bold"></asp:Label></div>
    </div>
    <asp:Panel runat="server" ID="pnlInfo"><br />
	    <div class="text-justify"><asp:Label runat="server" ID="labelInfo" Text="Este proceso encriptara automaticamente las nominas especificadas y las dejara preparadas para que se puedan enviar por email. Al final del proceso, se informara de cuantas se han encriptado correctamente y cuales han dado error"></asp:Label></div><br /><br />   
    	<div class="col-sm-4"><asp:Button runat="server" ID="btnChequear" text="Chequear nominas a encriptar" ToolTip="Comprueba si existen nominas a encriptar" CssClass="btn btn-primary col-xs-12" OnClientClick="myCheckFunction();"  UseSubmitBehavior="false" /></div><br /><br />
    </asp:Panel>
	<asp:Panel runat="server" ID="pnlResultados" CssClass="panel panel-default">
         <div class="panel-heading">
            <strong><asp:Label runat="server" ID="labelTitleNominas" Text="Nominas a encriptar" style="text-transform:uppercase"></asp:Label></strong>
        </div>
        <div class="panel-body">
            <ul>
		     <table>
			    <asp:Repeater runat="server" ID="rptNominas">
				    <ItemTemplate>
					    <tr>  
						    <td><li><asp:Label runat="server" ID="lblGrupo"></asp:Label></td>	                              
						    <td><asp:Label runat="server" ID="lblNumArchivos" /></td>								        																				            	                    
					    </tr>
				    </ItemTemplate>
			    </asp:Repeater>
		    </table>
            </ul>
            <br />
                <asp:Label runat="server" Text="Añadir nota común para todos los emails: (opcional)"></asp:Label>
                <textarea class="summernote" id="snOharra" runat="server"></textarea>

            <div class="form-inline">
		        <asp:Button runat="server" ID="btnEncriptar" Text="Encriptar y enviar aviso" CssClass="btn btn-primary" OnClientClick="myEncryptFunction(this);" UseSubmitBehavior="false"/>
                <asp:Button runat="server" ID="btnEncriptarPru" Text="Encriptar prueba (no escribe en BBDD)" CssClass="btn btn-primary"  OnClientClick="myEncryptFunction(this)" UseSubmitBehavior="false"/>
	        </div>	
        </div>										
    </asp:Panel><br />
    <asp:Panel runat="server" ID="pnlSinResultados" cssClass="alert alert-warning">
		<asp:Label runat="server" ID="labelSinResul" Text="No existen nominas por tratar" style="font-weight:bold;"></asp:Label>
    </asp:Panel>	
	<asp:Panel runat="server" ID="pnlResulEncript" CssClass="panel panel-default"> 
        <div class="panel-heading">
            <strong><asp:Label runat="server" ID="lblResul" Text="Resultado" style="text-transform:uppercase"></asp:Label></strong>
            
        </div>
        <div class="panel-body">
            <asp:Label runat="server" ID="lblMensa"></asp:Label>
            <br />
            <br />
            <asp:Repeater ID="allRes" runat="server">
                <HeaderTemplate>
                    <table id="tablaResultados" class="tablaResul table table-hover table-bordered table-responsive">
                        <thead>
                            <tr>
                                <th><asp:Label text="Cod" runat="server"></asp:Label></th>
                                <th><asp:Label text="Nombre" runat="server"></asp:Label></th>
                                <th><asp:Label text="Fecha" runat="server"></asp:Label></th>
                                <th><asp:Label text="Email" runat="server"></asp:Label></th>
                                <th><asp:Label text="Encriptado" runat="server"></asp:Label></th>
                                <th><asp:Label text="Envío Mail" runat="server"></asp:Label></th>
                            </tr>
                        </thead>
                    <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr>
                            <td><%#Eval("CodPersona")%></td>
                            <td><%#Eval("Nombre")%></td>
                            <td id="fecha" runat="server"></td>
                            <td><%#Eval("Mail")%></td>
                            <td><asp:Image ID="imgEnc" runat="server" ImageUrl='<%# If(Eval("EncriptadoOK").Equals(True), "~\App_Themes\Tema1\Images\info.gif", "~\App_Themes\Tema1\Images\error.gif") %>'/></td>
                            <td><asp:Image ID="imgEnv" runat="server" ImageUrl='<%# If(Eval("EnvioOK").Equals(True), "~\App_Themes\Tema1\Images\info.gif", "~\App_Themes\Tema1\Images\error.gif") %>'/></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater> 
        </div>
	</asp:Panel>
    <script>
        
        function myCheckFunction() {
            //var c = confirm('< %=itzultzaileWeb.Itzuli("Este proceso puede durar unos segundos. Se dispone a copiar todas las nominas al directorio local y realizar un recuento de las nominas a encriptar") %>');
            var c = confirm('<%=LocalizationLib.Itzuli("Este proceso puede durar unos segundos. Se dispone a copiar todas las nominas al directorio local y realizar un recuento de las nominas a encriptar") %>');
            if(!c)
                return false;
            else {
                var btn=$('#<%=btnChequear.ClientId%>');
                btn.attr("disabled", "disabled");
                btn.val('Chequeando...');
                return true;
            }
        }
        
        function myEncryptFunction(sender) {
            var test = "";
            var btn, btn2;
            if (sender.id.indexOf('Pru') >= 0) {
                test = " (Test)"                
                btn = $('#<%=btnEncriptarPru.ClientId%>');
                btn2 = $('#<%=btnEncriptar.ClientId%>');
            } else {
                btn=$('#<%=btnEncriptar.ClientId%>');
                btn2 = $('#<%=btnEncriptarPru.ClientId%>');
            }
            //var c = confirm('< %=itzultzaileWeb.Itzuli("Este proceso puede durar varios segundos. Se dispone a encriptar las nominas y a copiarlas al directorio de origen ¿Desea encriptar las nominas especificadas?") %>');
            var c = confirm('<%=LocalizationLib.Itzuli("Este proceso puede durar varios segundos. Se dispone a encriptar las nominas y a copiarlas al directorio de origen ¿Desea encriptar las nominas especificadas?") %>');
            if(!c)
                return false;
            else {
                btn.attr("disabled", "disabled");
                btn2.attr("style", "visibility:hidden")
                btn.val('Encriptando...' + test);
                return true;
            }
        }
    </script>
</asp:Content>