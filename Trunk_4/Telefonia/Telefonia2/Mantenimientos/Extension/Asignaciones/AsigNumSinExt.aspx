<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master" CodeBehind="AsigNumSinExt.aspx.vb" Inherits="Telefonia.AsigNumSinExt"  %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">    
    <script src="../../../js/Utiles.js" type="text/javascript"></script>
    
    <script language="javascript">
        function seleccionarItem()
        {            
            var lista=document.getElementById('<%=lbLista.ClientId%>');
            document.getElementById('<%=txtBuscar.ClientId%>').value=lista[lista.selectedIndex].text;            
        }   
        
        function dobleClickTlfno()
        {
          var btnVerDatosT=document.getElementById('<%=btnVerDatos.ClientId%>').name;                              
           __doPostBack(btnVerDatosT,'');
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
        
        function onKeyUpBusquedaPerso()
        {
            var listaAux=document.getElementById('ctl00$cp$lbAuxiliar2');            
            var lista=document.getElementById('ctl00$cp$listaPerso');                      
            var patron=document.getElementById('ctl00$cp$txtpersona').value;            
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
                <tit:Titulo runat="server" Texto="asignacionTelefonosSinExtension" />                                            
                <asp:MultiView ID="mvTelefonos" runat="server" ActiveViewIndex="0">  
                                               
                   <asp:View ID="vListado" runat="server">
                     <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnVerDatos">                  
                     <table width="25%">
                        <tr>
                            <td width="10%">
                                 <asp:Label runat="server" ID="labelTlfno" text="telefono"></asp:Label>
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
                  <fieldset style="width:600px">
                    <div>
                         <asp:Label runat="server" ID="labelTlfno2" text="telefono"></asp:Label>&nbsp;
                        <asp:Label Id="lblTlfno" runat="server" CssClass="negrita"></asp:Label>
                    </div>
                    <br />
                    <div>                        
                         <asp:Label runat="server" ID="labelCia" text="cia"></asp:Label>:&nbsp;
                         <asp:Label Id="lblCia" runat="server" CssClass="negrita"></asp:Label>&nbsp;&nbsp;
                    </div>
                   </fieldset>
                    <br /><br />
                    <fieldset style="width:600px">
                    <table>
                        <tr>
                            <td valign="top">
                                 <asp:Label runat="server" ID="labelAsigA" text="asignadaA"></asp:Label>:        
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <table cellpading="5px" cellspacing="5px">                                    
                                    <asp:Repeater runat="server" ID="rptUsu">
                                         <ItemTemplate>
                                            <tr>                                       
                                                <td>
                                                    <asp:Image ID="imgUsu" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" id="lblUsu"></asp:Label>
                                                </td>
                                                <td>&nbsp;&nbsp;</td>
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
                    <fieldset style="width:600px">
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
                                            <asp:Button runat="server" ID="btnAsignarPerso" text="asignar" OnClick="btnAsignar" CommandName="P" /> &nbsp;                                    
                                        </td>
                                    </tr>                        
                                 </table> 
                               </asp:Panel>
                            </td>                          
                        </tr>
                    </table>
                    </fieldset>
                     <asp:Panel ID="pnlHistorico" runat="server">
                       <br /><br />
                       <fieldset style="width:600px">
                         <asp:Label runat="server" ID="labelHistorico" text="historicoAsignaciones" CssClass="negrita"></asp:Label> 
                        <br /><br />
                          <asp:GridView runat="server" ID="gvHistorico" AutoGenerateColumns="false" CssClass="GridView" DatatextNames="FechaDesde,FechaHasta" > 
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />   
                                <Columns> 
                                     <asp:TemplateField >
                                           <HeaderTemplate>   
                                                 <asp:Label runat="server" text="persona"></asp:Label>                                                
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NombreUsuario")%>'></asp:Label>                                                                                                            
                                          </ItemTemplate>
                                     </asp:TemplateField> 
                                      <asp:TemplateField HeaderStyle-Width="25%">
                                           <HeaderTemplate>
                                                 <asp:Label runat="server" text="Fecha desde"></asp:Label>
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#FormatFecha(Eval("FechaDesde"))%>'></asp:Label>                                                                                                            
                                          </ItemTemplate>
                                     </asp:TemplateField>   
                                      <asp:TemplateField HeaderStyle-Width="25%">
                                           <HeaderTemplate>
                                                 <asp:Label runat="server" text="Fecha hasta"></asp:Label>
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#FormatFecha(Eval("FechaHasta"))%>'></asp:Label>                                                                                                            
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
