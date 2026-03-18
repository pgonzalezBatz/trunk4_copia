@imports web2
@Code
    ViewBag.title = "Histórico"
End Code

@If CBool(ViewData("havedata")) Then
    @<h4>@h.traducir("Talonarios recibidos este mes (recuerde que la fecha de corte es el " + ViewData("diaCorte") + ")")</h4>
    @For Each g In Model
            @<div class="dtipo">
                @Select Case g.key
                        Case "S"
                            @<i>Ayudas Batz</i>
                Case "C"
                            @<i>A cuenta del trabajador</i>
                
            End Select
        </div>
        @For Each o In g
                @o.fecha.toshortdatestring()
                @<strong>@h.traducir("Precio cheque:")</strong>
            @o.precio @Html.Encode("€")
            @h.traducir("Precio total talonario: ")
            @(o.precio * o.numeroCheques)
Next

    Next
    Else
    @<strong>@h.traducir("No se le ha distribuido ningun talonario este mes")</strong>
    @<br />
    @<strong> @h.traducir("Puede solicitar talonarios en conserjeria")</strong>
End If
<br />
<strong>
    <a href="@Url.Action("list")">@h.traducir("ver histórico completo")</a>
</strong>

