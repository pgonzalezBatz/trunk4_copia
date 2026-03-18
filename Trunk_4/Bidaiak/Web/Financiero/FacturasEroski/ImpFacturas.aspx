<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ImpFacturas.aspx.vb" Inherits="WebRaiz.ImpFacturas" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register src="Controles/ChequeoEjecucion.ascx" tagname="ChequeoEjec" tagprefix="uc" %>
<%@ Register src="Controles/SubirFactura.ascx" tagname="SubirFactura" tagprefix="uc" %>
<%@ Register src="Controles/IntegrarAlbaranes.ascx" tagname="IntegrarAlbaranes" tagprefix="uc" %>
<%@ Register src="Controles/ResumenFac.ascx" tagname="ResumenFac" tagprefix="uc" %>
<%@ Register src="Controles/PreAsientoCont.ascx" tagname="PreAsientoCont" tagprefix="uc" %>
<%@ Register src="Controles/FinalizarProc.ascx" tagname="FinalizarProc" tagprefix="uc" %>
<%@ Register src="Controles/ResumenFinalFac.ascx" tagname="ResumenFinal" tagprefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">                
    <asp:Wizard ID="wFactura" runat="server" CssClass="Wizard" Height="400px" Width="100%">
        <SideBarButtonStyle CssClass="SideBarButtonStyle" />
        <SideBarStyle CssClass="SideBarStyle" Width="200px" />
        <StepStyle CssClass="StepStyle" VerticalAlign="Top" />            
        <HeaderStyle CssClass="WizardHeaderStyle" />
        <SideBarTemplate>
            <asp:Label runat="server" Text="Pasos del proceso" style="font-weight:bold;font-size:15px;text-decoration:underline;"></asp:Label><br /><br />
            <asp:DataList ID="SideBarList" runat="server" CellPadding="3" CellSpacing="3">
              <ItemTemplate>                
                 <asp:LinkButton ID="SideBarButton" runat="server" OnClientClick="return false;"></asp:LinkButton>
              </ItemTemplate>
              <SelectedItemStyle CssClass="SideBarSelected"/>
            </asp:DataList>
       </SideBarTemplate>
       <HeaderTemplate>
           <asp:Label runat="server" ID="lblInfoAñoMes"></asp:Label>
           <asp:Label runat="server" id="lblTitulo"></asp:Label>
       </HeaderTemplate>     
        <WizardSteps>   
            <asp:WizardStep ID="wStep0" runat="server" Title="Chequeo ejecucion" StepType="Complete">                       
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:ChequeoEjec ID="ucStep0" runat="server" />            
                </div>
            </asp:WizardStep>         
            <asp:WizardStep ID="wStep1" runat="server" Title="Subir fichero mensual">                       
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:SubirFactura ID="ucStep1" runat="server" />            
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="wStep2" runat="server" Title="Integrar albaranes">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:IntegrarAlbaranes ID="ucStep2" runat="server" />
                </div>
            </asp:WizardStep>
            <asp:WizardStep  ID="wStep3" runat="server" Title="Resumen">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:ResumenFac ID="ucStep3" runat="server" />
                 </div>
            </asp:WizardStep>
            <asp:WizardStep  ID="wStep4" runat="server" Title="Previsualizar asiento contable">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:PreAsientoCont ID="ucStep4" runat="server" />
                 </div>
            </asp:WizardStep>
            <asp:WizardStep ID="wStep5" runat="server" Title="Finalizar">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:FinalizarProc ID="ucStep5" runat="server" />
                 </div>
            </asp:WizardStep>
             <asp:WizardStep ID="wStep6" runat="server" Title="Proceso completado" StepType="Complete">
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:ResumenFinal ID="ucStep6" runat="server" Modo="Import" />
                </div>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
