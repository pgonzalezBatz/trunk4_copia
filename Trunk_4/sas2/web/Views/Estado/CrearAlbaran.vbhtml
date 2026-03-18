@modeltype VMCreateAlbaran
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
End Code


<h3>@h.traducir("Agrupar Bultos en Albaran")    </h3>
<hr />


<form action="@Url.Action("AgruparMovimientoSave")" method="post">
    @Html.ValidationSummary()
   

    <input type="submit" class="btn btn-primary" value="@h.traducir("Agrupar")" />
</form>