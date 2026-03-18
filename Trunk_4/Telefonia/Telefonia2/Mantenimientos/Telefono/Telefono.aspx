<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master" CodeBehind="Telefono.aspx.vb" Inherits="Telefonia.Telefono" %>
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
          var btnVerDatos=document.getElementById('<%=btnModificar.ClientId%>').name;                              
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
                 <asp:MultiView ID="mvTelefonos" runat="server" ActiveViewIndex="0">    
                    <asp:View runat="server" ID="vListado">   
                         <tit:Titulo ID="titulo" runat="server" Texto="listadoTelefonos" />                                            
                         <br />
                        <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnModificar"> 
                         <table width="25%">
                            <tr>
                                <td width="10%">
                                     <asp:Label runat="server" ID="labelTlfno" text="telefono"></asp:Label>
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
                                    <asp:Button runat="server" ID="btnModificar" text="verDatos" OnClick="btnVerDetalle" CommandName="modificar" /> &nbsp;    
                                    <asp:Button runat="server" id="btnNuevo" text="nuevo" OnClick="btnVerDetalle" CommandName="nuevo" />
                                </td>
                            </tr>                      
                         </table> 
                        </asp:Panel> 
                         <div style="visibility:hidden;">
                            <asp:ListBox runat="server" ID="lbAuxiliar"></asp:ListBox>     
                        </div>     
                    </asp:View>
                     <asp:View runat="server" ID="vDetalle"> 
                            <fieldset>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <tit:Titulo runat="server" ID="titPopPup"></tit:Titulo>  
                                        </td>
                                    </tr>                                        
                                    <tr>
                                        <td>
                                             <asp:Label runat="server" ID="labelTlfno2" text="telefono"></asp:Label>    
                                        </td>
                                        <td>
                                            <asp:Panel runat="server" ID="pnlExistente">
                                                <asp:label runat="server" ID="lblNumero" CssClass="negrita"></asp:Label>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlNuevo">
                                                <asp:TextBox runat="server" ID="txtNumero"></asp:TextBox>                                            
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                             <asp:Label runat="server" ID="labelEmp" text="empresa"></asp:Label>    
                                        </td>
                                        <td>                                        
                                            <asp:DropDownList runat="server" ID="ddlCiaTlfno" AppendDataBoundItems="true"></asp:DropDownList>
                                        </td>
                                    </tr> 
                                     <tr>
                                        <td>
                                             <asp:Label  runat="server" ID="labeltlfno3" text="telefono"></asp:Label>    
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlTelefono" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                    </tr>  
                                    <asp:Panel runat="server" ID="pnlTlfno">
                                     <tr>
                                        <td>
                                             <asp:Label  runat="server" ID="labelTipo" text="tipo"></asp:Label>    
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlTipoFijo"></asp:DropDownList>
                                        </td>
                                    </tr>                                     
                                    </asp:Panel>                                       
                                    <asp:Panel runat="server" ID="pnlMovil">
                                        <tr>
                                            <td>
                                                 <asp:Label  runat="server" ID="labelModelo" text="modelo"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtModelo"></asp:TextBox>  
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                 <asp:Label  runat="server" ID="labelPin" text="pin"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPin"></asp:TextBox>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <asp:Label runat="server" ID="labelPuk" text="puk"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPuk"></asp:TextBox>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <asp:Label runat="server" ID="labelDual" text="dualizado"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chbDualizado" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                 <asp:Label runat="server" ID="labelRoaming" text="roaming"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chbRoaming" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <asp:Label runat="server" ID="labelPerfilMov" text="Perfil movil"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:Dropdownlist runat="server" ID="ddlPerfilMov" AppendDataBoundItems="true"></asp:Dropdownlist>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <asp:Label runat="server" ID="labelTarifa" text="Tarifa datos"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:Dropdownlist runat="server" ID="ddlTarifas" AppendDataBoundItems="true"></asp:Dropdownlist>  
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                 <asp:Label runat="server" ID="labelTipo2" text="tipo"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlTipo"></asp:DropDownList>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                 <asp:Label runat="server" ID="labelGestor" text="gestor"></asp:Label>    
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlGestores" AppendDataBoundItems="true"></asp:DropDownList>
                                            </td>
                                        </tr>                                        
                                    </asp:Panel> 
                                    <asp:Panel ID="pnlComun" runat="server">
                                     <tr>
                                        <td colspan="2">
                                             <asp:Label runat="server" ID="labelComen" text="comentarios"></asp:Label>    
                                        </td>                                        
                                        </tr>  
                                        <tr>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtComentarios" Columns="35" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                            </td>                                        
                                        </tr>  
                                    </asp:Panel>
                                    <asp:Panel ID="pnlObsoleto" runat="server">
                                     <td colspan="2">
                                        <asp:CheckBox runat="server" ID="chkObsoleto" text="obsoleto" TextAlign="Left" />                                        
                                    </td>
                                    </asp:Panel>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr> 
                                        <td colspan="2" align="center">
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
        </ContentTemplate>       
    </asp:UpdatePanel> 
    
     
</asp:Content>
