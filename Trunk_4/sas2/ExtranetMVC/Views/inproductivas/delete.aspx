<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.traducir("OF inproductivas")%>
    </title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContenido2" runat="server">
    <form action="" method="post">
            <h3><%=h.traducir("Estas a punto de borrar el siguiente viaje")%></h3>
            <strong><%=h.traducir("Fecha")%></strong><br />
            <%=Model.fecha%><br />
            <strong><%=h.traducir("Origen")%></strong><br />
            <%=Model.origen%><br />
            <strong><%=h.traducir("Destino")%></strong><br />
            <%=Model.destino%><br />
            <strong><%=h.traducir("Observaciones")%></strong><br />
            <%=Model.observacion%><br />
            <strong><%=h.traducir("Importe")%></strong><br />
            <%=Model.importe%><br />
            <a href="<%=Url.Action("list")%>"><%=h.traducir("Volver")%></a>
            <input type="submit" value="<%=h.traducir("Eliminar")%>" name="confirm" class="btn btn-primary" />
    </form>
</asp:Content>
