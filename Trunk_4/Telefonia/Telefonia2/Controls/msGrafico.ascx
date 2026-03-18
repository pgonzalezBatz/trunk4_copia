<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="msGrafico.ascx.vb" Inherits="Telefonia.msGrafico" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Chart ID="chGrafico" runat="server">
    <Titles>
        <asp:Title Name="Title"></asp:Title>		
    </Titles>
    <Legends>
        <asp:Legend Name="Legend"></asp:Legend>
    </Legends>
    <BorderSkin />    
    <Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>