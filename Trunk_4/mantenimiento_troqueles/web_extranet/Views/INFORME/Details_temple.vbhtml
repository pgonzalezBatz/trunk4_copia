@ModelType web_extranet.INFORMES

<h3>@h.traducir("Visualización del detalle del informe")</h3>
<hr />
    <div>
        <h4>INFORMES</h4>
        <dl >
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
                @Html.DisplayNameFor(Function(model) model.TRATAMIENTO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.TRATAMIENTO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.ANTESTRATAM)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.ANTESTRATAM)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.DESPUESTRATAM)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.DESPUESTRATAM)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.CALZO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.CALZO)
            </dd>
    
           
    
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.DUREZAREALTEMPLEMAX)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.DUREZAREALTEMPLEMAX)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.DUREZAREALTEMPLEMIN)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.DUREZAREALTEMPLEMIN)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.NUMEROMEDIDASTEMPLE)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.NUMEROMEDIDASTEMPLE)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.TEMPERATURATEMPLE)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.TEMPERATURATEMPLE)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.OPTICAPASO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.OPTICAPASO)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.F)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.F)
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
    
         
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.METROS)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.METROS)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.DUREZAREQUERIDATEMPLE)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.DUREZAREQUERIDATEMPLE)
            </dd>
        </dl>
    </div>
    <p>
        @*@Html.ActionLink("Edit", "Edit", New With {.id = Model.PrimaryKey}) |*@
        @Html.ActionLink("Back to List", "Index")
    </p>