<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage(Of web.viaje)" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Editar viaje")%>
    </title>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido1" runat="server">
    <a href="<%=url.action("list") %>"><%= h.traducir("Volver")%></a>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <h3><%= h.traducir("Viaje Nº ")%> <%= Html.DisplayFor(Function(m) m.Id)%></h3>
    <% If Model.ListOfAlbaran.Count > 0 Then%>
    <table class="table1">
        <thead>
            <tr>
            <th><%= h.traducir("Viaje")%></th>
            <th><%= h.traducir("Dirección salida")%></th>
            <th><%= h.traducir("Dirección destino")%></th>
            <th><%= h.traducir("Observación")%></th>
            <th><%= h.traducir("Albaran Nº")%></th>
            <th><%= h.traducir("Bulto Nº")%></th>
            <th><%= h.traducir("Peso bulto")%></th>
            </tr>
        </thead>
        <tbody>
            <%For Each a As web.Albaran In Model.ListOfAlbaran%>
            <%Dim Albaran = True%>
            <%For Each g As web.Agrupacion In a.ListOfAgrupacion%>
            <%Dim first = True%>
                <tr>
                    <%If Albaran Then%>
                     <%Dim s%>
                                       <% s = web.DBAccess.GetDireccionAlbaran(a.Id, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                        <td rowspan="<%=a.ListOfAgrupacion.Count()  %>">
                            <input type="checkbox" name="albaran" value="<%=a.Id %>" />
                        </td>
                         <td rowspan="<%=a.ListOfAgrupacion.Count()  %>">
                                 <%Dim e%>
                            <%If a.IdHelbide.HasValue Then%>
                                <% e = web.DBAccess.GetHelbide(a.IdHelbide, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                                <%= e.Calle%><br />
                                <%= e.CodigoPostal%> <%= e.Poblacion%><br />
                                <%= e.Provincia%><br />
                                <%= e.Pais%><br />
                            <%Else%>
                                Igorre
                            <%End If%>
                             
                        </td>
                        <td rowspan="<%=a.ListOfAgrupacion.Count()  %>">
                                        <%= s.Calle%><br />
                                        <%= s.CodigoPostal%> <%= s.Poblacion%><br />
                                        <%= s.Provincia%><br />
                                        <%= s.Pais%><br />
                        </td>
                        <td rowspan="<%=a.ListOfAgrupacion.Count()  %>"><%= a.Observaciones%></td>
                        <td rowspan="<%=a.ListOfAgrupacion.Count() %>">
                            <%= a.Id%><br />
                            <form action="<%=Url.Action("removealbaran") %>" method="post">
                                <%= Html.Hidden("id", Model.Id)%>
                                <%= Html.Hidden("albaran", a.Id)%>
                                <input type="submit" value="<%= h.traducir("Quitar albarán del viaje")%>" />
                            </form>
                        </td>
                        <%      Albaran = False%>
                    <%End If%>
                    <% If first Then%>
                        <td rowspan="<%=a.ListOfAgrupacion.Count() %>"> 
                            <%= g.Id%>
                            <form action="<%=Url.Action("removebulto") %>" method="post">
                                <%= Html.Hidden("id", Model.Id)%>
                                <%= Html.Hidden("bulto", g.Id)%>
                                <input type="submit" value="<%= h.traducir("Quitar bulto del viaje")%>" />
                            </form>
                        </td>
                        <td rowspan="<%=a.ListOfAgrupacion.Count() %>"><%= g.Peso%></td>
                        <%first = False%>
                    <%End If%>
                </tr>
        <%Next%>
        <%Next%>
        </tbody>
    </table>
    <%End If%>
    <% If Model.ListOfRecogida.Count > 0 Then%>
    <table class="table1">
        <thead>
            <tr>
            <th><%= h.traducir("Dirección salida")%></th>
            <th><%= h.traducir("Dirección destino")%></th>
            <th><%= h.traducir("Nº Recogida")%></th>
            <th><%= h.traducir("OF")%> -
                <%= h.traducir("OP")%> -
                <%= h.traducir("Peso")%>
            </th>
            <th><%= h.traducir("Creado")%></th>
            <th><%= h.traducir("Observación")%></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%For Each r In Model.ListOfRecogida%>
                <tr>
                     <td>
                        <%Dim s%>
                            <% s = web.DBAccess.GetDireccionProveedor(r.IdEmpresaRecogida, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                            <%=r.nombreEmpresaRecogida%><br />
                            <%= s.Calle%><br />
                            <%= s.CodigoPostal%> <%= s.Poblacion%><br />
                            <%= s.Provincia%><br />
                            <%= s.Pais%><br />
                    </td>
                    <td >
                        <%Dim e%>
                       <% s = web.DBAccess.GetDireccionProveedor(r.IdEmpresaEntrega, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                        <%=r.nombreEmpresaEntrega%><br />
                            <%= s.Calle%><br />
                            <%= s.CodigoPostal%> <%= s.Poblacion%><br />
                            <%= s.Provincia%><br />
                            <%= s.Pais%><br />
                    </td>
                    <td><%=r.Id%></td>
                    <td>
                        <%For Each ofop In r.ListOfOp%>
                            <div><%=ofop.Numord%> - <%=ofop.Numope%> - <%=ofop.Peso%></div>
                        <%Next%>
                    </td>
                    <td><%=r.nombreSab%></td>
                    <td><%=r.Observacion%></td>
                    <td>
                        <form action="<%=Url.action("removerecogida") %>" method="post">
                            <%= Html.Hidden("id", Model.Id)%>
                            <%= Html.Hidden("recogida", r.Id)%>
                            <input type="submit" value="<%= h.Traducir("Quitar recogida del viaje")%>" />
                        </form>
                    </td>
                </tr>
            <%next %>
        </tbody>
    </table>
    <%End If%>
    <br />
    <form action="<%=Url.action("removeviaje") %>" method="post">
        <%= Html.Hidden("viaje", Model.Id)%>
        <input type="submit" value="<%= h.Traducir("Eliminar viaje")%>" />
    </form>
</asp:Content>
