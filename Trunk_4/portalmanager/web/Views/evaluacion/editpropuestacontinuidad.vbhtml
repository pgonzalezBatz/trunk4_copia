@imports we
<h3 class="my-3">@H.Traducir("Añadir propuesta de continuidad")</h3>
@Html.ValidationSummary()
<form action="" method="post">
    <div class="form-group">
        <label>
            @H.Traducir("Aprueba la continuidad")
        </label>
        <br />
        <div class="form-check">
            @If Model Is Nothing Then
                @<input class="form-check-input" id="continua" type="radio" name="continua" value="True">
                @<label class="form-check-label">@H.Traducir("Si")</label>
            ElseIf Model.continua Then
                @<input class="form-check-input" id="continua" type="radio" name="continua" value="True" checked="checked">
                @<label class="form-check-label">@H.Traducir("Si")</label>
            Else
                @<input class="form-check-input" id="continua" type="radio" name="continua" value="True">
                @<label class="form-check-label">@H.Traducir("Si")</label>
            End If
        </div>
        <div class="form-check">
            @If Model Is Nothing Then
                @<input class="form-check-input" id="nocontinua" type="radio" name="continua" value="False">
                @<label class="form-check-label">@H.Traducir("No")</label>
            ElseIf Model.continua Then
                @<input class="form-check-input" id="nocontinua" type="radio" name="continua" value="False">
                @<label class="form-check-label">@H.Traducir("No")</label>
            Else
                @<input class="form-check-input" id="nocontinua" type="radio" name="continua" value="False" checked="checked">
                @<label class="form-check-label">@H.Traducir("No")</label>
            End If
        </div>
    </div>
    <div id="duracion" class="form-group">
        <label >
            @H.Traducir("Duracion")
        </label>
        @Html.TextBox("duracion", Nothing, New With {.class = "form-control"})
    </div>
    <div id="indice" class="form-group">
        <label >
            @H.Traducir("Indice")
        </label>
        @Html.TextBox("indice", Nothing, New With {.class = "form-control"})
    </div>

    <div id="motivo" class="form-group">
        <label >
            @H.Traducir("Motivo")
        </label>
        @Html.TextBox("motivo", Nothing, New With {.class = "form-control"})
    </div>

    <input class="btn btn-primary" type="submit" value="@H.Traducir("Guardar")" />



    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            function continua() {
                $("#duracion").show();
                $("#indice").show();
                $("#motivo").hide();

            };
            function nocontinua() {
                $("#duracion").hide();
                $("#indice").hide();
                $("#motivo").show();
            }
            if (document.getElementById('continua').checked) {
                continua();
            } else if (document.getElementById('nocontinua').checked) {
                nocontinua();
            }
            else {
                $("#duracion").hide();
                $("#indice").hide();
                $("#motivo").hide();
            };
            $("#continua").click(function () {
                continua();
            });
            $("#nocontinua").click(function () {
                nocontinua();
            });
        });
    </script>
</form>