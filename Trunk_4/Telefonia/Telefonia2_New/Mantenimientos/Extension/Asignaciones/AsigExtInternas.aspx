<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="AsigExtInternas.aspx.vb" Inherits="Telefonia.AsigExtInternas"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">    
    <script src="../../../js/Utiles.js" type="text/javascript"></script>
    
    <script language="javascript">
        function seleccionarItem()
        {            
            var lista=document.getElementById('<%=lbLista.ClientId%>');
            document.getElementById('<%=txtBuscar.ClientId%>').value=lista[lista.selectedIndex].text;            
        }   
        
        function dobleClickExt()
        {
          var btnVerDatos=document.getElementById('<%=btnVerDatos.ClientId%>').name;                              
           __doPostBack(btnVerDatos,'');
        }   
        
        function seleccionarPerso()
        {            
            var lista=document.getElementById('<%=listaPerso.ClientId%>');
            document.getElementById('<%=txtpersona.ClientId%>').value=lista[lista.selectedIndex].text;            
        } 
        
        function dobleClickPerso()
        {
          var btnAsignarP=document.getElementById('<%=btnAsignarPerso.ClientId%>').name;                              
           __doPostBack(btnAsignarP,'');
        }     
        
        function seleccionarDep()
        {            
            var lista=document.getElementById('<%=listaDep.ClientId%>');
            document.getElementById('<%=txtdepartamento.ClientId%>').value=lista[lista.selectedIndex].text;            
        }  
        
         function dobleClickDep()
        {
          var btnAsignarD=document.getElementById('<%=btnAsignarDep.ClientId%>').name;                              
           __doPostBack(btnAsignarD,'');
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

        function onKeyUpBusquedaPerso() {
            var listaAux = document.getElementById('<%=lbAuxiliar2.ClientId%>')
            var lista = document.getElementById('<%=listaPerso.ClientId%>')
            var patron = document.getElementById('<%=txtPersona.ClientId%>').value; 
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
        
        function onKeyUpBusquedaDep()
        {
            var listaAux = document.getElementById('<%=lbAuxiliar3.ClientId%>')
            var lista = document.getElementById('<%=listaDep.ClientId%>')
            var patron = document.getElementById('<%=txtDepartamento.ClientId%>').value;            
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
                <asp:MultiView ID="mvExtensiones" runat="server" ActiveViewIndex="0">  
                                               
                   <asp:View ID="vListado" runat="server">
                      <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnVerDatos">              
                     <table width="25%">
                        <tr>
                            <td width="10%">
                                <asp:Label runat="server" ID="labelExt" text="extension"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onKeyUpBusqueda();"></asp:TextBox>
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
                            </td>
                        </tr>                        
                     </table> 
                    </asp:Panel>
                </asp:View>
                
                <asp:View runat="server" ID="vDetalle"> 
                  <fieldset style="width:900px">
                    <div>
                        <asp:Label runat="server" id="labelExt2" text="extension"></asp:Label>&nbsp;
                        <asp:Label Id="lblExten" runat="server" CssClass="negrita"></asp:Label>
                        <asp:HiddenField ID="hfIdTlfno" runat="server" />
                    </div>
                    <br />
                    <div>
                        <asp:Label runat="server" ID="labelNombre" text="nombre"></asp:Label>:&nbsp;
                         <asp:Label Id="lblNombreExt" runat="server" CssClass="negrita"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label runat="server" ID="labelTipo" text="tipo"></asp:Label>:&nbsp;
                         <asp:Label Id="lblTipo" runat="server" CssClass="negrita"></asp:Label>&nbsp;&nbsp;
                    </div>
                   </fieldset>
                    <br /><br />
                    <fieldset style="width:900px">
                    <table>
                        <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="labelAsigA" text="asignadaA"></asp:Label>:        
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <table cellpading="5px" cellspacing="5px">                                    
                                    <asp:Repeater runat="server" ID="rptUsuDep">
                                         <ItemTemplate>
                                            <tr>                                       
                                                <td>
                                                    <asp:Image ID="imgUsuDep" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" id="lblUsuDep"></asp:Label>
                                                </td>
                                                <td>
                                                    (<asp:Label runat="server" ID="lblFAsig" CssClass="negrita"></asp:Label>)
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkDesasignar" text="desasignar" OnClick="lnkDesasignar_Click"></asp:LinkButton>
                                                </td>                                                                                     
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                               </table> 
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <br /><br />
                    <fieldset style="width:900px">
                    <table cellspacing="10px">
                        <tr>
                            <td>
                               <asp:Panel ID="pnlAsignarPerso" runat="server" DefaultButton="btnAsignarPerso"> 
                                <asp:Label runat="server" ID="labelAsigAPerso" text="asignarAPersona"></asp:Label>(
                                <asp:Image runat="server" Imageurl="~/App_Themes/Tema1/Images/persona.gif" />)
                                <table>
                                    <tr>                                        
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtPersona" onkeyup="javascript:onKeyUpBusquedaPerso();" Width="200px"></asp:TextBox>
                                        </td>                        
                                    </tr>
                                    <tr>                                        
                                        <td colspan="2">
                                              <asp:ListBox runat="server" ID="listaPerso" Rows="8" CssClass="font11" Width="200px"></asp:ListBox>        
                                        </td>                        
                                    </tr> 
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr> 
                                    <tr>
                                       <td>&nbsp;</td>
                                       <td>
                                            <asp:Button runat="server" ID="btnAsignarPerso" text="asignar" OnClick="btnAsignar" /> &nbsp;                                    
                                        </td>
                                    </tr>                        
                                 </table> 
                                 </asp:Panel>
                            </td>
                            <td>&nbsp;</td>
                            <asp:Panel runat="server" ID="pnlDepartamento">
                             <td>
                                <asp:Panel ID="pnlAsignarDep" runat="server" DefaultButton="btnAsignarDep"> 
                                <asp:Label runat="server" ID="labelAsigADepart" text="asignarADepartamento"></asp:Label>(
                                <asp:Image ID="Image1" runat="server" Imageurl="~/App_Themes/Tema1/Images/departamento.gif" />)
                                <table>
                                    <tr>                                        
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtDepartamento" onkeyup="javascript:onKeyUpBusquedaDep();" Width="200px"></asp:TextBox>
                                        </td>                        
                                    </tr>
                                    <tr>                       
                                        <td colspan="2">
                                              <asp:ListBox runat="server" ID="listaDep" Rows="8" CssClass="font11" Width="200px"></asp:ListBox>        
                                        </td>                        
                                    </tr> 
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr> 
                                    <tr>
                                       <td>&nbsp;</td>
                                       <td>
                                            <asp:Button runat="server" ID="btnAsignarDep" text="asignar" OnClick="btnAsignar" CommandName="D" /> &nbsp;                                    
                                        </td>
                                    </tr>                        
                                 </table> 
                                 </asp:Panel>
                               </td>
                            </asp:Panel>
                        </tr>
                    </table>
                    </fieldset>
                    <asp:Panel ID="pnlHistorico" runat="server">
                       <br /><br />
                       <fieldset style="width:900px">
                        <asp:Label runat="server" ID="labelHistorico" text="historicoAsignaciones" CssClass="negrita"></asp:Label> 
                        <br /><br />                           
                           <asp:GridView runat="server" ID="gvHistorico" AutoGenerateColumns="false" CssClass="GridView" DatatextNames="FechaDesde,FechaHasta" > 
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />   
                                <Columns> 
                                     <asp:TemplateField >
                                           <HeaderTemplate>   
                                                <asp:Label runat="server" text="Asignado a" />  
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAsignado"></asp:Label>                                                                                                            
                                          </ItemTemplate>
                                     </asp:TemplateField> 
                                      <asp:TemplateField HeaderStyle-Width="25%">
                                           <HeaderTemplate>
                                                <asp:Label runat="server" text="Fecha desde" />
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFechaDesde"></asp:Label>                                                                                                            
                                          </ItemTemplate>
                                     </asp:TemplateField>   
                                      <asp:TemplateField HeaderStyle-Width="25%">
                                           <HeaderTemplate>
                                                <asp:Label runat="server" text="Fecha hasta" />
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFechaHasta"></asp:Label>                                                                                                            
                                          </ItemTemplate>
                                     </asp:TemplateField>                                         
                                </Columns>
                           </asp:GridView> 
                        </fieldset>   
                     </asp:Panel>
                    <br /><br />
                    <div>
                        <asp:Button runat="server" ID="btnVolver" text="volver" />
                    </div>                                       
                </asp:View>                
            </asp:MultiView>
            <div style="visibility:hidden;">
                <asp:ListBox runat="server" ID="lbAuxiliar"></asp:ListBox>  
                <asp:ListBox runat="server" ID="lbAuxiliar2"></asp:ListBox>     
                <asp:ListBox runat="server" ID="lbAuxiliar3"></asp:ListBox>        
            </div>                     
        </ContentTemplate>       
    </asp:UpdatePanel> 
    
     
    
   
</asp:Content>
