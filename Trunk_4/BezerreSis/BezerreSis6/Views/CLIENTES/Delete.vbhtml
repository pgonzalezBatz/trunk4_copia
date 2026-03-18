@ModelType BezerreSis.CLIENTES
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>CLIENTES</h4>
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.NOMBRE)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.NOMBRE)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()
    End Using
</div>
