@ModelType BezerreSis.OBJETIVOS
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>OBJETIVOS</h4>
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.FECHA)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.FECHA)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.PPM_ANUAL)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.PPM_ANUAL)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.PPM_MENSUAL)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.PPM_MENSUAL)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.IPB_SEMESTRAL)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.IPB_SEMESTRAL)
        </dd>


        <dt>
            @Html.DisplayNameFor(Function(model) model.CLIENTES.NOMBRE)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CLIENTES.NOMBRE)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

    End Using
</div>
