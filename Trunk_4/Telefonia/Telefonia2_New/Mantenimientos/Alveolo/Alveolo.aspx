<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="Alveolo.aspx.vb" Inherits="Telefonia.Alveolo"  %> 
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>   
 <%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">    
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
        
        function onKeyUpBusqueda() {
            var listaAux=document.getElementById('<%=lbAuxiliar.ClientId%>');            
            var lista=document.getElementById('<%=lbLista.ClientId%>');                      
            var patron=document.getElementById('<%=txtBuscar.ClientId%>').value;            
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <asp:MultiView ID="mvAlveolos" runat="server" ActiveViewIndex="0">    
              <asp:View runat="server" ID="vListado">                               
                <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnVerDatos">
                     <table width="25%">
                        <tr>
                            <td width="10%"><asp:Label runat="server" ID="labelAlv" text="alveolo"></asp:Label></td>
                            <td nowrap>
                                <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onKeyUpBusqueda();"></asp:TextBox> &nbsp;&nbsp;                                
                                <asp:CheckBox runat="server" ID="chbMostrarObsoletos" text="Mostrar obsoletos" AutoPostBack="true" />
                            </td>                        
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><asp:ListBox runat="server" ID="lbLista" Rows="6" width="100%"></asp:ListBox></td>                        
                        </tr> 
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr> 
                        <tr>
                           <td>&nbsp;</td>
                           <td>
                                <asp:Button runat="server" ID="btnVerDatos" text="verDatos" OnClick="btnVerDetalle" CommandName="modificar" /> &nbsp;    
                                <asp:Button runat="server" id="btnNuevo" text="nuevo" OnClick="btnVerDetalle" CommandName="nuevo"></asp:Button>
                                <asp:Button runat="server" ID="btnRepartidor" text="Ir al repartidor" style="margin-left:15px;"/>
                            </td>
                        </tr>                        
                     </table> 
                 </asp:Panel>
            </asp:View>
            <asp:View runat="server" ID="vDetalle">                                                                   
                  <fieldset style="width:50%">
                   <table> 
                    <tr>
                        <td><asp:Label runat="server" ID="labelRuta" text="ruta"></asp:Label></td>
                        <td>
                            <asp:Panel runat="server" ID="pnlExistente"><asp:label runat="server" ID="lblRuta"></asp:label></asp:Panel>
                            <asp:Panel runat="server" ID="pnlNuevo">
                                <asp:TextBox runat="server" ID="txtRuta"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="meRuta" runat="server" TargetControlID="txtRuta" MaskType="Number" Mask="9-9-99" InputDirection="LeftToRight" AutoComplete ="true" AutoCompleteValue="*" ClearMaskOnLostFocus="false" />                                  
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="labelTipo" text="tipoAlveolo"></asp:Label></td>
                        <td><asp:DropDownList runat="server" ID="ddlTipo" AppendDataBoundItems="true"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="labelEst" text="estado"></asp:Label></td>                                
                        <td><asp:DropDownList runat="server" ID="ddlEstados" AppendDataBoundItems="true"></asp:DropDownList></td>                            
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel runat="server" ID="pnlPosicion" GroupingText="Posicion">
                                <asp:Label runat="server" ID="labelFila" Text="Fila"></asp:Label>
                                <asp:TextBox runat="server" ID="txtFila" Columns="2" style="text-align:center;" onkeydown="return soloNumeros(event);"></asp:TextBox>
                                <asp:Label runat="server" ID="labelCol" Text="Columna" style="margin-left:15px;"></asp:Label>
                                <asp:TextBox runat="server" ID="txtCol" Columns="2" style="text-align:center;" onkeydown="return soloNumeros(event);"></asp:TextBox>
                            </asp:Panel>
                        </td>
                    </tr>
                    <asp:Panel runat="server" ID="pnlObsoleto">
                    <tr>
                        <td colspan="2"><asp:CheckBox runat="server" ID="chbObsoleto" text="Obsoleto" /></td>                                                        
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr> 
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btnGuardar" text="Guardar" ValidationGroup="save" />&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnEliminar" text="Eliminar" Visible="false" />&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnVolver" text="volver" />                            
                            <cc1:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="btnEliminar" />                            
                        </td>
                    </tr>                                                                                   
                </table> 
              </fieldset>
           </asp:View>
        </asp:MultiView>
        <div style="visibility:hidden;"><asp:ListBox runat="server" ID="lbAuxiliar"></asp:ListBox></div> 
      </ContentTemplate>
    </asp:UpdatePanel>   
</asp:Content>
