<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master" CodeBehind="Otros.aspx.vb" Inherits="Telefonia.Otros" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script src="../../js/Utiles.js" type="text/javascript"></script>
    
    <script language="javascript">
        function seleccionarItem()
        {            
            var lista=document.getElementById('<%=lbLista.ClientId%>');
            document.getElementById('<%=txtBuscar.ClientId%>').value=lista[lista.selectedIndex].text;            
        } 
        
        function dobleClick()
        {
          var btnVerDatos=document.getElementById('<%=btnVerDatos.ClientId%>').name;                              
           __doPostBack(btnVerDatos,'');
        }   
        
        function onkeyupBusqueda() {
            var listaAux = document.getElementById('<%=lbAuxiliar.ClientId%>');
            var lista = document.getElementById('<%=lbLista.ClientId%>');
            var patron = document.getElementById('<%=txtBuscar.ClientId%>').value;                  
            var text;      
            var encontrados;       
            var regEx=new RegExp(patron,"i");
            
            lista.length=0;                                              
                              
             for (var idx = 0; idx < listaAux.length; idx ++) {                                                                        
                text=listaAux[idx].text;                                                
                encontrados =text.match(regEx);                        

                if(encontrados!=null) 
                {                  
                    if(encontrados.length>0) 
                        lista.options[lista.length]= new Option (text, listaAux[idx].value);              
                }                               
            }
                        
           if(lista.length==1)
           {
              lista.selectedIndex=0;
           }
           else
               lista.selectedIndex = -1;
        }  
        
    </script> 
       <asp:UpdatePanel runat="server">
           <ContentTemplate>
                <tit:Titulo runat="server" Texto="otros" />                                            
               <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnVerDatos"> 
                 <table width="25%">
                    <tr>
                        <td width="10%">
                             <asp:Label runat="server" ID="labelOtros" text="otros"></asp:Label>
                        </td>
                        <td nowrap>
                            <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onkeyupBusqueda();"></asp:TextBox>&nbsp;&nbsp;                                
                            <asp:Checkbox runat="server" ID="chbMostrarObsoletos" text="Mostrar obsoletos" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                              <asp:ListBox runat="server" ID="lbLista" Rows="6" Width="400px"></asp:ListBox>        
                        </td>
                    </tr> 
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr> 
                    <tr>
                       <td>&nbsp;</td>
                       <td>
                            <asp:Button runat="server" ID="btnVerDatos" text="verDatos" OnClick="btnVerDetalle" CommandName="modificar" /> &nbsp;    
                            <asp:Button runat="server" id="btnNuevo" text="nuevo" OnClick="btnVerDetalle" CommandName="nuevo" />
                        </td>
                    </tr>                    
                 </table> 
            </asp:Panel>
             <asp:Label runat="server" ID="lblHiddenPopUp" Style="display: none"></asp:Label>         
            <cc1:ModalPopupExtender ID="mpeOtro" runat="server"
                   TargetControlID="lblHiddenPopUp"
                   PopupControlID="pnlPopUp" 
                   CancelControlID="imgCerrar"
                   BackgroundCssClass="modalBackground"/> 
            <asp:Panel runat="server" ID="pnlPopUp" Style="display: none;" CssClass="modalBox">
              <table>
                 <tr>
                    <td>
                        <fieldset>
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <tit:Titulo runat="server" ID="titPopPup"></tit:Titulo>  
                                    </td>
                                </tr>
                                <asp:Panel runat="server" ID="pnlError">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="lblError" CssClass="MensajeError"></asp:Label>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td>
                                         <asp:Label runat="server" ID="labelOtros2" text="otros"></asp:Label>    
                                    </td>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlExistente">
                                            <asp:label runat="server" ID="lblOtro" CssClass="negrita"></asp:label>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlNuevo">
                                            <asp:TextBox runat="server" ID="txtOtro"></asp:TextBox> 
                                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOtro" ID="rfvOtro" ValidationGroup="Cia" Display="Dynamic" text="introduzcaNombre"></asp:RequiredFieldValidator>                                              
                                              <cc1:ValidatorCalloutExtender ID="vceCia" runat="server" 
                                                TargetControlID="rfvOtro" 
                                                Width="200px"
                                                HighlightCssClass="highlight" 
                                                WarningIconImageUrl="~/App_Themes/Tema1/Images/warning.gif"
                                                CloseImageUrl="~/App_Themes/Tema1/Images/close.gif">
                                            </cc1:ValidatorCalloutExtender>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <asp:Panel runat="server" ID="pnlObsoleto">
                                <tr>
                                    <td colspan="2">
                                        <asp:Checkbox runat="server" ID="chkObsoleto" text="obsoleto" TextAlign="Left" />                                        
                                    </td>
                                </tr>                                   
                                </asp:Panel>                           
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr> 
                                    <td colspan="2" align="center">
                                        <asp:Button runat="server" ID="btnGuardar" text="Guardar" />&nbsp;&nbsp;
                                    </td>
                                </tr>                                                                                   
                            </table> 
                          </fieldset>                   
                    </td>
                     <td valign="top">
                        <asp:ImageButton runat="server" ID="imgCerrar" ImageUrl="~/App_Themes/Tema1/images/cerrar.gif" />                     
                     </td>
                </tr>
               </table>     
            </asp:Panel>    
           <div style="visibility:hidden;">
             <asp:ListBox runat="server" ID="lbAuxiliar"></asp:ListBox>     
           </div> 
        </ContentTemplate>       
    </asp:UpdatePanel> 
</asp:Content>
