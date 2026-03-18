<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%= h.traducir("Listado de albaranes")%>
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContenido2" runat="server">
    <a href="<%=url.Content("~/help.html#albaran") %>" target="_blank"><%= h.traducir("Ayuda")%></a>
    <%If ViewData("listOfAlbaran") Is Nothing OrElse ViewData("listOfAlbaran").count = 0 Then%>
        <h1><%= h.traducir("No se han encontrado albaranes")%></h1>
    <%Else%>
       <h3><%= h.traducir("Listado de albaranes sin viaje")%></h3>
        <form action="<%=Url.Action("create","viaje") %>" method="get">
            <table class="table1">
                <thead>
                    <tr>
                    <th><%= h.traducir("Viaje")%></th>
                    <th><%= h.traducir("Empresa salida")%></th>

                    <th><%= h.traducir("Dirección destino")%></th>
                        <th><%= h.Traducir("Proveedor")%></th>
                    <th><%= h.traducir("Observación")%></th>
                    <th><%= h.traducir("Albaran Nº")%></th>
                        
                    <th><%= h.Traducir("Peso bulto")%></th>
                    <th><%= h.traducir("Nº bulto")%></th>
                        <th><%= h.traducir("Nº bulto padre")%></th>
                        <th><%= h.Traducir("Negocio")%></th>
                    </tr>
                </thead>
                <tbody>
                    <%For Each a As web.Albaran In ViewData("listOfAlbaran")%>
                    <%Dim e = web.DBAccess.GetDireccionAlbaran(a.Id, ConfigurationManager.ConnectionStrings("SAS").ConnectionString)%>
                    <%Dim Albaran = True%>
                    <%For Each g In a.ListOfAgrupacion%>
                            <tr>
                            <%If Albaran Then%>
                                <td rowspan="<%=a.ListOfAgrupacion.Count() %>">
                                    <input type="checkbox" name="albaran" value="<%=a.Id %>" />
                                    <a href="<%=Url.Action("etiquetas", New With {.idalbaran = a.Id}) %>"><%=h.traducir("Etiquetas") %></a>
                                    <a href="<%=Url.Action("printalbaran", New With {.idalbaran = a.Id}) %>"><%=h.traducir("Albaran") %></a>
                                </td>
                                <td rowspan="<%=a.ListOfAgrupacion.Count()  %>">
                                    <%Dim s%>
                                        <%= a.ListOfAgrupacion.First(Function(b) b.ListOfMovimiento.Count > 0).ListOfMovimiento.First.NombreEmpresaSalida%>
                                </td>
                                <td rowspan="<%=a.ListOfAgrupacion.Count()  %>">
                                    
                                        <%= e.Calle%><br />
                                        <%= e.CodigoPostal%> <%= e.Poblacion%><br />
                                        <%= e.Provincia%><br />
                                        <%= e.Pais%><br />
                                </td>
                                <td rowspan="<%=a.ListOfAgrupacion.Count()  %>">
                                    <strong style="font-size:1.2em;"><%= e.NombreEmpresa%></strong>
                                </td>
                                <td rowspan="<%=a.ListOfAgrupacion.Count()  %>"><%= a.Observaciones%></td>
                                <td rowspan="<%=a.ListOfAgrupacion.Count()  %>">
                                    <%= a.Id%><br />
                                    <a href="<%=Url.Action("addbulto", New With {.id = a.Id}) %>"><%= h.traducir("Añadir bulto")%></a>
                                    <a href="<%=Url.Action("edit", New With {.id = a.Id}) %>"><%= h.traducir("Quitar bulto")%></a>
                                </td>
                                
                                <%albaran = False%>
                            <%End If%>
                            <td><%=g.Peso %>Kg</td>
                            <td><%=g.Id %></td>
                                <td><%=g.idParent %></td>
                                <td><%=a.Negocio %></td>
                        </tr>
                <%Next%>
                <%Next%>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="15" align="left">
                            <input type="submit" value="<%= h.Traducir("Agrupar albaranes en viajes")%>" />
                        </td>
                    </tr>
                </tfoot>
            </table>
            </form>
        
    <%End If%>
    
    <div style="background-color:#C3C3C3; height:10px;width:10px; border:1px solid black;float:left;"></div>
    <span style="font-size:0.9em; margin-left:0.6em;font-style:italic;"><%= h.Traducir("albaranes de recogida")%></span>
    
</asp:Content>
