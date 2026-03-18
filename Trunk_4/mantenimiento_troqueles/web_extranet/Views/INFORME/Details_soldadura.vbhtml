@ModelType web_extranet.INFORMES

<h3>@h.traducir("Visualización del detalle del informe")</h3>
<hr />

    <div>
        <h4>INFORMES</h4>
        <dl>
            <dt>
                @Html.DisplayNameFor(Function(model) model.CLIENTE)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.CLIENTE)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.PROYECTO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.PROYECTO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.NPIEZA)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.NPIEZA)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.DESCPIEZA)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.DESCPIEZA)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.NTROQUEL)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.NTROQUEL)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.MATERIAL)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.MATERIAL)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.TRATAMSEC)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.TRATAMSEC)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.DUREZA)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.DUREZA)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.FECHAINFORME)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.FECHAINFORME)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.CREADOPOR)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.CREADOPOR)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.TIPOSOLDADURADURO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.TIPOSOLDADURADURO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.MATERIALAPORTACIONSOLDDURO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.MATERIALAPORTACIONSOLDDURO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.MATERIALAPORTACIONSOLDBLANDO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.MATERIALAPORTACIONSOLDBLANDO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.KGCONSUMIDOSSOLDDURO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.KGCONSUMIDOSSOLDDURO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.KGCONSUMIDOSSOLDBLANDO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.KGCONSUMIDOSSOLDBLANDO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.VARILLASOLDADURADURO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.VARILLASOLDADURADURO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.VARILLASOLDADURABLANDO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.VARILLASOLDADURABLANDO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.INTENSIDADSOLDADURADURO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.INTENSIDADSOLDADURADURO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.INTENSIDADSOLDADURABLANDO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.INTENSIDADSOLDADURABLANDO)
            </dd>
    
         
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.NOTAS)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.NOTAS)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.TIPOSOLDADURABLANDO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.TIPOSOLDADURABLANDO)
            </dd>
    
    
        </dl>
    </div>
    <p>
        @*@Html.ActionLink("Edit", "Edit", New With {.id = Model.PrimaryKey}) |*@
        @Html.ActionLink("Back to List", "Index")
    </p>
