@modeltype Boolean?

@Html.CheckBox("", Model.HasValue AndAlso Model.Value)