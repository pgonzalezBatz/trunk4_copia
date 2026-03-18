@imports web
@Code
    ViewBag.title = h.traducir("Reasignar elementos de usuari@ de baja a otr@")
    
End Code
    <form action="" method="post">
        <ul>
            <li>
                <strong>Selecciona el usuario origen y posteriormente los recursos que quieres reasignar o liberar</strong>
            </li>
            <li>
                <strong>Selecciona el usuario destino para reasignar o dejar vacio para liberar los recursos</strong>
            </li>
        </ul>
        
        
        @Html.Partial("userSearchBaja")
        <input type="submit" value="@h.traducir("Continuar (simplemente liberar)")" class="btn" name="liberar" />
        <br /><br />
        @Html.Partial("userSearch")
        <br />
        <input type="submit" value="@h.traducir("Continuar (y reasignar)")" class="btn"/>
    </form>
<script type="text/javascript">
    document.getElementById("user").focus();
</script>
