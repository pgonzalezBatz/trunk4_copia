<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="WT.sv" content="@Server.MachineName" />
    <title>@h.traducir("Telefonia")</title>
    <link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    @RenderSection("header", False)
</head>
<body>
    <div class="container">
        @RenderBody()
     </div>
        <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
        <script src="//intranet2.batz.es/baliabideorokorrak/bootstrap/js/bootstrap.min.js"></script>
        @RenderSection("scripts", False)
</body>
</html>
