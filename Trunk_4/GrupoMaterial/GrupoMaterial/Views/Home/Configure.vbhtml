@Code
    ViewData("Title") = "Configure"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Configure</h2>
@*"SELECT * FROM DIM_TIEMPO ..."*@
@Html.Label("Year")
@Html.DropDownList("YEAR")
@Html.Label("Month")
@Html.DropDownList("MONTH")
