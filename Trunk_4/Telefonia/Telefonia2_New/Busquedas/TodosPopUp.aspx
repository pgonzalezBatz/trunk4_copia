<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TodosPopUp.aspx.vb" Inherits="Telefonia.TodosPopUp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Imprimir</title>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div>
        <asp:Panel runat="server" ID="pnlPersona">  
           <asp:Panel runat="server" ID="pnlOtrasPlantasPersonas">                                            
             <table width="100%"  class="listadoRepeater11" cellpadding="0" cellspacing="0">                                    
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
                            <th align="center"><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/zoiper.png"/></th>
                        </tr>
                       </thead>
                    </HeaderTemplate>
                     <ItemTemplate>
                        <tr runat="server" id="tr">      
                            <td runat="server" id="td"></td>                                 
                            <td align="left" valign="top" nowrap>&nbsp;<asp:Label runat="server" ID="lblNombre"></asp:Label></td>
                            <td align="left" valign="top" nowrap>&nbsp;<asp:Label runat="server" ID="lblDepartamento"></asp:Label></td>
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
                                <asp:Literal  runat="server" text="nombre"></asp:Literal>
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
                    <tr>   
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
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlDepartamento">
         <asp:Panel runat="server" ID="pnlOtrasPlantasDept">
            <table width="100%" class="listadoRepeater11" cellpadding="0" cellspacing="0">                                    
                <asp:Repeater runat="server" ID="rptDepartamentos" EnableViewState="false">                                                
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
                            <th align="center"><asp:Image runat="server" tooltip="Zoiper" ImageUrl="~/App_Themes/Tema1/Images/zoiper.png"/></th>
                        </tr>
                       </thead>
                    </HeaderTemplate>
                     <ItemTemplate>
                        <tr runat="server" id="tr">                                                                              
                            <td align="left" valign="top" nowrap>&nbsp;<asp:Label ID="lblDepartamento" runat="server"></asp:Label></td>
                            <td align="left" valign="top" nowrap>&nbsp;<asp:Label ID="lblNombre" runat="server"></asp:Label></td>
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
                                <th><asp:Literal runat="server" text="Nº directo"></asp:Literal></th>
                                <th align="center"><asp:Image runat="server" tooltip="Extension inalambrica" ImageUrl="~/App_Themes/Tema1/Images/wireless.png"/></th>
                                <th><asp:Literal  runat="server" text="Nº directo"></asp:Literal></th>
                                <th align="center"><asp:Image runat="server" tooltip="Extension movil" ImageUrl="~/App_Themes/Tema1/Images/mobile.png"/></th>
                                <th><asp:Literal runat="server" text="Movil"></asp:Literal></th>
                                <th align="center"><asp:Image runat="server" tooltip="Skype" ImageUrl="~/App_Themes/Tema1/Images/skype.png"/></th>
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
    </asp:Panel>                              
   </div>
   <asp:HiddenField runat="server" ID="hfPrefijo" />
 </form>
</body>
</html>
