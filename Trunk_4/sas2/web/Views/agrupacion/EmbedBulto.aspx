<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.Traducir("Listado agrupaciones")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMenu2" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <%=h.traducir("Seleccionar el bulto en el que se quiere agrupar el bulto Nº") %><%=Request("id") %>
    <% If ViewData("listOfAgrupacion").count = 0 Then%>
        <h1><%= h.traducir("No se han encontrado bultos en los que introducir")%></h1>
    <%Else%>
    <h3><%= h.Traducir("Listado de bultos sin albaranes")%></h3>
    <table id="table1" class="table1">
        <thead>
            <tr>
                <th><%= h.traducir("Bulto Nº")%></th>
                <th>
                    <%= h.traducir("Peso bulto")%>
                </th>
                <th>
                    <%= h.Traducir("OF")%>
                </th>
                <th><%= h.Traducir("OP")%></th>
                <th><%= h.Traducir("Marca")%></th>
                <th><%= h.Traducir("Fecha de entrega")%></th>
                <th><%= h.Traducir("Cantidad")%></th>
                <th><%= h.Traducir("Peso")%></th>
                <th><%= h.Traducir("Proveedor")%></th>
                <th><%= h.Traducir("Empresa salida")%></th>
                <th><%= h.Traducir("Creado")%></th>
                <th><%= h.Traducir("Observación")%></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Agrupacion In ViewData("listOfAgrupacion")%>
            <%If a.ListOfMovimiento.Count = 0 %>
                    <tr>
                        <td><%= a.Id%>
                              <form action="" method="post">
                                    <%=Html.Hidden("idParent", a.Id) %>
                                    <input type="submit" value="<%=h.traducir("Agrupar") %>"/>
                                </form>
                        </td>
                        <td><%= a.Peso%></td>
                    </tr>
                <%End If %>
                <%Dim first = True%>
                <%For Each m In a.ListOfMovimiento%>
                        <tr>
                        <% If first Then%>
                            <td rowspan="<%=a.ListOfMovimiento.Count() %>">
                                <%= a.Id%><br />
                                <form action="" method="post">
                                    <%=Html.Hidden("idParent", a.Id) %>
                                    <input type="submit" value="<%=h.traducir("Agrupar") %>"/>
                                </form>
                            </td>
                            <td rowspan="<%=a.ListOfMovimiento.Count() %>"><%= a.Peso%></td>
                            <%first = False%>
                        <%End If%>
                        <td><%= m.Numord%></td>
                        <td><%= m.Numope%></td>
                        <td><%= m.Marca%> <br /> <%=m.Material %></td>
                        <td>
                        <%If m.FechaEntrega.HasValue Then%>
                        <%= m.FechaEntrega.Value.ToShortDateString%>
                        <%End If%>
                        </td>
                        <td><%= m.Cantidad%></td>
                        <td><%= m.Peso.Value.ToString("0.##")%></td>
                        <td><strong><%= m.NombreProveedor%></strong></td>
                        <td><%= m.NombreEmpresaSalida%></td>
                        <td><%= m.NombreSab%></td>
                        <td><%=m.Observacion %></td>
                           
                   </tr>
                <%Next%>
            <%Next%>
        </tbody>
    </table>
    
    <%End If%>
</asp:Content>
