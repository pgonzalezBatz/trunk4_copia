@imports web
<div class="alert alert-danger">
    <h4>@h.traducir("El enlace no esta activo.")<span class="glyphicon glyphicon-exclamation-sign"></span></h4>
</div>
@Code

    If (Model IsNot Nothing) Then
        @<h6>@Model</h6>
    End If

End code
<!--
<a href = "/extranetlogin/" >@h.Traducir("Volver al inicio")</a>
@Html.ActionLink(h.Traducir("Volver al inicio"), "Index", "access")
    -->
<a href='@Url.Action("Index", "access")' class="link">@h.traducir("Volver al inicio")</a>