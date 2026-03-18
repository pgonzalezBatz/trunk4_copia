    @imports web
@Code
    ViewBag.title = h.traducir("Formulario")
End Code
@If ViewData("notificado")  is Nothing Then
@<a href="@Url.Action("editFormulario", h.ToRouteValues(Request.QueryString, Nothing))">@h.traducir("Modificar el formulario")</a>
@<a href="@Url.Action("deleteFormulario", h.ToRouteValues(Request.QueryString, Nothing))">@h.traducir("Eliminar el formulario")</a>    
End If

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
                             @h.traducir(r.descripcionPregunta.Replace(Environment.NewLine," "))
                         </p>
                             <strong>@h.traducir("Puntuacion"):</strong>
                            @r.puntuacion
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
                             <strong>
                                 @i .- @h.traducir(r.descripcionPregunta)
                             </strong>
                        <br />
                    @r.texto
        </li>
                        i = i + 1
                    Next
                End If
                
            Next
                
    </ul>