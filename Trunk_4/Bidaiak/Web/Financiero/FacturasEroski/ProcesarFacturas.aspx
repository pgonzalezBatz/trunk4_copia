<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="ProcesarFacturas.aspx.vb" Inherits="WebRaiz.ProcesarFacturas" %>
<%@ Register src="Controles/ChequearEjecucion_Fact.ascx" tagname="ChequearEjecucion" tagprefix="uc" %>
<%@ Register src="Controles/SubirFichero_Fact.ascx" tagname="SubirFichero" tagprefix="uc" %>
<%@ Register src="Controles/ImportarAlbaranesTemp_Fact.ascx" tagname="ImportarAlbaranesTemp" tagprefix="uc" %>
<%@ Register src="Controles/ResumenImportacionTemp_Fact.ascx" tagname="ResumenImportacionTemp" tagprefix="uc" %>
<%@ Register src="Controles/VisualizarAsientosContables_Fact.ascx" tagname="VisualizarAsientosContables" tagprefix="uc" %>
<%@ Register src="Controles/FinalizarImportacion_Fact.ascx" tagname="FinalizarImportacion" tagprefix="uc" %>
<%@ Register src="Controles/Procesando_Fact.ascx" tagname="Procesando" tagprefix="uc" %>
<%@ Register src="Controles/Resultado_Fact.ascx" tagname="Resultado" tagprefix="uc" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:Wizard ID="wFacturas" runat="server" CssClass="Wizard2" Height="400px" Width="100%">
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
        <HeaderTemplate>
           <asp:Label runat="server" ID="lblInfoAñoMes"></asp:Label>
           <asp:Label runat="server" id="lblTitulo"></asp:Label>
       </HeaderTemplate>
        <WizardSteps>  
             <asp:WizardStep ID="wStep0" runat="server" Title="Chequeo ejecucion" StepType="Start">                       
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:ChequearEjecucion ID="ucChequearEjecucion_Step0" runat="server" />       
                </div>
            </asp:WizardStep>  
            <asp:WizardStep ID="wStep1" runat="server" Title="Subir fichero">                       
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:SubirFichero ID="ucSubirFichero_Step1" runat="server" />            
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="wStep2" runat="server" Title="Importar albaranes temporales">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:ImportarAlbaranesTemp ID="ucImportarAlbaranesTemp_Step2" runat="server" />
                </div>
            </asp:WizardStep>
            <asp:WizardStep  ID="wStep3" runat="server" Title="Resumen temporal">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:ResumenImportacionTemp ID="ucResumenImportacionTemp_Step3" runat="server" />
                 </div>
            </asp:WizardStep>
            <asp:WizardStep  ID="wStep4" runat="server" Title="Visualizar asientos contables">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:VisualizarAsientosContables ID="ucVisualizarAsientosContables_Step4" runat="server" />
                 </div>
            </asp:WizardStep>
             <asp:WizardStep ID="wStep5" runat="server" Title="Finalizar importación">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:FinalizarImportacion ID="ucFinalizarImportacion_Step5" runat="server" />
                 </div>
            </asp:WizardStep>
            <asp:WizardStep  ID="wStep6" runat="server" Title="Procesando">
                 <div style="margin-left:25px;margin-top:15px;">
                    <uc:Procesando ID="ucProcesando_Step6" runat="server" />
                 </div>
            </asp:WizardStep> 
            <asp:WizardStep ID="wStep7" runat="server" Title="Proceso completado" StepType="Complete">
                <div style="margin-left:25px;margin-top:15px;">
                    <uc:Resultado ID="ucResultado_Step7" runat="server" Modo="Import" />
                </div>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>