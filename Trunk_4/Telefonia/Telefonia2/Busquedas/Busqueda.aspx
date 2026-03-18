<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="Busqueda.aspx.vb" Inherits="Telefonia.Busqueda"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ Register Src="~/Controls/flash.ascx" TagName="Flash" TagPrefix="fls" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">    
    <script src="../js/Utiles.js" type="text/javascript"></script>    
    <script language="javascript" type="text/javascript">

        function seleccionarPerso() {
            var lista = document.getElementById('<%=listaPerso.ClientId%>');
            document.getElementById('<%=txtpersona.ClientId%>').value = lista[lista.selectedIndex].text;
        }

        function seleccionarDep() {
            var lista = document.getElementById('<%=listaDep.ClientId%>');
            document.getElementById('<%=txtdepartamento.ClientId%>').value = lista[lista.selectedIndex].text;
        }

        function onkeyUpBusquedaPerso() {
            var listaAux = document.getElementById('<%=lbAuxiliar1.ClientId%>');
            var lista = document.getElementById('<%=listaPerso.ClientId%>');
            var patron = document.getElementById('<%=txtpersona.ClientId%>').value;
            var text;
            var encontrados;
            var regEx = new RegExp(patron, "i");
            lista.length = 0;
            for (var idx = 0; idx < listaAux.length; idx++) {
                text = listaAux[idx].text;
                encontrados = text.match(regEx);

                if (encontrados != null) {
                    if (encontrados.length > 0)
                        lista.options[lista.length] = new Option(text, listaAux[idx].value);
                }
            }
            if (lista.length == 1) {
                lista.selectedIndex = 0;
            }
            else
                lista.selectedIndex = -1;
        }

        function onkeyUpBusquedaDep() {
            var listaAux = document.getElementById('<%=lbAuxiliar2.ClientId%>');
            var lista = document.getElementById('<%=listaDep.ClientId%>');
            var patron = document.getElementById('<%=txtdepartamento.ClientId%>').value;
            var text;
            var encontrados;
            var regEx = new RegExp(patron, "i");
            lista.length = 0;
            for (var idx = 0; idx < listaAux.length; idx++) {
                text = listaAux[idx].text;
                encontrados = text.match(regEx);

                if (encontrados != null) {
                    if (encontrados.length > 0)
                        lista.options[lista.length] = new Option(text, listaAux[idx].value);
                }
            }
            if (lista.length == 1) {
                lista.selectedIndex = 0;
            }
            else
                lista.selectedIndex = -1;
        }

        function dobleClickPerso() {
            var btnVerDatosP = document.getElementById('<%=btnVerDatosPerso.ClientId%>').name;
            __doPostBack(btnVerDatosP, '');
        }

        function dobleClickDpto() {
            var btnVerDatosD = document.getElementById('<%=btnVerDatosDepartamento.ClientId%>').name;
            __doPostBack(btnVerDatosD, '');
        }
        
	    var ModalProgress ='<%= ModalProgress.ClientID %>';               
	  
	    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {          
            $find(ModalProgress).show();
        }
        function endRequest(sender, args) {
            $find(ModalProgress).hide();
        }
    </script>                                
    <table width="80%">
        <tr>
            <td class="izquierda" valign="top"><asp:Image runat="server" ID="imgLogo" /></td>
            <td class="derecha" valign="bottom"><asp:Image runat="server" ID="imgDir"/></td>
        </tr>
        </table>
        <fieldset style="width:80%">
            <div>
                <asp:Label runat="server" ID="labelSelPlanta" text="seleccionePlanta"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlPlanta" AutoPostBack="true"></asp:DropDownList>
            </div>                   
            <asp:UpdatePanel ID="upBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>              
                    <table cellspacing="10px">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlBusquedaPerso" runat="server" DefaultButton="btnVerDatosPerso">
                                <table>
                                    <tr>                                        
                                        <td colspan="2">                                            
                                            <asp:TextBox runat="server" ID="txtPersona" onkeyup="javascript:onkeyUpBusquedaPerso();" Width="250px"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="tbwePersona" runat="server" TargetControlID="txtPersona" WatermarkCssClass="watermark"></cc1:TextBoxWatermarkExtender>
                                        </td>                        
                                    </tr>
                                    <tr>                                        
                                        <td colspan="2"><asp:ListBox runat="server" ID="listaPerso" Rows="8" CssClass="font11May" Width="260px"></asp:ListBox></td>                        
                                    </tr> 
                                    <tr><td colspan="2">&nbsp;</td></tr> 
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button runat="server" ID="btnVerDatosPerso" text="verDatos" OnClick="btnVerDatos" CommandName="P" /> &nbsp;</td>
                                    </tr>                        
                                    </table> 
                                </asp:Panel>
                            </td>
                            <td>&nbsp;</td>                            
                            <td>   
                                <asp:Panel ID="pnlBusquedaDep" runat="server" DefaultButton="btnVerDatosDepartamento">                           
                                <table>
                                    <tr>                                        
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtDepartamento" onkeyup="javascript:onkeyUpBusquedaDep();" Width="250px"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="tbweDepartamento" runat="server" TargetControlID="txtDepartamento" WatermarkCssClass="watermark"></cc1:TextBoxWatermarkExtender>
                                        </td>                        
                                    </tr>
                                    <tr>                       
                                        <td colspan="2"><asp:ListBox runat="server" ID="listaDep" Rows="8" CssClass="font11" Width="260px"></asp:ListBox></td>                        
                                    </tr> 
                                    <tr><td colspan="2">&nbsp;</td></tr> 
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button runat="server" ID="btnVerDatosDepartamento" text="verDatos" OnClick="btnVerDatos" CommandName="D" /> &nbsp;</td>
                                    </tr>                        
                                </table>
                                </asp:Panel> 
                            </td>                            
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr /><asp:Label runat="server" ID="labelExt" Text="Introduzca la extension a buscar"></asp:Label>&nbsp;
                                <asp:TextBox runat="server" ID="txtExtension" MaxLength="5" Columns="5" style="text-align:center;" onkeydown="return soloNumeros(event);"></asp:TextBox>
                                <asp:Button runat="server" ID="btnVerDatosExt" Text="Buscar extension" style="margin-left:20px;" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>             
       </fieldset><br /><br /> 
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlResul">
                <fieldset style="width:80%">
                    <table>
                        <tr>
                            <asp:Panel ID="pnlBusqPersona" runat="server">
                                <td>
                                    <asp:Image runat="server" ImageUrl="~/App_Themes/Tema1/Images/persona_small.gif" />&nbsp;
                                    <asp:Label ID="lblUsuario" runat="server" CssClass="negrita"></asp:Label>                            
                                </td>
                                <td>&nbsp;</td>
                            </asp:Panel>
                            <td>
                                <asp:Image runat="server" ImageUrl="~/App_Themes/Tema1/Images/departamento.gif" />&nbsp;
                                <asp:Label ID="lblDepartamento" runat="server" CssClass="negrita"></asp:Label>                            
                            </td>
                        </tr>
                    </table><br />
                    <asp:Panel runat="server" ID="pnlListado1">
                    <table width="85%" class="listadoRepeater">                                    
                        <asp:Repeater runat="server" ID="rptExten">                                                
                            <HeaderTemplate>
                                <thead>
                                    <tr>
                                        <th style="text-align:left" width="8%"><asp:Literal runat="server" text="planta"></asp:Literal></th>
                                        <th style="text-align:left" runat="server" id="thCabNikEuskeraz" visible='<%#VisualizarColumna()%>'></th>
                                        <th runat="server" visible='<%#VisualizarColumna()%>'><asp:Literal runat="server" text="nombre"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Extension fija" ImageUrl="~/App_Themes/Tema1/Images/telephone.png" /></th>
                                        <th><asp:Literal runat="server" text="Nº directo"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Extension inalambrica" ImageUrl="~/App_Themes/Tema1/Images/wireless.png"/></th>
                                        <th><asp:Literal runat="server" text="Nº directo"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/mobile.png"/></th>
                                        <th><asp:Literal runat="server" text="movil"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Zoiper" ImageUrl="~/App_Themes/Tema1/Images/zoiper.png"/></th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>
                                <ItemTemplate>
                                <tr runat="server" id="myTr">   
                                    <td style="text-align:left"><asp:Label runat="server" id="lblPlanta"></asp:Label></td>
                                    <td style="text-align:center" runat="server" visible='<%#VisualizarColumna()%>'><asp:Image ID="imgNE" runat="server" ImageUrl="~/App_Themes/Tema1/Images/NikEuskaraz_txiki.jpg" ToolTip="Nik euskaraz" /></td>                                                                          
                                    <td style="text-align:left;display:inline;" runat="server" visible='<%#VisualizarColumna()%>'><asp:Label runat="server" ID="lblNombre" CssClass="mayusculas"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" id="lblExtFija"></asp:Label>&nbsp;                                           
                                        <asp:ImageButton ID="imgLlamarDirecto" runat="server" tooltip="Llamar zoiper" ImageUrl="~/App_Themes/Tema1/Images/zoiper.png" OnClick="LlamadaZoiper" />
                                    </td>
                                    <td><asp:Label runat="server" id="lblFijo"></asp:Label></td>
                                    <td><asp:Label runat="server" ID="lblExtInalambrico"></asp:Label>&nbsp;</td>
                                    <td><asp:Label runat="server" ID="lblInalambrico"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" id="lblExtMovil"></asp:Label>
                                        <asp:ImageButton ID="imgLlamarMovil" runat="server" tooltip="Llamar zoiper" ImageUrl="~/App_Themes/Tema1/Images/zoiper.png" OnClick="LlamadaZoiper" />
                                    </td>
                                    <td><asp:Label runat="server" id="lblNumMovil"></asp:Label></td>
                                    <td><asp:Label runat="server" ID="lblZoiper"></asp:Label></td>  
                                </tr>
                            </ItemTemplate>                              
                        </asp:Repeater>
                    </table> 
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlListado2">
                    <table width="85%" class="listadoRepeater">                                    
                        <asp:Repeater runat="server" ID="rptExten2">                                                
                            <HeaderTemplate>
                                <thead>
                                    <tr>
                                        <th style="text-align:left" width="8%"><asp:Literal runat="server" text="planta"></asp:Literal></th>
                                        <th id="Th1" style="text-align:left" runat="server" visible='<%#VisualizarColumna()%>'><asp:Literal runat="server" text="nombre"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Extension fija" ImageUrl="~/App_Themes/Tema1/Images/telephone.png" /></th>
                                        <th><asp:Literal runat="server" text="Nº fijo"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Extension inalambrica" ImageUrl="~/App_Themes/Tema1/Images/wireless.png"/></th>
                                        <th><asp:Literal runat="server" text="Nº inalambrico"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/mobile.png"/></th>
                                        <th><asp:Literal runat="server" text="Movil"></asp:Literal></th>
                                        <th><asp:Image runat="server" tooltip="Skype" ImageUrl="~/App_Themes/Tema1/Images/skype.png"/></th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>
                                <ItemTemplate>
                                    <tr runat="server" id="myTr">   
                                    <td style="text-align:left"><asp:Label runat="server" id="lblPlanta" Text="Matrici"></asp:Label></td>                                    
                                    <td id="Td1" style="text-align:left" runat="server" visible='<%#VisualizarColumna()%>'><asp:Label runat="server" ID="lblNombre" Text='<%#Eval("NombreCompleto")%>' CssClass="mayusculas"></asp:Label></td>
                                    <td><asp:Label runat="server" Text='<%#FormatInt(Eval("ExtFija"))%>'></asp:Label>&nbsp;</td>
                                    <td><asp:Label runat="server" Text='<%#Eval("Fijo")%>'></asp:Label></td>
                                    <td><asp:Label runat="server" Text='<%#FormatInt(Eval("ExtInalambrica"))%>'></asp:Label>&nbsp;</td>
                                    <td><asp:Label runat="server" Text='<%#Eval("Inalambrico")%>'></asp:Label></td>
                                    <td><asp:Label runat="server" Text='<%#FormatInt(Eval("ExtMovil"))%>'></asp:Label>&nbsp;</td>
                                    <td><asp:Label runat="server" Text='<%#Eval("Movil")%>'></asp:Label></td>
                                    <td><asp:Label runat="server" Text='<%#FormatInt(Eval("Skype"))%>'></asp:Label></td>                                   
                                </tr>
                            </ItemTemplate>                               
                        </asp:Repeater>
                    </table> 
                    </asp:Panel><br />
                    <table>
                        <tr>
                            <asp:Panel runat="server" ID="pnlFoto">
                                <td valign="middle"><asp:Image runat="server" ID="imgFoto" ImageAlign="AbsMiddle" /></td>
                                <td width="50px">&nbsp;</td>
                                <td><asp:Image runat="server" ID="imgNikEuskaraz" ImageUrl="~/App_Themes/Tema1/Images/NikEuskaraz.png" ToolTip="Nik Euskaraz" /></td>
                            </asp:Panel>                        
                            <td><fls:Flash runat="server" id="flsSituacion" ></fls:Flash></td>
                        </tr>
                    </table>                                
                    </fieldset>  
                </asp:Panel>   
                <asp:HiddenField ID="hfTipoBusqueda" runat="server" />
                <asp:Panel ID="pnlZoiper" runat="server" Style="display: none;" CssClass="modalBox">
                <table>
                    <tr>
                        <td><iframe runat="server" id="ifZoiper" width="500px" height="350px" /></td>
                        <td valign="top"><asp:ImageButton runat="server" ID="imgCerrar" ImageUrl="~/App_Themes/Tema1/images/cerrar.gif" /></td>
                    </tr>                   
                </table>
                </asp:Panel>
                <asp:Label runat="server" ID="lblHidden" Style="display: none"></asp:Label>
                <cc1:ModalPopupExtender ID="mpeZoiper" runat="server" TargetControlID="lblHidden" PopupControlID="pnlZoiper" BackgroundCssClass="modalBackground" />
            </ContentTemplate> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnVerDatosPerso" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnVerDatosDepartamento" EventName="Click" />            
        </Triggers>      
    </asp:UpdatePanel>     
    <div style="visibility:hidden;">
        <asp:ListBox runat="server" ID="lbAuxiliar1"></asp:ListBox>     
        <asp:ListBox runat="server" ID="lbAuxiliar2"></asp:ListBox>     
    </div>      
     <asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress">
        <asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server">
            <ProgressTemplate>
                <div style="position: relative; top: 30%; text-align: center;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/Images/loadin.gif" />
                    <asp:Label ID="lblFiltrando" runat="server" Text="cargandoDatos"></asp:Label>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground2" PopupControlID="panelUpdateProgress" />
    <asp:HiddenField runat="server" ID="hfUsuarioZoiper" />   
</asp:Content>
