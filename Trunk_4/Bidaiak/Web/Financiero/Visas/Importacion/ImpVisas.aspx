<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ImpVisas.aspx.vb" Inherits="WebRaiz.ImpVisas" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<%@ Register src="Controles/ChequeoEjecucionVisas.ascx" tagname="ChequeoEjec" tagprefix="uc" %>
<%@ Register src="Controles/SubirVisas.ascx" tagname="SubirVisas" tagprefix="uc" %>
<%@ Register src="Controles/ImportarVisas.ascx" tagname="Importar" tagprefix="uc" %>
<%@ Register src="Controles/ResumenVisas.ascx" tagname="Resumen" tagprefix="uc" %>
<%@ Register src="Controles/PreAsientoContVisas.ascx" tagname="PreAsientoCont" tagprefix="uc" %>
<%@ Register src="Controles/FinalizarProcVisas.ascx" tagname="FinalizarProc" tagprefix="uc" %>
<%@ Register src="Controles/ResumenFinal.ascx" tagname="ResumenFinal" tagprefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
     <asp:Wizard ID="wVisas" runat="server" CssClass="Wizard" Height="400px" Width="100%">
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
            <asp:WizardStep ID="wStep1" runat="server" Title="Subir fichero visas">                       
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:SubirVisas ID="ucStep1" runat="server" />            
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="wStep2" runat="server" Title="importar">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:Importar ID="ucStep2" runat="server" />
                </div>
            </asp:WizardStep>
            <asp:WizardStep  ID="wStep3" runat="server" Title="Resumen">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:Resumen ID="ucStep3" runat="server" />
                 </div>
            </asp:WizardStep>  
             <asp:WizardStep  ID="wStep4" runat="server" Title="Previsualizar asiento contable">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:PreAsientoCont ID="ucStep4" runat="server" />
                 </div>
            </asp:WizardStep>          
            <asp:WizardStep ID="wStep5" runat="server" Title="finalizar">
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
