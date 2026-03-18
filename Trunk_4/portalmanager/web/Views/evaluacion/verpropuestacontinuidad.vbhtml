@imports web


<strong>@h.traducir("Aprobada la continuidad")</strong>
<br />
@If Model.continua Then
    @h.traducir("Si")
    @<br/>
    @<strong>
    @h.traducir("Duración")
</strong>
    @<br/>
    @Html.Display("duracion")
    @<br/>
    @<strong>
    @h.traducir("Indice")
</strong>
    @<br/>
    @Model.indice
Else
    @h.traducir("No")
    @<br/>
    @<strong>
    @h.traducir("Motivo")
</strong>
    @<br/>
    @Html.Display("motivo")
End If