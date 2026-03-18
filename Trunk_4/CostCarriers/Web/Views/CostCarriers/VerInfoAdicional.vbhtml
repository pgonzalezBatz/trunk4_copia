@code
    Dim viewDataDictonaryInfo As ViewDataDictionary = CType(ViewData("ViewDataDictionary"), ViewDataDictionary)
end code

@Html.Partial("~/Views/Shared/_ValidacionAdicionalPlanta.vbhtml", viewDataDictonaryInfo)

