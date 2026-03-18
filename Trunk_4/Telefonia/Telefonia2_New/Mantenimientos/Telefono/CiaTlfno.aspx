<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="CiaTlfno.aspx.vb" Inherits="Telefonia.CiaTlfno" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>

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
        
         function onKeyUpBusqueda()
        {
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
              lista.selectedIndex=-1;
        }  
          
    </script> 
       <asp:UpdatePanel runat="server">
           <ContentTemplate>                
                <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnVerDatos"> 
                 <table width="25%">
                    <tr>
                        <td width="10%">
                             <asp:Label runat="server" ID="labelComp" Text="compañia"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onKeyUpBusqueda();"></asp:TextBox>&nbsp;&nbsp;                                
                            <asp:Checkbox runat="server" ID="chbMostrarObsoletos" Text="Mostrar obsoletos" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                              <asp:ListBox runat="server" ID="lbLista" Rows="6" width="100%"></asp:ListBox>        
                        </td>
                    </tr> 
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr> 
                    <tr>
                       <td>&nbsp;</td>
                       <td>
                            <asp:Button runat="server" ID="btnVerDatos" Text="verDatos" OnClick="btnVerDetalle" CommandName="modificar" /> &nbsp;    
                            <asp:Button runat="server" id="btnNuevo" Text="nuevo" OnClick="btnVerDetalle" CommandName="nuevo" />
                        </td>
                    </tr>                    
                 </table> 
                </asp:Panel>
             <asp:Label runat="server" ID="lblHiddenPopUp" Style="display: none"></asp:Label>         
            <cc1:ModalPopupExtender ID="mpeCia" runat="server"
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
                                <asp:Panel runat="server" ID="pnlError">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="lblError" CssClass="MensajeError"></asp:Label>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td>
                                         <asp:Label runat="server" ID="labelCompañia" Text="compañia"></asp:Label>    
                                    </td>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlExistente">
                                            <asp:label runat="server" ID="lblCia"></asp:label>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlNuevo">
                                            <asp:TextBox runat="server" ID="txtCia"></asp:TextBox> 
                                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCia" ID="rfvCia" ValidationGroup="Cia" Display="Dynamic" Text="introduzcaCompañia"></asp:RequiredFieldValidator>                                              
                                              <cc1:ValidatorCalloutExtender ID="vceCia" runat="server" 
                                                TargetControlID="rfvCia" 
                                                Width="200px"
                                                HighlightCssClass="highlight" 
                                                WarningIconImageUrl="~/App_Themes/Tema1/Images/warning.gif"
                                                CloseImageUrl="~/App_Themes/Tema1/Images/close.gif">
                                            </cc1:ValidatorCalloutExtender>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         <asp:Label runat="server" ID="labelPref" Text="prefijo"></asp:Label>    
                                    </td>
                                    <td>                                        
                                       <asp:TextBox runat="server" ID="txtPrefijo"></asp:TextBox>    
                                    </td>
                                </tr>  
                                 <asp:Panel runat="server" ID="pnlObsoleto">
                                <tr>
                                    <td colspan="2">
                                        <asp:Checkbox runat="server" ID="chkObsoleto" Text="obsoleto" TextAlign="Left" />                                        
                                    </td>
                                </tr>                                   
                                </asp:Panel>                                                              
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr> 
                                    <td colspan="2" align="center">
                                        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" />&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" Visible="false" />
                                        <cc1:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="btnEliminar">
                                        </cc1:ConfirmButtonExtender>                                        
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
