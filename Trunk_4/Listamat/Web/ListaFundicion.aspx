<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ListaFundicion.aspx.vb" Inherits="Web.ListaFundicion" 
    MasterPageFile="~/Site1.Master" EnableViewState="false" %>
<%@ Import Namespace="System.Globalization" %>        
<%@ Import Namespace="Web"%>

<asp:Content ContentPlaceHolderID="cphContenido" runat="server">
    <div class="alingMiddle">
        <asp:DropDownList ID="ddlOf" runat="server" AutoPostBack="true" CssClass="fontGrande"></asp:DropDownList>
        <asp:DropDownList ID="ddlOp" runat="server" AutoPostBack="true" CssClass="fontGrande"></asp:DropDownList>
        <div id="d1" runat="server" class="div1">
            <a href="<%= "ListaFundicionMarcas.aspx?ord="+ ddlof.selectedvalue + "&op=" + ddlop.selectedvalue %>" >
                 <img src="App_Themes/Theme1/Images/anadirLM.gif" alt="Añadir" title="Añadir marca" />   
            </a>
            <asp:imagebutton ID="btnLanzar" runat="server" text="LanzarMarcasSeleccionadas"
                                            ImageUrl="~/App_Themes/Theme1/Images/lanzarLM.gif"  OnClick="Lanzar"/>
            <a href="<%= "ListaFundicionImprimir.aspx?ord="+ ddlof.selectedvalue + "&op=" + ddlop.selectedvalue %>">
                <img src="App_Themes/Theme1/Images/imprimirLM.png" alt="Vista imprimible" title="Vista imprimible, incluye marcas lanzadas" />
            </a>
            <asp:imagebutton runat="server" text="Ordenar marcas"
                                            ImageUrl="~/App_Themes/Theme1/Images/ordenarLM.gif"  OnClick="Ordenar"/>
        </div>
    </div>
    <asp:Repeater ID="rptMarcas" runat="server">
        <HeaderTemplate>
            <table class="table1">
                <thead>
                    <tr>
                        <th>
                            <%=h.traducir("Lanz.")%>
                        </th>
                        <th>
                            <%=h.traducir("Marca")%>
                        </th>
                        <th>
                            <%=h.traducir("Cantidad")%>
                        </th>
                        <th>
                            <%=h.traducir("Denominacion")%>
                        </th>
                        <th>
                            <%=h.traducir("Material")%>
                        </th>
                        <th>
                            <%=h.traducir("Dureza")%>
                        </th>
                        <th>
                            <%=h.traducir("TTemple")%>
                        </th>
                        <th>
                            <%=h.traducir("TSecundario")%>
                        </th>
                        <th>
                            <%=h.traducir("Realiza")%>
                        </th>
                        <th>
                            <%= h.traducir("Peso")%>
                        </th>
                        <th>
                            <%= h.traducir("Norma")%>
                        </th>
                          <th>
                            <%= h.traducir("Conjunto")%>
                        </th>
                        <th>
                            <%=h.traducir("Editar")%>
                        </th>
                        <th>
                            <%=h.traducir("Copiar")%>
                        </th>
                        <th>
                            <%=h.traducir("Eliminar")%>
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:CheckBox ID="cbLanzar" runat="server" />
                </td>
                <td>
                    <asp:Literal ID="ltlMarca" runat="server" Text="<%#Container.DataItem(0)%>"></asp:Literal>
                </td>
                <td>
                    <%#Container.DataItem(1)%>
                </td>
                <td>
                    <%#Container.DataItem(2)%>
                </td>
                <td>
                    <%#Container.DataItem(3)%>
                </td>
                <td>
                    <%#Container.DataItem(4)%>
                </td>
                <td>
                    <%#Container.DataItem(5)%>
                </td>
                <td>
                    <%#Container.DataItem(6)%>
                </td>
                <td align="center">
                    <%#Container.DataItem(7)%>
                </td>
                <td align="center">
                    <%# Container.DataItem(14)%>
                </td>
                <td align="center">
                    <%# Container.DataItem(15)%>
                </td>
                 <td align="center">
                    <%# Container.DataItem(16)%>
                </td>
                <td align="center">
                    <a href="<%# GetEditLink(Container.DataItem(0).trimend(" ")) %>">
                        <img src="App_Themes/Theme1/Images/lapiz.gif" alt="Editar" />
                    </a>
                </td>
                <td align="center">
                    <a href="<%# GetCopyLink(Container.DataItem(0).trimend(" ")) %>">
                        <img src="App_Themes/Theme1/Images/copiar.gif" alt="Copiar" />
                    </a>
                </td>
                <td align="center">
                    <asp:imagebutton ImageUrl="App_Themes/Theme1/Images/borrar.gif" text="Eliminar" runat="server" 
                        AlternateText="Eliminar" CommandArgument="<%#Container.DataItem(0)%>" OnClick="Eliminar"
                        OnClientClick="return confirm('Esto borrara la marca!');" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>