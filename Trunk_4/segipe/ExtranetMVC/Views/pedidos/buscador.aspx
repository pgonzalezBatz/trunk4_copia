<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage" %>


<%@ Import Namespace="ExtranetMVC" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <title>
        <%=h.Traducir("Buscador de pedidos")%> 
    </title>
    
     <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent1" runat="server">
    <br />
    <form method="post" action="<%=url.action("buscadorporpedido") %>">
        <fieldset>
            <legend><%=h.Traducir("Buscar pedidos por numero")%> </legend>
            <%=h.Traducir("Desde")%> -
            <%=h.Traducir("Hasta")%><br />
            <%=Html.TextBox("desde")%> -
            <%=Html.TextBox("hasta")%>
            <br />
            <input type="submit" value="<%=h.Traducir("Mostrar")%>" />
        </fieldset>
    </form>
    <br />
    <form method="post" action="<%=url.action("buscadorporfecha") %>">
        <fieldset>
            <legend><%=h.Traducir("Buscar pedidos por fecha")%> </legend>
            <%=h.Traducir("Desde")%> -
            <%=h.Traducir("Hasta")%><br />
            <input type="text" class="calendar" name="desde" />
            <input type="text" class="calendar" name="hasta" />
            <br />
            <input type="submit" value="<%=h.Traducir("Mostrar")%>" />
        </fieldset>
    </form>
   <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
        <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-<%=(h.GetCulture().Split("-")(0)) %>.js' type="text/javascript"></script>
  
    <script type="text/javascript">
        $(document).ready(function () {
                        $('.calendar').datepicker($.datepicker.regional['<%=h.GetCulture().Split(" - ")(0)%>']);
        });
    </script>
</asp:Content>
