<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="VerTodos.aspx.vb" Inherits="Telefonia.VerTodos"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">    
       <script src="../js/ajax.js" type="text/javascript"></script>
      <script language="javascript" type="text/javascript">
        var ModalProgress ='<%= ModalProgress.ClientID %>';        
                                
        function VerDepartamentos(idDep,dep)
        {
          window.open("DepartamentosPopUp.aspx?id=" + idDep + "&name=" + dep ,"Departamento","width=850,height=580");
        }
                          
      </script>
            
      <tit:Titulo runat="server" Texto="listaTelefonosCompleta" />                                                                                                                             
           <asp:UpdatePanel ID="upBusquedas" runat="server" UpdateMode="Conditional">
             <ContentTemplate>
                 <div>
                    <asp:Label runat="server" ID="labelSelPlanta" text="seleccionePlanta"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlPlanta" AutoPostBack="true"></asp:DropDownList>
                </div>  
                <br />
                <fieldset style="width:80%">
                   <table>
                    <tr>
                        <td valign="top"><asp:Label runat="server" ID="labelOrdenadoPor" text="ordenadaPor" CssClass="negrita"></asp:Label><b>:</b></td>
                        <td><asp:RadioButtonList runat="server" id="rblOrden" RepeatColumns="2" RepeatDirection="Horizontal" AutoPostBack="true"></asp:RadioButtonList> &nbsp;&nbsp;</td>
                        <td valign="top">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnImprimir" text="imprimir" /></td>
                    </tr>
                   </table>                                                                         
                  </fieldset>
                </ContentTemplate>
              </asp:UpdatePanel>       
            <br />                
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
              <asp:Panel runat="server" ID="pnlPersona">                    
                     <table border="1" cellpadding="0" cellspacing="0" bordercolor="gray" width="90%">
                       <tr>
                        <td>
                            <a name="Cabecera"></a>                        
                            <b>
                              <a href="#a" runat="server" id="hrA">A</a>-<a href="#b" runat="server" id="hrB">B</a>-
                              <a href="#c" runat="server" id="hrC">C</a>-<a href="#d" runat="server" id="hrD">D</a>-
                              <a href="#e" runat="server" id="hrE">E</a>-<a href="#f" runat="server" id="hrF">F</a>-
                              <a href="#g" runat="server" id="hrG">G</a>-<a href="#h" runat="server" id="hrH">H</a>-
                              <a href="#i" runat="server" id="hrI">I</a>-<a href="#j" runat="server" id="hrJ">J</a>-
                              <a href="#k" runat="server" id="hrK">K</a>-<a href="#l" runat="server" id="hrL">L</a>-
                              <a href="#m" runat="server" id="hrM">M</a>-<a href="#n" runat="server" id="hrN">N</a>-
                              <a href="#o" runat="server" id="hrO">O</a>-<a href="#p" runat="server" id="hrP">P</a>-
                              <a href="#q" runat="server" id="hrQ">Q</a>-<a href="#r" runat="server" id="hrR">R</a>-
                              <a href="#s" runat="server" id="hrS">S</a>-<a href="#t" runat="server" id="hrT">T</a>-
                              <a href="#u" runat="server" id="hrU">U</a>-<a href="#v" runat="server" id="hrV">V</a>-
                              <a href="#w" runat="server" id="hrW">W</a>-<a href="#x" runat="server" id="hrX">X</a>-
                              <a href="#y" runat="server" id="hrY">Y</a>-<a href="#z" runat="server" id="hrZ">Z</a>
                            </b>
                         </td>
                       </tr>
                       <tr>
                         <td>   
                            <asp:Panel runat="server" ID="pnlOtrasPlantasPersonas">                            
                                 <table width="100%" class="listadoRepeater2" cellpadding="0" cellspacing="0">                                    
                                    <asp:Repeater runat="server" ID="rptPersonas" EnableViewState="false">                                                
                                        <HeaderTemplate>   
                                          <thead>
                                            <tr>
                                                <th>&nbsp;</th>
                                                <th align="left">       
                                                    <img src="../App_Themes/Tema1/Images/persona_small.gif" />
                                                    <asp:Literal runat="server" text="nombre"></asp:Literal>
                                                </th>
                                                <th align="left">
                                                    <img src="../App_Themes/Tema1/Images/departamento.gif" />
                                                    <asp:Literal runat="server" text="departamento"></asp:Literal>
                                                </th>
                                                <th align="center"><asp:Image runat="server" tooltip="Extension fija" ImageUrl="~/App_Themes/Tema1/Images/telephone.png" /></th>
                                                <th><asp:Literal runat="server" text="Nº directo"></asp:Literal></th>
                                                <th align="center"><asp:Image runat="server" tooltip="Extension inalambrica" ImageUrl="~/App_Themes/Tema1/Images/wireless.png"/></th>
                                                <th><asp:Literal runat="server" text="Nº directo"></asp:Literal></th>
                                                <th align="center"><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/mobile.png"/></th>
                                                <th><asp:Literal runat="server" text="Movil"></asp:Literal></th>
                                                <th align="center"><asp:Image runat="server" tooltip="Zoiper" ImageUrl="~/App_Themes/Tema1/Images/zoiper.png"/></th>
                                            </tr>
                                           </thead>
                                        </HeaderTemplate>
                                         <ItemTemplate>
                                            <tr runat="server" id="tr">      
                                                <td runat="server" id="td"></td>                                 
                                                <td align="left">&nbsp;<asp:Label runat="server" ID="lblNombre"></asp:Label></td>
                                                <td align="left">&nbsp;<asp:LinkButton ID="lnkDepartamento" runat="server"></asp:LinkButton></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblExtFija"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblFijo"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblExtInalambrica"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblInalambrico"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblExtensionMovil" CssClass="textoAzul"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblTlfnoMovil" CssClass="textoAzul"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblZoiper" CssClass="textoAzul"></asp:Label></td>
                                            </tr>
                                        </ItemTemplate>                                                                                      
                                    </asp:Repeater>
                               </table> 
                          </asp:Panel>
                           <asp:Panel runat="server" ID="pnlMatriciPersonas">
                                 <table width="100%" class="listadoRepeater2" cellpadding="0" cellspacing="0">                                 
                                    <asp:Repeater runat="server" ID="rptPersonasMatrici">                                                
                                          <HeaderTemplate>
                                             <thead>
                                                <tr>
                                                    <th>&nbsp;</th>   
                                                    <th align="left">       
                                                        <img src="../App_Themes/Tema1/Images/persona_small.gif" />
                                                        <asp:Literal runat="server" text="nombre"></asp:Literal>
                                                    </th>
                                                    <th align="left">
                                                        <img src="../App_Themes/Tema1/Images/departamento.gif" />
                                                        <asp:Literal runat="server" text="departamento"></asp:Literal>
                                                    </th>
                                                    <th align="center"><asp:Image runat="server" tooltip="Extension fija" ImageUrl="~/App_Themes/Tema1/Images/telephone.png" /></th>
                                                    <th><asp:Literal runat="server" text="Nº fijo"></asp:Literal></th>
                                                    <th align="center"><asp:Image runat="server" tooltip="Extension inalambrica" ImageUrl="~/App_Themes/Tema1/Images/wireless.png"/></th>
                                                    <th><asp:Literal  runat="server" text="Nº inalambrico"></asp:Literal></th>
                                                    <th align="center"><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/mobile.png"/></th>
                                                    <th><asp:Literal runat="server" text="Movil"></asp:Literal></th>
                                                    <th align="center"><asp:Image runat="server" tooltip="Skype" ImageUrl="~/App_Themes/Tema1/Images/skype.png"/></th>
                                                </tr>
                                           </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="tr">   
                                                <td runat="server" id="td"></td>                                 
                                                <td align="left">&nbsp;<asp:Label runat="server" Text='<%#Eval("NombreCompleto")%>'></asp:Label></td>
                                                <td align="left">&nbsp;<asp:Label runat="server" Text='<%#Eval("Area")%>'></asp:Label></td>
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
                        </asp:Panel>
                       </td>
                    </tr>
                 </table>
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlDepartamento">
                  <table border="1" cellpadding="0" cellspacing="0" bordercolor="gray" width="90%">
                       <tr>
                         <td>  
                           <asp:Panel runat="server" ID="pnlOtrasPlantasDept">
                                 <table width="100%" class="listadoRepeater2" cellpadding="0" cellspacing="0" EnableViewState="false">                                    
                                    <asp:Repeater runat="server" ID="rptDepartamentos">                                                
                                        <HeaderTemplate>   
                                          <thead>
                                            <tr>                                            
                                                <th align="left">   
                                                    <img src="../App_Themes/Tema1/Images/departamento.gif" />
                                                    <asp:Literal runat="server" text="departamento"></asp:Literal>                                                    
                                                </th>
                                                <th align="left">
                                                    <img src="../App_Themes/Tema1/Images/persona_small.gif" />
                                                    <asp:Literal runat="server" text="nombre"></asp:Literal>
                                                </th>
                                               <th align="center"><asp:Image runat="server" tooltip="Extension fija" ImageUrl="~/App_Themes/Tema1/Images/telephone.png" /></th>
                                               <th><asp:Literal runat="server" text="Nº directo"></asp:Literal></th>
                                               <th align="center"><asp:Image runat="server" tooltip="Extension inalambrica" ImageUrl="~/App_Themes/Tema1/Images/wireless.png"/></th>
                                               <th><asp:Literal runat="server" text="Nº directo"></asp:Literal></th>
                                               <th align="center"><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/mobile.png"/></th>
                                               <th><asp:Literal runat="server" text="Movil"></asp:Literal></th>
                                               <th align="center"><asp:Image runat="server" tooltip="Zoiper" ImageUrl="~/App_Themes/Tema1/Images/zoiper.png"/></th>
                                            </tr>
                                           </thead>
                                        </HeaderTemplate>
                                         <ItemTemplate>
                                            <tr runat="server" id="tr">                                                                              
                                                <td align="left">&nbsp;<asp:LinkButton ID="lnkDepartamento" runat="server"></asp:LinkButton></td>
                                                <td align="left">&nbsp;<asp:Label ID="lblNombre" runat="server"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblExtFija"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblFijo"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblExtInalambrica"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblInalambrico"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblExtensionMovil" CssClass="textoAzul"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblTlfnoMovil" CssClass="textoAzul"></asp:Label></td>
                                                <td align="center">&nbsp;<asp:Label runat="server" ID="lblZoiper" CssClass="textoAzul"></asp:Label></td>
                                            </tr>
                                        </ItemTemplate>                                                                      
                                    </asp:Repeater>
                               </table> 
                          </asp:Panel>
                          <asp:Panel runat="server" ID="pnlDepartamentosMatrici">
                                 <table width="100%" class="listadoRepeater2" cellpadding="0" cellspacing="0">                                 
                                    <asp:Repeater runat="server" ID="rptDepartamentosMatrici">                                                
                                          <HeaderTemplate>
                                             <thead>
                                                <tr>                     
                                                    <th align="left">
                                                        <img src="../App_Themes/Tema1/Images/departamento.gif" />
                                                        <asp:Literal runat="server" text="departamento"></asp:Literal>
                                                    </th>
                                                    <th align="left">       
                                                        <img src="../App_Themes/Tema1/Images/persona_small.gif" />
                                                        <asp:Literal runat="server" text="nombre"></asp:Literal>
                                                    </th>                                                    
                                                    <th align="center"><asp:Image runat="server" tooltip="Extension fija" ImageUrl="~/App_Themes/Tema1/Images/telephone.png" /></th>
                                                    <th><asp:Literal runat="server" text="Nº fijo"></asp:Literal></th>
                                                    <th align="center"><asp:Image runat="server" tooltip="Extension inalambrica" ImageUrl="~/App_Themes/Tema1/Images/wireless.png"/></th>
                                                    <th><asp:Literal  runat="server" text="Nº inalambrico"></asp:Literal></th>
                                                    <th align="center"><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/mobile.png"/></th>
                                                    <th><asp:Literal runat="server" text="Movil"></asp:Literal></th>
                                                    <th><asp:Image runat="server" tooltip="Skype" ImageUrl="~/App_Themes/Tema1/Images/skype.png"/></th>
                                                </tr>
                                           </thead>
                                        </HeaderTemplate>
                                            <ItemTemplate>
                                            <tr runat="server" id="tr">                               
                                                <td align="left">&nbsp;<asp:Label runat="server" ID="lblArea"></asp:Label></td>
                                                <td align="left">&nbsp;<asp:Label runat="server" Text='<%#Eval("NombreCompleto")%>'></asp:Label></td>
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
                        </asp:Panel>
                       </td>
                    </tr>
                 </table>
            </asp:Panel>                                                                                            
            <asp:HiddenField runat="server" ID="hfPrefijo" />
         </ContentTemplate>   
    </asp:UpdatePanel> 

    <asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress">
       <asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server" >
        <ProgressTemplate>
          <div style="position: relative; top: 30%; text-align:center;">    
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/Images/loadin.gif" />                            
            <asp:Label ID="lblFiltrando" runat="server" text="cargandoDatos"></asp:Label>   
          </div>
        </ProgressTemplate>
      </asp:UpdateProgress>
     </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground2" PopupControlID="panelUpdateProgress" />         
</asp:Content>
