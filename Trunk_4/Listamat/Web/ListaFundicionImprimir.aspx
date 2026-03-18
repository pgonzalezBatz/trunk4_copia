<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ListaFundicionImprimir.aspx.vb" Inherits="Web.ListaFundicionImprimir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Globalization" %>        
<%@ Import Namespace="Web"%>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Lista Fundición</title>
    <link href="App_Themes/Theme1/print.css"  type="text/css" rel="Stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divHelp">
        <a href="https://xbi.batz.es/extranetlogin/PrintHelp.htm" target="_blank" title="La ayuda se abrira en otra ventana">
            <img id="imgHelp" src="App_Themes/Theme1/Images/help.png" alt="Ayuda oara imprimir" />
        </a>
    </div>
    <div>
        <table id="layout">
            <thead>
                <tr>
                    <td colspan="8" align="center">
                        <h2>
                            <%=h.traducir("Lista de Fundición")%>
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <div id="tableHead">
                            <span>Of:</span>
                            <asp:Literal ID="ltlOf" runat="server"></asp:Literal>
                            <span><%=h.traducir("Nº troquel")%>:</span>
                            <asp:Literal ID="ltlNTroquel" runat="server"></asp:Literal>
                            <span><%=h.traducir("Client")%>:</span>
                            <asp:Literal ID="ltlCliente" runat="server"></asp:Literal>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th><%=h.traducir("Marca")%></th>
                    <th><%=h.traducir("F. Lanza")%></th>
                    <th><%=h.traducir("Cant.")%></th>
                    <th><%=h.traducir("Denominación")%></th>
                    <th><%=h.traducir("Sem. Nec")%></th>
                    <th><%=h.traducir("Troquel-Marca")%></th>
                    <th><%=h.traducir("Material")%></th>
                    <th><%=h.traducir("Tramtamiento")%></th>
                    <th><%=h.traducir("Conjunto")%></th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptMarcas" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <%#Container.DataItem(0)%>
                            </td>
                            <td>
                                <%#If(Container.DataItem(8).length > 0, CDate(Container.DataItem(8)).ToShortDateString, "")%>
                            </td>
                            <td align="center">
                                <%#Container.DataItem(1)%>
                            </td>
                            <td>
                                <%#Container.DataItem(2)%>
                            </td>
                            <td>
                                <%#If(Container.DataItem(10).length > 0, Web.Utilities.GetSemanaAnual(Container.DataItem(10)), "")%>
                            </td>
                            <td>
                                <%#ltlNTroquel.Text + "-" + Container.DataItem(0)%>
                            </td>
                            <td>
                                <%#Container.DataItem(3)%>
                            </td>
                            <td>
                                <%#Container.DataItem(5)%> <br />
                                <%#Container.DataItem(6)%>
                            </td>
                             <td>
                                <%#Container.DataItem(16)%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
