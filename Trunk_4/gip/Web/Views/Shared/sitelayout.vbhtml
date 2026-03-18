@imports web
<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="WT.sv" content="@Server.MachineName" />
    <link href="//intranet2.batz.es/baliabideorokorrak/estilointranet.css" rel="stylesheet" type="text/css" />
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="@Url.Content("~/Content/Site.css")" rel="Stylesheet" type="text/css"  />
    <title>@h.traducir("Mantenimiento de proveedores") @RenderSection("title", False)</title>
    @RenderSection("header",False)
</head>
<body>
    <div >
        @RenderSection("menu1", False)
    </div>
        @RenderSection("beforebody", False)
    <div class="container">
        @RenderBody()
    </div>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
</body>
</html>
@RenderSection("scripts", False)
