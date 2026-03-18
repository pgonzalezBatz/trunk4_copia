<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Añadir Albaran")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <table class="table1">
        <thead>
            <tr>
            <th><%= h.traducir("Dirección salida")%></th>
            <th><%= h.traducir("Dirección destino")%></th>
            <th><%= h.traducir("Observación")%></th>
            <th><%= h.traducir("Albaran Nº")%></th>
            <th><%= h.traducir("Acciones")%></th>
            <th><%= h.traducir("Peso bulto")%></th>
            <th><%= h.traducir("OF")%></th>
            <th><%= h.traducir("OP")%></th>
            <th><%= h.traducir("Marca")%></th>
            <th><%= h.traducir("Fecha de entrega")%></th>
            <th><%= h.traducir("Cantidad")%></th>
            <th><%= h.traducir("Proveedor")%></th>
            <th><%= h.traducir("Creado")%></th>
            <th><%= h.Traducir("Negocio")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Albaran In Model%>
            <%Dim Albaran = True%>
            <%For Each g As web.Agrupacion In a.ListOfAgrupacion%>
            <%Dim first = True%>
            <%For Each m In g.ListOfMovimiento%>
                <tr>
                    <%If Albaran Then%>
                        <td rowspan="<%=a.ListOfAgrupacion.sum(function(gr) gr.ListOfMovimiento.count())  %>">
                                    <%Dim s%>
                                       <% s = web.DBAccess.GetDireccionProveedor(a.ListOfAgrupacion.First.ListOfMovimiento.First.EmpresaSalida, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                                        <%= s.Calle%><br />
                                        <%= s.CodigoPostal%> <%= s.Poblacion%><br />
                                        <%= s.Provincia%><br />
                                        <%= s.Pais%><br />
                        </td>
                        <td rowspan="<%=a.ListOfAgrupacion.sum(function(gr) gr.ListOfMovimiento.count())  %>">
                            <%Dim e%>
                            <%If a.IdHelbide.HasValue Then%>
                                <% e = web.DBAccess.GetHelbide(a.IdHelbide, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                            <%Else%>
                                <% e = web.DBAccess.GetDireccionProveedor(a.ListOfAgrupacion.First().ListOfMovimiento.First.CodPro, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                            <%End If%>
                                <%= e.Calle%><br />
                                <%= e.CodigoPostal%> <%= e.Poblacion%><br />
                                <%= e.Provincia%><br />
                                <%= e.Pais%><br />
                        </td>
                        <td rowspan="<%=a.ListOfAgrupacion.sum(function(gr) gr.ListOfMovimiento.count())  %>"><%= a.Observaciones%></td>
                        <td rowspan="<%=a.ListOfAgrupacion.sum(function(gr) gr.ListOfMovimiento.count())  %>">
                            <%= a.Id%><br />
                        </td>
                        <td rowspan="<%=a.ListOfAgrupacion.sum(function(gr) gr.ListOfMovimiento.count())  %>">
                            <form action="?<%=Request.QueryString.ToString() %>" method="post">
                                <%= Html.Hidden("idAlbaran", a.Id)%>
                                <input type="submit" value="<%= h.Traducir("Añadir albaran a viaje")%>" />
                            </form>
                        </td>
                        <%albaran = False%>
                    <%End If%>
                    <% If first Then%>
                        <td rowspan="<%=g.ListOfMovimiento.count() %>"><%= g.Peso%></td>
                        <%first = False%>
                    <%End If%>
                    <td><%= m.Numord%></td>
                    <td><%= m.Numope%></td>
                    <td><%= m.Marca%></td>
                    <td>
                    <%If m.FechaEntrega.HasValue Then%>
                    <%= m.FechaEntrega.Value.ToShortDateString%>
                    <%End If%>
                    </td>
                    <td><%= m.Cantidad%></td>
                    <td><%= m.NombreProveedor%></td>
                    <td><%= m.NombreSab%></td>
                    <td><%= m.Negocio%></td>                    
                </tr>
            <%Next%>
        <%Next%>
        <%Next%>
        </tbody>
    </table>
</asp:Content>
