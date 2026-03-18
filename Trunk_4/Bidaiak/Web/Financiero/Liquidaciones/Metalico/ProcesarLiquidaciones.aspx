<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ProcesarLiquidaciones.aspx.vb" Inherits="WebRaiz.ProcesarLiquidaciones" %>
<%@ Register src="Controles/ListadoLiq.ascx" tagname="ListadoLiq" tagprefix="uc" %>
<%@ Register src="Controles/ResumenLiq.ascx" tagname="ResumenLiq" tagprefix="uc" %>
<%@ Register src="Controles/ProcesandoLiq.ascx" tagname="ProcesandoLiq" tagprefix="uc" %>
<%@ Register src="Controles/ResultadoLiq.ascx" tagname="ResultadoLiq" tagprefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">        
     <asp:Wizard ID="WLiquidaciones" runat="server" CssClass="Wizard2" Height="400px" Width="100%">
        <SideBarButtonStyle CssClass="SideBarButtonStyle2" />
        <SideBarStyle CssClass="SideBarStyle2" Width="200px" />
        <StepStyle CssClass="StepStyle2" VerticalAlign="Top" />            
        <HeaderStyle CssClass="WizardHeaderStyle2" />
         <NavigationStyle CssClass="NavigationStyle2" />
        <SideBarTemplate>
            <asp:Label runat="server" Text="Pasos del proceso" style="font-weight:bold;font-size:15px;text-transform:uppercase"></asp:Label><br /><br />
            <asp:DataList ID="SideBarList" runat="server" CellPadding="3" CellSpacing="3">
              <ItemTemplate>                
                 <asp:LinkButton ID="SideBarButton" runat="server" OnClientClick="return false;"></asp:LinkButton>
              </ItemTemplate>
              <SelectedItemStyle CssClass="SideBarSelected2"/>
            </asp:DataList>
       </SideBarTemplate>   
        <WizardSteps>            
            <asp:WizardStep ID="wStep1" runat="server" Title="Listar liquidaciones" StepType="Start">                       
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:ListadoLiq ID="ucListado_Step1" runat="server" />            
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="wStep2" runat="server" Title="Resumen">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:ResumenLiq ID="ucResumen_Step2" runat="server" />
                </div>
            </asp:WizardStep>
            <asp:WizardStep  ID="wStep3" runat="server" Title="Procesando">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:ProcesandoLiq ID="ucProcesando_Step3" runat="server" />
                 </div>
            </asp:WizardStep>            
             <asp:WizardStep ID="wStep4" runat="server" Title="Proceso completado" StepType="Finish">
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:ResultadoLiq ID="ucResul_Step4" runat="server" />
                </div>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
