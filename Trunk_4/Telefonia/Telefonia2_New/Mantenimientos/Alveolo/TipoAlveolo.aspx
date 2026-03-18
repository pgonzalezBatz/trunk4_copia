<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="TipoAlveolo.aspx.vb" Inherits="Telefonia.TipoAlveolo"  %> 
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
                <asp:MultiView ID="mvTiposAlveolo" runat="server" ActiveViewIndex="0">    
                 <asp:View runat="server" ID="vListado">   
                 <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnVerDatos">
                 <table width="25%">
                    <tr>
                        <td width="10%">
                            <asp:Label runat="server" ID="labelTipo" text="tipo"></asp:Label>
                        </td>
                        <td nowrap>
                            <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onKeyUpBusqueda();"></asp:TextBox>&nbsp;&nbsp;                                
                            <asp:CheckBox runat="server" ID="chbMostrarObsoletos" text="Mostrar obsoletos" AutoPostBack="true" />
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
                            <asp:Button runat="server" ID="btnVerDatos" text="verDatos" OnClick="btnVerDetalle" CommandName="modificar" /> &nbsp;    
                            <asp:Button runat="server" id="btnNuevo" text="nuevo" OnClick="btnVerDetalle" CommandName="nuevo" />
                        </td>
                    </tr>                        
                 </table> 
                 </asp:Panel>
             </asp:View>
             <asp:View runat="server" ID="vDetalle">
                <fieldset style="width:70%">
                  <table width="100%">                        
                       <asp:Panel runat="server" ID="pnlObsoleto">
                        <tr>
                           <td colspan="2">
                            <asp:CheckBox runat="server" ID="chbObsoleto" text="Obsoleto" />
                            </td>
                        </tr>
                        </asp:Panel>
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gvTipoAlv" AutoGenerateColumns="false" CssClass="GridView" width="90%"> 
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />   
                                    <PagerStyle HorizontalAlign="Center"/>
                                    <PagerSettings PageButtonCount="5" />
                                    <Columns> 
                                        <asp:BoundField DataField="Id" Visible="false" />
                                         <asp:TemplateField >
                                               <HeaderTemplate>   
                                                    <asp:Label runat="server" text="cultura"></asp:Label>                                                
                                               </HeaderTemplate>
                                              <ItemTemplate>
                                                    <asp:Label ID="lblCultura" runat="server" Text='<%#Eval("Cultura")%>'></asp:Label>                                                                                                            
                                              </ItemTemplate>
                                         </asp:TemplateField> 
                                          <asp:TemplateField HeaderStyle-Width="80%">
                                               <HeaderTemplate>
                                                    <asp:Label runat="server" text="tipo"></asp:Label>
                                               </HeaderTemplate>
                                              <ItemTemplate>
                                                    <asp:Textbox ID="txtNombre" runat="server" Text='<%#Eval("Nombre")%>' Width="75%"></asp:Textbox>                                                                                                            
                                              </ItemTemplate>
                                         </asp:TemplateField>                                          
                                    </Columns>
                               </asp:GridView> 
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnGuardar" text="Guardar" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnEliminar" text="Eliminar" Visible="false" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnVolver" text="volver" />
                                <cc1:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="btnEliminar">
                                </cc1:ConfirmButtonExtender>             
                            </td>
                        </tr> 
                      </table>                                                                                                                                                  
                  </fieldset>                   
                    
               </asp:View>                  
          </asp:MultiView> 
           <div style="visibility:hidden;">
                <asp:ListBox runat="server" ID="lbAuxiliar"></asp:ListBox>     
            </div> 
        </ContentTemplate>       
    </asp:UpdatePanel> 
    
     
    
   
</asp:Content>
