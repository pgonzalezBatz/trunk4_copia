<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Calendario.ascx.vb" Inherits="KEM.Calendario" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicTextToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:TextBox ID="txtFecha" runat="server" Width="80" ValidationGroup="Fecha" MaxLength="10"></asp:TextBox>
<asp:Panel runat="server" ID="panel1" CssClass="popupControl">
    <asp:UpdatePanel>
        <ContentTemplate>
            <center>
                <asp:Calendar ID="calendarAjax" runat="server" BackColor="White" BorderColor="#999999"
                    CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                    ForeColor="Black" Width="160px" OnSelectionChanged="Calendar_SelectionChanged">
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                </asp:Calendar>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:RegularExpressionValidator runat="server" id="revFecha" ControlToValidate="txtFecha" ErrorMessage="!!!!" ValidationExpression="(0[1-9]|[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|[1-9]|1[012])[- /.](19|20)\d\d" ValidationGroup="Fecha"></asp:RegularExpressionValidator>
<ajaxToolkit:PopupControlExtender ID="popupControlExtender1" runat="server" TargetControlID="txtFecha" PopupControlID="Panel1" Position="Bottom" />
