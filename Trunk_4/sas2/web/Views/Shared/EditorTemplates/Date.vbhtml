@modeltype Date

@Html.TextBox("", Model.ToShortDateString(), New With {.class = "calendar form-control"})