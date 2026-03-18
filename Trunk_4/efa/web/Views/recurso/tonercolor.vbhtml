@imports web

@section header
    <title>@h.traducir("Colores de toner para impresora")</title>
End section

@section  beforebody
  <a href="@Url.Action("cogertonerimpresora")" style="float:left;">
    <img src="@Url.Content("~/Content/back.png")" alt="@h.traducir("Volver")" />
</a>
<a href="@Url.Action("logout")" style="float:right;" >
    <img src="@Url.Content("~/Content/exit.png")" alt="@h.traducir("Cerrar sessión")" />
</a>
<br style="clear:both;" />
End Section
<h3 class="touch">
    @h.traducir("Seleccione el color"):
</h3>
        @For Each c In Model
@<a class="touch touch3" href="@Url.Action("cogertonercolorconfirmar", new with {.idimpresora=c.idimpresora, .idcolor=c.idcolor})">
    
    @c.idcolor  @c.color  (@c.stock)
     @If (Request("idImpresora") Is Nothing) Then
                @<span style="font-size:0.6em;">
         @c.nombreImpresora
    </span>
            End If
</a>
        Next
<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript" charset="UTF-8"></script>
<script type="text/javascript">
    $(function () {
        $('.touch').click(function (e) {
            if (!confirm("@Html.Raw(h.traducir("¿Estas seguro que quieres coger el toner?"))")) {
                e.preventDefault();
            };
        });
    });
</script>