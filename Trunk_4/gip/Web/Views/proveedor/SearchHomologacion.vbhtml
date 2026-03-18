@imports web
@ModelType IEnumerable(Of String())
@section title
    - @h.Traducir("Busqueda homologacion")
End section
@section menu1
    @Html.Partial("menu")
End Section

<h2>Búsqueda homologaciones</h2>

<form action="" method="get">
    <div class="row">
        <div class="col-sm-9">
            <div class="input-group">
                @Html.TextBox("q", Nothing, New With {.autofocus = "", .class = "form-control"})
                <div class="input-group-btn">
                    <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search"></i></button>
                </div>
            </div>
        </div>
        <div class="col-sm-3">
            @Html.DropDownList("idplanta", Nothing, New With {.class = "form-control"})
        </div>
    </div>
</form>
<br />
@If Model Is Nothing Then

Else
    @<span>  @h.traducir("Nº de resultados") </span>
    @<span Class="badge">@ViewBag.count @Html.Encode(" ")</span>
    @<table class="table table-responsive">
        <thead>
            <tr>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Código"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("nombre"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("R.S."))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("tipo"))
                </th>
                @*<th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("Prov. global"))
                </th>*@
                <th></th>
            </tr>
        </thead>
        <tbody>
            @For Each item In Model
                Dim currentItem = item
                @<tr>
                    <td>
                        <strong>@Html.DisplayFor(Function(modelItem) currentItem(2))</strong>
                    </td>
                     <td>
                         @*<strong>@Html.DisplayFor(Function(modelItem) If(currentItem(5).Equals("GLOBAL"), currentItem(4), currentItem(1)))</strong>*@
                         <strong>
                             @Code 
                                 If currentItem(5).Equals("GLOBAL") Then
                                    @currentItem(4)
                                 Else
                                     @currentItem(1)
                                 End If
                             end Code
                         </strong>
                     </td>
                    <td>
                        <strong>@Html.DisplayFor(Function(modelItem) currentItem(3))</strong>
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) currentItem(5))
                    </td>
                    @*<td>
                        @Html.DisplayFor(Function(modelItem) currentItem(4))
                    </td>*@
                    <td>
                        @Html.ActionLink(h.Traducir("Homologar"), "homologar", h.ToRouteValues(Request.QueryString, New With {.id = currentItem(0), .id2 = currentItem(6)}))
                    </td>
                </tr>
                                 Next
        </tbody>
    </table>
End If
@code
    Dim take = If(Request("take"), proveedorController.TakeLimit)
    Dim skip = If(Request("skip"), proveedorController.skipMin)
End Code

<span class="pagination"></span>
<ul class="pager">
    @If skip > proveedorController.skipMin Then
        @<li>
            <a href="@Url.Action("searchHomologacion", h.ToRouteValues(Request.QueryString, New With {.skip = skip - take}))">@h.traducir("Menos")</a>
        </li>

    End If
    @If ViewBag.count > take Then
        @<li>
            <a href="@Url.Action("searchHomologacion", h.ToRouteValues(Request.QueryString, New With {.skip = skip + take}))">@h.traducir("Mas")</a>
        </li>
    End If
