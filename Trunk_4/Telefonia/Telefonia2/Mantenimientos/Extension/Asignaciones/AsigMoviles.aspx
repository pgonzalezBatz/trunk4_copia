<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master" CodeBehind="AsigMoviles.aspx.vb" Inherits="Telefonia.AsigMoviles" EnableEventValidation="false" %>
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

        function exportarExcel() {
            var movil = document.getElementById('<%=txtMovil.ClientId%>');
            var estados=document.getElementById('<%=ddlEstados.ClientId%>');
            window.open('ExportExcelMovil.aspx?mov=' + movil.value + '&est=' + estados.options[estados.selectedIndex].value, 'Listado', 'width=1,height=1');
        }                                                                           
                
    </script>            
       <asp:UpdatePanel runat="server">
           <ContentTemplate>
                <tit:Titulo runat="server" Texto="asignacionMovilesLibres" />                                            
                <asp:MultiView ID="mvMoviles" runat="server" ActiveViewIndex="0">  
                                               
                   <asp:View ID="vListado" runat="server">    
                     <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">                                                  
                    <fieldset style="width:60%">
                     <div>                                          
                          <asp:Label runat="server" ID="labelMovil" text="movil"></asp:Label>&nbsp;
                          <asp:TextBox runat="server" ID="txtMovil"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                          <asp:DropDownList runat="server" ID="ddlEstados"></asp:DropDownList>&nbsp;&nbsp;&nbsp;                          
                          <asp:Button runat="server" ID="btnBuscar" text="Buscar" />
                          <asp:Button runat="server" ID="btnExportarExcel" Text="Exportar a excel" style="margin-left:15px;" />
                     </div>                     
                   </fieldset>
                   </asp:Panel>
                   <div>                       
                      <br />
                      <asp:GridView runat="server" ID="gvMoviles" AutoGenerateColumns="false" CssClass="GridView" Width="60%" AllowSorting="true" allowPaging="false" PageSize="30" PagerSettings-Mode="NumericFirstLast">  
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />   
                            <PagerStyle HorizontalAlign="Center"/>
                            <PagerSettings PageButtonCount="5" />
                            <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                 <br />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label runat="server" key="noExisteNingunRegistro"></asp:Label>                 
                            </EmptyDataTemplate>
                            <Columns>                                                                                         
                                 <asp:TemplateField HeaderStyle-Width="15%" SortExpression="numero" ItemStyle-HorizontalAlign="Center">
                                      <HeaderTemplate>   
                                            <asp:LinkButton runat="server" text="movil" CommandName="Sort" CommandArgument="numero"></asp:LinkButton>
                                      </HeaderTemplate>
                                      <ItemTemplate>
                                            <asp:Label runat="server" id="lblNumero"></asp:Label>                                                                                                            
                                      </ItemTemplate>
                                 </asp:TemplateField> 
                                  <asp:TemplateField HeaderStyle-Width="15%" SortExpression="extension" ItemStyle-HorizontalAlign="Center">
                                       <HeaderTemplate>
                                            <asp:LinkButton runat="server" text="extension" CommandName="Sort" CommandArgument="extension"></asp:LinkButton>                                            
                                       </HeaderTemplate>
                                      <ItemTemplate>
                                            <asp:Label runat="server" id="lblExtension" Width="75%"></asp:Label>                                                                                                            
                                      </ItemTemplate>
                                 </asp:TemplateField>   
                                  <asp:TemplateField HeaderStyle-Width="40%" ItemStyle-CssClass="noWrap" SortExpression="nombre">
                                       <HeaderTemplate>
                                            <asp:LinkButton runat="server" text="asignadaA" CommandName="Sort" CommandArgument="nombre"></asp:LinkButton>                                                                                        
                                       </HeaderTemplate>
                                      <ItemTemplate>
                                            <asp:Label runat="server" id="lblNombre" Width="75%"></asp:Label>                                                                                                            
                                      </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField visible="false" ItemStyle-CssClass="noWrap">                                       
                                      <ItemTemplate>
                                            <asp:Label runat="server" id="lblDepartamento" Width="75%"></asp:Label>                                                                                                            
                                      </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-CssClass="noWrap" ItemStyle-HorizontalAlign="Center">
                                       <HeaderTemplate>
                                            <asp:Label runat="server" text="Fecha desde"></asp:Label>                                                                                        
                                       </HeaderTemplate>
                                      <ItemTemplate>
                                            <asp:Label runat="server" id="lblFechaDesde"></asp:Label>                                                                                                            
                                      </ItemTemplate>
                                 </asp:TemplateField>                                           
                            </Columns>
                       </asp:GridView> 
                    </div>                        
                </asp:View>
                
                <asp:View runat="server" ID="vDetalle"> 
                  <fieldset style="width:600px">
                    <div>
                        <asp:Label runat="server" ID="labelMovil2" text="movil"></asp:Label>&nbsp;
                        <asp:Label Id="lblMovil" runat="server" CssClass="negrita"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label runat="server" ID="labelExt" text="extension"></asp:Label>&nbsp;
                        <asp:Label Id="lblExtension" runat="server" CssClass="negrita"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:HiddenField ID="hfIdTelefono" runat="server" />
                    </div>                   
                   </fieldset>
                    <br /><br />                     
                    <fieldset style="width:600px">
                       <asp:Label runat="server" ID="labelAsigA" text="asignadaA"></asp:Label>:  
                       <asp:Label ID="lblUsuarioAsignado" runat="server" CssClass="negrita"></asp:Label>&nbsp;&nbsp;
                       <asp:Panel ID="pnlDevolver" runat="server">
                           <br />
                           <asp:Label runat="server" ID="labelFechaFin" text="fechaFin"></asp:Label>&nbsp;
                           <asp:TextBox runat="server" ID="txtFechaFin" Width="80px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                           <cc1:CalendarExtender ID="calFechaFin" runat="server" TargetControlID="txtFechaFin" EnabledOnClient="true"></cc1:CalendarExtender>
                           <asp:Button runat="server" ID="btnDevolver" text="devolver" />
                       </asp:Panel>
                   </fieldset>
                   
                    <asp:Panel runat="server" ID="pnlAsignar">
                        <br /><br />
                        <fieldset style="width:600px">
                            <table cellspacing="10px">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelAsigAPerso" text="asignarAPersona"></asp:Label>(
                                        <asp:Image runat="server" Imageurl="~/App_Themes/Tema1/Images/persona.gif" />)
                                        <table>
                                            <tr>                                        
                                                <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onKeyUpBusqueda();" Width="200px"></asp:TextBox>
                                                </td>                        
                                            </tr>
                                            <tr>                                        
                                                <td>
                                                      <asp:ListBox runat="server" ID="lbLista" Rows="8" CssClass="font11" Width="200px"></asp:ListBox>        
                                                </td>                       
                                                <td>
                                                   &nbsp;
                                                </td> 
                                                <td valign="top">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="labelFInicio" text="fechaInicio"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtFechaInicio" Width="80px"></asp:TextBox>                                                                    
                                                                <cc1:CalendarExtender ID="calFechaInicio" runat="server" TargetControlID="txtFechaInicio" EnabledOnClient="true"></cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                       <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:Button runat="server" ID="btnAsignar" text="asignar" />                                                                
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>                                                                       
                                         </table> 
                                    </td>                                     
                                </tr>                                
                            </table>
                        </fieldset>
                    </asp:Panel>                    
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
                                                <asp:Label ID="Label1" runat="server" text="Asignado a" />  
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%#ObtenerAsignadoA(Eval("NombreUsuario"),Eval("NombreOtros"))%>'></asp:Label>                                                                                                            
                                          </ItemTemplate>
                                     </asp:TemplateField> 
                                      <asp:TemplateField HeaderStyle-Width="25%">
                                           <HeaderTemplate>
                                                <asp:Label ID="Label3" runat="server" text="Fecha desde" />
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%#FormatFecha(Eval("FechaDesde"))%>'></asp:Label>                                                                                                            
                                          </ItemTemplate>
                                     </asp:TemplateField>   
                                      <asp:TemplateField HeaderStyle-Width="25%">
                                           <HeaderTemplate>
                                                <asp:Label ID="Label5" runat="server" text="Fecha hasta" />
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%#FormatFecha(Eval("FechaHasta"))%>'></asp:Label>                                                                                                            
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
             </div>                    
        </ContentTemplate>              
    </asp:UpdatePanel> 
    
      
    
   
</asp:Content>
