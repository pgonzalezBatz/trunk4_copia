@imports web

@section header
    <title>@h.traducir("Reasignar elemento")</title>
<style type="text/css">
    ul {
        list-style: none;
        padding: 0;
    }

    li {
        margin: 0.5em;
        float: left;
        width: 31%;
    }

    ul li input[type=submit] {
        padding: 0.6em;
        width: 90%;
    } 
</style>
End section

<h3>@h.traducir("Selecciona la persona que a la que le quieras asignar el telefono ")@request("id") </h3>
<ul>
    @For Each u In Model
    @<li>
        <form action="" method="post">
            <input type="hidden" name="codpersona" value="@u.codpersona" />
            <input type="submit" value="@u.nombre.ToString.TrimEnd(" ") @u.Apellido1.ToString.TrimEnd(" ") @u.Apellido2" />
        </form>
    </li>
    Next
</ul>
<br style="clear:left;" />