@imports web
@Code
    ViewBag.title = h.traducir("Añadir registro al inventario")
    
End Code

    <div id="notifications">
        
        @If IsNumeric(Request("idEtiqueta")) Then
            @h.traducir("Reasignación de etiqueta")@Html.Encode(" ")@Request("idEtiqueta")@<br />
        Else
            @h.Traducir("Introducir usuario al que se le van a asignar uno o mas recursos")
        End If
    </div>
    <form action="" method="post">
            @Html.Partial("userSearch")
            
        @If IsNumeric(Request("idEtiqueta")) Then
        @h.traducir("Asignar al departamento"):
        @<input type="checkbox" name="departamento" value="departamento" />    
        End If
        <br />
            <input type="submit" value="@h.Traducir("Continuar")" class="btn"/>
    </form>
<script type="text/javascript">
    document.getElementById("user").focus();
</script>
