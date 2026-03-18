@imports web

@Code
    Dim extrainfo = ViewData("extrainfo")
    Dim fechaProrroga = If(extrainfo.fechaFinContrato, extrainfo.fechaFinContrato.ToShortDateString(), "-")
End Code

<br />
<div style="margin-left:20px">
    <h6>Fecha de evaluación: @extrainfo.fechaVencimiento.ToShortDateString() </h6>
    <h6>Fecha de fin de contrato: @fechaProrroga</h6>
    <h6>Tipo: @extrainfo.nombreEvaluacion - @extrainfo.descEvaluacion</h6>
</div>
<br />

<h3 class="my-3">Evaluación</h3>
<div class="alert alert-info" role="alert">
    <p>@H.Traducir("Las puntuaciones que se muestran en letra pequeña solo sirven para orientar y marcar limites")</p>
    <hr />
    <p>
        @H.Traducir("Las respuestas en los recuadros pequeños tienen que ser numericas y pueden llevar decimales")
    </p>
</div>
@Html.ValidationSummary()
<form action="" method="post">
    <ul class="list-group mb-3">
        @code
            Dim i As Integer = 1
            Dim tituloOtrasPreguntas As Boolean = True
        End Code
        @For Each pg In Model
            If pg.key = TipoPregunta.puntuacion Then
                For Each p As Pregunta In pg
                    @<li class="list-group-item">
                        <strong>
                            @i .-
                            @H.Traducir(p.titulo)
                            @Html.Hidden(p.id.ToString + "|idpregunta", p.titulo)
                        </strong>
                        <p>
                            @H.Traducir(p.descripcion.Replace(Environment.NewLine, " "))
                        </p>
                        <small class="font-weight-light">
                            @For Each pr In p.respuestasPosibles
                                @pr.puntuacion @Html.Encode("-") @H.Traducir(pr.descripcion)
                                @<br />
                            Next
                        </small>
                        @code
                            Dim prLast = Nothing
                            If ViewData("listOfRespuesta") IsNot Nothing Then
                                prLast = CType(ViewData("listOfRespuesta"), List(Of Respuesta)).SingleOrDefault(Function(lr) lr.tituloPregunta = p.titulo)
                            End If
                        End Code
                        <div class="row">
                            <div class="col-1">
                                @If prLast IsNot Nothing Then
                                    @Html.TextBox(p.id.ToString + "|puntos", prLast.puntuacion, New With {.class = "form-control"})
                                Else
                                    @Html.TextBox(p.id.ToString + "|puntos", Nothing, New With {.class = "form-control"})
                                End If
                            </div>
                        </div>

                    </li>
                                    i = i + 1
                                Next
                            Else
                                For Each p As Pregunta In pg
                    @<li>
                        @If tituloOtrasPreguntas Then
                            @<h4 class="my-3">
                                @H.Traducir("Otras preguntas")
                                @Html.Hidden(p.id.ToString + "|idpregunta", p.titulo)
                            </h4>
                            tituloOtrasPreguntas = False
                        End If
                        <strong>
                            @i .- @H.Traducir(p.descripcion)
                        </strong>
                        <br />
                        @If ViewData(p.id.ToString + "|texto") IsNot Nothing Then
                            @Html.TextArea(p.id.ToString + "|texto", CType(ViewData(p.id.ToString + "|texto"), String), New With {.class = "form-control"})
                        Else
                            Dim prLast2 As Respuesta = Nothing
                            If ViewData("listOfRespuesta") IsNot Nothing Then
                                prLast2 = CType(ViewData("listOfRespuesta"), List(Of Respuesta)).SingleOrDefault(Function(lr) lr.descripcionPregunta = p.descripcion)
                            End If
                            If prLast2 IsNot Nothing Then
                                @Html.TextArea(p.id.ToString + "|texto", prLast2.texto, New With {.class = "form-control"})
                            Else
                                @Html.TextArea(p.id.ToString + "|texto", CType(ViewData(p.id.ToString + "|texto"), String), New With {.class = "form-control"})
                            End If


                        End If

                    </li>
                                        i = i + 1
                                    Next
                                End If

                            Next

    </ul>
    @Html.Hidden("idFormulario")
    @Html.Hidden("idSabColaborador")
    @Html.Hidden("ticksVencimiento")
    <h5>Tras completar la evaluación de tu colaborador/a, deberás de reunirte y trasladarle la evaluación</h5>
    <input Class="btn btn-primary" type="submit" value="@H.Traducir("Guardar")" />
</form>
<Script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.2.min.js" type="text/javascript"></Script>
<Script type="text/javascript">
    $('form').submit(function () {
        $(':submit', this).attr('disabled', 'disabled');
    });
</Script>