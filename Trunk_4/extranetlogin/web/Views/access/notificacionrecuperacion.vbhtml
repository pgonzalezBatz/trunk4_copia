@imports web
@code
    Layout = ""
End Code
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Batz S. Coop. - Extranet</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
</head>
<body>
    <div id="incluyeTodo">
        <div id="cabecera">
            
        </div>
        <div id="fondoContenido">
            Este email ha sido enviado por la Extranet de Xbat debido a que el usuario @ViewData("usuario") ha solicitado recordar la contraseña.
            Si no sabe por que recibe este email, ignorelo.
            En caso contrario, siga <a href="https://extranet.batz.es/extranetlogin/access/uniquereset?id=@ViewData("otp")">Este enlace</a>
            <br /><br />
            Si no puede acceder al enlace pruebe a pegar esta ruta en el navegador: <br />
            https://extranet.batz.es/extranetlogin/access/uniquereset?id=@ViewData("otp")
            <br /><br />
            Un saludo,
            <br />
            Batz S.Coop.
        </div>
    </div>
</body>
</html>