
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Grupo Material</title>
    @Styles.Render("~/Content/myCss")
    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/modernizr")
    @Scripts.Render("~/bundles/bootstrap")

</head>
<body>






    <nav class="navbar navbar-default navbar-fixed-top">
        <div class="container-fluid">
            <div class="navbar-header">
                <button class="navbar-toggle" type="button" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <img class="navbar-brand" src="https://intranet2.batz.es/baliabideorokorrak/logo_batz_menu.png" />
            </div>

            <div class="navbar-collapse collapse">

                <ul Class="nav navbar-nav">

                    <li>@Html.ActionLink("Inicio", "index2", "Home")</li>
                    <li>@Html.ActionLink("Lanzar job", "Job", "Home")</li>
                    <li>@Html.ActionLink("Administrar", "Administrar", "Home")</li>
                    <li>@Html.ActionLink("Criticidades", "Criticidades", "Home")</li>


                </ul> <ul class='nav navbar-nav navbar-right'>


                    <li> <a> <span Class="glyphicon glyphicon-user"></span>&nbsp;@ViewData("Usuario")</a></li>
                    <li> <a href=@ViewData("Servidor") id="aSalir"><span class="glyphicon glyphicon-off"></span>&nbsp;Salir</a></li>



                </ul>

            </div>
        </div>

    </nav>





    <div Class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - Batz Group</p>
        </footer>
    </div>


    @RenderSection("scripts", required:=False)
    <script type="text/javascript">
        Function() deleteItem() {
            var data = confirm("Are you sure? Every child/descendant in the hierarchy will be removed...");
            If(data == False) {
                Return False;
            } else {
                Return True;
            }
        }
        $(document).ready(function () {
            $("#sortableTable").tablesorter({
                headers: { '.actionColumn': { sorter: false } }
            });
        });
    </script>

</body>
</html>
