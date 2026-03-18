<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPCR.Master" CodeBehind="ListadoMaquinas.aspx.vb" Inherits="CostesReales.ListadoMaquinas" %>
<%@ MasterType VirtualPath="~/Master/MPCR.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:Panel ID="pnlListadoMaquinas" runat="server">
            <asp:GridView ID="grdListadoMaquinas" runat="server" AutoGenerateColumns="False" AllowPaging="True" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <Columns>                    
<%--                    <asp:TemplateField HeaderText="PORTADOR">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPortador" runat="server" Text='<%# Bind("PORTADOR") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("PORTADOR") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   --%> 
                    <asp:TemplateField HeaderText="Maquina">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMaquina" runat="server" Text='<%# Bind("Maquina") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Maquina") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField HeaderText="Maquina_des">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMaquinaDes" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Maquina_des") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                </Columns>
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />
            </asp:GridView>
        </asp:Panel>        
    </div>
</asp:Content>
