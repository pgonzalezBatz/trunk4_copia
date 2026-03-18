@imports web

<h4>
@h.traducir("El sistema ha detectado que su usuario pertenece a varias plantas. Seleccione la planta a la que desea conectarse:")
</h4>
@For Each m In Model
    @<form action = "" method="post">
        @Html.Hidden("idsab", m.idSab)
             <input type="submit" value="@h.traducir(m.nombre).ToUpper"  class="btn btn-default btn-lg btn-block btn-primary" />
            <br />
</form>
Next
