 @imports web

<h3 class="my3">Formulario</h3>
    <a class="btn btn-success" href="@Url.Action("addpregunta", H.ToRouteValues(Request.QueryString, Nothing))">@h.traducir("Añadir pregunta")</a>

    <ul class="list-group my-3">
        @If Model.count > 0 Then

            @For Each p As Pregunta In Model
                @<li class="list-group-item">
                    <strong>
                        @p.id .-
                        @p.titulo
                        <a  href="@Url.Action("editpregunta", New With {.idFormulario = Request("idFormulario"), .idPregunta = p.id})">@h.traducir("Editar pregunta")</a>
                        @If p.tipoPregunta = TipoPregunta.puntuacion Then
                            @Html.Encode("|")
                            @<a href="@Url.Action("addrespuestaposible", New With {.idFormulario = Request("idFormulario"), .idPregunta = p.id})">@h.traducir("Añadir respuesta posible")</a>
                        End If

                    </strong>
                    <p>
                        @p.descripcion
                    </p>
                    <ul class="list-group">
                        @If p.respuestasPosibles.Count > 0 Then
                            @For Each r In p.respuestasPosibles
                                @<li class="list-group-item">

                                    @r.descripcion
                                    <strong>@r.puntuacion @Html.Encode(" ") @h.traducir("puntos")</strong>
                                    <a href="@Url.Action("editrespuestaposible", New With {.idFormulario = Request("idFormulario"), .idPregunta = p.id, .idRespuesta = r.id})">@h.traducir("Editar respuesta posible")</a>

                                </li>
                            Next
                                              End If
                    </ul>

                </li>
            Next
        End If
    </ul>
