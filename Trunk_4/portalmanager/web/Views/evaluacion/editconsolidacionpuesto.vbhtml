@imports web
@Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Añadir consolidación de cambio de puesto")</legend>
        <label>
            @h.traducir("Aprueba la consolidación")
        </label>
        <br />
        @If Model Is Nothing Then
            @<input id="continua" type="radio" name="continua" value="True" >@h.traducir("Si")    
            @<input id="nocontinua" type="radio" name="continua" value="False">@h.traducir("No")
        ElseIf Model.continua Then
            @<input id="continua" type="radio" name="continua" value="True" checked="checked">@h.traducir("Si")    
            @<input id="nocontinua" type="radio" name="continua" value="False">@h.traducir("No")
        Else
            @<input id="continua" type="radio" name="continua" value="True" >@h.traducir("Si")    
            @<input id="nocontinua" type="radio" name="continua" value="False" checked="checked">@h.traducir("No")
        End If
        <br />
        <label id="argumento">
            @h.traducir("Argumento")                            
            <br />
            @Html.TextBox("argumento")
        </label>
        <br />
        <label id="indice">
            @h.traducir("Indice")
            <br />
            @Html.TextBox("indice")
        </label>
        <br />
        <input type="submit" value="@h.traducir("Guardar")" />
    </fieldset>
</form>