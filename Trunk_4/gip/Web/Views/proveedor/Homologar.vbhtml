@imports web
@ModelType Integer
@section title
    - @h.Traducir("Homologar")
End section
@section menu1
    @Html.Partial("menu")
End Section
@Code

    ViewData("Title") = "Homologar"
    Dim idProvider = Model
    Dim isGlobal = ViewBag.isGlobal
    Dim tipo = If(isGlobal, "global", "local")
    Dim strCnGM = ConfigurationManager.ConnectionStrings("grupomaterial").ConnectionString
    Dim strCnSAB = ConfigurationManager.ConnectionStrings("oracle").ConnectionString
    Dim nombre = Db.getProviderName(idProvider, isGlobal, strCnSAB)
    Dim lstElementos = Db.getElementListWithHomologationsForProvider(idProvider, isGlobal, strCnGM)
    Dim plantasActivas = System.Configuration.ConfigurationManager.AppSettings("plantasActivas").Split(";")
    Dim lstPlantas = Db.getPlantasConPertenencia(idProvider, isGlobal, strCnGM).Where(Function(o) plantasActivas.Contains(o(0))).OrderBy(Function(o2) CInt(o2(0)))
End Code

<h2>Proveedor @tipo "@nombre"</h2>
<br />
<h3>Homologación por elemento: </h3>
<a id="verTabla1" runat="server" data-toggle="collapse" aria-expanded="true" href="#myTable1">Ver/ocultar</a>
<br />

<asp:UpdatePanel>
    <table class="table table-striped collapse in" id="myTable1" style="height:auto;width:auto">
        <thead>
            <tr style="background:#337ab7;color:white;font-weight:600;text-align:center">
                <td>Comodity</td>
                <td>Familia</td>
                <td>Subfamilia</td>
                <td>Elemento</td>
                <td>Homologado</td>
            </tr>
        </thead>
        <tbody>
            @For Each item In lstElementos
                @<tr>
                    <td>@item(0)</td>
                    <td>@item(1)</td>
                    <td>@item(2)</td>
                    <td>@item(3)</td>
                    @*<td>@Html.CheckBox("homologado", CInt(item(5)) > 0, New With {.id = item(4), .onclick = "homologar(this," & idProvider & "," & If(isGlobal, "true", "false") & ")"})</td>*@
                    <td style="text-align:center">@Html.CheckBox("homologado", CInt(item(5)) > 0, New With {.id = item(4), .onclick = "homologar(this," & idProvider & "," & isGlobal.ToString.ToLower & ")"})</td>
                </tr>
            Next
        </tbody>
    </table>
</asp:UpdatePanel>

<h3>Pertenencia a panel de planta: </h3>
<a id="verTabla2" runat="server" data-toggle="collapse" aria-expanded="true" href="#myTable2">Ver/ocultar</a>
<br />
<table class="table table-striped collapse in" id="myTable2" style="height:auto;width:auto">
    <thead>
        <tr style="background:#337ab7;color:white;font-weight:600;text-align:center">
            <td>Planta</td>
            <td>Pertenece</td>
        </tr>
    </thead>
    <tbody>
        @For Each item In lstPlantas
            @<tr>
                <td>@item(1)</td>
                <td style="text-align:center">@Html.CheckBox("pertenece", CInt(item(2)) > 0, New With {.id = item(0), .onclick = "setPertenencia(this," & idProvider & "," & isGlobal.ToString.ToLower & ")"})</td>
            </tr>
        Next
    </tbody>
</table>

<script type="text/javascript">
    function homologar(element, idProvider, isGlobal) {
        $.ajax({
            url: '@(Url.Action("HomologarItem", "proveedor"))',
            type: 'post',
            data: {
                elementId: element.id,
                providerId: idProvider,
                isGlobal: isGlobal,
                setChecked: element.checked
            }
        });
    }
    function setPertenencia(element, idProvider, isGlobal) {
        $.ajax({
            url: '@(Url.Action("SetPertenencia", "proveedor"))',
            type: 'post',
            data: {
                elementId: element.id,
                providerId: idProvider,
                isGlobal: isGlobal,
                setChecked: element.checked
            }
        });
    }

</script>