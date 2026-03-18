<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master" CodeBehind="TipoLinea.aspx.vb" Inherits="Telefonia.TipoLinea"  %>
 <%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
 <%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">    
    <script src="../../js/Utiles.js" type="text/javascript"></script>
    
    <script language="javascript">
        function seleccionarItem()
        {            
            var lista=document.getElementById('<%=lbLista.ClientId%>');
            document.getElementById('<%=txtBuscar.ClientId%>').value=lista[lista.selectedIndex].text;            
        }  
        
        function seleccionarItem2()
        {            
            var lista=document.getElementById('<%=lbLista2.ClientId%>');
            document.getElementById('<%=txtBuscar2.ClientId%>').value=lista[lista.selectedIndex].text;            
        }     
        
        function dobleClick1()
        {
          var btnVerDatos1=document.getElementById('<%=btnVerDatos.ClientId%>').name;                              
           __doPostBack(btnVerDatos1,'');
        }
        
        function dobleClick2()
        {
          var btnVerDatos2=document.getElementById('<%=btnVerDatosM.ClientId%>').name;                              
           __doPostBack(btnVerDatos2,'');
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
        
        
        function onKeyUpBusqueda2()
        {
            var listaAux=document.getElementById('<%=lbAuxiliar2.ClientId%>');            
            var lista=document.getElementById('<%=lbLista2.ClientId%>');                       
            var patron=document.getElementById('<%=txtBuscar2.ClientId%>').value;            
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
                <tit:Titulo runat="server" ID="titLin" Texto="listadoTiposLineas" />                                         
                <asp:MultiView ID="mvTiposLin" runat="server" ActiveViewIndex="0">    
                    <asp:View runat="server" ID="vListado"> 
                       <cc1:TabContainer ID="tabC" runat="server" ActiveTabIndex="0" Width="80%">
                         <cc1:TabPanel ID="tabP1" runat="server" HeaderText="interna">
                            <ContentTemplate>
                                 <asp:Panel ID="pnlBusqueda1" runat="server" DefaultButton="btnVerDatos">
                                 <table width="50%">
                                    <tr>
                                        <td width="10%">
                                            <asp:Label runat="server" ID="labelTipo" Text="tipo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onKeyUpBusqueda();"></asp:TextBox>&nbsp;&nbsp;                                
                                            <asp:CheckBox runat="server" ID="chbMostrarObsoletos" Text="Mostrar obsoletos" AutoPostBack="true" />
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
                                            <asp:Button runat="server" id="btnNuevo" Text="nuevo" OnClick="btnVerDetalle" CommandName="nuevo"></asp:Button>
                                        </td>
                                    </tr>                        
                                 </table> 
                                 </asp:Panel>
                               </ContentTemplate>
                            </cc1:TabPanel>
                            
                             <cc1:TabPanel ID="tabP2" runat="server" HeaderText="movil">
                                <ContentTemplate>
                                     <asp:Panel ID="pnlBusqueda2" runat="server" DefaultButton="btnVerDatosM">
                                     <table width="50%">
                                        <tr>
                                            <td width="10%">
                                                <asp:Label runat="server" ID="labelTipo2" Text="tipo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBuscar2" onkeyup="javascript:onKeyUpBusqueda2();"></asp:TextBox>
                                            </td>                        
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                  <asp:ListBox runat="server" ID="lbLista2" Rows="6" width="100%"></asp:ListBox>        
                                            </td>                        
                                        </tr> 
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr> 
                                        <tr>
                                           <td>&nbsp;</td>
                                           <td>
                                                <asp:Button runat="server" ID="btnVerDatosM" Text="verDatos" OnClick="btnVerDetalle" CommandName="modificarM" /> &nbsp;    
                                                <asp:Button runat="server" id="btnNuevoM" Text="nuevo" OnClick="btnVerDetalle" CommandName="nuevoM" />
                                            </td>
                                        </tr>                        
                                     </table> 
                                     </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                       </cc1:TabContainer>
                    </asp:View>
            
             <asp:View runat="server" ID="vDetalle">             
                 <fieldset style="width:70%">
                    <br />
                    <table>
                        <tr>
                            <td>
                                 <asp:Label ID="labelTipoExten" runat="server" Text="tipoExtensionAsociada"></asp:Label>
                                 <asp:DropDownList runat="server" ID="ddlExtension" AppendDataBoundItems="true"></asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkRequiereAlv" Text="requiereAlveolo" />
                            </td>
                            <td>&nbsp;</td>
                            <asp:Panel runat="server" ID="pnlObsoleto">
                             <td>
                               <asp:CheckBox runat="server" ID="chbObsoleto" Text="Obsoleto" />                              
                             </td>
                             </asp:Panel>  
                        </tr>
                    </table>                                    
                    <br />                   
                    <table width="100%">                                             
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gvTipoLinea" AutoGenerateColumns="false" CssClass="GridView" width="90%"> 
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />   
                                    <PagerStyle HorizontalAlign="Center"/>
                                    <PagerSettings PageButtonCount="5" />
                                    <Columns> 
                                        <asp:BoundField DataField="Id" Visible="false" />
                                         <asp:TemplateField >
                                               <HeaderTemplate>   
                                                    <asp:Label runat="server" Text="cultura"></asp:Label>                                                
                                               </HeaderTemplate>
                                              <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCultura" Text='<%#Eval("Cultura")%>'></asp:Label>                                                                                                            
                                              </ItemTemplate>
                                         </asp:TemplateField> 
                                          <asp:TemplateField HeaderStyle-Width="80%">
                                               <HeaderTemplate>
                                                    <asp:Label runat="server" Text="tipo"></asp:Label>
                                               </HeaderTemplate>
                                              <ItemTemplate>
                                                    <asp:Textbox ID="txtNombre" runat="server" Text='<%#Eval("Nombre")%>'></asp:Textbox>                                                                                                            
                                              </ItemTemplate>
                                         </asp:TemplateField>                                          
                                    </Columns>
                               </asp:GridView> 
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" Visible="false" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnVolver" Text="volver" />
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
                <asp:ListBox runat="server" ID="lbAuxiliar2"></asp:ListBox>    
            </div> 
            </ContentTemplate>       
        </asp:UpdatePanel>                
</asp:Content>
