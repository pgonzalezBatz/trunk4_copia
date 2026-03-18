    @imports web

<h3 class="my3">Formulario</h3>
@Html.ValidationSummary()
<form action="" method="post">
    <ul>
        @code
            Dim i As Integer = 1
            Dim tituloOtrasPreguntas As Boolean = True
        End Code
        @For Each pg In Model
            If pg.key = TipoPregunta.puntuacion Then
                For Each r As Respuesta In pg
            @<li>
                <h3>
                    @i .-
                    @h.traducir(r.tituloPregunta)
                </h3>
                <p>
                    @h.traducir(r.descripcionPregunta.Replace(Environment.NewLine, " "))
                </p>
                <strong>@h.traducir("Puntuacion"):</strong>
                @Html.TextBox(r.id.ToString + "|puntos", r.puntuacion, New With {.class = "numericbox"})
                @h.traducir(r.texto.Replace(Environment.NewLine, " "))
            </li>
                    i = i + 1
                Next
            Else
                For Each r As Respuesta In pg
            @<li>
                @If tituloOtrasPreguntas Then
                    @<h3>
                        @h.traducir("Otras preguntas")
                        @r.tituloPregunta
                    </h3>
                        tituloOtrasPreguntas = False
                    End If
                <strong>@i .-      @h.traducir(r.descripcionPregunta.Replace(Environment.NewLine, " "))        </strong>
        <br />
                 @Html.TextArea(r.id.ToString + "|texto", r.texto)
                <br />
            </li>
                    i = i + 1
                Next
            End If

        Next

    </ul>
    <input type="submit" value="@h.traducir("Guardar")" />
</form>