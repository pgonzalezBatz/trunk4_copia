<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPTelefonia.Master" CodeBehind="AltasBajas.aspx.vb" Inherits="Telefonia.AltasBajas" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MPTelefonia.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script language="javascript" type="text/javascript">
       var ModalProgress ='<%= ModalProgress.ClientID %>';     
     </script>        
    <asp:Label runat="server" ID="labelTitulo" text="Altas / Bajas" CssClass="Titulo"></asp:Label><br /> <br />
    <fieldset style="width:750px">
        <asp:Label runat="server" ID="labelInfo" text="Al sincronizar se obtendran todos los usuarios dados de alta o de baja desde una fecha especificada" ></asp:Label><br /><br />
        <asp:Label runat="server" ID="labelFInicio" text="Fecha de inicio de sincronizacion" ></asp:Label>&nbsp;
        <asp:Label runat="server" id="lblFechaSincro" CssClass="negrita"></asp:Label><br /><br />       
        <div>
            <asp:Button runat="server" text="sincronizar" ID="btnSincronizar" style="margin-left:80px;" />
            <asp:Button runat="server" text="Fijar fecha" ID="btnFijarFecha" style="margin-left:30px;" />&nbsp;<span class="font11">(*)</span>            
        </div><br />
        <asp:CheckBox runat="server" ID="chkMostrarBajasSinExt" text="Mostrar bajas sin extension ni nº de telefono asociado" /><br />
        <span class="font11">(*)</span><asp:Label runat="server" ID="labelInfo2" text="Guarda la fecha para que la proxima sincronizacion, se sincronice a partir de esta" CssClass="font11"></asp:Label>
    </fieldset><br /> <br />
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
           <asp:Panel runat="server" ID="pnlImprimir">
                <asp:ImageButton runat="server" ID="imgImprimir" text="Imprimir" OnClientClick="window.print();return false;" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/imprimir.gif" style="margin-left:15px;" />
                <asp:Label runat="server" ID="labelImprimir" text="Imprimir" CssClass="negrita"></asp:Label>
           </asp:Panel>
            <table cellpadding="3px" cellspacing="0px" border="0px" width="750px" class="listadoRepeater">
                <asp:Repeater runat="server" ID="rptUsuarios">
                    <HeaderTemplate>
                        <tr>
                            <th><asp:Label runat="server" text="usuario"></asp:Label></th>
                            <th><asp:Label runat="server" text="Informacion"></asp:Label></th>
                            <th><asp:Label runat="server" text="accion"></asp:Label></th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr runat="server" id="myTr">
                            <td align="left" style="border-width:0px;"><asp:Label runat="server" ID="lblUsuario"></asp:Label></td>
                            <td align="left" style="border-width:0px;"><asp:Label runat="server" ID="lblInformacion"></asp:Label></td>
                            <td align="center" style="border-width:0px;">
                                <asp:Label runat="server" ID="lblAccion" CssClass="mayusculas"></asp:Label>
                                <asp:Panel runat="server" ID="pnlCambiar"><asp:Button runat="server" ID="btnCambiar" text="cambiar" OnClick="CambiarIdSab" /></asp:Panel>
                            </td>
                        </tr>
                    </ItemTemplate>                                                  
                </asp:Repeater>
            </table>
        </ContentTemplate>
        <Triggers><asp:AsyncPostBackTrigger ControlID="btnSincronizar" EventName="Click" /></Triggers>
    </asp:UpdatePanel>    
     <asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress">
       <asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server" >
        <ProgressTemplate>
          <div style="position: relative; top: 30%; text-align:center;">    
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/Images/loadin.gif" />                            
            <asp:Label ID="lblFiltrando" runat="server" text="sincronizandoDatos"></asp:Label>   
          </div>
        </ProgressTemplate>
      </asp:UpdateProgress>
     </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground2" PopupControlID="panelUpdateProgress" />  
</asp:Content>
