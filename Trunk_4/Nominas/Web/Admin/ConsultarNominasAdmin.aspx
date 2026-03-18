<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="ConsultarNominasAdmin.aspx.vb" Inherits="Nominas.ConsultarNominasAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:Label runat="server" ID="labelMesAno" Text="Seleccione fechas de las nóminas y usuarios a consultar"></asp:Label><br />
    <br />
    <div class="form-inline form-group">
        <asp:Label runat="server" ID="lblDesde" Text="Desde:"></asp:Label>
        <div class='input-group date myDatePicker'>
            <input type="text" id="datepicker1" runat="server" class="form-control" />
            <span class="input-group-addon">
                <span class="glyphicon glyphicon-calendar"></span>
            </span>
        </div>
    </div>
    <div class="form-inline form-group">
        <asp:Label runat="server" ID="lblHasta" Text="Hasta:"></asp:Label>
        <div class='input-group date myDatePicker'>
            <input type="text" id="datepicker2" runat="server" class="form-control" />
            <span class="input-group-addon">
                <span class="glyphicon glyphicon-calendar"></span>
            </span>
        </div>
    </div>
    <div class="form-inline form-group">
        <asp:Label runat="server" ID="Label1" Text="Usuarios:"></asp:Label>
        <input type="text" id="users" runat="server" class="form-control" />
        <asp:Label runat="server" ID="label2" Text="(Codigos de trabajador separados por punto y coma ';')" Style="font-size: 10px"></asp:Label>
    </div>
    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-primary" />
    <asp:Label runat="server" ID="lblError" Text="Debes rellenar todos los campos" ForeColor="DarkRed" Visible="false"></asp:Label>
    <br />
    <br />

    <%--<table class="table" id="resultTable" runat="server">--%>

    <table class="table table-bordered table-striped">
        <thead>
            <tr id="tableHead" runat="server" style="color:#eee;background-color:#337ab7;">
                <th>Código de trabajador</th>
                <th>Nombre</th>
                <th>Fecha</th>
                <th>Tipo</th>
                <th>Link</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater runat="server" ID="rptNominas">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="codTra"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="nomTra"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="fecha"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="tipo"></asp:Label></td>
                        <td style="text-align:center">
                            <asp:LinkButton runat="server" ID="lnkVer" CssClass="" OnClick="VerNomina" style="color:#449d44;background-color:none;padding:0px"><span class="glyphicon glyphicon-download"></span></asp:LinkButton></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <asp:Button runat="server" ID="descargaNominas" CssClass="btn btn-success" Text="Descargar resultados" />

    <asp:Panel runat="server" ID="pnlSinResultados" CssClass="alert alert-warning">
        <asp:Label runat="server" Text="No se ha encontrado ninguna nomina" Style="font-weight: bold;"></asp:Label><br />
    </asp:Panel>
    <br />

    <script type="text/javascript">
        $(function () {
            //$.SeparadorDecimal("@System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator");
            $('.myDatePicker').datetimepicker({
                    //format: "<%=System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.CultureName).DateTimeFormat.ShortDatePattern.ToUpper %>",
                //defaultDate: "",
                locale: "es",
                viewMode: 'months',
                format: 'YYYY/MM',
                maxDate: moment()
            });
        });
    </script>


</asp:Content>
