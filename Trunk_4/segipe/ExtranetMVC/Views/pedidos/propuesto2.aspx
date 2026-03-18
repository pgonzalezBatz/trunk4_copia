<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.Traducir("Pedidos propuestos2 (Lineas)")%> 
    </title>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent1" runat="server">
    <%=Html.Partial("observaciones")%>
    <%=Html.Partial("adjuntos")%>
    <table>
        <thead>
            <tr>
                <th><%=h.Traducir("Nº Linea")%> </th>
                <th><%=h.Traducir("Of")%> </th>
                <th><%=h.Traducir("OP")%> </th>
                <th><%=h.Traducir("Marca")%> </th>
                <th><%=h.Traducir("Descripción")%> </th>
                <th><%=h.Traducir("Cantidad")%> </th>
                <th><%=h.Traducir("Precio unitario")%> </th>
                <th><%=h.Traducir("Descuento")%> </th>
                <th><%=h.Traducir("Importe")%> </th>
                <th><%=h.Traducir("Fecha")%> </th>
                <th><%=h.Traducir("Fecha propuesta")%> </th>
            </tr>
        </thead>
        <tbody>
            <%For Each l In Model%>
                <tr>
                    <td><%=l.linea%></td>
                    <td><%=l.numord%></td>
                    <td><%=l.numope%></td>
                    <td><%=l.marca%></td>
                    <td>
                        <%=l.descripcion0%><br />
                        <%=l.descripcion1%><br />
                        <%=l.descripcion2%>
                    </td>
                    <td><%=l.cantidad%></td>
                    <td><%=l.unitario%></td>
                    <td><%=l.descuento%></td>
                    <td><%=l.importe%></td>
                    <td><%=l.fecha.toshortdatestring()%></td>
                    <td>
                        <%If IsDate(l.fechapropuesta) Then%>
                            l.fechapropuesta.toshortdatestring()
                        <%End If%>
                    </td>
                </tr>
            <%Next%>
        </tbody>
    </table>
</asp:Content>
