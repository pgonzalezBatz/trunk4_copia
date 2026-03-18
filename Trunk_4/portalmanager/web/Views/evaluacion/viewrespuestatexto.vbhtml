@imports web
@Code
    Dim extrainfo = ViewData("extrainfo")
End Code

@*@If Model.Count > 0 Then
        @<h4>Fecha de evaluación: @Model(0).fecha </h4>
    End If*@
<br />
<div style="margin-left:20px">
    <h6>Fecha de evaluación: @extrainfo.fecha1 </h6>
    <h6>Fecha de fin de contrato: @extrainfo.fecha2 </h6>
    <h6>Fecha de realización de la evaluación: @extrainfo.fecha3 </h6>
    <h6>Tipo: @extrainfo.tipoEv - @extrainfo.descEv</h6>
</div>
<br />

<h3 class="my-3">Respuestas No puntuadas</h3>

@For Each p In Model
    @<strong>
        @p.pregunta
    </strong>
    @<br />
    @p.respuesta
    @<br />
Next